using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using ImmersiveVRTools.Runtime.Common.PropertyDrawer;
using ImmersiveVRTools.Runtime.Common.ScriptableObject;
using VRControllersOnboarding.Runtime.Integrations;
using CommonUsages = UnityEngine.XR.CommonUsages;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using InputDevice = UnityEngine.InputSystem.InputDevice;
#endif

namespace VRControllersOnboarding.Runtime.CompletionRules
{
    [CreateAssetMenu(fileName = "JoystickMoved", menuName = AssetPath + "Completion Rule/JoystickMoved", order = MenuOrder.CompletionRules)]
    public class JoystickMovedCompletionRule: OnboardingActionCompletionRule
    {
        //TODO: add information that value has to be outside of those to be considered on
        [SerializeField] [MinMaxSlider(-1, 1)] private Vector2 MoveThresholdLeftRight = new Vector2(-0.3f, 0.3f);
        [SerializeField] [MinMaxSlider(-1, 1)] private Vector2 MoveThresholdUpDown = new Vector2(-0.3f, 0.3f);

        public override bool IsRuleSatisfied(ContextObjectResolveResult contextObjectResolveResult, ScriptableObjectStateRoot rootState)
        {
            //SteamVR handling - takes priority if defined
#if VRControllersOnboarding_Integrations_SteamVR
            //TODO: make sure notes mention the below
            //Joystick will not use Vector2 as with that renderComponentName is not present and linked button can't be resolved automatically, instead bool action will be used, eg snap-left, snap-right
            var inputType = SteamVRButtonGameObjectResolver.ControllerHandednessToSteamVrInputSourceMap[contextObjectResolveResult.ControllerButtonMappingSet.Handednes];
            return contextObjectResolveResult.ControllerButtonMappingSet.SteamVrBoolAction.GetState(inputType);
#else

            //New input system handling - if enabled
#if ENABLE_INPUT_SYSTEM
            const bool IsNewInputSystemEnabled = true;
#else
        const bool IsNewInputSystemEnabled = false;
#endif

            if (IsNewInputSystemEnabled && !string.IsNullOrEmpty(contextObjectResolveResult.ControllerButtonMappingSet.NewInputSystemActionFullPath))
            {
#if ENABLE_INPUT_SYSTEM
                var inputActionReference = contextObjectResolveResult.ControllerButtonMappingSet.NewInputSystemAction;
                if (inputActionReference.action.phase == InputActionPhase.Disabled)
                {
                    inputActionReference.action.Enable(); //TODO: does that need disabling?
                }

                var joystickValue = inputActionReference.action.ReadValue<Vector2>();
                return IsJoystickValueOutsideOfThreshold(joystickValue);
#endif
            }
            //old input system handling - last chance to handle
            else
            {
                var inputDevice = UnityXRControllerMapping.GetCachedInputDevice(contextObjectResolveResult, rootState);
                if (contextObjectResolveResult.ResolvedUnityXRControllerMapping //WARN: mapping in that case is disregarded, it's always primary2DAxis
                    && inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out var joystickValue)
                )
                {
                    return IsJoystickValueOutsideOfThreshold(joystickValue);;
                }
                else
                {
                    //no system can handle exception
                    throw new Exception($"{nameof(ControllerButtonMappingSet.NewInputSystemActionFullPath)} action not set on {nameof(ControllerButtonMappingSet)} - no system can handle, please make sure to set 'ActiveInputHandling' in Player Settings appropriately.");
                }
            }

            return false;
#endif
        }



        private bool IsJoystickValueOutsideOfThreshold(Vector2 joystickValue)
        {
            return (joystickValue.x <= MoveThresholdLeftRight.x || joystickValue.x >= MoveThresholdLeftRight.y)
                   && (joystickValue.y <= MoveThresholdUpDown.x || joystickValue.y > MoveThresholdUpDown.y);
        }
    }
}
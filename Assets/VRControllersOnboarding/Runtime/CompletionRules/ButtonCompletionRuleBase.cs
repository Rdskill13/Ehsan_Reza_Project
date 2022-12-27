using System;
using System.Collections.Generic;
using UnityEngine.XR;
using ImmersiveVRTools.Runtime.Common.ScriptableObject;
using UnityEngine;
using VRControllersOnboarding.Runtime.Integrations;

namespace VRControllersOnboarding.Runtime.CompletionRules
{
    public abstract class ButtonCompletionRuleBase: OnboardingActionCompletionRule
    {
        private static readonly float PressThreshold = 0.1f;
        
        protected bool IsButtonPressed(ContextObjectResolveResult contextObjectResolveResult, ScriptableObjectStateRoot rootState)
        {
            //SteamVR handling - takes priority if defined
#if VRControllersOnboarding_Integrations_SteamVR
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
                if (inputActionReference.action.phase == UnityEngine.InputSystem.InputActionPhase.Disabled)
                {
                    inputActionReference.action.Enable(); //TODO: does that need disabling?
                }

                if (inputActionReference.action.activeControl is UnityEngine.InputSystem.Controls.Vector2Control)
                {
                    throw new Exception($"Unable to check if button is pressed due to invalid setup, not a button. Did you intend to use JoystickMoved instead?");
                }
                
                return inputActionReference.action.ReadValue<float>() > PressThreshold;
#endif
            }
            //old input system handling - last chance to handle
            else
            {
                if (UnityXRControllerMapping.IsNullInstance(contextObjectResolveResult.ResolvedUnityXRControllerMapping))
                {
                    Debug.LogWarning($"{GetType().Name}: ResolvedUnityXRControllerMapping is null instance, resolving rule with true so user can continue. " +
                                     $"If that's not intended you should make sure mappings are set correctly.");
                    return true;
                }
                
                var inputDevice = UnityXRControllerMapping.GetCachedInputDevice(contextObjectResolveResult, rootState);
                if (contextObjectResolveResult.ResolvedUnityXRControllerMapping
                    && inputDevice.IsPressed(contextObjectResolveResult.ResolvedUnityXRControllerMapping.LegacyInputSystemButton, out var isPressed, PressThreshold)
                )
                {
                    //.IsPressed is bit misleading, it works in a TryIsPressed way, where return value indicates if that system is enabled and out value indicates if it's pressed or not
                    return isPressed;
                }
                else
                {
                    //no system can handle exception
                    throw new Exception(
                        $"Mapping: {contextObjectResolveResult.ResolvedUnityXRControllerMapping.name} " +
                        $"with UnityXRBinding: '{contextObjectResolveResult.ResolvedUnityXRControllerMapping.LegacyInputSystemButton}' can't be handled." +
                        $" {nameof(ControllerButtonMappingSet.NewInputSystemActionFullPath)} not set on {nameof(ControllerButtonMappingSet)} - no system can handle, please make sure to set 'ActiveInputHandling' in Player Settings appropriately.");
                }
            }

            return false;
#endif
        }
    }
}
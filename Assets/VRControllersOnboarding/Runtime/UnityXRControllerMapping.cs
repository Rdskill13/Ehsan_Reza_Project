using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.XR;
using ImmersiveVRTools.Runtime.Common.Extensions;
using ImmersiveVRTools.Runtime.Common.PropertyDrawer;
using ImmersiveVRTools.Runtime.Common.ScriptableObject;

namespace VRControllersOnboarding.Runtime
{
    [Serializable]
    public class UnityXRControllerMapping: OnboardingEntityScriptableBase, IOrderableNestedScriptableObject
    {
        private static UnityXRControllerMapping _NullLeftHandMappings;
        public static UnityXRControllerMapping NullLeftHandMappings => _NullLeftHandMappings ?? (_NullLeftHandMappings = CreateNullMapping(ControllerHandedness.Left));
        
        private static UnityXRControllerMapping _NullRightHandMappings;
        public static UnityXRControllerMapping NullRightHandMappings => _NullRightHandMappings ?? (_NullRightHandMappings = CreateNullMapping(ControllerHandedness.Right));

        private static UnityXRControllerMapping CreateNullMapping(ControllerHandedness controllerHandedness)
        {
            var scriptableObject = ScriptableObject.CreateInstance<UnityXRControllerMapping>();
            scriptableObject.ControllerHandedness = controllerHandedness;
            scriptableObject.LegacyInputSystemButton = ExtractedInputHelpers.Button.None;
            scriptableObject.UnityXRControllerMappingsRoot = null;

            return scriptableObject;
        }

        public static bool IsNullInstance(UnityXRControllerMapping mapping)
        {
            return mapping == NullLeftHandMappings || mapping == NullRightHandMappings;
        }

        public bool ShowUnityXRControllerMappingsRootInEditor => false;
        
        public static readonly Dictionary<ControllerHandedness, XRNode> ControllerHandednessToUnityLegacyXrInputXRNodeMap = new Dictionary<ControllerHandedness, XRNode>()
        {
            [ControllerHandedness.Left] = XRNode.LeftHand,
            [ControllerHandedness.Right] = XRNode.RightHand,
        }.AsAllElementsRequiredMap();
        
        [SerializeField] public ExtractedInputHelpers.Button LegacyInputSystemButton;
        [SerializeField] public ControllerHandedness ControllerHandedness; //this has to be on that object, eg PrimaryButton will be on Oculus either X/A based on what controller it is
        [SerializeField] [ShowIf(nameof(ShowUnityXRControllerMappingsRootInEditor))] public ControllerMappings UnityXRControllerMappingsRoot;

        public int Order { get; set; }
        
        public static UnityXRControllerMapping ResolvedUnityXRControllerMapping(ControllerButtonMappingSet forMappingSet)
        {
            var device = InputDevices.GetDeviceAtXRNode(UnityXRControllerMapping.ControllerHandednessToUnityLegacyXrInputXRNodeMap[forMappingSet.Handednes]);

            if (device == null || device.name == null)
            {
                throw new Exception($"Unable to find device for hand '{forMappingSet.Handednes}' - make sure controllers are present");
            }
            
            var matchingMapping = forMappingSet.UnityXRControllerButtonMappingEntries
                .FirstOrDefault(m => Regex.IsMatch(device.name, m.UnityXRControllerMappingsRoot.DeviceNameMatchingRegex));

            if (!matchingMapping)
            {
                Debug.LogWarning($"Unable to find mappings for device named '{device.name}' - " +
                                    $"make sure {nameof(ControllerMappings.DeviceNameMatchingRegex)} on {nameof(ControllerMappings)} is set correctly for devices. " +
                                    $"It's also possible that for UnityXR there are no mappings for that button, if that's the case old input (UnityXR) system can not be used.");
                
                return forMappingSet.Handednes == ControllerHandedness.Left ? NullLeftHandMappings : NullRightHandMappings;
            }
            
            return matchingMapping;
        }
        
        public static UnityEngine.XR.InputDevice GetCachedInputDevice(ContextObjectResolveResult contextObjectResolveResultBase,
            ScriptableObjectStateRoot rootState)
        {
            var cachedInputDevicesState = rootState.Get<CachedInputDevicesState>(rootState);
            var controllerHandedness = contextObjectResolveResultBase.ResolvedUnityXRControllerMapping.ControllerHandedness;
            if (!cachedInputDevicesState.ControllerHanenessToInputDeviceMap.ContainsKey(controllerHandedness))
            {
                cachedInputDevicesState.ControllerHanenessToInputDeviceMap[controllerHandedness] =
                    InputDevices.GetDeviceAtXRNode(
                        UnityXRControllerMapping.ControllerHandednessToUnityLegacyXrInputXRNodeMap[controllerHandedness]);
            }

            var inputDevice = cachedInputDevicesState.ControllerHanenessToInputDeviceMap[controllerHandedness];
            return inputDevice;
        }
    }
}
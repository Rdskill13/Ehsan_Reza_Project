#if VRControllersOnboarding_Integrations_OVR

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ImmersiveVRTools.Runtime.Common.Extensions;
using OVRTouchSample;
using UnityEngine;



namespace VRControllersOnboarding.Runtime.Integrations
{
    public class OVRButtonGameObjectResolver: IButtonGameObjectResolver
    {
        private Dictionary<ControllerHandedness, OVRInput.Controller> HandenessToOvrControllerMap = new Dictionary<ControllerHandedness, OVRInput.Controller>()
        {
            [ControllerHandedness.Left] = OVRInput.Controller.LTouch,
            [ControllerHandedness.Right] = OVRInput.Controller.RTouch
        }.AsAllElementsRequiredMap();

        private static readonly FieldInfo OvrTouchControllerControllerHandenessFieldInfo = typeof(TouchController).GetField("m_controller", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        //TODO: make sure docs are clear you have to use custom hands / TouchController script with OVR integration due OVRHands helper using skinned mesh renderer that can not be highlighted separately.
        //TODO; another option would be to use custom button resolver and adjust prefabs
        public ContextObjectResolveResult Resolve(ControllerButtonMappingSet forMappingSet)
        {
            if (string.IsNullOrEmpty(forMappingSet.OVRButtonName))
            {
                throw new Exception($"{nameof(forMappingSet.OVRButtonName)} on {nameof(ControllerButtonMappingSet)} - {forMappingSet.name} is not set, please add if you intend to use OVR integration");
            }
            
            var touchControllerComponent = GameObject.FindObjectsOfType<TouchController>().ToList();
            if (!touchControllerComponent.Any())
            {
                throw new Exception($"There are no {nameof(TouchController)} components in the scene, did you intend to use Oculus Integration with the tool? If add required components to the scene.");
            }
            
            var touchControllerComponentForCorrectHand = touchControllerComponent.FirstOrDefault(c => ((OVRInput.Controller) OvrTouchControllerControllerHandenessFieldInfo.GetValue(c)) == HandenessToOvrControllerMap[forMappingSet.Handednes]);
            if (!touchControllerComponentForCorrectHand)
            {
                throw new Exception($"There is no{nameof(TouchController)} components for {HandenessToOvrControllerMap[forMappingSet.Handednes]} handeness, please make sure it's configured.");
            }

            var matchingButton = TransformExtensions.FindChildRecursive(touchControllerComponentForCorrectHand.transform, forMappingSet.OVRButtonName);
            if (!matchingButton)
            {
                throw new ContextObjectNotFoundException($"Button named {forMappingSet.OVRButtonName}, on mapping: '{forMappingSet.name}' not found, please make sure it's present in the scene or check if corresponding {nameof(ControllerButtonMappingSet)} configuration is correct.");
            }

            var skinnedMeshRenderer = matchingButton.GetComponent<SkinnedMeshRenderer>();
            if (!skinnedMeshRenderer)
            {
                throw new Exception($"{nameof(SkinnedMeshRenderer)} not present on matching button - {matchingButton.name}, please make sure your using {nameof(TouchController)} setup.");
            }
            
            return new ContextObjectResolveResult(
                skinnedMeshRenderer.rootBone.gameObject,
                skinnedMeshRenderer,
                forMappingSet,
                UnityXRControllerMapping.ResolvedUnityXRControllerMapping(forMappingSet)
            );
        }
    }
}

#endif
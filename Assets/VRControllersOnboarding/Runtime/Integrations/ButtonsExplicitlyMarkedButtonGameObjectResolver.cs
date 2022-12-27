using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.XR;

namespace VRControllersOnboarding.Runtime.Integrations
{
    public class ButtonsExplicitlyMarkedButtonGameObjectResolver : IButtonGameObjectResolver
    {
        public ContextObjectResolveResult Resolve(ControllerButtonMappingSet forMappingSet)
        {
            var matchingExplicitlyMarkedControllerButtons = GameObject.FindObjectsOfType<ExplicitlyMarkedControllerButton>()
                .Where(b => b.ControllerButtonMappingSet == forMappingSet)
                .ToList();

            if (matchingExplicitlyMarkedControllerButtons.Count == 1)
            {
                return new ContextObjectResolveResult(
                    matchingExplicitlyMarkedControllerButtons[0].gameObject,
                    matchingExplicitlyMarkedControllerButtons[0].MeshRenderer,
                    forMappingSet,
                    UnityXRControllerMapping.ResolvedUnityXRControllerMapping(forMappingSet)
                );
            }
            
            if (!matchingExplicitlyMarkedControllerButtons.Any())
            {
                throw new ContextObjectNotFoundException($"There are no matching object with {nameof(ExplicitlyMarkedControllerButton)} script. Make sure those are set correctly on object " +
                                    $"and are referencing correct {nameof(ControllerButtonMappingSet)} - {forMappingSet.name}");
            }

            if (matchingExplicitlyMarkedControllerButtons.Count > 1)
            {
                foreach (var matchingExplicitlyMarkedControllerButton in matchingExplicitlyMarkedControllerButtons)
                {
                    Debug.LogWarning($"Multiple matching {nameof(ExplicitlyMarkedControllerButton)} referencing correct {nameof(ControllerButtonMappingSet)} - {forMappingSet.name}, click to view.", matchingExplicitlyMarkedControllerButton);
                }
                throw new Exception($"There are multiple({matchingExplicitlyMarkedControllerButtons.Count}) object with {nameof(ExplicitlyMarkedControllerButton)} referencing correct {nameof(ControllerButtonMappingSet)} - {forMappingSet.name}. There has to be exactly 1");
            }
            
            throw new ContextObjectNotFoundException($"Unable to resolve matching {nameof(ExplicitlyMarkedControllerButton)} referencing correct {nameof(ControllerButtonMappingSet)} - {forMappingSet.name}, make sure setup is correct");
        }
    }
}
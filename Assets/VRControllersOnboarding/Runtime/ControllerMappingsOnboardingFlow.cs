using System.Collections.Generic;
using UnityEngine;
using VRControllersOnboarding.Runtime;

public class ControllerMappingsOnboardingFlow: OnboardingEntityScriptableBase
{
    [SerializeField] public string Name;
    [SerializeField] public ControllerMappings ControllerMappings; //TODO: just for validation to make sure button setup comes from same mappings
}
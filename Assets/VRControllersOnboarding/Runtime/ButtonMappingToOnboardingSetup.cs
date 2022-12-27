using UnityEngine;
using VRControllersOnboarding.Runtime;

public class ButtonMappingToOnboardingSetup: MappingToOnboardingSetupBase
{
    [SerializeField] public ControllerButtonMappingSet ControllerButtonMappingSet;
    
    public override ContextObjectResolveResult ResolveContextObject(OnboardingFlowState onboardingFlowState)
    {
        return onboardingFlowState.ButtonGameObjectResolver.Resolve(ControllerButtonMappingSet);
    }
}
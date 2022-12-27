using System.Collections.Generic;
using UnityEngine;
using ImmersiveVRTools.Runtime.Common.PropertyDrawer;
using ImmersiveVRTools.Runtime.Common.ScriptableObject;

public abstract class MappingToOnboardingSetupBase: OnboardingEntityScriptableBase, IOrderableNestedScriptableObject
{
    [SerializeField] [InlineManagement] public List<OnboardingGuidance> Guidances;
    [SerializeField] [InlineManagement] public List<OnboardingActionCompletionRule> Rules;
    
    public int Order { get; set; }

    public abstract ContextObjectResolveResult ResolveContextObject(OnboardingFlowState onboardingFlowState);
}
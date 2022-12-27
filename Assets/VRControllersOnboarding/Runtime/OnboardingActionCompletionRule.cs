using ImmersiveVRTools.Runtime.Common.ScriptableObject;

public abstract class OnboardingActionCompletionRule : OnboardingEntityScriptableBase
{
    public abstract bool IsRuleSatisfied(ContextObjectResolveResult contextObjectResolveResult, ScriptableObjectStateRoot rootState);
}

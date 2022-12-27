using UnityEngine;
using ImmersiveVRTools.Runtime.Common.ScriptableObject;

namespace VRControllersOnboarding.Runtime.CompletionRules
{
    [CreateAssetMenu(fileName = "TimePassed", menuName = AssetPath + "Completion Rule/TimePassed", order = MenuOrder.CompletionRules)]
    public class TimePassedCompletionRule: OnboardingActionCompletionRule
    {
        [SerializeField] private float SecondsPassedToConsiderSatisfied = 1f;

        public override bool IsRuleSatisfied(ContextObjectResolveResult contextObjectResolveResult, ScriptableObjectStateRoot rootState)
        {
            var state = rootState.Get<TimePassedCompletionRuleState>(contextObjectResolveResult.GameObject);
            if (state.StartedAt == 0)
            {
                state.StartedAt = Time.time;
            }

            if (Time.time - state.StartedAt > SecondsPassedToConsiderSatisfied)
            {
                state.StartedAt = 0; //restart, in case that same rule will be re-used
                return true;
            }

            return false;
        }
        
        private class TimePassedCompletionRuleState: ScriptableObjectState
        {
            public float StartedAt { get; set; }
        }
    }
}
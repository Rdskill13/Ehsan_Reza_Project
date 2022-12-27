using System.Collections.Generic;
using System.Linq;
using ImmersiveVRTools.Runtime.Common.ScriptableObject;
using UnityEngine;

namespace VRControllersOnboarding.Runtime.CompletionRules
{
    [CreateAssetMenu(fileName = "CombinedAnyOf", menuName = AssetPath + "Completion Rule/Combined Any Of (OR) ", order = MenuOrder.CompletionRules)]
    public class CombinedOrCompletionRule : OnboardingActionCompletionRule
    {
        [SerializeField] private List<OnboardingActionCompletionRule> AnyOfCompletionRules;
        
        public override bool IsRuleSatisfied(ContextObjectResolveResult contextObjectResolveResult, ScriptableObjectStateRoot rootState)
        {
            return AnyOfCompletionRules.Any(rule => rule.IsRuleSatisfied(contextObjectResolveResult, rootState));
        }
    }
}
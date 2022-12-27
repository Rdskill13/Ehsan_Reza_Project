using UnityEngine;
using ImmersiveVRTools.Runtime.Common.ScriptableObject;

namespace VRControllersOnboarding.Runtime.CompletionRules
{
    [CreateAssetMenu(fileName = "ButtonPressed", menuName = AssetPath + "Completion Rule/ButtonPressed", order = MenuOrder.CompletionRules)]
    public class ButtonPressedCompletionRule: ButtonCompletionRuleBase
    {
        private static readonly float PressThreshold = 0.1f;
        
        public override bool IsRuleSatisfied(ContextObjectResolveResult contextObjectResolveResult, ScriptableObjectStateRoot rootState)
        {
            return IsButtonPressed(contextObjectResolveResult, rootState);
        }
    }
}
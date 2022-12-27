using ImmersiveVRTools.Runtime.Common.PropertyDrawer;
using ImmersiveVRTools.Runtime.Common.ScriptableObject;
using UnityEngine;

namespace VRControllersOnboarding.Runtime.CompletionRules
{
    [CreateAssetMenu(fileName = "CustomSignal", menuName = AssetPath + "Completion Rule/Custom Signal", order = MenuOrder.CompletionRules)]
    public class CustomSignalCompletionRule : OnboardingActionCompletionRule
    {
        [ReferenceOptions(ForceVariableOnly = true)] 
        [SerializeField] private CustomSignalReference CustomSignal;
        
        public override bool IsRuleSatisfied(ContextObjectResolveResult contextObjectResolveResult, ScriptableObjectStateRoot rootState)
        {
            var state = rootState.GetGlobal<CustomSignalsState>();
            if (state.Entries.Contains(CustomSignal.Variable))
            {
                state.Entries.Remove(CustomSignal.Variable);
                return true;
            }

            return false;
        }
    }
}

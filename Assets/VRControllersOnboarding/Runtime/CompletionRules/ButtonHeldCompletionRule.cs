using UnityEngine;
using ImmersiveVRTools.Runtime.Common.ScriptableObject;

namespace VRControllersOnboarding.Runtime.CompletionRules
{
    [CreateAssetMenu(fileName = "ButtonHeld", menuName = AssetPath + "Completion Rule/ButtonHeld", order = MenuOrder.CompletionRules)]
    public class ButtonHeldCompletionRule: ButtonCompletionRuleBase
    {
        [SerializeField] private float HoldForNSecondsToTrigger = 3f;
        
        public override bool IsRuleSatisfied(ContextObjectResolveResult contextObjectResolveResult, ScriptableObjectStateRoot rootState)
        {
            var  isButtonPressed = IsButtonPressed(contextObjectResolveResult, rootState);
            var state = rootState.Get<ButtonHeldCompletionRuleState>(contextObjectResolveResult.GameObject);

            if (state.IsBeingHeld && !isButtonPressed)
            {
                state.StopHolding();
            }
            else if (!state.IsBeingHeld && isButtonPressed)
            {
                state.ButtonPressStartedTime = Time.time;
            }
            else if(state.IsBeingHeld)
            {
                if (Time.time - state.ButtonPressStartedTime > HoldForNSecondsToTrigger)
                {
                    return true;
                }
            }

            return false;
        }

        private class ButtonHeldCompletionRuleState : ScriptableObjectState
        {
            public float ButtonPressStartedTime { get; set; }
            public bool IsBeingHeld => ButtonPressStartedTime != 0;

            public void StopHolding()
            {
                ButtonPressStartedTime = 0f;
            }
        }
    }
}
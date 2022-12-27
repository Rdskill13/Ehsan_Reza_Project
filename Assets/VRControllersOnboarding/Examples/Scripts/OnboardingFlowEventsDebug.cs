using UnityEngine;

public class OnboardingFlowEventsDebug : MonoBehaviour
{
    public void HandleStepStarted(OnboardingFlowEvents.StepStartedArgs args)
    {
        Debug.Log("HandleStepStarted");
    }
    
    public void HandleStepFinished(OnboardingFlowEvents.StepFinishedArgs args)
    {
        Debug.Log("HandleStepFinished");
    }
    
    public void HandleFlowCompleted(OnboardingFlowEvents.FlowCompletedArgs args)
    {
        Debug.Log("HandleFlowCompleted");
    }
    
    public void HandleFlowSkipped(OnboardingFlowEvents.FlowSkippedArgs args)
    {
        Debug.Log("HandleFlowSkipped");
    }
    
    public void HandleRuleSatisfied(OnboardingFlowEvents.RuleSatisfiedArgs args)
    {
        Debug.Log("HandleRuleSatisfied");
    }
    
    public void HandleUserAttentionGrabberStartedArgs(OnboardingFlowEvents.UserAttentionGrabberStartedArgs args)
    {
        Debug.Log("UserAttentionGrabberStartedArgs");
    }
}

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OnboardingFlowEventsDebug : MonoBehaviour
{

    [SerializeField] private GameObject LeftHand;
    [SerializeField] private GameObject RightHand;

    [SerializeField] private ActionBasedController left_XR_controller;
    [SerializeField] private ActionBasedController Right_XR_controller;


    public void HandleStepStarted(OnboardingFlowEvents.StepStartedArgs args)
    {
       // Debug.Log("HandleStepStarted");
    }
    
    
    public void HandleStepFinished(OnboardingFlowEvents.StepFinishedArgs args)
    {
       // Debug.Log("HandleStepFinished");
    }
    
    public void HandleFlowCompleted(OnboardingFlowEvents.FlowCompletedArgs args)
    {
        // Debug.Log("HandleFlowCompleted");
       // Destroy(RightHand.transform.gameObject);
        Right_XR_controller.modelPrefab = RightHand.transform;
        Right_XR_controller.modelParent = RightHand.transform;
        Right_XR_controller.model = RightHand.transform;

      //  Destroy(LeftHand.transform.gameObject);
        left_XR_controller.modelPrefab = LeftHand.transform;
        left_XR_controller.modelParent = LeftHand.transform;
        left_XR_controller.model = LeftHand.transform;

        //  ActionBasedController.Can_change_3dmodel = true;

        LeftHand.SetActive(true);
        RightHand.SetActive(true);

       
    }

    public void HandleFlowSkipped(OnboardingFlowEvents.FlowSkippedArgs args)
    {
        // Debug.Log("HandleFlowSkipped");
        
        Right_XR_controller.modelPrefab = RightHand.transform;
        Right_XR_controller.modelParent = RightHand.transform;
        Right_XR_controller.model = RightHand.transform;


        left_XR_controller.modelPrefab = LeftHand.transform;
        left_XR_controller.modelParent = LeftHand.transform;
        left_XR_controller.model = LeftHand.transform;

        //  ActionBasedController.Can_change_3dmodel = true;

        LeftHand.SetActive(true);
        RightHand.SetActive(true);
    }
    
    public void HandleRuleSatisfied(OnboardingFlowEvents.RuleSatisfiedArgs args)
    {
      //  Debug.Log("HandleRuleSatisfied");
    }
    
    public void HandleUserAttentionGrabberStartedArgs(OnboardingFlowEvents.UserAttentionGrabberStartedArgs args)
    {
      //  Debug.Log("UserAttentionGrabberStartedArgs");
    }
}

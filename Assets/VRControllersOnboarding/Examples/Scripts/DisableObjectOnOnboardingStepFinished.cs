using UnityEngine;

[AddComponentMenu(OnboardingEntityScriptableBase.AssetPath + "Examples/Disable Object On Onboarding Step Finished", MenuOrder.Examples)]
public class DisableObjectOnOnboardingStepFinished : MonoBehaviour
{
    [SerializeField] private OnboardingFlowEvents OnboardingFlowEvents;
    [SerializeField] private MappingToOnboardingSetupBase Step;
    
    void Start()
    {
        OnboardingFlowEvents.StepFinished.AddListener(stepFinishedArgs =>
        {
            if (stepFinishedArgs.CompletedStep == Step)
            {
                gameObject.SetActive(false);
            }
        });
    }
}
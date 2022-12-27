using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu(OnboardingEntityScriptableBase.AssetPath + "Examples/Enable Object On Onboarding Step Started", MenuOrder.Examples)]
public class EnableObjectOnOnboardingStepStarted : MonoBehaviour
{
    [SerializeField] private OnboardingFlowEvents OnboardingFlowEvents;
    [SerializeField] private MappingToOnboardingSetupBase Step;
    
    void Start()
    {
        OnboardingFlowEvents.StepStarted.AddListener(stepStartedArgs =>
        {
            if (stepStartedArgs.StepStarted == Step)
            {
                gameObject.SetActive(true);
            }
        });
    }
}
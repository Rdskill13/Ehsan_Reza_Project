using System;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu(OnboardingEntityScriptableBase.AssetPath + "Onboarding Flow Events", order: MenuOrder.Default)]
public class OnboardingFlowEvents: MonoBehaviour
{
    [Serializable]
    public class FlowStartedUnityEvent: UnityEvent<FlowStartedArgs> { }
    public FlowStartedUnityEvent FlowStarted = new FlowStartedUnityEvent();
    
    public class FlowStartedArgs
    {
        public FlowStartedArgs()
        {

        }
    }
    
    [Serializable]
    public class StepStartedUnityEvent: UnityEvent<StepStartedArgs> { }
    public StepStartedUnityEvent StepStarted = new StepStartedUnityEvent();
    
    public class StepStartedArgs
    {
        public MappingToOnboardingSetupBase PreviousStep { get; }
        public MappingToOnboardingSetupBase StepStarted { get; }

        public StepStartedArgs(MappingToOnboardingSetupBase previousStep, MappingToOnboardingSetupBase stepStarted)
        {
            PreviousStep = previousStep;
            StepStarted = stepStarted;
        }
    }
    
    [Serializable]
    public class StepFinishedUnityEvent: UnityEvent<StepFinishedArgs> { }
    public StepFinishedUnityEvent StepFinished = new StepFinishedUnityEvent();
    
    public class StepFinishedArgs
    {
        public MappingToOnboardingSetupBase CompletedStep { get; }
        
        public StepFinishedArgs(MappingToOnboardingSetupBase completedStep)
        {
            CompletedStep = completedStep;
        }
    }
    
    [Serializable]
    public class FlowCompletedUnityEvent: UnityEvent<FlowCompletedArgs> { }
    public FlowCompletedUnityEvent FlowCompleted = new FlowCompletedUnityEvent();
    
    public class FlowCompletedArgs
    {
        public FlowCompletedArgs(MappingToOnboardingSetupBase completedOnStep)
        {
            CompletedOnStep = completedOnStep;
        }

        public MappingToOnboardingSetupBase CompletedOnStep { get; }
    }
    
    [Serializable]
    public class FlowSkippedUnityEvent: UnityEvent<FlowSkippedArgs> { }
    public FlowSkippedUnityEvent FlowSkipped = new FlowSkippedUnityEvent();
    
    public class FlowSkippedArgs
    {
        public MappingToOnboardingSetupBase SkippedOnStep { get; }

        public FlowSkippedArgs(MappingToOnboardingSetupBase skippedOnStep)
        {
            SkippedOnStep = skippedOnStep;
        }
    }


    
    [Serializable]
    public class RuleSatisfiedUnityEvent: UnityEvent<RuleSatisfiedArgs> { }
    public RuleSatisfiedUnityEvent RuleSatisfied = new RuleSatisfiedUnityEvent();
    
    public class RuleSatisfiedArgs
    {
        public OnboardingActionCompletionRule SatisfiedRule { get; }
        public MappingToOnboardingSetupBase SatifiedRuleStepSetup { get; }

        public RuleSatisfiedArgs(OnboardingActionCompletionRule satisfiedRule, MappingToOnboardingSetupBase satifiedRuleStepSetup)
        {
            SatisfiedRule = satisfiedRule;
            SatifiedRuleStepSetup = satifiedRuleStepSetup;
        }
    }

    
    [Serializable]
    public class UserAttentionGrabberStartedUnityEvent: UnityEvent<UserAttentionGrabberStartedArgs> { }
    public UserAttentionGrabberStartedUnityEvent UserAttentionGrabberStarted = new UserAttentionGrabberStartedUnityEvent();

    public class UserAttentionGrabberStartedArgs
    {
        public MappingToOnboardingSetupBase StartedOnStep { get; }


        public UserAttentionGrabberStartedArgs(MappingToOnboardingSetupBase startedOnStep)
        {
            StartedOnStep = startedOnStep;
        }
    }
    
    [Serializable]
    public class FlowAbortedDueToExceptionUnityEvent: UnityEvent<FlowAbortedDueToExceptionArgs> { }
    public FlowAbortedDueToExceptionUnityEvent FlowAbortedDueToException = new FlowAbortedDueToExceptionUnityEvent();
    
    public class FlowAbortedDueToExceptionArgs
    {
        public Exception Exception { get; }

        public FlowAbortedDueToExceptionArgs(Exception exception)
        {
            Exception = exception;
        }
    }
}



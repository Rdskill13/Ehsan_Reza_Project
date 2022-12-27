using System;
using System.Collections.Generic;
using System.Linq;
using ImmersiveVRTools.Runtime.Common.PropertyDrawer;
using UnityEngine;
using VRControllersOnboarding.Runtime;
using ImmersiveVRTools.Runtime.Common.ScriptableObject;


[Serializable]
[CreateAssetMenu(fileName = "OnboardingFlow", menuName = AssetPath + "OnboardingFlow", order = MenuOrder.Default)]
public class OnboardingFlow: OnboardingEntityScriptableBase
{
    [SerializeField] public bool ManualExplicitlyMarkedButtonResolution; //TODO: need to add custom editor to show some help how that is setup
    [SerializeField] public float RemindUserAboutGuidanceIfNotCompletedAfterNSeconds = 30;
    [SerializeField] public List<MappingToOnboardingSetupBase> MappingToOnboardingSetupEntries;

    [Header("Skip Onboarding")]
    [SerializeField] [InlineManagement(IsCreatingNewDisabled = true)] private MappingToOnboardingSetupBase SkipOnboardingStep;
    [SerializeField] [InlineManagement] private OnboardingActionCompletionRule SkipTrigger;

    public List<MappingToOnboardingSetupBase> MappingToOnboardingSetupEntriesOrdered => MappingToOnboardingSetupEntries.OrderBy(s => s.Order).ToList();
    
    public void ProcessUpdate(ScriptableObjectStateRoot rootState, MonoBehaviour coroutineExecutor, OnboardingFlowEvents onboardingFlowEvents, out bool stopFurtherUpdateExecution)
    {
        try
        {
            stopFurtherUpdateExecution = false;
            var state = rootState.Get<OnboardingFlowState>(this);

            if (!state.IsFlowAlreadyStarted)
            {
                onboardingFlowEvents.FlowStarted?.Invoke(new OnboardingFlowEvents.FlowStartedArgs());
                state.IsFlowAlreadyStarted = true;
            }
            
            if (SkipOnboardingStep && SkipTrigger)
            {
                var skipOnboardingState = rootState.Get<SkipOnboardingState>(this);
                if (skipOnboardingState.ContextObjectResolveResult == null)
                {
                    skipOnboardingState.ContextObjectResolveResult = SkipOnboardingStep.ResolveContextObject(state);
                }

                if (SkipTrigger.IsRuleSatisfied(skipOnboardingState.ContextObjectResolveResult, rootState))
                {
                    var skippedOnStep = state.CurrentlyExecutingOnboardingSetup;
                    StopGuidanceForCurrentSetupStep(state, rootState, coroutineExecutor);
                    Debug.Log("Skip onboarding triggered");
                    stopFurtherUpdateExecution = true;
                    onboardingFlowEvents.FlowSkipped?.Invoke(new OnboardingFlowEvents.FlowSkippedArgs(skippedOnStep));
                    return;
                }
            }

            if (!state.CurrentlyExecutingOnboardingSetup)
            {
                ChangeCurrentlyExecutingSetup(MappingToOnboardingSetupEntriesOrdered.First(), rootState, onboardingFlowEvents, coroutineExecutor);
            }

            foreach (var guidance in state.CurrentlyExecutingOnboardingSetup.Guidances)
            {
                try
                {
                    var grabUserAttentionGuidanceState = rootState
                            .Get<OnboardingGuidanceRenderUserAttentionGrabberAfterTooMuchTimePassedState>(guidance);
                    var timePassed = Time.time - grabUserAttentionGuidanceState.LastRemindedAt;
                    if (timePassed > RemindUserAboutGuidanceIfNotCompletedAfterNSeconds)
                    {
                        grabUserAttentionGuidanceState.LastRemindedAt = Time.time;
                        var coroutineEnumerator = guidance.RenderUserAttentionGrabberAfterTooMuchTimePassedCoroutine(state.contextObjectResolutionResult, rootState);
                        var coroutine = coroutineExecutor.StartCoroutine(coroutineEnumerator);
                        grabUserAttentionGuidanceState.AttentionGrabberCoroutine = coroutine;

                        onboardingFlowEvents.UserAttentionGrabberStarted?.Invoke(
                            new OnboardingFlowEvents.UserAttentionGrabberStartedArgs(
                                state.CurrentlyExecutingOnboardingSetup));
                    }

                    guidance.ProcessUpdate(state.contextObjectResolutionResult, rootState);
                }
                catch (Exception e)
                {
                    Debug.Log($"Unable to process guidance: {guidance.name}, {e}");
                }
            }

            var areAllRulesSatisfied = true;
            foreach (var rule in state.CurrentlyExecutingOnboardingSetup.Rules)
            {
                var isRuleSatisfied = rule.IsRuleSatisfied(state.contextObjectResolutionResult, rootState);
                TriggerRuleSatisfiedEventIfFirstTimeSatisfied(onboardingFlowEvents, isRuleSatisfied, state, rule);
                if (!isRuleSatisfied)
                {
                    areAllRulesSatisfied = false;
                }
            }

            if (areAllRulesSatisfied)
            {
                var currentIndexStep = MappingToOnboardingSetupEntriesOrdered.IndexOf(state.CurrentlyExecutingOnboardingSetup);
                Debug.Log($"All rules for step: '{state.CurrentlyExecutingOnboardingSetup.name}' ({currentIndexStep + 1} of {MappingToOnboardingSetupEntriesOrdered.Count}) satisfied");

                var completedStep = state.CurrentlyExecutingOnboardingSetup;
                var nextStep = MappingToOnboardingSetupEntriesOrdered.Count > currentIndexStep + 1
                    ? MappingToOnboardingSetupEntriesOrdered[currentIndexStep + 1]
                    : null;
                if (nextStep)
                {
                    ChangeCurrentlyExecutingSetup(nextStep, rootState, onboardingFlowEvents, coroutineExecutor);
                }
                else
                {
                    StopGuidanceForCurrentSetupStep(state, rootState, coroutineExecutor);
                    state.CurrentlyExecutingOnboardingSetup = null;
                    onboardingFlowEvents.StepFinished?.Invoke(new OnboardingFlowEvents.StepFinishedArgs(completedStep));
                    onboardingFlowEvents.FlowCompleted?.Invoke(new OnboardingFlowEvents.FlowCompletedArgs(completedStep));
                    Debug.Log($"Onboarding workflow finished.");
                    stopFurtherUpdateExecution = true;
                }
            }
        }
        // catch (ContextObjectNotFoundException e) {} //Could potentially handle differently if needed?
        catch (Exception e)
        {
            Debug.LogError($"Onboarding workflow: further execution stopped due to: {e}");
            stopFurtherUpdateExecution = true;
            onboardingFlowEvents.FlowAbortedDueToException?.Invoke(new OnboardingFlowEvents.FlowAbortedDueToExceptionArgs(e));
        }
    }

    private static void TriggerRuleSatisfiedEventIfFirstTimeSatisfied(OnboardingFlowEvents onboardingFlowEvents,
        bool isRuleSatisfied, OnboardingFlowState state, OnboardingActionCompletionRule r)
    {
        if (isRuleSatisfied)
        {
            if (!state.RulesCompletedForSetupStep.ContainsKey(state.CurrentlyExecutingOnboardingSetup))
                state.RulesCompletedForSetupStep[state.CurrentlyExecutingOnboardingSetup] =
                    new List<OnboardingActionCompletionRule>();

            if (!state.RulesCompletedForSetupStep[state.CurrentlyExecutingOnboardingSetup].Contains(r))
            {
                state.RulesCompletedForSetupStep[state.CurrentlyExecutingOnboardingSetup].Add(r);
                onboardingFlowEvents.RuleSatisfied?.Invoke(
                    new OnboardingFlowEvents.RuleSatisfiedArgs(r, state.CurrentlyExecutingOnboardingSetup));
            }
        }
    }

    public void ChangeCurrentlyExecutingSetup(MappingToOnboardingSetupBase step, ScriptableObjectStateRoot rootState, OnboardingFlowEvents onboardingFlowEvents, MonoBehaviour coroutineExecutor)
    {
        if (!MappingToOnboardingSetupEntries.Contains(step))
        {
            throw new Exception($"Step: '{step.name}' is not part of OnboardingFlow");
        }
        
        var state = rootState.Get<OnboardingFlowState>(this);
        MappingToOnboardingSetupBase completedStep = null;
        if (state.CurrentlyExecutingOnboardingSetup)
        {
            completedStep = state.CurrentlyExecutingOnboardingSetup;
            StopGuidanceForCurrentSetupStep(state, rootState, coroutineExecutor);
            onboardingFlowEvents.StepFinished?.Invoke(new OnboardingFlowEvents.StepFinishedArgs(completedStep));
        }
        
        state.CurrentlyExecutingOnboardingSetup = step;
        onboardingFlowEvents.StepStarted?.Invoke(new OnboardingFlowEvents.StepStartedArgs(completedStep, state.CurrentlyExecutingOnboardingSetup));
        state.contextObjectResolutionResult = step.ResolveContextObject(state);
       
        foreach (var guidance in step.Guidances)
        {
            try
            {
                var grabUserAttentionGuidanceState = rootState.Get<OnboardingGuidanceRenderUserAttentionGrabberAfterTooMuchTimePassedState>(guidance);
                grabUserAttentionGuidanceState.LastRemindedAt = Time.time; //initial start counts as a reminder
                guidance.StartGuidance(state.contextObjectResolutionResult, rootState);
            }
            catch (Exception e)
            {
                Debug.Log($"Unable to start guidance: {guidance.name}, {e}");
            }
        }
        
        Debug.Log($"Changing CurrentlyExecutingOnboardingSetup: {step.name}");
    }

    private static void StopGuidanceForCurrentSetupStep(OnboardingFlowState state, ScriptableObjectStateRoot rootState, MonoBehaviour coroutineExecutor)
    {
        if(!state.CurrentlyExecutingOnboardingSetup)
            return;
        
        foreach (var guidance in state.CurrentlyExecutingOnboardingSetup.Guidances)
        {
            var grabUserAttentionGuidanceState = rootState
                .Get<OnboardingGuidanceRenderUserAttentionGrabberAfterTooMuchTimePassedState>(guidance);
            if (grabUserAttentionGuidanceState.AttentionGrabberCoroutine != null)
            {
                coroutineExecutor.StopCoroutine(grabUserAttentionGuidanceState.AttentionGrabberCoroutine);
            }
            
            guidance.StopGuidance(state.contextObjectResolutionResult, rootState);
        }
    }

    private class SkipOnboardingState : ScriptableObjectState
    {
        public ContextObjectResolveResult ContextObjectResolveResult { get; set; }
    }
}

public class CustomSignalsState : ScriptableObjectState
{
    public HashSet<CustomSignalVariable> Entries { get; set; } = new HashSet<CustomSignalVariable>();
}

public class OnboardingFlowState: ScriptableObjectState
{
    public MappingToOnboardingSetupBase CurrentlyExecutingOnboardingSetup { get; set; }
    public IButtonGameObjectResolver ButtonGameObjectResolver { get; set; }
    public ContextObjectResolveResult contextObjectResolutionResult { get; set; }
    public Dictionary<MappingToOnboardingSetupBase, List<OnboardingActionCompletionRule>> RulesCompletedForSetupStep { get; set; } 
        = new Dictionary<MappingToOnboardingSetupBase, List<OnboardingActionCompletionRule>>();
    
    public bool IsFlowAlreadyStarted { get; set; }
}

public class CachedInputDevicesState: ScriptableObjectState
{
    public Dictionary<ControllerHandedness, UnityEngine.XR.InputDevice> ControllerHanenessToInputDeviceMap { get; set; }

    public CachedInputDevicesState()
    {
        ControllerHanenessToInputDeviceMap = new Dictionary<ControllerHandedness, UnityEngine.XR.InputDevice>();
    }
}
using System;
using UnityEngine;
using VRControllersOnboarding.Runtime;
using ImmersiveVRTools.Runtime.Common.PropertyDrawer;
using ImmersiveVRTools.Runtime.Common.ScriptableObject;
using VRControllersOnboarding.Runtime.Integrations;

[RequireComponent(typeof(OnboardingFlowEvents))]
[AddComponentMenu(OnboardingEntityScriptableBase.AssetPath + "VR Controller Onboarding Manager")]
public class VRControllerOnboardingManager : MonoBehaviour
{
    [SerializeField] private OnboardingFlowEvents _onboardingFlowEvents;
    public OnboardingFlowEvents OnboardingFlowEvents => _onboardingFlowEvents ?? (_onboardingFlowEvents = GetComponent<OnboardingFlowEvents>());
    
    [Header("Debug")] [SerializeField] private bool IsDebug;
    [SerializeField] [ShowIf(nameof(IsDebug))] private OnboardingFlow DebugOnboardingFlowToRun;
    [SerializeField] [ShowIf(nameof(IsDebug))] private OnboardingFlow CurrentlyExecutingFlow;
    [SerializeField] [ShowIf(nameof(IsDebug))] private ScriptableObjectStateRoot OnboardingFlowRootState;

    public OnboardingFlowState OnboardingFlowState { get; private set; }
    
    public void StartOnboarding(OnboardingFlow flow)
    {
        CurrentlyExecutingFlow = flow;
        OnboardingFlowRootState = new ScriptableObjectStateRoot();
        var buttonGameObjectResolver = ButtonGameObjectResolverFactory.Generate(flow.ManualExplicitlyMarkedButtonResolution);
        OnboardingFlowState = OnboardingFlowRootState.Get<OnboardingFlowState>(CurrentlyExecutingFlow);
        OnboardingFlowState.ButtonGameObjectResolver = buttonGameObjectResolver;
    }

    public void ChangeCurrentlyExecutingStep(MappingToOnboardingSetupBase step)
    {
        if (!CurrentlyExecutingFlow)
        {
            Debug.LogWarning($"There's no onboarding flow executing, can't change current step");
            return;
        }
        
        CurrentlyExecutingFlow.ChangeCurrentlyExecutingSetup(step, OnboardingFlowRootState, OnboardingFlowEvents, this);
    }

    public void RegisterCustomSignal(CustomSignalVariable variable)
    {
        if (OnboardingFlowRootState != null)
        {
            OnboardingFlowRootState.GetGlobal<CustomSignalsState>().Entries.Add(variable);
        }
    }

    private void Reset()
    {
        _onboardingFlowEvents = OnboardingFlowEvents;
    }

    private void Update()
    {
        if (CurrentlyExecutingFlow)
        {
            bool stopFurtherUpdateExecution;
            CurrentlyExecutingFlow.ProcessUpdate(OnboardingFlowRootState, this, OnboardingFlowEvents, out stopFurtherUpdateExecution);
            if (stopFurtherUpdateExecution)
            {
                CurrentlyExecutingFlow = null;
            }
        }
    }
    
#if IMMERSIVE_VR_TOOLS_DEVELOP_AIDS
    [UnityEditor.MenuItem("DEBUG/Start Default Controller Onboarding #F1")]
    static void StartOnboardingDebug()
    {
        var onboardingManager = GameObject.FindObjectOfType<VRControllerOnboardingManager>();
        onboardingManager.StartOnboarding(onboardingManager.DebugOnboardingFlowToRun);
    }
    
    [SerializeField] [ShowIf(nameof(IsDebug))] public int MoveToWorkflowStepIndex;

    [UnityEditor.MenuItem("DEBUG/Move to worfklow step #F2")]
    static void MoveToWorkflowStep()
    {
        var onboardingManager = GameObject.FindObjectOfType<VRControllerOnboardingManager>();
        onboardingManager.ChangeCurrentlyExecutingStep(onboardingManager.CurrentlyExecutingFlow.MappingToOnboardingSetupEntries[onboardingManager.MoveToWorkflowStepIndex]);
    }
#endif
}

public interface IButtonGameObjectResolver
{
    ContextObjectResolveResult Resolve(ControllerButtonMappingSet forMappingSet);
}

public class ButtonGameObjectResolverFactory
{
    public static IButtonGameObjectResolver Generate(bool manualExplicitlyMarkedButtonResolution)
    {
#if VRControllersOnboarding_Integrations_SteamVR
        if (!manualExplicitlyMarkedButtonResolution)
        {
            return new SteamVRButtonGameObjectResolver();
        }
        else
        {
            WarnUserAboutManualButtonResolutionEnabled($"Steam VR");
        }
#endif
        
#if VRControllersOnboarding_Integrations_OVR
        if (!manualExplicitlyMarkedButtonResolution)
        {
            return new OVRButtonGameObjectResolver();
        }
        else
        {
            WarnUserAboutManualButtonResolutionEnabled($"OVR");
        }
            
#endif
        
        if (manualExplicitlyMarkedButtonResolution)
        {
            return new ButtonsExplicitlyMarkedButtonGameObjectResolver();
        }
        
        throw new Exception($"Invalid {nameof(OnboardingFlow)} setup, no button resolver, either select integration from settings or set {nameof(OnboardingFlow.ManualExplicitlyMarkedButtonResolution)} on {nameof(OnboardingFlow)}");
    }

    private static void WarnUserAboutManualButtonResolutionEnabled(string compiledPlatform)
    {
        Debug.LogWarning(
            $"{compiledPlatform} is compiled, but {nameof(OnboardingFlow)} is set with {nameof(OnboardingFlow.ManualExplicitlyMarkedButtonResolution)} - if you intend to use automatic resolution, untick the box on {nameof(OnboardingFlow)}");
    }
}

//WARN: Unfortunately due to serialization limits within Unity that class can not be more specialized / generic. Consuming classes will need to work out if data is available or not 
public class ContextObjectResolveResult
{
    public GameObject GameObject { get; }
    public Renderer MeshRenderer { get; }
    public ControllerButtonMappingSet ControllerButtonMappingSet { get; }
    public UnityXRControllerMapping ResolvedUnityXRControllerMapping { get; }

    public ContextObjectResolveResult(GameObject gameObject, Renderer meshRenderer, ControllerButtonMappingSet controllerButtonMapping, UnityXRControllerMapping resolvedUnityXRControllerMapping)
    {
        GameObject = gameObject;
        MeshRenderer = meshRenderer;
        ControllerButtonMappingSet = controllerButtonMapping;
        this.ResolvedUnityXRControllerMapping = resolvedUnityXRControllerMapping;
    }
}

public class ContextObjectNotFoundException : Exception
{
    public ContextObjectNotFoundException(string message) : base(message)
    {
    }
}
using System.Collections;
using UnityEngine;
using ImmersiveVRTools.Runtime.Common.ScriptableObject;

public abstract class OnboardingGuidance : OnboardingEntityScriptableBase
{
    public void StartGuidance(ContextObjectResolveResult contextObjectResolutionResult, ScriptableObjectStateRoot state)
    {
        StartGuidanceInternal(contextObjectResolutionResult, state);
    }

    public void StopGuidance(ContextObjectResolveResult contextObjectResolutionResult, ScriptableObjectStateRoot state)
    {
        
        StopGuidanceInternal(contextObjectResolutionResult, state);
    }

    public void ProcessUpdate(ContextObjectResolveResult contextObjectResolutionResult, ScriptableObjectStateRoot state)
    {
        ProcessUpdateInternal(contextObjectResolutionResult, state);
    }

    public abstract void StartGuidanceInternal(ContextObjectResolveResult contextObjectResolutionResult, ScriptableObjectStateRoot state);
    public abstract void ProcessUpdateInternal(ContextObjectResolveResult contextObjectResolutionResult, ScriptableObjectStateRoot state);
    public abstract void StopGuidanceInternal(ContextObjectResolveResult contextObjectResolutionResult, ScriptableObjectStateRoot state);
    
    public abstract IEnumerator RenderUserAttentionGrabberAfterTooMuchTimePassedCoroutine(ContextObjectResolveResult contextObjectResolutionResult, ScriptableObjectStateRoot state);
    
    protected bool IsMeshRendererAvailable(ContextObjectResolveResult contextObjectResolutionResultBase)
    {
        if (contextObjectResolutionResultBase == null || !contextObjectResolutionResultBase.MeshRenderer)
        {
            Debug.LogWarning($"Unable to start '{GetType().Name}' as there's no button game object found");
            return false;
        }

        return true;
    }
}

public class OnboardingGuidanceRenderUserAttentionGrabberAfterTooMuchTimePassedState: ScriptableObjectState
{
    public float LastRemindedAt { get; set; }
    public Coroutine AttentionGrabberCoroutine { get; set; }
}
using System.Collections;
using UnityEngine;
using ImmersiveVRTools.Runtime.Common.ScriptableObject;

[CreateAssetMenu(fileName = "HighlightGuidance", menuName = AssetPath + "Guidance/HighlightGuidance", order = MenuOrder.Guidance)]
public class HighlightGuidance : HighlightGuidanceBase
{
}

public abstract class HighlightGuidanceBase : OnboardingGuidance
{
    //TODO: make sure materials will be auto-adjusted to URP/Builtin pipeline
    [SerializeField] private Material HighlightMaterial;
    [SerializeField] private int RemindUserAboutActionToggleHighlightNTimes = 3;
    [SerializeField] private float RemindUserAboutActionToggleHighlightNTimesDelay = 0.5f;
    
    private Material _previousMaterial;
    

   
    public override void StartGuidanceInternal(ContextObjectResolveResult contextObjectResolutionResult, ScriptableObjectStateRoot state)
    {
        if (!IsMeshRendererAvailable(contextObjectResolutionResult, state))
        {
            return;
        }

        HighlightElement(contextObjectResolutionResult, state);
    }


    public override void ProcessUpdateInternal(ContextObjectResolveResult contextObjectResolutionResult, ScriptableObjectStateRoot state)
    {
    }

    public override void StopGuidanceInternal(ContextObjectResolveResult contextObjectResolutionResult, ScriptableObjectStateRoot state)
    {
        if (!IsMeshRendererAvailable(contextObjectResolutionResult, state))
        {
            return;
        }

        UnhighlightElement(contextObjectResolutionResult, state);
    }
    
    public override IEnumerator RenderUserAttentionGrabberAfterTooMuchTimePassedCoroutine(ContextObjectResolveResult contextObjectResolutionResult, ScriptableObjectStateRoot state)
    {
        if (!IsMeshRendererAvailable(contextObjectResolutionResult, state))
        {
            yield break;
        }

        for (var i = 0; i < RemindUserAboutActionToggleHighlightNTimes; i++)
        {
            UnhighlightElement(contextObjectResolutionResult, state);
            yield return new WaitForSeconds(RemindUserAboutActionToggleHighlightNTimesDelay);
            HighlightElement(contextObjectResolutionResult, state);
            yield return new WaitForSeconds(RemindUserAboutActionToggleHighlightNTimesDelay);
        }
    }
    
    private void HighlightElement(ContextObjectResolveResult contextObjectResolutionResult, ScriptableObjectStateRoot state)
    {
        var meshRenderer = ResolveMeshRenderer(contextObjectResolutionResult, state);
        _previousMaterial = meshRenderer.material;
        meshRenderer.material = HighlightMaterial;
    }

    private void UnhighlightElement(ContextObjectResolveResult contextObjectResolutionResult,  ScriptableObjectStateRoot state)
    {
        var meshRenderer = ResolveMeshRenderer(contextObjectResolutionResult, state);
        if (_previousMaterial)
        {
            meshRenderer.material = _previousMaterial;
        }
        _previousMaterial = null;
    }

    protected bool IsMeshRendererAvailable(ContextObjectResolveResult contextObjectResolutionResult,  ScriptableObjectStateRoot state)
    {
        if (!ResolveMeshRenderer(contextObjectResolutionResult, state))
        {
            Debug.LogWarning($"Unable to start '{GetType().Name}' as there's no mesh renderer found, make sure setup is correct and object has mesh renderer");
            return false;
        }

        return true;
    }
    
    private Renderer ResolveMeshRenderer(ContextObjectResolveResult contextObjectResolutionResult, ScriptableObjectStateRoot state)
    {
        return contextObjectResolutionResult.MeshRenderer;
    }
}
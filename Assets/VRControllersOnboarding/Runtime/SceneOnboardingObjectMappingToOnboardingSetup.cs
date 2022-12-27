using System.Linq;
using UnityEngine;
using ImmersiveVRTools.Runtime.Common.PropertyDrawer;

public class SceneOnboardingObjectMappingToOnboardingSetup: MappingToOnboardingSetupBase
{
    [SerializeField] 
    [ReferenceOptions(ForceVariableOnly = true)] 
    public SceneOnboardingObjectIdentifierReference SceneObjectIdentifier;
    
    public override ContextObjectResolveResult ResolveContextObject(OnboardingFlowState onboardingFlowState)
    {
        var sceneOnboardingObject = GameObject.FindObjectsOfType<SceneOnboardingObject>().FirstOrDefault(s => s.Identifier.Variable == SceneObjectIdentifier.Variable);
        if (!sceneOnboardingObject)
        {
            Debug.LogWarning($"Unable to find {nameof(SceneOnboardingObject)} with ID {SceneObjectIdentifier} - make sure it's present");
            
            return new ContextObjectResolveResult(null, null, null, null);
        }

        var meshRenderer = sceneOnboardingObject.GetComponent<MeshRenderer>();
        if (!meshRenderer)
        {
            Debug.LogWarning($"Unable to find {nameof(MeshRenderer)} on {nameof(SceneOnboardingObject)} with ID {SceneObjectIdentifier} - make sure it's present");
            
            return new ContextObjectResolveResult(null, null, null, null);
        }

        return new ContextObjectResolveResult(
            sceneOnboardingObject.gameObject,
            meshRenderer,
            null,
            null
        );
    }
}
using System;
using System.Linq;
using UnityEngine;
using ImmersiveVRTools.Runtime.Common.PropertyDrawer;

[RequireComponent(typeof(MeshRenderer))]
[AddComponentMenu(OnboardingEntityScriptableBase.AssetPath + "Guidance/Scene Onboarding Object", MenuOrder.Guidance)]
public class SceneOnboardingObject: MonoBehaviour
{
    [SerializeField] 
    [ReferenceOptions(ForceVariableOnly = true)] 
    public SceneOnboardingObjectIdentifierReference Identifier;

    void Start()
    {
        var duplicates = FindObjectsOfType<SceneOnboardingObject>()
            .Where(o => o.Identifier == Identifier)
            .ToList();

        if (duplicates.Any())
        {
            Console.WriteLine($"Identifier {Identifier} for '{nameof(SceneOnboardingObject)} is not unique, please adjust. Duplicates found:", this);

            foreach (var duplicate in duplicates)
            {
                Console.WriteLine($"Duplicate: {duplicate.name}", duplicate);
            }
        }
    }
}

[Serializable]
public class SceneOnboardingObjectIdentifierReference : Reference<string, SceneOnboardingObjectIdentifierVariable>
{
}
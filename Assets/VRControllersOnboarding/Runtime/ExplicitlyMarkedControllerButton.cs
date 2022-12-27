using UnityEngine;
using VRControllersOnboarding.Runtime;

[AddComponentMenu(OnboardingEntityScriptableBase.AssetPath + "Explicitly Marked Controller Button")]
public class ExplicitlyMarkedControllerButton : MonoBehaviour
{
    [SerializeField] public ControllerButtonMappingSet ControllerButtonMappingSet;
    [SerializeField] public Renderer MeshRenderer;
}
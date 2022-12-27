using System.Linq;
using ImmersiveVRTools.Runtime.Common.PropertyDrawer;
using UnityEngine;

[AddComponentMenu(OnboardingEntityScriptableBase.AssetPath + "Completion Rule/Custom Signal Emitter", MenuOrder.CompletionRules)]
public class CustomSignalEmitter: MonoBehaviour
{
    [SerializeField] 
    [ReferenceOptions(ForceVariableOnly = true)] 
    public CustomSignalReference Signal;

    public void Emit()
    {
        var vRControllerOnboardingManagers = FindObjectsOfType<VRControllerOnboardingManager>()
            .ToList();
        foreach (var vrControllerOnboardingManager in vRControllerOnboardingManagers)
        {
            vrControllerOnboardingManager.RegisterCustomSignal(Signal.Variable);
        }
    }
}
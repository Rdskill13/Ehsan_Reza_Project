using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;


[AddComponentMenu(OnboardingEntityScriptableBase.AssetPath + "Examples/Onboarding Auto Start", MenuOrder.Examples)]
public class OnboardingAutoStart : MonoBehaviour
{
    [SerializeField] private VRControllerOnboardingManager VRControllerOnboardingManager;
    [SerializeField] private OnboardingFlow OnboardingFlowToRun;
    [SerializeField] private float AutoStartAfterNSeconds = 2;
    
    void Start()
    {
        StartCoroutine(StartOnboarding());
    }

    IEnumerator StartOnboarding()
    {
        var onboardingWillStart = false;
        do
        {
            yield return new WaitForSeconds(AutoStartAfterNSeconds);
            
#if VRControllersOnboarding_Integrations_XRToolkit
            onboardingWillStart = !string.IsNullOrEmpty(InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).name)
                      && !string.IsNullOrEmpty(InputDevices.GetDeviceAtXRNode(XRNode.RightHand).name);
#elif VRControllersOnboarding_Integrations_SteamVR
            var renderModels = GameObject.FindObjectsOfType<Valve.VR.SteamVR_RenderModel>(); 
            onboardingWillStart = renderModels.Any(r => r.gameObject.GetComponentInParent<Valve.VR.InteractionSystem.Hand>().handType == Valve.VR.SteamVR_Input_Sources.LeftHand)
                                  && renderModels.Any(r => r.gameObject.GetComponentInParent<Valve.VR.InteractionSystem.Hand>().handType == Valve.VR.SteamVR_Input_Sources.RightHand);
#endif
            
            if (onboardingWillStart)
            {
                VRControllerOnboardingManager.StartOnboarding(OnboardingFlowToRun);
            }
            else
            {
               // Debug.LogWarning($"Controllers not initialized, will try auto-start onboarding in {AutoStartAfterNSeconds} seconds. " +
                 //         $"Please make sure controllers are on and visible in the scene.");
            }
        } while (!onboardingWillStart);
        
       

    }
}

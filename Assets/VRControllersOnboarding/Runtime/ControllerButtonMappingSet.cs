using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ImmersiveVRTools.Runtime.Common.PropertyDrawer;
using ImmersiveVRTools.Runtime.Common.Utilities;
using UnityEngine.Serialization;

namespace VRControllersOnboarding.Runtime
{
    [CreateAssetMenu(fileName = "ControllerButtonMappingSet", menuName = AssetPath + "ControllerButtonMappingSet", order = MenuOrder.Default)]
    public class ControllerButtonMappingSet : OnboardingEntityScriptableBase
    {
        [SerializeField] public ControllerHandedness Handednes; //TODO: is that needed, with mappings themselves carring that? is it just steam using it?

        [Help("Following mappings are set as explained on UnityXR mapping page. Unfortunately they are not handling all buttons on all controllers. " +
              "Generally it's advised to use more streamlined New Input System, or SteamVR. More info: https://docs.unity3d.com/Manual/xr_input.html", UnityMessageType.Info)]
        [FormerlySerializedAs("ControllerButtonMappingEntries")] [SerializeField] public List<UnityXRControllerMapping> UnityXRControllerButtonMappingEntries;
        
        [SerializeField] [ShowIf(nameof(ShowNewInputSystemActionFullPath))] public string NewInputSystemActionFullPath;

#if ENABLE_INPUT_SYSTEM
        private bool ShowNewInputSystemActionFullPath { get; } = true; //TODO: remove after testing
        [SerializeField] public UnityEngine.InputSystem.InputActionReference NewInputSystemAction;

#else
        private bool ShowNewInputSystemActionFullPath { get; } = true;
#endif

        [SerializeField] public string OVRButtonName;

        [SerializeField] [ShowIf(nameof(ShowSteamVrActionFullPath))] public string SteamVRActionFullPath;
        
#if VRControllersOnboarding_Integrations_SteamVR
        private bool ShowSteamVrActionFullPath { get; } = false;
        
        [SerializeField] public Valve.VR.SteamVR_Action_Boolean SteamVrBoolAction;

#else 
        private bool ShowSteamVrActionFullPath { get; } = true;
#endif
        
        [SerializeField] [TextArea(1, 10)] public string MappingNotes;
        
        private void OnValidate()  
        {
            if(!_onEnableRun)  
                return;

#if VRControllersOnboarding_Integrations_SteamVR
            var steamVrPath = SteamVrBoolAction.GetPath();
            SteamVRActionFullPath = !string.IsNullOrEmpty(steamVrPath) ? steamVrPath : "";
#endif
            
#if ENABLE_INPUT_SYSTEM
            NewInputSystemActionFullPath = NewInputSystemAction ? NewInputSystemAction.name : "";
#endif
        }

        private bool _onEnableRun;
        private void OnEnable()
        {
            _onEnableRun = true;

#if UNITY_EDITOR
#if VRControllersOnboarding_Integrations_SteamVR
            if (!string.IsNullOrEmpty(SteamVRActionFullPath))
            {
                Valve.VR.SteamVR_Action_Boolean boolAction = null;

                if((boolAction = Valve.VR.SteamVR_Input.GetActionFromPath<Valve.VR.SteamVR_Action_Boolean>(SteamVRActionFullPath)) != null)
                {
                    SteamVrBoolAction = boolAction;
                }
                
                if(boolAction == null)
                {
                    Debug.LogWarning($"Unable to find SteamVR action for '{SteamVRActionFullPath}', please update binding", this);
                }
            }  
#endif
            
#if ENABLE_INPUT_SYSTEM
            if (!string.IsNullOrEmpty(NewInputSystemActionFullPath))
            {
                NewInputSystemAction = RuntimeSafeAssetDatabaseHelper.GetAssetOrSubAsset<UnityEngine.InputSystem.InputActionReference>(NewInputSystemActionFullPath); 
            }
#endif
#endif
        }

        private void OnDisable()
        {
            _onEnableRun = false;
        }
    }
}
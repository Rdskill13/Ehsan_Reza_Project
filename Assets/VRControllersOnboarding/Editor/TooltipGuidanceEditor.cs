using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace VRControllersOnboarding.Editor
{
    [CustomEditor(typeof(TooltipGuidance))]
    public class TooltipGuidanceEditor: UnityEditor.Editor
    {
        private VRControllerOnboardingManager _vRControllerOnboardingManager;
        
        public override void OnInspectorGUI()
        {
            if (!_vRControllerOnboardingManager)
            {
                _vRControllerOnboardingManager = GameObject.FindObjectOfType<VRControllerOnboardingManager>();
            }
            
            serializedObject.Update();
            
            var iterator = serializedObject.GetIterator();
            var enterChildren = true;
            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false; //first call to NextVisible needs to have enter children = true...
                EditorGUILayout.PropertyField(iterator, false);
                if (iterator.name == nameof(TooltipGuidance.TooltipOffset))
                {
                    var tooltipGuidance = (TooltipGuidance)target;
                    var isButtonDisabled = IsAdjustOffsetButtonDisabled();
                    if (isButtonDisabled)
                    {
                        EditorGUILayout.HelpBox("You can only adjust in playmode when tooltip is visible for your guidance", MessageType.Warning);
                        GUI.enabled = false;
                    }
                    
                    if (GUILayout.Toggle(tooltipGuidance.IsAdjustingTooltipOffsetViaHandles, $"Adjust Offset via scene handles", "Button")
                    )
                    {
                        tooltipGuidance.IsAdjustingTooltipOffsetViaHandles = true;
                    }
                    else
                    {
                        tooltipGuidance.IsAdjustingTooltipOffsetViaHandles = false;
                    }
                    
                    if (isButtonDisabled)
                        GUI.enabled = true;
                }
            }

            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties(); 
            }
        }

        private bool IsAdjustOffsetButtonDisabled()
        {
            if(!EditorApplication.isPlaying) return true;
            if (!_vRControllerOnboardingManager) return true;
            
            if (_vRControllerOnboardingManager.OnboardingFlowState != null && _vRControllerOnboardingManager.OnboardingFlowState.CurrentlyExecutingOnboardingSetup)
            {
                if (_vRControllerOnboardingManager.OnboardingFlowState.CurrentlyExecutingOnboardingSetup.Guidances.Any(g => g == (TooltipGuidance)target))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
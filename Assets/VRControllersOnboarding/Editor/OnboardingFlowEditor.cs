using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using ImmersiveVRTools.Editor.Common.Editor;

namespace VRControllersOnboarding.Editor
{
    [CustomEditor(typeof(OnboardingFlow))]
    [CanEditMultipleObjects]
    public class OnboardingFlowEditor : NestedScriptableObjectEditorBase<MappingToOnboardingSetupBase>
    { 
        protected override string NestedPropertyName { get; } = nameof(OnboardingFlow.MappingToOnboardingSetupEntries);
        protected override string NestedPropertyLabel { get; } = "Onboarding Setup";
        protected override List<AddNestedScriptableObjectParams> AddNestedScriptableObjectParams { get; } = new List<AddNestedScriptableObjectParams>
        {
            new AddNestedScriptableObjectParams("Button Mapping", typeof(ButtonMappingToOnboardingSetup)),
            new AddNestedScriptableObjectParams("Scene Onboarding Object Mapping", typeof(SceneOnboardingObjectMappingToOnboardingSetup)),
        };

        private List<string> _movePropertyNamesToTop;
        protected override List<string> MovePropertyNamesToTop { get; } = new List<string>
        {
            nameof(SceneOnboardingObjectMappingToOnboardingSetup.SceneObjectIdentifier),
            nameof(ButtonMappingToOnboardingSetup.ControllerButtonMappingSet)
        }; 

        protected override void RenderNestedProperty(SerializedProperty property, SerializedObject nestedObject, ref Rect editorRect)
        {
            if (property.name == nameof(ButtonMappingToOnboardingSetup.ControllerButtonMappingSet))
            {
                GUI.enabled = false;
                base.RenderNestedProperty(property, nestedObject, ref editorRect);
                GUI.enabled = true;
                if (GUI.Button(editorRect, "Change/Preview UnityXR Mappings"))
                {
                    var window = (ChangeUnityXRMappingsEditorWindow)EditorWindow.GetWindow(typeof(ChangeUnityXRMappingsEditorWindow));
                    window.Init(property, nestedObject);
                }

                editorRect.y += EditorGUIUtility.singleLineHeight;
                
                //TODO: ButtonGameObject resolution is set to ButtonsExplicitlyMarked then show correct ExplicitlyMarkedControllerButton, what if they are on prefabs and not on scene? let them know at runtime?
            }
            else
            {
                base.RenderNestedProperty(property, nestedObject, ref editorRect);
            }
        }
    }
}
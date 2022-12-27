using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using ImmersiveVRTools.Editor.Common.Editor;
using VRControllersOnboarding.Runtime;

namespace VRControllersOnboarding.Editor
{
    [CustomEditor(typeof(ControllerMappings))]
    [CanEditMultipleObjects]
    public class ControllerMappingsEditor : NestedScriptableObjectEditorBase<UnityXRControllerMapping>
    {
        protected override string NestedPropertyName { get; } = nameof(ControllerMappings.Mappings);
        protected override string NestedPropertyLabel { get; } = "Button Bindings";
        protected override List<AddNestedScriptableObjectParams> AddNestedScriptableObjectParams { get; } = new List<AddNestedScriptableObjectParams>()
        {
            new AddNestedScriptableObjectParams("Add Binding", typeof(UnityXRControllerMapping),
                (createdScriptableObject, parent) =>
                {
                    ((UnityXRControllerMapping)createdScriptableObject).UnityXRControllerMappingsRoot = (ControllerMappings)parent;
                })
        };
    }
}
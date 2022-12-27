using UnityEditor;
using UnityEngine;

namespace VRControllersOnboarding.Editor
{
    public class OnboardingEntityEditorScriptableBase<TType>: UnityEditor.Editor
        where TType : ScriptableObject
    {
        protected const string AssetPath = "Assets/Create/VR Controllers Onboarding/";
    
        protected static void CreateInternal()
        {
            var asset = ScriptableObject.CreateInstance<TType>();
            //TODO: how to specify where to create?
            AssetDatabase.CreateAsset(asset, $"Assets/{asset.GetType().Name}.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
}
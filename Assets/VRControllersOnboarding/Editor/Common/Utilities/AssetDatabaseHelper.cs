using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ImmersiveVRTools.Editor.Common.Utilities
{
    public class AssetDatabaseHelper
    {
        public static List<T> GetAllScriptableObjects<T>() where T : ScriptableObject
        {
            var guids = AssetDatabase.FindAssets("t:" + typeof(T).Name); //FindAssets uses tags check documentation for more info
            return guids.Select(AssetDatabase.GUIDToAssetPath).Select(AssetDatabase.LoadAssetAtPath<T>).ToList();
        }

        public static T GetAsset<T>(string name) where T : UnityEngine.Object  
        {
            if (TryGetAssetPath<T>(name, out var assetPath))
            {
                var assets = AssetDatabase.LoadAllAssetsAtPath(assetPath);
                if (assets.Length == 0) return null;
                if (assets.Length == 1) return (T) assets[0];
                else
                {
                    return (T)assets.FirstOrDefault(a => a.name == name);
                }
            }

            return null;
        }

        public static bool TryGetAssetPath<T>(string name, out string assetPath) where T : class
        {
            var found = AssetDatabase.FindAssets($"{name} t:{typeof(T).Name}");
            if (found.Length == 0)
            {
                Debug.LogWarning($"Unable to find new input system action for {name} please update input profile.");
                assetPath = string.Empty;
                return false;
            }

            if (found.Length > 1)
            {
                Debug.LogWarning($"Multiple new input system actions found for {name} using first.");
            }

            assetPath = AssetDatabase.GUIDToAssetPath(found.First());
            return true;
        }
    }
}
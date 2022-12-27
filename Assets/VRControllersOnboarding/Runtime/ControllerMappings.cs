using System;
using System.Collections.Generic;
using UnityEngine;

namespace VRControllersOnboarding.Runtime
{
    [Serializable]
    [CreateAssetMenu(fileName = "ControllerMappings", menuName = AssetPath + "ControllerMappings", order = MenuOrder.Default)]
    //TODO: rename to UnityXRControllerMappings
    public class ControllerMappings : OnboardingEntityScriptableBase
    {
        [SerializeField] public string DeviceNameMatchingRegex;
        [SerializeField] public List<UnityXRControllerMapping> Mappings;
    }

    public enum ControllerHandedness
    {
        Left,
        Right
    }
}
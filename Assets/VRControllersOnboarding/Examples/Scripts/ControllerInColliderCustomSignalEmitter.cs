using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CustomSignalEmitter))]
[RequireComponent(typeof(BoxCollider))]
[AddComponentMenu(OnboardingEntityScriptableBase.AssetPath + "Examples/Controller In Collider Custom Signal Emitter", MenuOrder.Examples)]
public class ControllerInColliderCustomSignalEmitter : MonoBehaviour
{
    private CustomSignalEmitter _customSignalEmitter;
    private BoxCollider _boxCollider;
    
    [SerializeField] private List<GameObject> Controllers;
    
    void Start()
    {
        _customSignalEmitter = GetComponent<CustomSignalEmitter>();
        _boxCollider = GetComponent<BoxCollider>();
    }
    
    void FixedUpdate()
    {
        foreach (var controller in Controllers)
        {
            if (_boxCollider.bounds.Contains(controller.transform.position))
            {
                _customSignalEmitter.Emit();
            }
        }
    }
}

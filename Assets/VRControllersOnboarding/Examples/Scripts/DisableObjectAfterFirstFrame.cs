using UnityEngine;

[AddComponentMenu(OnboardingEntityScriptableBase.AssetPath + "Examples/Disable Object After First Frame", MenuOrder.Examples)]
public class DisableObjectAfterFirstFrame : MonoBehaviour
{
    private bool _isAfterFirstUpdate = false;
    private bool _alreadyExecuted = false;
    
    void LateUpdate()
    {
        if (_alreadyExecuted)
        {
            return;
        }
        
        if (_isAfterFirstUpdate)
        {
            gameObject.SetActive(false);
            _alreadyExecuted = true;
        }

        _isAfterFirstUpdate = true;
    }
}
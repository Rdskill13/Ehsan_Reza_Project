using UnityEngine;

[AddComponentMenu(OnboardingEntityScriptableBase.AssetPath + "Examples/Move In Front Of Camera", MenuOrder.Examples)]
public class MoveInfrontOfCamera : MonoBehaviour
{
    [SerializeField] public float DistanceFromCamera = 1;
    [SerializeField] public Vector3 Adjustment;

    #if IMMERSIVE_VR_TOOLS_DEVELOP_AIDS
    [UnityEditor.MenuItem("DEBUG/Position Demo Objects in front of camera #F3")]
    static void StartOnboardingDebug()
    {
        foreach (var positioner in GameObject.FindObjectsOfType<MoveInfrontOfCamera>())
        {
            positioner.MoveInFrontOfCamera();
        }
    }
#endif
    
    public void MoveInFrontOfCamera()
    {
        var camera = Camera.main;

        transform.position = camera.transform.position + (camera.transform.forward * DistanceFromCamera) + Adjustment;
    }
}

using System.Collections;
using System.Linq;
using UnityEngine;
using ImmersiveVRTools.Runtime.Common.PropertyDrawer;
using ImmersiveVRTools.Runtime.Common.ScriptableObject;
using VRControllersOnboarding.Runtime.UX;


[CreateAssetMenu(fileName = "TooltipGuidance", menuName = AssetPath + "Guidance/TooltipGuidance", order = MenuOrder.Guidance)]
public class TooltipGuidance : OnboardingGuidance
{
    [SerializeField] public Vector3 TooltipOffset = new Vector3(0, 0, 0);
    
    [SerializeField] private GameObject ControllerTooltipPrefab;
    [SerializeField] private bool ShowTooltipToButtonConnectionAsLine;
    [SerializeField] [TextArea(3, 10)] private string Text;
    
    [Tooltip("By default tooltip will grow as large in width as needed (including line breaks). Use this field to force specific width.")]
    [SerializeField] private int ForceWidth = 0;
    
    public bool IsAdjustingTooltipOffsetViaHandles { get; set; }

    public override void StartGuidanceInternal(ContextObjectResolveResult contextObjectResolutionResult, ScriptableObjectStateRoot state)    
    {
        if (!IsMeshRendererAvailable(contextObjectResolutionResult))
        {
            return;
        }

        var userFacingTooltip = contextObjectResolutionResult.GameObject.GetComponent<UserFacingTooltip>();
        if ( !userFacingTooltip || userFacingTooltip.IsBeingDestroyed)
        {
            var wasAdjustingTooltipOffsetViaHandlesTriggered = false;
            userFacingTooltip = contextObjectResolutionResult.GameObject.AddComponent<UserFacingTooltip>();
            var initialAreSmoothTransitionsEnabled = userFacingTooltip.AreSmoothTransitionsEnabled;
            var tooltipGuidanceState = state.Get<TooltipGuidanceState>(this);
            tooltipGuidanceState.UserFacingTooltip = userFacingTooltip;
            userFacingTooltip.Initialize(
                ControllerTooltipPrefab, contextObjectResolutionResult.GameObject.transform,
                Text,
                (startLocalPosition) =>
                {
                    //HACK: bit of a hack to work that out in here, offset may not be called if smooth move is enabled and controller is not moved beyond trigger point
                    if (IsAdjustingTooltipOffsetViaHandles)
                    {
                        userFacingTooltip.AreSmoothTransitionsEnabled = false;
                        wasAdjustingTooltipOffsetViaHandlesTriggered = true;
                        
                        if (!tooltipGuidanceState.TooltipOffsetTransform)
                        {
                            tooltipGuidanceState.TooltipOffsetTransform = new GameObject("TooltipOffsetAdjustment").transform;
                            tooltipGuidanceState.TooltipOffsetTransform.parent = userFacingTooltip.CanvasOffset.parent;
                            tooltipGuidanceState.TooltipOffsetTransform.position = userFacingTooltip.CanvasOffset.position;
                        }
                        
#if UNITY_EDITOR
                        if(!tooltipGuidanceState.WasTooltipOffsetTransformGameObjectMadeActiveAlready) {
                            UnityEditor.Selection.activeGameObject = tooltipGuidanceState.TooltipOffsetTransform.gameObject;
                            tooltipGuidanceState.WasTooltipOffsetTransformGameObjectMadeActiveAlready = true;
                        }
                        else if(UnityEditor.Selection.activeGameObject != tooltipGuidanceState.TooltipOffsetTransform.gameObject) {
                            tooltipGuidanceState.WasTooltipOffsetTransformGameObjectMadeActiveAlready = false;
                            IsAdjustingTooltipOffsetViaHandles = false;
                        }
#endif

                        TooltipOffset = tooltipGuidanceState.TooltipOffsetTransform.localPosition - startLocalPosition;
                    }
                    else if(wasAdjustingTooltipOffsetViaHandlesTriggered)
                    {
                        userFacingTooltip.AreSmoothTransitionsEnabled = initialAreSmoothTransitionsEnabled;
                        wasAdjustingTooltipOffsetViaHandlesTriggered = false;
                    }

                    return TooltipOffset;
                },
                () => Text,
                () => ForceWidth,
                ShowTooltipToButtonConnectionAsLine
            );

        }
    }

    public override void ProcessUpdateInternal(ContextObjectResolveResult contextObjectResolutionResult, ScriptableObjectStateRoot state)
    {

    }

    public override void StopGuidanceInternal(ContextObjectResolveResult contextObjectResolutionResult, ScriptableObjectStateRoot state)
    {
        if (!IsMeshRendererAvailable(contextObjectResolutionResult))
        {
            return;
        }

        var userFacingTooltip = contextObjectResolutionResult.GameObject.GetComponent<UserFacingTooltip>();
        if (userFacingTooltip)
        {
            IsAdjustingTooltipOffsetViaHandles = false;
            userFacingTooltip.SelfDestruct();
        }
    }

    public override IEnumerator RenderUserAttentionGrabberAfterTooMuchTimePassedCoroutine(ContextObjectResolveResult contextObjectResolutionResult, ScriptableObjectStateRoot state)
    {
        yield break;
        //TODO: add some background color lerp to bring user attention?
    }

    private void Reset()
    {
        ShowTooltipToButtonConnectionAsLine = true;
        if (!ControllerTooltipPrefab)
        {
#if UNITY_EDITOR

            var controllerTooltipPrefabsFound = UnityEditor.AssetDatabase.FindAssets("ControllerTooltip t:Prefab");
            if (controllerTooltipPrefabsFound.Length == 0)
            {
                Debug.LogWarning("No ControllerTooltip prefab found, you have to add manually");
                return;
            }

            if (controllerTooltipPrefabsFound.Length > 1)
            {
                Debug.LogWarning("Multiple ControllerTooltip prefabs found, using first");
            }

            ControllerTooltipPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(UnityEditor.AssetDatabase.GUIDToAssetPath(controllerTooltipPrefabsFound.First()));
#endif
        }
    }

    private class TooltipGuidanceState: ScriptableObjectState
    {
        public UserFacingTooltip UserFacingTooltip { get; set; }
        public Transform TooltipOffsetTransform { get; set; }
        public bool WasTooltipOffsetTransformGameObjectMadeActiveAlready { get; set; }
    }
}
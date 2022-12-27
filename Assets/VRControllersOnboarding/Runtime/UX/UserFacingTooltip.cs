using System;
using System.Collections;
using ImmersiveVRTools.Runtime.Common.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace VRControllersOnboarding.Runtime.UX
{
    public class UserFacingTooltip: MonoBehaviour
    {
        [SerializeField] private GameObject TooltipPrefab;
        [SerializeField] private Transform AttachToButtonTransform;
        [SerializeField] private bool ShowTooltipToButtonConnectionAsLine;
        [SerializeField] private Vector3 StaticTooltipOffset;
        [SerializeField] private string TextContent;

        [Header("Smooth Transitions")]
        [SerializeField] public bool AreSmoothTransitionsEnabled = true;
        [SerializeField] private float SmoothTooltipMoveOnlyAfterThreshold = 0.05f;
        [SerializeField] private float SmoothTooltipMoveLerpOverNSeconds = 0.25f;
        
        [SerializeField] private float SmoothTooltipRotateOnlyAfterThreshold = 0.05f;
        [SerializeField] private float SmoothTooltipRotateLerpOverNSeconds = 0.25f;

        public bool IsBeingDestroyed { get; private set; }
        private Camera MainCamera => _mainCamera ?? (_mainCamera = Camera.main);

        private GameObject _tooltipRootObject;
        private Transform _textStartAnchor;
        private Transform _textEndAnchor;
        public Transform CanvasOffset { get; private set; }
        private LineRenderer _lineRenderer;
        private Canvas _textCanvas;
        private Text _text;
        private TextMesh _textMesh;
        private Transform _textHintParent;
        private Camera _mainCamera;
        private GameObject _textPanel;
        private RectTransform _textPanelRectTransform;
        private ContentSizeFitter _contentSizeFitter;
        
        private Vector3 _distanceFromRootAtLastPositionUpdate;
        private Vector3 _tooltipLockedInPositionAtLastPositionUpdate;
        private Vector3 _lastPlayerToTooltipDirectionWhenRotationAdjusted;
        private Quaternion _lastTooltipRotationWhenRotationAdjusted;
        private bool _forceSmoothUpdateNextFrame = false;
        private Coroutine _smoothMoveCoroutine;
        private Coroutine _smoothRotateCoroutine;
        
        private Func<Vector3, Vector3> _getTooltipOffset;
        private Func<string> _getTooltipText;
        private Func<int> _getWidth;

        public void Initialize(GameObject controllerPrefab, Transform attachToButtonTransform, string textContent, Func<Vector3, Vector3> getTooltipOffset, 
            Func<string> getTooltipText, Func<int> getWidth, bool showTooltipToButtonConnectionAsLine)
        {
            _getWidth = getWidth;
            _getTooltipText = getTooltipText;
            TooltipPrefab = controllerPrefab;
            AttachToButtonTransform = attachToButtonTransform;
            TextContent = textContent;
            _getTooltipOffset = getTooltipOffset;
            ShowTooltipToButtonConnectionAsLine = showTooltipToButtonConnectionAsLine;
        }
        
        private void Start()
        {
            CreateAndInitializeElements();
        }

        public void EnableSmoothTransitions()
        {
            
        }

        public void DisableSmoothTransitions()
        {
            
        }

        public void SelfDestruct()
        {
            IsBeingDestroyed = true;
            Destroy(this);
            Destroy(_textHintParent.gameObject);
        }

        private void Update()
        {
            _getTooltipOffset(Vector3.zero); //HACK: this is to make AdjustTooltipOffset work, in that getter there's a bit of logic that's going to turn off smooth transitions that prevent adjustments from working
            
            MakeTooltipFacePlayer(_forceSmoothUpdateNextFrame);
            UpdateDynamicEditorAdjustableValues(_forceSmoothUpdateNextFrame);
            _forceSmoothUpdateNextFrame = false;

            if (ShowTooltipToButtonConnectionAsLine)
            {
                var lineTransform = _lineRenderer.transform;
                _lineRenderer.useWorldSpace = false;
                _lineRenderer.SetPosition(0, lineTransform.InverseTransformPoint(_textStartAnchor.position));
                _lineRenderer.SetPosition(1, lineTransform.InverseTransformPoint(_textEndAnchor.position));
            }
            else
            {
                _lineRenderer.enabled = false;
            }
        }

        private void MakeTooltipFacePlayer(bool forceUpdate)
        {
            var playerTransform = MainCamera.transform;
            var vDir = CalculatePlayerToTooltipDirectionWhenRotationAdjusted(playerTransform);

            if (AreSmoothTransitionsEnabled)
            {
                if (forceUpdate || (vDir - _lastPlayerToTooltipDirectionWhenRotationAdjusted).magnitude >= SmoothTooltipRotateOnlyAfterThreshold)
                {
                    _lastPlayerToTooltipDirectionWhenRotationAdjusted = vDir;
                
                    var canvasOffsetRotation = CalculateCanvasOffsetRotationToFaceCamera(vDir, playerTransform);
                    if (_smoothRotateCoroutine != null)
                    {
                        StopCoroutine(_smoothRotateCoroutine);
                        _smoothRotateCoroutine = null;
                    }

                    _smoothRotateCoroutine = StartCoroutine(SmoothRotate(canvasOffsetRotation, SmoothTooltipRotateLerpOverNSeconds));
                }
                else
                {
                    CanvasOffset.rotation = _lastTooltipRotationWhenRotationAdjusted;
                }
 
            }
            else
            {
                var canvasOffsetRotation = CalculateCanvasOffsetRotationToFaceCamera(vDir, playerTransform);
                CanvasOffset.rotation = canvasOffsetRotation;
            }
        }

        private static Quaternion CalculateCanvasOffsetRotationToFaceCamera(Vector3 vDir, Transform playerTransform)
        {
            var standardLookat = Quaternion.LookRotation(vDir, Vector3.up);
            var upsideDownLookat = Quaternion.LookRotation(vDir, playerTransform.up);

            float flInterp;
            if (playerTransform.forward.y > 0.0f)
            {
                flInterp = RemapNumberClamped(playerTransform.forward.y, 0.6f, 0.4f, 1.0f, 0.0f);
            }
            else
            {
                flInterp = RemapNumberClamped(playerTransform.forward.y, -0.8f, -0.6f, 1.0f, 0.0f);
            }

            var canvasOffsetRotation = Quaternion.Slerp(standardLookat, upsideDownLookat, flInterp);
            return canvasOffsetRotation;
        }

        private IEnumerator SmoothRotate(Quaternion endRotation, float overNSeconds)
        {
            var startRotation = CanvasOffset.rotation;
            float elapsedTime = 0;
            
            while (elapsedTime < overNSeconds)
            {
                elapsedTime += Time.deltaTime;

                var canvasOffsetRotationLerped = Quaternion.Lerp(startRotation, endRotation, (elapsedTime / overNSeconds));
                CanvasOffset.rotation = canvasOffsetRotationLerped;
                _lastTooltipRotationWhenRotationAdjusted = canvasOffsetRotationLerped;
                
                yield return null;
            }

            _smoothRotateCoroutine = null;
        }
        
        private void CreateAndInitializeElements()
        {
            _textHintParent = new GameObject("Tooltip").transform;
            _textHintParent.SetParent(this.transform);
            _textHintParent.localPosition = Vector3.zero;
            _textHintParent.localRotation = Quaternion.identity;
            _textHintParent.localScale = Vector3.one;

            
            var hintStartPos = GetStartPosition();
            _tooltipRootObject = GameObject.Instantiate(TooltipPrefab, hintStartPos, Quaternion.identity) as GameObject;
            _tooltipRootObject.name = "Tooltip_Root";
            _tooltipRootObject.transform.SetParent(_textHintParent);
            _tooltipRootObject.transform.localRotation = Quaternion.identity;
            _tooltipRootObject.layer = gameObject.layer;
            _tooltipRootObject.tag = gameObject.tag;

            _textStartAnchor = _tooltipRootObject.transform.Find("Start");
            _textEndAnchor = _tooltipRootObject.transform.Find("End");
            CanvasOffset = _tooltipRootObject.transform.Find("CanvasOffset");
            _lineRenderer = _tooltipRootObject.transform.Find("Line").GetComponent<LineRenderer>();
            _textCanvas = _tooltipRootObject.GetComponentInChildren<Canvas>();
            _text = _textCanvas.GetComponentInChildren<Text>();
            _textMesh = _textCanvas.GetComponentInChildren<TextMesh>();
            _textPanel = _textCanvas.transform.Find("TextPanel").gameObject;
            _textPanelRectTransform = _textPanel.GetComponent<RectTransform>();
            _contentSizeFitter = _textPanel.GetComponent<ContentSizeFitter>();

            _distanceFromRootAtLastPositionUpdate = CalculateCanvasDiffFromRoot();
            _tooltipLockedInPositionAtLastPositionUpdate = CanvasOffset.position;
            
            UpdateDynamicEditorAdjustableValues(true);
            
            _lastPlayerToTooltipDirectionWhenRotationAdjusted = CalculatePlayerToTooltipDirectionWhenRotationAdjusted(MainCamera.transform);
            _lastTooltipRotationWhenRotationAdjusted = CanvasOffset.rotation;

            _forceSmoothUpdateNextFrame = true;
        }

        private Vector3 CalculatePlayerToTooltipDirectionWhenRotationAdjusted(Transform playerTransform)
        {
            return playerTransform.position - CanvasOffset.position;
        }

        private Vector3 CalculateCanvasDiffFromRoot()
        {
            return CanvasOffset.position - _textHintParent.position;
        }

        private Vector3 GetStartPosition()
        {
            return AttachToButtonTransform.position + (AttachToButtonTransform.forward * 0.01f);
        }

        private IEnumerator SmoothMove(Vector3 endPosition, float overNSeconds)
        {
            var startPos = _textEndAnchor.localPosition;
            float elapsedTime = 0;
            
            while (elapsedTime < overNSeconds)
            {                
                elapsedTime += Time.deltaTime;
                
                var endPositionLerped = Vector3.Lerp(startPos, endPosition, (elapsedTime / overNSeconds));
                _textEndAnchor.localPosition = endPositionLerped;
                CanvasOffset.localPosition = endPositionLerped;
                _tooltipLockedInPositionAtLastPositionUpdate = CanvasOffset.position;
                _distanceFromRootAtLastPositionUpdate = CalculateCanvasDiffFromRoot();
                
                yield return null;
            }

            _smoothMoveCoroutine = null;
        }

        private void UpdateDynamicEditorAdjustableValues(bool forceUpdate)
        {
            //position
            var currentDistanceFromRoot =  CalculateCanvasDiffFromRoot();
            var newPositionAdjustment = _distanceFromRootAtLastPositionUpdate - currentDistanceFromRoot;

            if (AreSmoothTransitionsEnabled)
            {
                if (!forceUpdate && newPositionAdjustment.magnitude <= SmoothTooltipMoveOnlyAfterThreshold)
                {
                    _textEndAnchor.position = _tooltipLockedInPositionAtLastPositionUpdate;
                    CanvasOffset.position = _tooltipLockedInPositionAtLastPositionUpdate;
                }
                else
                {
                    if (_smoothMoveCoroutine != null)
                    {
                        StopCoroutine(_smoothMoveCoroutine);
                        _smoothMoveCoroutine = null;
                    }

                    var endPosition = CalculateTooltipEndPosition();
                    StartCoroutine(SmoothMove(endPosition, SmoothTooltipMoveLerpOverNSeconds));
                }
            }
            else
            {
                var endPosition = CalculateTooltipEndPosition();
                _textEndAnchor.localPosition = endPosition;
                CanvasOffset.localPosition = endPosition;
            }

            
            //text
            var previousContent = _text.text;
            TextContent = _getTooltipText();
            var shouldUpdateTextContent = TextContent != previousContent;
            if (_text && shouldUpdateTextContent)
            {
                _text.text = TextContent;
            }

            if (_textMesh && shouldUpdateTextContent)
            {
                _textMesh.text = TextContent;
            }
            
            //width
            var forceWidth = _getWidth();
            if (forceWidth != 0 && Math.Abs(_textPanelRectTransform.sizeDelta.x - forceWidth) > 0.01f)
            {
                _contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
                _text.horizontalOverflow = HorizontalWrapMode.Wrap;
                _textPanelRectTransform.sizeDelta = new Vector2(forceWidth, 0); //height will be re-calculated
            }
            else if(forceWidth == 0 && _text.horizontalOverflow == HorizontalWrapMode.Wrap)
            {
                _contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                _text.horizontalOverflow = HorizontalWrapMode.Overflow;
            }
        }

        private Vector3 CalculateTooltipEndPosition()
        {
            _textStartAnchor.position = AttachToButtonTransform.position;
            var endPosition = _getTooltipOffset?.Invoke(_textStartAnchor.localPosition) ?? StaticTooltipOffset;
            return endPosition;
        }

        private static float RemapNumberClamped( float num, float low1, float high1, float low2, float high2 )
        {
            return Mathf.Clamp( RemapNumber( num, low1, high1, low2, high2 ), Mathf.Min( low2, high2 ), Mathf.Max( low2, high2 ) );
        }

        private static float RemapNumber( float num, float low1, float high1, float low2, float high2 )
        {
            return low2 + ( num - low1 ) * ( high2 - low2 ) / ( high1 - low1 );
        }
    }
}
using UnityEngine;
using ImmersiveVRTools.Runtime.Common.Debug;
using ImmersiveVRTools.Runtime.Common.ScriptableObject;

namespace VRControllersOnboarding.Runtime.CompletionRules
{
    [CreateAssetMenu(fileName = "ObjectMoved", menuName = AssetPath + "Completion Rule/Object Moved", order = MenuOrder.CompletionRules)]
    public class ObjectMovedCompletionRule: OnboardingActionCompletionRule
    {
        [SerializeField] private float DistanceToConsiderSatisfied = 0.2f;
        
        [SerializeField] private bool IsDebug;

        public override bool IsRuleSatisfied(ContextObjectResolveResult contextObjectResolveResult, ScriptableObjectStateRoot rootState)
        {
            var state = rootState.Get<ObjectMovedCompletionRuleState>(contextObjectResolveResult.GameObject);
            if (!state.TransformDistanceReference)
            {
                InitializeTransformDistanceReference(contextObjectResolveResult, state);
            }
            
            if (IsDebug)
            {
                DebugDraw.Point(state.TransformDistanceReference.position, Color.red, DistanceToConsiderSatisfied);
            }
            
            return Mathf.Abs(Vector3.Distance(contextObjectResolveResult.GameObject.transform.position, state.TransformDistanceReference.position)) > DistanceToConsiderSatisfied;
        }
        
        private void InitializeTransformDistanceReference(ContextObjectResolveResult contextObjectResolveResult, ObjectMovedCompletionRuleState state)
        {
            if (state.TransformDistanceReference == null)
            {
                state.TransformDistanceReference = new GameObject(name + "-DistanceReference").transform;
                if (!contextObjectResolveResult.GameObject.transform.parent) state.TransformDistanceReference.parent = contextObjectResolveResult.GameObject.transform.parent;
                state.TransformDistanceReference.position = contextObjectResolveResult.GameObject.transform.position;
            }
        }
    }

    public class ObjectMovedCompletionRuleState: ScriptableObjectState
    {
        public Transform TransformDistanceReference { get; set; }
    }
}
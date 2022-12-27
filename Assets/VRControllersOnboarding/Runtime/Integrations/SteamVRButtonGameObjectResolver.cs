#if VRControllersOnboarding_Integrations_SteamVR

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using ImmersiveVRTools.Runtime.Common.Extensions;

namespace VRControllersOnboarding.Runtime.Integrations
{
    public class SteamVRButtonGameObjectResolver: IButtonGameObjectResolver
    {
        public static readonly Dictionary<ControllerHandedness, SteamVR_Input_Sources> ControllerHandednessToSteamVrInputSourceMap = 
            new Dictionary<ControllerHandedness, SteamVR_Input_Sources>()
        {
            [ControllerHandedness.Left] = SteamVR_Input_Sources.LeftHand,
            [ControllerHandedness.Right] = SteamVR_Input_Sources.RightHand,
        }.AsAllElementsRequiredMap();
        
        private static FieldInfo _meshRenderersField;

        public ContextObjectResolveResult Resolve(ControllerButtonMappingSet forMappingSet)
        {
            var renderModels = GameObject.FindObjectsOfType<Valve.VR.SteamVR_RenderModel>(); 
            if (_meshRenderersField == null)
            {
                _meshRenderersField = typeof(SteamVR_RenderModel).GetField("meshRenderers", BindingFlags.Instance | BindingFlags.NonPublic);
            }

            foreach (var renderModel in renderModels)
            {
                var parentHand = renderModel.gameObject.GetComponentInParent<Hand>();
                if (parentHand.handType != ControllerHandednessToSteamVrInputSourceMap[forMappingSet.Handednes])
                {
                    continue;
                }
                
                var meshRenderers = (List<MeshRenderer>)_meshRenderersField.GetValue(renderModel);
                var matchingRenderer = meshRenderers
                    .FirstOrDefault(mr => forMappingSet.SteamVrBoolAction != null 
                                          && mr.enabled && mr.gameObject.name == forMappingSet.SteamVrBoolAction.renderModelComponentName);
                if (matchingRenderer)
                {
                    return new ContextObjectResolveResult(
                        matchingRenderer.gameObject.transform.Find("attach").gameObject, //steamVR button middle point with have 'attach' child on it
                        matchingRenderer,
                        forMappingSet,
                        null
                    );
                }
            }
            
            //TODO: do we need to handle that differently on higher level, eg retry?
            throw new ContextObjectNotFoundException($"Unable to find SteamVR controller game object for button '{(forMappingSet.SteamVrBoolAction != null ? forMappingSet.SteamVrBoolAction.renderModelComponentName : "Unknown")}'. MappingSet: '{forMappingSet.name}'" +
                                                     $"Make sure SteamVR input bindings are set correct as well as MappingSets are pointing to correct actions.");
        }
    }
}

#endif
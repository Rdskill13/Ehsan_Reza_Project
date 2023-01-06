using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class MRIConfig_transform : BaseTeleportationInteractable
{

    [SerializeField]
    [Tooltip("The Transform that represents the teleportation destination.")]
    Transform m_TeleportAnchorTransform;

    //*********** this is my code
    [SerializeField] private bool Under_MRI = false;
    [SerializeField] private Transform MRI_Pos_Rot;


    [SerializeField] private GameObject MyteleportationMainPlacement;

    [SerializeField] private AudioSource MRAudio;
    //***********

    /// <summary>
    /// The <see cref="Transform"/> that represents the teleportation destination.
    /// </summary>
    public Transform teleportAnchorTransform
    {
        get => m_TeleportAnchorTransform;
        set => m_TeleportAnchorTransform = value;
    }

    /// <summary>
    /// See <see cref="MonoBehaviour"/>.
    /// </summary>
    protected void OnValidate()
    {
        if (m_TeleportAnchorTransform == null)
            m_TeleportAnchorTransform = transform;
    }

    /// <inheritdoc />
    protected override void Reset()
    {
        base.Reset();
        m_TeleportAnchorTransform = transform;
    }

    /// <summary>
    /// Unity calls this when drawing gizmos.
    /// </summary>
    protected void OnDrawGizmos()
    {
        if (m_TeleportAnchorTransform == null)
            return;

        Gizmos.color = Color.blue;
        GizmoHelpers.DrawWireCubeOriented(m_TeleportAnchorTransform.position, m_TeleportAnchorTransform.rotation, 1f);

        GizmoHelpers.DrawAxisArrows(m_TeleportAnchorTransform, 1f);
    }

    /// <inheritdoc />
    protected override bool GenerateTeleportRequest(IXRInteractor interactor, RaycastHit raycastHit, ref TeleportRequest teleportRequest)
    {
        if (m_TeleportAnchorTransform == null)
            return false;


        if (Under_MRI)
        {
            teleportRequest.destinationPosition = MRI_Pos_Rot.position;
            teleportRequest.destinationRotation = MRI_Pos_Rot.rotation;

            MRAudio.Play();

        }
        else
        {
            teleportRequest.destinationPosition = m_TeleportAnchorTransform.position;
            teleportRequest.destinationRotation = m_TeleportAnchorTransform.rotation;

        }


        MyteleportationMainPlacement.gameObject.SetActive(false);
        return true;
    }
}
  


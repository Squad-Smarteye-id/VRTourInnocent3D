using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Seville
{
    public class SESocketInteractor : XRSocketInteractor
    {
        [Header("Framework Settings")]
        public Transform parentArea;

        [Tooltip("make sure you have set objName on XRGrabIntractableTwoAttach")]
        public List<string> targetObjNames = new List<string>();
        private MeshRenderer mesh;

        protected override void Awake()
        {
            base.Awake();
            mesh = GetComponent<MeshRenderer>();
        }

        [System.Obsolete]
        protected override void OnSelectEntered(XRBaseInteractable interactable)
        {
            base.OnSelectEntered(interactable);

            interactable.transform.SetParent(parentArea);
            ToggleMesh(false);

            var obj = interactable.GetComponent<XRGrabInteractableTwoAttach>();
            obj.retainTransformParent = false;
        }

        [System.Obsolete]
        protected override void OnSelectExited(XRBaseInteractable interactable)
        {
            base.OnSelectExited(interactable);

            ToggleMesh(true);
            var obj = interactable.GetComponent<XRGrabInteractableTwoAttach>();
            obj.retainTransformParent = true;
        }

        [System.Obsolete]
        protected override void OnHoverEntered(XRBaseInteractable interactable)
        {
            base.OnHoverEntered(interactable);

            ToggleMesh(false);
        }

        [System.Obsolete]
        protected override void OnHoverExited(XRBaseInteractable interactable)
        {
            base.OnHoverEntered(interactable);

            ToggleMesh(true);
        }

        [System.Obsolete]
        public override bool CanHover(XRBaseInteractable interactable)
        {
            return base.CanHover(interactable) && MatchUsingTag(interactable);
        }

        [System.Obsolete]
        public override bool CanSelect(XRBaseInteractable interactable)
        {
            return base.CanSelect(interactable) && MatchUsingTag(interactable);
        }

        private bool MatchUsingTag(XRBaseInteractable interactable)
        {
            var obj = interactable.GetComponent<XRGrabInteractableTwoAttach>();

            return targetObjNames.Contains(obj.objName);
            // return interactable.CompareTag(targetTag);
        }

        private void ToggleMesh(bool state)
        {
            mesh.enabled = state;
        }
    }
}
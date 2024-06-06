using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VRInnocent.Content
{
    public class SuccessPopup : MonoBehaviour
    {
        public CanvasGroup parentCanvasGroup;
        public GameObject childObject;

        [Space]
        public UnityEvent OnSuccess;

        void OnEnable()
        {
            childObject.transform.localPosition = new Vector3(childObject.transform.localPosition.x, 100, childObject.transform.localPosition.z);
            LeanTween.alphaCanvas(parentCanvasGroup, 1, 0.5f)
                .setOnComplete(StartMoveAnimation);
        }

        void StartMoveAnimation()
        {
            LeanTween.moveLocalY(childObject, 30, 1f)
                .setEase(LeanTweenType.easeOutQuad);

            Invoke("AnimationComplete", 3f);
        }

        void AnimationComplete()
        {
            OnSuccess?.Invoke();
            parentCanvasGroup.alpha = 0f;
            this.gameObject.SetActive(false);
        }
    }
}
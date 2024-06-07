using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace VRInnocent.Content
{
    public class SuccessPopup : MonoBehaviour
    {
        public List<PopupContent> popupContentList;

        [Header("Component UI")]
        public CanvasGroup parentCanvasGroup;
        public GameObject childObject;
        public Image iconImg;
        public TextMeshProUGUI messageText;

        [Space]
        public UnityEvent OnSuccess;

        [System.Serializable]
        public struct PopupContent
        {
            public string popupName;
            public Sprite iconPopup;
            public string message;
        }

        public void ShowPopupAnimate(string popupName)
        {
            var contenTemp = popupContentList.Find(x => x.popupName == popupName);
            iconImg.sprite = contenTemp.iconPopup;
            messageText.text = contenTemp.message;

            childObject.transform.localPosition = new Vector3(childObject.transform.localPosition.x, 100, childObject.transform.localPosition.z);
            LeanTween.alphaCanvas(parentCanvasGroup, 1, 0.5f).setDelay(.5f)
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
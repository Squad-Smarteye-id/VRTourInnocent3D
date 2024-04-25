using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Seville
{
    public class PopUpController : MonoBehaviour
    {
        public RectTransform panelPopup;
        public TextMeshProUGUI UIText;
        public Image UIImage;

        [Space]
        [Header("Content Pop-Up")]
        public Sprite contentImage;
        [TextArea]
        public string contentText;

        private Vector2 showPosition = Vector2.zero; // Posisi tampil relatif terhadap parent canvas
        private Vector2 hidePosition = new Vector2(0, -1080); // Posisi tidak tampil relatif terhadap parent canvas

        public void OnClickOpenPopup()
        {
            panelPopup.gameObject.SetActive(true);

            if (!contentImage) UIImage.transform.parent.transform.gameObject.SetActive(false);
            else UIImage.sprite = contentImage;

            UIText.text = contentText;

            LeanTween.value(panelPopup.gameObject, callOnUpdate: (val) => panelPopup.anchoredPosition = val,
                            from: hidePosition, to: showPosition, time: 1f)
                      .setEase(LeanTweenType.easeInOutSine);
        }

        public void OnClickClosePopup()
        {
            LeanTween.value(panelPopup.gameObject, callOnUpdate: (val) => panelPopup.anchoredPosition = val,
                            from: showPosition, to: hidePosition, time: 1f)
                      .setEase(LeanTweenType.easeInOutSine)
                      .setOnComplete(() =>
                      {
                          panelPopup.gameObject.SetActive(true);
                      });
        }
    }
}
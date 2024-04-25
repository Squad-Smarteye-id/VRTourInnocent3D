using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Tproject.Quest
{
    public class QuestItem : MonoBehaviour
    {
        public Image iconImage;
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI descriptionText;
        public TextMeshProUGUI state;

        [Tooltip("change image when task is done")]
        public bool changingImage;
        public Image frameImage;
        public Color32 doneColor;

        public void SetValueItem(Sprite _icon, string _title, string _desc, bool _state)
        {
            iconImage.sprite = _icon;
            titleText.text = _title;
            descriptionText.text = _desc;
            if (!_state)
            {
                state.text = "TODO";
            }
            else
            {
                frameImage.color = doneColor;
                state.text = "DONE";
            }
        }
    }
}
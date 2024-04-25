using UnityEngine;
using TMPro;
using System;

namespace Seville
{
    public class BtnOption : MonoBehaviour
    {
        public TextMeshProUGUI UI_text;
        public bool validation;
        public Action<bool> sendAnswer;

        public void OnClickAnswerQuestion()
        {
            sendAnswer?.Invoke(validation);

            // Debug.Log($"player click btn: {validation}");
        }
    }
}
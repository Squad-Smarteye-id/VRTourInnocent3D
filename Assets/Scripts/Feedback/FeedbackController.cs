using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using VRInnocent.Utils;

namespace VRInnocent.Content
{
    public class FeedbackController : MonoBehaviour
    {
        private Color defaultColor = Color.white; // Warna default (FFFFFF)
        public Color positiveColor;
        public Color negativeColor;

        [Space]
        [Header("UI Components")]
        public GameObject panelContainer;
        private CanvasGroup canvasGroup;
        public Button[] buttonPositive;
        public Button[] buttonNegative;

        public List<int> interplayScore = new List<int>();

        void Start()
        {
            for (int i = 0; i < buttonPositive.Length; i++)
            {
                interplayScore.Add(-1);
            }

            canvasGroup = panelContainer.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = panelContainer.AddComponent<CanvasGroup>();
            }

            panelContainer.transform.localPosition = new Vector3(-1084, 0, 0);
            canvasGroup.alpha = 0;
        }

        public void ResetAllButtonColors()
        {
            for (int i = 0; i < buttonPositive.Length; i++)
            {
                interplayScore[i] = -1;
                ResetBtnSelectionColors(i);
            }
        }

        void ResetBtnSelectionColors(int questionIndex)
        {
            buttonPositive[questionIndex].GetComponent<Image>().color = defaultColor;
            buttonNegative[questionIndex].GetComponent<Image>().color = defaultColor;
        }

        public void SetPositiveFeedback(int questionIndex)
        {
            if (questionIndex < interplayScore.Count)
            {
                ResetBtnSelectionColors(questionIndex);
                buttonPositive[questionIndex].GetComponent<Image>().color = positiveColor;
                interplayScore[questionIndex] = 0;
            }
        }

        public void SetNegativeFeedback(int questionIndex)
        {
            if (questionIndex < interplayScore.Count)
            {
                ResetBtnSelectionColors(questionIndex);
                buttonNegative[questionIndex].GetComponent<Image>().color = negativeColor;
                interplayScore[questionIndex] = 1;
            }
        }

        public void ShowPanel() =>
            UIAnimator.SlideHorizontalWithFade(panelContainer, canvasGroup, 0, 1);

        public void HidePanel() =>
            UIAnimator.SlideHorizontalWithFade(panelContainer, canvasGroup, -1084, 0);

        public void SendFeedback()
        {
            if (ValidateScores())
            {
                string json = JsonConvert.SerializeObject(interplayScore, Formatting.Indented);
                Debug.Log("JSON: " + json);

                ResetAllButtonColors();
            }
            else
            {
                Debug.Log($"terdapat nilai yang masih kosong");
            }
        }

        private bool ValidateScores()
        {
            foreach (int score in interplayScore)
            {
                if (score == -1)
                {
                    return false; // Mengembalikan false jika ada skor yang -1
                }
            }
            return true; // Mengembalikan true jika semua skor sudah terisi
        }
    }
}
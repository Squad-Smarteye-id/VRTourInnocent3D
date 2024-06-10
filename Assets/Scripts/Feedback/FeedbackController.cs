using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRInnocent.Utils;
using VRInnocent.RestAPI;
using Newtonsoft.Json.Linq;
using UnityEngine.Events;
using Unity.VisualScripting;

namespace VRInnocent.Content
{
    public class FeedbackController : RestAPIHandler
    {
        public string interplayName;
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

        [Space]
        [Header("Event")]
        [Space]
        public UnityEvent OnFeedbackSent;

        void Start()
        {
            if (string.IsNullOrEmpty(interplayName)) Debug.LogWarning("Interplay name is null");

            for (int i = 0; i < buttonPositive.Length; i++)
            {
                interplayScore.Add(0);
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
                interplayScore[i] = 0;
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
                interplayScore[questionIndex] = 2; // number 2 for positive feedback
            }
        }

        public void SetNegativeFeedback(int questionIndex)
        {
            if (questionIndex < interplayScore.Count)
            {
                ResetBtnSelectionColors(questionIndex);
                buttonNegative[questionIndex].GetComponent<Image>().color = negativeColor;
                interplayScore[questionIndex] = 1; // number 2 for negative feedback
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
                Dictionary<string, string> jData = new Dictionary<string, string>
                {
                    {"userId", PlayerManager.Instance.userId},
                    {"for", interplayName.ToString()},
                    {"ratingQ1", interplayScore[0].ToString()},
                    {"ratingQ2", interplayScore[1].ToString()},
                    {"ratingQ3", interplayScore[2].ToString()},
                    {"ratingQ4", interplayScore[3].ToString()},
                    {"ratingQ5", interplayScore[4].ToString()}
                };

                RowData dataObject = new RowData(jData);
                restAPI.PostAction(dataObject.baseData, OnSuccessResult, OnProtocolErr, DataProcessingErr, "feedback");

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
                if (score == 0)
                {
                    return false; // Mengembalikan false jika ada skor yang 0
                }
            }
            return true; // Mengembalikan true jika semua skor sudah terisi
        }

        public override void OnSuccessResult(JObject result)
        {
            OnFeedbackSent?.Invoke();
        }

        public override void OnProtocolErr(JObject result)
        {

        }

        public override void DataProcessingErr(JObject result)
        {

        }
    }
}
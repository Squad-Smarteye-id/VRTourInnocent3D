using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

namespace Seville
{
    public class QuizController : MonoBehaviour
    {
        public List<MQuizContent> contensStaging;
        [System.Serializable]
        public struct MQuizContent
        {
            public string question;
            public Sprite illustrationSprite;
            public List<optionSection> optionList;

            [System.Serializable]
            public struct optionSection
            {
                public string answerText;
                public bool trueBox;
            }
        }

        [Header("Score UI")]
        public GameObject panelScore;
        public TextMeshProUGUI trueResultText;
        public TextMeshProUGUI totalScoreText;
        private int scoreTrue;
        private int totalScore;

        [Header("Validation UI")]
        public GameObject validationPanel;
        public Image imgValidationProgress;

        [Header("Quiz UI")]
        public GameObject quizContainer;
        public TextMeshProUGUI questionUI;
        public Image ilustrationImage;
        [Space]
        public List<GameObject> optionBtn;

        int _currentIndex = 0;

        // Handler action
        Action quizFinishToHandler;

        public bool StartQuiz(List<MQuizContent> quizConten, Action act)
        {
            if (_currentIndex == quizConten.Count)
            {
                Debug.Log($"Player has been answerd this question section");
                return false;
            }

            quizFinishToHandler = act;
            foreach (var item in quizConten)
            {
                contensStaging.Add(item);
            }

            quizContainer.SetActive(true);
            SetUpQuestionAndAnswers(_currentIndex);

            return true;
        }

        private void SetUpQuestionAndAnswers(int questionIndex)
        {
            questionUI.text = contensStaging[questionIndex].question;

            if (contensStaging[questionIndex].illustrationSprite != null)
            {
                ilustrationImage.gameObject.SetActive(true);
                ilustrationImage.sprite = contensStaging[questionIndex].illustrationSprite;
            }
            else ilustrationImage.gameObject.SetActive(false);

            var shuffledOptions = contensStaging[questionIndex].optionList.ToList();
            Shuffle(shuffledOptions);

            for (int i = 0; i < optionBtn.Count; i++)
            {
                if (i < shuffledOptions.Count)
                {
                    optionBtn[i].SetActive(true);

                    var btnTarget = optionBtn[i].GetComponent<BtnOption>();
                    btnTarget.UI_text.text = shuffledOptions[i].answerText;
                    btnTarget.validation = shuffledOptions[i].trueBox;
                    btnTarget.sendAnswer = Answer;
                }
                else
                {
                    optionBtn[i].SetActive(false);
                }
            }
        }

        private void Shuffle<T>(List<T> list)
        {
            int n = list.Count;
            System.Random rnd = new System.Random();
            while (n > 1)
            {
                int k = rnd.Next(n--);
                T temp = list[n];
                list[n] = list[k];
                list[k] = temp;
            }
        }

        public void Answer(bool answerValidation)
        {
            StartCoroutine(nameof(CheckingAnswer), answerValidation);
        }

        IEnumerator CheckingAnswer(bool answerValidation)
        {
            validationPanel.SetActive(true);

            bool isChecking = false;

            float i = 0;
            while (i < 1f)
            {
                i += Mathf.Clamp01(1f * Time.deltaTime);
                imgValidationProgress.fillAmount = i;

                if (!isChecking)
                {
                    if (answerValidation)
                    {
                        scoreTrue++;
                        totalScore++;
                        // Debug.Log($"the answer is: true | current score {scoreTrue} / {totalScore}");
                    }
                    else
                    {
                        totalScore++;
                        // Debug.Log($"the answer is: false | current score {scoreTrue} / {totalScore}");
                    }

                    isChecking = true;
                }

                yield return null;
            }

            validationPanel.SetActive(false);

            if (_currentIndex < contensStaging.Count - 1)
            {
                _currentIndex++;
                SetUpQuestionAndAnswers(_currentIndex);
            }
            else
            {
                quizContainer.SetActive(false);
                contensStaging.Clear();


                StartCoroutine(nameof(ShowScore));
            }
        }

        IEnumerator ShowScore()
        {
            panelScore.SetActive(true);
            trueResultText.text = scoreTrue.ToString();
            totalScoreText.text = totalScore.ToString();

            yield return new WaitForSeconds(3f);

            quizFinishToHandler?.Invoke();
            panelScore.SetActive(false);
        }

        public void ResetQuiz()
        {
            _currentIndex = 0;
            scoreTrue = 0;
            totalScore = 0;
        }
    }
}
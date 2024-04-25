using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Seville
{
    public class QuizHandler : MonoBehaviour
    {
        public QuizController quizController;
        public List<QuizController.MQuizContent> quizContentList;

        [SerializeField] bool playOnStart = false;
        public UnityEvent AfterDialogFinish;
        private bool isPlaying = false;

        private void Start()
        {
            if (playOnStart) ShowQuiz();
        }

        public void ShowQuiz()
        {
            if (isPlaying) return;

            bool checkquestion = quizController.StartQuiz(quizContentList, QuizIsDone);

            if (!checkquestion) AfterDialogFinish?.Invoke();

            isPlaying = true;
        }

        public void QuizIsDone()
        {
            AfterDialogFinish?.Invoke();

            isPlaying = false;
        }
    }
}
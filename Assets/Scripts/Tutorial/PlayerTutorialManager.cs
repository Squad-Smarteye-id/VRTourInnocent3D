using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem.iOS;

namespace ScriptTutorials
{
    [Serializable]
    public class MyTutorial
    {
        public GameObject TutorialCanvas;
        public bool tutorialStatus;
        public int relatedTestIndex; // Indeks aksi pengujian terkait
        public Image checkbox;
        public Button NextButton;
    }

    public class PlayerTutorialManager : MonoBehaviour
    {
        [SerializeField] private List<MyTutorial> tutorialList = new List<MyTutorial>();
        [SerializeField] private float tutorialSwitchDelay;

        private int currentTutorialIndex = 0;

        public Sprite chekedChekbox;
        public Sprite buttonEnable;

        public UnityEvent Onfinish;
        //public UnityEvent onRestart;

        private void Awake()
        {
            // Inisialisasi dictionary aksi pengujian
            tutorialTests.Add(0, false); // HeadTest
            tutorialTests.Add(1, false); // WalkTest
            tutorialTests.Add(2, false); // SelectTest
            tutorialTests.Add(3, false); // CursorTest

        }

        private void Start()
        {
            
        }

        public void SetupCurrentTutorial()
        {
            // Memeriksa apakah indeks tutorial saat ini valid
            if (currentTutorialIndex >= 0 && currentTutorialIndex < tutorialList.Count)
            {
                // Menonaktifkan semua tutorial
                foreach (MyTutorial tutorial in tutorialList)
                {
                    tutorial.TutorialCanvas.SetActive(false);
                }

                // Mengaktifkan tutorial yang tepat
                tutorialList[currentTutorialIndex].TutorialCanvas.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Indeks tutorial saat ini tidak valid.");
            }
        }


        public void CompleteCurrentTutorial()
        {
            if (currentTutorialIndex >= 0 && currentTutorialIndex < tutorialList.Count)
            {
                MyTutorial currentTutorial = tutorialList[currentTutorialIndex];

                if (!currentTutorial.tutorialStatus) // Pastikan tutorial belum selesai
                {
                    // Menandai tutorial sebagai selesai
                    currentTutorial.tutorialStatus = true;
                    currentTutorial.checkbox.sprite = chekedChekbox;
                    currentTutorial.NextButton.image.sprite = buttonEnable;
                    currentTutorial.NextButton.enabled = true;
                    Debug.Log("Tutorial ke-" + (currentTutorialIndex + 1) + " selesai.");

                }
            }
            else
            {
                Debug.LogWarning("Indeks tutorial saat ini tidak valid.");
            }
        }

        public void NextTutorial()
        {
            StartCoroutine(SwitchToNextTutorial());
        }

        private IEnumerator SwitchToNextTutorial()
        {
            yield return new WaitForSeconds(tutorialSwitchDelay); // Menunggu selama waktu yang ditentukan

            MyTutorial currentTutorial = tutorialList[currentTutorialIndex];

            currentTutorialIndex++;
            if (currentTutorialIndex < tutorialList.Count)
            {
                SetupCurrentTutorial(); // Menyiapkan tutorial berikutnya
            }
            else
            {
                currentTutorial.TutorialCanvas.SetActive(false);
                Onfinish.Invoke();
            }
        }

        //public void RestartGame()
        //{
        //    // Mengatur ulang status setiap tutorial
        //    foreach (MyTutorial tutorial in tutorialList)
        //    {
        //        tutorial.tutorialStatus = false;
        //        tutorial.checkbox.sprite = null; // Atur sprite checkbox kembali ke default atau kosong
        //        tutorial.NextButton.image.sprite = null; // Atur sprite tombol kembali ke default atau kosong
        //        /*tutorial.NextButton.enabled = false;*/ // Nonaktifkan tombol
        //    }

        //    // Atur ulang indeks tutorial saat ini ke yang pertama
        //    currentTutorialIndex = 0;
        //    SetupCurrentTutorial();

        //    onRestart.Invoke();
        //    Debug.Log("Permainan diulang.");
        //}

        // Mendapatkan indeks tutorial saat ini
        public int GetCurrentTutorialIndex()
        {
            return currentTutorialIndex;
        }

        // Mendapatkan jumlah tutorial dalam daftar
        public int GetTutorialListCount()
        {
            return tutorialList.Count;
        }

        // Daftar aksi pengujian yang sesuai dengan indeks tutorial
        private Dictionary<int, bool> tutorialTests = new Dictionary<int, bool>();

        

    }
}

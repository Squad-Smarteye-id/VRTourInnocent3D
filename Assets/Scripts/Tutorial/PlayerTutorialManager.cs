using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace ScriptTutorials
{
    [Serializable]
    public class MyTutorial
    {
        public GameObject TutorialCanvas;
        public bool tutorialStatus;
        public int relatedTestIndex; // Indeks aksi pengujian terkait
        public Image checkbox;
    }

    public class PlayerTutorialManager : MonoBehaviour
    {
        [SerializeField] private List<MyTutorial> tutorialList = new List<MyTutorial>();
        [SerializeField] private float tutorialSwitchDelay;
        private int currentTutorialIndex = 0;
        public Sprite chekedChekbox;

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

            currentTutorialIndex++;
            if (currentTutorialIndex < tutorialList.Count)
            {
                SetupCurrentTutorial(); // Menyiapkan tutorial berikutnya
            }
            else
            {
                Debug.Log("Semua tutorial selesai.");
                // Tambahkan logika untuk menyelesaikan urutan tutorial
            }
        }

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

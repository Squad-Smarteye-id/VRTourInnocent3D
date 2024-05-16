using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ScriptTutorials
{
    [Serializable]
    public class MyTutorial
    {
        public Sprite tutorialPicture;
        public bool tutorialStatus;
        public int relatedTestIndex; // Indeks aksi pengujian terkait
    }

    public class PlayerTutorialManager : MonoBehaviour
    {
        [SerializeField] private List<MyTutorial> tutorialList = new List<MyTutorial>();
        [SerializeField] private Image tutorialImage;
        [SerializeField] private float tutorialSwitchDelay = 3f; // Jeda (delay) sebelum beralih ke tutorial berikutnya

        private int currentTutorialIndex = 0;

        private void Start()
        {
            // Memulai tutorial pertama saat game dimulai
            SetupCurrentTutorial();
        }

        private void SetupCurrentTutorial()
        {
            // Memeriksa apakah indeks tutorial saat ini valid
            if (currentTutorialIndex >= 0 && currentTutorialIndex < tutorialList.Count)
            {
                MyTutorial currentTutorial = tutorialList[currentTutorialIndex];

                // Menampilkan gambar tutorial pada Image UI
                if (tutorialImage != null && currentTutorial.tutorialPicture != null)
                {
                    tutorialImage.sprite = currentTutorial.tutorialPicture;
                }
                else
                {
                    Debug.LogWarning("Image tutorial atau gambar tutorial tidak diatur dengan benar.");
                }

                // Memperbarui aksi pengujian yang terkait dengan tutorial saat ini
                int relatedTestIndex = currentTutorial.relatedTestIndex;
                if (relatedTestIndex >= 0 && relatedTestIndex < tutorialTests.Count)
                {
                    tutorialTests[currentTutorial.relatedTestIndex] = currentTutorial.tutorialStatus;
                }
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
                    Debug.Log("Tutorial ke-" + (currentTutorialIndex + 1) + " selesai.");

                    // Menjalankan coroutine untuk beralih ke tutorial berikutnya setelah jeda
                    StartCoroutine(SwitchToNextTutorialWithDelay());
                }
            }
            else
            {
                Debug.LogWarning("Indeks tutorial saat ini tidak valid.");
            }
        }

        private IEnumerator SwitchToNextTutorialWithDelay()
        {
            // Menunggu selama beberapa detik sebelum melanjutkan ke tutorial berikutnya
            yield return new WaitForSeconds(tutorialSwitchDelay);

            // Lanjut ke tutorial berikutnya jika masih ada
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

        private void Awake()
        {
            // Inisialisasi dictionary aksi pengujian
            tutorialTests.Add(0, false); // HeadTest
            tutorialTests.Add(1, false); // WalkTest
            tutorialTests.Add(2, false); // SelectTest
        }
    }
}

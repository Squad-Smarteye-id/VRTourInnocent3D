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
    }

    public class PlayerTutorialManager : MonoBehaviour
    {
        [SerializeField] private List<MyTutorial> tutorialList = new List<MyTutorial>();
        [SerializeField] private Image _tutorialImage;
        [SerializeField] private float tutorialSwitchDelay; // Jeda (delay) sebelum beralih ke tutorial berikutnya

        private int currentTutorialIndex = 0;

        private void Start()
        {
            // Memulai tutorial pertama saat game dimulai
            SetupCurrentTutorial();
        }

        private void Update()
        {

        }

        private void SetupCurrentTutorial()
        {
            // Memeriksa apakah indeks tutorial saat ini valid
            if (currentTutorialIndex >= 0 && currentTutorialIndex < tutorialList.Count)
            {
                MyTutorial currentTutorial = tutorialList[currentTutorialIndex];

                // Menampilkan gambar tutorial pada Image UI
                if (_tutorialImage != null && currentTutorial.tutorialPicture != null)
                {
                    _tutorialImage.sprite = currentTutorial.tutorialPicture;
                }
                else
                {
                    Debug.LogWarning("Image tutorial atau gambar tutorial tidak diatur dengan benar.");
                }
            }
            else
            {
                Debug.LogWarning("Indeks tutorial saat ini tidak valid.");
            }
        }

        public void CompleteCurrentTutorial()
        {
            // Menandai tutorial saat ini sebagai selesai
            Debug.Log("Tutorial ke-" + (currentTutorialIndex + 1) + " selesai.");

            // Menjalankan coroutine untuk menunggu sebelum beralih ke tutorial berikutnya
            StartCoroutine(SwitchToNextTutorialWithDelay());
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


    }
}

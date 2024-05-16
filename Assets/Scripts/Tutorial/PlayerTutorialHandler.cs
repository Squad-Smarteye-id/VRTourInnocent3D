using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

namespace ScriptTutorials
{
    public class PlayerTutorialHandler : MonoBehaviour
    {
        private InputData inputData;
        private PlayerTutorialManager tutorialManager;

        private bool tutorialStarted;

        // Daftar aksi pengujian yang sesuai dengan indeks tutorial
        private Dictionary<int, System.Action> tutorialTests = new Dictionary<int, System.Action>();

        private void Start()
        {
            inputData = GetComponent<InputData>();
            tutorialManager = GetComponent<PlayerTutorialManager>();

            tutorialStarted = false;
            // Menambahkan aksi pengujian ke dalam dictionary
            tutorialTests.Add(0, HeadTest);
            tutorialTests.Add(1, WalkTest);
            tutorialTests.Add(2, SelectTest);
        }

        private void Update()
        {
            // Lakukan pengujian secara berurutan di sini
            if (tutorialStarted) { 
            
            RunSequentialTests();
            
            }
        }

        public void TutorialStart()
        {
            tutorialStarted = true;
        }

        private void RunSequentialTests()
        {
            // Jika tutorial manager atau input data tidak diatur, keluar dari metode ini
            if (tutorialManager == null || inputData == null)
                return;

            int currentTutorialIndex = tutorialManager.GetCurrentTutorialIndex();

            // Memeriksa apakah indeks tutorial saat ini valid dan terdapat aksi pengujian yang terkait
            if (tutorialTests.ContainsKey(currentTutorialIndex))
            {
                // Menjalankan aksi pengujian yang sesuai dengan indeks tutorial saat ini
                tutorialTests[currentTutorialIndex].Invoke();
            }
            else
            {
                Debug.LogWarning("Tidak ada aksi pengujian yang terkait dengan indeks tutorial saat ini.");

                // Jika tutorialList kosong dan indeks saat ini adalah 0, jalankan HeadTest
                if (currentTutorialIndex == 0 && tutorialManager.GetTutorialListCount() == 0)
                {
                    Debug.LogWarning("Menggunakan HeadTest karena tutorialList kosong.");
                    HeadTest();
                }
            }
        }

        // Metode pengujian untuk tutorial kepala
        private void HeadTest()
        {
            if (inputData._HMD.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion headMountedValue))
            {
                Vector3 eulerRotation = headMountedValue.eulerAngles;

                lookArround();

                // Debug.Log("headMountedValue: " + headMountedValue);
            }

            // liat arah
            void lookArround()
            {
                float yRotation = headMountedValue.eulerAngles.y;
                if (yRotation >= 50.0f && yRotation <= 90.0f || yRotation >= 270.0f && yRotation <= 310.0f)
                {
                    Debug.Log("head berhasil : " + yRotation);
                    tutorialManager.CompleteCurrentTutorial();

                }
            }
        }

        // Metode pengujian untuk tutorial berjalan
        private void WalkTest()
        {
            Vector2 joystickInput = inputData.GetLeftThumbstickInput();

            // Lakukan pengujian gerakan maju atau mundur
            if (joystickInput.y > 0.5f || joystickInput.y < -0.5f)
            {
                Debug.Log("Pengujian gerakan berjalan berhasil: " + joystickInput.y);
                tutorialManager.CompleteCurrentTutorial();
            }
        }

        // Metode pengujian untuk tutorial pemilihan
        private void SelectTest()
        {
            float leftTriggerValue;
            float rightTriggerValue;

            // Periksa input tombol kiri
            if (inputData._leftController.TryGetFeatureValue(CommonUsages.trigger, out leftTriggerValue))
            {
                if (leftTriggerValue >= 1)
                {
                    Debug.Log("Tombol kiri diaktifkan");
                    tutorialManager.CompleteCurrentTutorial();
                }
            }

            // Periksa input tombol kanan
            if (inputData._rightController.TryGetFeatureValue(CommonUsages.trigger, out rightTriggerValue))
            {
                if (rightTriggerValue >= 1)
                {
                    Debug.Log("Tombol kanan diaktifkan");
                    tutorialManager.CompleteCurrentTutorial();
                }
            }
        }
    }
}

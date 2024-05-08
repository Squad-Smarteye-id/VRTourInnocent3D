using UnityEngine;
using UnityEngine.XR;
using System.Collections;
using System.Collections.Generic;

namespace ScriptTutorials
{
    public class PlayerTutorialHandler : MonoBehaviour
    {
        private InputData InputData;
        private PlayerTutorialManager playerTutorialManager;

        private float lefttriggerValue;
        private float righttriggerValue;

        private void Start()
        {
            InputData = GetComponent<InputData>();
            playerTutorialManager = GetComponent<PlayerTutorialManager>();
        }

        #region test method



        //test nengok
        public void HeadTest()
        {
            // tutor kepala
            if (InputData._HMD.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion headMountedValue))
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
                    playerTutorialManager.CompleteCurrentTutorial();

                }
            }
        }



        //test jalan
        public void WalkTest()
        {
            Vector2 joystickInput = InputData.GetLeftThumbstickInput(); // Menggunakan fungsi GetLeftThumbstickInput dari InputData

            // Cek input untuk gerakan maju atau mundur
            if (joystickInput.y > 0.5f || joystickInput.y < -0.5f)
            {
                Debug.Log("Player sedang berjalan: " + joystickInput.y);

                // Tandai tutorial berjalan sebagai selesai
                playerTutorialManager.CompleteCurrentTutorial();
            }
        }



        //test select tombol.
        public void SelectTest()
        {
            // Check left trigger
            if (InputData._leftController.TryGetFeatureValue(CommonUsages.trigger, out lefttriggerValue))
            {
                if (lefttriggerValue >= 1)
                {
                    Debug.Log("Left trigger activated");
                    playerTutorialManager.CompleteCurrentTutorial();
                }
            }

            // Check right trigger
            if (InputData._rightController.TryGetFeatureValue(CommonUsages.trigger, out righttriggerValue))
            {
                if (righttriggerValue >= 1)
                {
                    Debug.Log("Right trigger activated");
                    playerTutorialManager.CompleteCurrentTutorial();
                }
            }

        }

        #endregion


        private void Update()
        {
            SelectTest();
        }


    }
}


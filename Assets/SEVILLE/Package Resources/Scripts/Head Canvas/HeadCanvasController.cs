
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;
using TMPro;
using Tproject.Quest;

namespace Seville
{
    public class HeadCanvasController : MonoBehaviour
    {
        public Transform playerHead;
        public float spawnDistance = 2;
        public float maxDistance = 5f;

        [Header("Quest Canvas Components")]
        public bool useQuestCanvas;
        public DataManager dataManager;
        public QuestController questController;
        public ScoreController scoreController;
        public InputActionProperty secondaryBtnAction;
        float distanceBetweenObjects;

        [Header("Menu Canvas Components")]
        public bool useMenuCanvas;
        public GameObject MenuCanvas;
        public InputActionProperty primaryBtnAction;

        [Header("Notification Canvas Components")]
        public GameObject UI_popupPanel;
        public TextMeshProUGUI UI_message;
        public float showNotificationTime = 5f;
        bool popupState = false;

        private void Update()
        {
            if (useQuestCanvas)
                CheckingQuestCanvas();

            if (useMenuCanvas) CheckingMenuCanvas();

            if (popupState)
                InfoCanvasPos();
        }

        private void CheckingMenuCanvas()
        {
            if (!MenuCanvas) return;

            if (primaryBtnAction.action.WasPressedThisFrame())
            {
                // Debug.Log($"is {secondaryBtnAction.action.name} klick");
                MenuCanvas.SetActive(!MenuCanvas.activeSelf);

                MenuCanvas.transform.position = playerHead.position + new Vector3(playerHead.forward.x, 0, playerHead.forward.z).normalized * spawnDistance;
            }

            HandleCanvasLook(MenuCanvas, playerHead, maxDistance);
        }

        private void CheckingQuestCanvas()
        {
            if (!questController) return;

            if (secondaryBtnAction.action.WasPressedThisFrame())
            {
                // Debug.Log($"is {secondaryBtnAction.action.name} klick");

                if (questController.questCanvas.activeSelf == false)
                    questController.PrintItems();
                else questController.CloseQuestCanvas();

                questController.questCanvas.transform.position = playerHead.position + new Vector3(playerHead.forward.x, 0, playerHead.forward.z).normalized * spawnDistance;
            }

            HandleCanvasLook(questController.questCanvas, playerHead, maxDistance);
        }

        public void HandleCanvasLook(GameObject canvasTarget, Transform playerHead, float maxDistance)
        {
            if (canvasTarget.activeSelf == true)
            {
                canvasTarget.transform.LookAt(new Vector3(playerHead.position.x, canvasTarget.transform.position.y, playerHead.position.z));
                canvasTarget.transform.forward *= -1;
            }

            float distanceBetweenObjects = 0f;

            if (playerHead != null)
            {
                distanceBetweenObjects = Vector3.Distance(playerHead.position, canvasTarget.transform.position);

                if (distanceBetweenObjects < maxDistance)
                    Debug.DrawLine(playerHead.position, canvasTarget.transform.position, Color.green);
            }
            else Debug.LogWarning("HeadCanvas has not been assigned");

            if (distanceBetweenObjects > maxDistance && canvasTarget.activeSelf == true)
                canvasTarget.SetActive(false);
        }

        public void ShowNotificationMessage(string msg)
        {
            UI_popupPanel.SetActive(true);
            UI_message.text = msg;

            if (playerHead != null)
                UI_popupPanel.transform.position = playerHead.position + new Vector3(playerHead.forward.x, 0, playerHead.forward.z).normalized * spawnDistance;
            else Debug.LogWarning("HeadCanvas has not been assigned");

            popupState = true;
            Invoke("DisactiveInfoCanvas", showNotificationTime);
        }

        private void InfoCanvasPos()
        {
            UI_popupPanel.transform.LookAt(new Vector3(playerHead.position.x, UI_popupPanel.transform.position.y, playerHead.position.z));
            UI_popupPanel.transform.forward *= -1;
        }

        private void DisactiveInfoCanvas()
        {
            popupState = false;
            UI_popupPanel.SetActive(false);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (playerHead != null && questController)
            {
                GUI.color = Color.black;
                Handles.Label(transform.position - (playerHead.position -
                 questController.questCanvas.transform.position) / 2, distanceBetweenObjects.ToString());
            }
        }
#endif
    }
}
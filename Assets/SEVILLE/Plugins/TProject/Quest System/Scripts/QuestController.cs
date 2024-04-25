using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Seville;

namespace Tproject.Quest
{
    public class QuestController : MonoBehaviour
    {
        public bool isPlayOnStart;
        public DataManager dataManager;
        [HideInInspector] public List<DataManager.QuestItem> toDoList;
        public QuestItem itemPrefabs;
        public Transform itemListParent;
        public GameObject questCanvas;
        public NotificationMessage popupMessage;
        public Transform canvas;
        public HeadCanvasController headCanvas;

        public ScoreController scoreController;

        private void Start()
        {
            this.dataManager = headCanvas.dataManager;

            if (isPlayOnStart)
                PrintItems();
        }

        public void FinishItem(int index)
        {
            var dataTarget = dataManager.GetQuestData();

            if (index > dataTarget.Count - 1)
            {
                Debug.LogWarning($"number {index} is out of todolist count");
                return;
            }

            if (!dataTarget[index].isDone)
            {
                DataManager.QuestItem temp = dataTarget[index];
                temp.isDone = true;

                if (scoreController) scoreController.IncreaseScore(temp.score);
                headCanvas.ShowNotificationMessage(dataTarget[index].doneMessage);

                // DataManager.Instance.UpdateQuizItemDone(temp, index);
                dataManager.UpdateQuizItemDone(temp, index);
            }

            GetTodoList();
            // PrintItems();
        }

        private void GetTodoList()
        {
            toDoList.Clear();

            if (toDoList.Count == dataManager.GetQuestData().Count) return;

            foreach (var item in dataManager.GetQuestData())
            {
                toDoList.Add(item);
            }
        }

        public void PrintItems()
        {
            GetTodoList();

            StartCoroutine(PrintQuestItem());
        }

        IEnumerator PrintQuestItem()
        {
            questCanvas.SetActive(true);

            for (int child = 0; child < itemListParent.childCount; child++)
            {
                Destroy(itemListParent.transform.GetChild(child).gameObject);
            }

            yield return new WaitUntil(() => itemListParent.childCount == 0);

            int i = 0;
            while (i < toDoList.Count + 1)
            {
                if (i == toDoList.Count)
                {
                    // bug can't scroll in awake
                    var rowTemp = Instantiate(itemPrefabs, itemListParent);
                    // rowTemp.textUI.text = " ";
                    StartCoroutine(DestroyTemp(rowTemp.gameObject));
                }
                else
                {
                    var itemGO = Instantiate(itemPrefabs, itemListParent);
                    itemGO.SetValueItem(toDoList[i].iconSprite, toDoList[i].title, toDoList[i].description, toDoList[i].isDone);
                }

                yield return new WaitForSeconds(0.01f);
                i++;
            }
        }

        IEnumerator DestroyTemp(GameObject go)
        {
            yield return new WaitUntil(() => go != null);
            Destroy(go);
            // Debug.Log($"destroy {go}");
        }

        public void ShowMessage(string msg)
        {
            var popup = Instantiate(popupMessage, canvas);
            popup.textMessage.text = msg;
        }

        public void CloseQuestCanvas()
        {
            questCanvas.SetActive(false);
            toDoList.Clear();
        }
    }
}
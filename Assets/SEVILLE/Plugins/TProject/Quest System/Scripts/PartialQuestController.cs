using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Seville;
using UnityEngine.Events;
using System.Linq;

namespace Tproject.Quest
{
    public class PartialQuestController : MonoBehaviour
    {
        public List<ItemFormat> toDoList;

        [System.Serializable]
        public struct ItemFormat
        {
            public Sprite iconSprite;
            public string title;
            [TextArea]
            public string description;
            public bool isDone;
            public string doneMessage;
        }

        public bool isPlayOnStart;
        public QuestItem itemPrefabs;
        public Transform itemListParent;
        public GameObject questCanvas;

        [Tooltip("Insert the 'HeadCanvasController' component found in the Head Canvas game object.")]
        public HeadCanvasController headCanvas;

        [Space]
        public UnityEvent OnQuestFinished;

        private void Start()
        {
            if (isPlayOnStart)
                OpenQuest();
            else CloseQuestCanvas();
        }

        public void OpenQuest()
        {
            if (toDoList.Count == 0)
            {
                Debug.LogWarning($"toDoList.Count is {toDoList.Count}, please fill the list");
                return;
            }

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

        public void FinishItem(int index)
        {
            var dataTarget = toDoList;

            if (index > dataTarget.Count - 1)
            {
                Debug.LogWarning($"number {index} is out of todolist count");
                return;
            }

            if (!dataTarget[index].isDone)
            {
                var temp = dataTarget[index];
                temp.isDone = true;

                if (headCanvas)
                    headCanvas.ShowNotificationMessage(dataTarget[index].doneMessage);
                else Debug.LogWarning($"Canvas notification hasn't been assigned");

                toDoList[index] = temp;
            }

            if (toDoList.All(item => item.isDone))
            {
                OnQuestFinished.Invoke();
            }

            OpenQuest();
        }

        IEnumerator DestroyTemp(GameObject go)
        {
            yield return new WaitUntil(() => go != null);
            Destroy(go);
            // Debug.Log($"destroy {go}");
        }

        public void CloseQuestCanvas()
        {
            questCanvas.SetActive(false);

            var checker = toDoList.Exists((val) => val.isDone == false);

            if (!checker)
            {
                toDoList.Clear();
                Debug.LogWarning($"todo list has been done");
            }
        }
    }
}
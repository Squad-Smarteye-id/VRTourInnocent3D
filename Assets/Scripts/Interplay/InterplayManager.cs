using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Interplay
{
    public class InterplayManager : MonoBehaviour
    {
        [Header("Zone")]
        public ZoneCondition zoneCondition = ZoneCondition.hustler;
        public enum ZoneCondition
        {
            hustler, hacker, hipster
        }

        [Header("Interplay Assets")]
        public List<InterplayHandler> interplayHandlers;

        [Header("Barrier Assets")]
        public GameObject barrierHacker;
        public GameObject barrierHipster;

        [Header("Barrier")]
        public int HackerLimit;
        public int HipsterLimit;

        private void Start()
        {
            SetupScene();

        }

        private void Update()
        {
            BarrierOpen();
        }

        //Set up the initial scene
        private void SetupScene()
        {
            if (zoneCondition == ZoneCondition.hustler)
            {
                barrierHacker.SetActive(true);
                barrierHipster.SetActive(true);
            }
            else
            {
                Debug.Log("Zone not specified.");
            }
        }

        public void BarrierOpen()
        {

            // Check if isVisited has been fulfilled
            List<InterplayHandler> visitedHandlers = interplayHandlers.FindAll(handler => handler.isVisited);

            if (visitedHandlers.Count >= HackerLimit && barrierHacker.activeSelf)
            {
                barrierHacker.SetActive(false);
                Debug.Log("Hacker barrier opened.");
            }

            if (visitedHandlers.Count >= HipsterLimit && barrierHipster.activeSelf)
            {
                barrierHipster.SetActive(false);
                Debug.Log("Hipster barrier opened.");
            }

        }


    }
}

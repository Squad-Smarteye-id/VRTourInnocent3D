using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Seville
{
    public class TriggerArea : MonoBehaviour
    {
        public string targetTag;
        [Space]
        public UnityEvent OnPlayerEnter;
        public UnityEvent OnPlayerStay;
        public UnityEvent OnPlayerExit;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == targetTag)
            {
                OnPlayerEnter?.Invoke();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == targetTag)
            {
                OnPlayerStay?.Invoke();
                // Debug.Log($"Player stay");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == targetTag)
            {
                OnPlayerExit?.Invoke();
            }
        }
    }
}
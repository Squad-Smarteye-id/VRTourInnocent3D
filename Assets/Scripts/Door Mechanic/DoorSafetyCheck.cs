using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSafetyCheck : MonoBehaviour
{
    public DoorAnimation door;

    private void Start()
    {
        door = GetComponentInParent<DoorAnimation>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == door.player && !door.isOpening)
        {
            door.OpenDoors();
        }
    }
}

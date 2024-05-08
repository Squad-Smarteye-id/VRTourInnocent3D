using UnityEngine;
using Tproject;

public class DoorAnimation : MonoBehaviour
{
    public GameObject leftDoor;
    public GameObject rightDoor;

    public float openDistance = 1.0f;
    public float duration = 2.0f;
    public float closeDelay = 1.0f;

    [Space]
    public GameObject player;
    public TargetScanner scanner;

    private Vector3 leftClosedPosition;
    private Vector3 rightClosedPosition;
    [HideInInspector] public bool isOpening = false;
    private bool isMoving = false;

    void Start()
    {
        // Simpan posisi awal pintu
        leftClosedPosition = leftDoor.transform.position;
        rightClosedPosition = rightDoor.transform.position;
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.R)) OpenDoors();
        // if (Input.GetKeyDown(KeyCode.T)) CloseDoors();

        DoorSensor();
    }

    private void DoorSensor()
    {
        GameObject target = scanner.DetectPlayer(transform, player);

        if (target != null)
        {
            float checkPosition = Vector3.Distance(transform.position, target.transform.position);

            if (checkPosition >= scanner.detectionRadius && !isMoving)
            {
                CloseDoors();
            }

            if (checkPosition < scanner.detectionRadius && !isMoving)
            {
                OpenDoors();
            }
        }
        else
        {
            if (!isMoving)
            {
                CloseDoors();
            }
        }
    }

    public void OpenDoors()
    {

        LeanTween.cancel(leftDoor);
        LeanTween.cancel(rightDoor);

        isMoving = true;
        isOpening = true;

        LeanTween.moveX(leftDoor, leftClosedPosition.x - openDistance, duration).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => isMoving = false);
        LeanTween.moveX(rightDoor, rightClosedPosition.x + openDistance, duration).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => isMoving = false);
    }

    public void CloseDoors()
    {
        if (isOpening)
        {
            LeanTween.cancel(leftDoor);
            LeanTween.cancel(rightDoor);

            isOpening = false;
            isMoving = true;
            LeanTween.moveX(leftDoor, leftClosedPosition.x, duration).setDelay(closeDelay).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => isMoving = false);
            LeanTween.moveX(rightDoor, rightClosedPosition.x, duration).setDelay(closeDelay).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => isMoving = false);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        scanner.EditorGizmo(transform);
    }
#endif
}

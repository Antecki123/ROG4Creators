using UnityEngine;

public class QuestPointer : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform pointer;

    private Transform targetTransform;

    private void Update()
    {
        var distance = 4f;
        if (playerTransform != null && Vector3.Distance(playerTransform.position, targetTransform.position) > distance)
        {
            pointer.gameObject.SetActive(true);
            transform.position = playerTransform.position;

            var toPosition = targetTransform;
            var fromPosition = pointer.position;
            fromPosition.z = 0f;

            var direction = (toPosition.position - fromPosition).normalized;
            var angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) % 360;

            transform.localEulerAngles = new Vector3(0f, 0f, angle);
        }

        else
            pointer.gameObject.SetActive(false);
    }

    public void SetTarget(Transform target) => targetTransform = target;
}
using System.Collections;
using Cinemachine;
using UnityEngine;

public class Door : MonoBehaviour, IDoor
{
    [Header("Component References")]
    [SerializeField] private Animator animator;
    [Space]
    [SerializeField] private Door exitDoor;
    [SerializeField] private Location sendToLocation;
    [SerializeField] private CinemachineVirtualCamera activeVirtualCamera;
    [SerializeField] private CinemachineVirtualCamera nextVirtualCamera;

    private bool availableDoor;

    [Header("Animations")]
    private readonly string openDoor = "Open";
    private readonly string closeDoor = "Close";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && exitDoor != null)
        {
            SoundManager.instance.Play("Door");

            animator.SetTrigger(openDoor);
            availableDoor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager.instance.Play("Door");

            animator.SetTrigger(closeDoor);
            availableDoor = false;
        }
    }

    public void MovePlayer(GameObject player) => StartCoroutine(WalkTheDoor(player));

    private IEnumerator WalkTheDoor(GameObject player)
    {
        if (availableDoor && exitDoor != null)
        {
            var fadeDuration = 2f;
            StartCoroutine(UserInterface.instance.FadeScene(fadeDuration));


            yield return new WaitForSeconds(fadeDuration / 2);
            player.transform.position = exitDoor.transform.position;
        }

        activeVirtualCamera.m_Priority = 0;
        nextVirtualCamera.m_Priority = 20;
        QuestManager.instance.actualLocation = sendToLocation;
    }
}
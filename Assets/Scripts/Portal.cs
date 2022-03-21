using System.Collections;
using UnityEngine;

public class Portal : MonoBehaviour, IDoor
{
    [Header("Component References")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject finishFX;
    [Space]
    [SerializeField] private Portal exitPortal;

    private bool availableTeleport;

    [Header("Animations")]
    private readonly string openPortal = "Open";
    private readonly string closePortal = "Close";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetTrigger(openPortal);
            availableTeleport = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetTrigger(closePortal);
            availableTeleport = false;
        }
    }

    public void MovePlayer(GameObject player) => StartCoroutine(TeleportPlayer(player));

    private IEnumerator TeleportPlayer(GameObject player)
    {
        if (availableTeleport && exitPortal != null)
        {
            player.transform.position = exitPortal.transform.position;

            yield return new WaitForSeconds(.1f);
            var effect = Instantiate(finishFX, player.transform.position - Vector3.forward * 2, Quaternion.Euler(new Vector3(-90f, 0, 0)));
            effect.transform.localScale *= 3f;

            SoundManager.instance.Play("Teleport");

            Destroy(effect, 2f);
        }
    }
}
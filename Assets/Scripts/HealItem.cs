using UnityEngine;

public class HealItem : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private GameObject markerFX;
    [SerializeField] private GameObject pickUpFX;
    [Space]
    [SerializeField] private float heal = 50f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerManager>().TakeDamage(-heal);
            SoundManager.instance.Play("Heal");

            GetComponent<SpriteRenderer>().enabled = false;
            markerFX.SetActive(false);
            //pickUpFX.SetActive(true);

            var healFX = Instantiate(pickUpFX, collision.transform.position, Quaternion.identity);
            healFX.SetActive(true);
            healFX.transform.parent = collision.transform;
            Destroy(healFX, 2f);

            Destroy(gameObject, 1f);
        }
    }
}
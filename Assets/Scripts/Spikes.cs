using UnityEngine;

public class Spikes : MonoBehaviour
{ 
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.GetComponent<PlayerManager>().TakeDamage(Time.time * .5f);
    }
}
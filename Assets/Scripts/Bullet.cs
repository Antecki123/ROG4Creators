using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject impactEffect;

    [Header("Projectile Properties")]
    [HideInInspector] public float damage;
    [HideInInspector] public float speed;

    private void Start() =>  rb.velocity = transform.right * speed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CameraCoffin"))
            return;

        else if (collision.CompareTag("Enemy"))
        {
            var hitPoint = collision.GetComponent<IDamageable<float>>();
            hitPoint.TakeDamage(damage);
            SoundManager.instance.Play("EnemyHit");
        }

        else if (collision.CompareTag("Player"))
        {
            var hitPoint = collision.GetComponent<IDamageable<float>>();
            hitPoint.TakeDamage(damage);
        }

        StartCoroutine(SpawnImpactEffect());
        Destroy(gameObject);
    }

    private IEnumerator SpawnImpactEffect()
    {
        var impactGameObject = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(impactGameObject, 2f);

        yield return null;
    }
}
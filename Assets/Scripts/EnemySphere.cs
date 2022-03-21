using System.Collections;
using UnityEngine;

public class EnemySphere : MonoBehaviour, IDamageable<float>
{
    [Header("Component References")]
    [SerializeField] private GameObject deathEffect;
    [Space]
    [SerializeField] private GameObject damagePopup;
    [SerializeField] private Transform popupTransform;
    [Space]
    [SerializeField] private SpriteRenderer[] sprites;

    [Header("Enemy Properties")]
    [SerializeField] private float enemyHealth = 100f;
    [SerializeField] private float enemyMeleeDamage = 40f;
    [Space]
    [SerializeField, Range(0, 10)] private float enemyMovmentSpeed = .5f;
    [SerializeField, Range(0, 10)] private float enemyWalkAmplitudeX = 5f;
    [SerializeField, Range(0, 10)] private float enemyWalkAmplitudeY = 3f;

    private Vector3 startPosition;
    private bool isDead = false;

    private void Start() => startPosition = transform.position;

    private void Update() => Patroling();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var hitPoint = collision.GetComponent<IDamageable<float>>();
            hitPoint.TakeDamage(enemyMeleeDamage);
        }
    }

    private void Patroling()
    {
        float positionX = startPosition.x + Mathf.Sin(Time.time * enemyMovmentSpeed) * enemyWalkAmplitudeX;
        float positionY = startPosition.y + Mathf.Cos(Time.time * enemyMovmentSpeed) * enemyWalkAmplitudeY;
        float positionZ = transform.position.z;

        transform.position = new Vector3(positionX, positionY, positionZ);
    }

    public void TakeDamage(float damage)
    {
        enemyHealth -= damage;
        StartCoroutine(GetHit());

        var popup = Instantiate(damagePopup);
        popup.GetComponent<FloatingPoints>().ShowDamage(damage, popupTransform);
        Destroy(popup, .4f);

        if (enemyHealth <= 0 && !isDead)
            Die();
    }

    private void Die()
    {
        isDead = true;

        SoundManager.instance.Play("EnemyExplode");

        var effect = Instantiate(deathEffect, transform.localPosition, Quaternion.identity);
        Destroy(effect, 2f);
        Destroy(gameObject);
    }

    private IEnumerator GetHit()
    {
        foreach (var sprite in sprites)
            sprite.color = new Color32(110, 65, 65, 255);

        yield return new WaitForSeconds(.2f);

        foreach (var sprite in sprites)
            sprite.color = Color.white;
    }

    private void OnDrawGizmosSelected()
    {
        var sizeX = enemyWalkAmplitudeX * 2 + transform.localScale.x;
        var sizeY = enemyWalkAmplitudeY * 2 + transform.localScale.y;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(sizeX, sizeY, 0));
    }
}
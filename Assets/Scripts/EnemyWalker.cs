using System.Collections;
using UnityEngine;

public class EnemyWalker : MonoBehaviour, IDamageable<float>
{
    [Header("Component References")]
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [Space]
    [SerializeField] private GameObject damagePopup;
    [SerializeField] private Transform popupTransform;
    [Space]
    [SerializeField] private SpriteRenderer[] sprites;

    [Header("Enemy Properties")]
    [SerializeField] private float enemyHealth = 100f;
    [SerializeField] private float enemyMeleeDamage = 40f;
    [Space]
    [SerializeField, Range(0, 20)] private float enemyShootRange = 10f;
    [SerializeField, Range(0, 10)] private float enemyMovmentSpeed = .5f;
    [SerializeField, Range(0, 10)] private float enemyWalkAmplitude = 3f;
    [SerializeField] private float enemyShootDelay = 3f;

    private Vector3 startPosition;
    private float countdown = 0f;
    private float lastPositionX;

    private bool facingRight = true;
    private bool isDead = false;

    private void Start() => startPosition = transform.position;

    private void Update()
    {
        Patroling();
        countdown -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Vector2 rayDirection;
        if (facingRight)
            rayDirection = Vector2.right;
        else
            rayDirection = Vector2.left;

        RaycastHit2D raycastHit = Physics2D.Raycast(firePoint.position, rayDirection, enemyShootRange);
        Debug.DrawRay(firePoint.position, rayDirection * enemyShootRange);

        if (raycastHit && raycastHit.collider.CompareTag("Player") && countdown <= 0)
        {
            StartCoroutine(FireProjectile());
            countdown = enemyShootDelay;
        }
    }

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
        float positionX = startPosition.x + Mathf.Sin(Time.time * enemyMovmentSpeed) * enemyWalkAmplitude;
        float positionY = transform.position.y;
        float positionZ = transform.position.z;

        transform.position = new Vector3(positionX, positionY, positionZ);

        // Check if character move direction is negative or positive value
        var direction = transform.position.x - lastPositionX;
        if (direction > 0 && !facingRight)
            Flip();
        else if (direction < 0 && facingRight)
            Flip();

        lastPositionX = transform.position.x;
    }

    private void Flip()
    {
        // Switch the way the character is labelled as facing.
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
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
    private IEnumerator FireProjectile()
    {
        yield return new WaitForSeconds(.5f);

        var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Destroy(bullet, 3f);
    }

    private void OnDrawGizmosSelected()
    {
        var sizeX = enemyWalkAmplitude * 2 + transform.localScale.x;
        var sizeY = transform.localScale.y;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.localPosition, new Vector3(sizeX, sizeY, 0));
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave : MonoBehaviour, IDamageable<float>
{
    public static List<GameObject> waveEnemyList = new List<GameObject>();

    [Header("Component References")]
    [SerializeField] private GameObject deathEffect;
    [Space]
    [SerializeField] private GameObject damagePopup;
    [SerializeField] private Transform popupTransform;
    [Space]
    [SerializeField] private SpriteRenderer[] sprites;

    [Header("Enemy Properties")]
    [SerializeField] private float enemyHealth = 20f;
    [SerializeField] private float enemyMeleeDamage = 10f;
    [Space]
    [SerializeField, Range(0, 10)] private float enemyMovmentSpeed = .5f;
    [SerializeField] private bool isFlying = false;

    private CharacterController2D player;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float countdown = 0f;
    private float lastPositionX;

    private bool facingRight = true;
    private bool isDead = false;

    private void Start()
    {
        player = FindObjectOfType<CharacterController2D>();

        startPosition = transform.position;
        waveEnemyList.Add(this.gameObject);
    }

    private void Update()
    {
        Patroling();
        countdown -= Time.deltaTime;
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
        if (isFlying)
        {
            var pos = Vector3.MoveTowards(transform.localPosition, player.transform.position, Time.deltaTime * enemyMovmentSpeed);
            targetPosition.x = pos.x;
            targetPosition.y = pos.y;
            targetPosition.z = transform.position.z;
        }
        else
        {
            var pos = Vector3.MoveTowards(transform.localPosition, player.transform.position, Time.deltaTime * enemyMovmentSpeed);
            targetPosition.x = pos.x;
            targetPosition.y = transform.position.y;
            targetPosition.z = transform.position.z;
        }

            transform.localPosition = targetPosition;

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
        waveEnemyList.Remove(this.gameObject);

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
}
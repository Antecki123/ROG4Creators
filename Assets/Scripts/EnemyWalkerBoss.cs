using System.Collections;
using UnityEngine;

public class EnemyWalkerBoss : MonoBehaviour, IDamageable<float>
{
    [Header("Component References")]
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject bigBulletPrefab;
    [SerializeField] private Transform firePoint;
    [Space]
    [SerializeField] private GameObject damagePopup;
    [SerializeField] private Transform popupTransform;
    [Space]
    [SerializeField] private SpriteRenderer[] sprites;
    [SerializeField] private Transform[] deathEffectPositions;

    [Header("Enemy Properties")]
    [SerializeField] private string bossName;
    [SerializeField] private float enemyMaxHealth = 1000f;
    [SerializeField] private float enemyMeleeDamage = 40f;
    [SerializeField, Range(0, 10)] private float enemyWalkAmplitude = 3f;
    [SerializeField, Range(0, 10)] private float enemyMovmentSpeed = .5f;
    [Space]
    [SerializeField] private float enemyShootDelay = 10f;

    private Vector3 playerPosition;
    private Vector3 startPosition;
    private float enemyHealth = 1000f;
    private float countdown = 10f;
    private bool isDead = false;
    private bool isShoot = false;

    public float EnemyHealth { get => enemyHealth; private set { } }
    public float EnemyMaxHealth { get => enemyMaxHealth; private set { } }
    public string EnemyName { get => bossName; private set { } }

    private void OnEnable()
    {
        enemyHealth = enemyMaxHealth;
        startPosition = transform.position;
        playerPosition = FindObjectOfType<CharacterController2D>().transform.position;

        UserInterface.instance.BossFight(bossName, true);

        //InvokeRepeating("Fire", 4f, 2f);
    }

    private void Update()
    {
        if (countdown <= 0 && !isShoot)
        {
            isShoot = true;
            StartCoroutine(FireSpread());
        }

        if (!isShoot && !isDead)
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
        float positionX = startPosition.x + Mathf.Sin(Time.time * enemyMovmentSpeed) * enemyWalkAmplitude;
        float positionY = transform.position.y;
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

        UserInterface.instance.UpdateBossHealth(enemyHealth, enemyMaxHealth);

        if (enemyHealth <= 0 && !isDead)
            StartCoroutine(Die());
    }
    /*
    private void Fire()
    {
        var direction = new Vector3();
        direction.x = firePoint.rotation.x;
        direction.y = firePoint.rotation.y;
        direction.z = Quaternion.LookRotation(firePoint.position, playerPosition);
        print(direction.z);

        var bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(direction));
        Destroy(bullet, 5f);
    }
    */
    private IEnumerator Die()
    {
        isDead = true;

        for (int i = 0; i < 15; i++)
        {
            var effectPosition = deathEffectPositions[Random.Range(0, deathEffectPositions.Length - 1)];
            var effect = Instantiate(deathEffect, effectPosition.position, Quaternion.identity);
            effect.transform.localScale *= Random.Range(.5f, 2f);

            yield return new WaitForSeconds(.3f);
            Destroy(effect, 2f);
        }
        var effectLast = Instantiate(deathEffect, transform.position, Quaternion.identity);
        effectLast.transform.localScale *= 3f;

        UserInterface.instance.BossFight(bossName, false);

        Destroy(effectLast, 2f);
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

    private IEnumerator FireSpread()
    {
        var angleRange = new Vector2(-140f, -200f);
        var angle = angleRange.x;
        var fireFrequency = 5f;

        while (angle >= angleRange.y)
        {
            yield return new WaitForSeconds(.3f);

            var bulletDirection = Quaternion.Euler(0, transform.rotation.eulerAngles.y, angle);
            var bullet = Instantiate(bigBulletPrefab, firePoint.position, bulletDirection);

            angle -= fireFrequency;
        }
        countdown = enemyShootDelay;
        isShoot = false;
    }

    private void OnDrawGizmosSelected()
    {
        var sizeX = enemyWalkAmplitude * 2 + transform.localScale.x;
        var sizeY = transform.localScale.y;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.localPosition, new Vector3(sizeX, sizeY, 0));
    }
}
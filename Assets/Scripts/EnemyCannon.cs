using System.Collections;
using UnityEngine;

public class EnemyCannon : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePosition;

    [Header("Enemy Properties")]
    [SerializeField] private float enemyShootDelay = 3f;

    private Transform playerTransform;

    private void OnEnable()
    {
        if (playerTransform != null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            StartCoroutine(Shoot());
        }
    }

    private IEnumerator Shoot()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) <= 10f)
            Instantiate(bulletPrefab, firePosition.position, firePosition.rotation);

        yield return new WaitForSeconds(enemyShootDelay);
        StartCoroutine(Shoot());
    }
}
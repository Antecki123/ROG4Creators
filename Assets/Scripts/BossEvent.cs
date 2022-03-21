using System.Collections;
using UnityEngine;
using Cinemachine;

public class BossEvent : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private GameObject bossHealthBar;
    [SerializeField] private GameObject finalDoor;
    [Space]
    [SerializeField] private AudioSource mainTheme;
    [SerializeField] private AudioSource bossTheme;
    [SerializeField] private AudioSource winTheme;
    [Space]
    [SerializeField] private CinemachineVirtualCamera vc;

    private EnemyWalkerBoss enemy;
    private bool startBossFight = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ChangeMusic());

            bossPrefab.SetActive(true);
            bossHealthBar.SetActive(true);

            startBossFight = true;
            enemy = bossPrefab.GetComponent<EnemyWalkerBoss>();
            vc.m_Priority = 20;
        }
    }

    private void Update()
    {
        if (startBossFight && enemy.EnemyHealth <= 0)
            StartCoroutine(BossDefeat());
    }

    private IEnumerator ChangeMusic()
    {
        while (mainTheme.volume != 0)
        {
            mainTheme.volume -= .001f;
            bossTheme.volume += .002f;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator BossDefeat()
    {
        while (bossTheme.volume != 0)
        {
            bossTheme.volume -= .001f;
            winTheme.volume += .002f;
            yield return new WaitForEndOfFrame();
        }

        bossHealthBar.SetActive(false);
        finalDoor.GetComponent<SpriteRenderer>().sortingOrder = -1;
    }
}
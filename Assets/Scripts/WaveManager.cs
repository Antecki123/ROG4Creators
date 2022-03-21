using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Manager")]
    [HideInInspector] public bool isDone = false;
    public bool endWave = false;

    [SerializeField] private Transform spawnPointLeft;
    [SerializeField] private Transform spawnPointRight;
    private Transform[] spawns;
    [Space]
    [SerializeField] private List<GameObject> wave1List;
    [SerializeField] private List<GameObject> wave2List;
    [SerializeField] private List<GameObject> wave3List;

    private Queue<GameObject> wave1;
    private Queue<GameObject> wave2;
    private Queue<GameObject> wave3;


    private void Start()
    {
        wave1 = new Queue<GameObject>(wave1List);
        wave2 = new Queue<GameObject>(wave2List);
        wave3 = new Queue<GameObject>(wave3List);

        UserInterface.instance.BossFight("Wave 1/3", true);
        UserInterface.instance.UpdateBossHealth(3, 3);

        InvokeRepeating("SpawnEnemy", 3f, 1.5f);

        spawns = new Transform[2];
        spawns[0] = spawnPointLeft;
        spawns[1] = spawnPointRight;
    }

    private void OnDisable()
    {
        UserInterface.instance.BossFight("", false);
        isDone = true;
    }

    private void SpawnEnemy()
    {
        var randomSpawn = Random.Range(0, 2);

        if (wave1.Count != 0)
        {
            Instantiate(wave1.Dequeue(), spawns[randomSpawn].position, spawns[randomSpawn].rotation);
            return;
        }

        else if (wave2.Count != 0)
        {
            UserInterface.instance.BossFight("Wave 2/3", true);
            UserInterface.instance.UpdateBossHealth(2, 3);
            Instantiate(wave2.Dequeue(), spawns[randomSpawn].position, spawns[randomSpawn].rotation);
            return;
        }
        else if (wave3.Count != 0)
        {
            UserInterface.instance.BossFight("Wave 3/3", true);
            UserInterface.instance.UpdateBossHealth(1, 3);
            Instantiate(wave3.Dequeue(), spawns[randomSpawn].position, spawns[randomSpawn].rotation);
            return;
        }

        if (wave1.Count == 0 && wave2.Count == 0 && wave3.Count == 0 && EnemyWave.waveEnemyList.Count == 0)
        {
            endWave = true;
            UserInterface.instance.UpdateBossHealth(0, 3);
            this.gameObject.SetActive(false);
        }
    }
}
using System.Collections;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    [Header("Game Objests")]
    [SerializeField] private GameObject player;
    [SerializeField] private QuestPointer questMarker;
    [Space]
    [SerializeField] private GameObject cityToBarDoor;
    [SerializeField] private GameObject barToCityDoor;
    [SerializeField] private GameObject toUndergroundDoor;
    [SerializeField] private GameObject fromUndergroundDoor;
    [SerializeField] private GameObject exitDoor;
    [SerializeField] private GameObject bossDoor;

    [Space]
    [SerializeField] private NPC barmaid;
    [SerializeField] private NPC vlad;
    [SerializeField] private NPC savedRobot;
    [SerializeField] private EnemyWalkerBoss walkerBoss;

    [Header("Quest Objectives")]
    [SerializeField] private WaveManager waveManager;
    [HideInInspector] public Location actualLocation = Location.City;
    public int activeObjective = 1;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Start() => StartCoroutine(Conversation1());

    private void Update()
    {
        switch (activeObjective)
        {
            case 1:
                Objective1();
                break;
            case 2:
                Objective2();
                break;
            case 3:
                Objective3();
                break;
            case 4:
                Objective4();
                break;
            case 5:
                Objective5();
                break;
            case 6:
                Objective6();
                break;
            case 7:
                Objective7();
                break;
            case 8:
                Objective8();
                break;
        }
    }

    private void Objective1()
    {
        savedRobot.SetMarker(false);
        vlad.SetMarker(true);
        barmaid.SetMarker(true);

        vlad.SetActiveDialouge("Vlad_1");

        questMarker.gameObject.SetActive(true);

        if (actualLocation == Location.City)
            questMarker.SetTarget(cityToBarDoor.transform);
        else
            questMarker.SetTarget(barmaid.transform);

        if (Vector3.Distance(barmaid.transform.position, player.transform.position) <= 1.5f && activeObjective == 1)
        {
            activeObjective++;
            StartCoroutine(Conversation2());
        }
    }

    private void Objective2()
    {
        savedRobot.SetMarker(true);
        vlad.SetMarker(true);
        barmaid.SetMarker(false);

        vlad.SetActiveDialouge("Vlad_2");

        questMarker.gameObject.SetActive(true);

        if (actualLocation == Location.City)
            questMarker.SetTarget(toUndergroundDoor.transform);
        else if (actualLocation == Location.Bar)
            questMarker.SetTarget(barToCityDoor.transform);
        else
            questMarker.SetTarget(savedRobot.transform);

        if (Vector3.Distance(savedRobot.transform.position, player.transform.position) <= 1.5f && activeObjective == 2)
        {
            activeObjective++;
            StartCoroutine(Conversation3());
        }
    }

    private void Objective3()
    {
        questMarker.gameObject.SetActive(false);

        savedRobot.SetMarker(false);
        vlad.SetMarker(false);
        barmaid.SetMarker(false);

        //if (waveManager.isDone)
        if (EnemyWave.waveEnemyList.Count == 0 && waveManager.endWave)
        {
            exitDoor.GetComponent<BoxCollider2D>().enabled = true;

            activeObjective++;
            StartCoroutine(Conversation4());
        }
    }

    private void Objective4()
    {
        questMarker.gameObject.SetActive(true);

        if (actualLocation == Location.Underground)
            questMarker.SetTarget(exitDoor.transform);
        else if (actualLocation == Location.City)
            questMarker.SetTarget(cityToBarDoor.transform);
        else if (actualLocation == Location.Bar)
            questMarker.SetTarget(barmaid.transform);

        savedRobot.SetMarker(false);
        vlad.SetMarker(false);
        barmaid.SetMarker(true);

        if (Vector3.Distance(barmaid.transform.position, player.transform.position) <= 1.5f && activeObjective == 4)
        {
            UserInterface.instance.UpdatePoints(1500);

            activeObjective++;
            StartCoroutine(Conversation5());
        }
    }

    private void Objective5()
    {
        questMarker.gameObject.SetActive(true);

        if (actualLocation == Location.Underground)
            questMarker.SetTarget(fromUndergroundDoor.transform);
        else if (actualLocation == Location.City)
            questMarker.SetTarget(vlad.transform);
        else if (actualLocation == Location.Bar)
            questMarker.SetTarget(barToCityDoor.transform);

        savedRobot.SetMarker(false);
        vlad.SetMarker(true);
        barmaid.SetMarker(false);

        if (Vector3.Distance(vlad.transform.position, player.transform.position) <= 1.5f && activeObjective == 5)
        {
            activeObjective++;
            StartCoroutine(Conversation6());
        }
    }

    private void Objective6()
    {
        questMarker.gameObject.SetActive(true);

        if (actualLocation == Location.Underground)
            questMarker.SetTarget(walkerBoss.transform);
        else if (actualLocation == Location.City)
            questMarker.SetTarget(toUndergroundDoor.transform);
        else if (actualLocation == Location.Bar)
            questMarker.SetTarget(barToCityDoor.transform);

        savedRobot.SetMarker(false);
        vlad.SetMarker(false);
        barmaid.SetMarker(false);

        if (Vector3.Distance(walkerBoss.transform.position, player.transform.position) <= 10f && activeObjective == 6)
        {
            activeObjective++;
            StartCoroutine(Conversation7());
        }
    }

    private void Objective7()
    {
        questMarker.gameObject.SetActive(false);

        savedRobot.SetMarker(false);
        vlad.SetMarker(false);
        barmaid.SetMarker(false);

        walkerBoss.enabled = true;

        if (walkerBoss != null)
        {
            UserInterface.instance.BossFight(walkerBoss.EnemyName, true);
            UserInterface.instance.UpdateBossHealth(walkerBoss.EnemyHealth, walkerBoss.EnemyMaxHealth);
        }
        else
        {
            UserInterface.instance.BossFight("", false);
            activeObjective++;
        }
    }

    private void Objective8()
    {
        questMarker.gameObject.SetActive(true);

        if (actualLocation == Location.Underground)
            questMarker.SetTarget(bossDoor.transform);
        else if (actualLocation == Location.City)
            questMarker.SetTarget(vlad.transform);
        else if (actualLocation == Location.Bar)
            questMarker.SetTarget(barToCityDoor.transform);

        savedRobot.SetMarker(false);
        vlad.SetMarker(true);
        barmaid.SetMarker(false);

        if (Vector3.Distance(vlad.transform.position, player.transform.position) <= 1.5f && activeObjective == 8)
        {
            activeObjective++;
            StartCoroutine(Conversation8());
        }
    }

    private IEnumerator Conversation1()
    {
        yield return new WaitForSeconds(5f);

        DialogManager.instance.CreateDialog("Rog_1", true);
        SoundManager.instance.PlayDialogue("Rog_1");
    }

    private IEnumerator Conversation2()
    {
        DialogManager.instance.CreateDialog("Barmaid_1", true);
        SoundManager.instance.PlayDialogue("Barmaid_1");

        yield return new WaitForSeconds(3.7f);

        DialogManager.instance.CreateDialog("Rog_2", true);
        SoundManager.instance.PlayDialogue("Rog_2");

        yield return new WaitForSeconds(2.7f);

        DialogManager.instance.CreateDialog("Barmaid_2", true);
        SoundManager.instance.PlayDialogue("Barmaid_2");

        yield return new WaitForSeconds(8.3f);

        DialogManager.instance.CreateDialog("Rog_3", true);
        SoundManager.instance.PlayDialogue("Rog_3");

        yield return new WaitForSeconds(3.3f);

        DialogManager.instance.CreateDialog("Barmaid_3", true);
        SoundManager.instance.PlayDialogue("Barmaid_3");

        yield return new WaitForSeconds(3.3f);
    }

    private IEnumerator Conversation3()
    {
        DialogManager.instance.CreateDialog("Robot_1", true);
        SoundManager.instance.PlayDialogue("Robot_1");

        yield return new WaitForSeconds(3.7f);

        DialogManager.instance.CreateDialog("Rog_4", true);
        SoundManager.instance.PlayDialogue("Rog_4");

        yield return new WaitForSeconds(3.7f);

        waveManager.gameObject.SetActive(true);
    }

    private IEnumerator Conversation4()
    {
        DialogManager.instance.CreateDialog("Robot_2", true);
        SoundManager.instance.PlayDialogue("Robot_2");

        yield return new WaitForSeconds(2.7f);
    }

    private IEnumerator Conversation5()
    {
        DialogManager.instance.CreateDialog("Barmaid_4", true);
        SoundManager.instance.PlayDialogue("Barmaid_4");

        yield return new WaitForSeconds(3.8f);

        DialogManager.instance.CreateDialog("Barmaid_5", true);
        SoundManager.instance.PlayDialogue("Barmaid_5");

        yield return new WaitForSeconds(7.7f);
    }

    private IEnumerator Conversation6()
    {
        DialogManager.instance.CreateDialog("Vlad_3", true);
        SoundManager.instance.PlayDialogue("Vlad_3");

        yield return new WaitForSeconds(10.2f);

        DialogManager.instance.CreateDialog("Rog_5", true);
        SoundManager.instance.PlayDialogue("Rog_5");

        yield return new WaitForSeconds(4.4f);
    }

    private IEnumerator Conversation7()
    {
        DialogManager.instance.CreateDialog("Rog_6", true);
        SoundManager.instance.PlayDialogue("Rog_6");

        yield return new WaitForSeconds(2f);

        DialogManager.instance.CreateDialog("Boss", true);
        SoundManager.instance.PlayDialogue("Boss");

        yield return new WaitForSeconds(5.1f);
    }

    private IEnumerator Conversation8()
    {
        DialogManager.instance.CreateDialog("Vlad_4", true);
        SoundManager.instance.PlayDialogue("Vlad_4");

        yield return new WaitForSeconds(9f);

        DialogManager.instance.CreateDialog("Rog_7", true);
        SoundManager.instance.PlayDialogue("Rog_7");

        yield return new WaitForSeconds(2f);

        UserInterface.instance.GameOver();
    }
}

public enum Location { City, Bar, Underground }
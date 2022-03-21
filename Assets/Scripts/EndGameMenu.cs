using UnityEngine;
using TMPro;
using System.Collections;

public class EndGameMenu : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private GameObject endScreen;

    private void OnEnable() => StartCoroutine(OverScreen());

    private IEnumerator OverScreen()
    {
        Time.timeScale = .3f;
        yield return new WaitForSecondsRealtime(2f);

        var screenSpeed = 5f;
        while(endScreen.transform.localPosition != Vector3.zero)
        {
            yield return new WaitForEndOfFrame();
            endScreen.transform.Translate(Vector3.up * screenSpeed);
        }
    }
}
using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;

    [Header("Component UI References")]
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private TextMeshProUGUI dialogLine;

    [Header("Dialogs")]
    [SerializeField] [Range(0, 100)] private float typingSpeed = 40f;
    [SerializeField] private Dialog[] sentences;

    private int index = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    public void CreateDialog(string ID, bool openPanel)
    {
        index = Array.FindIndex(sentences, sentence => sentence.sentenceID == ID);

        dialogPanel.SetActive(openPanel);
        dialogLine.text = string.Empty;

        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        float typeLetter = 0;
        int charIndex = 0;

        while (charIndex < sentences[index].sentence.Length)
        {
            typeLetter += Time.deltaTime * typingSpeed;
            charIndex = Mathf.FloorToInt(typeLetter);
            charIndex = Mathf.Clamp(charIndex, 0, sentences[index].sentence.Length);

            dialogLine.text = sentences[index].sentence.Substring(0, charIndex);

            yield return null;
        }
        dialogLine.text = sentences[index].sentence;

        yield return new WaitForSeconds(.5f);
        dialogLine.text = string.Empty;
        dialogPanel.gameObject.SetActive(false);
    }
}

[System.Serializable]
public struct Dialog
{
    public string sentenceID;
    [TextArea] public string sentence;
}
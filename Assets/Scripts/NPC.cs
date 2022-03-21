using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private GameObject dialougeMarker;

    private string dialogue;

    public void SetMarker(bool state) => dialougeMarker.SetActive(state);
    public void SetActiveDialouge(string dialogID) => dialogue = dialogID;

    public void TalkTo()
    {
        if (dialogue != string.Empty)
        {
            DialogManager.instance.CreateDialog(dialogue, true);
            SoundManager.instance.PlayDialogue(dialogue);
        }
    }
}
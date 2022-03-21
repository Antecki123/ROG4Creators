using System.Collections;
using UnityEngine;
using TMPro;

public class FloatingPoints : MonoBehaviour
{
    [SerializeField] private TextMeshPro floatingText;

    public void ShowDamage(float damage, Transform popupTransform)
    {
        transform.position = popupTransform.position;
        floatingText.text = $"-{damage}";
    }
}
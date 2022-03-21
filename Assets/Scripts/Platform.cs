using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private Collider2D boxCollider;
    private CharacterController2D player;

    private void Start() => player = FindObjectOfType<CharacterController2D>();

    private void Update()
    {
        var offset = 2f;
        if (player && player.transform.position.y > transform.position.y + offset)
            boxCollider.enabled = true;
        else
            boxCollider.enabled = false;
    }
}

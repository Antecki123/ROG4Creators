using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IDamageable<float>
{
    [Header("Component References")]
    [SerializeField] private CharacterController2D characterController;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject deathEffect;

    public static int money;

    [Header("Player Properties")]
    [SerializeField] private float maxHealth = 100f;
    private float actualHealth = 100f;

    [Header("Animations")]
    private readonly int Speed = Animator.StringToHash("Speed");
    private readonly string isDead = "isDead";

    private void Start()
    {
        actualHealth = maxHealth;
        UserInterface.instance.UpdatePoints(9000);
    }

    private void LateUpdate()
    {
        animator.SetFloat(Speed, Mathf.Abs(characterController.HorizontalMove));
    }

    public void TakeDamage(float damage)
    {
        actualHealth -= damage;

        if (actualHealth > maxHealth)
            actualHealth = maxHealth;

        if (actualHealth <= 0)
            StartCoroutine(Die());
        if (damage > 0)
            StartCoroutine(UserInterface.instance.AvatarHit(Color.red, 255));
        else if (damage < 0)
            StartCoroutine(UserInterface.instance.AvatarHit(Color.green, 2));

        UserInterface.instance.UpdateHealthBar(actualHealth, maxHealth);
    }

    private IEnumerator Die()
    {
        animator.SetTrigger(isDead);
        characterController.isDead = true;

        yield return new WaitForSeconds(1f);

        Instantiate(deathEffect, transform.position, Quaternion.identity);
        UserInterface.instance.GameOver();

        Destroy(gameObject);
    }
}
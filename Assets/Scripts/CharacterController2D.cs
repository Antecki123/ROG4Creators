using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterController2D : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private WeaponManager weaponManager;
    [Space]
    [SerializeField] private Transform groundCollider;

    [Header("Controller Properties")]
    [SerializeField, Range(0, 50)] private float runSpeed = 15f;
    [SerializeField, Range(0, 50)] private float jumpSpeed = 30f;

    [HideInInspector] public bool isDead = false;
    private bool isJump;
    private float horizontalMove;
    private bool facingRight = true;        // For determining which way the player is currently facing.

    [SerializeField] private GameObject pendingObject;

    public float HorizontalMove { get => horizontalMove; private set { } }

    private InputControls inputActions;
    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void Awake()
    {
        inputActions = new InputControls();
        inputActions.Gameplay.Movement.performed += ctx => horizontalMove = ctx.ReadValue<float>();
        inputActions.Gameplay.Movement.canceled += ctx => horizontalMove = 0;

        inputActions.Gameplay.Jump.performed += ctx => isJump = true;
        inputActions.Gameplay.Jump.canceled += ctx => isJump = false;

        inputActions.Gameplay.Shoot.performed += ctx => weaponManager.isFire = true;
        inputActions.Gameplay.Shoot.canceled += ctx => weaponManager.isFire = false;

        inputActions.Gameplay.Interaction.performed += ctx => OnInteraction();
        inputActions.Gameplay.SwapWeapon.performed += ctx => weaponManager.OnSwapWeapon();
        inputActions.Gameplay.Reload.performed += ctx => weaponManager.OnReload();

        inputActions.Gameplay.OpenMenu.performed += ctx => UserInterface.instance.OpenPausePanel();
    }

    private void FixedUpdate()
    {
        if (IsGrounded() && isJump)
            rb2D.velocity = Vector2.up * jumpSpeed;

        Move(horizontalMove);
    }

    private void OnTriggerStay2D(Collider2D collision) => pendingObject = collision.gameObject;
    private void OnTriggerExit2D(Collider2D collision) => pendingObject = null;

    private void OnInteraction()
    {
        if (pendingObject != null)
        {
            if (pendingObject.CompareTag("Item"))
                weaponManager.OnInteraction(pendingObject);

            else if (pendingObject.CompareTag("Portal"))
                pendingObject.GetComponent<IDoor>().MovePlayer(gameObject);

            else if (pendingObject.CompareTag("Vending"))
                pendingObject.GetComponent<VendingMachine>().OpenPanel();

            else if (pendingObject.CompareTag("NPC"))
                pendingObject.GetComponent<NPC>().TalkTo();
        }
    }

    private void Move(float move)
    {
        // Move the character by finding the target velocity
        if (!isDead)
            rb2D.velocity = new Vector2(runSpeed * move, rb2D.velocity.y);
        else
            rb2D.velocity = Vector2.zero;

        if (horizontalMove > 0 && !facingRight)
            Flip();
        else if (horizontalMove < 0 && facingRight)
            Flip();
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private bool IsGrounded()
    {
        // Checks if the character is on the ground
        RaycastHit2D raycastHit = Physics2D.CapsuleCast(groundCollider.position, new Vector2(1, .2f),
            CapsuleDirection2D.Horizontal, 0f, Vector2.zero);

        return raycastHit;
    }
}
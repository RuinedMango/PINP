using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    public Rigidbody2D body;
    public PlayerInput controls;
    public float moveSpeed;

    private Vector2 moveDir = Vector2.zero;

    /*these floats are the force you use to jump, the max time you want your jump to be allowed to happen,
     * and a counter to track how long you have been jumping*/
    public float jumpForce;
    public float jumpTime;
    private float jumpTimeCounter;
    /*this bool is to tell us whether you are on the ground or not
     * the layermask lets you select a layer to be ground; you will need to create a layer named ground(or whatever you like) and assign your
     * ground objects to this layer.
     * The stoppedJumping bool lets us track when the player stops jumping.*/
    private bool grounded;
    [SerializeField] private LayerMask whatIsGround;
    private bool stoppedJumping;

    /*the public transform is how you will detect whether we are touching the ground.
     * Add an empty game object as a child of your player and position it at your feet, where you touch the ground.
     * the float groundCheckRadius allows you to set a radius for the groundCheck, to adjust the way you interact with the ground*/

    [SerializeField] private Transform groundCheck;
    public float groundCheckRadius;

    private void OnEnable()
    {
        controls.ActivateInput();
    }

    private void OnDisable()
    {
        controls.DeactivateInput();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jumpTimeCounter = jumpTime;
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = controls.actions.FindAction("Move").ReadValue<Vector2>();

        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if (grounded)
        {
            jumpTimeCounter = jumpTime;
        }

    }

    private void FixedUpdate()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }

        HandleMove();
        HandleJump();
    }

    private void HandleMove()
    {
        body.velocityX = moveDir.x * (moveSpeed * 100) * Time.fixedDeltaTime;
    }
    
    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded)
            {
                body.velocity = new Vector2(body.velocity.x, jumpForce);
                stoppedJumping = false;
            }
        }
        if (Input.GetKey(KeyCode.Space) && !stoppedJumping)
        {
            if (jumpTimeCounter > 0)
            {
                body.velocity = new Vector2(body.velocity.x, jumpForce);
                jumpTimeCounter -= Time.fixedDeltaTime;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpTimeCounter = 0;
            stoppedJumping = true;
        }
    }
}

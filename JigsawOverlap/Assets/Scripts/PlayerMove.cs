using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb;
    public Animator anim;

   
    private float xInput;
    private int facingdir = 1;
    private bool facingRight;

    [Header("Speed info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    [Header("Collison info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatisGround;
    private bool isGrounded;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
        xInput = Input.GetAxisRaw("Horizontal");
        Movement();

        if (Input.GetKeyDown(KeyCode.R))
            Flip();


        FlipController();
        AnimatorControllers();
        CollisionChecks();

    }

    private void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatisGround);
    }

    private void Movement()
    {
        rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        if(isGrounded)
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void AnimatorControllers()
    {
        bool isMoving = rb.velocity.x != 0;

        anim.SetFloat("yVelocity",rb.velocity.y);

        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
    }

    private void Flip()
    {
        facingdir = facingdir * -1;
        facingRight = !facingRight; // true false switcher
        transform.Rotate(0, 180, 0);
    }
    
    private void FlipController()
    {
        if (rb.velocity.x < 0 && !facingRight)
        {
            Flip();
        }else if (rb.velocity.x > 0 && facingRight)
        {
            Flip();
        }
    }


    private void OnDrawGizmos()
    {
        // Ground Check Distance
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y-groundCheckDistance ));
    }
}

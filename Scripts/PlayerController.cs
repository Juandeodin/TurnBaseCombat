using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Vector2 = UnityEngine.Vector2;

public class PlayerController : MonoBehaviour
{
    public float longidlleTime = 5f;
    private float longIdleTimer;
    public float speed = 2.5f;
    public float jumpForce = 2.5f;
    //Referencias
    private Rigidbody2D rigidbody;
    private Animator animator;
    //Movimiento
    private Vector2 movement;
    //mirar
    private bool right = true;
    //suelo
    private bool isGrounded = false;
    //atacar
    private bool isAttacking;

    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking == false)
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal"); // con el raw en vez darte el 0,564 o -059354 o datos asi, solo te da +1 y -1
            movement = new UnityEngine.Vector2(horizontalInput, 0f);

            //flip character
            if (horizontalInput < 0f && right == true)
            {
                Flip();


            }
            else if (horizontalInput > 0f && right == false)
            {

                Flip();
            }
        }
        //esta en el suelo?
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        //esta Saltando?
        if (Input.GetButtonDown("Jump") && isGrounded == true && isAttacking == false) {

            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        }
        //quiere Atacar?
        if (Input.GetButtonDown("Fire1") && isGrounded == true && isAttacking == false)
        {
            movement = Vector2.zero;
            rigidbody.velocity = Vector2.zero;
            animator.SetTrigger("Atack");


        }

    }
    void FixedUpdate()
    {
        if (isAttacking == false)
        {
            float horizontalVelocity = movement.normalized.x * speed;
            rigidbody.velocity = new Vector2(horizontalVelocity, rigidbody.velocity.y);
        }

    }

    void LateUpdate()
    {
        animator.SetBool("Iddle", movement == Vector2.zero);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("vertical velocity", rigidbody.velocity.y);
        //Animator
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Atack"))
        {
            isAttacking = true;

        }
        else
        {

            isAttacking = false;

        }
        //Long Iddle
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
        {
        
            longIdleTimer += Time.deltaTime;
            
            if (longIdleTimer >= longidlleTime)
            {
             
                animator.SetTrigger("LongIdle");
               
            }



        }
        else
        {
            longIdleTimer = 0f;
        }
    }

        private void Flip()
        {
            right = !right;
            float localScalex = transform.localScale.x;
            localScalex = localScalex * -1f;
            transform.localScale = new UnityEngine.Vector3(localScalex, transform.localScale.y, transform.localScale.z);


        }
    private void OnEnable()
    {
        
    }

}



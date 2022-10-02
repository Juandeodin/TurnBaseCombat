using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    private bool right=true;
    public Animator animator;
    private bool isAttacking = false;


    Vector2 movement;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        //Input
        movement.x=Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        //flip character
        if (movement.x < 0f && right == true)
        {
            Flip();


        }
        else if (movement.x > 0f && right == false)
        {

            Flip();
        }
        //Atacar
        if (Input.GetButtonDown("Fire1") &&  isAttacking == false)
        {
            movement = Vector2.zero;
            rb.velocity = Vector2.zero;
            animator.SetTrigger("Atack");


        }
    }
    private void FixedUpdate() // se llama más que el update(se usa para físicas)
    {
        if (isAttacking == false) { 
        //Movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);//rb donde estamos+ la direccion + la velocidad
    }
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Atack"))
        {
            isAttacking = true;

        }
        else
        {

            isAttacking = false;

        }

    }
    private void Flip()
    {
        right = !right;
        float localScalex = transform.localScale.x;
        localScalex = localScalex * -1f;
        transform.localScale = new UnityEngine.Vector3(localScalex, transform.localScale.y, transform.localScale.z);


    }
    private void LateUpdate()
    {
        animator.SetBool("Iddle", movement == Vector2.zero);
    }
}

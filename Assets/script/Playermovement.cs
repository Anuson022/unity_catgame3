using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Playermovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private LayerMask GroundLayer;
    [SerializeField] private LayerMask WallLayer;
    [SerializeField] private float jumpPower;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D BoxCollider;
    private float wallJumpCooldown;
    private float HorizontalInput;

    private void Awake()
    {
        //grab reference form game object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        BoxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        float HorizontalInput = Input.GetAxis("Horizontal");

        body.velocity = new Vector2(HorizontalInput * speed,body.velocity.y);
        //right flip
        if (HorizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(5, 5, 1);
        }
        //left flip
        else if(HorizontalInput < -0.01f) 
        {
            transform .localScale = new Vector3(-5,5,1);
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded())
        {
            jump();
        }
        //set animation
        anim.SetBool("run", HorizontalInput != 0);
        anim.SetBool("grounded",isGrounded());

        //Wall jump logic
        if (wallJumpCooldown > 0.2f)
        {
            body.velocity = new Vector2(HorizontalInput * speed, body.velocity.y);

            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
                body.gravityScale = 7;

            if (Input.GetKey(KeyCode.Space))
                jump();
        }
        else
            wallJumpCooldown += Time.deltaTime;
    }
    private void jump() 
    {
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
        else if (onWall() && isGrounded())
        {
            /*if (HorizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);
            */
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("jump");
            wallJumpCooldown = 0f;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*if(collision.gameObject.tag == "Grounded") 
        {
            grounded = true;
        }*/
    }
    private bool isGrounded() 
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(BoxCollider.bounds.center, BoxCollider.bounds.size, 0, Vector2.down, 0.1f, GroundLayer);
        //Debug.Log(raycastHit.collider);
        return raycastHit.collider != null;

    }
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(BoxCollider.bounds.center, BoxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, WallLayer);
        return raycastHit.collider != null;
    }
}

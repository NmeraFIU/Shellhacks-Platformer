using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int exJumps = 2;
    public LayerMask groundLayer;
    public Transform feet;
    bool isGrounded;
    int jumpCount;
    float jumpCoolDown;
    float jumpPower = 7.0f;

    int dcounter = 0;

    public GameObject timer;

    private Animator anim;
    private SpriteRenderer sprite;
    private float dirX = 0f;

    private enum MovementState {idle, walking, jumping, falling }

    private Rigidbody2D rb;

    private Vector3 respawnPoint;
    public GameObject damageDetector;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        respawnPoint = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {

        dirX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector3(dirX * 5f, rb.velocity.y);

        if (Input.GetButtonDown("Jump"))
        {
            //rb.velocity = new Vector3(0, 7f, 0);
            Jump();
        }

        UpdateAnimationUpdate();

        CheckGrounded();
        if (Input.GetButtonUp("Jump"))
        {
            rb.velocity = new Vector3(0, -2f, 0);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = respawnPoint;
        }
    }

    private void UpdateAnimationUpdate()
    {
        MovementState state;

        if (dirX > 0f)
        {
            state = MovementState.walking;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.walking;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        } else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            //*****Player dying and respawning*****
            Debug.Log("You died!");
            transform.position = respawnPoint;
        }
        else if (collision.gameObject.CompareTag("Checkpoint"))
        {
            //I believe this is redundant code. Will worry about deleting later.
            Debug.Log(collision.gameObject.transform.position);
            Debug.Log("Checkpoint reached!");
            respawnPoint = collision.gameObject.transform.position;
        }
    }

    void Jump()
    {
        if (isGrounded || jumpCount < exJumps)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            jumpCount++;
        }
    }

    void CheckGrounded()
    {
        if (Physics2D.OverlapCircle(feet.position, 0.5f, groundLayer))
        {
            isGrounded = true;
            jumpCount = 0;
            jumpCoolDown = Time.time + 0.2f;
        }
        else if (Time.time < jumpCoolDown)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //*****update player's spawn point at checkpoint*****
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            Debug.Log(collision.gameObject.transform.position);
            Debug.Log("Checkpoint reached!");
            respawnPoint = collision.gameObject.transform.position;
        } else if (collision.gameObject.CompareTag("Damage")) {
            transform.position = respawnPoint;
            dcounter++;
        } 
    }
}

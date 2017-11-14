using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Character's states
// Idle, Walk, Run, Jump, Die, Crawl, Hike

public class PlatformCharacter2D : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rigidBody;
    private SpriteRenderer sRenderer;

    [SerializeField]
    private bool isGrounded = false;
    private bool isAFacingRight = true;

    public LayerMask groundLayer;

    public float speed;
    public float jumpForce;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        sRenderer = GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start ()
    {
	    	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        //TODO: Replace the string with a hash
        animator.SetFloat("HorizontalSpeed", Mathf.Abs(rigidBody.velocity.x));

        //TODO: Validate if there is a better way to construct this 2D points
        //TODO: OverlapCircle or CircleCast
        Vector2 colliderPos = new Vector2(transform.position.x, transform.position.y - 0.85f);
        isGrounded = Physics2D.OverlapCircle(colliderPos, 0.5f, groundLayer) == null ? false : true;
        //Debug.Log("IsGrounded: " + isGrounded);
        //animator.SetBool("Ground", isGrounded);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y - 0.85f, transform.position.z), 0.5f);
    }

    private void Flip()
    {
        // Faster than negate the scale
        isAFacingRight = !isAFacingRight;
        sRenderer.flipX = !isAFacingRight;
    }
        

    public void Die()
    {
        animator.SetBool("Dead", true);
    }

    public void Move(float move, bool jump)
    {

        float upVelocity = 0;
        
        //Debug.Log("Jump" + jump);

        if(move > 0 && !isAFacingRight)
        {
            Flip();
        }
        else if(move < 0 && isAFacingRight)
        {
            Flip();
        }

        if(isGrounded && jump)
        {   
            jump = false;
            upVelocity = jumpForce;
            //rigidBody.AddForce(Vector2.up * 10.5f, ForceMode2D.Impulse);
            animator.SetTrigger("Jump");
        }
        else
        {
            upVelocity = 0;
        }

        rigidBody.velocity = new Vector2(move * speed, rigidBody.velocity.y + upVelocity);
    }
}

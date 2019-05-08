using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private float moveSpeed;
    [SerializeField]
    private float currentSpeed;
    public float sprintSpeed;
    public float fullSpeed;
    public float sneakSpeed;
    public bool sneaking;
    public bool sprint;
    public int health;
    public bool dead;
    public bool inAir;
    private float horizontal;
    private float vertical;
    [SerializeField]
    private float jumpTimer;
    public float jumpForce;
    private Rigidbody rb;
    private Animator playerAnim;

	// Use this for initialization
	void Start ()
    {
        playerAnim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        moveSpeed = fullSpeed;
        this.transform.position = GameObject.FindWithTag("StartSpawn").transform.position;
        this.transform.rotation = GameObject.FindWithTag("StartSpawn").transform.rotation;
        dead = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        
        jumpTimer -= 0.1f;

        if(Input.GetButton("Sneak"))
        {
            sneaking = true;
        }
        else
        {
            sneaking = false;
        }

        if(sneaking)
        {
            moveSpeed = sneakSpeed;
        }
        else
        {
            moveSpeed = fullSpeed;
        }

        if(Input.GetButton("Sprint"))
        {
            sprint = true;
        }
        else
        {
            sprint = false;
        }

        if(sprint)
        {
            moveSpeed = sprintSpeed;
        }
        else
        {
            moveSpeed = fullSpeed;
        }

        if(Input.GetButton("Jump") && !inAir && jumpTimer <= 0)
        {
            jumpTimer = 3.0f;
            rb.AddForce(Vector3.up * jumpForce);
            inAir = true;
        }

        if(health <= 0)
        {
            dead = true;
        }

        if (dead)
        {
            this.transform.position = GameObject.FindWithTag("StartSpawn").transform.position;
            this.transform.rotation = GameObject.FindWithTag("StartSpawn").transform.rotation;
            dead = false;
            health = 5;
        }

        PlayerMovement();

        playerAnim.SetBool("Sneaking", sneaking);
        playerAnim.SetBool("Sprinting", sprint);
        playerAnim.SetBool("Dead", dead);
        playerAnim.SetBool("InAir", inAir);
    }

    void PlayerMovement()
    {
        currentSpeed = new Vector2(horizontal, vertical).sqrMagnitude;
        
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector3 playerInput = new Vector3(horizontal, 0f, vertical);
        Vector3 playerMove = Vector3.ClampMagnitude(playerInput, 1.0f) * moveSpeed * Time.deltaTime;
        transform.Translate(playerMove, Space.Self);
        playerAnim.SetFloat("Speed", currentSpeed);
    }

    

    //public IEnumerator TakeDamage()
    //{
    //    yield return new WaitForSeconds(2);
    //    health -= 1;
    //    if (dead)
    //    {
    //        this.transform.position = startSpawn.transform.position;
    //        this.transform.rotation = startSpawn.transform.rotation;
    //        dead = false;
    //        health = 5;
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            inAir = false;
        }
    }
}

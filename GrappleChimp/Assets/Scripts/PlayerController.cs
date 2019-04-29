using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private float moveSpeed;
    public float fullSpeed;
    public float sneakSpeed;
    public bool sneaking;

	// Use this for initialization
	void Start ()
    {
        moveSpeed = fullSpeed;
	}
	
	// Update is called once per frame
	void Update ()
    {
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
        PlayerMovement();
    }

    void PlayerMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 playerMove = new Vector3(horizontal, 0f, vertical) * moveSpeed * Time.deltaTime;
        transform.Translate(playerMove, Space.Self);
    }
}

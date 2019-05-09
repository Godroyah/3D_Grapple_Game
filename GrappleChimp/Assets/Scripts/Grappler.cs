using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappler : MonoBehaviour {

    public Camera mainCam;
    public float maxGrappleDist;
    public float pullSpeed;
    [SerializeField]
    private float layerWeight;
    private Animator playerAnim;
    private PlayerController playerController;

    private Vector3 grappleTarget;
    [SerializeField]
    private bool aiming = false;
    public bool grappled = false;


	// Use this for initialization
	void Start ()
    {
        playerAnim = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButton(1) && !grappled)
        {
            aiming = true;
        }
        else
        {
            aiming = false;
        }

        if(aiming)
        {
            if(layerWeight < 1.0f)
            {
                layerWeight += 0.1f;
            }
            playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("Aiming"), layerWeight);
        }
        else
        {
            if(layerWeight > 0.0f)
            {
                layerWeight -= 0.1f;
            }
            playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("Aiming"), layerWeight);
        }

	    if(Input.GetMouseButtonUp(0) && aiming)
        {
            GrappleRay();
        }

        if(grappled)
        {
            PlayerGrappled();
        }
        playerAnim.SetBool("Aiming", aiming);
        playerAnim.SetBool("Grappled", grappled);
    }

    private void PlayerGrappled()
    {
        transform.position = Vector3.Lerp(transform.position, grappleTarget, pullSpeed * Time.deltaTime);

        float dist = Vector3.Distance(transform.position, grappleTarget);

        if(dist <= 2.5f)
        {
            grappled = false;
        }
    }

    private void GrappleRay()
    {
        Ray grappleRay = mainCam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(mainCam.transform.position, Input.mousePosition, Color.red);
        RaycastHit hit;

        if(Physics.Raycast(grappleRay, out hit, maxGrappleDist))
        {
            if(hit.collider.CompareTag("GrappleTarget"))
            {
                grappleTarget = hit.point;
                grappled = true;
                aiming = false;
            }
        }
    }
}

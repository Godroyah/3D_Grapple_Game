using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappler : MonoBehaviour {

    public Camera mainCam;
    public float maxGrappleDist;
    public float pullSpeed;

    private Vector3 grappleTarget;
    public bool grappled = false;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(Input.GetMouseButtonDown(0))
        {
            GrappleRay();
        }

        if(grappled)
        {
            PlayerGrappled();
        }
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
        RaycastHit hit;

        if(Physics.Raycast(grappleRay, out hit, maxGrappleDist))
        {
            if(hit.collider.CompareTag("GrappleTarget"))
            {
                grappleTarget = hit.point;
                grappled = true;
            }
        }
    }
}

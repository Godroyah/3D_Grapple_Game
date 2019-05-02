﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamLooker : MonoBehaviour
{
    public float xClamp;
    public float rotSpeed;
    public Transform Target, Player;
    [SerializeField]
    float mouseX, mouseY;
    float offset;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        //if (this.transform.rotation.z == -180.0f || this.transform.rotation.z == 180.0f)
        //{
        //    thisCam.rotation = new Quaternion (thisCam.rotation.x, thisCam.rotation.y, 0.0f, thisCam.rotation.w);
        //}
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    void CameraRotation()
    {

        mouseX = Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime;

        xClamp += mouseY;

        if (xClamp > 50.0f)
        {
            xClamp = 50.0f;
            mouseY = 0.0f;
            ClampXAxis(205.0f);
            transform.localRotation = Quaternion.identity;
        }

        else if (xClamp < -45.0f)
        {
            xClamp = -45.0f;
            mouseY = 0.0f;
            ClampXAxis(45.0f);
            transform.localRotation = Quaternion.identity;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.Rotate(Vector3.left * mouseY);
        }
        else
        {
            Player.Rotate(Vector3.up * mouseX);
            Target.transform.Rotate(Vector3.left * mouseY);
        }

    }

    private void ClampXAxis(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }

}

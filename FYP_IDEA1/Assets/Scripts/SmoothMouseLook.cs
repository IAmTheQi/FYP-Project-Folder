﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[AddComponentMenu("Camera-Control/Smooth Mouse Look")]
public class SmoothMouseLook : MonoBehaviour
{
    GameObject parentObject;
    GameObject gameController;

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX;
    public float sensitivityY;

    public float minimumX;
    public float maximumX;

    public float minimumY;
    public float maximumY;

    float multiplier;

    float rotationX;
    float rotationY;

    private List<float> rotArrayX = new List<float>();
    float rotAverageX = 0F;

    private List<float> rotArrayY = new List<float>();
    float rotAverageY = 0F;

    public float frameCounter;

    public Quaternion originalRotation;

    void Awake()
    {
        gameController = GameObject.Find("GameController");
        parentObject = GameObject.Find("PlayerController");

        sensitivityX = 5f;
        sensitivityY = 5f;

        frameCounter = 5f;

        multiplier = 1.0f;

        if (SceneManager.GetActiveScene().name == "Level1")
        {
            rotationX = 90f;
        }
        else if (SceneManager.GetActiveScene().name == "Level2")
        {
            rotationX = 180f;
        }

        rotationY = 0f;
    }

    void Update()
    {
        if (parentObject.GetComponent<PlayerLogic>().pauseGame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (!parentObject.GetComponent<PlayerLogic>().IsDead())
        {
            if (parentObject.GetComponent<PlayerLogic>().aimDownSight)
            {
                multiplier = 0.1f;
            }
            else if (!parentObject.GetComponent<PlayerLogic>().aimDownSight)
            {
                multiplier = 1.0f;
            }
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (axes == RotationAxes.MouseXAndY)
            {
                rotAverageY = 0f;
                rotAverageX = 0f;

                rotationY += Input.GetAxis("Mouse Y") * sensitivityY * multiplier;
                rotationX += Input.GetAxis("Mouse X") * sensitivityX * multiplier;

                rotArrayY.Add(rotationY);
                rotArrayX.Add(rotationX);

                if (rotArrayY.Count >= frameCounter)
                {
                    rotArrayY.RemoveAt(0);
                }
                if (rotArrayX.Count >= frameCounter)
                {
                    rotArrayX.RemoveAt(0);
                }

                for (int j = 0; j < rotArrayY.Count; j++)
                {
                    rotAverageY += rotArrayY[j];
                }
                for (int i = 0; i < rotArrayX.Count; i++)
                {
                    rotAverageX += rotArrayX[i];
                }

                rotAverageY /= rotArrayY.Count;
                rotAverageX /= rotArrayX.Count;

                rotAverageY = ClampAngle(rotAverageY, minimumY, maximumY);
                rotAverageX = ClampAngle(rotAverageX, minimumX, maximumX);

                Quaternion yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);
                Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);

                transform.localRotation = originalRotation * yQuaternion;
                parentObject.transform.localRotation = originalRotation * xQuaternion;
            }
        }
    }

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb)
            rb.freezeRotation = true;
        originalRotation = transform.localRotation;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        angle = angle % 360;
        if ((angle >= -360F) && (angle <= 360F))
        {
            if (angle < -360F)
            {
                angle += 360F;
            }
            if (angle > 360F)
            {
                angle -= 360F;
            }
        }
        return Mathf.Clamp(angle, min, max);
    }
}
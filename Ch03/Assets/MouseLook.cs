using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour {

    public enum RotationAxes {
        MouseXAndY = 0,
        MouseX = 1,
        MouseY = 2
    }

    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityHor = 9.0f;
    public float sensitivityVert = 9.0f;
    public float minVertAngle = -45.0f;
    public float maxVertAngle = 45.0f;

    private float rotX = 0;
    private Vector3 pitchRot = new Vector3();

    void Start () {
        // Disallow physics rotation on player
        Rigidbody body = GetComponent<Rigidbody>();
        if (null != body) {
            body.freezeRotation = true;
        }
	}
	
	void Update () {
		if (axes == RotationAxes.MouseX) {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityHor, 0);
            Debug.Log(Input.GetAxis("Mouse X"));
        }
        else if (axes == RotationAxes.MouseY) {
            rotX -= Input.GetAxis("Mouse Y") * sensitivityVert;
            rotX = Mathf.Clamp(rotX, minVertAngle, maxVertAngle);

            pitchRot.x = rotX;
            pitchRot.y = transform.localEulerAngles.y;

            transform.localEulerAngles = pitchRot;
        }
        else {
            rotX -= Input.GetAxis("Mouse Y") * sensitivityVert;
            rotX = Mathf.Clamp(rotX, minVertAngle, maxVertAngle);

            float delta = Input.GetAxis("Mouse X") * sensitivityHor;

            pitchRot.x = rotX;
            pitchRot.y = transform.localEulerAngles.y + delta;

            transform.localEulerAngles = pitchRot;
        }
	}
}

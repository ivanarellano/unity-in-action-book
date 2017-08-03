using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSInput : MonoBehaviour {

    public float speed = 6.0f;
    public float gravity = -9.8f;

    private CharacterController charController;

	void Start () {
        charController = GetComponent<CharacterController>();
	}
	
	void Update () {
        float deltaX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float deltaZ = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        Vector3 movement = new Vector3(deltaX, 0, deltaZ);
        movement = Vector3.ClampMagnitude(movement, speed);
        movement.y = gravity;

        movement = transform.TransformDirection(movement);
        charController.Move(movement);
    }
}

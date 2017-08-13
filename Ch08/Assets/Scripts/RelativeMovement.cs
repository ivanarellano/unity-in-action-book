using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class RelativeMovement : MonoBehaviour {

    [SerializeField] private Transform target;

    public float pushForce = 3.0f;

    public float moveSpeed = 6.0f;
    public float rotSpeed = 15.0f;

    public float jumpSpeed = 15.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10.0f;
    public float minFall = -1.5f;

    private CharacterController charController;
    private ControllerColliderHit contact;
    private Animator animator;
    private float vertSpeed;

    void Start() {
        charController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        vertSpeed = minFall;
    }

    void Update () {
        Vector3 movement = Vector3.zero;

        float horInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        if (horInput != 0 || vertInput != 0) {
            movement.x = horInput * moveSpeed;
            movement.z = vertInput * moveSpeed;
            movement = Vector3.ClampMagnitude(movement, moveSpeed);

            Quaternion tmpRot = target.rotation;
            target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
            movement = target.TransformDirection(movement);
            target.rotation = tmpRot;

            /// Replaced with smooth rotation by using Lerp
            /// Removing lerp and using only this line will cause snap rotations
            // transform.rotation = Quaternion.LookRotation(movement);

            Quaternion direction = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * Time.deltaTime);
        }

        animator.SetFloat("speed", movement.sqrMagnitude);

        bool hitGround = false;
        RaycastHit hit;
        if (vertSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out hit)) {
            /// charController.height = the height without the rounded ends
            /// charController.radius = the rounded ends
            /// divide (c.h + c.r) in half because the ray begins at the middle of the character
            /// but use 1.9f to extend the distance check to just beyond the feet
            float check = (charController.height + charController.radius) / 1.9f;
            hitGround = hit.distance <= check;
        }

        if (hitGround) {
            if (Input.GetButtonDown("Jump")) {
                vertSpeed = jumpSpeed;
            } else {
                vertSpeed = -0.1f;
                animator.SetBool("jumping", false);
            }
        } else {
            vertSpeed += gravity * 5 * Time.deltaTime;
            if (vertSpeed < terminalVelocity) {
                vertSpeed = terminalVelocity;
            }

            // don't trigger this at the beggining of the level
            if (contact != null) {
                animator.SetBool("jumping", true);
            }

            if (charController.isGrounded) {
                /// Dot product of two vectors ranges between -1 and 1
                /// 1 meaning they point in exactly the same direction
                /// -1 when they point in exactly opposite
                if (Vector3.Dot(movement, contact.normal) < 0) {
                    /// nudge in collision normal direction
                    movement = contact.normal * moveSpeed;
                } else {
                    /// keep forward momentum going in collision normal direction
                    movement += contact.normal * moveSpeed;
                }
            }
        }
        movement.y = vertSpeed;

        movement *= Time.deltaTime;
        charController.Move(movement);
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        contact = hit;

        Rigidbody body = hit.collider.attachedRigidbody;
        if (body != null && !body.isKinematic) {
            body.velocity = hit.moveDirection * pushForce;
        }
    }
}

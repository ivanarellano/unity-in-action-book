using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PointClickMovement : MonoBehaviour{

    [SerializeField] private Transform target;

    public float pushForce = 3.0f;
    public float moveSpeed = 6.0f;
    public float rotSpeed = 15.0f;
    public float jumpSpeed = 15.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10.0f;
    public float minFall = -1.5f;
    public float deceleration = 25.0f;
    public float targetBuffer = 1.75f;

    private float curSpeed = 0f;
    private Vector3 targetPos = Vector3.one;
    private CharacterController charController;
    private ControllerColliderHit contact;
    private Animator animator;
    private float vertSpeed;

    void Start() {
        charController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        vertSpeed = minFall;
    }

    void Update() {
        Vector3 movement = Vector3.zero;

        /// Set target position on click
        if (Input.GetMouseButton(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;

            if (Physics.Raycast(ray, out mouseHit)) {
                targetPos = mouseHit.point;
                curSpeed = moveSpeed;
            }
        }

        /// Move if target position is set
        if (targetPos != Vector3.one) {
            Vector3 adjustedPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
            Quaternion targetRot = Quaternion.LookRotation(adjustedPos - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotSpeed * Time.deltaTime);

            movement = curSpeed * Vector3.forward;
            movement = transform.TransformDirection(movement);

            /// Decelerate to 0 when close to target
            if (Vector3.Distance(targetPos, transform.position) < targetBuffer) {
                curSpeed -= deceleration * Time.deltaTime;
                if (curSpeed <= 0) {
                    targetPos = Vector3.one;
                }
            }
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

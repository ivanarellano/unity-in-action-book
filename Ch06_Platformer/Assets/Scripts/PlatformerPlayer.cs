using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerPlayer : MonoBehaviour {

    public float speed = 250.0f;
    public float jumpForce = 12.0f;

    private Rigidbody2D body;
    private Animator animator;
    private BoxCollider2D box;

	void Start () {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();
	}
	
	void Update () {
        float deltaX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        Vector2 movement = new Vector2(deltaX, body.velocity.y);
        body.velocity = movement;

        Vector3 max = box.bounds.max;
        Vector3 min = box.bounds.min;
        Vector2 corner1 = new Vector2(max.x, min.y - .1f);
        Vector2 corner2 = new Vector2(min.x, min.y - .2f);
        Collider2D hit = Physics2D.OverlapArea(corner1, corner2);

        /// To visualize in Game view, enable Gizmos
        // Debug.DrawLine(corner1, corner2, Color.red, 2.0f);

        bool grounded = hit != null;

        body.gravityScale = grounded && deltaX == 0 ? 0 : 1;
        if (grounded && Input.GetKeyDown(KeyCode.Space)) {
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        MovingPlatform platform = null;
        if (grounded) {
            platform = hit.GetComponent<MovingPlatform>();
        }

        transform.parent = platform != null ? platform.transform : null;

        animator.SetFloat("speed", Mathf.Abs(deltaX));

        Vector3 pScale = Vector3.one;
        if (platform != null) {
            pScale = platform.transform.localScale;
        }

        // x is -1 or 1 based on deltaX
        transform.localScale = new Vector3(
            Mathf.Sign(deltaX) / pScale.x, 1 / pScale.y, 1);
	}
}

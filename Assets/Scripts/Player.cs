using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] private int speed;
    [SerializeField] private int jumpingPower;

    private bool isFacingRight;

    private Transform groundCheck;
    private int extraJumps;

    float horizontal;

    Rigidbody2D rb;



    private void Start() {
        Components();
    }

    private void Update() {
        Inputs();
    }
    private void FixedUpdate() {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }
    private void Components() {
        rb = GetComponent<Rigidbody2D>();
        groundCheck = gameObject.transform.GetChild(1).GetComponent<Transform>();
    }

    private void Inputs() {

        Flip();

        horizontal = Input.GetAxisRaw("Horizontal");


        if (IsGrounded()) {
            extraJumps = 2;
        }
        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0) {
            rb.velocity = Vector2.up * jumpingPower;
            extraJumps--;
        } else if (Input.GetKeyDown(KeyCode.Space) && extraJumps == 0 && IsGrounded()) {
            rb.velocity = Vector2.up * jumpingPower;
        }

    }

    private bool IsGrounded() {
        int layerMask = 1 << 8;
        return Physics2D.OverlapCircle(groundCheck.position, .1f, layerMask);
    }

    private void Flip() {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f) {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}

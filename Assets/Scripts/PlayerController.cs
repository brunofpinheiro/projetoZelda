using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private CharacterController controller;
    private Animator animator;

    [Header("Config Player")]
    public float movementSpeed = 3f;

    private Vector3 direction;
    private bool isWalk;

    float horizontal;
    float vertical;

    // Start is called before the first frame update
    void Start() {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        Inputs();
        MoveCharacter();
        UpdateAnimator();
    }

    void Inputs() {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Fire1")) {
            animator.SetTrigger("Attack");
        }
    }

    void MoveCharacter() {
        direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude > 0.1f) {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);
            isWalk = true;
        } else {
            isWalk = false;
        }
        
        controller.Move(direction * movementSpeed * Time.deltaTime);
    }
    
    void UpdateAnimator() {
        animator.SetBool("isWalk", isWalk);
    }
}

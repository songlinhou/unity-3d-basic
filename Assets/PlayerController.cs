using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private CharacterController characterController;
    private Transform camTransform;

    [SerializeField] float maxSpeed = 3f;
    [SerializeField] float movementDamper = 0.1f;

    [SerializeField] float rotateSpeed = 3f;

    [SerializeField] float jumpSpeed = 30f;

    private float ySpeed;

    private float originalStepOffset;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        camTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); // 0 - 1
        float vertical = Input.GetAxis("Vertical"); // 0 - 1

        Vector3 movementDirection = new Vector3(horizontal, 0, vertical);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude); // 0 - 1
        if (Input.GetKey(KeyCode.LeftShift))
        {
            inputMagnitude = inputMagnitude / 2;
        }
        // animator
        // animator.SetFloat("movement", inputMagnitude);

        animator.SetFloat("movement", inputMagnitude, movementDamper, Time.deltaTime);
        float speed = inputMagnitude * maxSpeed;
        // animator.speed = speed;

        movementDirection = Quaternion.AngleAxis(camTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.Normalize(); // L2-norm = 1

        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (characterController.isGrounded)
        {
            // characterController.stepOffset = originalStepOffset;
            ySpeed = -0.5f;
            // ySpeed = 0f;
            animator.SetBool("isGround", true);

            // Input.GetKeyDown(KeyCode.Space)
            if (Input.GetButtonDown("Jump"))
            {
                ySpeed = jumpSpeed;
                animator.SetTrigger("jump");
            }
        }
        else
        {
            // characterController.stepOffset = 0;
            animator.SetBool("isGround", false);

        }




        // Vector3 velocity = movementDirection * speed; // L2-norm (0 - 1) / (0 - 0.5)
        // float ySpeed = characterController.velocity.y;
        // ySpeed += Physics.gravity.y * Time.deltaTime;
        // velocity = new Vector3(velocity.x, ySpeed, velocity.z);
        // characterController.Move(velocity * Time.deltaTime);

        if (movementDirection != Vector3.zero)
        {
            // during movement
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection, Vector3.up); // how the player should rotate
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }

        if (characterController.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger("jump");
            }
        }

    }

    void OnAnimatorMove()
    {
        Vector3 deltaPosition = animator.deltaPosition;
        deltaPosition.y = ySpeed * Time.deltaTime;
        characterController.Move(deltaPosition);
        // characterController.Move(velocity);
    }
}

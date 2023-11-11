using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JillMovement : MonoBehaviour
{

    /* -------------------------------------------------------------------------- */
    /*                       headers para inspector de Unity                      */
    /* -------------------------------------------------------------------------- */

    //^ ------------------------------- movimiento ------------------------------- */
    [Header("Movimiento")]
    public CharacterController controller;
    public float speed = 10.0f;
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    public float jumpSpeed = 6.0f;

    public Transform cam;
    private Vector3 jumpForce = Vector3.zero;
    public float coyoteTime = 0.2f;
    private float coyoteCurrent = 0;
    public float gravity = 10.0f;


    // ^ ------------------------------- animaciones ------------------------------ */
    [Header("Animaciones")]
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        // * Llenamos el animador con el objeto del unity
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        caminar();
        saltar();
    }

    private void caminar()
    {
        var H_axis = Input.GetAxis("Horizontal");
        var V_axis = Input.GetAxis("Vertical");
        var dir = new Vector3(H_axis, 0, V_axis).normalized;

        if (dir.magnitude >= 0.1f)
        {

            animator.SetBool("caminar", true);
            animator.SetBool("idle", false);

            var camForward = cam.transform.forward;
            var camRight = cam.transform.right;

            camForward.y = 0;
            camRight.y = 0;

            camForward.Normalize();
            camRight.Normalize();

            var moveDir = (camForward * V_axis + camRight * H_axis).normalized;

            if (moveDir != Vector3.zero)
            {
                // Rotate the character to the direction of movement
                var targetAngle = (Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg) + cam.eulerAngles.y;
                var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, turnSmoothTime * Time.deltaTime);
                var moveDir2 = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir2 * speed * Time.deltaTime);
            }
        }
        else
        {
            animator.SetBool("caminar", false);
            animator.SetBool("idle", true);
        }
    }

    private void saltar()
    {
        bool wasGrounded = controller.isGrounded;

        // Obtenemos el saltar
        var jump = Input.GetButtonDown("Jump");
        if ((controller.isGrounded || coyoteCurrent <= coyoteTime) && jump)
        {
            jumpForce.y = jumpSpeed;
            animator.SetTrigger("saltar");
        }
        else
        {
            animator.ResetTrigger("saltar");
        }

        animator.SetBool("idle", controller.isGrounded);
        coyoteCurrent = 0;

        // Realizar el salto
        if (!wasGrounded)
        {
            coyoteCurrent += Time.deltaTime;
            jumpForce.y -= gravity * Time.deltaTime;
        }

        // Realizar el salto
        if (wasGrounded && jump)
        {
            jumpForce.y = jumpSpeed;
            animator.SetTrigger("saltar");
        }
        else if (wasGrounded)
        {
            // Asegurarse de que el trigger se resetee solo si estás en el suelo
            animator.ResetTrigger("saltar");
        }


        if (controller.isGrounded && jumpForce.y < 0)
        {
            jumpForce.y = -2f;
            coyoteCurrent = 0;
        }

        controller.Move(jumpForce * Time.deltaTime);
    }

}

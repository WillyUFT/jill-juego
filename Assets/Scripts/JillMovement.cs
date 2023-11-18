using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JillMovement : MonoBehaviour
{

    //* -------------------------------------------------------------------------- */
    //*                       headers para inspector de Unity                      */
    //* -------------------------------------------------------------------------- */

    public GameManager gameManager;

    //^ ------------------------------- movimiento ------------------------------- */
    [Header("Movimiento")]
    public CharacterController controller;
    public float velocidadMovimiento = 5.0f;
    public float gravedad = -9.81f;
    private Vector3 jumpForce = Vector3.zero;
    public float jumpSpeed = 2.0f;
    public float coyoteTime = 0.2f;
    private readonly float coyoteCurrent = 0;
    public float alturaJill = 2f;

    // ^ --------------------------------- Cámara --------------------------------- */
    [Header("Cámara")]
    public Transform camara;

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
        fumar();
    }

    private void caminar()
    {

        //& -------------------------- Obtenemos los inputs -------------------------- */
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movimiento = Vector3.zero;
        float velocidadAnimaciones = 0;

        if (horizontal != 0 || vertical != 0)
        {

            // Prendemos la animación de caminar
            animator.SetBool("caminar", true);
            animator.SetBool("idle", false);

            // Acá buscamos hacia donde está mirando la cámara
            Vector3 adelanteCamara = camara.forward;
            // Para que la cámar no mire al piso
            adelanteCamara.y = 0;
            adelanteCamara.Normalize();
            // Hacemos lo mismo pero con el horizontal
            Vector3 derechaCamara = camara.right;
            derechaCamara.y = 0;
            derechaCamara.Normalize();

            // Movemos al personaje
            Vector3 direccion = adelanteCamara * vertical + derechaCamara * horizontal;
            velocidadAnimaciones = Mathf.Clamp01(direccion.magnitude);
            direccion.Normalize();
            movimiento = direccion * velocidadMovimiento * velocidadAnimaciones * Time.deltaTime;

            // Rotamos al personaje en la dirección a la que avanzamos
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direccion), 0.1f);

        }
        else
        {
            // Prendemos la animación del idle
            animator.SetBool("idle", true);
            animator.SetBool("caminar", false);
        }

        // ! Aplicamos gravedad
        movimiento.y += gravedad * Time.deltaTime;
        // Se reinicia la gravedad en caso de que estemos en el suelo
        if (controller.isGrounded && movimiento.y < 0)
        {
            movimiento.y = 0;
        }


        // * Finalmente nos movemos
        controller.Move(movimiento);
    }

    private void saltar()
    {
        var jump = Input.GetButtonDown("Jump");

        if (jump && IsGrounded())
        {
            jumpForce.y += Mathf.Sqrt(jumpSpeed * -3.0f * gravedad);

            animator.SetTrigger("saltar");
        }

        jumpForce.y += gravedad * Time.deltaTime;
        controller.Move(jumpForce * Time.deltaTime);
    }

    private bool IsGrounded()
    {
        RaycastHit hit;
        alturaJill = 0.1f;
        Vector3 dir = new Vector3(0, -1);

        if (Physics.Raycast(transform.position, dir, out hit, alturaJill))
        {
            return hit.collider != null;
        }
        return false;
    }

    private void fumar() {
        
        var fumar = Input.GetButtonDown("Fumar");
    
        if (fumar) {
            gameManager.perderVida();
        }
    }

}

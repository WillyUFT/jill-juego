using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JillMovement : MonoBehaviour
{
    //* -------------------------------------------------------------------------- */
    //*                       headers para inspector de Unity                      */
    //* -------------------------------------------------------------------------- */

    public GameManager gameManager;
    public Rigidbody rb;

    //^ ------------------------------- movimiento ------------------------------- */
    [Header("Movimiento")]
    public CharacterController controller;
    public float velocidadMovimiento = 5.0f;
    public float gravedad = -9.81f;
    private Vector3 jumpForce = Vector3.zero;
    public float jumpSpeed = 2.0f;
    public float coyoteTime = 0.2f;
    public float alturaJill = 2f;
    public float tiempoEnElAire = 0;
    public float empujeFuerza = 15f;

    // ^ --------------------------------- Combate -------------------------------- */
    private bool isPegando = false;
    private bool isFumando = false;

    // ^ --------------------------------- Cámara --------------------------------- */
    [Header("Cámara")]
    public Transform camara;

    // ^ ------------------------------- animaciones ------------------------------ */
    [Header("Animaciones")]
    public Animator animator;

    // ^ --------------------------------- Rebote --------------------------------- */
    [Header("Rebote")]
    [SerializeField]
    private float velocidadRebote;

    [Header("Sonidos")]
    [SerializeField]
    private AudioClip salto;

    // Start is called before the first frame update
    void Start()
    {
        // * Llenamos el animador con el objeto del unity
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        caminar();
        pegar();
        fumar();
    }

    private void caminar()
    {
        // bool jump = Input.GetButtonDown("Jump");
        bool jump = false;

        // Obtenemos los inputs para el movimiento y el salto
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movimiento = Vector3.zero;
        float velocidadAnimaciones = 0;

        // Manejando el movimiento
        if (horizontal != 0 || vertical != 0)
        {
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
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direccion),
                0.1f
            );

            //    Prendemos la animación de caminar
            animator.SetBool("caminar", true);
            animator.SetBool("idle", false);
        }
        else
        {
            animator.SetBool("idle", true);
            animator.SetBool("caminar", false);
        }

        // * Manejando el salto
        if (jump && IsGrounded())
        {
            jumpForce.y = Mathf.Sqrt(jumpSpeed * -2.0f * gravedad);
            tiempoEnElAire = 0; // Reseteamos el contador cuando estamos saltamos
            animator.SetTrigger("saltar");
            Sonidos.Instance.EjecutarSonido(salto);
        }

        if (!IsGrounded())
        {
            tiempoEnElAire += Time.deltaTime;
            jumpForce.y += gravedad * tiempoEnElAire * Time.deltaTime;
        }
        else if (controller.isGrounded && jumpForce.y < 0)
        {
            jumpForce.y = 0;
        }

        // Movimiento final (incluyendo salto)
        jumpForce.y += gravedad * Time.deltaTime;
        movimiento += jumpForce * Time.deltaTime;
        controller.Move(movimiento);
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

    private void pegar()
    {
        var pegar = Input.GetButtonDown("Pegar");

        if (pegar && !isPegando)
        {
            isPegando = true;
            animator.SetTrigger("pegar");
        }
    }

    public void TerminarPegar()
    {
        isPegando = false;
    }

    private void fumar()
    {
        var fumar = Input.GetButtonDown("Fumar");

        if (fumar && !isFumando)
        {
            if (gameManager.ganarVida())
            {
                isFumando = true;
                animator.SetTrigger("fumar");
            }
            else
            {
                Debug.Log("No tenemos cigarros, F");
            }
        }
    }

    public void RecibirDano()
    {
        gameManager.perderVida();
    }

    public void RecibirDanoLetal()
    {
        gameManager.oneShot();
        Destroy(gameObject);
    }

    public void TerminaFumar()
    {
        isFumando = false;
    }

    //* ------------------ para cuando choquemos con una botella ----------------- */
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "enemigo")
        {
            // Obtener la dirección del empuje
            Vector3 pushDirection = collision.transform.position - transform.position;
            pushDirection = pushDirection.normalized;

            // Aplicar una fuerza al jugador
            collision.gameObject
                .GetComponent<Rigidbody>()
                .AddForce(pushDirection * empujeFuerza, ForceMode.Impulse);
        }
    }

    public void Rebote()
    {
        jumpForce.y = velocidadRebote;
    }
}

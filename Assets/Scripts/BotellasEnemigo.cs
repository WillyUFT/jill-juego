using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotellasEnemigo : MonoBehaviour
{
    [SerializeField]
    private JillMovement jill;

    [Header("Pared")]
    [SerializeField]
    private float distanciaPared;

    [SerializeField]
    private Transform controladorPared;
    private Vector3 direccionParedControlador;

    [SerializeField]
    private bool comenzoDerecha;

    [Header("Movimiento")]
    [SerializeField]
    private float velocidadMovimiento;

    // * Esto es para que veamos hacia donde patrulla
    // * (izquierda-derecha) o (atrás-adelante)
    [SerializeField]
    private bool patrullaHorizontal;
    private Rigidbody rb;

    //* ------------------------- para controlar el giro ------------------------- */
    private Quaternion targetRotation;
    private bool isRotating = false;
    public float rotationSpeed = 5f;
    private readonly float tiempoParaGirarNuevamente = 0.5f;
    private float ultimoGiro;

    private void Girar()
    {
        ultimoGiro = Time.time;
        comenzoDerecha = !comenzoDerecha;
        targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + 180, 0);
        isRotating = true;
        velocidadMovimiento *= -1;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector3 normalContacto = other.GetContact(0).normal;

            if (normalContacto.y <= -0.7)
            {
                other.gameObject.GetComponent<JillMovement>().Rebote();
                Destroy(gameObject);
            }
            else
            {
                jill.RecibirDanoLetal();
            }
        }
    }

    private void FixedUpdate()
    {
        if (isRotating)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            );

            if (Quaternion.Angle(transform.rotation, targetRotation) < 1)
            {
                isRotating = false;
                transform.rotation = targetRotation;
            }
        }

        if (comenzoDerecha)
        {
            direccionParedControlador = Vector3.right;
        }
        else
        {
            direccionParedControlador = Vector3.left;
        }

        RaycastHit infoPared;
        bool paredDetectada = Physics.Raycast(
            controladorPared.position,
            direccionParedControlador,
            out infoPared,
            distanciaPared
        );

        if (patrullaHorizontal)
        {
            rb.velocity = new Vector3(-velocidadMovimiento, rb.velocity.y, rb.velocity.z);
        }
        else
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -velocidadMovimiento);
        }

        if (
            paredDetectada
            && infoPared.collider.CompareTag("terreno")
            && Time.time > ultimoGiro + tiempoParaGirarNuevamente
        )
        {
            Girar();
        }
    }

    void OnDrawGizmos()
    {
        if (controladorPared == null)
            return;

        Gizmos.color = Color.green;
        if (comenzoDerecha)
        {
            Vector3 direccionPared = Vector3.right * distanciaPared;
            Gizmos.DrawLine(controladorPared.position, controladorPared.position + direccionPared);
        }
        else
        {
            Vector3 direccionPared = Vector3.left * distanciaPared;
            Gizmos.DrawLine(controladorPared.position, controladorPared.position + direccionPared);
        }
    }
}

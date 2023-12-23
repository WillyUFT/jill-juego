using UnityEngine;
using UnityEngine.AI;

public class RadSuperiorController : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameObject player;

    [SerializeField]
    private JillMovement jill;

    [SerializeField]
    private ParticleSystem particulas;

    [Header("Persecución")]
    [SerializeField]
    bool persecucionLinea;

    [SerializeField]
    private float rangoPersecucion = 10f;

    [SerializeField]
    private Transform controladorJugador;

    [SerializeField]
    private float distanciaJugador;
    public bool persiguiendo = false;

    [Header("Sonido")]
    [SerializeField]
    private AudioClip perro;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (persecucionLinea)
        {
            RaycastHit infoJugador;
            bool jugadorDetectado = Physics.Raycast(
                controladorJugador.position,
                controladorJugador.forward,
                out infoJugador,
                distanciaJugador
            );

            if (jugadorDetectado && infoJugador.collider.CompareTag("Player"))
            {
                persiguiendo = true;
            }
        }
        else
        {
            bool jugadorDetectado = Physics.CheckSphere(
                controladorJugador.position,
                distanciaJugador,
                LayerMask.GetMask("Player")
            );

            if (jugadorDetectado)
            {
                persiguiendo = true;
            }
        }

        if (persiguiendo)
        {
            agent.SetDestination(player.transform.position);
        }
    }

    void OnDrawGizmos()
    {
        if (persecucionLinea)
        {
            Gizmos.color = Color.red;
            Vector3 direccionRad = controladorJugador.forward * distanciaJugador;
            Gizmos.DrawLine(
                controladorJugador.position,
                controladorJugador.position + direccionRad
            );
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(controladorJugador.position, distanciaJugador);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector3 normalContacto = other.GetContact(0).normal;

            if (normalContacto.y <= -0.7)
            {
                other.gameObject.GetComponent<JillMovement>().Rebote();
                Instantiate(particulas, transform.position, Quaternion.identity);
                Sonidos.Instance.EjecutarSonido(perro);
                Destroy(gameObject);
            }
            else
            {
                jill.RecibirDano();
                other.gameObject.GetComponent<JillMovement>().Rebote();
                Instantiate(particulas, transform.position, Quaternion.identity);
                Sonidos.Instance.EjecutarSonido(perro);
                Destroy(gameObject);
            }
        }
    }
}

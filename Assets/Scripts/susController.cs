using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class susController : MonoBehaviour
{

    // ^ ------------------------------- animaciones ------------------------------ */
    [Header("Animaciones")]
    public Animator animator;

    [Header("Movimiento")]
    public Transform target;
    public float velocidadSus;


    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        caminar();


    }


    void caminar()
    {

        var caminar = Input.GetKey(KeyCode.T);

        if (caminar)
        {
            animator.SetBool("caminar", true);
            animator.SetBool("idle", false);

            float step = velocidadSus * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        } else
        {
            animator.SetBool("caminar", false);
            animator.SetBool("idle", true);
        }

    }

}

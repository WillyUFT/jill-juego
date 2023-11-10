using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class jillController : MonoBehaviour
{

    // * Acá vamos a crear los booleanos para realizar los movimientos
    public bool caminar = false;
    public bool idle = true;
    public bool bailar1 = false;
    public bool bailar2 = false;

    // * creamos el animador
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        // * Llenamos el animador con el objeto del unity
        animator = gameObject.GetComponent<Animator>();
    }

    // & Con esta función Jill va a caminar
    public void caminarButton()
    {

        salirLolikami2();
        apagarBooleanos();

        idle = false;
        bailar1 = false;
        bailar2 = false;

        if (!caminar)
        {
            caminar = true;
            animator.SetBool("caminar", true);
        }
        else
        {
            caminar = false;
            animator.SetBool("caminar", false);
        }

    }

    // & Función para realizar el primer baile de Jill
    public void bailar1Button()
    {
        salirLolikami2();
        apagarBooleanos();

        idle = false;
        caminar = false;
        bailar2 = false;

        if (!bailar1)
        {
            bailar1 = true;
            animator.SetBool("lolikami1", true);
        }
        else
        {
            bailar1 = false;
            animator.SetBool("lolikami1", false);
        }
    }

    // & Función para realizar el segundo baile de Jill
    public void bailar2Button()
    {
        apagarBooleanos();

        idle = false;
        caminar = false;
        bailar1 = false;

        if (!bailar2)
        {
            bailar2 = true;
            animator.SetBool("lolikami2", true);
        }
        else
        {
            bailar2 = false;
            animator.SetBool("lolikami2", false);
        }
    }

    public void idleButton()
    {
        salirLolikami2();
        apagarBooleanos();

        caminar = false;
        bailar1 = false;
        bailar2 = false;


        if (!idle)
        {
            idle = true;
            animator.SetBool("idle", true);
        }
        else
        {
            idle = false;
            animator.SetBool("idle", false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void apagarBooleanos()
    {
        animator.SetBool("idle", false);
        animator.SetBool("caminar", false);
        animator.SetBool("lolikami1", false);
        animator.SetBool("lolikami2", false);
    }

    private void salirLolikami2()
    {
        if (bailar2)
        {
            animator.SetBool("salirLolikami2", true);

        }
        else
        {
            animator.SetBool("salirLolikami2", false);
        }
    }
}

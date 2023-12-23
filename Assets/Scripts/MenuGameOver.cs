using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuGameOver : MonoBehaviour
{
    [SerializeField]
    private GameObject menuGameOver;
    [SerializeField] private GameManager gamema;
    public Image imagenHappy;
    public Sprite nuevaImagen;

    private void Start()
    {
        gamema.MuerteJugador += ActivarMenuGameOver;
    }

    private void ActivarMenuGameOver(object sender, EventArgs e)
    {
        menuGameOver.SetActive(true);
    }

    public void Reiniciar()
    {
        Debug.Log("Reiniciando...");
        imagenHappy.sprite = nuevaImagen;
        StartCoroutine(RetrasarReinicio());
    }

    private IEnumerator RetrasarReinicio()
    {
        yield return new WaitForSeconds(2); // Espera 1 segundo
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void aa()
    {
        Debug.Log("jasjdajdj");
    }
}

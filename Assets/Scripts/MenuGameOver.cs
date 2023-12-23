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

    [SerializeField]
    private GameManager gamema;
    public Image imagenHappy;
    public Sprite nuevaImagen;

    private void Start()
    {
        gamema.MuerteJugador += ActivarMenuGameOver;
    }

    private void ActivarMenuGameOver(object sender, EventArgs e)
    {
        menuGameOver.SetActive(true);
        StartCoroutine(SecuenciaReinicio());
    }

    private IEnumerator SecuenciaReinicio()
    {
        // Mostrar el menú de game over durante 1.5 segundos
        yield return new WaitForSeconds(1.5f);

        // Cambiar la imagen
        imagenHappy.sprite = nuevaImagen;

        // Esperar otros 1.5 segundos para mostrar la nueva imagen
        yield return new WaitForSeconds(1.5f);

        // Reiniciar la escena
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisionOk : MonoBehaviour
{
    [SerializeField]
    private GameObject menu;
    public float tiempoDeEspera = 2f;

    void Start()
    {
        StartCoroutine(DesactivarMenuDespuesDeTiempo(tiempoDeEspera));
    }

    private IEnumerator DesactivarMenuDespuesDeTiempo(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        DesactivarMenu();
    }

    private void DesactivarMenu()
    {
        Debug.Log("Desactivando menú...");
        menu.SetActive(false);
    }
}

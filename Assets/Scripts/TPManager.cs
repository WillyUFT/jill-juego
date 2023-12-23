using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TPManager : MonoBehaviour
{
    [SerializeField]
    private string nombreEscena;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("XDD");
            SceneManager.LoadScene(nombreEscena);
        }
    }
}

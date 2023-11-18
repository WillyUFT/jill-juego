using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cigarro : MonoBehaviour
{

    public GameManager gameManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.sumarCigarro();
            Destroy(this.gameObject);
        }
    }

}

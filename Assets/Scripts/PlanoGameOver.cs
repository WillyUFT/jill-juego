using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanoGameOver : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameManager != null)
            {
                gameManager.oneShot();
            }
            Destroy(other.gameObject);
        }
    }
}

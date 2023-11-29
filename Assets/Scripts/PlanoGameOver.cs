using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanoGameOver : MonoBehaviour
{
    public GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.FindAnyObjectByType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found");
        }
    }

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

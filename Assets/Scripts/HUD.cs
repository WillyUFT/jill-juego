using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{

    public GameManager gameManager;
    public TextMeshProUGUI vinoColoColoScore;
    public TextMeshProUGUI cigarroScore;
    public TextMeshProUGUI vidaScreen;

    public void actualizarVinoColoColoScore(int score)
    {
        vinoColoColoScore.text = score.ToString();
    }

    public void actualizarCigarroScore(int score)
    {
        cigarroScore.text = score.ToString();
    }

    public void actualizarVida(int vida) {
        vidaScreen.text = vida.ToString();
    }

}

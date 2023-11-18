using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int VinoColoColoScore { get { return vinoColoColoScore; } }
    private int vinoColoColoScore;

    public int CigarroScore { get { return cigarroScore; } }
    private int cigarroScore;

    public int vidaJill;

    public HUD hud;

    public void Start() {
        vidaJill = 100;
    }

    public void sumarVinoColoColo()
    {
        vinoColoColoScore += 1;
        hud.actualizarVinoColoColoScore(vinoColoColoScore);
    }

    public void sumarCigarro()
    {
        cigarroScore += 1;
        hud.actualizarCigarroScore(cigarroScore);
    }

    public void perderVida() {
        vidaJill -= 20;
        hud.actualizarVida(vidaJill);
    }

    public void ganarVida() {
        vidaJill += 40;
        hud.actualizarVida(vidaJill);
    }


}

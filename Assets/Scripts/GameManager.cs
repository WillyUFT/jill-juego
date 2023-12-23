using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int VinoColoColoScore
    {
        get { return vinoColoColoScore; }
    }
    private int vinoColoColoScore;

    public int CigarroScore
    {
        get { return cigarroScore; }
    }
    private int cigarroScore;

    public int vidaJill;

    public HUD hud;

    public event EventHandler MuerteJugador;

    [SerializeField]
    private AudioClip morir;

    [SerializeField]
    private AudioClip dmg;

    public void Start()
    {
        vidaJill = 40;
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

    public void perderVida()
    {
        if (vidaJill - 20 <= 0)
        {
            Sonidos.Instance.EjecutarSonido(morir);
            vidaJill = 0;
        }
        else
        {
            Sonidos.Instance.EjecutarSonido(dmg);
            vidaJill -= 20;
        }

        if (vidaJill <= 0)
        {
            MuerteJugador?.Invoke(this, EventArgs.Empty);
        }

        hud.actualizarVida(vidaJill);
    }

    // * -------------------- Para morir de golpe, oneshoteado -------------------- */
    public void oneShot()
    {
        vidaJill = 0;
        Sonidos.Instance.EjecutarSonido(morir);
        hud.actualizarVida(vidaJill);
        MuerteJugador?.Invoke(this, EventArgs.Empty);
    }

    //* -------------------- Esta es la función para curarnos -------------------- */
    public bool ganarVida()
    {
        if (cigarroScore > 1 && vidaJill != 100)
        {
            if (vidaJill + 40 >= 100)
            {
                vidaJill = 100;
            }
            else
            {
                vidaJill += 40;
            }
            hud.actualizarVida(vidaJill);
            cigarroScore -= 1;
            hud.actualizarCigarroScore(cigarroScore);
            return true;
        }
        else
        {
            return false;
        }
    }
}

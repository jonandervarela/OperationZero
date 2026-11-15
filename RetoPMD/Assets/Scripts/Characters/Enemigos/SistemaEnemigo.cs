
using UnityEngine;

public class SistemaEnemigo : MonoBehaviour
{
    [Header("Vida del enemigo")]
    public int vidaMaxima = 100;
    public int vidaActual;

    [Header("Daño")]
    public int danoAtaque = 15;

    private Animator animator;
    private bool muerto = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        vidaActual = vidaMaxima;
    }

    // Recibir daño de los proyectiles de ZERO.
    public void RecibirDano(int cantidad)
    {
        if (muerto)
            return;

        vidaActual -= cantidad;
        vidaActual = Mathf.Max(vidaActual, 0);

        Debug.Log("Enemigo recibe daño. Vida: " + vidaActual);

        if (vidaActual <= 0)
        {
            EjecutarMuerte();
        }
        else if (animator != null)
        {
            animator.SetTrigger("RecibirDano");
        }
    }

    // Ejecutar la animación de muerte.
    private void EjecutarMuerte()
    {
        muerto = true;

        if (animator != null)
            animator.SetTrigger("Morir");

        Debug.Log("¡Enemigo eliminado!");
    }

    public bool EstaVivo()
    {
        return !muerto;
    }
}

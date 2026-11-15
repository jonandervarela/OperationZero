
using UnityEngine;
using UnityEngine.UI;

public class SistemaVida : MonoBehaviour
{
    [Header("Vida de ZERO")]
    public int vidaMaxima = 100;
    public int vidaActual = 100;

    [Header("Segmentos de vida")]
    public Image[] segmentosVida;
    public Sprite segmentoLleno;
    public Sprite segmentoVacio;

    [Header("Retratos de ZERO")]
    public Image retratoZero;
    public Sprite retratoNormal;
    public Sprite retratoHerido;
    public Sprite retratoCritico;

    void Start()
    {
        vidaActual = vidaMaxima;
        ActualizarHUD();
    }

    // Quita vida cuando ZERO recibe un golpe.
    public void RecibirDano(int cantidad)
    {
        vidaActual -= cantidad;

        // Evitamos que la vida sea menor que cero.
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);

        ActualizarHUD();
    }

    // Recupera vida al utilizar un botiquín.
    public void Curar(int cantidad)
    {
        vidaActual += cantidad;

        // Evitamos superar la vida máxima.
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);

        ActualizarHUD();
    }

    // Actualiza los segmentos y el retrato.
    void ActualizarHUD()
    {
        int segmentosActivos = Mathf.CeilToInt(
            (float)vidaActual / vidaMaxima * segmentosVida.Length
        );

        for (int i = 0; i < segmentosVida.Length; i++)
        {
            if (i < segmentosActivos)
                segmentosVida[i].sprite = segmentoLleno;
            else
                segmentosVida[i].sprite = segmentoVacio;
        }

        ActualizarRetrato();
    }

    // Cambia el retrato según la salud restante.
    void ActualizarRetrato()
    {
        float porcentajeVida = (float)vidaActual / vidaMaxima;

        if (porcentajeVida > 0.60f)
            retratoZero.sprite = retratoNormal;

        else if (porcentajeVida > 0.30f)
            retratoZero.sprite = retratoHerido;

        else
            retratoZero.sprite = retratoCritico;
    }
}

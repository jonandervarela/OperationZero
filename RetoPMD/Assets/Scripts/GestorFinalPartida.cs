

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GestorFinalPartida : MonoBehaviour
{
    [Header("Paneles finales")]
    public GameObject panelVictoria;
    public GameObject panelDerrota;

    [Header("Personajes")]
    public AccionesZero jugador;
    public SistemaEnemigo enemigo;

    [Header("Tiempo de espera")]
    public float retrasoPantallaFinal = 1.5f;

    private bool partidaTerminada = false;

    void Start()
    {
        // Ocultar las pantallas al comenzar.
        panelVictoria.SetActive(false);
        panelDerrota.SetActive(false);

        Time.timeScale = 1f;
    }

    void Update()
    {
        if (partidaTerminada)
            return;

        // Comprobar si ZERO ha muerto.
        if (jugador != null && jugador.EstaMuerto())
        {
            partidaTerminada = true;
            StartCoroutine(EsperarDerrota());
            return;
        }

        // Comprobar si el SWAT ha muerto.
        if (enemigo != null && !enemigo.EstaVivo())
        {
            partidaTerminada = true;
            StartCoroutine(EsperarVictoria());
        }
    }

    // Esperar antes de mostrar la derrota.
    IEnumerator EsperarDerrota()
    {
        yield return new WaitForSeconds(retrasoPantallaFinal);

        panelDerrota.SetActive(true);

        Time.timeScale = 0f;

        Debug.Log("MISIÓN FALLIDA.");
    }

    // Esperar antes de mostrar la victoria.
    IEnumerator EsperarVictoria()
    {
        yield return new WaitForSeconds(retrasoPantallaFinal);

        panelVictoria.SetActive(true);

        Time.timeScale = 0f;

        Debug.Log("¡MISIÓN COMPLETADA!");
    }

    // Volver al menú principal.
    public void VolverAlMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MenuPrincipal");
    }
}

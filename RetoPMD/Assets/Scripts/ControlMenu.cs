
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlMenu : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelBotones;
    public GameObject panelCreditos;
    public GameObject panelHistoria;

    void Start()
    {
        // Mostrar los botones principales
        panelBotones.SetActive(true);

        // Ocultar los paneles secundarios
        panelCreditos.SetActive(false);
        panelHistoria.SetActive(false);
    }

    // INICIAR MISION
    public void IniciarMision()
    {
        SceneManager.LoadScene("Zona1_ControlSeguridad");
    }

    // CREDITOS
    public void AbrirCreditos()
    {
        panelBotones.SetActive(false);
        panelCreditos.SetActive(true);
    }

    public void CerrarCreditos()
    {
        panelCreditos.SetActive(false);
        panelBotones.SetActive(true);
    }

    // HISTORIA
    public void AbrirHistoria()
    {
        panelBotones.SetActive(false);
        panelHistoria.SetActive(true);
    }

    public void CerrarHistoria()
    {
        panelHistoria.SetActive(false);
        panelBotones.SetActive(true);
    }

    // SALIR
    public void Salir()
    {
        Application.Quit();
        Debug.Log("Cerrando el juego...");
    }
}

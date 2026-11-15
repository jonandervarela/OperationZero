
using UnityEngine;

public class ControlOpciones : MonoBehaviour
{
    public GameObject panelOpciones;

    void Start()
    {
        panelOpciones.SetActive(false);

        AudioListener.volume =
            PlayerPrefs.GetFloat("Volumen", 1f);
    }

    public void AbrirOpciones()
    {
        panelOpciones.SetActive(true);
    }

    public void CerrarOpciones()
    {
        panelOpciones.SetActive(false);
    }

    public void CambiarVolumen(float volumen)
    {
        AudioListener.volume = volumen;

        PlayerPrefs.SetFloat("Volumen", volumen);
        PlayerPrefs.Save();
    }
}

using UnityEngine;
using TMPro;

public class ObjetoRecogible : MonoBehaviour
{
    [Header("Texto de interacción")]
    public TextMeshPro textoRecoger;

    private bool jugadorCerca = false;

    void Start()
    {
        // Ocultar el mensaje al iniciar el juego
        if (textoRecoger != null)
        {
            textoRecoger.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Si ZERO está cerca y pulsa E, recoge el objeto
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            RecogerObjeto();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Comprobar si quien entra en el objeto es el jugador
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;

            if (textoRecoger != null)
            {
                textoRecoger.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Ocultar el mensaje cuando ZERO se aleja
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;

            if (textoRecoger != null)
            {
                textoRecoger.gameObject.SetActive(false);
            }
        }
    }

    private void RecogerObjeto()
    {
        Debug.Log("ZERO ha recogido el suministro");

        // Destruir el objeto y su texto
        Destroy(gameObject);
    }
}
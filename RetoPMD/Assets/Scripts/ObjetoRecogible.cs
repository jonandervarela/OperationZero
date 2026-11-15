
using System.Collections;
using UnityEngine;
using TMPro;

public class ObjetoRecogible : MonoBehaviour
{
    [Header("Interfaz")]
    public TMP_Text textoRecoger;

    [Header("Jugador")]
    public Animator animatorJugador;

    private InventarioZero inventario;

    private bool jugadorCerca = false;
    private bool recogiendo = false;

    void Start()
    {
        // Ocultar el mensaje al comenzar.
        if (textoRecoger != null)
        {
            textoRecoger.gameObject.SetActive(false);
        }

        // Buscar a ZERO por su etiqueta.
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");

        if (jugador != null)
        {
            // Obtener su Animator.
            if (animatorJugador == null)
            {
                animatorJugador = jugador.GetComponent<Animator>();
            }

            // Obtener su inventario.
            inventario = jugador.GetComponent<InventarioZero>();
        }
        else
        {
            Debug.LogError("No se encuentra al jugador con Tag Player.");
        }
    }

    void Update()
    {
        // Comprobar si ZERO está cerca y pulsa E.
        if (jugadorCerca && !recogiendo && Input.GetKeyDown(KeyCode.E))
        {
            RecogerObjeto();
        }
    }

    // Detectar cuando ZERO entra en el área del botiquín.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            MostrarTexto();
        }
    }

    // Mantener el mensaje visible mientras ZERO esté cerca.
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !recogiendo)
        {
            jugadorCerca = true;
            MostrarTexto();
        }
    }

    // Ocultar el mensaje cuando ZERO se aleja.
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;

            if (textoRecoger != null)
            {
                textoRecoger.gameObject.SetActive(false);
            }
        }
    }

    // Mostrar el mensaje de interacción.
    private void MostrarTexto()
    {
        if (textoRecoger != null && !recogiendo)
        {
            textoRecoger.gameObject.SetActive(true);
        }
    }

    // Iniciar la recogida del botiquín.
    private void RecogerObjeto()
    {
        recogiendo = true;

        // Ocultar el mensaje.
        if (textoRecoger != null)
        {
            textoRecoger.gameObject.SetActive(false);
        }

        // Desactivar el collider para evitar otra recogida.
        Collider2D colisionador = GetComponent<Collider2D>();

        if (colisionador != null)
        {
            colisionador.enabled = false;
        }

        // Activar la animación de ZERO.
        if (animatorJugador != null)
        {
            animatorJugador.SetTrigger("Recoger");
        }
        else
        {
            Debug.LogError("No se ha encontrado el Animator de ZERO.");
        }

        // Esperar a que termine la animación.
        StartCoroutine(FinalizarRecogida());
    }

    // Finalizar la recogida y guardar el botiquín.
    private IEnumerator FinalizarRecogida()
    {
        // Esperar la duración de la animación.
        yield return new WaitForSeconds(1f);

        // Añadir el botiquín al inventario.
        if (inventario != null)
        {
            inventario.AnadirBotiquin();
        }
        else
        {
            Debug.LogError("ZERO no tiene InventarioZero.");
        }

        // Eliminar el botiquín del escenario.
        Destroy(gameObject);
    }
}

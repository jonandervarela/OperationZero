
using UnityEngine;
using TMPro;

public class InventarioZero : MonoBehaviour
{
    [Header("Inventario")]
    public int botiquines = 0;

    [Header("Curacion")]
    public int curacionBotiquin = 30;

    [Header("Interfaz")]
    public TMP_Text textoCantidadBotiquines;

    private SistemaVida sistemaVida;

    void Start()
    {
        sistemaVida = GetComponent<SistemaVida>();

        // Actualizamos el contador al iniciar el juego.
        ActualizarHUDInventario();
    }

    void Update()
    {
        // Pulsar H para utilizar un botiquin.
        if (Input.GetKeyDown(KeyCode.H))
        {
            UsarBotiquin();
        }
    }

    // Añadir un botiquin al inventario.
    public void AnadirBotiquin()
    {
        botiquines++;

        Debug.Log("Botiquines guardados: " + botiquines);

        ActualizarHUDInventario();
    }

    // Utilizar un botiquin para recuperar vida.
    public void UsarBotiquin()
    {
        if (botiquines <= 0)
        {
            Debug.Log("No tienes botiquines.");
            return;
        }

        if (sistemaVida == null)
        {
            Debug.LogError("No se encuentra SistemaVida en ZERO.");
            return;
        }

        if (sistemaVida.vidaActual >= sistemaVida.vidaMaxima)
        {
            Debug.Log("ZERO ya tiene la vida completa.");
            return;
        }

        // Recuperamos vida.
        sistemaVida.Curar(curacionBotiquin);

        // Consumimos el botiquin.
        botiquines--;

        Debug.Log("Botiquin utilizado. Quedan: " + botiquines);

        ActualizarHUDInventario();
    }

    // Actualizar el contador visual del inventario.
    private void ActualizarHUDInventario()
    {
        if (textoCantidadBotiquines != null)
        {
            textoCantidadBotiquines.text = "x" + botiquines;
        }
    }
}

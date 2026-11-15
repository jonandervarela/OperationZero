
using UnityEngine;

public class ProyectilZero : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidad = 12f;
    public int dano = 20;
    public float tiempoVida = 3f;

    private Vector2 direccion;

    void Start()
    {
        // Destruir la bala después de unos segundos.
        Destroy(gameObject, tiempoVida);
    }

    public void ConfigurarDireccion(Vector2 nuevaDireccion)
    {
        direccion = nuevaDireccion.normalized;
    }

    void Update()
    {
        // Mover la bala en la dirección indicada.
        transform.Translate(
            direccion * velocidad * Time.deltaTime,
            Space.World
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Evitar que la bala dañe a ZERO.
        if (other.CompareTag("Player"))
            return;

        // Buscar el sistema de vida del enemigo.
        SistemaEnemigo enemigo = other.GetComponentInParent<SistemaEnemigo>();

        if (enemigo != null)
        {
            enemigo.RecibirDano(dano);
            Destroy(gameObject);
        }
    }
}

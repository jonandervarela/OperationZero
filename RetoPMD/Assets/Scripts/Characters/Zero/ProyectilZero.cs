
using UnityEngine;

public class ProyectilZero : MonoBehaviour
{
    [Header("Proyectil")]
    public float velocidad = 12f;
    public int dano = 20;
    public float tiempoVida = 3f;

    private Vector2 direccion;

    void Start()
    {
        Destroy(gameObject, tiempoVida);
    }

    public void ConfigurarDireccion(Vector2 nuevaDireccion)
    {
        direccion = nuevaDireccion.normalized;
    }

    void Update()
    {
        transform.Translate(
            direccion * velocidad * Time.deltaTime,
            Space.World
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ignorar al propio jugador.
        if (other.CompareTag("Player"))
            return;

        // Ignorar los detectores de proximidad.
        if (other.isTrigger)
            return;

        // Buscar el sistema de vida del enemigo.
        SistemaEnemigo enemigo =
            other.GetComponentInParent<SistemaEnemigo>();

        if (enemigo != null)
        {
            // No dañar cadáveres.
            if (!enemigo.EstaVivo())
                return;

            enemigo.RecibirDano(dano);

            Destroy(gameObject);
        }
    }
}

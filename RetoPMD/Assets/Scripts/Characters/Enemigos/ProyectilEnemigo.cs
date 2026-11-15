
using UnityEngine;

public class ProyectilEnemigo : MonoBehaviour
{
    [Header("Proyectil")]
    public float velocidad = 8f;
    public int dano = 10;
    public float tiempoVida = 4f;

    private Vector2 direccion;
    private Collider2D[] collidersDisparador;

    void Start()
    {
        Destroy(gameObject, tiempoVida);
    }

    public void ConfigurarDisparo(Vector2 nuevaDireccion, Transform disparador)
    {
        direccion = nuevaDireccion.normalized;

        // Guardar los colliders del enemigo que dispara.
        collidersDisparador =
            disparador.GetComponentsInChildren<Collider2D>();

        // Evitar que la bala choque con su propio disparador.
        Collider2D[] collidersBala =
            GetComponents<Collider2D>();

        foreach (Collider2D bala in collidersBala)
        {
            foreach (Collider2D enemigo in collidersDisparador)
            {
                if (enemigo != null)
                {
                    Physics2D.IgnoreCollision(bala, enemigo, true);
                }
            }
        }
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
        // Ignorar los colliders del enemigo que dispara.
        if (collidersDisparador != null)
        {
            foreach (Collider2D enemigo in collidersDisparador)
            {
                if (other == enemigo)
                    return;
            }
        }

        // Ignorar detectores de proximidad.
        if (other.isTrigger)
            return;

        // Impactar contra ZERO.
        if (other.CompareTag("Player"))
        {
            AccionesZero acciones =
                other.GetComponentInParent<AccionesZero>();

            if (acciones != null)
            {
                acciones.RecibirDano(dano);
            }

            Destroy(gameObject);
            return;
        }

        // Desaparecer al golpear una pared u objeto físico.
        Destroy(gameObject);
    }
}

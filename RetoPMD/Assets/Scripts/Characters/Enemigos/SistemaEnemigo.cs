
using UnityEngine;

public class SistemaEnemigo : MonoBehaviour
{
    [Header("Vida")]
    public int vidaMaxima = 100;
    public int vidaActual;

    [Header("Daño")]
    public int danoAtaque = 15;

    [Header("Cadaver")]
    public float gravedadMuerte = 1f;
    public Vector2 tamanoColliderMuerte = new Vector2(1.7f, 0.9f);
    public Vector2 offsetColliderMuerte = new Vector2(0f, -0.3f);

    private Animator animator;
    private Rigidbody2D rb;
    private CapsuleCollider2D capsule;
    private Collider2D[] colliders;
    private DisparoEnemigo disparoEnemigo;

    private bool muerto = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        capsule = GetComponent<CapsuleCollider2D>();
        colliders = GetComponents<Collider2D>();

        disparoEnemigo = GetComponent<DisparoEnemigo>();
    }

    void Start()
    {
        vidaActual = vidaMaxima;
    }

    public void RecibirDano(int cantidad)
    {
        if (muerto) return;

        vidaActual -= cantidad;
        vidaActual = Mathf.Max(vidaActual, 0);

        Debug.Log("Enemigo recibe daño. Vida: " + vidaActual);

        if (vidaActual <= 0)
        {
            EjecutarMuerte();
        }
        else
        {
            // Cancelar cualquier disparo interrumpido por el daño.
            if (disparoEnemigo != null)
                disparoEnemigo.CancelarDisparo();

            if (animator != null)
            {
                animator.ResetTrigger("Disparar");
                animator.SetTrigger("RecibirDano");
            }
        }
    }

    private void EjecutarMuerte()
    {
        if (muerto) return;

        muerto = true;

        Debug.Log("¡Enemigo eliminado!");

        if (disparoEnemigo != null)
            disparoEnemigo.CancelarDisparo();

        if (animator != null)
        {
            animator.ResetTrigger("RecibirDano");
            animator.ResetTrigger("Disparar");
            animator.SetTrigger("Morir");
        }

        if (capsule != null)
        {
            capsule.direction = CapsuleDirection2D.Horizontal;
            capsule.size = tamanoColliderMuerte;
            capsule.offset = offsetColliderMuerte;
        }

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.simulated = true;
            rb.gravityScale = gravedadMuerte;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        GameObject jugador = GameObject.FindGameObjectWithTag("Player");

        foreach (Collider2D col in colliders)
        {
            if (col == null) continue;

            if (col.isTrigger)
            {
                col.enabled = false;
            }
            else if (jugador != null)
            {
                Collider2D[] collidersJugador =
                    jugador.GetComponentsInChildren<Collider2D>();

                foreach (Collider2D colJugador in collidersJugador)
                {
                    if (colJugador != null)
                        Physics2D.IgnoreCollision(col, colJugador, true);
                }
            }
        }
    }

    public bool EstaVivo()
    {
        return !muerto;
    }
}

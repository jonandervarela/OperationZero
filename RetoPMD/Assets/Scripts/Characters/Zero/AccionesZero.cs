
using UnityEngine;

public class AccionesZero : MonoBehaviour
{
    [Header("Disparo")]
    public GameObject prefabBala;
    public Transform puntoDisparo;
    public float tiempoEntreDisparos = 0.3f;

    [Header("Referencias")]
    public SistemaVida sistemaVida;

    [Header("Sonido")]
    private AudioSource audioSource;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private MovimientoZero movimiento;

    private float siguienteDisparo;
    private bool muerto = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        movimiento = GetComponent<MovimientoZero>();

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (sistemaVida == null)
        {
            sistemaVida = GetComponent<SistemaVida>();
        }
    }

    void Update()
    {
        if (muerto)
            return;

        // F para disparar.
        if (Input.GetKeyDown(KeyCode.F))
        {
            Disparar();
        }
    }

    // Iniciar la animación de disparo.
    public void Disparar()
    {
        if (muerto || Time.time < siguienteDisparo)
            return;

        if (prefabBala == null || puntoDisparo == null)
        {
            Debug.LogError("Falta asignar la bala o el punto de disparo.");
            return;
        }

        siguienteDisparo = Time.time + tiempoEntreDisparos;

        animator.SetTrigger("Disparar");
    }

    // Crear la bala mediante el evento de animación.
    public void CrearBala()
    {
        if (muerto)
            return;

        if (prefabBala == null || puntoDisparo == null)
            return;

        GameObject bala = Instantiate(
            prefabBala,
            puntoDisparo.position,
            Quaternion.identity
        );

        float direccion = spriteRenderer.flipX ? -1f : 1f;

        ProyectilZero proyectil =
            bala.GetComponent<ProyectilZero>();

        if (proyectil != null)
        {
            proyectil.ConfigurarDireccion(
                new Vector2(direccion, 0)
            );
        }

        // Reproducir el sonido del disparo.
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }

        Debug.Log("ZERO ha disparado.");
    }

    // Recibir daño.
    public void RecibirDano(int cantidad)
    {
        if (muerto || sistemaVida == null)
            return;

        sistemaVida.RecibirDano(cantidad);

        if (sistemaVida.vidaActual > 0)
        {
            animator.SetTrigger("RecibirDano");
        }
        else
        {
            Morir();
        }
    }

    // Morir.
    public void Morir()
    {
        if (muerto)
            return;

        muerto = true;

        if (movimiento != null)
            movimiento.BloquearMovimiento();

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.simulated = true;
            rb.gravityScale = 1f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.linearVelocity = new Vector2(
                0f,
                rb.linearVelocity.y
            );
        }

        CapsuleCollider2D capsule =
            GetComponent<CapsuleCollider2D>();

        if (capsule != null)
        {
            capsule.direction = CapsuleDirection2D.Horizontal;
            capsule.size = new Vector2(1.7f, 0.7f);
            capsule.offset = new Vector2(0f, -0.3f);
        }

        animator.ResetTrigger("Disparar");
        animator.ResetTrigger("RecibirDano");
        animator.SetTrigger("Morir");

        Debug.Log("ZERO ha muerto.");
    }

    // Consultar si ZERO está muerto.
    public bool EstaMuerto()
    {
        return muerto;
    }
}

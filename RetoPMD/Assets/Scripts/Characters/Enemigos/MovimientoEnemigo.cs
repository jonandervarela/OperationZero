
using UnityEngine;

public class MovimientoEnemigo : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidadPatrulla = 1.5f;
    public float velocidadPersecucion = 2.5f;
    public float distanciaPatrulla = 3f;

    [Header("Deteccion")]
    public float rangoVision = 6f;
    public float distanciaAtaque = 4f;
    public Transform jugador;

    [Header("Sonido de deteccion")]
    public AudioSource audioAlerta;

    [Header("Obstaculos")]
    public float distanciaObstaculo = 0.65f;
    public LayerMask capaObstaculos;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private SistemaEnemigo sistemaEnemigo;
    private Collider2D miCollider;

    private float posicionInicial;
    private int direccion = 1;

    private bool persiguiendo;
    private bool muerto;
    private bool zeroMuerto;

    // Indica si el enemigo ya ha detectado al jugador.
    private bool alertaReproducida;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        sistemaEnemigo = GetComponent<SistemaEnemigo>();
        miCollider = GetComponent<Collider2D>();
    }

    void Start()
    {
        posicionInicial = transform.position.x;

        // Buscar a ZERO automáticamente.
        if (jugador == null)
        {
            GameObject objetoJugador =
                GameObject.FindGameObjectWithTag("Player");

            if (objetoJugador != null)
                jugador = objetoJugador.transform;
        }
    }

    void Update()
    {
        // Comprobar si el SWAT ha muerto.
        if (sistemaEnemigo != null &&
            !sistemaEnemigo.EstaVivo())
        {
            muerto = true;

            rb.linearVelocity = new Vector2(
                0, rb.linearVelocity.y
            );

            if (animator != null)
                animator.SetBool("Caminar", false);

            return;
        }

        if (muerto)
            return;

        // Comprobar si ZERO ha muerto.
        zeroMuerto = false;

        if (jugador != null)
        {
            AccionesZero acciones =
                jugador.GetComponent<AccionesZero>();

            if (acciones != null)
                zeroMuerto = acciones.EstaMuerto();
        }

        // Comprobar si ZERO está dentro del rango de visión.
        bool detectandoAhora =
            !zeroMuerto &&
            jugador != null &&
            Mathf.Abs(
                jugador.position.x - transform.position.x
            ) <= rangoVision;

        // Reproducir alerta cada vez que detecta a ZERO.
        if (detectandoAhora && !alertaReproducida)
        {
            if (audioAlerta != null &&
                audioAlerta.clip != null)
            {
                audioAlerta.PlayOneShot(audioAlerta.clip);
            }

            alertaReproducida = true;

            Debug.Log("SWAT ha detectado a ZERO.");
        }

        // Si ZERO sale del rango, preparar otra alerta.
        if (!detectandoAhora)
        {
            alertaReproducida = false;
        }

        persiguiendo = detectandoAhora;

        // Perseguir al jugador.
        if (persiguiendo)
        {
            float diferencia =
                jugador.position.x - transform.position.x;

            if (Mathf.Abs(diferencia) > distanciaAtaque)
                direccion = diferencia > 0 ? 1 : -1;
        }
        else
        {
            // Patrullar entre los dos límites.
            if (transform.position.x >=
                posicionInicial + distanciaPatrulla)
            {
                direccion = -1;
            }

            if (transform.position.x <=
                posicionInicial - distanciaPatrulla)
            {
                direccion = 1;
            }
        }

        // Detectar paredes.
        Vector2 origen = transform.position;
        origen.y += 0.2f;
        origen.x += direccion * 0.5f;

        RaycastHit2D golpe = Physics2D.Raycast(
            origen,
            Vector2.right * direccion,
            distanciaObstaculo,
            capaObstaculos
        );

        if (golpe.collider != null &&
            !golpe.collider.isTrigger &&
            golpe.collider != miCollider &&
            !golpe.collider.CompareTag("Player"))
        {
            if (!persiguiendo)
                direccion *= -1;
        }

        // Girar el personaje.
        if (spriteRenderer != null)
            spriteRenderer.flipX = direccion < 0;

        // Activar animación de caminar.
        bool caminando = !persiguiendo ||
            (jugador != null &&
             Mathf.Abs(
                 jugador.position.x - transform.position.x
             ) > distanciaAtaque);

        if (animator != null)
            animator.SetBool("Caminar", caminando);
    }

    void FixedUpdate()
    {
        if (muerto)
            return;

        if (sistemaEnemigo != null &&
            !sistemaEnemigo.EstaVivo())
            return;

        float movimiento = direccion;
        float velocidad = velocidadPatrulla;

        if (persiguiendo && !zeroMuerto && jugador != null)
        {
            velocidad = velocidadPersecucion;

            float distancia = Mathf.Abs(
                jugador.position.x - transform.position.x
            );

            if (distancia <= distanciaAtaque)
                movimiento = 0;
        }

        // Movimiento horizontal.
        rb.linearVelocity = new Vector2(
            movimiento * velocidad,
            rb.linearVelocity.y
        );
    }
}


using UnityEngine;

public class MovimientoZero : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 4f;

    [Header("Salto")]
    public float fuerzaSalto = 8f;
    public Transform groundCheck;
    public float radioSuelo = 0.2f;
    public LayerMask capaSuelo;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float movimientoHorizontal;
    private bool estaEnSuelo;
    private bool movimientoBloqueado;
    private bool muerto;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (muerto)
            return;

        // Comprobar si ZERO está tocando el suelo.
        estaEnSuelo = groundCheck != null &&
            Physics2D.OverlapCircle(
                groundCheck.position,
                radioSuelo,
                capaSuelo
            );

        // Leer movimiento horizontal.
        movimientoHorizontal = movimientoBloqueado
            ? 0
            : Input.GetAxisRaw("Horizontal");

        // Girar el personaje.
        if (movimientoHorizontal < 0)
            spriteRenderer.flipX = true;
        else if (movimientoHorizontal > 0)
            spriteRenderer.flipX = false;

        // Actualizar animación de caminar.
        animator.SetBool(
            "Caminando",
            movimientoHorizontal != 0 && estaEnSuelo
        );

        // Actualizar animación de salto.
        animator.SetBool("Saltando", !estaEnSuelo);

        // Saltar solamente si está en el suelo.
        if (!movimientoBloqueado &&
            Input.GetKeyDown(KeyCode.Space) &&
            estaEnSuelo)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                fuerzaSalto
            );

            animator.SetBool("Saltando", true);
            animator.SetBool("Caminando", false);
        }

        // SALTAR con la tecla ESPACIO.
        if (Input.GetKeyDown(KeyCode.Space) && estaEnSuelo)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                fuerzaSalto
            );

            estaEnSuelo = false;
            animator.SetBool("Saltando", true);
            animator.SetBool("Caminando", false);
        }
    }

    void FixedUpdate()
    {
        if (muerto || movimientoBloqueado)
        {
            rb.linearVelocity = new Vector2(
                0,
                rb.linearVelocity.y
            );
            return;
        }

        rb.linearVelocity = new Vector2(
            movimientoHorizontal * velocidad,
            rb.linearVelocity.y
        );
    }

    // Bloquear el movimiento de ZERO.
    public void BloquearMovimiento()
    {
        movimientoBloqueado = true;
        movimientoHorizontal = 0;

        rb.linearVelocity = new Vector2(
            0,
            rb.linearVelocity.y
        );

        animator.SetBool("Caminando", false);
    }

    // Permitir que ZERO vuelva a moverse.
    public void DesbloquearMovimiento()
    {
        if (!muerto)
            movimientoBloqueado = false;
    }

    // Detener definitivamente el movimiento al morir.
    public void DetenerMovimiento()
    {
        muerto = true;
        movimientoBloqueado = true;
        movimientoHorizontal = 0;
        rb.linearVelocity = Vector2.zero;
    }

    // Dibujar el detector del suelo.
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(
                groundCheck.position,
                radioSuelo
            );
        }
    }
}

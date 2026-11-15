
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
            ) != null;

        // Leer movimiento horizontal.
        movimientoHorizontal = movimientoBloqueado
            ? 0
            : Input.GetAxisRaw("Horizontal");

        // Girar el personaje.
        if (movimientoHorizontal < 0)
            spriteRenderer.flipX = true;
        else if (movimientoHorizontal > 0)
            spriteRenderer.flipX = false;

        // Animación de caminar.
        animator.SetBool(
            "Caminando",
            movimientoHorizontal != 0 && estaEnSuelo
        );

        // Animación de salto.
        animator.SetBool("Saltando", !estaEnSuelo);

        // W para saltar.
        if (!movimientoBloqueado &&
            Input.GetKeyDown(KeyCode.W) &&
            estaEnSuelo)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                fuerzaSalto
            );

            animator.SetBool("Saltando", true);
            animator.SetBool("Caminando", false);

            Debug.Log("ZERO está saltando");
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

    public void DesbloquearMovimiento()
    {
        if (!muerto)
            movimientoBloqueado = false;
    }

    public void DetenerMovimiento()
    {
        muerto = true;
        movimientoBloqueado = true;
        movimientoHorizontal = 0;

        rb.linearVelocity = Vector2.zero;
    }

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

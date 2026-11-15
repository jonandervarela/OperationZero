
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

    // Controles tactiles
    private bool tocarIzquierda;
    private bool tocarDerecha;
    private bool solicitarSalto;

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

        estaEnSuelo = groundCheck != null &&
            Physics2D.OverlapCircle(
                groundCheck.position,
                radioSuelo,
                capaSuelo
            ) != null;

        // Teclado y controles tactiles.
        float teclado = Input.GetAxisRaw("Horizontal");
        float tactil = 0;

        if (tocarIzquierda) tactil -= 1;
        if (tocarDerecha) tactil += 1;

        movimientoHorizontal = movimientoBloqueado
            ? 0
            : Mathf.Clamp(teclado + tactil, -1, 1);

        if (movimientoHorizontal < 0)
            spriteRenderer.flipX = true;
        else if (movimientoHorizontal > 0)
            spriteRenderer.flipX = false;

        animator.SetBool(
            "Caminando",
            movimientoHorizontal != 0 && estaEnSuelo
        );

        animator.SetBool("Saltando", !estaEnSuelo);

        // Salto con W o boton tactil.
        if (!movimientoBloqueado &&
            (Input.GetKeyDown(KeyCode.W) || solicitarSalto) &&
            estaEnSuelo)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                fuerzaSalto
            );

            animator.SetBool("Saltando", true);
            animator.SetBool("Caminando", false);
        }

        solicitarSalto = false;
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

    // Metodos para botones tactiles.
    public void MantenerIzquierda(bool pulsado)
    {
        tocarIzquierda = pulsado;
    }

    public void MantenerDerecha(bool pulsado)
    {
        tocarDerecha = pulsado;
    }

    public void Saltar()
    {
        solicitarSalto = true;
    }

    public void BloquearMovimiento()
    {
        movimientoBloqueado = true;
        movimientoHorizontal = 0;
        tocarIzquierda = false;
        tocarDerecha = false;

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

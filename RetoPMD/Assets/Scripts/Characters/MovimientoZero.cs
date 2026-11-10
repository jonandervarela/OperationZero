using UnityEngine;

public class MovimientoZero : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 4f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float movimientoHorizontal;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Leer las teclas A/D o las flechas izquierda/derecha
        movimientoHorizontal = Input.GetAxisRaw("Horizontal");

        // Activar la animación de caminar o estar quieto
        animator.SetBool("Caminando", movimientoHorizontal != 0);

        // Girar el personaje según la dirección
        if (movimientoHorizontal < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movimientoHorizontal > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    void FixedUpdate()
    {
        // Mover al personaje sin modificar la velocidad vertical
        rb.linearVelocity = new Vector2(
            movimientoHorizontal * velocidad,
            rb.linearVelocity.y
        );
    }
}

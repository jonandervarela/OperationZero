
using UnityEngine;

public class AccionesZero : MonoBehaviour
{
    [Header("Disparo")]
    public GameObject prefabBala;
    public Transform puntoDisparo;
    public float tiempoEntreDisparos = 0.3f;

    [Header("Referencias")]
    public SistemaVida sistemaVida;

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

        if (sistemaVida == null)
            sistemaVida = GetComponent<SistemaVida>();
    }

    void Update()
    {
        if (muerto)
            return;

        // Disparar con F.
        if (Input.GetKeyDown(KeyCode.F))
        {
            Disparar();
        }
    }

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

        // Activar animación de disparo.
        animator.SetTrigger("Disparar");

        // Crear proyectil.
        GameObject bala = Instantiate(
            prefabBala,
            puntoDisparo.position,
            Quaternion.identity
        );

        // Calcular dirección según hacia dónde mira ZERO.
        float direccion = spriteRenderer.flipX ? -1f : 1f;

        ProyectilZero proyectil = bala.GetComponent<ProyectilZero>();

        if (proyectil != null)
        {
            proyectil.ConfigurarDireccion(new Vector2(direccion, 0));
        }
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

        animator.ResetTrigger("Disparar");
        animator.ResetTrigger("RecibirDano");
        animator.SetTrigger("Morir");

        Debug.Log("ZERO ha muerto.");
    }

    public bool EstaMuerto()
    {
        return muerto;
    }
}

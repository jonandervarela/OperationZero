
using UnityEngine;

public class DisparoEnemigo : MonoBehaviour
{
    [Header("Disparo")]
    public GameObject prefabBala;
    public Transform puntoDisparo;
    public float distanciaDisparo = 6f;
    public float tiempoEntreDisparos = 1.5f;

    [Header("Posicion del disparo")]
    public float distanciaHorizontal = 0.65f;
    public float alturaDisparo = 0.2f;

    [Header("Referencias")]
    public Transform jugador;

    [Header("Sonido")]
    public AudioSource audioDisparo;

    private Animator animator;
    private SistemaEnemigo sistemaEnemigo;

    private float siguienteDisparo;
    private bool muerto;
    private bool disparoPendiente;

    void Awake()
    {
        animator = GetComponent<Animator>();
        sistemaEnemigo = GetComponent<SistemaEnemigo>();
    }

    void Start()
    {
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
        if (muerto)
            return;

        if (sistemaEnemigo != null &&
            !sistemaEnemigo.EstaVivo())
        {
            muerto = true;
            disparoPendiente = false;
            return;
        }

        if (jugador == null ||
            prefabBala == null ||
            puntoDisparo == null)
            return;

        AccionesZero acciones =
            jugador.GetComponent<AccionesZero>();

        if (acciones != null && acciones.EstaMuerto())
        {
            disparoPendiente = false;
            return;
        }

        float diferenciaX =
            Mathf.Abs(jugador.position.x - transform.position.x);

        float diferenciaY =
            Mathf.Abs(jugador.position.y - transform.position.y);

        if (diferenciaX > distanciaDisparo || diferenciaY > 1.5f)
            return;

        if (Time.time < siguienteDisparo || disparoPendiente)
            return;

        siguienteDisparo = Time.time + tiempoEntreDisparos;
        disparoPendiente = true;

        animator.SetTrigger("Disparar");
    }

    // Crear bala mediante evento de animacion.
    public void CrearBala()
    {
        if (muerto || !disparoPendiente)
            return;

        if (sistemaEnemigo != null &&
            !sistemaEnemigo.EstaVivo())
            return;

        if (jugador == null)
            return;

        AccionesZero acciones =
            jugador.GetComponent<AccionesZero>();

        if (acciones != null && acciones.EstaMuerto())
        {
            disparoPendiente = false;
            return;
        }

        float direccionX =
            jugador.position.x > transform.position.x ? 1f : -1f;

        puntoDisparo.localPosition = new Vector3(
            distanciaHorizontal * direccionX,
            alturaDisparo,
            0
        );

        GameObject bala = Instantiate(
            prefabBala,
            puntoDisparo.position,
            Quaternion.identity
        );

        Vector2 direccion = (
            jugador.position - puntoDisparo.position
        ).normalized;

        ProyectilEnemigo proyectil =
            bala.GetComponent<ProyectilEnemigo>();

        if (proyectil != null)
        {
            proyectil.ConfigurarDisparo(direccion, transform);
        }

        // Reproducir sonido del disparo.
        if (audioDisparo != null && audioDisparo.clip != null)
        {
            audioDisparo.PlayOneShot(audioDisparo.clip);
        }

        disparoPendiente = false;

        Debug.Log("SWAT ha disparado.");
    }

    public void CancelarDisparo()
    {
        disparoPendiente = false;
    }
}

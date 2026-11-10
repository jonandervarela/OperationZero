using UnityEngine;

public class BrilloObjeto : MonoBehaviour
{
    public Transform halo;
    public float velocidadBrillo = 3f;
    public float intensidad = 0.15f;
    public float velocidadMovimiento = 1.5f;
    public float alturaMovimiento = 0.12f;

    private Vector3 escalaInicial;
    private Vector3 posicionInicial;

    void Start()
    {
        if (halo != null)
        {
            escalaInicial = halo.localScale;
            posicionInicial = halo.localPosition;
        }
    }

    void Update()
    {
        if (halo == null)
            return;

        // Efecto de pulsación del halo
        float brillo = 1f + Mathf.Sin(Time.time * velocidadBrillo) * intensidad;
        halo.localScale = escalaInicial * brillo;

        // Movimiento suave hacia arriba y abajo
        float movimiento = Mathf.Sin(Time.time * velocidadMovimiento) * alturaMovimiento;

        halo.localPosition = posicionInicial + new Vector3(0, movimiento, 0);
    }
}
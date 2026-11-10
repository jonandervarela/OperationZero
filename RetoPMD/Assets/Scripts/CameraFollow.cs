
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform objetivo;
    public SpriteRenderer fondo;
    public float suavizado = 5f;

    private Camera camara;

    void Start()
    {
        camara = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (objetivo == null || fondo == null)
            return;

        // Calcular la mitad del ancho que ve la camara
        float mitadAnchoCamara =
            camara.orthographicSize * camara.aspect;

        // Obtener los limites reales del fondo
        float limiteIzquierdo =
            fondo.bounds.min.x + mitadAnchoCamara;

        float limiteDerecho =
            fondo.bounds.max.x - mitadAnchoCamara;

        // Evitar problemas si el fondo es mas pequeño que la camara
        if (limiteIzquierdo > limiteDerecho)
        {
            limiteIzquierdo = fondo.bounds.center.x;
            limiteDerecho = fondo.bounds.center.x;
        }

        // Limitar la posicion de la camara
        float posicionX = Mathf.Clamp(
            objetivo.position.x,
            limiteIzquierdo,
            limiteDerecho
        );

        Vector3 nuevaPosicion = new Vector3(
            posicionX,
            transform.position.y,
            transform.position.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            nuevaPosicion,
            suavizado * Time.deltaTime
        );
    }
}


using UnityEngine;

public class Zombi : MonoBehaviour
{
    [Header("Datos del zombi")]
    public int vidaInicial = 3;
    public int vida = 3;
    public float velocidad = 1f;
    public int danoCasa = 1;
    public int danoPlanta = 1;

    [Header("Casa")]
    public Casa casa;

    [Header("Plantas")]
    public Planta[] plantas;

    [Header("Ataque")]
    public float distanciaAtaque = 0.5f;
    public float tiempoEntreAtaques = 1f;

    private float tiempoActual = 0f;
    private Collider2D miCollider;

    private void Awake()
    {
        miCollider = GetComponent<Collider2D>();
    }

    // Reiniciar los valores cada vez que aparece un zombi nuevo.
    public void Inicializar(Casa casaNueva, Planta[] plantasNuevas)
    {
        vida = vidaInicial;
        tiempoActual = 0f;

        casa = casaNueva;
        plantas = plantasNuevas;

        Debug.Log("Zombi inicializado. Vida: " + vida);
    }

    public void Simulate()
    {
        if (vida <= 0 || casa == null || casa.vida <= 0)
            return;

        // Buscar una planta delante del zombi.
        Planta objetivo = BuscarPlanta();

        if (objetivo != null)
        {
            float limitePlanta = ObtenerLimitePlanta(objetivo);

            if (transform.position.x > limitePlanta)
            {
                CaminarHasta(limitePlanta);
            }
            else
            {
                AtacarPlanta(objetivo);
            }

            return;
        }

        // Si no hay plantas delante, avanzar hacia la casa.
        float limiteCasa = ObtenerLimiteCasa();

        if (transform.position.x > limiteCasa)
        {
            CaminarHasta(limiteCasa);
        }
        else
        {
            // Atacar la casa periódicamente.
            tiempoActual += Time.deltaTime;

            if (tiempoActual >= tiempoEntreAtaques)
            {
                casa.RecibirDano(danoCasa);
                tiempoActual = 0f;
            }
        }
    }

    Planta BuscarPlanta()
    {
        if (plantas == null || casa == null)
            return null;

        Planta objetivo = null;
        float posicionMasCercana = float.MinValue;

        foreach (Planta planta in plantas)
        {
            if (planta == null ||
                planta.vida <= 0 ||
                !planta.gameObject.activeInHierarchy)
                continue;

            float distanciaY = Mathf.Abs(
                transform.position.y -
                planta.transform.position.y
            );

            float posicionX = planta.transform.position.x;

            // La planta debe estar delante y antes de la casa.
            if (posicionX < transform.position.x &&
                posicionX > casa.transform.position.x &&
                distanciaY < 0.4f)
            {
                // Elegir la planta más cercana.
                if (posicionX > posicionMasCercana)
                {
                    posicionMasCercana = posicionX;
                    objetivo = planta;
                }
            }
        }

        return objetivo;
    }

    float ObtenerLimitePlanta(Planta planta)
    {
        Collider2D colliderPlanta =
            planta.GetComponent<Collider2D>();

        float bordeDerecho;

        if (colliderPlanta != null)
        {
            bordeDerecho = colliderPlanta.bounds.max.x;
        }
        else
        {
            bordeDerecho = planta.transform.position.x;
        }

        float anchoZombi = miCollider != null
            ? miCollider.bounds.extents.x
            : 0f;

        return bordeDerecho +
               anchoZombi +
               distanciaAtaque;
    }

    float ObtenerLimiteCasa()
    {
        Collider2D colliderCasa =
            casa.GetComponent<Collider2D>();

        float bordeDerecho;

        if (colliderCasa != null)
        {
            bordeDerecho = colliderCasa.bounds.max.x;
        }
        else
        {
            bordeDerecho = casa.transform.position.x;
        }

        float anchoZombi = miCollider != null
            ? miCollider.bounds.extents.x
            : 0f;

        return bordeDerecho +
               anchoZombi +
               distanciaAtaque;
    }

    void CaminarHasta(float limiteX)
    {
        Vector3 posicion = transform.position;

        posicion.x = Mathf.Max(
            limiteX,
            posicion.x - velocidad * Time.deltaTime
        );

        transform.position = posicion;
    }

    void AtacarPlanta(Planta planta)
    {
        if (planta == null || planta.vida <= 0)
            return;

        tiempoActual += Time.deltaTime;

        if (tiempoActual >= tiempoEntreAtaques)
        {
            planta.RecibirDano(danoPlanta);
            tiempoActual = 0f;
        }
    }

    public void RecibirDano(int dano)
    {
        if (vida <= 0 || dano <= 0)
            return;

        vida -= dano;

        if (vida <= 0)
        {
            vida = 0;

            Debug.Log("Un zombi ha sido eliminado");

            gameObject.SetActive(false);
        }
    }
}
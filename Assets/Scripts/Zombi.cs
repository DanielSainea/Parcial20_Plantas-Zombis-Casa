
using UnityEngine;

public class Zombi : MonoBehaviour
{
    [Header("Datos del zombi")]
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

    public void Simulate()
    {
        if (vida <= 0 || casa == null || casa.vida <= 0)
            return;

        Planta objetivo = BuscarPlanta();

        // Si hay una planta delante, el zombi la ataca.
        if (objetivo != null)
        {
            float distancia = transform.position.x -
                              objetivo.transform.position.x;

            if (distancia <= distanciaAtaque)
            {
                AtacarPlanta(objetivo);
            }
            else
            {
                Caminar();
            }

            return;
        }

        // Si no hay plantas, avanza hacia la casa.
        float distanciaCasa = transform.position.x -
                              casa.transform.position.x;

        if (distanciaCasa > distanciaAtaque)
        {
            Caminar();
        }
        else
        {
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
        Planta objetivo = null;
        float posicionMasCercana = float.MinValue;

        if (plantas == null)
            return null;

        foreach (Planta planta in plantas)
        {
            if (planta == null || planta.vida <= 0 ||
                !planta.gameObject.activeInHierarchy)
                continue;

            float distanciaY = Mathf.Abs(
                transform.position.y - planta.transform.position.y
            );

            float posicionX = planta.transform.position.x;
            float posicionCasa = casa.transform.position.x;

            // La planta debe estar delante y en el mismo carril.
            if (posicionX < transform.position.x &&
                posicionX > posicionCasa &&
                distanciaY < 0.4f)
            {
                if (posicionX > posicionMasCercana)
                {
                    posicionMasCercana = posicionX;
                    objetivo = planta;
                }
            }
        }

        return objetivo;
    }

    void Caminar()
    {
        transform.position += Vector3.left *
                              velocidad * Time.deltaTime;
    }

    void AtacarPlanta(Planta planta)
    {
        tiempoActual += Time.deltaTime;

        if (tiempoActual >= tiempoEntreAtaques)
        {
            planta.RecibirDano(danoPlanta);
            tiempoActual = 0f;
        }
    }

    public void RecibirDano(int dano)
    {
        if (vida <= 0)
            return;

        vida -= dano;

        if (vida <= 0)
        {
            vida = 0;
            gameObject.SetActive(false);
        }
    }
}

using UnityEngine;

public class Zombi : MonoBehaviour
{
    [Header("Datos del zombi")]
    public int vida = 3;
    public float velocidad = 1f;
    public int danoCasa = 1;

    [Header("Casa")]
    public Casa casa;

    [Header("Ataque")]
    public float distanciaAtaque = 0.5f;
    public float tiempoEntreAtaques = 1f;
    private float tiempoActual = 0f;

    public void Simulate()
    {
        if (vida <= 0 || casa == null || casa.vida <= 0)
            return;

        float distanciaY = Mathf.Abs(
            transform.position.y - casa.transform.position.y
        );

        if (distanciaY > 0.4f)
            return;

        float distanciaX = transform.position.x -
                           casa.transform.position.x;

        if (distanciaX > distanciaAtaque)
        {
            transform.position += Vector3.left *
                                  velocidad * Time.deltaTime;
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
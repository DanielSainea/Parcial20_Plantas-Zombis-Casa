
using UnityEngine;

public class Planta : MonoBehaviour
{


    [Header("Vida de la planta")]
    public int vida = 3;

    public void RecibirDano(int dano)
    {
        if (vida <= 0)
            return;

        vida -= dano;

        if (vida <= 0)
        {
            vida = 0;
            gameObject.SetActive(false);
            Debug.Log("Una planta ha sido destruida");
        }
    }
    [Header("Datos de la planta")]
    public float alcance = 10f;
    public float tiempoEntreAtaques = 1f;
    private float tiempoActual = 0f;

    [Header("Proyectil")]
    public proyectil prefabProyectil;
    public Transform puntoDisparo;

    [Header("Zombis")]
    public Zombi[] zombis;

    public void Simulate()
    {
        tiempoActual += Time.deltaTime;

        if (tiempoActual >= tiempoEntreAtaques)
        {
            foreach (Zombi zombi in zombis)
            {
                if (zombi == null || zombi.vida <= 0)
                    continue;

                float distanciaX = zombi.transform.position.x -
                                   transform.position.x;

                float distanciaY = Mathf.Abs(zombi.transform.position.y -
                                              transform.position.y);

                if (distanciaX > 0 && distanciaX <= alcance &&
                    distanciaY < 0.4f)
                {
                    Disparar();
                    tiempoActual = 0f;
                    break;
                }
            }
        }
    }

    void Disparar()
    {
        if (prefabProyectil != null && puntoDisparo != null)
        {
            Instantiate(prefabProyectil, puntoDisparo.position,
                        Quaternion.identity);
        }
    }
}
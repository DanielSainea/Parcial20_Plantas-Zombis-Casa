
using System.Collections.Generic;
using UnityEngine;

public class Simulate : MonoBehaviour
{
    [Header("Entidades")]
    public Planta[] plantas;
    public List<Zombi> zombis = new List<Zombi>();
    public Casa casa;

    [Header("Aparicion")]
    public Zombi prefabZombi;
    public Transform[] puntosAparicion;
    public float tiempoEntreApariciones = 5f;

    private float tiempoActual = 0f;
    private int siguienteFila = 0;

    void Update()
    {
        if (casa == null || casa.vida <= 0)
            return;

        foreach (Planta planta in plantas)
        {
            if (planta != null)
            {
                planta.zombis = zombis.ToArray();
                planta.Simulate();
            }
        }

        foreach (Zombi zombi in zombis)
        {
            if (zombi != null && zombi.gameObject.activeInHierarchy)
            {
                zombi.Simulate();
            }
        }

        casa.Simulate();

        tiempoActual += Time.deltaTime;

        if (tiempoActual >= tiempoEntreApariciones)
        {
            tiempoActual = 0f;
            CrearZombi();
        }
    }

    void CrearZombi()
    {
        if (prefabZombi == null || puntosAparicion.Length == 0)
            return;

        Transform punto = puntosAparicion[siguienteFila];

        Zombi nuevoZombi = Instantiate(
            prefabZombi,
            punto.position,
            Quaternion.identity
        );

        nuevoZombi.casa = casa;
        zombis.Add(nuevoZombi);

        siguienteFila++;

        if (siguienteFila >= puntosAparicion.Length)
            siguienteFila = 0;
    }
}
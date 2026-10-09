
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
        if (casa == null)
            return;

        // La simulacion termina cuando destruyen la casa.
        if (casa.vida <= 0)
        {
            casa.Simulate();
            return;
        }

        // Las plantas buscan zombis y disparan.
        foreach (Planta planta in plantas)
        {
            if (planta != null &&
                planta.gameObject.activeInHierarchy)
            {
                planta.zombis = zombis.ToArray();
                planta.Simulate();
            }
        }

        // Los zombis caminan y atacan.
        foreach (Zombi zombi in zombis)
        {
            if (zombi != null &&
                zombi.gameObject.activeInHierarchy)
            {
                zombi.plantas = plantas;
                zombi.casa = casa;
                zombi.Simulate();
            }
        }

        // Limpiar zombis muertos sin detener el generador.
        zombis.RemoveAll(z =>
            z == null || z.vida <= 0 ||
            !z.gameObject.activeInHierarchy);

        casa.Simulate();

        // Generar zombis independientemente de los que estén vivos.
        tiempoActual += Time.deltaTime;

        if (tiempoActual >= tiempoEntreApariciones)
        {
            tiempoActual = 0f;
            CrearZombi();
        }
    }

    void CrearZombi()
    {
        if (prefabZombi == null ||
            puntosAparicion == null ||
            puntosAparicion.Length == 0)
            return;

        Transform punto = puntosAparicion[siguienteFila];

        if (punto == null)
            return;

        Zombi nuevoZombi = Instantiate(
            prefabZombi,
            punto.position,
            Quaternion.identity
        );

        nuevoZombi.casa = casa;
        nuevoZombi.plantas = plantas;

        zombis.Add(nuevoZombi);

        siguienteFila++;

        if (siguienteFila >= puntosAparicion.Length)
            siguienteFila = 0;

        Debug.Log("Nuevo zombi generado");
    }
}
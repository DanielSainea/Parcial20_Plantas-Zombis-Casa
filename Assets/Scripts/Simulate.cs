
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

        if (casa.vida <= 0)
        {
            casa.Simulate();
            return;
        }

        if (zombis == null)
            zombis = new List<Zombi>();

        // Las plantas disparan a los zombis.
        if (plantas != null)
        {
            foreach (Planta planta in plantas)
            {
                if (planta == null ||
                    !planta.gameObject.activeInHierarchy)
                    continue;

                planta.zombis = zombis.ToArray();
                planta.Simulate();
            }
        }

        // Los zombis buscan plantas y atacan la casa.
        foreach (Zombi zombi in zombis)
        {
            if (zombi == null ||
                !zombi.gameObject.activeInHierarchy)
                continue;

            zombi.casa = casa;
            zombi.plantas = plantas;
            zombi.Simulate();
        }

        // Retirar los zombis muertos de la lista.
        zombis.RemoveAll(z =>
            z == null ||
            z.vida <= 0 ||
            !z.gameObject.activeInHierarchy
        );

        casa.Simulate();

        // El generador no depende de la lista de zombis vivos.
        tiempoActual += Time.deltaTime;

        if (tiempoActual >= tiempoEntreApariciones)
        {
            tiempoActual -= tiempoEntreApariciones;
            CrearZombi();
        }
    }

    void CrearZombi()
    {
        if (prefabZombi == null)
        {
            Debug.LogError(
                "No hay prefab de zombi asignado en Simulate."
            );
            return;
        }

        if (puntosAparicion == null ||
            puntosAparicion.Length == 0)
        {
            Debug.LogError(
                "No hay puntos de aparicion asignados en Simulate."
            );
            return;
        }

        // Buscar el siguiente punto válido.
        Transform punto = null;

        for (int i = 0; i < puntosAparicion.Length; i++)
        {
            if (siguienteFila >= puntosAparicion.Length)
                siguienteFila = 0;

            punto = puntosAparicion[siguienteFila];
            siguienteFila++;

            if (punto != null)
                break;
        }

        if (punto == null)
        {
            Debug.LogError(
                "Todos los puntos de aparicion estan vacios."
            );
            return;
        }

        Zombi nuevoZombi = Instantiate(
            prefabZombi,
            punto.position,
            Quaternion.identity
        );

        nuevoZombi.gameObject.SetActive(true);

        nuevoZombi.casa = casa;
        nuevoZombi.plantas = plantas;

        // Asegurar que el nuevo zombi se registre.
        if (!zombis.Contains(nuevoZombi))
            zombis.Add(nuevoZombi);

        Debug.Log(
            "Zombi creado: " + nuevoZombi.name +
            " | Activo: " +
            nuevoZombi.gameObject.activeInHierarchy +
            " | Posicion: " + nuevoZombi.transform.position
        );
    }
}

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
        // Comprobar que exista la casa.
        if (casa == null)
            return;

        // Finalizar la simulacion si destruyen la casa.
        if (casa.vida <= 0)
        {
            casa.Simulate();
            return;
        }

        // Asegurar que exista la lista de zombis.
        if (zombis == null)
        {
            zombis = new List<Zombi>();
        }

        // Actualizar las plantas y sus objetivos.
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

        // Actualizar el movimiento y los ataques de los zombis.
        foreach (Zombi zombi in zombis)
        {
            if (zombi == null ||
                !zombi.gameObject.activeInHierarchy)
                continue;

            zombi.casa = casa;
            zombi.plantas = plantas;

            zombi.Simulate();
        }

        // Eliminar zombis muertos o desactivados de la lista.
        zombis.RemoveAll(z =>
            z == null ||
            z.vida <= 0 ||
            !z.gameObject.activeInHierarchy
        );

        // Actualizar el estado de la casa.
        casa.Simulate();

        // Generar zombis independientemente de las muertes.
        tiempoActual += Time.deltaTime;

        if (tiempoActual >= tiempoEntreApariciones)
        {
            tiempoActual -= tiempoEntreApariciones;
            CrearZombi();
        }
    }

    void CrearZombi()
    {
        // Comprobar el prefab.
        if (prefabZombi == null)
        {
            Debug.LogError(
                "ERROR: No hay prefab de zombi asignado."
            );
            return;
        }

        // Comprobar los puntos de aparicion.
        if (puntosAparicion == null ||
            puntosAparicion.Length == 0)
        {
            Debug.LogError(
                "ERROR: No hay puntos de aparicion asignados."
            );
            return;
        }

        // Buscar un punto de aparicion valido.
        Transform punto = null;

        for (int i = 0; i < puntosAparicion.Length; i++)
        {
            if (siguienteFila >= puntosAparicion.Length)
            {
                siguienteFila = 0;
            }

            Transform candidato = puntosAparicion[siguienteFila];

            siguienteFila++;

            if (candidato != null)
            {
                punto = candidato;
                break;
            }
        }

        if (punto == null)
        {
            Debug.LogError(
                "ERROR: Todos los puntos de aparicion son nulos."
            );
            return;
        }

        // Crear una instancia independiente del prefab.
        Zombi nuevoZombi = Instantiate(
            prefabZombi,
            punto.position,
            Quaternion.identity
        );

        // Reiniciar vida y temporizador, y asignar referencias.
        nuevoZombi.Inicializar(casa, plantas);

        // Activar el nuevo zombi.
        nuevoZombi.gameObject.SetActive(true);

        // Añadirlo a la lista de la simulacion.
        zombis.Add(nuevoZombi);

        Debug.Log(
            "ZOMBI GENERADO: " + nuevoZombi.name +
            " | Vida: " + nuevoZombi.vida +
            " | Activo: " +
            nuevoZombi.gameObject.activeInHierarchy +
            " | Fila: " + (siguienteFila - 1)
        );
    }
}

using UnityEngine;

public class Casa : MonoBehaviour
{
    [Header("Datos de la casa")]
    public int vida = 20;

    private bool destruida = false;

    public void Simulate()
    {
        if (vida <= 0 && !destruida)
        {
            vida = 0;
            destruida = true;

            Debug.Log("PERDISTE: los zombis destruyeron la casa");
        }
    }

    public void RecibirDano(int dano)
    {
        if (vida <= 0 || dano <= 0)
            return;

        vida -= dano;

        if (vida <= 0)
            vida = 0;

        Debug.Log("Vida de la casa: " + vida);

        Simulate();
    }
}
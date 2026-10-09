
using UnityEngine;

public class Casa : MonoBehaviour
{
    [Header("Datos de la casa")]
    public int vida = 20;

    public void Simulate()
    {
        if (vida <= 0)
        {
            Debug.Log("PERDISTE: los zombis destruyeron la casa");
        }
    }

    public void RecibirDano(int dano)
    {
        if (vida <= 0)
        {
            return;
        }

        vida -= dano;

        if (vida < 0)
        {
            vida = 0;
        }

        Debug.Log("Vida de la casa: " + vida);
    }
}
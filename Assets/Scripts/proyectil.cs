
using UnityEngine;

public class proyectil : MonoBehaviour
{
    [Header("Datos del proyectil")]
    public float velocidad = 8f;
    public int dano = 1;
    public float limiteX = 15f;

    void Update()
    {
        transform.position += Vector3.right * velocidad * Time.deltaTime;

        if (transform.position.x > limiteX)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D otro)
    {
        Zombi zombi = otro.GetComponent<Zombi>();

        if (zombi != null && zombi.vida > 0)
        {
            zombi.RecibirDano(dano);
            Destroy(gameObject);
        }
    }
}
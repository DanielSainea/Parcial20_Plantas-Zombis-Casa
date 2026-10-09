
using UnityEngine;

public class MovimientoZombi : MonoBehaviour
{
    [SerializeField] private float velocidad = 1f;

    private void Update()
    {
        transform.position += Vector3.left
            * velocidad
            * Time.deltaTime;
    }
}
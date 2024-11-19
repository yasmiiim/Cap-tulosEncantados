using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class geradorFogo : MonoBehaviour
{
    public GameObject bolaDeFogoPrefab;
    public float intervalo = 3f;

    private void Start()
    {
        StartCoroutine(GerarBolasDeFogo());
    }

    private IEnumerator GerarBolasDeFogo()
    {
        while (true)
        {
            Instantiate(bolaDeFogoPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(intervalo);
        }
    }
}

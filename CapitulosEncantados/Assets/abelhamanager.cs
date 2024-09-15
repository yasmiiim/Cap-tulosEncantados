/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abelhamanager : MonoBehaviour
{
    public GameObject beePrefab;        // O Prefab da abelha
    public Transform spawnPoint;        // Ponto de spawn
    public int numberOfBees = 5;        // Número de abelhas no grupo
    public float spawnDelay = 0.5f;     // Tempo entre o spawn de cada abelha
    public float beeSpeed = 2.0f;       // Velocidade das abelhas
    public Vector2 moveDirection = Vector2.right; // Direção do movimento das abelhas

    private bool beesSpawned = false;

    void Start()
    {
        if (!beesSpawned)
        {
            StartCoroutine(SpawnBees());
            beesSpawned = true; // Garante que o spawn ocorre uma única vez por cena
        }
    }

    // Função que gera as abelhas no spawnPoint
    private IEnumerator SpawnBees()
    {
        for (int i = 0; i < numberOfBees; i++)
        {
            GameObject bee = Instantiate(beePrefab, spawnPoint.position, Quaternion.identity);
            bee.GetComponent<Rigidbody2D>().velocity = moveDirection * beeSpeed;

            yield return new WaitForSeconds(spawnDelay); // Delay entre o spawn de cada abelha
        }
    }
}*/


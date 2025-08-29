using System.Collections;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private GameObject[] itemPrefabs;
    [SerializeField] private float spawnInterval = 10f;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            foreach (var pos in spawnPositions)
            {
                if (pos == null || itemPrefabs.Length == 0) continue;
                if (pos.childCount > 0) continue;
                int index = Random.Range(0, itemPrefabs.Length);
                Instantiate(itemPrefabs[index], pos.position, Quaternion.identity, pos);
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
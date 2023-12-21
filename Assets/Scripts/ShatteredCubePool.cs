using System.Collections.Generic;
using UnityEngine;

public class ShatteredCubePool : MonoBehaviour
{
    public GameObject shatteredCubePrefab;
    public int poolSize = 10;

    private List<GameObject> pool;

    private void Start()
    {
        pool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject shatteredCube = Instantiate(shatteredCubePrefab, Vector3.zero, Quaternion.identity);
            shatteredCube.SetActive(false);
            pool.Add(shatteredCube);
        }
    }

    public GameObject GetShatteredCubeFromPool(Vector3 position)
    {
        // Find an available shattered cube in the pool
        foreach (var shatteredCube in pool)
        {
            if (!shatteredCube.activeSelf)
            {
                shatteredCube.transform.position = position;
                return shatteredCube;
            }
        }
        return null; // No available shattered cube in the pool
    }
}
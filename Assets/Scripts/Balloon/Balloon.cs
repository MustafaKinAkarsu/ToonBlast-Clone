using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    List<GameObject> balloon = new List<GameObject>();
    public static Balloon Instance;

    private void Start()
    {
        Instance = this;
    }

    public void AddToBalloon(GameObject obj)
    {
        balloon.Add(obj);
    }
    public void DestroyBalloon()
    {
        if (balloon != null)
        {
            foreach (GameObject ball in balloon)
            {
                var Coords = GridController.Instance.GetCoordFromTile(ball);
                int tileRow = Coords.Item1;
                int tileCol = Coords.Item2;
                if (tileRow >= 0 && tileRow < GridGenerator.Instance.rows && tileCol >= 0 && tileCol < GridGenerator.Instance.columns)
                {
                    Destroy(ball);
                    GridGenerator.Instance.instantiatedPrefabs[tileRow, tileCol] = null;
                }
            }
        }
    }
}

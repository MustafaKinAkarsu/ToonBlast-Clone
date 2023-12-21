using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

public class GridController : MonoBehaviour
{
    GridGenerator _gridGenerator;
    [SerializeField] GameManager manager;
    [SerializeField] ShatteredCubePool shatteredCubePool;
    AudioManager _audioManager;
    public static GridController Instance { get; private set; }
    public int previousRandom;
    private Vector3 goalPosition = new Vector3(-0.45f, 7.52f, 90.00f);
    Queue<GameObject> tilesToMove = new Queue<GameObject>();

    private void Start()
    {
        Instance = this;
        _gridGenerator = GridGenerator.Instance;
        _audioManager = AudioManager.Instance;
    }

    public void CheckAndDestroyChains(GameObject startTile)
    {
        List<GameObject> matchingTiles = new List<GameObject>();
        List<GameObject> balloon = new List<GameObject>();
        matchingTiles.Add(startTile);

        // Get the row and column of the start tile
        var startCoords = GetCoordFromTile(startTile);
        int startRow = startCoords.Item1;
        int startCol = startCoords.Item2;

        // Check and add adjacent matching tiles to the list
        CheckAdjacentTiles(startRow, startCol, startTile.tag, matchingTiles, balloon);

        // If there are more than two matching tiles, destroy them
        if (matchingTiles.Count >= 2)
        {
            foreach (GameObject matchingTile in matchingTiles)
            {
                var Coords = GetCoordFromTile(matchingTile);
                int tileRow = Coords.Item1;
                int tileCol = Coords.Item2;
                GameObject shatteredCube = shatteredCubePool.GetShatteredCubeFromPool(matchingTile.transform.position);
                if (shatteredCube != null)
                {
                    shatteredCube.SetActive(true);
                    shatteredCube.GetComponent<ShatteredCube>().PlayShatterAnimation();
                    _audioManager.PlaySound(0);
                }               
                _gridGenerator.instantiatedPrefabs[tileRow, tileCol] = null;
                if (matchingTile.CompareTag(manager.goalCubeTag))
                {
                    //Vector3[] path = new Vector3[] { matchingTile.transform.position, goalPosition };
                    //float duration = 1.0f;
                    //matchingTile.transform.DOPath(path, duration).SetEase(Ease.Linear).OnComplete(() => Destroy(matchingTile));
                    //manager.UpdateGoal();
                    //_audioManager.PlaySound(2);
                    tilesToMove.Enqueue(matchingTile);
                }
                else Destroy(matchingTile);
            }
            StartCoroutine(MoveTilesToGoal());
            Balloon.Instance.DestroyBalloon();
            if (matchingTiles.Count >= 5)
            {
                GridGenerator.Instance.RocketCreation(startCol, startRow);                
            }
            StartCoroutine(WaitforDestroy(0.3f));            
        }
    }
    IEnumerator MoveTilesToGoal()
    {
        float delay = 0.05f;  // The consistent delay before each tile starts moving

        while (tilesToMove.Count > 0)
        {
            yield return new WaitForSeconds(delay);  // Wait for the delay

            GameObject tileToMove = tilesToMove.Dequeue();
            Vector3[] path = new Vector3[] { tileToMove.transform.position, goalPosition };
            float duration = 1.0f;

            tileToMove.transform.DOPath(path, duration).SetEase(Ease.Linear).OnComplete(() =>
            {
                Destroy(tileToMove);
                manager.UpdateGoal();
                _audioManager.PlaySound(2);
            });
        }
    }
    private void CheckAdjacentTiles(int row, int col, string tag, List<GameObject> matchingTiles, List<GameObject> balloon)
    {
        // Check the top neighbor
        CheckNeighborAndAdd(row - 1, col, tag, matchingTiles, balloon);

        // Check the bottom neighbor
        CheckNeighborAndAdd(row + 1, col, tag, matchingTiles, balloon);

        // Check the left neighbor
        CheckNeighborAndAdd(row, col - 1, tag, matchingTiles, balloon);

        // Check the right neighbor
        CheckNeighborAndAdd(row, col + 1, tag, matchingTiles, balloon);
    }

    private void CheckNeighborAndAdd(int row, int col, string tag, List<GameObject> matchingTiles, List<GameObject> balloon)
    {

        if (row >= 0 && row < _gridGenerator.rows && col >= 0 && col < _gridGenerator.columns)
        {
            GameObject neighborTile = _gridGenerator.GetCell(row, col);
            if (neighborTile.CompareTag("Balloon"))
            {
                Balloon.Instance.AddToBalloon(neighborTile);
            }
            if (neighborTile.CompareTag(tag) && !matchingTiles.Contains(neighborTile))
            {
                matchingTiles.Add(neighborTile);
                CheckAdjacentTiles(row, col, tag, matchingTiles, balloon);
            }
        }
    }
    
    public (int,int) GetCoordFromTile(GameObject tile)
    {
        for (int row = 0; row < _gridGenerator.rows; row++)
        {
            for (int col = 0; col < _gridGenerator.columns; col++)
            {
                if (_gridGenerator.instantiatedPrefabs[row, col] == tile)
                {
                    return (row, col);
                }
            }
        }
        return (-1,-1); 
    }

    // Sýkýntý yaþarsan aynýsýnýn GetRowFromTile olaný vardý bir öncekinde return row dönen
    //public int GetColumnFromTile(GameObject tile)
    //{
    //    for (int row = 0; row < _gridGenerator.rows; row++)
    //    {
    //        for (int col = 0; col < _gridGenerator.columns; col++)
    //        {
    //            if (_gridGenerator.instantiatedPrefabs[row, col] == tile)
    //            {
    //                return col;
    //            }
    //        }
    //    }
    //    return -1; 
    //}
    public void CheckAndHandleFalling()
    {
        bool stillFalling = true;
        while (stillFalling)
        {
            stillFalling = false;
            for (int col = 0; col < _gridGenerator.columns; col++)
            {
                for (int row = 0; row < _gridGenerator.rows; row++)
                {
                    if (_gridGenerator.instantiatedPrefabs[row, col] == null)
                    {
                        // The current cell is empty, find a cell above to fall down
                        for (int aboveRow = row + 1; aboveRow < _gridGenerator.rows; aboveRow++)
                        {
                            if (_gridGenerator.instantiatedPrefabs[aboveRow, col] != null)
                            {
                                stillFalling = true;
                                // Found a cell to fall down
                                _gridGenerator.instantiatedPrefabs[aboveRow - 1, col] = _gridGenerator.instantiatedPrefabs[aboveRow, col];


                                Vector2 vector = _gridGenerator.instantiatedPrefabs[aboveRow - 1, col].transform.position;
                                vector.y -= _gridGenerator.CellSize.y;
                                _gridGenerator.instantiatedPrefabs[aboveRow - 1, col].transform.position = vector;
                                _gridGenerator.instantiatedPrefabs[aboveRow, col] = null;
                            }
                        }
                    }
                }
            }
        }
    }
    public void CheckAndFill()
    {
        for (int col = 0; col < _gridGenerator.columns; col++)
        {
            for (int row = 0; row < _gridGenerator.rows; row++)
            {
                if (_gridGenerator.instantiatedPrefabs[row, col] == null)
                {
                    int random;
                    random = UnityEngine.Random.Range(0, 5);
                    previousRandom = random;
                    _gridGenerator.Creation(col, row, random);
                }
            }
        }
    }

    public IEnumerator WaitforDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        CheckAndHandleFalling();
        CheckAndFill();
    }
   
}


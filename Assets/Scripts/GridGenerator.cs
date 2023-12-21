using UnityEngine;
using Unity;
using System.Collections.Generic;

public class GridGenerator : MonoBehaviour
{
    public GameObject quad; 
    [HideInInspector] public int rows;
    [HideInInspector] public int columns;
    private Vector3 startPosition;
    public Vector3 StartPosition
    {
        get { return startPosition; }
        set { startPosition = value; }
    }
    public GameObject[] tilePrefabs;
    public GameObject[] rocketPrefabs;
    int previousRandom;
    public GameObject[,] instantiatedPrefabs;
    Vector3 quadSize;
    Vector3 cellSize;
    public Vector3 CellSize
    {
        get { return cellSize; }
        set { cellSize = value; }
    }
    public Vector3 QuadSize
    {
        get { return quadSize; }
        set { quadSize = value; }
    }
    public static GridGenerator Instance { get; private set; }

    void Start()
    {
        Instance = this;
        StartPosition = quad.GetComponent<Renderer>().bounds.min;
        QuadSize = quad.GetComponent<Renderer>().bounds.size;
        rows = (int)QuadSize.y;
        columns = (int)QuadSize.x;
        instantiatedPrefabs = new GameObject[rows, columns];
        CreateGrid();
    }

    public void CreateGrid()
    {     
        CellSize = new Vector3(quadSize.x / columns, quadSize.y / rows, 1f);
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                int random;
                random = Random.Range(0, 6);
                Creation(col, row, random);              
            }
        }
    }
    public void Creation(int col, int row, int random)
    {
        previousRandom = random;
        float xPos = (col * cellSize.x + cellSize.x / 2) + startPosition.x;
        float yPos = (row * cellSize.y + cellSize.y / 2) + startPosition.y;
        Vector3 position = new Vector3(xPos, yPos, 0f);
        GameObject cell = Instantiate(tilePrefabs[random], position, tilePrefabs[random].transform.rotation);
        cell.transform.parent = transform;
        instantiatedPrefabs[row, col] = cell;
    }
    public void RocketCreation(int col, int row)
    {
        int rand = UnityEngine.Random.Range(0,2);
        float xPos = (col * cellSize.x + cellSize.x / 2) + startPosition.x;
        float yPos = (row * cellSize.y + cellSize.y / 2) + startPosition.y;
        Vector3 position = new Vector3(xPos, yPos, 0f);
        GameObject cell = Instantiate(rocketPrefabs[rand], position, rocketPrefabs[rand].transform.rotation);
        cell.transform.parent = transform;
        instantiatedPrefabs[row, col] = cell;
    }
    public GameObject GetCell(int row, int col)
    {
        if (row >= 0 && row < rows && col >= 0 && col < columns)
        {
            return instantiatedPrefabs[row, col];
        }
        else
        {
            Debug.LogError("Row or column index out of bounds.");
            return null;
        }
    }
    public Vector3 GetStartPosition()
    {
        return quad.transform.position - quadSize / 2f;
    }

    public Vector3 GetCellSize()
    {
        return cellSize;
    }
    public void ChangeQuadSize(int Width, int Height)
    {
        quad.GetComponent<RectTransform>().localScale = new Vector3(Width, Height, 1f);
    }
}
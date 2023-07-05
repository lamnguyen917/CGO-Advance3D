using System;
using UnityEngine;

[Flags]
enum Direction // wall direction
{
    None = 0,
    Left = 1,
    Top = 2,
    Right = 4,
    Bottom = 8
}

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private int size = 10;
    [SerializeField] int cellSize = 10;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject floorPrefab;


    private int[,] _cells;

    private void Start()
    {
        _cells = new int[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                _cells[i, j] = (int)(Direction.Bottom | Direction.Left | Direction.Right | Direction.Top);
            }
        }

        Render();
    }

    void Render()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                CreateCell(j, i, _cells[i, j]);
            }
        }
    }

    void CreateCell(int x, int z, int value)
    {
        GameObject floor = Instantiate(floorPrefab, new Vector3(x * cellSize, 0, -z * cellSize), Quaternion.identity);
        floor.name = $"CELL-{x}-{z}-{value}";
        var hasTopWall = (value & (int)Direction.Top) == (int)Direction.Top;
        var hasLeftWall = (value & (int)Direction.Left) == (int)Direction.Left;
        var hasRightWall = (value & (int)Direction.Right) == (int)Direction.Right;
        var hasBottomWall = (value & (int)Direction.Bottom) == (int)Direction.Bottom;

        if (hasTopWall)
        {
            GameObject wall = Instantiate(wallPrefab, floor.transform);
            wall.transform.localPosition = new Vector3(0, 0, 5);
            wall.transform.localEulerAngles = new Vector3(0, 0, 0);
        }

        if (hasBottomWall)
        {
            GameObject wall = Instantiate(wallPrefab, floor.transform);
            wall.transform.localPosition = new Vector3(0, 0, -5);
            wall.transform.localEulerAngles = new Vector3(0, 0, 0);
        }

        if (hasLeftWall)
        {
            GameObject wall = Instantiate(wallPrefab, floor.transform);
            wall.transform.localPosition = new Vector3(-5, 0, 0);
            wall.transform.localEulerAngles = new Vector3(0, 90, 0);
        }

        if (hasRightWall)
        {
            GameObject wall = Instantiate(wallPrefab, floor.transform);
            wall.transform.localPosition = new Vector3(5, 0, 0);
            wall.transform.localEulerAngles = new Vector3(0, 90, 0);
        }
    }
}

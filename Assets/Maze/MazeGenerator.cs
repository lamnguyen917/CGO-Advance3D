using System;
using System.Collections.Generic;
using UnityEngine;

[Flags]
enum Direction // wall direction
{
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
    [SerializeField] private GameObject doorPrefab;
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


        Generate();
        Render();
    }

    void Generate()
    {
        // for (int i = 0; i < size; i++)
        // {
        //     for (int j = 0; j < size; j++)
        //     {
        //         Direction[] nextDirections =
        //         {
        //             Direction.Bottom,
        //             Direction.Left,
        //             Direction.Right,
        //             Direction.Top
        //         };
        //         foreach (var direction in nextDirections)
        //         {
        //             BreakWall(i, j, direction);
        //         }
        //     }
        // }
        Visit(0, 0);
    }

    void Visit(int row, int col)
    {
        Debug.Log($"Visit {row} - {col}");
        Direction[] nextDirections =
        {
            Direction.Bottom,
            Direction.Left,
            Direction.Right,
            Direction.Top
        };
        int n = nextDirections.Length;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, 4);
            (nextDirections[k], nextDirections[n]) = (nextDirections[n], nextDirections[k]);
        }

        foreach (var direction in nextDirections)
        {
            // Debug.Log(direction);
            // var currentRow = row;
            // var currenCol = col;
            // var nextRow = currentRow;
            // var nextCol = currenCol;
            //
            // if (direction == Direction.Left) nextCol = currenCol - 1;
            // if (direction == Direction.Right) nextCol = currenCol + 1;
            // if (direction == Direction.Top) nextRow = currentRow - 1;
            // if (direction == Direction.Bottom) nextRow = currentRow + 1;
            //
            // if (nextRow < 0 || nextRow >= size) return;
            // if (nextCol < 0 || nextCol >= size) return;
            // if (_cells[nextRow, nextCol] != (int)(Direction.Bottom | Direction.Left | Direction.Right | Direction.Top))
            //     continue;
            //
            // _cells[currentRow, currenCol] -= (int)direction;
            // _cells[nextRow, nextCol] -= (int)GetOpposite(direction);
            // Visit(nextRow, nextCol);
            BreakWall(row, col, direction);
        }
    }

    Direction GetOpposite(Direction direction)
    {
        if (direction == Direction.Left) return Direction.Right;
        if (direction == Direction.Right) return Direction.Left;
        if (direction == Direction.Bottom) return Direction.Top;
        return Direction.Bottom;
    }

    void BreakWall(int currentRow, int currenCol, Direction direction)
    {
        var nextRow = currentRow;
        var nextCol = currenCol;

        if (direction == Direction.Left) nextCol = currenCol - 1;
        if (direction == Direction.Right) nextCol = currenCol + 1;
        if (direction == Direction.Top) nextRow = currentRow - 1;
        if (direction == Direction.Bottom) nextRow = currentRow + 1;

        if (nextRow < 0 || nextRow >= size) return;
        if (nextCol < 0 || nextCol >= size) return;
        if (_cells[nextRow, nextCol] != (int)(Direction.Bottom | Direction.Left | Direction.Right | Direction.Top))
            return;

        _cells[currentRow, currenCol] -= (int)direction;
        _cells[nextRow, nextCol] -= (int)GetOpposite(direction);
        Visit(nextRow, nextCol);
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
        else
        {
            GameObject wall = Instantiate(doorPrefab, floor.transform);
            wall.transform.localPosition = new Vector3(0, 0, 5);
            wall.transform.localEulerAngles = new Vector3(0, 0, 0);
            wall.name = "door";
        }

        if (hasBottomWall)
        {
            GameObject wall = Instantiate(wallPrefab, floor.transform);
            wall.transform.localPosition = new Vector3(0, 0, -5);
            wall.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            GameObject wall = Instantiate(doorPrefab, floor.transform);
            wall.transform.localPosition = new Vector3(0, 0, -5);
            wall.transform.localEulerAngles = new Vector3(0, 0, 0);
            wall.name = "door";
        }

        if (hasRightWall)
        {
            GameObject wall = Instantiate(wallPrefab, floor.transform);
            wall.transform.localPosition = new Vector3(5, 0, 0);
            wall.transform.localEulerAngles = new Vector3(0, 90, 0);
        }
        else
        {
            GameObject wall = Instantiate(doorPrefab, floor.transform);
            wall.transform.localPosition = new Vector3(5, 0, 0);
            wall.transform.localEulerAngles = new Vector3(0, 90, 0);
            wall.name = "door right";
        }

        if (hasLeftWall)
        {
            GameObject wall = Instantiate(wallPrefab, floor.transform);
            wall.transform.localPosition = new Vector3(-5, 0, 0);
            wall.transform.localEulerAngles = new Vector3(0, 90, 0);
        }
        else
        {
            GameObject wall = Instantiate(doorPrefab, floor.transform);
            wall.transform.localPosition = new Vector3(-5, 0, 0);
            wall.transform.localEulerAngles = new Vector3(0, 90, 0);
            wall.name = "door left";
        }

        // GameObject topWall = Instantiate(hasTopWall ? wallPrefab : doorPrefab, floor.transform);
        // topWall.transform.localPosition = new Vector3(0, 0, 5);
        // topWall.transform.localEulerAngles = new Vector3(0, 0, 0);
        //
        // GameObject bottomWall = Instantiate(hasBottomWall ? wallPrefab : doorPrefab, floor.transform);
        // bottomWall.transform.localPosition = new Vector3(0, 0, -5);
        // bottomWall.transform.localEulerAngles = new Vector3(0, 0, 0);
        //
        // GameObject leftWall = Instantiate(hasLeftWall ? wallPrefab : doorPrefab, floor.transform);
        // leftWall.transform.localPosition = new Vector3(5, 0, 0);
        // leftWall.transform.localEulerAngles = new Vector3(0, 90, 0);
        //
        // GameObject rightWall = Instantiate(hasRightWall ? wallPrefab : doorPrefab, floor.transform);
        // rightWall.transform.localPosition = new Vector3(-5, 0, 0);
        // rightWall.transform.localEulerAngles = new Vector3(0, 90, 0);
    }
}

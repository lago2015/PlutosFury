using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeGenerator : MonoBehaviour
{
    private enum Room
    {
        start = -1,
        end = -2,
        fourWall = 0,
        threeWallOpenRight = 1,
        threeWallOpenBottom = 2,
        threeWallOpenLeft = 3,
        threeWallOpenTop = 4,
        twoWallOpenTopAndBottom = 5,
        twoWallOpenLeftAndRight = 6,
        twoWallOpenTopAndLeft = 7,
        twoWallOpenTopAndRight = 8,
        twoWallOpenBottomAndLeft = 9,
        twoWallOpenBottomAndRight = 10,
        oneWallClosedRight = 11,
        oneWallClosedBottom = 12,
        oneWallClosedLeft = 13,
        oneWallClosedTop = 14
    };

    public GameObject testTile;
    public GameObject startEndTestTile;

    public int height = 5;
    public int width = 5;

    public float tileHeight = 9.0f;
    public float tileWidth = 16.0f;

    private Room[] maze;

    private int indexX;
    private int indexY;

    private int index;
    // REFERENCE
    // index = indexY * width + indexX;//

    // Use this for initialization
    void Start ()
    {   
        // Initialize the maze size
        maze = new Room[height * width];

        GenerateMaze();
    }

    void GenerateMaze ()
    {
        ClearMaze();
        PickStartRoom();
        PickEndRoom();
        DrawTestTiles();
    }

    // Set all elements of the maze to 0
    void ClearMaze ()
    {
        for (int i = 0; i < maze.Length; i++)
            maze[i] = Room.fourWall;
    }

    void PickStartRoom ()
    {
        int randRow = Random.Range(0, height);
        SetElement(randRow, 0, Room.start);
    }
    
    void PickEndRoom ()
    {
        int randRow = Random.Range(0, height);
        SetElement(randRow, width - 1, Room.end);
    }

    // Set the value of the maze element at a specific column and row
    void SetElement (int row, int col, Room type)
    {
        maze[row * width + col] = type;
    }

    Room GetElement (int row, int col)
    {
        return maze [row * width + col];
    }

    // Get the index for an element at a specific column and row
    int GetIndex (int row, int col)
    {
        return row * width + col;
    }

    // Get the X grid value from our array index
    int GetColumn (int index)
    {
        return index % width;
    }

    // Get the Y grid value from our array index
    int GetRow (int index)
    {
        return index / width;
    }

    // Helper function to draw out the maze with colored tiles
    void DrawTestTiles ()
    {
        for (int i = 0; i < maze.Length; i++)
        {
            if (maze[i] == Room.start || maze[i] == Room.end)
            {
                GameObject tile = GameObject.Instantiate(startEndTestTile);
                tile.transform.position = new Vector3(GetColumn(i) * tileWidth, GetRow(i) * tileHeight, 0.0f);
                tile.transform.rotation = Quaternion.identity;
                tile.transform.parent = this.transform;
                tile.name = "Row: " + GetRow(i) + " | Col: " + GetColumn(i);
                Debug.Log("Start/End Row: " + GetRow(i) + " | Col: " + GetColumn(i));
            }
            else
            {
                GameObject tile = GameObject.Instantiate(testTile);
                tile.transform.position = new Vector3(GetColumn(i) * tileWidth, GetRow(i) * tileHeight, 0.0f);
                tile.transform.rotation = Quaternion.identity;
                tile.transform.parent = this.transform;
                tile.name = "Row: " + GetRow(i) + " | Col: " + GetColumn(i);
            }
        }
    }
}

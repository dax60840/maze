using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/*CELL VALUES:
 * -1 = outer walls
 * 0 = walls
 * 
*/

public class MazeGenerator : MonoBehaviour {

    GameObject M;
    public Cell[,] Maze;
    public int arrayHeight;
    public int arrayWidth;
    public int centerWidth;
    public int centerHeight;
    public GameObject outerWall;
    public GameObject wall;
    string generationTime;



    void Start () {

        Debug.Log("arrayHeight:" + arrayHeight + " arrayWidth:" + arrayWidth);

        M = GameObject.Find("Maze");

        Create();
	}

    public void CreateTransition()
    {
        Cell[,] tempMaze = new Cell[arrayHeight, arrayWidth]; ;
        Init(tempMaze);
        Generate(tempMaze);
        DigCenter(tempMaze);

        
    }


    public void Create() {

        Clear();

        Maze = new Cell[arrayHeight,arrayWidth];

        var watch = System.Diagnostics.Stopwatch.StartNew();
        Init(Maze);
        Generate(Maze);
        DigCenter(Maze);
        Construct(Maze);

        watch.Stop();
        generationTime = watch.ElapsedMilliseconds.ToString();
        Debug.Log("Generate in " + generationTime + "ms");
    }


    void Clear()
    {
        foreach (Transform child in M.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    void DigCenter(Cell[,] maze)
    {
        int X_mazeStart = (arrayHeight / 2) - (centerHeight / 2) + 1;
        int Y_mazeStart = (arrayWidth / 2) - (centerWidth / 2) + 1;

        for(int x = X_mazeStart; x < X_mazeStart + centerHeight ; x++)
        {
           for (int y = Y_mazeStart; y < Y_mazeStart + centerWidth; y++)
           {
                maze[x, y].Element = null;
                maze[x, y].Value = 0;
           }
        }
    }


    void Init(Cell[,] maze)
    {
        for (int x = 0; x < arrayHeight; x++)
        {
            for (int y = 0; y < arrayWidth; y++)
            {
                maze[x, y] = new Cell(0, null, x, y);

                if (x == 0 || x == arrayHeight - 1 || y == 0 || y == arrayWidth - 1)
                {
                    maze[x, y].Value = -1;
                    maze[x, y].Element = outerWall;

                }
                else if (x % 2 == 0 || y % 2 == 0)
                {
                    maze[x, y].Value = 1;
                    maze[x, y].Element = wall;
                }
                else
                {
                    maze[x, y].Value = 0;
                    maze[x, y].Element = null;
                }
            }
        }
    }



    void Generate(Cell[,] maze)
    {


        /*
        create a array or list to hold a list of last visited cells
        and call it lastCell,we will use it as a stack.
        int totalCells = total number of cells present.
        int currentCell = 0;
        int visitedCells = 0;

        currentCell = choose a cell at random.
        visitedCells++; increment the value of visitedCells

        while visitedCells < totalCells
            find all neighbors of currentCell which haven't visited yet.  
                if one or more found
                    choose one at random
                    destroy or remove the wall between it and currentCell
                    add currentCell location on the lastCell.
                    make that random neighbor cell currentCell
                    visitedCells++
                else 
                    get the most recent entry off the lastCell
                    make it currentCell
                endIf
        endWhile
            */

        Stack<Cell> lastCell = new Stack<Cell>();
        List<Cell> Neighbors = new List<Cell>();
        int totalCells = (arrayHeight/2) * (arrayWidth/2);
        int X_currentCell = 0;
        int Y_currentCell = 0;
        int visitedCells = 0;
        
        X_currentCell = UnityEngine.Random.Range(1, arrayHeight-1);
        if (X_currentCell % 2 == 0)
            X_currentCell++;
        Y_currentCell = UnityEngine.Random.Range(1, arrayWidth-1);
        if (Y_currentCell % 2 == 0)
            Y_currentCell++;

        try {
            maze[X_currentCell, Y_currentCell].visited = true;
        }
        catch
        {
            Debug.Log("inspector bug in Unity");
        }
        visitedCells++;

        while (visitedCells < totalCells || lastCell.Count > 0 )
        {
            Neighbors.Clear();

            if (X_currentCell + 2 < arrayHeight -1 && maze[X_currentCell + 2, Y_currentCell].visited != true)
            {
                Neighbors.Add(maze[X_currentCell + 2, Y_currentCell]);
            }
            if (X_currentCell - 2 > 0 && maze[X_currentCell - 2, Y_currentCell].visited != true)
            {
                Neighbors.Add(maze[X_currentCell - 2, Y_currentCell]);
            }
            if (Y_currentCell + 2 < arrayWidth -1 && maze[X_currentCell, Y_currentCell + 2].visited != true)
            {
                Neighbors.Add(maze[X_currentCell, Y_currentCell + 2]);
            }
            if (Y_currentCell - 2 > 0 && maze[X_currentCell, Y_currentCell - 2].visited != true)
            {
                Neighbors.Add(maze[X_currentCell, Y_currentCell - 2]);
            }

            if (Neighbors.Count != 0)
            {
                Cell nextCell = Neighbors[UnityEngine.Random.Range(0, Neighbors.Count)];
                int wallX = (nextCell.X + X_currentCell) / 2;
                int wallY = (nextCell.Y + Y_currentCell) / 2;
                maze[wallX, wallY].Value = 0;
                maze[wallX, wallY].Element = null;
                lastCell.Push(maze[X_currentCell, Y_currentCell]);
                X_currentCell = nextCell.X;
                Y_currentCell = nextCell.Y;

                visitedCells++;
                maze[X_currentCell, Y_currentCell].visited = true;
            }
            else
            {
                var temp = lastCell.Pop();
                X_currentCell = temp.X;
                Y_currentCell = temp.Y;
            }
        } 
    }



    void Construct(Cell[,] maze)
    {
        for (int x = 0; x < arrayHeight; x++)
        {
            for (int y = 0; y < arrayWidth; y++)
            {
                if(maze[x,y].Element != null)
                {
                    GameObject temp = Instantiate(maze[x, y].Element, new Vector3(x, 1, y), Quaternion.identity) as GameObject;
                    temp.transform.parent = M.transform;
                }

            }
        }
    }
}

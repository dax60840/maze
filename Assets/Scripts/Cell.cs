using UnityEngine;
using System.Collections;

public class Cell {
    
    public int Value;
    public GameObject Element;
    public bool visited; //useful to MazeGenerate()
    public int X;
    public int Y;

    public Cell(int value, GameObject element, int x, int y)
    {
        Value = value;
        Element = element;
        X = x;
        Y = y;
        visited = false;
    }
}

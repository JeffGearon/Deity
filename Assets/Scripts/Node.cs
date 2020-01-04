using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public SpriteRenderer rend;    

    public int gridX;
    public int gridY;

    public Node parent;

    public TerrainSquare relativeTerrainSquare;
 
    public int fCost;   // the combined value of the hCost and gCost 
    public int hCost;   // the distance the node is to the end nodea
    public int gCost;  // the distance the node is to the start node

    public bool traversable; // can the node be moved across    

    private void OnMouseDown()
    {
        Debug.Log(name + " was Clicked!");
        Debug.Log("Clicked node grid reference: " + gridX + " , " + gridY);

        Debug.Log(relativeTerrainSquare.name);
        Debug.Log("Clicked terrain square grid reference: " + relativeTerrainSquare.gridX + " ,a "+ relativeTerrainSquare.gridY);
        Debug.Log(name + relativeTerrainSquare.contains);
        Debug.Log(name + relativeTerrainSquare.containsObject);
    }
}

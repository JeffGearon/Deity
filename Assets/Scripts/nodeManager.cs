using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nodeManager : MonoBehaviour
{
   public Node node; // the base node to be instantiated at each position
   
   public Node [,] nodes;

   public TerrainManager terrainManager;      

    public void GenerateNodes()
    {
        terrainManager = FindObjectOfType<TerrainManager>();        
        nodes = new Node[terrainManager.gridSizeX, terrainManager.gridSizeY];
               
        for (int x = 0; x < terrainManager.gridSizeX; x++)
        {
            for (int y = 0; y < terrainManager.gridSizeY; y++)
            {
                Node newNode = Instantiate(node, new Vector3(x, y, 0), transform.rotation);
                newNode.transform.parent = transform;      // assigns newNode to be a child to the node manager
                newNode.name = ("node(" + x + "," + y + ")");   // assigns new name to newNode

                newNode.gridX = x;      // assigns the int values to the newNodes grid pos for distance calculations
                newNode.gridY = y;

                nodes[x, y] = newNode;                          // assigns newNode to the nodes array                              

                newNode.traversable = true;
                
                newNode.relativeTerrainSquare = terrainManager.terrainGrid[x, y];
                terrainManager.terrainGrid[x, y].relativeNode = newNode;
            }
        }
        Debug.Log("MapGen Complete");             
    }

    public void ClearMap()
    {
        int childMarker = transform.childCount;

        for (int i = 0; i < childMarker; i++)
        {
            DestroyImmediate(transform.GetChild(0).transform.gameObject);           
        }
    }
}

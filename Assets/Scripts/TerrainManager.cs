using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public List<TerrainSquare> freeTerrain;     // contains all tiles that DO NOT have objects placed on them
    public List<TerrainSquare> occupiedTerrain; // contains all tiles that currently have objects on them

    public List<FollowerManager> followerPriorityList; // contains list of followers waiting there turn to path find.

    public int startFollowers;

    public int gridSizeX;
    public int gridSizeY;

    private int totalSquares;               // the total number of squares on the map
    private float onePercentOfSquares;      // one percent of the total squares of the map

    public TerrainSquare[,] terrainGrid;    // the array of terrain squares that make up the map

    public TerrainSquare terrainSquare;     // the base terrain to be instantiated
    public nodeManager nodeMan;             // the reference to the node manager

    public List<Sprite> tileColours;

    public GameObject treeManager;          // the parent object for all trees on map

    public List<TreePlant> allTrees;        // a list of all trees currently on the map
    public List<FollowerManager> allFollowers; // a list of all followers currently on the map
    
    public TreePlant tree;
    public FollowerManager follower;
        
    public int mapTreePercent;              // the percentage of the squares that will be covered by trees
    private int totalStartTrees;            // the number of squares that will have a tree on it at the start of the game    

    public void Awake()
    {        
        GenerateMap();

        nodeMan.GenerateNodes();

        PlaceTrees();

        PlaceStarterFollowers();
    }

    public void GenerateMap()
    {
        totalSquares = gridSizeX * gridSizeY;           // calulate the total number of tiles.
        onePercentOfSquares = totalSquares * 0.01f;     // calculate how many tiles is one percent of the total.

        nodeMan = FindObjectOfType<nodeManager>(); 

        terrainGrid = new TerrainSquare[gridSizeX,gridSizeY];

      

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                TerrainSquare newTerrainSquare = Instantiate(terrainSquare, new Vector3(x, y, 0), transform.rotation);
                newTerrainSquare.transform.parent = transform;
                newTerrainSquare.name = ("Terrain: " + x + "," + y);

                newTerrainSquare.gridX = x;
                newTerrainSquare.gridY = y;

                freeTerrain.Add(newTerrainSquare);    // adding the new terrain to the free terrain list so that objects can be placed on it
                //Debug.Log("Free Terrain Count: " + freeTerrain.Count);
                newTerrainSquare.isEmpty = true;    // setting the bool isEmpty to true so that objects can be placed within it

                terrainGrid[x, y] = newTerrainSquare;                
            }
        }        
    }    

    public void ClearMap()
    {
        int childMarker = transform.childCount;

        // loop through all the children of the terrain manager

        for (int i = 0; i < childMarker; i++)
        {
            DestroyImmediate(transform.GetChild(0).transform.gameObject);            
        }

        nodeMan.ClearMap();

        freeTerrain.Clear();
        occupiedTerrain.Clear();
        allTrees.Clear();

        int treeMarker = treeManager.transform.childCount;

        for (int i = 0; i < treeMarker; i++)
        {
            DestroyImmediate(treeManager.transform.GetChild(0).transform.gameObject);
        }
    }

    /* The method for the initial placement of trees on the map.
     
       Places trees until either there are no free squares or the number of trees is equal to the total number of trees to be placed.
    */

    public void PlaceTrees()
    {
        totalStartTrees = Mathf.RoundToInt (onePercentOfSquares* mapTreePercent);       // working out how many trees to place based on the percentage set publicly
        Debug.Log("Total number of trees to be place: " + totalStartTrees);

        do
        {
            TerrainSquare randomTerrainSquare = freeTerrain[Random.Range(0, freeTerrain.Count)];     // grabbing a random terrain Square form the free terrain list

            freeTerrain.Remove(randomTerrainSquare);      // removing the terrain Square from the free terrain list.  This means new objects can now longer be placed on it

            Debug.Log("Random Free Terrain Square: " + randomTerrainSquare.name);
            Debug.Log("Free Terrain Count: " + freeTerrain.Count);
            occupiedTerrain.Add(randomTerrainSquare);     // adding the terrain Square to the occupied list.

            TreePlant newTree = Instantiate(tree, randomTerrainSquare.transform.position, transform.rotation);        // instantiating a new tree at the position of the random terrain Square}
            allTrees.Add(newTree);
            newTree.transform.parent = treeManager.transform;               // assigns the parent of newTree to the treeManager

            randomTerrainSquare.contains = 1;            
            randomTerrainSquare.containsObject = newTree.gameObject;        // assigns the game object contained within the randomTerrainSquare as the newTree
            Debug.Log("TerrainSquare contains: " + randomTerrainSquare.contains);

        } while (allTrees.Count < totalStartTrees);     // perform this do while loop as long as the trees count is less than the totalStartTrees variable.        
    }

    public void PlaceStarterFollowers()
    {
        int followersCount = 0;

        do 
            {
                TerrainSquare randomTerrainSquare = freeTerrain[Random.Range(0, freeTerrain.Count)];     // grabbing a random terrain Square form the free terrain list

            freeTerrain.Remove(randomTerrainSquare);      // removing the terrain Square from the free terrain list.  This means new objects can now longer be placed on it

            occupiedTerrain.Add(randomTerrainSquare);     // adding the terrain Square to the occupied list.

            Instantiate(follower, randomTerrainSquare.transform.position, transform.rotation);        // instantiating a new follower at the position of the random terrain Square}

            followersCount++;

        } while ((followersCount < startFollowers));
    }

    
}

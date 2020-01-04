using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerManager : MonoBehaviour
{
    public TerrainManager terrainManager;
    public nodeManager nodeManager;
    public BuildingManager buildManager;
    public FollowerStats myStats;    

    public GameObject wayPoint;

    public Node startNode;
    public Node targetNode;

    public GameObject target;
    private int targetType;      // the index of the object searched for.
                                 /* 0 = moving to continue search
                                  * 1 = looking for wood
                                  * 2 = moving to begin contruction
                                  * 
                                  * 
                                  */

    public Node currentNode;     

    public List<Node> openList = new List<Node> { };  // setting up the open list 
    public List<Node> closedList = new List<Node> { }; // setting up the closed list

    public List<Node> finalPath = new List<Node> { };       // setting up the final path list

    public int searchRange;             // how many squares away a follower can see and search for objects within
    

    public List<GameObject> objectsInRange; // list of objects found within range

    public List<GameObject> perimeter;      // traversable tiles on the outer ring of the searchRange.

    int waypointMarker = 0;

    public float speed;

    public float distance;
    public float distanceBuffer;

    private void Start()
    {
        terrainManager = FindObjectOfType<TerrainManager>();        
        nodeManager = FindObjectOfType<nodeManager>();
        buildManager = FindObjectOfType<BuildingManager>();
        myStats = GetComponent<FollowerStats>();

        MasterDrill();
    }

    private void Update()
    {
        if (waypointMarker < finalPath.Count)
        {
            distance = Vector3.Distance(transform.position, finalPath[waypointMarker].transform.position);
            if (Vector3.Distance(transform.position, finalPath[waypointMarker].transform.position) > distanceBuffer)
            {
                transform.position = Vector3.MoveTowards(gameObject.transform.position, finalPath[waypointMarker].transform.position, Time.deltaTime * speed);
            }
            else if (Vector3.Distance(transform.position, finalPath[waypointMarker].transform.position) < distanceBuffer)
            {
                waypointMarker++;
                Debug.Log("Path count:" + finalPath.Count);
                Debug.Log("Waypointmarker value:" + waypointMarker);
                if (target != null)
                {
                    if (Vector3.Distance(transform.position, target.transform.position) < distanceBuffer) // how close to the final target
                    {
                        Debug.Log("Target reached: now cut wood");
                        finalPath.Clear();
                        openList.Clear();
                        closedList.Clear();

                        finalPath.Clear();
                        waypointMarker = 0;

                        PerformTargetAction();
                    }
                }
            }
        }
    }
    
    private void OnMouseDown()
    {
        Debug.Log(name + " was Clicked!");
        MasterDrill();
    }

    // used to check if a follower is homeless. If yes they will search for wood and build a home.

    private void MasterDrill()
    {

        terrainManager.followerPriorityList.Add(this);          // adds this follower to the priority list.

        Debug.Log("Follower Priority List: " + terrainManager.followerPriorityList[0]);

        if (terrainManager.followerPriorityList[0] == this)     // this checks if the priority list was empty when this follower was added.
        {
            FollowerHasPriority();                              // if the priority list was empty it continues it's drills as normal.
        }
                
        Debug.Log("Master Drill called.");        
    }

    private void FollowerHasPriority()
    {
        Debug.Log(this.gameObject.name + " has priority.");

        AmIHomeless();
    }

    private void AmIHomeless()
    {
        Debug.Log("Am I homeless?");
        if (myStats.hasHome == false)
        {
            Debug.Log("Follower is homeless.");

            


                if (myStats.carriedQuantity == 0)
                    {
                        // search for wood or trees

                        targetType = 1;     // the index for targetType wood is 1.
                        FindObjectsWithinRange();

                        // if this returns no objects the follower will have to move position to search for the object somewhere else
                        SelectObjectWithinRangeAtRandom();
                        }
                else if (myStats.carriedQuantity == 1)     // either hardcoded amount or the variable of build amount for an abode (maybe small abode).
                    {
                        BuildHome();
                    }             
        }
        else if (myStats.hasHome == true)
        {
            Debug.Log("Follower has a home.");
        }
    }

    public void BuildHome()
    {
        Debug.Log("Building home.");
        targetType = 0;     // setting the target type to an empty square.
        FindBuildSpaceWithinRange();
        SelectObjectWithinRangeAtRandom();
        targetType = 2;     // this is so that the script knows what to do when is arrives.
    }

    public void BuildBuilding()                                                     // use to construct buildings.
    {
        Debug.Log("Beginning Construction.");

        Node buildCentreNode = FindGridReference(this.gameObject);         // so that building always snap to the grid.
            
        Building newBuilding = Instantiate(buildManager.abode, buildCentreNode.transform.position, buildCentreNode.transform.rotation);

        newBuilding.transform.parent = buildManager.transform;

        myStats.home = newBuilding;
        myStats.hasHome = true;
    }

    public void FindBuildSpaceWithinRange()        // target type must be changed before this method is called. This is so it searches for the correct things.
    {
        Debug.Log("Build Space search started.");
        objectsInRange = new List<GameObject>();

        int newXRef = Mathf.RoundToInt(transform.position.x);
        int newYRef = Mathf.RoundToInt(transform.position.y);

        for (int x = -searchRange; x <= searchRange; x++)
        {
            for (int y = -searchRange; y <= searchRange; y++)
            {
                if (newXRef + x >= 0 && newYRef + y >= 0 && newXRef + x < terrainManager.gridSizeX && newYRef + y < terrainManager.gridSizeY)
                {                                                                                     // target type must be changed before this method is called              
                    if (terrainManager.terrainGrid[newXRef + x, newYRef + y].contains == targetType) // TO DO must create safety if statement in cases falls out of grid range
                    {
                        GameObject foundTile = terrainManager.terrainGrid[newXRef + x, newYRef + y].gameObject;
                        objectsInRange.Add(foundTile);                        
                    }
                    else if (x == -searchRange || x == searchRange || y == -searchRange || y == searchRange)
                    {
                        if (terrainManager.terrainGrid[newXRef + x, newYRef + y].terrainType == 0)
                        {
                            perimeter.Add(terrainManager.terrainGrid[newXRef + x, newYRef + y].gameObject);
                        }
                    }
                }
            }
        }        
    }

    public void FindObjectsWithinRange()        // target type must be changed before this method is called. This is so it searches for the correct things.
    {
        perimeter.Clear();                      // clearing the perimeter list to be recalculated when needed.

        Debug.Log("Object search started.");
        objectsInRange = new List<GameObject>();

        int newXRef = Mathf.RoundToInt(transform.position.x);
        int newYRef = Mathf.RoundToInt(transform.position.y);

        for (int x = -searchRange; x <= searchRange; x++)
        {
            for (int y = -searchRange; y <= searchRange; y++)
            {
                if (newXRef + x >= 0 && newYRef + y >= 0 && newXRef + x < terrainManager.gridSizeX && newYRef + y < terrainManager.gridSizeY)
                {                                                                                     // target type must be changed before this method is called              
                    if (terrainManager.terrainGrid[newXRef + x, newYRef + y].contains == targetType) // TO DO must create safety if statement in cases falls out of grid range
                    {
                        GameObject foundObject = terrainManager.terrainGrid[newXRef + x, newYRef + y].containsObject;
                        objectsInRange.Add(foundObject);
                        Debug.Log("Object Found");
                    }
                    else if (x == -searchRange || x == searchRange || y == -searchRange || y == searchRange)
                    {
                        if (terrainManager.terrainGrid[newXRef + x, newYRef + y].terrainType == 0)
                        {                            
                            perimeter.Add(terrainManager.terrainGrid[newXRef + x, newYRef + y].gameObject);                            
                        }
                    }
                }
            }
        }
        // if this returns no objects the follower will have to move position to search for the object somewhere else
        SelectObjectWithinRangeAtRandom();
    }

    public void SelectObjectWithinRangeAtRandom()
    {
        Debug.Log("objects in range count: " + objectsInRange.Count);
        if (objectsInRange.Count != 0)
        {
            target = objectsInRange[Random.Range(0, objectsInRange.Count)];
            perimeter.Clear();  // clearing the perimeter list to be recalculated when needed.
            FindRoute();
        }
        else
        {
            Debug.Log("Moving to new space to continue search.");
            targetType = 0;     // the target type for an empty tile.

            Debug.Log("Target to be set to perimeter tile.");
            target = perimeter[Random.Range(0, perimeter.Count)];
                        
            FindRoute();
        }
    }

    public void FindRoute()
    {
        startNode = FindGridReference(gameObject); //finds the grid reference for the follower 
        Debug.Log("Start reference found: " + startNode.name);

        if (target != null)     // checking if the target exists
        {
            targetNode = FindGridReference(target);         //finds the grid reference for the target
            Debug.Log("Target reference found: " + targetNode.name);
        }
           

        openList.Add(startNode);
        startNode.fCost = 0;
        Debug.Log("The start node has been added");

        while (openList.Count > 0)
        {
            currentNode = openList[0];
            
            for (int i = 0; i < openList.Count; i++)
            {                
                if (currentNode.fCost > openList[i].fCost || currentNode.fCost == openList[i].fCost && currentNode.hCost > openList[i].hCost)   // if the i node has better potential
                {
                    currentNode = openList[i];                    
                }
            }

            openList.Remove(currentNode);       // removing the node with the lowest fCost from the openList
            closedList.Add(currentNode);        // adding the node with the lowest fCost from the openList

            if (currentNode == targetNode)
            {
                finalPath = RetracePath(startNode);

                Debug.Log("Final path count: " + finalPath.Count);
                
                break;
            }

            {
                foreach (Node neighbour in FindNeighbours())
                {
                    if (!neighbour.traversable || closedList.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                    if (newMovementCostToNeighbour < neighbour.gCost || !openList.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);

                        neighbour.fCost = neighbour.gCost + neighbour.hCost;

                        neighbour.parent = currentNode;

                        if (!openList.Contains(neighbour))
                        {
                            openList.Add(neighbour);
                        }
                    }
                }
            }
        }
    }

    public Node FindGridReference(GameObject testObject)
    {
        Debug.Log("Method sucessfully called");

        int newXNodeRef = Mathf.RoundToInt(testObject.transform.position.x);
        int newYNodeRef = Mathf.RoundToInt(testObject.transform.position.y);

        return nodeManager.nodes[newXNodeRef, newYNodeRef];
    } 

    public List<Node> FindNeighbours ()
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if ( x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = currentNode.gridX + x;
                int checkY = currentNode.gridY + y;

                if (checkX >= 0 && checkX < terrainManager.gridSizeX && checkY >= 0 && checkY < terrainManager.gridSizeY)
                {
                    neighbours.Add(nodeManager.nodes[checkX, checkY]);
                }
            }
        }     
        return neighbours;
    }

    public int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }
        return 14 * dstX + 10 * (dstY - dstX);
    }

    public List<Node> RetracePath(Node start)
    {
        Debug.Log("Path being retraced.");

        List<Node> path = new List<Node>();

        while (currentNode != start)
        {
            path.Add(currentNode);

            currentNode = currentNode.parent;
        }

        path.Reverse();     // reverse the order of the path list.

        Debug.Log("Path reversed.");

        // Reset the h, g, f costs of all nodes in the open and closed lists. 
        // This is done so that we can continue to find paths in the future.

        foreach (Node currentNode in openList)
        {
            currentNode.fCost = 0;
            currentNode.gCost = 0;
            currentNode.hCost = 0;
            //currentNode.parent = null;

        }

        foreach (Node currentNode in closedList)
        {
            currentNode.fCost = 0;
            currentNode.gCost = 0;
            currentNode.hCost = 0;
            //currentNode.parent = null;
        }

        Debug.Log(this.gameObject.name + ": Preparing removal from followerPriority List.");

        terrainManager.followerPriorityList.Remove(this);       // removing this follower from the priority list

        Debug.Log(this.gameObject.name + ": This follower removed from followerPriority List.");

        if (terrainManager.followerPriorityList.Count > 0)
        {
            Debug.Log("followerPriority List is not equal to null.");
            terrainManager.followerPriorityList[0].FollowerHasPriority();   // lets the next follower know it can continue it's drills.
        }


        
        return path;
    }

    public void PerformTargetAction()
    {
        if (target != null)     // make sure the target still exists
        {
            Debug.Log("Target type: " + targetType);

            if (targetType == 1)
            {
                targetNode.relativeTerrainSquare.contains = 0;      // assigning the Terrain squares contains index to 0 (meaning empty).
                                                                    // this ensures it doesn't show up when we search for objects of a specific index. e.g 1 for trees.   
                myStats.carriedQuantity = 1;
                Destroy(target);
                terrainManager.freeTerrain.Add(targetNode.relativeTerrainSquare);   // let's the terrain manager know that the terrain square associated with the target node is now free.
                AmIHomeless();
            }
            else if (targetType == 0)
            {
                Debug.Log("Destination reached.");
                AmIHomeless();
            } 
            else if (targetType == 2)
            {
                BuildBuilding();
            }
        } 

        else if ( target == null)
        {
            Debug.Log("Target is null");
            AmIHomeless();
        }
    }
}

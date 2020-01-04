using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSquare : MonoBehaviour
{
    public int gridX;
    public int gridY;

    public int terrainType;

    public Node relativeNode;

    public TerrainManager terrainManager;
    public SpriteRenderer rend;

    public bool isEmpty;             // whether the square is empty
    public int contains;      // what the square contains 
    public GameObject containsObject; // reference of the actual object contained within the square
    
    /* contains explained:  0 = empty
                            1 = tree
                            2 = bush
                            3 = wolf
                            4 = deer
                            5 = fish
                            6 = follower
                            7 = building
    */

    public void AssignTerrainType()
    {
        rend = relativeNode.GetComponent<SpriteRenderer>();

        if (terrainType == 0)
        {
            rend.color = new Color(0.2685f, 0.6698f, 0.4496f, 1f);            
            relativeNode.traversable = true;
        }
        else if (terrainType == 1)
        {
            rend.color = new Color(0.1666f, 0.6982f, 0.9056f, 1f);
            relativeNode.traversable = false;
        }
    }
    
}

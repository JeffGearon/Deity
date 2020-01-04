using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public Sprite sprite;

    public int health;

    private Node[,] builtUpArea;
    private Node[,] walkableArea;

    private Node entrance;

    public SpriteRenderer rend;
    
}

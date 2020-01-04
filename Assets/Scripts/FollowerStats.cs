using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerStats : MonoBehaviour
{  
    public int age;
    public int health;

    public int hunger;
    public int fatigue;

    public int hungerLimit;         // the level of hunger before a follower is rated as "Hungry".
    public int fatigueLimit;        // the the level of fatigue before a follower is rated as "Tired".

    public bool hasHome;          // whether the follower is homeless or not.

    public GameObject carriedObject;
    public int carriedQuantity;

    public Building home;
    public Building myTownCentre; 

    
}

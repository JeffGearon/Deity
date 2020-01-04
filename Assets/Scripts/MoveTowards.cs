using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowards : MonoBehaviour
{
    FollowerManager followerManager;
    public float speed;

    public float distance;
    public float distanceBuffer;
        
    int waypointMarker = 0;

    public List<Node> path;

    private void Start()
    {
        followerManager = GetComponent<FollowerManager>();
    }

    private void Update()
    {
       if (waypointMarker < path.Count)
        {            
            distance = Vector3.Distance(transform.position, path[waypointMarker].transform.position);
            if (Vector3.Distance(transform.position, path[waypointMarker].transform.position) > distanceBuffer)
            {                   
                transform.position = Vector3.MoveTowards(gameObject.transform.position, path[waypointMarker].transform.position, Time.deltaTime * speed);
            }
            else if (Vector3.Distance(transform.position, path[waypointMarker].transform.position) < distanceBuffer)
            {
                waypointMarker++;
                Debug.Log("Path count:" + path.Count);
                Debug.Log("Waypointmarker value:" + waypointMarker);
                if (followerManager.target != null)
                {
                    if (Vector3.Distance(transform.position, followerManager.target.transform.position) < distanceBuffer) // how close to the final target
                    {
                        Debug.Log("Target reached: now cut wood");
                        followerManager.finalPath.Clear();
                        followerManager.openList.Clear();
                        followerManager.closedList.Clear();

                        path.Clear();
                        
                        followerManager.PerformTargetAction();
                    }
                }
                
            }
        }        
    }     
}

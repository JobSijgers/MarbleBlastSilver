using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaypointTraveler : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float speed;
    [SerializeField] private float waitAtDestination; 

    private int currentWaypointIndex;
    private float timer;

    void FixedUpdate()
    {
        //checks if reached destination
        if (Vector3.Distance(waypoints[currentWaypointIndex].position, transform.position) < 0.05f)
        {
            //if at last waypoint, set first waypoint as destination
            currentWaypointIndex++;
            timer = 0;
            if (currentWaypointIndex > waypoints.Length -1)
            {
                currentWaypointIndex = 0;
            }
        }
        else
        {
            if (timer >= waitAtDestination)
            {
                //moves the gameobject to the tranform of currentwaypointindex
                transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, speed * Time.deltaTime);
            }
            else
            {
                timer += Time.deltaTime;
            }
        }

    }
}

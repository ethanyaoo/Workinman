using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandRoom : MonoBehaviour
{
    public LayerMask layerMask;
    public Recursion recursion;

    public int xPos;
    public int yPos;
    public int xMax;
    public int yMax;

    /// <summary>
    /// Used to check for rooms outside of the main path
    /// Detecting whether or not this position is a room already or not
    /// </summary>
    void Update()
    {
        // Checks if position already has a instantiated place
        Collider2D roomDetect = Physics2D.OverlapCircle(transform.position, 1, layerMask);

        // Make sure to only change position points that aren't used 
        if (roomDetect == null && recursion.stopGeneration == true)
        {
            // Check for border
            if ((xPos > 0) && (xPos < xMax - 1) && (yPos > 0) && (yPos < yMax - 1))
            {
                int rand = Random.Range(0, recursion.rooms.Length);

                Instantiate(recursion.rooms[rand], transform.position, Quaternion.identity);

                Destroy(gameObject);
            }
        }
    }
}
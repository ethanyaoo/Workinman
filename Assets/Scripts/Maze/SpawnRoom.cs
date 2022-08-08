using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoom : MonoBehaviour
{
    public LayerMask layerMask;
    public PathGeneration pathGeneration;

    /// <summary>
    /// Used to check for rooms outside of the main path
    /// Detecting whether or not this position is a room already or not
    /// </summary>
    void Update()
    {
        Collider2D roomDetect = Physics2D.OverlapCircle(transform.position, 1, layerMask);

        if (roomDetect == null && pathGeneration.stopGeneration == true)
        {
            int rand = Random.Range(0, pathGeneration.rooms.Length);

            Instantiate(pathGeneration.rooms[rand], transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}

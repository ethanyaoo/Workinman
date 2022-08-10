using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombExplosion : MonoBehaviour
{
    public Tilemap tilemap;

    public void ExplodeTile(Vector3 tilePos, float radius)
    {
        //Tile tile = tilemap.GetTile<Tile>(tilePos);

        for (int i = -(int)radius; i < radius; i++)
        {
            for (int j = -(int)radius; j < radius; j++)
            {
                Vector3Int tempTilePos = tilemap.WorldToCell(tilePos + new Vector3(i, j, 0));

                if (tilemap.GetTile(tempTilePos) != null)
                {
                    Debug.Log("Explode Tile");
                    DestroyTile(tempTilePos);
                }
            }
        }

        //tilemap.SetTile(tilePos, null);
    }

    public void DestroyTile(Vector3Int tilePos)
    {
        Debug.Log("Destroy tile");
        tilemap.SetTile(tilePos, null);
    }
}

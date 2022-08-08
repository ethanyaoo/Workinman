using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] private int pointsForCoin = 100;

    private bool collected = false;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !collected)
        {
            FindObjectOfType<GameSession>().AddToScore(pointsForCoin);

            // Plays audio at camera position
            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);

            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBomb : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rigidbody2d;
    [SerializeField] float bombSpeed = 20f;
    [SerializeField] private float bombRadius = 3f;
    [SerializeField] private float bombTimer = 0f;

    AgentMovement agentMovement;

    private float xSpeed;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        agentMovement = FindObjectOfType<AgentMovement>();

        xSpeed = agentMovement.transform.localScale.x * bombSpeed;
    }

    void Update()
    {
        rigidbody2d.velocity = new Vector2(xSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Tile"))
        {
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Tileset"))
        {
            Vector3Int vector3Int = Vector3Int.FloorToInt(collision.transform.position);

            FindObjectOfType<BombExplosion>().ExplodeTile(vector3Int, bombRadius);
        }

        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.transform.tag);

        BombExplosion bombExplosion = FindObjectOfType<BombExplosion>();

        bombExplosion.ExplodeTile(transform.position, bombRadius);

        // StartCoroutine(ExplodeBomb());

        if (!collision.transform.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        else if (collision.transform.CompareTag("Tile"))
        {
            Destroy(collision.gameObject);
        }
        else if (collision.transform.CompareTag("Tileset"))
        {
            Vector3Int vector3Int = Vector3Int.FloorToInt(collision.transform.position);

            FindObjectOfType<BombExplosion>().ExplodeTile(transform.position, bombRadius);
        }
    }

    IEnumerator ExplodeBomb()
    {
        yield return new WaitForSecondsRealtime(0);

        BombExplosion bombExplosion = FindObjectOfType<BombExplosion>();

        bombExplosion.ExplodeTile(transform.position, bombRadius);
    }
}

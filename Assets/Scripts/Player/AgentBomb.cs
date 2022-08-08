using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBomb : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rigidbody2d;
    [SerializeField] float bombSpeed = 20f;

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

        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.transform.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}

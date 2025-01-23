using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    public float speed = 2.0f; //Speed of the enemy movement
    public bool vertical = false; //Determines if the enemy moves vertically or horizontally
    public int damage = 1; //Amount of health to reduce from the player

    private float direction = 1.0f; //Direction of movement (1 for forward, -1 for backward)
    private Rigidbody2D rigidbody2d;
    private float timer;
    public float changeDirectionTime = 3.0f; //Time interval for changing direction

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>(); //Get the Rigidbody2D component for physics-based movement
        gameObject.layer = LayerMask.NameToLayer("Default"); //Set the layer to Default
        timer = changeDirectionTime; //Initialize the timer
    }

    void Update()
    {
        timer -= Time.deltaTime; //Decrease the timer over time
        if (timer <= 0)
        {
            direction = -direction; //Reverse direction when the timer expires
            timer = changeDirectionTime; //Reset the timer
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position; //Get the current position of the enemy

        if (vertical)
        {
            position.y += speed * direction * Time.deltaTime; //Move vertically
        }
        else
        {
            position.x += speed * direction * Time.deltaTime; //Move horizontally
        }

        rigidbody2d.MovePosition(position); //Update the enemy's position
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Character")) //Check if the collision is with the Character layer
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>(); //Get the PlayerController component
            if (player != null)
            {
                player.ChangeHealth(-damage); //Reduce the player's health
            }
        }
    }
}

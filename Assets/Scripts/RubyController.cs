using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public int maxHealth = 5;
    public float speed = 3.0f;
    public int health
    {
        get
        {
            return currentHealth;
        }
    }

    private int currentHealth;
    private float horizontal;
    private float vertical;
    private Rigidbody2D rigidbody2d;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();

        currentHealth = 1;
    }

    void Update()
    {
        GetInputs();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    private void GetInputs()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    public void ChangeHelth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    private float distance = 3.0f;
    private float horizontal;
    private float vertical;
    private Rigidbody2D rigidbody2d;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
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
        position.x = position.x + distance * horizontal * Time.deltaTime;
        position.y = position.y + distance * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    private void GetInputs()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCollectable : MonoBehaviour
{
    private GameState _gameState;

    private void Start()
    {
        _gameState = FindObjectOfType<GameState>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        RubyController controller = collision.GetComponent<RubyController>();

        if (controller != null)
        {
            _gameState.AmmoCount += 5;
            Destroy(gameObject);
        }
    }
}

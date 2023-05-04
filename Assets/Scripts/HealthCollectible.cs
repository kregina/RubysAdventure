using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
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
            if (_gameState.health < _gameState.maxHealth)
            {
                controller.ChangeHealth(1);
                Destroy(gameObject);
            }
        }
    }
}

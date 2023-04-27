using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        RubyController controller = collision.GetComponent<RubyController>();
        bool canAddHeath = (controller.health < controller.maxHealth);

        if (controller != null && canAddHeath)
        {
            controller.ChangeHealth(1);
            Destroy(gameObject);
        }
    }
}

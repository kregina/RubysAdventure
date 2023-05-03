using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public ParticleSystem smokeEffect;
    public Slider lifeSlider;
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;
    public int maxLife;

    private int currentDamaged = 0;
    private Rigidbody2D rigidbody2d;
    private float timer;
    private int direction = 1;
    private bool broken = true;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();

        lifeSlider.value = maxLife;
        lifeSlider.maxValue = maxLife;
        currentDamaged = maxLife;
    }

    void Update()
    {
        if (!broken)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    void FixedUpdate()
    {
        if (!broken)
        {
            return;
        }

        Vector2 position = rigidbody2d.position;

        Move(position);
    }

    private void Move(Vector2 position)
    {
        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }

        rigidbody2d.MovePosition(position);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }

    public void Fix(int damage)
    {
        currentDamaged -= damage;
        lifeSlider.value = currentDamaged;
        if (currentDamaged <= maxLife)
        {
            lifeSlider.fillRect.gameObject.SetActive(true);
            broken = false;
            rigidbody2d.simulated = false;
            animator.SetTrigger("Fixed");
            smokeEffect.Stop();
        }
    }
}

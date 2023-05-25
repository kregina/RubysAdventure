using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;
    public float timeInvincible = 2.0f;
    public GameObject projectilePrefab;
    public GameObject dialogBox;
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    public CanvasGroup exitBackgroundImageCanvasGroup;
    public Image uiAmmoImage;
    public GameObject[] ammoPrefabs;
    public AudioClip shootClip;
    public AudioClip hitSound;

    private bool isInvincible;
    private bool isAlive = true;
    private float invincibleTimer;
    private float horizontal;
    private float vertical;
    private float m_Timer;

    private GameState _gameState;
    private Rigidbody2D rigidbody2d;
    private Animator animator;
    private Vector2 lookDirection = new Vector2(1, 0);
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        _gameState = FindObjectOfType<GameState>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            Move();
        }
        Invisibility();
        Shoot();
        Talk();
        CallToogleAmmoBagVisibility();

        // this cannot be here, it'll look for this since the game start
        HandleFactoryEnter();

        if (_gameState.health == 0)
        {
            EndLevel();
        }
    }

    void CallToogleAmmoBagVisibility()
    {
        if (_gameState.canShoot)
        {
            ToogleAmmoBagVisibility(true);
        }
        else
        {
            ToogleAmmoBagVisibility(false);
        }
    }

    private void Talk()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            HandleNpcDialog();
        }
    }

    private void HandleFactoryEnter()
    {
        RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("Factory"));
        if (hit.collider != null)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                SceneManager.LoadScene(2);
            }
        }
    }

    private void HandleNpcDialog()
    {
        RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
        if (hit.collider != null)
        {
            NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
            if (character != null)
            {
                character.DisplayDialog();
                _gameState.canShoot = true;

                uiAmmoImage.gameObject.SetActive(true);
            }
        }
    }

    private void ToogleAmmoBagVisibility(bool visibility)
    {
        foreach (var item in ammoPrefabs)
        {
            if(item)
            {
                item.SetActive(visibility);
            }
        }
    }

    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_gameState.canShoot && _gameState.AmmoCount > 0)
            {
                _gameState.AmmoCount--;
                Launch();
                audioSource.PlayOneShot(shootClip);
            }
        }
    }

    private void Invisibility()
    {
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
    }

    private void Move()
    {

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (horizontal != 0 || vertical != 0)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    void FixedUpdate()
    {
        CalcPosition();
    }

    private void CalcPosition()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            animator.SetTrigger("Hit");

            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
            PlaySound(hitSound);
        }

        _gameState.health = Mathf.Clamp(_gameState.health + amount, 0, _gameState.maxHealth);
        _gameState.ChangeHealthValue(_gameState.health / (float)_gameState.maxHealth);
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("TutorialMove"))
        {
            dialogBox.SetActive(false);
            Destroy(collision.gameObject);
        }
    }

    void EndLevel()
    {
        isAlive = false;
        m_Timer += Time.deltaTime;
        exitBackgroundImageCanvasGroup.alpha = m_Timer / fadeDuration;
        float rotationSpeed = 1.0f;
        float rotationAngle = -90f;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, rotationAngle), rotationSpeed * Time.deltaTime);

        if (m_Timer > fadeDuration + displayImageDuration)
        {
            SceneManager.LoadScene(0);
        }
    }
}

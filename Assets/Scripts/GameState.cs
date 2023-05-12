using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class GameState : MonoBehaviour
{
    public Image healthBarMask;
    public static GameState Instance;

    public int health;
    public int maxHealth = 5;
    public bool canShoot;

    private int _ammoCount = 0;
    private float originalSize;

    private void Awake()
    {

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        originalSize = healthBarMask.rectTransform.rect.width;
        health = maxHealth;
        canShoot = false;
    }

    public void ChangeHealthValue(float value)
    {
        healthBarMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    }

    public int AmmoCount
    {
        get { return _ammoCount; }
        set
        {
            _ammoCount = value;
            OnAmmoCountChanged?.Invoke(_ammoCount);
        }
    }

    public event Action<int> OnAmmoCountChanged;
}

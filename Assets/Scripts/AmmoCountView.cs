using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoCountView : MonoBehaviour
{
    public TextMeshProUGUI ammoCountText;

    private GameState _gameState;

    private void Start()
    {
        _gameState = FindObjectOfType<GameState>();
        _gameState.OnAmmoCountChanged += OnAmmoCountChanged;
        UpdateAmmoCountText(_gameState.AmmoCount);
    }

    private void OnDestroy()
    {
        _gameState.OnAmmoCountChanged -= OnAmmoCountChanged;
    }

    private void OnAmmoCountChanged(int ammoCount)
    {
        UpdateAmmoCountText(ammoCount);
    }

    private void UpdateAmmoCountText(int ammoCount)
    {
        ammoCountText.text = $"{ammoCount}";
    }
}

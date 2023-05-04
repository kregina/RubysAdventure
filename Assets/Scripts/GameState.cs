using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameState : MonoBehaviour
{
    private int _ammoCount = 0;

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

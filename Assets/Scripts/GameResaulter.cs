using System;
using UnityEngine;

public class GameResaulter : MonoBehaviour
{
    public event Action<bool> OnGameResault;
    private void Start()
    {
        EventManager.Instance.AddListener(EventKey.StageEnd,new Action<bool>(Resault));
    }

    public void Resault(bool checkWin)
    {
        OnGameResault?.Invoke(checkWin);
    }
}

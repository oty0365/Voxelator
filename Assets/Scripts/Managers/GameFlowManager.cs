using System;
using System.Collections;
using UnityEngine;

public class GameFlowManager : SceneSingletonMonoBehaviour<GameFlowManager>
{
    [SerializeField] GameObject[] activationRows;
    void Start()
    {
        Activation();
    }

    private void Activation()
    {
        foreach (var obj in activationRows)
        {
            obj.SetActive(true);
        }
        MapManager.Instance.ChangeMap(MapCode.WindWildPlain);
        PlayerStatus.Instance.ResetStatus();
        Extracter.Instance.UpLoadStats();
        PlayerController.Instance.playerDash.ResetDashCoolDown();
        MapManager.Instance.InstantiateMap();
        TimeManager.Instance.StartGame();
    }

}

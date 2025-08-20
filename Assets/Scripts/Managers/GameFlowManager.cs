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
        PlayerStatus.Instance.ResetStatus();
        Extracter.Instance.UpLoadStats();
        PlayerController.Instance.playerDash.ResetDashCoolDown();
    }

}

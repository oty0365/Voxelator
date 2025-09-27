using System;
using System.Collections;
using UnityEngine;

public class GameFlowManager : SceneSingletonMonoBehaviour<GameFlowManager>
{
    [SerializeField] GameObject[] activationRows;
    public GameObject mapBanner;
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
//        MapManager.Instance.ChangeMap(MapCode.WindWildPlain);
        PlayerStatus.Instance.ResetStatus();
        Extracter.Instance.UpLoadStats();
        PlayerController.Instance.playerDash.ResetDashCoolDown();
        var map = MapManager.Instance.InstantiateMap();
        var mapSetter = map.GetComponent<MapSetter>();
        mapSetter.SetMapBanner(mapBanner);
        MapManager.Instance.SetMapSetter(mapSetter);
        TimeManager.Instance.StartGame();
    }

}

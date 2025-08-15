using UnityEngine;

public class GameFlowManager : SceneSingletonMonoBehaviour<GameFlowManager>
{
    void Start()
    {
        PlayerStatus.Instance.ResetStatus();
        Extracter.Instance.UpLoadStats();
        PlayerController.Instance.playerDash.ResetDashCoolDown();
    }

}

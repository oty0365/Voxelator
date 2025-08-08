using UnityEngine;

public class GameFlowManager : HalfSingleMono<GameFlowManager>
{
    void Start()
    {
        PlayerStatus.Instance.ResetStatus();
        Extracter.Instance.UpLoadStats();
        PlayerController.Instance.playerDash.ResetDashCoolDown();
    }

}

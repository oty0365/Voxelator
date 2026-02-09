using UnityEngine;

public class GameResaultConnector : MonoBehaviour,IConnecter
{
    [SerializeField] private GameResaulter gameResaulter;
    [SerializeField] private GameResaultPanel gameResaultPanel;
    public void OnConnect()
    {
        gameResaulter.OnGameResault += gameResaultPanel.ShowResault;
    }

    public void OnDisconnect()
    {
        gameResaulter.OnGameResault -= gameResaultPanel.ShowResault;
    }
    
    void Start()
    {
        OnConnect();
    }

    void OnDestroy()
    {
        OnDisconnect();
    }
    
}

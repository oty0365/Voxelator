using UnityEngine;

public class GameStratQuiterConnecter : MonoBehaviour, IConnecter,IPannelUI
{
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject quitButton;
    [SerializeField] private GameObject pannel;
    [SerializeField] private GameObject playerData;

    private IPannel _ipannel;

    void Start()
    {
        OnConnect();
    }

    public void OnConnect()
    {
        startButton.GetComponent<IButton>().onClick += OnStart;
        quitButton.GetComponent<IButton>().onClick += OnQuit;
        pannel.GetComponent<MapSelectUI>().Initialize(playerData.GetComponent<IDataGetSetter<PlayerStageInfo>>());
        _ipannel = pannel.GetComponent<IPannel>();
    }

    public void OnActiveUI()
    {
        pannel.SetActive(true);
        _ipannel.OnActive();
    }

    public void OnInactiveUI()
    {
        pannel.SetActive(false);
        _ipannel.OnInActive();
    }

    public void OnStart()
    {
        OnActiveUI();
    }

    public void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 빌드된 상태라면 앱 종료
        Application.Quit();
#endif
    }
}
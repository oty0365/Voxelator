using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MapSelectUI : MonoBehaviour, IPannel
{
    [Header("Map Data")]
    [SerializeField] private MapListSO mapList;

    [Header("UI Components")]
    [SerializeField] private Image mapImage;
    [SerializeField] private TMP_Text mapNameText;

    private int _currentIndex = 0;
    private int _limitedIndex = 0;
    private IDataGetSetter<PlayerStageInfo> _istage;
    private PlayerStageInfo _stage = new();

    public void Initialize(IDataGetSetter<PlayerStageInfo> istage)
    {
        _istage = istage;
    }
    public void OnActive()
    {
        _istage.Get(_stage);
        _limitedIndex = _stage.limitStage;
        UpdateMapUI();
    }

    public void OnInActive()
    {
    }

    private void UpdateMapUI()
    {
        if (mapList == null || _limitedIndex == 0) return;

        var data = mapList.maps[_currentIndex];
        mapNameText.text = data.mapName;
        mapImage.sprite = data.mapImage;
        MapManager.Instance.ChangeMap(data.mapCode);
    }

    public void OnNext()
    {
        if (mapList == null || _limitedIndex == 0) return;

        _currentIndex = (_currentIndex + 1) % _limitedIndex;
        UpdateMapUI();
    }

    public void OnPrev()
    {
        if (mapList == null || _limitedIndex == 0) return;

        _currentIndex = (_currentIndex - 1 + mapList.maps.Length) % _limitedIndex;
        UpdateMapUI();
    }

    public void OnSelect()
    {
        SceneManager.LoadScene("InGameScene");
    }

    public void OnQuit()
    {
        gameObject.SetActive(false);
    }
}
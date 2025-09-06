using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapSetter : MonoBehaviour
{
    [SerializeField] private MapDataSO mapData;

    public void SetMapBanner(GameObject banner)
    {
        banner.GetComponent<Image>().sprite = mapData.mapBanner;
        banner.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Scripter.Instance.Translation(mapData.mapName); 
    }
}

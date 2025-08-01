using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class AugmentSet
{
    public TextMeshProUGUI augName;
    public Image augImage;
    public TextMeshProUGUI augDesc;
}
public class AugmentUI : MonoBehaviour
{
    [SerializeField] private GameObject augmentPanel;
    [SerializeField] AugmentSet[] augmentSet;
    private AugmentData[] augmentDatas;
    [SerializeField] private RectTransform center;

    private void Start()
    {
        augmentPanel.SetActive(false);
        AugmentManager.Instance.setUi+=UpdateUI;
    }


    private void UpdateUI(AugmentData[] datas)
    {
        augmentDatas = datas;
        for (var i = 0; i < augmentSet.Length; i++)
        {
            augmentSet[i].augName.text = Scripter.Instance.Translation(datas[i].augmentName);
            augmentSet[i].augImage.sprite = datas[i].augmentSprite;
            augmentSet[i].augDesc.text = Scripter.Instance.TranslationWithVariable(datas[i].augmentDescription, Extracter.Instance.CollectAugmentsAsString(datas[i].augmentBehavior));
        }
        augmentPanel.SetActive(true);
    }

    public void AugmentSelection(int index)
    {
        ObjectPooler.Instance.Get(augmentDatas[index].augmentBehavior.gameObject,center.transform.position,Vector2.zero);
        Time.timeScale = 1;
        augmentPanel.SetActive(false);

    }
}

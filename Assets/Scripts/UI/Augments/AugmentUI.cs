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
            augmentSet[i].augName.text = datas[i].augmentName;
            augmentSet[i].augImage.sprite = datas[i].augmentSprite;
            augmentSet[i].augDesc.text = datas[i].augmentDescription;
        }
        augmentPanel.SetActive(true);
    }

    public void AugmentSelection(int index)
    {
        StartCoroutine(CardMoveFlow(index));
    }
    private IEnumerator CardMoveFlow(int index)
    {
        var card = augmentSet[index].augDesc.transform.parent.gameObject;
        var rectTransform = card.GetComponent<RectTransform>();
        var origin = rectTransform.transform.position;
        card.GetComponent<CardUI>().isSelected = true;
        while (Vector2.Distance(Vector2.zero, rectTransform.transform.position) > 0.1f)
        {
            rectTransform.transform.position = Vector3.Lerp(rectTransform.transform.position, center.transform.position, 5 * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(1.1f);
        card.SetActive(false);
        card.GetComponent<CardUI>().isSelected = false;
        rectTransform.transform.position = origin;
        ObjectPooler.Instance.Get(augmentDatas[index].augmentPrefab, center.transform.position, new Vector3(0, 0, 0));
        Time.timeScale = 1;
        augmentPanel.SetActive(false);
    }
}

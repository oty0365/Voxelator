using System;
using System.Collections;
using NUnit.Framework.Constraints;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

[Serializable]
public class AugmentSet
{
    public Image augmentFrame;
    public TextMeshProUGUI augName;
    public Image augImage;
    public TextMeshProUGUI augDesc;
}
public class AugmentUI : MonoBehaviour
{

    [SerializeField] private GameObject augmentPanel;
    [SerializeField] AugmentSet[] augmentSet;
    private AugmentDataSO[] augmentDatas;
    [SerializeField] private RectTransform center;
    [SerializeField] private float cardAnimationDuration = 0.3f; 
    [SerializeField] private float cardAnimationDelay = 0.1f; 

    private void Start()
    {
        augmentPanel.SetActive(false);
    }


    public void UpdateUI(AugmentDataSO[] datas)
    {
        augmentDatas = datas;
        for (var i = 0; i < augmentSet.Length; i++)
        {
            augmentSet[i].augmentFrame.transform.localScale = Vector2.zero; 
            augmentSet[i].augmentFrame.color = SetColor(datas[i].augmentState);
            augmentSet[i].augmentFrame.GetComponent<CardUI>().isSelected = false;
            augmentSet[i].augmentFrame.gameObject.SetActive(true);
            augmentSet[i].augName.text = Scripter.Instance.Translation(datas[i].augmentName);
            augmentSet[i].augImage.sprite = datas[i].augmentSprite;
            var text = "";
            if (datas[i].augmentBehavior.TryGetComponent<AugmentDescriptionObj>(out var comp))
            {
                text = Scripter.Instance.TranslationWithVariable(datas[i].augmentDescription,comp.augmentedDatasSO.datas);
            }
            else
            {
                text = Scripter.Instance.Translation(datas[i].augmentDescription);
            }
            augmentSet[i].augDesc.text = text;
        }
        
        augmentPanel.SetActive(true);
        StartCoroutine(AnimateCardsSequentially());
    }

    private IEnumerator AnimateCardsSequentially()
    {
        for (int i = 0; i < augmentSet.Length; i++)
        {
            StartCoroutine(AnimateCardScale(i));
            yield return new WaitForSecondsRealtime(cardAnimationDelay);
        }
    }

    private IEnumerator AnimateCardScale(int cardIndex)
    {
        Transform cardTransform = augmentSet[cardIndex].augmentFrame.transform;
        float elapsedTime = 0f;
        Vector2 startScale = Vector2.zero;
        Vector2 targetScale = Vector2.one;

        while (elapsedTime < cardAnimationDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / cardAnimationDuration;
            t = 1f - Mathf.Pow(1f - t, 3f);
            
            cardTransform.localScale = Vector2.Lerp(startScale, targetScale, t);
            yield return null;
        }
        cardTransform.localScale = targetScale;
    }

    private Color SetColor(AugmentState augmentState)
    {
        var color = Color.white;
        switch (augmentState)
        {
            case AugmentState.Stat:
                color = Color.white;
                break;
            case AugmentState.Util:
                color = Color.green;
                break;
            case AugmentState.Weapon:
                color = Color.blue;
                break;
            case AugmentState.Active:
                color = Color.red;
                break;
        }
        return color;

    }

    public void AugmentSelection(int index)
    {
        StartCoroutine(MoveToCenterFlow(index));
    }
    private IEnumerator MoveToCenterFlow(int index)
    {
        var originPos = augmentSet[index].augmentFrame.transform.position;
        var currentTransform = augmentSet[index].augName.transform.parent.transform;
        var targetPos = center.transform.position;
        var currentSize = 1f;
        for(var i = 0; i < augmentSet.Length; i++)
        {
            if (i != index)
            {
                augmentSet[i].augName.transform.parent.gameObject.SetActive(false);
            }
        }

        while (Vector2.Distance(targetPos, currentTransform.position) > 0.01f)
        {
            currentTransform.position = Vector2.Lerp(currentTransform.position, targetPos, 5 * Time.unscaledDeltaTime);
            yield return null;
        }
        currentTransform.position = center.transform.position;
        while (1-currentSize<0.99f)
        {
            augmentSet[index].augName.transform.parent.localScale = new Vector2(currentSize,currentSize);
            currentSize=Mathf.Lerp(currentSize, 0, 11f * Time.unscaledDeltaTime);
            yield return null;
        }
        ObjectPoolManager.Instance.Get(augmentDatas[index].augmentBehavior.gameObject, center.transform.position, Vector2.zero);
        Time.timeScale = 1;
        currentTransform.position = originPos;
        augmentPanel.SetActive(false);
    } 
}
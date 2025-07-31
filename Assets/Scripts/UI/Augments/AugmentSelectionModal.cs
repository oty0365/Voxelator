/*using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class AugmentCard
{
    //private GameObject modalObj;
    public TextMeshProUGUI augName;
    public Image augImage;
    public TextMeshProUGUI augDesc;
}
public class AugmentSelectionModal : MonoBehaviour,ShowHider
{
    public Action fadeOut;
    [SerializeField] private AugmentCard[] augmentCards;
    [SerializeField] private AugmentDatas augmentDatas;
    [SerializeField] private TextMeshProUGUI descriptionPannel;
    [SerializeField] private RectTransform center;

    public void RandomizeCard()
    {
        List<AugmentData> availableCards = new List<AugmentData>(augmentDatas.augments);

        for (int i = availableCards.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            (availableCards[i], availableCards[randomIndex]) = (availableCards[randomIndex], availableCards[i]);
        }
        for (int i = 0; i < augmentCards.Length && i < availableCards.Count; i++)
        {
            UpdateCard(i, availableCards[i]);
        }
    }
    public void RandomizeCardAlternative()
    {
        List<AugmentData> availableCards = new List<AugmentData>(augmentDatas.augments);

        for (int i = 0; i < augmentCards.Length && availableCards.Count > 0; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableCards.Count);
            AugmentData selectedCard = availableCards[randomIndex];

            UpdateCard(i, selectedCard);
            availableCards.RemoveAt(randomIndex);
        }
    }

    private void UpdateCard(int slotIndex, AugmentData card)
    {
        augmentCards[slotIndex].augName.text = Scripter.Instance.Translation(card.augmentName);
        augmentCards[slotIndex].augDesc.text = Scripter.Instance.Translation(card.augmentDesc);
        augmentCards[slotIndex].augImage.sprite = card.augmentSprite;
        augmentCards[slotIndex].igEvent = card.effect;
    }
    private void InitDescPannel()
    {
        descriptionPannel.gameObject.SetActive(true);
        descriptionPannel.text = Scripter.Instance.Translation("RuneDescriptionPannel");
        foreach(var i in augmentCards)
        {
            i.augDesc.transform.parent.gameObject.SetActive(true);
        }
    }
    public void Show()
    {
        InitDescPannel();
        gameObject.SetActive(true);
    }
    public void Hide()
    {
       gameObject.SetActive(false);
    }
    public void OnClikedCard(int index)
    {
        foreach( var i in augmentCards[index].igEvent)
        {
            //GameEventManager.Instance.eventsDict[i].Invoke();
        }
        for( var i= 0;i < augmentCards.Length;i++)
        {
            if (i != index)
            {
                augmentCards[i].augDesc.transform.parent.gameObject.SetActive(false);
            }
        }
        StartCoroutine(CardMoveFlow(index));
    }
    private IEnumerator CardMoveFlow(int index)
    {
        var card = augmentCards[index].augDesc.transform.parent.gameObject;
        var rectTransform = card.GetComponent<RectTransform>();
        var origin = rectTransform.transform.position;
        card.GetComponent<Button>().interactable = false;
        while (Vector2.Distance(center.transform.position, rectTransform.transform.position) > 0.1f)
        {
            rectTransform.transform.position = Vector3.Lerp(rectTransform.transform.position, center.transform.position, 5 * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(1.1f);
        card.SetActive(false);
        rectTransform.transform.position = origin;
        card.GetComponent<Button>().interactable = true;
        descriptionPannel.gameObject.SetActive(false);
        fadeOut.Invoke();
    }
}
*/
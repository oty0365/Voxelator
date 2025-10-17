using UnityEngine;

public class AugmentUICardUIConnecter : MonoBehaviour,IConnecter
{
    [SerializeField] private AugmentUI augmentUi;
    [SerializeField] private CardUI[] cardUI;

    void Start()
    {
        OnConnect();
    }

    public void OnConnect()
    {
        foreach (CardUI cardUI in cardUI)
        {
            cardUI.onSelected += augmentUi.AugmentSelection;
        }
    }
}

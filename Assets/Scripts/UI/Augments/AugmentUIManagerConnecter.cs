using System;
using UnityEngine;

public class AugmentUIManagerConnecter : MonoBehaviour,IConnecter
{
    [SerializeField] private AugmentManager augmentManager;
    [SerializeField] private AugmentUI augmentUi;
    private void Start()
    {
        OnConnect();
    }

    public void OnConnect()
    {
        augmentManager.setUi+=augmentUi.UpdateUI;
    }
}

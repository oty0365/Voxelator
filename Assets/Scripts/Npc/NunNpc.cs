using UnityEngine;

public class NunNpc : MonoBehaviour,IInteractable
{
    [SerializeField] DialogsSO elfDialogsSO;
        
    public void OnInteract()
    {
        DialogManager.Instance.StartConversation(elfDialogsSO);
    }

    public void ExitInteract()
    {
        
    }
}

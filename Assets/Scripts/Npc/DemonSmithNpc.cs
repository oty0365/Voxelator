using UnityEngine;

public class DemonSmithNpc : MonoBehaviour,IInteractable
{
    [SerializeField] DialogsSO dialogsSO;
        
    public void OnInteract()
    {
        DialogManager.Instance.StartConversation(dialogsSO);
    }

    public void ExitInteract()
    {
        
    }
}

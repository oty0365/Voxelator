using UnityEngine;

public class NunNpc : MonoBehaviour,IInteractable
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

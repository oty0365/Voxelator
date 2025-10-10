using UnityEngine;

public class RobotFighterNpc : MonoBehaviour,IInteractable
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

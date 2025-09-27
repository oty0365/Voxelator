using UnityEngine;

public class RobotFighterNpc : MonoBehaviour,IInteractable
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

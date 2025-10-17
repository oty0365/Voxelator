using UnityEngine;

[CreateAssetMenu(fileName = "DialogScriptSO", menuName = "Scriptable Objects/DialogScriptSO")]
public class DialogScriptSO : ScriptableObject
{
    public string talker;
    public bool hasSelection;
    public Sprite talkersFace;
    public string[] dialogue;
    public FieldEventKey[] eventsWhileTalk;
}

using UnityEngine;

public interface ISaveable
{
    public string GetSavePath();
    public void OnSave();
    public void OnRemove();
    public void OnLoad();
}

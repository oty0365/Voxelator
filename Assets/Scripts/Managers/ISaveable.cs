using UnityEngine;

public interface ISaveAble
{
    public string GetSavePath();
    public void OnSave();
    public void OnRemove();
    public void OnLoad();
}

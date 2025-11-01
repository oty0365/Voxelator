using UnityEngine;

public class RuntimeUpgradeStatManager : SingletonMonoBehaviour<RuntimeUpgradeStatManager>
{
    private int _atk;
    private int _def;
    private int _hp;

    public int GetAtk()
    {
        return _atk;
    }

    public int GetDef()
    {
        return _def;
    }

    public int GetHp()
    {
        return _hp;
    }

    public void SetAtk(int atk)
    {
        _atk = atk;
    }

    public void SetDef(int def)
    {
        _def = def;
    }

    public void SetHp(int hp)
    {
        _hp = hp;
    }
}

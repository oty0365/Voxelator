using TMPro;
using UnityEngine;

public class LevelUpPannel : MonoBehaviour,IPannel
{
    private IDataGetSetter<PlayerStatUpgradeInfo> _iupgrades;
    private ISaveable _isaveable;
    private PlayerStatUpgradeInfo _upgrades = new();
    [SerializeField] private TextMeshProUGUI atkText;
    [SerializeField] private TextMeshProUGUI defText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI required;
    [SerializeField] private TextMeshProUGUI currentPoint;
    [SerializeField] private TextMeshProUGUI currentLevel;

    public void Initialize(IDataGetSetter<PlayerStatUpgradeInfo> idata,ISaveable saveable)
    {
        _iupgrades = idata;
        _isaveable = saveable;
    }
    public void OnActive()
    {
        _iupgrades.Get(_upgrades);
        UpdateUI();
    }
    public void OnInActive()
    {

    }
    public void Plus(int type)
    {
        if (_upgrades.upgradePoint >= (_upgrades.level + 1) * 2)
        {
            if (type == 1)
            {
                _upgrades.atk++;
            }
            else if(type == 2)
            {
                _upgrades.def++;
            }
            else if (type == 3)
            {
                _upgrades.hp++;
            }
            _upgrades.upgradePoint -= (_upgrades.level + 1) * 2;
            _upgrades.level++;
            UpdateUI();
        }
    }
    public void Mius(int type)
    {
        var didmin = false;
        if (type == 1)
        {
            if (_upgrades.atk > 0)
            {
                _upgrades.atk--;
                didmin = true;
            }
        }
        else if (type == 2)
        {
            if (_upgrades.def > 0)
            {
                _upgrades.def--;
                didmin = true;
            }
        }
        else if (type == 3)
        {
            if (_upgrades.hp > 0)
            {
                _upgrades.hp--;
                didmin = true;
            }
        }
        if (didmin)
        {
            if (_upgrades.level > 0)
            {
                _upgrades.upgradePoint += _upgrades.level * 2;
                _upgrades.level--;
            }
            UpdateUI();
        }
    }
    public void Save()
    {
        _iupgrades.Set(_upgrades);
        _isaveable.OnSave();
    }

    private void UpdateUI()
    {
        atkText.text = _upgrades.atk.ToString();
        defText.text = _upgrades.def.ToString();
        hpText.text = _upgrades.hp.ToString();
        currentPoint.text = _upgrades.upgradePoint.ToString();
        required.text = ((_upgrades.level+1) * 2).ToString();
        currentLevel.text = _upgrades.level.ToString();
    }
}

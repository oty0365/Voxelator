using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : HalfSingleMono<PlayerStatusUI>
{
    [SerializeField] private Slider playerExp;
    [SerializeField] private TextMeshProUGUI playerLevelText;
    [SerializeField] private TextMeshProUGUI playerHpText;
    [SerializeField] private TextMeshProUGUI playerAtkText;
    [SerializeField] private TextMeshProUGUI playerDefText;

    public void SetMaxExp(float exp)
    {
        playerExp.maxValue = exp;
    }
    public void SetExp(float exp)
    {
        playerExp.value = exp;
    }

    public void SetLevel(int level)
    {
        playerLevelText.text = level.ToString();
    }

    public void SetAtk(float atk)
    {
        playerAtkText.text = atk.ToString();
    }

    public void SetDef(float def)
    {
        playerDefText.text = def.ToString();
    }

    public void SetHp(float hp,float maxHp)
    {
        playerHpText.text="<color=green>"+hp+"</color>/"+maxHp;
    }

}

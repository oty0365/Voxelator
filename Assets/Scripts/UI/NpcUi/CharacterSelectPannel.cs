using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectPannel : MonoBehaviour,IPannel
{
    private IDataGetSetter<PlayerCharacterInfo> _icharacters;
    private ISaveable _isaveable;
    private PlayerCharacterInfo _character = new();
    [SerializeField] Image characterSelectImage;
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] private int index = 0; 

    
    public void Initialize(IDataGetSetter<PlayerCharacterInfo> idata,ISaveable saveable)
    {
        _icharacters = idata;
        _isaveable = saveable;
    }
    
    public void OnActive()
    {
        UpdateDraw();
    }

    public void UpdateDraw()
    {
        _icharacters.Get(_character);
        var so = CharacterManager.Instance.GetData(_character.characterNames[index]);
        characterSelectImage.sprite = so.characterSprite;
        characterName.text = so.characterName;
        description.text = so.description;
    }

    public void OnInActive()
    {
        
    }

    public void MoveNext(int dir)
    {
        _icharacters.Get(_character);
        var origin = index;
        if (dir > 0)
        {
            index++;
        }
        else
        {
            index--;
        }

        if (index < 0 && index >= _character.characterNames.Count)
        {
            index = origin;
        }
        
        UpdateDraw();
    }
}

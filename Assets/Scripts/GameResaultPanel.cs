using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameResaultPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resaultText;
    [SerializeField] private TextMeshProUGUI subResaultText;
    [SerializeField] private Image backgroundImage;

    private void Start()
    {
        backgroundImage.gameObject.SetActive(false);
    }
    
    public void ShowResault(bool checkWin)
    {
        backgroundImage.gameObject.SetActive(true);
        resaultText.text = checkWin ? "승리" : "패배";
        subResaultText.text = checkWin ? "스킬포인트를 얻었다!" : "...";
        
    }
}

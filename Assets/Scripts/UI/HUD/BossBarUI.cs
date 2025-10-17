using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossBarUI : MonoBehaviour,IEvent
{
    [Header("UI")]
    [SerializeField] private GameObject bossBarPanel;
    [SerializeField] private TextMeshProUGUI bossNameText;
    [SerializeField] private Slider bossHpSlider;
    [Header("설정")] 
    [SerializeField] private float speed;
    
    private void Start()
    {
        bossBarPanel.SetActive(false);
    }

    public void StartBossBattle(string bossName,AEnemy bossModel)
    {
        bossBarPanel.SetActive(true);
        bossNameText.text = Scripter.Instance.Translation(bossName);
        bossHpSlider.maxValue = 50;
        StartCoroutine(ShowBossHpBarFlow(bossModel));
    }
    private IEnumerator ShowBossHpBarFlow(AEnemy bossModel)
    {
        bossHpSlider.value = 0;
        for (float i = 0; i < bossHpSlider.maxValue; i += Time.deltaTime*speed)
        {
            bossHpSlider.value = i;
            yield return null;
        }
        Connection(bossModel);
    }

    private void Connection(AEnemy bossModel)
    {
        bossModel.enemyData.health.OnChanged+=OnHpUpdate;
        OnHpUpdate(bossModel.enemyData.health.Value,bossModel.enemyData.health.MaxValue);
    }

    private void OnHpUpdate(float currentVal, float maxVal)
    {
        bossHpSlider.maxValue = maxVal;
        bossHpSlider.value = currentVal;        


    }

    public void EndBossBattle()
    {
        bossBarPanel.SetActive(false);
    }

    public void Subscribe()
    {
        EventManager.Instance.AddListener(EventKey.OnBossBattleStart,new Action<string,AEnemy>(StartBossBattle));
        EventManager.Instance.AddListener(EventKey.OnBossBattleEnd,new Action(EndBossBattle));
    }

    public void Unsubscribe()
    {
        EventManager.Instance.RemoveListener(EventKey.OnBossBattleStart,new Action<string,AEnemy>(StartBossBattle));
        EventManager.Instance.RemoveListener(EventKey.OnBossBattleEnd,new Action(EndBossBattle));
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }
}


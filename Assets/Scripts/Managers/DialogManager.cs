using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : SceneSingletonMonoBehaviour<DialogManager>
{

    [Header("대화 설정")]
    public DialogsSO currentDialogs;
    public int currentIndex;
    public bool isEventing;

    [Header("대화 UI")] 
    public GameObject dialogPanel;
    public Image dialogContainer;
    public Image talkerPortraitImage;
    public TextMeshProUGUI talkerNameTmp;
    public TextMeshProUGUI talkerDialogTmp;

    [Header("수치")]
    [SerializeField] private float textSpeed = 0.05f;
    [SerializeField] private float fadeSpeed = 15f;
    [SerializeField] private float fadeTime = 0.8f;

    private bool _isPuttingText;
    private Coroutine _putTextFlow;
    private Scripter scripter;

    private void Start()
    {
        dialogPanel.gameObject.SetActive(false);
        scripter = Scripter.Instance;
    }

    public void StartConversation(DialogsSO dialogs)
    {
        currentDialogs = dialogs;
        currentIndex = 0;
        _isPuttingText = false;
        StartCoroutine(FadeInFlow());
    }

    private IEnumerator FadeInFlow()
    {
        var color = Color.clear;
        dialogContainer.color = color;
        talkerNameTmp.color = color;
        talkerDialogTmp.color = color;
        talkerPortraitImage.color = color;
        dialogPanel.gameObject.SetActive(true);
        for (var i = 0f; i < fadeTime; i += Time.deltaTime)
        {
            color=Color.Lerp(color, Color.white, fadeSpeed * Time.deltaTime);
            dialogContainer.color = color;
            talkerNameTmp.color = color;
            talkerDialogTmp.color = color;
            talkerPortraitImage.color = color;
            yield return null;
        }
        PutText();
    }

    private IEnumerator FadeOutFlow()
    {
        var color = Color.white;
        dialogContainer.color = color;
        talkerNameTmp.color = color;
        talkerDialogTmp.color = color;
        talkerPortraitImage.color = color;
        for (var i = 0f; i < fadeTime; i += Time.deltaTime)
        {
            color=Color.Lerp(color, Color.clear, fadeSpeed * Time.deltaTime);
            dialogContainer.color = color;
            talkerNameTmp.color = color;
            talkerDialogTmp.color = color;
            talkerPortraitImage.color = color;
            yield return null;
        }
        dialogPanel.gameObject.SetActive(false);
    }

    public void NextText()
    {
        if (!isEventing)
        {

            if (_isPuttingText)
            {
                StopCoroutine(_putTextFlow);
                talkerDialogTmp.text = scripter.scripts[currentDialogs.dialogScripts[currentIndex].dialogue[0]].currentText;
                _isPuttingText = false;
                return;
            }

            currentIndex++;

            if (currentIndex >= currentDialogs.dialogScripts.Length)
            {
                EndConversation();
                return;
            }

            PutText();
        }
    }

    private void EndConversation()
    {
        StartCoroutine(FadeOutFlow());
    }

    private void PutText()
    {
        var currentDia = currentDialogs.dialogScripts[currentIndex];
        if (currentDia.eventsWhileTalk.Length > 0)
        {
            isEventing = true;
            foreach(var i in currentDia.eventsWhileTalk)
            {
                EventManager.Instance.Invoke(i);
            }
        }
        else
        {
            _isPuttingText = true;
            talkerPortraitImage.sprite = currentDia.talkersFace;
            talkerNameTmp.text = scripter.scripts[currentDia.talker].currentText;
            _putTextFlow = StartCoroutine(PutTextFlow(scripter.scripts[currentDia.dialogue[0]].currentText));
        }
    }

    private IEnumerator PutTextFlow(string dialog)
    {
        talkerDialogTmp.text = "";

        dialog = dialog.Replace("\\n", "\n");

        int i = 0;
        while (i < dialog.Length)
        {
            if (dialog[i] == '<')
            {
                int tagEnd = dialog.IndexOf('>', i);
                if (tagEnd != -1)
                {
                    string tag = dialog.Substring(i, tagEnd - i + 1);
                    talkerDialogTmp.text += tag;
                    i = tagEnd + 1;
                    continue;
                }
            }

            if (dialog[i] == '\n')
            {
                talkerDialogTmp.text += '\n';
                i++;
                continue;
            }

            talkerDialogTmp.text += dialog[i];
            i++;

            yield return new WaitForSeconds(textSpeed);
        }

        _isPuttingText = false;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    public GameObject windowDialog;
    public TextMeshProUGUI textDialog;
    public string[] messages;
    public float textSpeed = 0.05f; // Скорость вывода текста

    private int currentDialogIndex = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    private void Start()
    {
        StartDialog();
    }

    public void OnSignalReceived()
    {
        NextDialog();
    }
    private void StartDialog()
    {
        windowDialog.SetActive(true);
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        isTyping = true;
        textDialog.text = "";

        foreach (char c in messages[currentDialogIndex])
        {
            textDialog.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
    }

    private float cooldownTime = 2f; // Время кулдауна в секундах
    private float cooldownTimer = 0f;

    public void NextDialog()
    {
        if (!isTyping)
        {
            currentDialogIndex++;
            if (currentDialogIndex < messages.Length)
            {
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                }
                typingCoroutine = StartCoroutine(TypeText());
            }
            else
            {
                windowDialog.SetActive(false); // Завершение диалога
            }
        }
        else if (cooldownTimer <= 0f)
        {
            // Сбросить таймер кулдауна
            cooldownTimer = cooldownTime;
        }
    }

    private void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }
}

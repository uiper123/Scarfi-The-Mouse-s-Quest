using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string fullText;
    public float typingSpeed = 0.05f; // Задержка между буквами
    public int maxCharacters; // Кол0во символов

    private void Start()
    {
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        int charactersTyped = 0;
        foreach (char letter in fullText.ToCharArray())
        {
            if (charactersTyped < maxCharacters)
            {
                textDisplay.text += letter;
                charactersTyped++;
                yield return new WaitForSeconds(typingSpeed);
            }
            else
            {
                break; // Прекращаем печатать, когда достигнуто maxCharacters
            }
        }
    }

    public void SetText(string text)
    {
        fullText = text;
        textDisplay.text = ""; // Очистить текущий текст
        StartCoroutine(TypeText());
    }

    public void ClearText()
    {
        StopAllCoroutines(); // Остановить печатание
        textDisplay.text = ""; // Очистить текст
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance; 
    public Dictionary<string, Item> collectedItems = new Dictionary<string, Item>();
    public TextMeshProUGUI itemCountText; // Ссылка на текстовый объект UI для отображения количества собранных предметов
    private int requiredItemCount = 3; // Количество предметов, необходимых для завершения игры
    public UIController uicontroller;

    private void Start()
    {
        UpdateItemCountText();
    }

    private void Update()
    {
        // Обновляем счет
        uicontroller.SetScore(ScoreManager.instance.score);
    }

    private void UpdateItemCountText()
    {
        itemCountText.text = $"Collect: {collectedItems.Count}/{requiredItemCount}";
    }

    public void AddItemToInventory(Item item)
    {
        // Проверяем, является ли предмет ключом
        if (item.name.StartsWith("Ключ")) 
        {
            // Добавляем ключ в словарь, используя его имя как ключ
            collectedItems[item.name] = item;
        }
    
        // Проверяем, собраны ли все три ключа
        if (collectedItems.Count == 3)
        {
            CompleteGame();
        }
    }

    private void CompleteGame()
    {
        // Здесь можно добавить дополнительную логику, например, показать экран завершения игры
        SceneManager.LoadScene("EndGameScene"); // Загрузите сцену завершения игры
    }
}

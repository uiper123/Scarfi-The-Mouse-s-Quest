using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    public List<Item> items = new List<Item>();
    public GameObject potionItemCounterPrefab; // Prefab счетчика предметов
    public Transform counterParent; // Родительский объект для счетчика
    private Text counterText; // Текст счетчика
    private int requiredItems = 3; // Количество предметов, необходимых для зелья
    void Start()
    {
        // Создаем экземпляр счетчика предметов и получаем его текст
        GameObject counter = Instantiate(potionItemCounterPrefab, counterParent);
        counterText = counter.GetComponent<Text>();
        UpdateCounterText();
    }

    public void AddItem(Item item)
    {
        items.Add(item);
        UpdateCounterText();

        if (items.Count >= requiredItems)
        {
            // Вызываем метод завершения игры у PlayerController
           
        }
    }

    
    private void UpdateCounterText()
    {
        counterText.text = $"Collect {items.Count}/{requiredItems}"; // Обновляем текст счетчика
    }
    
}

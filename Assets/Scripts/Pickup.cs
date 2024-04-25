using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
    public Item item; // Предмет, который этот объект представляет
    
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Inventory inventory = other.GetComponent<Inventory>();
            if (inventory != null) {
                inventory.AddItem(item);
                 // Уничтожаем объект после подбора
            }
            InventoryManager.instance.AddItemToInventory(item);
            Destroy(gameObject);
        }
    }
}
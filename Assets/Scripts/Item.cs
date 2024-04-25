using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string name;
    public ItemType type;
    public float cooldown;
    public Sprite itemIcon;// Время перезарядки для мышиного клыка
    

    
}
public enum ItemType
{
    MouseRoar,
    Key,
    // Другие типы предметов
}

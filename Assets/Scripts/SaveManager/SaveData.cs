using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.SaveManager
{
    [System.Serializable]
    public class SaveData
    {
        public string levelstr; // Новое поле для имени уровня
        public Vector3 playerPosition;
        public int level; // Индекс сцены // Или int level, если вы используете индексы уровней
        public float maxHealth;
        public int currentHealth;
        public float currentStamina;
        public float armor;
        public int currentCheckpointIndex;
        public float mouseRoarCooldown;
        
        public List<string> collectedItems; // Список идентификаторов собранных предметов
        public bool hasMouseRoar; // Флаг наличия способности мышиного рыка
        public int score; // Счет игрока
    
        public SaveData()
        {
            // Инициализируйте поля значениями по умолчанию, если необходимо
            playerPosition = Vector3.zero;
            currentStamina = 0f;
            armor = 0f;
            currentCheckpointIndex = 0;
            mouseRoarCooldown = 0f;
             // Список идентификаторов собранных предметов
            hasMouseRoar = false; // Флаг наличия способности мышиного рыка
            score = 0; // Счет игрока
        }
    }
    
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.SaveManager
{
    public static class SaveSystem
    {
        public static void SaveProgress(SaveData data)
        {
            
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString("SaveData", json);
            PlayerPrefs.Save();
        }

        public static SaveData LoadProgress()
        {
            if (PlayerPrefs.HasKey("SaveData"))
            {
                string json = PlayerPrefs.GetString("SaveData");
                return JsonUtility.FromJson<SaveData>(json);
            }
            
            return null; // Или возвращайте дефолтные значения
        }
        
        public static void ResetProgress()
        {
            PlayerPrefs.DeleteKey("SaveData");
            PlayerPrefs.Save();
        }
    }
}


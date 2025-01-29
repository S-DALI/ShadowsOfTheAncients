using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VP
{
    [System.Serializable]
    public class CharacterSaveData
    {
        [Header("Scene Index")]
        public int sceneIndex;
        [Header("Characte Name")]
        public string characterName="Character";

        [Header("Time played")]
        public float secondsPlayed;

        [Header("World Cordinates")]
        public float xPosition;
        public float yPosition;
        public float zPosition;

        [Header("Resources")]
        public int currentHealth;
        public float currentStamina;

        [Header("Stats")]
        public int vitality;
        public int endurance;
    }
}

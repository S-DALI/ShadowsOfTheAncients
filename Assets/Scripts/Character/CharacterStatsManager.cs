using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace VP
{
    public class CharacterStatsManager : MonoBehaviour
    {
        CharacterManager character;
        [Header("Stamina Regeneration")]
        private float staminaRegenerationTimer = 0;
        private float staminaTickTimer = 0;
        [SerializeField] private float staminaRegenerationDelay = 3;
        [SerializeField] private float staminaRegenerationAmount = 1;
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }
        protected virtual void Start()
        {

        }
        public int CalculateHealthBasedOnVitalityLevel(int vitality)
        {
            float health = 0;

            health = vitality * 20;
            return Mathf.RoundToInt(health);
        }
        public int CalculateStaminaBasedOnEnduranceLevel(int edurance)
        {
            float stamina = 0;

            stamina = edurance * 10;
            return Mathf.RoundToInt(stamina);
        }
        public virtual void RegenerateStamina()
        {
            if (!character.IsOwner)
                return;
            if (character.characterNetworkManager.isSprinting.Value)
                return;
            if (character.isPerformingAction)
                return;
            staminaRegenerationTimer += Time.deltaTime;
            if (staminaRegenerationTimer >= staminaRegenerationDelay)
            {
                if (character.characterNetworkManager.currentStamina.Value < character.characterNetworkManager.maxStamina.Value)
                {
                    staminaTickTimer += Time.deltaTime;
                    if (staminaTickTimer >= 0.1)
                    {
                        staminaTickTimer = 0;
                        character.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount;
                    }
                }
            }
        }
        public virtual void ResetStaminaRegenerationTimer(float previousStaminaAmount, float currentStaminaAmout)
        {
            if (currentStaminaAmout < previousStaminaAmount)
            {
                staminaRegenerationTimer = 0;
            }
        }
    }
}

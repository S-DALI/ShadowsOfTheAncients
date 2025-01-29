using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace VP
{
    public class UI_Character_Save_Slots : MonoBehaviour
    {
        SaveFileDataWriter saveFileDataWriter;

        [Header("Game Slot")]
        public CharacterSlot characterSlot;

        [Header("Character Info")]
        public TextMeshProUGUI characterName;
        public TextMeshProUGUI timePlayed;

        private void OnEnable()
        {
            LoadSaveSlots();
        }

        private void LoadSaveSlots()
        {
            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

            switch(characterSlot)
            {
                case CharacterSlot.CharacterSlot_01:
                    FillingSlotsData(WorldSaveGameManager.instance.characterSlot01);
                    break;
                case CharacterSlot.CharacterSlot_02:
                    FillingSlotsData(WorldSaveGameManager.instance.characterSlot02);
                    break;
                case CharacterSlot.CharacterSlot_03:
                    FillingSlotsData(WorldSaveGameManager.instance.characterSlot03);
                    break;
                case CharacterSlot.CharacterSlot_04:
                    FillingSlotsData(WorldSaveGameManager.instance.characterSlot04);
                    break;
                case CharacterSlot.CharacterSlot_05:
                    FillingSlotsData(WorldSaveGameManager.instance.characterSlot05);
                    break;
                case CharacterSlot.CharacterSlot_06:
                    FillingSlotsData(WorldSaveGameManager.instance.characterSlot06);
                    break;
                case CharacterSlot.CharacterSlot_07:
                    FillingSlotsData(WorldSaveGameManager.instance.characterSlot07);
                    break;
                case CharacterSlot.CharacterSlot_08:
                    FillingSlotsData(WorldSaveGameManager.instance.characterSlot08);
                    break;
                case CharacterSlot.CharacterSlot_09:
                    FillingSlotsData(WorldSaveGameManager.instance.characterSlot09);
                    break;
                case CharacterSlot.CharacterSlot_10:
                    FillingSlotsData(WorldSaveGameManager.instance.characterSlot10);
                    break;
            }
        }
        private void FillingSlotsData(CharacterSaveData characterSaveData)
        {
            saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
            if (saveFileDataWriter.CheckToSeeIfFileExist() && characterSaveData!=null)
            {
                characterName.text = characterSaveData.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public void LoadGameFromCharacterSlot()
        {
            WorldSaveGameManager.instance.currentCharacterSlotBeingUsed = characterSlot;
            WorldSaveGameManager.instance.LoadGame();
        }

        public void SelectCurrentSlot()
        {
            TitleScreenManager.instance.SelectCharacterSlot(characterSlot);
        }
    }
}


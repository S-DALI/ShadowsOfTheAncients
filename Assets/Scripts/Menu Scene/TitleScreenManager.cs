using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace VP
{
    public class TitleScreenManager : MonoBehaviour
    {
        public static TitleScreenManager instance;
        [Header("Menus")]
        [SerializeField] GameObject tittleScreenMainMenu;
        [SerializeField] GameObject tittleScreenLoadMenu;
        [Header("Buttons")]
        [SerializeField] Button loadMenuReturnButton;
        [SerializeField] Button mainMenuLoadButton;
        [SerializeField] Button mainMenuNewGameButton;
        [SerializeField] Button deleteCharacterPopUpConfirmButton;
        [SerializeField] Button noCharacterSlotsOkayButton;
        [Header("Pop Ups")]
        [SerializeField] GameObject noCharacterSlotsPopUp;
        [SerializeField] GameObject deleteCharacterSlotPopUp;
        [Header("Character Slot")]
        public CharacterSlot currentSelectedSlot = CharacterSlot.NO_SLOT;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }

        public void StartNewGame()
        {
            WorldSaveGameManager.instance.AttemptToCreateNewGame();
        }
        public void OpenLoadGameMenu()
        {
            tittleScreenMainMenu.SetActive(false);
            tittleScreenLoadMenu.SetActive(true);

            loadMenuReturnButton.Select();

        }
        public void CloseLoadGameMenu()
        {
            tittleScreenLoadMenu.SetActive(false);
            tittleScreenMainMenu.SetActive(true);

            mainMenuLoadButton.Select();
        }
        public void DisplayNoFreeCharacterSlotsPopUp()
        {
            noCharacterSlotsPopUp.SetActive(true);
            noCharacterSlotsOkayButton.Select();
        }

        public void CloseNoFreeCharacterSlotsPopUP()
        {
            noCharacterSlotsPopUp.SetActive(false);
            mainMenuLoadButton.Select();
        }
        public void SelectCharacterSlot(CharacterSlot characterSlot)
        {
            currentSelectedSlot = characterSlot;
        }
        public void SelectNoSlot()
        {
            currentSelectedSlot = CharacterSlot.NO_SLOT;
        }
        public void AttemptToDeleteCharacterSlot()
        {
            if (currentSelectedSlot != CharacterSlot.NO_SLOT)
            {
                deleteCharacterSlotPopUp.SetActive(true);
                deleteCharacterPopUpConfirmButton.Select();
            }
        }
        public void DeleteCharacterSlot()
        {
            deleteCharacterSlotPopUp.SetActive(false);
            WorldSaveGameManager.instance.DeleteGame(currentSelectedSlot);
            tittleScreenLoadMenu.SetActive(false);
            tittleScreenLoadMenu.SetActive(true);
            loadMenuReturnButton.Select();
        }
        public void CloseDeleteCharacterPopUp()
        {
            deleteCharacterSlotPopUp.SetActive(false);
            loadMenuReturnButton.Select();
        }
    }
}

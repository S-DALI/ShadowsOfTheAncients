using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VP;
namespace VP
{
    public class TitleScreenLoadMenuInputManager : MonoBehaviour
    {
        PlayerControllers playerControllers;
        [Header("Title Screen Inputs")]
        [SerializeField] bool deleteCharacterSlot = false;
        private void Update()
        {
            if (deleteCharacterSlot)
            {
                deleteCharacterSlot = false;
                TitleScreenManager.instance.AttemptToDeleteCharacterSlot();
            }
        }

        private void OnEnable()
        {
            if (playerControllers == null)
            {
                playerControllers = new PlayerControllers();
                playerControllers.UI.Delete.performed += i => deleteCharacterSlot = true;
            }
            playerControllers.Enable();
        }
        private void OnDisable()
        {
            playerControllers.Disable();
        }
    }
}

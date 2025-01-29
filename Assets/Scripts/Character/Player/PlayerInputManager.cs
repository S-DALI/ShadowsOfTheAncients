using UnityEngine;
using UnityEngine.SceneManagement;

namespace VP
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;

        public PlayerManager player;
        PlayerControllers playerControllers;
        [Header("Player Movement Input")]
        [SerializeField] private Vector2 movementInput;
        public float horizontalInput;
        public float verticalInput;
        public float moveAmount;
        [Header("Player Action Input")]
        [SerializeField] bool dodgeInput = false;
        [SerializeField] bool sprintInput = false;
        [SerializeField] bool jumpInput = false;
        [SerializeField] bool lbInput = false;
        [Header("Camera Movement Input")]
        [SerializeField] private Vector2 cameraInput;
        public float cameraHorizontalInput;
        public float cameraVerticalInput;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(instance);
            }
        }
        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            SceneManager.activeSceneChanged += OnSceneChanges;

            instance.enabled = false;

            if(playerControllers != null)
            {
                playerControllers.Disable();
            }
        }

        private void OnSceneChanges(Scene oldScene, Scene newScene)
        {
            if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldIndexScene())
            {
                instance.enabled = true;
                if (playerControllers != null)
                {
                    playerControllers.Enable();
                }
            }
            else
            {
                instance.enabled = false;

                if (playerControllers != null)
                {
                    playerControllers.Disable();
                }
            }
        }

        private void OnEnable()
        {
            if (playerControllers == null)
            {
                playerControllers = new PlayerControllers();
                playerControllers.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                playerControllers.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
                playerControllers.PlayerActions.Dodge.performed += i => dodgeInput = true;
                playerControllers.PlayerActions.Jump.performed += i => jumpInput = true;
                playerControllers.PlayerActions.LB.performed += i => lbInput = true;


                playerControllers.PlayerActions.Sprint.performed += i => sprintInput = true;
                playerControllers.PlayerActions.Sprint.canceled += i => sprintInput = false;
            }
            playerControllers.Enable();
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChanges;
        }
        private void OnApplicationFocus(bool focus)
        {
            if (enabled)
            {
                if (focus)
                {
                    playerControllers.Enable();
                }
                else
                {
                    playerControllers.Disable();
                }
            }
        }
        private void Update()
        {
            HandleAllInputs();
        }
        private void HandleAllInputs()
        {
            HandleCameraMovementInput();
            HadlePlayerMovementInput();
            HandleDodgeInput();
            HandleSprintInput();
            HandleJumpInput();
            HandleLBInput();
        }
        private void HadlePlayerMovementInput()
        {
            horizontalInput = movementInput.x;
            verticalInput = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
            if (moveAmount <= 0.5 && moveAmount > 0)
            {
                moveAmount = 0.5f;
            }
            if (moveAmount > 0.5f && moveAmount <= 1)
            {
                moveAmount = 1;
            }

            if (player == null)
                return;



            player.playerAnimatorManager.UpdateAnimatorMovementParametrs(0,moveAmount, player.playerNetworkManager.isSprinting.Value);
        }
        private void HandleCameraMovementInput()
        {
            cameraHorizontalInput = cameraInput.x;
            cameraVerticalInput = cameraInput.y;
        }
        private void HandleDodgeInput()
        {
            if(dodgeInput)
            {
                dodgeInput = false;
                player.playerLocomotionManager.AttemptToPerformDodge();
            }
        }
        private void HandleSprintInput()
        {
            if(sprintInput)
            {
                player.playerLocomotionManager.HandleSprinting();
            }
            else
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }
        }
        private void HandleJumpInput()
        {
            if(jumpInput)
            {
                jumpInput = false;
                player.playerLocomotionManager.AttemptToPerformJump();
            }
        }
        private void HandleLBInput()
        {
            if(lbInput)
            {
                lbInput = false;
                player.playerNetworkManager.SetCharacterActionHand(true);
                player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oneHandLbAction, player.playerInventoryManager.currentRightHandWeapon);
            }
        }
    }
}

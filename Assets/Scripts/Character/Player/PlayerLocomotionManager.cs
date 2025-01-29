using UnityEngine;

namespace VP
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        private PlayerManager player;

        [HideInInspector] public float horizontalMovement;
        [HideInInspector] public float verticalMovement;
        [HideInInspector] public float moveAmount;

        [Header("MovementSettings")]
        private Vector3 moveDirection;
        private Vector3 targetRotationDirection;
        [SerializeField] private float walkingSpeed = 4;
        [SerializeField] private float runningSpeed = 8;
        [SerializeField] private float sprintingSpeed = 16f;
        [SerializeField] private float rotationSpeed = 15;

        [SerializeField] private int sprintingStaminaCost = 1;
        [Header("Dodge")]
        private Vector3 dodgeDirection;
        [SerializeField] private int dodgeStaminaCost = 25;
        [Header("Jump")]
        private Vector3 jumpDirection;
        [SerializeField] private float jumpStaminaCostFromEdurance = 2.5f;        
        [SerializeField] private float jumpHeight = 8f;
        [SerializeField] private float jumpForwardSpeed = 10;
        [SerializeField] private float freeFallSpeed = 4;
        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }
        protected override void Update()
        {
            base.Update();

            if (player.IsOwner)
            {
                player.characterNetworkManager.animatorVerticalValue.Value = verticalMovement;
                player.characterNetworkManager.animatorHorizontalValue.Value = horizontalMovement;
                player.characterNetworkManager.networkMoveAmount.Value = moveAmount;
            }
            else
            {
                verticalMovement = player.characterNetworkManager.animatorVerticalValue.Value;
                horizontalMovement = player.characterNetworkManager.animatorHorizontalValue.Value;
                moveAmount = player.characterNetworkManager.networkMoveAmount.Value;

                player.playerAnimatorManager.UpdateAnimatorMovementParametrs(0, moveAmount,player.playerNetworkManager.isSprinting.Value);
            }
        }
        public void HandleAllMovement()
        {
            HandleGroundedMovement();
            HandleRotation();
            HandleJumpingMovement();
            HandleFreeFallMovement();
        }

        private void GetMovementValues()
        {
            verticalMovement = PlayerInputManager.instance.verticalInput;
            horizontalMovement = PlayerInputManager.instance.horizontalInput;
            moveAmount = PlayerInputManager.instance.moveAmount;
        }

        private void HandleGroundedMovement()
        {
            if (!player.canMove)
                return;
            GetMovementValues();

            moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
            moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement;
            moveDirection.Normalize();
            moveDirection.y = 0f;
            if (player.playerNetworkManager.isSprinting.Value)
            {
                player.characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);
            }
            else
            {
                if (PlayerInputManager.instance.moveAmount > 0.5f)
                {
                    player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
                }
                else if (PlayerInputManager.instance.moveAmount <= 0.5f)
                {
                    player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
                }
            }
        }
        private void HandleJumpingMovement()
        {
            if (player.playerNetworkManager.isJumping.Value)
            {
                player.characterController.Move(jumpDirection * jumpForwardSpeed * Time.deltaTime);
            }
        }
        private void HandleFreeFallMovement()
        {
            if(!player.isGrounded)
            {
                Vector3 freeFallDirection;
                freeFallDirection = PlayerCamera.instance.transform.forward * PlayerInputManager.instance.verticalInput;
                freeFallDirection +=PlayerCamera.instance.transform.right * PlayerInputManager.instance.horizontalInput;
                freeFallDirection.y = 0;
                player.characterController.Move(freeFallDirection * freeFallSpeed * Time.deltaTime);
            }
        }
        public void HandleSprinting()
        {
            if (player.isPerformingAction)
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }
            if(player.playerNetworkManager.currentStamina.Value<=0) 
            {
                player.playerNetworkManager.isSprinting.Value = false;
                return;
            }

            if (moveAmount >= 0.5f)
            {
                player.playerNetworkManager.isSprinting.Value = true;
            }
            else
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }
            if(player.playerNetworkManager.isSprinting.Value)
            {
                player.playerNetworkManager.currentStamina.Value-= sprintingStaminaCost * Time.deltaTime;
            }
        }
        private void HandleRotation()
        {
            if (!player.canRotate)
                return;

            targetRotationDirection = Vector3.zero;
            targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
            targetRotationDirection = targetRotationDirection + PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
            targetRotationDirection.Normalize();
            targetRotationDirection.y = 0f;
            if (targetRotationDirection == Vector3.zero)
            {
                targetRotationDirection = transform.forward;
            }

            Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }
        public void AttemptToPerformDodge()
        {
            if (player.isPerformingAction)
                return;
            if (player.playerNetworkManager.currentStamina.Value <= 0)
                return;
            if (PlayerInputManager.instance.moveAmount > 0)
            {
                dodgeDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
                dodgeDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
                dodgeDirection.y = 0f;
                dodgeDirection.Normalize();

                Quaternion playerRotation = Quaternion.LookRotation(dodgeDirection);
                player.transform.rotation = playerRotation;
                player.playerAnimatorManager.PlayTargetActionAnimation("DodgeForward01", true, true);
            }
            else
            {
                player.playerAnimatorManager.PlayTargetActionAnimation("BackStep01", true, true);
            }
            player.playerNetworkManager.currentStamina.Value -= dodgeStaminaCost;
            ResetStaminaValue();
        }
        public void AttemptToPerformJump()
        {
            if (player.isPerformingAction)
                return;
            if (player.playerNetworkManager.currentStamina.Value <= 0)
                return;
            //if(player.isJumping)
            //    return;
            //if (!player.isGrounded)
            //    return;

            player.playerAnimatorManager.PlayTargetActionAnimation("Main_Jump_01", false,true,true);
            player.playerNetworkManager.isJumping.Value = true;
            player.playerNetworkManager.currentStamina.Value -= jumpStaminaCostFromEdurance * player.playerNetworkManager.endurance.Value ;
            ResetStaminaValue();
            jumpDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
            jumpDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
            jumpDirection.y = 0;
            if(jumpDirection!=Vector3.zero)
            {
                if(player.playerNetworkManager.isSprinting.Value)
                {
                    jumpDirection *= 1;
                }
                else if(PlayerInputManager.instance.moveAmount>0.5f)
                {
                    jumpDirection *= 0.5f;
                }else if(PlayerInputManager.instance.moveAmount<=0.5)
                {
                    jumpDirection *= 0.25f;
                }
            }
        }
        public void ApplyJumpingVelocity()
        {
            yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
        }
        private void ResetStaminaValue()
        {
            if(player.playerNetworkManager.currentStamina.Value<0)
            {
                player.playerNetworkManager.currentStamina.Value = 0;
            }
        }
    }
}

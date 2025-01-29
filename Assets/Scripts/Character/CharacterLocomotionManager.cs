using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VP
{
    public class CharacterLocomotionManager : MonoBehaviour
    {
        CharacterManager characterManager;
        [Header("Ground Check && Jumping")]
        [SerializeField] protected float gravityForce = -9.81f;
        [SerializeField] LayerMask groundLayer;
        [SerializeField] float groundCheckSphereRadius = 1;
        [SerializeField] protected Vector3 yVelocity;
        [SerializeField] protected float groundedYVelocity = -20;
        [SerializeField] protected float fallStartYVelocity = -5;
        protected bool fallingVelocityHasBeenSet = false;
        protected float inAirTimer;
        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
        }
        protected virtual void Update()
        {
            HandleGroundCheck();
            if(characterManager.isGrounded)
            {
                if(yVelocity.y<=0)
                {
                    inAirTimer = 0;
                    fallingVelocityHasBeenSet = false;
                    yVelocity.y = groundedYVelocity;
                }
            }else
            {
                if(!characterManager.characterNetworkManager.isJumping.Value && !fallingVelocityHasBeenSet)
                {
                    fallingVelocityHasBeenSet = true;
                    yVelocity.y = fallStartYVelocity;
                }
                inAirTimer += Time.deltaTime;
                characterManager.animator.SetFloat("InAirTime", inAirTimer);

                yVelocity.y += gravityForce * Time.deltaTime;
                //characterManager.characterController.Move(yVelocity * Time.deltaTime);

            }
            characterManager.characterController.Move(yVelocity * Time.deltaTime);
        }
        public bool HandleGroundCheck()
        {
            characterManager.isGrounded = Physics.CheckSphere(characterManager.transform.position, groundCheckSphereRadius, groundLayer);
            return characterManager.isGrounded;
        }
        protected void OnDrawGizmosSelected()
        {
           // Gizmos.DrawSphere(characterManager.transform.position, groundCheckSphereRadius);
        }
    }
}

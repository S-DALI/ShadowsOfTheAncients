using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace VP
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        CharacterManager character;

        int vertical;
        int horizontal;
        [Header("Damage Animations")]
        public string lastDamageAnimationPlayed;

        [Header("Damage Animations")]
        [SerializeField] string hit_Forward_Medium_01 = "hit_Forward_Medium_01";
        [SerializeField] string hit_Forward_Medium_02 = "hit_Forward_Medium_02";

        [SerializeField] string hit_Backward_Medium_01 = "hit_Backward_Medium_01";
        [SerializeField] string hit_Backward_Medium_02 = "hit_Backward_Medium_01";

        [SerializeField] string hit_Left_Medium_01= "hit_Left_Medium_01";
        [SerializeField] string hit_Left_Medium_02 = "hit_Left_Medium_02";

        [SerializeField] string hit_Right_Medium_01= "hit_Right_Medium_01";
        [SerializeField] string hit_Right_Medium_02 = "hit_Right_Medium_02";

        public List<string> forward_Medium_Damage = new List<string>();
        public List<string> backward_Medium_Damage = new List<string>();
        public List<string> left_Medium_Damage = new List<string>();
        public List<string> right_Medium_Damage = new List<string>();
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }
        protected virtual void Start()
        {
            forward_Medium_Damage.Add(hit_Forward_Medium_01);
            forward_Medium_Damage.Add(hit_Forward_Medium_02);

            backward_Medium_Damage.Add(hit_Backward_Medium_01);
            backward_Medium_Damage.Add(hit_Backward_Medium_02);

            left_Medium_Damage.Add(hit_Left_Medium_01);
           left_Medium_Damage.Add(hit_Left_Medium_02);

            right_Medium_Damage.Add(hit_Right_Medium_01);
            right_Medium_Damage.Add(hit_Right_Medium_02);

        }
        public string GetRandomAnimationFromList(List<string> animationList)
        {
            List<string> list = new List<string>();
            foreach(var item in animationList)
            {
                list.Add(item);
            }
            list.Remove(lastDamageAnimationPlayed);
            for(int i = list.Count-1;i>-1;i--)
            {
                if (list[i]==null)
                {
                    list.RemoveAt(i);
                }
            }
            int randomValue = Random.Range(0, list.Count);

            return list[randomValue];
        }
        public void UpdateAnimatorMovementParametrs(float horizontalValue, float verticalValue, bool isSprinting)
        {
            float horizontalAmount = horizontalValue;
            float verticalAmount = verticalValue;
            if(isSprinting)
            {
                verticalAmount = 2;
            }
            character.animator.SetFloat(horizontal, horizontalAmount,0.15f,Time.deltaTime);
            character.animator.SetFloat(vertical, verticalAmount,0.15f,Time.deltaTime);

        }
        public virtual void PlayTargetActionAnimation(
            string targetAnimation, 
            bool isPerfomingAction, 
            bool applyRootMotion = true, 
            bool canRotate = false, 
            bool canMove = false)
        {
            Debug.Log("Playing Animation: "+ targetAnimation);
            character.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnimation, 0.2f);


            character.isPerformingAction = isPerfomingAction;
            character.canMove = canMove;
            character.canRotate = canRotate;

            character.characterNetworkManager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
        }

        public virtual void PlayTargetAttackActionAnimation(AttackType attackType,
          string targetAnimation,
          bool isPerfomingAction,
          bool applyRootMotion = true,
          bool canRotate = false,
          bool canMove = false)
        {
            character.characterCombatManager.currentAttackType = attackType;

            character.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnimation, 0.2f);


            character.isPerformingAction = isPerfomingAction;
            character.canMove = canMove;
            character.canRotate = canRotate;

            character.characterNetworkManager.NotifyTheServerOfAttackActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
        }
    }
}



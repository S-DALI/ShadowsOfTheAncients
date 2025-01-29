using UnityEngine;
namespace VP
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Light Attack Action")]
    public class LightAttackWeaponItemAction : WeaponItemAction
    {
        [SerializeField] string light_Attack_01 = "Main_Light_Attack_01";
        public override void AttempToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {

            base.AttempToPerformAction(playerPerformingAction, weaponPerformingAction);  
            
            if (!playerPerformingAction.IsOwner) return;

            if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
                return;
            if (!playerPerformingAction.isGrounded)
                return;

            PerformLightAttack(playerPerformingAction,weaponPerformingAction);
        }

        public void PerformLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.playerNetworkManager.isUsingRightHand.Value)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack01,light_Attack_01, true);
            }
            if (playerPerformingAction.playerNetworkManager.isUsingLeftHand.Value)
            {

            }
        }
    }
}



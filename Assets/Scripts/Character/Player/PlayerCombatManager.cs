using Unity.Netcode;
using UnityEngine;
namespace VP
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        PlayerManager player;

        public WeaponItem currentWeaponBeingUsed;
        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }
        public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformAction)
        {
            weaponAction.AttempToPerformAction(player, weaponPerformAction);

            player.playerNetworkManager.NotifyTheServerOfWeaponActionServerRpc(NetworkManager.Singleton.LocalClientId, weaponAction.actionID, weaponPerformAction.itemID);
        }

        public virtual void DrainStaminaBasedOnAttack()
        {
            if (!player.IsOwner)
                return;
            if (currentWeaponBeingUsed == null)
                return;

            float staminaDeducate = 0;
            switch (currentAttackType)
            {
                case AttackType.LightAttack01:
                    staminaDeducate = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.light_Attack_01_Modifier;
                    break;
                default: 
                    break;
            }
            player.playerNetworkManager.currentStamina.Value -= Mathf.RoundToInt(staminaDeducate);
        }
    }
}

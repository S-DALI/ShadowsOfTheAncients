using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VP
{
    [CreateAssetMenu(menuName ="Character Actions/Weapon Actions/Test Action")]
    public class WeaponItemAction : ScriptableObject
    {
        public int actionID;
        public virtual void AttempToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if(playerPerformingAction.IsOwner)
            {
                playerPerformingAction.playerNetworkManager.currentWeaponBeingUsed.Value = weaponPerformingAction.itemID;
            }
            Debug.Log("THE ACTION HAS FIRED");
        }
    }
}

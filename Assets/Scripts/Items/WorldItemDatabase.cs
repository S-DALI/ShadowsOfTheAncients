using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace VP
{
    public class WorldItemDatabase : MonoBehaviour
    {
        public static WorldItemDatabase Instance;

        public WeaponItem unarmedWeapon;

        [Header("Weapons")]
        [SerializeField] List<WeaponItem> weapons = new List<WeaponItem>();

        [Header("Items")]
        private List<Item> items = new List<Item>();

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            foreach(var weapon in weapons)
            {
                items.Add(weapon);
            }

            for(int i = 0; i < weapons.Count; i++)
            {
                items[i].itemID = i;
            }
        }
        public WeaponItem GetWeaponByID(int id)
        {
            return weapons.FirstOrDefault(weapon => weapon.itemID==id);
        }
    }
}

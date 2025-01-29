using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VP
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        CharacterManager character;
        [Header("VFX")]
        [SerializeField] GameObject bloodSplatterVFX;
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();  
        }
        public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
        {
            effect.ProcessEffect(character);
        }
        public void PlayBloodSplatterVFX(Vector3 contactPoint)
        {
            if(bloodSplatterVFX != null)
            {
                GameObject bloodSplatter = Instantiate(bloodSplatterVFX,contactPoint,Quaternion.identity);
            }
            else
            {
                GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.instance.bloodSplatterVFX, contactPoint, Quaternion.identity);
            }
        }
    }
}

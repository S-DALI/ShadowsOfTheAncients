using System.Collections.Generic;
using UnityEngine;
using VP;

namespace SG
{
    public class DamageCollider : MonoBehaviour
    {
        [Header("Collider")]
        [SerializeField] protected Collider damageCollider;
        [Header("Damage")]
        public float physicalDamage = 0;
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage = 0;
        public float holyDamage = 0;

        [Header("Contact Point")]
        protected Vector3 contactPoint;
        [Header("Character Damaged")]
        protected List<CharacterManager> characterDamaged = new List<CharacterManager>();

        [Header("VFX")]
        [SerializeField] protected GameObject vfxTrail;

        protected virtual void Awake()
        {

        }
        protected virtual void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();
            if (damageTarget != null)
            {

                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);


                DamageTarget(damageTarget);
            }
        }

        protected virtual void DamageTarget(CharacterManager damageTarget)
        {
            if (characterDamaged.Contains(damageTarget))
                return;

            characterDamaged.Add(damageTarget);
            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.lightningDamage = lightningDamage;
            damageEffect.holyDamage = holyDamage;

            damageEffect.contactPoint = contactPoint;

            damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
        }
        public virtual void EnableDamageCollider()
        {
            damageCollider.enabled = true;
            if(vfxTrail != null)
             vfxTrail.SetActive(true);
            characterDamaged.Clear();
        }
        public virtual void DisableDamageCollider()
        {
            damageCollider.enabled = false;
            if (vfxTrail != null)
                vfxTrail.SetActive(false);
            characterDamaged.Clear();
        }
    }
}

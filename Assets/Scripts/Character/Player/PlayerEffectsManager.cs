using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VP
{
    public class PlayerEffectsManager : CharacterEffectsManager
    {
        [Header("Debug / Delete Later")]
        [SerializeField] InstantCharacterEffect effectToTest;
        [SerializeField] bool processEffacte = false;

        private void Update()
        {
            if(processEffacte)
            {
                processEffacte=false;
                InstantCharacterEffect effect = Instantiate(effectToTest);

                ProcessInstantEffect(effect);
            }
        }
    }
}

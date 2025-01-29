using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VP
{
    public class UtilityDestroyAfterTime : MonoBehaviour
    {
        [SerializeField] private float timeUntilDestroyed = 5;

        private void Awake()
        {
            Destroy(gameObject,timeUntilDestroyed);
        }
    }
}

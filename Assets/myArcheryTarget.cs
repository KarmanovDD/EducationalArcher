//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Target that sends events when hit by an arrow
//
//=============================================================================

using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Valve.VR.InteractionSystem
{
    //-------------------------------------------------------------------------
    public class myArcheryTarget : MonoBehaviour
    {
        public UnityEvent onTakeDamage;
        public GameObject corpse;

        public bool onceOnly = true;

        public Transform baseTransform;
        public Transform fallenDownTransform;
        public float fallTime = 0.5f;



        private bool targetEnabled = true;


        //-------------------------------------------------
        private void ApplyDamage()
        {
            OnDamageTaken();
        }

        private void OnDamageTaken()
        {
            if (targetEnabled)
            {
                onTakeDamage.Invoke();
                StartCoroutine(this.FallDown());

                if (onceOnly)
                {
                    corpse.SetActive(false);
                }
            }
        }


        //-------------------------------------------------
        private IEnumerator FallDown()
        {
            if (baseTransform)
            {
                Quaternion startingRot = baseTransform.rotation;

                float startTime = Time.time;
                float rotLerp = 0f;

                while (rotLerp < 1)
                {
                    rotLerp = Util.RemapNumberClamped(Time.time, startTime, startTime + fallTime, 0f, 1f);
                    baseTransform.rotation = Quaternion.Lerp(startingRot, fallenDownTransform.rotation, rotLerp);
                    yield return null;
                }
            }

            yield return null;
        }
    }
}

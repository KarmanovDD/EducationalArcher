using UnityEngine;
using System.Collections;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.Events;

//-------------------------------------------------------------------------
public class mySignedBaloon : MonoBehaviour
    {
        private Hand hand;

        public GameObject popPrefab;

        public float maxVelocity = 5f;
        [HideInInspector]
        public bool isAnswer = false;

        public float lifetime = 15f;
        public bool burstOnLifetimeEnd = false;
        public UnityEvent onCorrectPop;
        public UnityEvent onMistakePop;
        public UnityEvent onExpirePop;


        public GameObject lifetimeEndParticlePrefab;
        public SoundPlayOneshot lifetimeEndSound;

        private float destructTime = 0f;
        private float releaseTime = 99999f;

        public SoundPlayOneshot collisionSound;
        private float lastSoundTime = 0f;
        private float soundDelay = 0.2f;

        private Rigidbody balloonRigidbody;

        private bool bParticlesSpawned = false;

        private static float s_flLastDeathSound = 0f;


        //-------------------------------------------------
        void Start()
        {
        destructTime = Time.time + lifetime + Random.value;
            hand = GetComponentInParent<Hand>();
            balloonRigidbody = GetComponent<Rigidbody>();
        }


        //-------------------------------------------------
        void Update()
        {
            if ((destructTime != 0) && (Time.time > destructTime))
            {
                if (burstOnLifetimeEnd)
                {
                    SpawnParticles(lifetimeEndParticlePrefab, lifetimeEndSound);
                }

            if (isAnswer)
            {
                print("on expire event");
                onExpirePop.Invoke();
            }

            Destroy(gameObject);
            }
        }


        //-------------------------------------------------
        private void SpawnParticles(GameObject particlePrefab, SoundPlayOneshot sound)
        {
            // Don't do this twice
            if (bParticlesSpawned)
            {
                return;
            }

            bParticlesSpawned = true;

            if (particlePrefab != null)
            {
                GameObject particleObject = Instantiate(particlePrefab, transform.position, transform.rotation) as GameObject;
                particleObject.GetComponent<ParticleSystem>().Play();
                Destroy(particleObject, 2f);
            }

            if (sound != null)
            {
                float lastSoundDiff = Time.time - s_flLastDeathSound;
                if (lastSoundDiff < 0.1f)
                {
                    sound.volMax *= 0.25f;
                    sound.volMin *= 0.25f;
                }
                sound.Play();
                s_flLastDeathSound = Time.time;
            }
        }


        //-------------------------------------------------
        void FixedUpdate()
        {
            // Slow-clamp velocity
            if (balloonRigidbody.velocity.sqrMagnitude > maxVelocity)
            {
                balloonRigidbody.velocity *= 0.97f;
            }
        }



        //-------------------------------------------------
        private void ApplyDamage()
        {
            SpawnParticles(popPrefab, null);
            if (isAnswer)
                onCorrectPop.Invoke();
            else
                onMistakePop.Invoke();

            Destroy(gameObject);
        }

    private void OnDestroy()
    {
        onCorrectPop.RemoveAllListeners();
        onExpirePop.RemoveAllListeners();
        onMistakePop.RemoveAllListeners();
    }


    void OnCollisionEnter(Collision collision)
        {
            if (bParticlesSpawned)
            {
                return;
            }

            Hand collisionParentHand = null;

            BalloonHapticBump balloonColliderScript = collision.gameObject.GetComponent<BalloonHapticBump>();

            if (balloonColliderScript != null && balloonColliderScript.physParent != null)
            {
                collisionParentHand = balloonColliderScript.physParent.GetComponentInParent<Hand>();
            }

            if (Time.time > (lastSoundTime + soundDelay))
            {
                if (collisionParentHand != null) // If the collision was with a controller
                {
                    if (Time.time > (releaseTime + soundDelay)) // Only play sound if it's not immediately after release
                    {
                        collisionSound.Play();
                        lastSoundTime = Time.time;
                    }
                }
                else // Collision was not with a controller, play sound
                {
                    collisionSound.Play();
                    lastSoundTime = Time.time;

                }
            }

            if (destructTime > 0) // Balloon is released away from the controller, don't do the haptic stuff that follows
            {
                return;
            }

            if (balloonRigidbody.velocity.magnitude > (maxVelocity * 10))
            {
                balloonRigidbody.velocity = balloonRigidbody.velocity.normalized * maxVelocity;
            }

            if (hand != null)
            {
                ushort collisionStrength = (ushort)Mathf.Clamp(Util.RemapNumber(collision.relativeVelocity.magnitude, 0f, 3f, 500f, 800f), 500f, 800f);

                hand.TriggerHapticPulse(collisionStrength);
            }
        }

    }

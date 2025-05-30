using System;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace KatAudio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioChild : MonoBehaviour
    {
        [NonSerialized] public AudioSource Source;
        [NonSerialized] public Transform FollowTarget;
        
        private void Awake()
        {
            Source = GetComponent<AudioSource>();
        }

        public void StartSound(CancellationToken token = default)
        {
            StartCoroutine(OnUpdate());
        }

        private IEnumerator OnUpdate()
        {
            while (Source.isPlaying)
            {
                FollowingTarget();
                yield return null;
            }
            
            this.gameObject.SetActive(false);
        }

        private void FollowingTarget()
        {
            if(!FollowTarget) return;
            transform.position = FollowTarget.position;
        }

        private void OnDisable()
        {
            Source.clip = null;
            FollowTarget = null;
        }
    }
}

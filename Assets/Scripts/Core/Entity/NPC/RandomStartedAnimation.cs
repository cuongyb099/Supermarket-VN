using UnityEngine;

namespace Core.Entity.NPC
{
    [RequireComponent(typeof(Animator))]
    public class RandomStartedAnimation : MonoBehaviour
    {
        [SerializeField] protected string[] statedAnimation;
        public Animator Anim { get; private set; }
        
        protected virtual void Awake()
        {
            Anim = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            RandomAnimation();
        }

        private void RandomAnimation()
        {
            Anim.Play(statedAnimation[Random.Range(0, statedAnimation.Length)]);
        }
    }
}

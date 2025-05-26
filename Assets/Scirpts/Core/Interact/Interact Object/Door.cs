using DG.Tweening;
using UnityEngine;

namespace Core.Interact
{
    public class Door : InteractObject
    {
        [SerializeField] protected Transform leftDoor;
        [SerializeField] protected Transform rightDoor;
        [SerializeField] protected float openTime = 0.25f;
        [SerializeField] protected float openAngle = 90f;
        protected Collider leftDoorCollider;
        protected Collider rightDoorCollider;
        public bool IsOpened { get; protected set; }
        
        protected override void Awake()
        {
            base.Awake();
            leftDoorCollider = leftDoor.GetComponent<Collider>();
            rightDoorCollider = rightDoor.GetComponent<Collider>();
        }

        protected virtual void Reset()
        {
            leftDoor = transform.GetChild(0);
            rightDoor = transform.GetChild(1);
        }

        protected override void OnInteract(Transform source)
        {
            this.CanInteract = false;
            this.ObjectOutline.DisableOutline();
            SetActiveDoorCollider(false);
            OpenDoor();

            DOVirtual.DelayedCall(openTime + 0.01f, () =>
            {
                CanInteract = true;
                SetActiveDoorCollider(true);
            });
        }

        private void OpenDoor()
        {
            if (!IsOpened)
            {
                DoorAnimation(leftDoor);
                DoorAnimation(rightDoor, -1);
                IsOpened = true;
                return;
            }

            DoorAnimation(leftDoor);
            DoorAnimation(rightDoor, -1);
            IsOpened = false;
        }

        protected virtual void SetActiveDoorCollider(bool active)
        {
            leftDoorCollider.enabled = active;
            rightDoorCollider.enabled = active;
        }
        
        private void DoorAnimation(Transform doorTransform, float factor = 1)
        {
            Vector3 doorTargetAngle = doorTransform.localEulerAngles;
          
            if (!IsOpened)
            {
                doorTargetAngle.y += openAngle * factor;
            }
            else
            {
                doorTargetAngle.y -= openAngle * factor;
            }
            
            doorTransform.DOLocalRotate(doorTargetAngle, openTime);
        }
    }
}

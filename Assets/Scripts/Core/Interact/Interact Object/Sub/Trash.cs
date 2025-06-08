using DG.Tweening;
using UnityEngine;

namespace Core.Interact
{
    public class Trash : InteractObject
    {
        [SerializeField] protected Transform leftDoor;
        [SerializeField] protected Transform rightDoor;
        [SerializeField] protected float openTime = 0.25f;
        [SerializeField] protected float openAngle = 90f;
        public bool IsOpened { get; protected set; }

        protected virtual void Reset()
        {
            leftDoor = transform.GetChild(0);
            rightDoor = transform.GetChild(1);
        }

        protected override void OnInteract(Interactor source)
        {
            if (source.CurrentInteractMode == InteractMode.Idle)
            {
                this.CanInteract = false;
                this.outline.DisableOutline();
                SetActiveCollision(false);
                OpenDoor();

                DOVirtual.DelayedCall(openTime + 0.01f, () =>
                {
                    CanInteract = true;
                    SetActiveCollision(true);
                });
            }
            else if ((source.CurrentInteractMode == InteractMode.HoldingItem || source.CurrentObjectInHand is Box) && IsOpened)
            {
                Object.Destroy(source.CurrentObjectInHand.gameObject);
                source.SwitchInteractMode(InteractMode.Idle);
            }
        }

        private void OpenDoor()
        {
            if (!IsOpened)
            {
                DoorAnimation(leftDoor);
                DoorAnimation(rightDoor);
                IsOpened = true;
                return;
            }

            DoorAnimation(leftDoor);
            DoorAnimation(rightDoor);
            IsOpened = false;
        }


        private void DoorAnimation(Transform doorTransform, float factor = 1)
        {
            Vector3 doorTargetAngle = doorTransform.localEulerAngles;
            doorTargetAngle.x += openAngle * factor;
            doorTransform.DOLocalRotate(doorTargetAngle, openTime);
        }

        public override void Focus(Interactor source)
        {
            if (source.CurrentInteractMode == InteractMode.Idle || source.CurrentInteractMode == InteractMode.HoldingItem || source.CurrentObjectInHand is Box)
            {
                outline.EnableOutline();
                ResetInteract().Forget();
            }
        }
    }
}
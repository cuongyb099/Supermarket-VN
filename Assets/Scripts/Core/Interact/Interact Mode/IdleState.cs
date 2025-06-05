using KatLib.State_Machine;
using UnityEngine;

namespace Core.Interact.Interact_Mode
{
	[System.Serializable]
	public class IdleState : InteractState
	{
		public IdleState(InteractData data, Interactor interactor, StateMachine<InteractMode, InteractState> stateMachine) : base(data, interactor, stateMachine)
		{
		
		}

		public override YieldInstruction OnUpdate()
		{
			if (data.LeftInteract.WasPressedThisFrame() && data.CurrentTargetFristSlot)
			{
				data.CurrentTargetFristSlot.Interact(this.interactor);
				return null;
			}
		
			return CheckInteractObject(ref data.CurrentTargetFristSlot);
		}

		protected virtual YieldInstruction CheckInteractObject(ref InteractObject interactObject)
		{
			Transform cameraTransform = this.data.CamTransform;
			RaycastHit[] hits = this.data.RayHits;
			float rayDistance = this.data.RayDistance;
			if (interactObject is { CanInteract: false })
			{
				interactObject = null;
			}
                
			int hitCount = Physics.RaycastNonAlloc(cameraTransform.position, 
				cameraTransform.forward, hits, rayDistance, data.RaycastLayer);

			for (int i = 0; i < hitCount; i++)
			{
				var hit = hits[i];
                
				if(!hit.transform.TryGetComponent(out InteractObject interactable) 
				   || !interactable.CanInteract) continue;
                
				interactObject?.UnFocus(this.interactor);
				interactable.Focus(this.interactor);
				if(!interactable.CanInteract) return null;
				interactObject = interactable;
				return null;
			}

			interactObject?.UnFocus(this.interactor);
			data.ResetTargetSlot(ref interactObject);

			return null;
		}
	}
}
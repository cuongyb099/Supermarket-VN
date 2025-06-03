using KatLib.State_Machine;
using UnityEngine;

namespace Core.Interact.Interact_Mode
{
	[System.Serializable]
	public class IdleState : InteractState
	{
		protected const int checkPerSecond = 10;

		public IdleState(InteractData data, Interactor interactor, StateMachine<InteractMode, InteractState> stateMachine) : base(data, interactor, stateMachine)
		{
		
		}

		public override YieldInstruction OnUpdate()
		{
			if (data.Interact.WasPressedThisFrame() && data.CurrentTarget)
			{
				data.CurrentTarget.Interact(this.interactor);
				stateMachine.ChangeState(InteractMode.HoldingItem);
				return null;
			}
			
			Transform cameraTransform = this.data.CamTransform;
			InteractObject currentTarget = this.data.CurrentTarget;
			RaycastHit[] hits = this.data.RayHits;
			float rayDistance = this.data.RayDistance;
			if (currentTarget is { CanInteract: false })
			{
				currentTarget = null;
			}
                
			int hitCount = Physics.RaycastNonAlloc(cameraTransform.position, 
				cameraTransform.forward, hits, rayDistance, data.RaycastLayer);

			for (int i = 0; i < hitCount; i++)
			{
				var hit = hits[i];
                
				if(!hit.transform.TryGetComponent(out InteractObject interactable) 
				   || !interactable.CanInteract) continue;
                
				currentTarget?.UnFocus();
				data.CurrentTarget = interactable;
				interactable.Focus();
				return null;
			}

			currentTarget?.UnFocus();
			data.ResetCurrentTarget();

			return null;
		}
	}
}
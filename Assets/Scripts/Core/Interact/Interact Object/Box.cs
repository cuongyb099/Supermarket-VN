using Core.Constant;
using UnityEngine;

namespace Core.Interact
{
    public class Box : PlacableObject
    {
        [SerializeField] protected GameGrid grid;
        [SerializeField] protected Animator anim;
        //public int count;

        protected override void Reset()
        {
            base.Reset();
            CanInteract = true;
            grid = GetComponentInChildren<GameGrid>();
            anim = GetComponentInChildren<Animator>();
        }

        public GameObject Item;

        protected override void Awake()
        {
             base.Awake();
            // var gridSize = grid.gridSize;
            // for (int x = 0; x < gridSize.x; x++)
            // {
            //     for (int y = 0; y < gridSize.y; y++)
            //     {
            //         for (int z = 0; z < gridSize.z; z++)
            //         {
            //         }
            //     }
            // }

            // for (int i = 0; i < count; i++)
            // {
            //     var clone = Instantiate(Item, grid.GetPosition(0, 0, i), Quaternion.identity);
            //     clone.transform.SetParent(transform);
            //     clone.GetComponentInChildren<Collider>().enabled = false;
            // }
        }

        [ContextMenu("Open")]
        public void OpenBox()
        {
            anim.Play(AnimConstant.OpenBox);
        }

        [ContextMenu("Close")]
        public void CloseBox()
        {
            anim.Play(AnimConstant.CloseBox);
        }
    }
}

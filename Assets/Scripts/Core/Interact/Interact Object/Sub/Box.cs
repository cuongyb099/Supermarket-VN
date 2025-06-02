using System.Collections;
using System.Collections.Generic;
using Core.Constant;
using Core.Input;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Core.Interact
{
    public class Box : PlacableObject
    {
        [SerializeField] protected Grid3D Grid3D;
        [SerializeField] protected Animator anim;
        [SerializeField] protected GameObject item;
        [SerializeField] private Vector3Int itemSize;
        public bool IsOpen { get; protected set; }
        protected List<Vector3> cacheItemPosition = new List<Vector3>();
        protected const float animTransition = 0.25f;
        protected Coroutine coroutine;
        
        protected override void Reset()
        {
            base.Reset();
            CanInteract = true;
            Grid3D = GetComponentInChildren<Grid3D>();
            anim = GetComponentInChildren<Animator>();
        }
      
        protected HashSet<Vector3Int> existCell = new HashSet<Vector3Int>();
    
        protected override void Awake()
        {
            base.Awake();
            AutoFillItem();
        }
    
        public void CreateItems(Vector3Int startCellPosition, Vector3Int itemSize, bool isMidX, bool isMidZ)
        {
            int count = 0;
            Vector3 objectPosition = Vector3.zero;
            
            for(int x = 0; x < itemSize.x; x++)
            for(int z = 0; z < itemSize.z; z++)
            {
                var cellPosition = new Vector3Int(startCellPosition.x + x, startCellPosition.y, startCellPosition.z + z);
                
                if(existCell.Contains(cellPosition) || !Grid3D.CellToWorldPosition(cellPosition, out var nextCellWP)) return;

                objectPosition += nextCellWP;
                count++;
                
                existCell.Add(cellPosition);
            }

            var itemClone = Instantiate(item, transform, true);
            
            Vector3 itemWorldPosition = objectPosition / count;
            
            if (isMidX)
            {
                Grid3D.CellToWorldPosition(new Vector3Int(0, 0, 0), out var startPoint);
                Grid3D.CellToWorldPosition(new Vector3Int(Grid3D.Size.x - 1, 0, 0), out var endPoint);
                itemWorldPosition.x = (startPoint.x + endPoint.x) / 2;
            }

            if (isMidZ)
            {
                Grid3D.CellToWorldPosition(new Vector3Int(0, 0, 0), out var startPoint);
                Grid3D.CellToWorldPosition(new Vector3Int(0, 0, Grid3D.Size.z - 1), out var endPoint);
                itemWorldPosition.z = (startPoint.z + endPoint.z) / 2;
            }
            
            itemClone.transform.position = itemWorldPosition;
            cacheItemPosition.Add(itemClone.transform.localPosition);
        }

        protected override void OnInteract(Transform source)
        {
            base.OnInteract(source);
            
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            
            rb.constraints = RigidbodyConstraints.None;
            coroutine = StartCoroutine(CheckInput());
        }

        [ContextMenu("OpenBox")]
        public void OpenBox()
        {
            anim.CrossFadeInFixedTime(AnimConstant.OpenBox, animTransition);
        }
        [ContextMenu("CloseBox")]
        public void CloseBox()
        {
            anim.CrossFadeInFixedTime(AnimConstant.CloseBox, animTransition);
        }

        public void AutoFillItem()
        {
            int maxItemX = Grid3D.Size.x / itemSize.x;
            int maxItemY = Grid3D.Size.y / itemSize.y;
            int maxItemZ = Grid3D.Size.z / itemSize.z;
            
            if(maxItemX <= 0 || maxItemY <= 0 || maxItemZ <= 0) return;

            int spaceX = 999;
            int spaceZ = 999;
            
            if (maxItemX > 1)
            {
                spaceX = (Grid3D.Size.x - maxItemX * itemSize.x) / (maxItemX - 1);
            }

            if (maxItemZ > 1)
            {
                spaceZ = (Grid3D.Size.z - maxItemZ * itemSize.z) / (maxItemZ - 1);
            }
            
            for(int x = 0; x < Grid3D.Size.x; x += itemSize.x + spaceX)
            for (int y = 0; y < Grid3D.Size.y; y += itemSize.y)
            for(int z = 0; z < Grid3D.Size.z; z += itemSize.z + spaceZ)
            {
                if(x + itemSize.x > Grid3D.Size.x 
                   || y + itemSize.y > Grid3D.Size.y
                   || z + itemSize.z > Grid3D.Size.z) continue;
                        
                var cellPosition = new Vector3Int(x, y, z);
                        
                if (existCell.Contains(cellPosition)
                    || !Grid3D.CellToWorldPosition(cellPosition, out Vector3 result)) continue;

                bool isMidX = spaceX == 999;
                bool isMidZ = spaceZ == 999;;
                
                CreateItems(cellPosition, itemSize, isMidX, isMidZ);
            }
        }

        private IEnumerator CheckInput()
        {
            InputAction close = InputManager.Instance.PlayerInputMap.Default.Close;
            
            while (true)
            {
                if (close.WasPressedThisFrame())
                {
                    IsOpen = !IsOpen;
                    if (IsOpen) OpenBox();
                    else CloseBox();
                }
                
                yield return null;
            }
        }
        
        public override void ResetToIdle()
        {
            base.ResetToIdle();
            StopCoroutine(coroutine);
        }
    }
    
    
}

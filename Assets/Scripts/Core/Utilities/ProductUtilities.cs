using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Core.Utilities
{
    public static class ProductUtilities
    {
        public static void CalculateProductPosition(Product product, Grid3D grid, ref List<Vector3> productsPosition, bool ignoreYAxis = false)
        {
            Vector3Int productSize = product.GetSize();
            int maxItemX = grid.Size.x / productSize.x;
            int maxItemY = ignoreYAxis ? 1 : grid.Size.y / productSize.y;
            productSize.y = ignoreYAxis ? 1 : productSize.y;
            int bonusIteratorY = ignoreYAxis ? 9999999 : 0;
            int maxItemZ = grid.Size.z / productSize.z;
            
            if(maxItemX <= 0 || maxItemY <= 0 || maxItemZ <= 0) return;

            //Eazy :v
            int spaceX = 9999999;
            int spaceZ = 9999999;
            
            if (maxItemX > 1)
            {
                spaceX = (grid.Size.x - maxItemX * productSize.x) / (maxItemX - 1);
            }

            if (maxItemZ > 1)
            {
                spaceZ = (grid.Size.z - maxItemZ * productSize.z) / (maxItemZ - 1);
            }
            
            for(int x = 0; x < grid.Size.x; x += productSize.x + spaceX)
            for (int y = 0; y < grid.Size.y; y += productSize.y + bonusIteratorY)
            for(int z = 0; z < grid.Size.z; z += productSize.z + spaceZ)
            {
                if(x + productSize.x > grid.Size.x 
                   || y + productSize.y > grid.Size.y
                   || z + productSize.z > grid.Size.z) continue;
                        
                var cellPosition = new Vector3Int(x, y, z);
                        
                if (!grid.IsInvalidCell(cellPosition)) continue;

                bool isMidX = spaceX == 9999999;
                bool isMidZ = spaceZ == 9999999;;
                
                CreateProductPosition(product, grid, ref productsPosition, cellPosition, isMidX, isMidZ);
            }
        }
        
        private static void CreateProductPosition(Product product, Grid3D grid, ref List<Vector3> productsPosition ,Vector3Int startCellPosition, bool isMidX, bool isMidZ)
        {
            int count = 0;
            Vector3Int productSize = product.GetSize();
            Vector3 objectPosition = Vector3.zero;
            
            for(int x = 0; x < productSize.x; x += productSize.x - 1)
            for(int z = 0; z < productSize.z; z += productSize.z - 1)
            {
                var cellPosition = new Vector3Int(startCellPosition.x + x, startCellPosition.y, startCellPosition.z + z);
                
                if(!grid.CellToWorldPosition(cellPosition, out var nextCellWP)) return;

                objectPosition += nextCellWP;
                count++;
            }

            Vector3 productPosition = objectPosition / count;
            
            if (isMidX)
            {
                grid.CellToWorldPosition(new Vector3Int(0, 0, 0), out var startPoint);
                grid.CellToWorldPosition(new Vector3Int(grid.Size.x - 1, 0, 0), out var endPoint);
                productPosition.x = (startPoint.x + endPoint.x) / 2;
            }

            if (isMidZ)
            {
                grid.CellToWorldPosition(new Vector3Int(0, 0, 0), out var startPoint);
                grid.CellToWorldPosition(new Vector3Int(0, 0, grid.Size.z - 1), out var endPoint);
                productPosition.z = (startPoint.z + endPoint.z) / 2;
            }
            
            productsPosition.Add(grid.transform.InverseTransformPoint(productPosition));
        }

        public static void DoProductCurveAnim(this Transform productTransform, TrajectoryCurveSO trajectoryCurve, Vector3 targetPosition)
        {
            productTransform.DOKill();
            DOVirtual.Float(0f, 1f, trajectoryCurve.Duration, (normalizedTime) =>
            {
                var tempPosition = Vector3.Lerp(productTransform.localPosition, targetPosition, normalizedTime);
                tempPosition.y += trajectoryCurve.Curve.Evaluate(normalizedTime) * trajectoryCurve.MaxHeight;
                productTransform.localPosition = tempPosition;
                productTransform.localRotation = Quaternion.Lerp(productTransform.localRotation, Quaternion.identity, normalizedTime);
            })
            .SetEase(trajectoryCurve.EaseMode);
        }
    }
}


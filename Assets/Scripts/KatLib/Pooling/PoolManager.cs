using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace KatLib.Pooling
{
	public enum PoolType
	{
		Projectile,
		VFX,
		Enemy,
		UIWorldSpace,
		None
	}

	public class PoolManager : Singleton.Singleton<PoolManager>
	{
		private readonly Dictionary<Object, IObjectPool> _objectPools = new ();
		private readonly Dictionary<PoolType, Transform> _poolsHolder = new(); 
		
		protected override void Awake()
		{
			base.Awake();
			SetupHolder();
		}

		private void SetupHolder()
		{
			GameObject holder = new GameObject("Pool Holder");
			holder.transform.SetParent(transform);
			var child = new Transform[transform.childCount];
			
			for (int i = 0; i < transform.childCount; i++)
			{
				child[i] = transform.GetChild(i);
			}

			foreach (PoolType pool in Enum.GetValues(typeof(PoolType)))
			{
				if (pool == PoolType.None) continue;
				
				var poolName = pool.ToString();
				
				Transform existTransform = child.FirstOrDefault(x => x.name == poolName);
				
				if (existTransform)
				{
					_poolsHolder.Add(pool, existTransform);
					continue;
				}
				
				GameObject empty = new (poolName);
				empty.transform.SetParent(holder.transform);
				_poolsHolder.Add(pool, empty.transform);
			}
		}

		public GameObject SpawnObject(GameObject objectToSpawn, Vector3 position, Quaternion rotation, PoolType poolType = PoolType.None)
		{
			if (!_objectPools.ContainsKey(objectToSpawn))
			{
				_objectPools.Add(objectToSpawn, new GameObjectObjectPool(objectToSpawn));
			}
			
			var spawnableObj = _objectPools[objectToSpawn].GetFromPool(position, rotation) as GameObject;
			
			if (poolType != PoolType.None)
			{
				spawnableObj.transform.SetParent(GetPoolParent(poolType).transform);
			}
			
			return spawnableObj;
		}

		public T SpawnObject<T>(T objectToSpawn, Vector3 position, Quaternion rotation,
			PoolType poolType = PoolType.None) where T : Component
		{
			if (!_objectPools.ContainsKey(objectToSpawn))
			{
				_objectPools.Add(objectToSpawn, new ComponentObjectPool(objectToSpawn));
			}
			
			var spawnableObj = _objectPools[objectToSpawn].GetFromPool(position, rotation) as T;

			if (poolType != PoolType.None)
			{
				spawnableObj.transform.SetParent(GetPoolParent(poolType).transform);
			}
			
			return spawnableObj;
		}
		
		public void ClearPool(bool includePersistent)
		{
			if (!includePersistent) return;
			_objectPools.Clear();
		}
		
		public GameObject GetPoolParent(PoolType poolType)
		{
			return _poolsHolder[poolType].gameObject;
		}
	}
}
using System.Collections.Generic;
using UnityEngine;

namespace BattleCubes
{
	public static class Pool
	{
		private const int DefaultPoolSize = 10;

		private static readonly Dictionary<GameObject, GameObjectPool> Pools = new Dictionary<GameObject, GameObjectPool>();

		public static void Init(GameObject prefab = null, int count = DefaultPoolSize)
		{
			if (prefab != null && Pools.ContainsKey(prefab) == false)
				Pools[prefab] = new GameObjectPool(prefab, count);
		}

		public static void Preload(GameObject prefab, int count = 1)
		{
			Init(prefab, count);
			var objects = new GameObject[count];
			for (var i = 0; i < count; i++)
				objects[i] = Spawn(prefab, Vector3.zero, Quaternion.identity);

			for (var i = 0; i < count; i++)
				Despawn(objects[i]);
		}

		public static GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot)
		{
			Init(prefab);
			return Pools[prefab].Spawn(pos, rot);
		}

		public static void Despawn(GameObject obj)
		{
			var poolObject = obj.GetComponent<PoolBehaviour>();
			if (poolObject == null)
				GameObject.Destroy(obj);
			else
				poolObject.MyPool.Despawn(obj);
		}
	}
}

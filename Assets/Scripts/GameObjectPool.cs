using System.Collections.Generic;
using UnityEngine;

namespace BattleCubes
{
	public class GameObjectPool
	{
		private readonly Stack<GameObject> _pool;
		private readonly GameObject _prefab;

		public GameObjectPool(GameObject prefab, int stackLength)
		{
			_prefab = prefab;
			_pool = new Stack<GameObject>(stackLength);
		}

		public GameObject Spawn(Vector3 pos, Quaternion rot)
		{
			GameObject obj;
			if (_pool.Count == 0)
			{
				obj = GameObject.Instantiate(_prefab, pos, rot);
				obj.AddComponent<PoolBehaviour>().MyPool = this;
			}
			else
			{
				obj = _pool.Pop();
				if (obj == null)
					return Spawn(pos, rot);
			}

			obj.transform.position = pos;
			obj.transform.rotation = rot;
			obj.SetActive(true);
			return obj;

		}

		public void Despawn(GameObject obj)
		{
			obj.SetActive(false);
			_pool.Push(obj);
		}
	}
}

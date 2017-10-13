using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BattleCubes
{
	public class GameplayController : MonoBehaviour
	{
		private const string HorizontalAxis = "Horizontal";
		private const string VerticalAxis = "Vertical";

		[SerializeField] private PlayerController _player;
		[SerializeField] private float SpawnDelay = 2f;
		[SerializeField] private int SpawnMax = 10;
		[SerializeField] private Monster[] _monsterProtos;

		private readonly List<Monster> _monsters = new List<Monster>();
		private float _lastSpawn;
		private Camera _cachedCamera;

		private void Awake()
		{
			_cachedCamera = Camera.allCameras[0];
		}

		private void Update()
		{
			var inputH = Input.GetAxis(HorizontalAxis);
			if(Mathf.Abs(inputH) > 0.1f) 
				_player.Rotate(inputH);

			var inputY = Input.GetAxis(VerticalAxis);
			if (Mathf.Abs(inputY) > 0.1f)
				_player.Move(inputY);

			if (Input.GetKey(KeyCode.X))
				_player.Fire();

			if (Input.GetKeyDown(KeyCode.W))
				_player.NextWeapon();

			if (Input.GetKeyDown(KeyCode.Q))
				_player.PrevWeapon();

			if (_player.Health <= 0)
			{
				foreach (var monster in _monsters)
					Pool.Despawn(monster.gameObject);
				_monsters.Clear();
				_player.Reset();
			}

			if (Time.time - _lastSpawn > SpawnDelay && _monsters.Count < SpawnMax && _monsterProtos.Length > 0)
			{
				_lastSpawn = Time.time;
				SpawnMonster();
			}

			foreach (var monster in _monsters)
				monster.UpdateMonster();
		}

		private void SpawnMonster()
		{
			var randX = Random.Range(0, 2) == 0 ? -1 : 1;
			var randY = Random.Range(0, 2) == 0 ? -1 : 1;

			var posX = _cachedCamera.transform.position.x + _cachedCamera.orthographicSize * _cachedCamera.aspect * randX;
			var posY = (_cachedCamera.transform.position.y + _cachedCamera.orthographicSize + 1) * randY;

			var pos = new Vector2(posX, posY);

			var monster = Pool.Spawn(_monsterProtos[Random.Range(0, _monsterProtos.Length)].gameObject,
				pos, Quaternion.identity).GetComponent<Monster>();
			monster.InitMonster(_player.transform);
			_monsters.Add(monster);
		}
	}
}

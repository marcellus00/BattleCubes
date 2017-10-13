using System.Collections.Generic;
using UnityEngine;

namespace BattleCubes
{
	public class PlayerController : MonoBehaviour
	{
		[SerializeField] private float _health = 5f;
		[SerializeField] private float _defence = 0.5f;
		[SerializeField] private float _rotateSpeed = -150.0f;
		[SerializeField] private float _moveSpeed = 3.0f;
		[SerializeField] private Weapon[] _weapons;

		private float _lastShot;
		private int _currentWeaponId;
		private GameObject _projectile;
		private readonly List<Weapon> _firedProjectiles = new List<Weapon>();
		private Camera _cachedCamera;

		public float Health { get; private set; }

		public void Reset()
		{
			Health = _health;
			transform.position = Vector3.zero;
			transform.rotation = Quaternion.identity;
			foreach (var firedProjectile in _firedProjectiles)
				Pool.Despawn(firedProjectile.gameObject);
		}

		public void Rotate(float value)
		{
			transform.Rotate(0, 0, value * Time.deltaTime * _rotateSpeed);
		}

		public void Move(float value)
		{
			transform.Translate(0, value * Time.deltaTime * _moveSpeed, 0);

			var edgeXPos = _cachedCamera.transform.position.x + _cachedCamera.orthographicSize*_cachedCamera.aspect;
			var edgeYPos = _cachedCamera.transform.position.y + _cachedCamera.orthographicSize + 1;

			if (Mathf.Abs(transform.position.x) > edgeXPos)
				transform.position = new Vector2(transform.position.x * -0.9f, transform.position.y);

			if (Mathf.Abs(transform.position.y) > edgeYPos)
				transform.position = new Vector2(transform.position.x, transform.position.y*-0.9f);
		}

		public void Fire()
		{
			if (Time.time - _lastShot < _weapons[_currentWeaponId].Delay) return;
			_lastShot = Time.time;
			var readyProjectile = Pool.Spawn(_weapons[_currentWeaponId].gameObject, _projectile.transform.position, _projectile.transform.rotation)
				.GetComponent<Weapon>();
			if(!_firedProjectiles.Contains(readyProjectile)) _firedProjectiles.Add(readyProjectile);
			readyProjectile.Launch();
		}

		public void NextWeapon()
		{
			SwitchWeapon(true);
		}

		public void PrevWeapon()
		{
			SwitchWeapon(false);
		}

		public void AddDamage(float damage)
		{
			Health = Health - damage * _defence;
		}

		private void Awake()
		{
			_cachedCamera = Camera.allCameras[0];
			if (_weapons.Length == 0)
			{
				Debug.LogWarning("[PlayerController] No weapons!");
				return;
			}

			Health = _health;
			InitWeapon();
		}

		private void SwitchWeapon(bool next)
		{
			var nextId = next ? _currentWeaponId + 1 : _currentWeaponId - 1;
			if (nextId >= _weapons.Length) nextId = 0;
			if (nextId < 0) nextId = _weapons.Length - 1;
			_currentWeaponId = nextId;
			InitWeapon();
		}

		private void InitWeapon()
		{
			if (_projectile != null) Pool.Despawn(_projectile);
			_projectile = Pool.Spawn(_weapons[_currentWeaponId].gameObject, Vector2.zero, Quaternion.identity);
			_projectile.transform.SetParent(transform);
			_projectile.transform.localPosition = Vector2.zero;
			_projectile.transform.localRotation = Quaternion.identity;
		}
	}
}

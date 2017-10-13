using UnityEngine;

namespace BattleCubes
{
	public class Weapon : MonoBehaviour
	{
		[SerializeField] private float _damage, _speed, _delay;

		public float Damage { get { return _damage; } }
		public float Delay { get { return _damage; } }
		public bool Launched { get; private set; }

		private static Camera _cachedCamera;

		private Rigidbody2D _rg;

		public void Launch()
		{
			Launched = true;
			_rg.AddForce(transform.up * _speed);
		}

		private void FixedUpdate()
		{
			if (!Launched) return;
			var viewportPos = _cachedCamera.WorldToViewportPoint(transform.position);
			if (viewportPos.x > 1 || viewportPos.x < 0 || viewportPos.y > 1 || viewportPos.y < 0) Pool.Despawn(gameObject);
		}
		private void Awake()
		{
			if(_cachedCamera == null)
				_cachedCamera = Camera.allCameras[0];
			_rg = GetComponent<Rigidbody2D>();
		}

		private void OnEnable()
		{
			Launched = false;
		}
	}
}

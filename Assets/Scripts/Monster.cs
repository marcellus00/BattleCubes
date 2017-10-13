﻿using UnityEngine;
			var dir = _target.position - transform.position;
			var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
			transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
		}
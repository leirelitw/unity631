﻿using UnityEngine;
using System.Collections;

public class rotatorGifts : MonoBehaviour {

	void Update () {
		transform.Rotate (new Vector3 (0, 45, 0) * Time.deltaTime);
	}
}
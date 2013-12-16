﻿using UnityEngine;
using System.Collections;

public class PointWonder : Point {
	
	void Awake() {
		m_hp = 100;
		m_money = 10;
	}
	
	void Update() {
		if(dead) {
			GetComponent<tk2dSprite>().SetSprite("BigCrater");
		}
	}
}

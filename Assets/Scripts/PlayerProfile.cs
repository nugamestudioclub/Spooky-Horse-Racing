﻿using System;
using UnityEngine;

public class PlayerProfile {
	public string Name { get; private set; }

	public static PlayerProfile Idle = new PlayerProfile("Idle Player");

	public PlayerProfile(string name) {
		Name = name;
	}
}
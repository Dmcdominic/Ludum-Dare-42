﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GameSettings {
	public bool fullscreen;
	//public bool cameraShake;
	public float masterVolume;
	public float localMusicVolume;
	public float localSFXVolume;

	// Reference the two variables below for music and sound effects respectively, with something like this:
	// SettingsManager.gameSettings.musicVolume;
	// SettingsManager.gameSettings.SFXVolume;
	public float musicVolume;
	public float SFXVolume;
}

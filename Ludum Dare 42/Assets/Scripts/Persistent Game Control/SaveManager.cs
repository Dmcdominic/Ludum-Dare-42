using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour {

	// Path settings
	public static string generalPath() {
		return Application.persistentDataPath;
	}

	private static string path() {
		return generalPath() + "/SaveData.dat";
	}

	private static SaveData _saveData;
	public static SaveData saveData {
		get {
			if (_saveData == null) {
				_saveData = new SaveData(-1);
			}
			return _saveData;
		}
	}

	// Singleton instance setup
	private static SaveManager _instance;
	public static SaveManager Instance { get { return _instance; } }

	void Awake() {
		if (_instance != null && _instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			_instance = this;
		}

		if (transform.parent == null) {
			DontDestroyOnLoad(this);
		}
	}

	// Save level progress (where int level should be the index of the level completed, but 1-indexed)
	public void saveLevelProgress(int level) {
		if (level > saveData.levelProgress) {
			Save(level);
		}
	}

	// Resets saved level progress to -1
	public void resetProgress() {
		Save(-1);
	}

	// Saves the highest beaten level
	private static void Save(int level) {
		saveData.levelProgress = level;

		//BinaryFormatter bf = new BinaryFormatter();
		//FileStream file;

		//if (!File.Exists(path())) {
		//	file = File.Create(path());
		//} else {
		//	file = File.Open(path(), FileMode.Open);
		//}

		//bf.Serialize(file, saveData);
		//file.Close();
	}

	// Load level progress
	private static void LoadLevelProgress() {
		if (File.Exists(path())) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(path(), FileMode.Open);

			_saveData = (SaveData)bf.Deserialize(file);
			file.Close();
		} else {
			_saveData = new SaveData(-1);
		}
	}

}

[Serializable]
public class SaveData {
	public int levelProgress;
	public SaveData(int progress) {
		levelProgress = progress;
	}
}
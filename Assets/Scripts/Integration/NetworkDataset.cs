using UnityEngine;
using System.Collections.Generic;

public class NetworkDataset
{
	[System.Serializable]
	public class Root
	{
		public Data data;
	}

	[System.Serializable]
	public class Data
	{
		public Entity[] entities;
		public DialogEntry[] dialogs;
		public string context;
	}

	// Mirrors JSON entity data
	[System.Serializable]
	public class Entity
	{
		public int id;
		public string name;
		public bool is_enemy;
		public string description;
		public int attack;
		public int defense;
		public int health;
	}

	[System.Serializable]
	public class DialogEntry
	{
		public string id;
		public string characterId;
		public string content;
	}
}

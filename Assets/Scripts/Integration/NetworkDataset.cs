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
		public Card[] cards;
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



	[System.Serializable]
	public struct Card
	{
		public string name;
		public string desc;
		public int cost;
		public actionType[] cardType;
		public DamageType[] damageType;
		public int[] Amount;
		public string spriteLink;

		// Constructor to initialize all fields. Null array arguments are converted to empty arrays.
		public Card(string name, string desc, int cost, actionType[] cardType, DamageType[] damageType, int[] Amount, string spriteLink)
		{
			this.name = name;
			this.desc = desc;
			this.cost = cost;
			this.cardType = cardType;
			this.damageType = damageType;
			this.Amount = Amount;
			this.spriteLink = spriteLink;
		}
	}
	public enum actionType
	{
		attack = 0,
		defend = 1,
		heal = 2,
		special = 3,
	}
	public enum DamageType
	{
		physical = 0,
		elemental = 1,
		holy = 2,
		dark = 3
	}
}

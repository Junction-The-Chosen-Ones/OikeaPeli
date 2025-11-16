using UnityEngine;
using System.Collections.Generic;

public class NetworkDataset
{
	[System.Serializable]
	public class Root
	{
		public Data data;
	}

	public class cardRoot
	{
		public Data[] data;
	}
	public class cardData
	{
		public Card[] cards;
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
		public bool is_boss;
	}

	[System.Serializable]
	public class DialogEntry
	{
		public string id;
		/// <summary>
		/// See <see cref="Entity.id"/>
		/// </summary>
		public string characterId;
		public string content;
	}



	[System.Serializable]
	public class Card
	{
		public string name;
		public string desc;
		public int cost;
		public cardType[]? cardType;
		public DamageType[]? damageType;
		public int[] amount;
		public string spriteLink;

		// Constructor to initialize all fields. Null array arguments are converted to empty arrays.
		public Card(string name, string desc, int cost, cardType[] cardType, DamageType[] damageType, int[] Amount, string spriteLink)
		{
			this.name = name;
			this.desc = desc;
			this.cost = cost;
			this.cardType = cardType;
			this.damageType = damageType;
			this.amount = Amount;
			this.spriteLink = spriteLink;
		}

		public CardManager.card ToCMCard()
		{
			return new CardManager.card(
				this.name,
				this.desc,
				this.cost,
				this.cardType ?? new cardType[] { },
				this.damageType ?? new DamageType[] { },
				this.amount
			);
		}
	}
	public enum cardType
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

using UnityEngine;

public class NetworkDataPost
{
	
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

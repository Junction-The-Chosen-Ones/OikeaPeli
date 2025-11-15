using UnityEngine;

public class NetworkDataPost
{
	public class roomCompletion
	{
		public int killCount;
		public string roomType;
	}
	public struct Card
	{
		public int id;
		public string name;
		public CardType cardType;
		public EffectType effectType;
		public DamageType damageType;
		public int power;
		public int cost;
	}
	public enum CardType
	{
		healing = 0,
		defend = 1,
		damage = 2,
	}
	public enum EffectType
	{
		stun = 0,
		direct = 1,
		heal = 2,
		overtime = 3,
	}
	public enum DamageType
	{
		physical = 0,
		elematal = 1,
		holy = 2,
		dark = 3
	}
}

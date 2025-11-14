using UnityEngine;
using System.Collections.Generic;

public class NetworkDataset 
{
	public class Character
	{
		public string id;
		public string name;
		public int health;
		public int attack;
		public int defense;
		public bool isEnemy;
	}
	public class Ally
	{
		public string id;
		public string name;
		public int health;
		public int attack;
		public int defense;
	}
	public struct Card
	{
		public string id;
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
	public enum EffectType{
		stun = 0,
		direct = 1,
		heal = 2,
		overtime = 3,
	}
	public enum DamageType{
		physical = 0,
		elematal = 1,
		holy = 2,
		dark = 3
	}

	
	public class Story
	{
		public List<StoryNode> nodes;
		public List<Character> characterList;
		public List<Dialog> dialog;
	}
	public class Dialog
	{
		public string id;
		public string characterId;
		public string content;
	}
	public class  StoryNode
	{
		public string id;
		public string content;
		public StoryNode parent = null;
		public StoryNode[] children;
		public string[] choices;
		
	}
}

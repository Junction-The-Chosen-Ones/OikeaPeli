using UnityEngine;
using System.Collections.Generic;

public class target
{
	public class completedRooms
	{
		public int id;
		public List<string> rooms;
	}

	public class Character
	{
		public int id;
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

	public class Story
	{
		public List<StoryNode> nodes;
		public List<Character> characterList;
		public List<Dialog> dialog;
	}
	public class Dialog
	{
		public int id;
		public string characterId;
		public string content;
	}
	public class StoryNode
	{
		public int id;
		public string content;
		public StoryNode parent = null;
		public StoryNode[] children;
		public string[] choices;

	}
}

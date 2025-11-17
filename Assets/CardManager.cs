using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static NetworkDataset;
public class CardManager : MonoBehaviour
{


	public List<card> cardsFu = new List<card>{ };
	public List<card> cards = new();
	public int[] hand = new int[6] {0,0,0,0,0,0};
	public static GameObject[] cardobjects;
	public List<int> cardpool = new() { 1, 2, 3, 4 };
	public UIHandler uihandler;
	GameObject UICanvas;

	public class card
	{
		public string name;
		public string desc;
		public int cost;
		public cardType[] cardtype;
		public DamageType[] damagetype;
		public int[] amount;
		
		public card(string namec, string descc, int costc, cardType[] cardtypec, DamageType[] damagetypec, int[] amountc)
		{
			this.name = namec;
			this.desc = descc;
			this.cost = costc;
			this.cardtype = cardtypec;
			this.damagetype = damagetypec;
			this.amount = amountc;
		}

	}
	void Start()
	{
		cards.Add(new card("This shouldn't happen", "You have accessed the 0th index of the cards array", 1, new cardType[] { cardType.special }, new DamageType[] { DamageType.dark }, new int[] { 1000 }));
		cards.Add(new card("Shoot", "Deal 8 damange", 1, new cardType[] { cardType.attack }, new DamageType[] { DamageType.physical }, new int[] { 8 }));
		cards.Add(new card("Defend", "Defend for 3", 2, new cardType[] { cardType.defend }, new DamageType[] { DamageType.physical }, new int[] { 3 }));
		cards.Add(new card("Bandage", "Heal 5 hp to your character", 3, new cardType[] { cardType.heal }, new DamageType[] { DamageType.physical }, new int[] { 5 }));
		cards.Add(new card("Blade", "Slice for 8 damange", 1, new cardType[] { cardType.attack }, new DamageType[] { DamageType.physical }, new int[] { 8 }));
		cards.AddRange(cardsFu);

		InitializeGame();

	}

	public void CardProq(int card)
	{
		gameObject.GetComponentInChildren<PlayerStats>().HPHandling(Mathf.RoundToInt(((cards[card].cost*-1)/3))); 
		for(int i = 0; i < cards[i].cardtype.Length; i++)
		{
			switch (cards[card].cardtype[i])
			{
				case cardType.attack:
					gameObject.GetComponentInChildren<Enemy>().HPHandling(cards[card].amount[i]*-1);
					Debug.Log(cards[card].amount[i] * -1);
					break;
				case cardType.special:
					gameObject.GetComponentInChildren<Enemy>().HPHandling(-cards[card].amount[i] * -1);
					Debug.Log(cards[card].amount[i] * -1);
					break;
				case cardType.defend:
					gameObject.GetComponentInChildren<PlayerStats>().Shield += (cards[card].amount[i]);
					gameObject.GetComponentInChildren<PlayerStats>().UpdateText();
					break;
				case cardType.heal:
					gameObject.GetComponentInChildren<PlayerStats>().HPHandling(cards[card].amount[i]);
					break;
			}
		}
	}

	public void EnemyCardProq(int card)
	{
		for (int i = 0; i < cards[i].cardtype.Length; i++)
		{
			switch (cards[card].cardtype[i])
			{
				case cardType.attack:
					gameObject.GetComponentInChildren<PlayerStats>().HPHandling(cards[card].amount[i]*-1);
					Debug.Log(cards[card].amount[i] * -1);
					break;
				case cardType.special:
					gameObject.GetComponentInChildren<PlayerStats>().HPHandling(cards[card].amount[i] * -1);
					Debug.Log(cards[card].amount[i] * -1);
					break;
				case cardType.defend:
					gameObject.GetComponentInChildren<Enemy>().Shield += (cards[card].amount[i]);
					gameObject.GetComponentInChildren<Enemy>().UpdateText();
					break;
				case cardType.heal:
					gameObject.GetComponentInChildren<Enemy>().HPHandling(cards[card].amount[i]);
					break;
			}
		}
	}

	public void AddCardToHand(int kortti)
	{
		for (int i = 0; i < 6; i++)
		{
			if (hand[i] == 0)
			{
				hand[i] = kortti;
				break;
			}
		}
		UpdateCardVisual();
	}

	public void RemoveCardFromHand(int index)
	{
		hand[index] = 0;
		uihandler.cardobjects[index].GetComponent<HoverAndSelection>().hover = false;
		uihandler.cardobjects[index].GetComponent<CardVisual>().selected = false;
		uihandler.cardobjects[index].GetComponent<CardVisual>().trans.anchoredPosition = new Vector2(uihandler.cardobjects[index].GetComponent<CardVisual>().trans.anchoredPosition.x, 182);
		UpdateCardVisual();
	}

	public void AddCardToHand()
	{
		Debug.Log("Cardpool size: " + cardpool.Count);
		for(int i = 0; i < 6; i++)
		{
			if(hand[i] == 0)
			{
				var _cpi = Random.Range(1, cardpool.Count);
				Debug.LogError("Cardpool trying to get cardpool index: " + _cpi);
				hand[i] = cardpool[_cpi]; // Random.Range(1, cardpool.Count)
				break;
			}
		}
		UpdateCardVisual();
	}
	void InitializeGame()
	{
		/*
		for (int i = 0; i < 4; i++)
		{
			AddRandomCardToPool();
		}*/
		for(int i = 0; i < 3; i++)
		{
			AddCardToHand();
		}
	}
	void NewTurn()
	{
		AddCardToHand();
	}

	void UpdateCardVisual()
	{
		for(int i = 0; i < uihandler.cardobjects.Length; i++)
		{
			if(hand[i] == 0)
			{
				
				uihandler.cardobjects[i].SetActive(false);
			}
			else {
				uihandler.cardobjects[i].SetActive(true);
				uihandler.cardobjects[i].GetComponent<CardVisual>().set(cards[hand[i]].name, cards[hand[i]].desc, cards[hand[i]].cost.ToString());
			}
		}
	}


	public void PrintCards(List<CardManager.card> cardList)
	{
		if (cardList == null || cardList.Count == 0)
		{
			Debug.Log("No cards in the list.");
			return;
		}

		for (int i = 0; i < cardList.Count; i++)
		{
   //         if (c.name == null) print("1");
   //         if (c.desc == null) print("2");
			//if (c.cost == 0) print("3");
			//if (c.amount == null) print("4");
			string cardInfo = $"Card {i}:\n" +
							  $"  Name: {cardList[i].name}\n" +
							  $"  Desc: {cardList[i].desc}\n" +
							  $"  Cost: {cardList[i].cost}\n" +
							  $"  Amount: {string.Join(", ", cardList[i].amount)}\n";


			Debug.Log(cardInfo);
		}
	}

	void Update()
	{
		//PrintCards(cards);
		//print("deck size: " + cards.Count);
	}

	card ParseCardJSON(string json)
	{
		if (string.IsNullOrEmpty(json))
		{
			Debug.LogError("Card JSON is null or empty.");
			return null;
		}
		card parsedCard = JsonUtility.FromJson<card>(json);
		return parsedCard;
	}

	public void AddRandomCardToPool()
	{
		int _rcount = cards.Count;
		int _r = Random.Range(1, _rcount);
		cardpool.Add(_r);
	}
}

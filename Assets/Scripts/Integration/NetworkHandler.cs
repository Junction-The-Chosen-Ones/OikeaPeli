using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using static NetworkDataset;
public class NetworkHandler : MonoBehaviour
{
	static Root networkDataset;
	static Data story;
	static Entity[] entities;
	static Card[] cards;
	static cardRoot cardRoot;
	static List<Card> cardList;
	public CardManager cardManager;
	public MapManager map;

	public Root storyJson;
	private static string cardsJson;
	private static bool cardsReady = false;

	private void Start()
	{
		cardManager = GameObject.FindFirstObjectByType<CardManager>();
		map = FindFirstObjectByType<MapManager>();
	
		StartCoroutine(GetRequest("https://backend-new-0wd9.onrender.com/cards/all-cards", (response) =>
		{
			cardsJson = response;
			var t = JsonConvert.DeserializeObject<cardRoot>(cardsJson);
			
			cardRoot = t; // JsonConvert.DeserializeObject<cardRoot>(cardsJson);
			cards = cardRoot.data[0].cards;
			
			Debug.Log("Cards JSON: " + cardsJson);

			Debug.Log("Cards: " + cards);
			cardList = cards.ToList();


            foreach (var card in cards)
			{
				Debug.Log($"Card Name: {card.name}, Description: {card.desc}, Cost: {card.cost}, Amount: {card.amount[0]}, Cardtype: {card.cardType}");
				cardManager.cardsFu.Add(card.ToCMCard());
            }
			
			for (int i = 0; i < 10; i++)
			{
				cardManager.AddRandomCardToPool();
			}
			Debug.LogWarning("Cardpool contains the following indices: " + cardManager.cardpool.ToArray());
			
			cardsReady = true;
		}));

		// Fetc entities
		StartCoroutine(GetRequest("https://backend-new-0wd9.onrender.com/gen/entities", (response) => {
			var t = JsonConvert.DeserializeObject<fu>(response);
			var fu = t.entities;


			foreach (Entity e in fu)
			{
				// Skippa the player
				if (!e.is_enemy)
				{
					continue;
				}
				map.enemies.Add(e);
			}
		}));

		StartCoroutine(GetRequest("https://backend-new-0wd9.onrender.com/gen/dialogs", (response) =>
		{
			var t = JsonConvert.DeserializeObject<ck>(response);
			var ck = t.dialogs;

			foreach (var d in ck)
			{
				Debug.LogError("Loading dialog: " + d.ToString());
				map.dialogs.Add(d);
			}
		}));

		// Fetch full story
		StartCoroutine(GetRequest("https://backend-new-0wd9.onrender.com/gen/full-story", (response) =>
		{
			storyJson = JsonConvert.DeserializeObject<Root>(response);
			Debug.Log("Full Story JSON: " + storyJson);
		}));
	}

	class fu
	{
		public Entity[] entities;
	}

	class ck
	{
		public DialogEntry[] dialogs;
	}

	private void Update()
	{
		//thing();
	}

	IEnumerator GetRequest(string uri, System.Action<string> onSuccess)
	{
		using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
		{
			yield return webRequest.SendWebRequest();

			if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
			{
				Debug.LogError($"Error requesting {uri}: {webRequest.error}");
			}
			else
			{
				string jsonResponse = webRequest.downloadHandler.text;
				onSuccess?.Invoke(jsonResponse);
			}
		}
	}

	

	//public void thing()
	//{
	//	//cardRoot cardRoot = JsonConvert.DeserializeObject<cardRoot>(cardsJson);
	//	//cards = cardRoot.data[0].cards;

	//	var _c = cards;
	//	if (_c == null)
	//	{
	//		Debug.LogError("fuck");
	//		return;
	//	}

	//	foreach (var card in _c)
	//	{
	//		Debug.Log($"Card Name: {card.name}, Description: {card.desc}, Cost: {card.cost}");
	//		cardManager.cards.Add(card.ToCMCard());
	//	}
	//}


	Root ParseJSON(string jsonInput)
	{
		return JsonConvert.DeserializeObject<Root>(jsonInput);
	}

	Card[] GetAllCards(Root root)
	{
		var cards = root.data.cards;
		return cards;
	}

	Card GetCard(Root root, int i) 
	{
		return root.data.cards[i];
	}


	Card GetRandomCard(Root root)
	{
		var cards = root.data.cards;
		if (cards.Length == 0)
		{
			Debug.LogWarning("No cards available in the dataset.");
			return default;
		}
		int randomIndex = UnityEngine.Random.Range(0, cards.Length);
		return cards[randomIndex];
	}
}

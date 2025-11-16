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
	Root networkDataset;
	Data story;
	Entity[] entities;
	Card[] cards;
	cardRoot cardRoot;
	List<Card> cardList;
	public CardManager cardManager;

	private string storyJson;
	private string cardsJson;
	private bool cardsReady = false;

	private void Start()
	{
		cardManager = GameObject.FindFirstObjectByType<CardManager>();
	
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
                Debug.Log($"Card Name: {card.name}, Description: {card.desc}, Cost: {card.cost}");
                CardManager.cards.Add(card.ToCMCard());
			}

			for (int i = 0; i < 10; i++)
			{
				cardManager.AddRandomCardToPool();
			}
			Debug.LogWarning("Cardpool contains the following indices: " + cardManager.cardpool.ToArray());
			
			cardsReady = true;
		}));


		// Fetch full story
		StartCoroutine(GetRequest("https://backend-new-0wd9.onrender.com/gen/full-story", (response) =>
		{
			storyJson = response;
			Debug.Log("Full Story JSON: " + storyJson);

		}));
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

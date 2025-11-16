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
	NetworkDataset.Root networkDataset;
	NetworkDataset.Data story;
	NetworkDataset.Entity[] entities;
	NetworkDataset.Card[] cards;
	NetworkDataset.cardRoot cardRoot;
	CardManager cardManager;

	private string storyJson;
	private string cardsJson;
	private bool cardsReady = false;

	private void Start()
	{

	
		StartCoroutine(GetRequest("https://backend-new-0wd9.onrender.com/cards/all-cards", (response) =>
		{
			cardsJson = response;
			cardRoot cardRoot = JsonConvert.DeserializeObject<cardRoot>(cardsJson);
			cards = cardRoot.data;
			Debug.Log("Cards JSON: " + cardsJson);
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
		thing();
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

	

	public void thing()
	{
		cardRoot cardRoot = JsonConvert.DeserializeObject<cardRoot>(cardsJson);
		cards = cardRoot.data;

		List<NetworkDataset.Card> cardList = cards.ToList();
		print(cardList.Count);
	}


	NetworkDataset.Root ParseJSON(string jsonInput)
	{
		return JsonConvert.DeserializeObject<NetworkDataset.Root>(jsonInput);
	}

	NetworkDataset.Card[] GetAllCards(NetworkDataset.Root root)
	{
		var cards = root.data.cards;
		return cards;
	}

	NetworkDataset.Card GetCard(NetworkDataset.Root root, int i) 
	{
		return root.data.cards[i];
	}


	NetworkDataset.Card GetRandomCard(NetworkDataset.Root root)
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

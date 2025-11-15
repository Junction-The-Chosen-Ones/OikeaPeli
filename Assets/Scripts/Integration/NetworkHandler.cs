using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Networking;


public class NetworkHandler : MonoBehaviour
{
	NetworkDataset networkDataset = new NetworkDataset();
	private string jsonResponse;


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	private void Start()
    {
		StartCoroutine(GetRequest("https://backend-new-0wd9.onrender.com/gen/full-story"));
		StartCoroutine(GetRequest("https://backend-new-0wd9.onrender.com/cards/random-card"));
	}



	IEnumerator GetRequest(string uri)
	{
		using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
		{
			yield return webRequest.SendWebRequest();

			if (webRequest.result != UnityWebRequest.Result.Success)
			{
				Debug.LogError("Error: " + webRequest.error);
				yield break;
			}

			jsonResponse = webRequest.downloadHandler.text;
			Debug.Log("Received JSON: " + jsonResponse);

			// Deserialize correctly
			NetworkDataset.Root root = JsonUtility.FromJson<NetworkDataset.Root>(jsonResponse);

			// Validation checks
			if (root == null)
			{
				Debug.LogError("Root is NULL — JSON does not match class structure!");
				yield break;
			}

			if (root.data == null)
			{
				Debug.LogError("root.data is NULL — JSON missing `data` field!");
				yield break;
			}

			Debug.Log("Entities: " + root.data.entities.Length);
			Debug.Log("Dialogs: " + root.data.dialogs.Length);
			Debug.Log("Context: " + root.data.context.Substring(0, 40) + "...");
			Debug.Log("Cards: " + root.data.cards.Length);
		}
	}

	// Update is called once per frame
	private void Update()
    {

	}
}

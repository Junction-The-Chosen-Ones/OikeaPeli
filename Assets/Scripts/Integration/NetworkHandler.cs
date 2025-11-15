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
		StartCoroutine(Upload());
		StartCoroutine(GetRequest("https://backend-new-0wd9.onrender.com/gen/full-story"));
	}
	IEnumerator Upload()
	{
		string url = "http://junction.nekofromit.com/";

		// Convert C# object to JSON
		string json = JsonUtility.ToJson(networkDataset);
		Debug.Log("Sending JSON: " + json);

		// Create POST request manually (Unity 6000+)
		var request = new UnityWebRequest(url, "POST");
		byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

		request.uploadHandler = new UploadHandlerRaw(bodyRaw);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");

		yield return request.SendWebRequest();

		if (request.result == UnityWebRequest.Result.Success)
		{
			Debug.Log("Upload success!");
			Debug.Log("Server response: " + request.downloadHandler.text);
		}
		else
		{
			Debug.LogError("Upload failed: " + request.error);
		}
	}


	IEnumerator GetRequest(string uri)
	{
		using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
		{
			yield return webRequest.SendWebRequest();

			string[] pages = uri.Split('/');
			int page = pages.Length - 1;

			if (webRequest.result != UnityWebRequest.Result.Success)
			{
				Debug.LogError("Error: " + webRequest.error);
				yield break;
			}

			// Save JSON
			jsonResponse = webRequest.downloadHandler.text;
			Debug.Log("Received JSON: " + jsonResponse);

			// Deserialize here
			networkDataset = JsonUtility.FromJson<NetworkDataset>(jsonResponse);
			//Debug.Log("Lenght of response data: " + NetworkDataset.ToString().Length);
		}
	}

	// Update is called once per frame
	private void Update()
    {
		string network = JsonUtility.ToJson(networkDataset);
		string json = JsonUtility.ToJson(networkDataset);
		print(json);
		//print(network);



	}
}

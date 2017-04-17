using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class BookText : MonoBehaviour {

	public string bookTextPath;
	public int charsPerPage = 2000;
	public TextAsset bookTextReference;

	public Text[] pagesText;
	Text pageText;

	string textContents;
	int currentPageIndex = 0;
	int maxPageIndex;

	void Start () {
		pageText = GetComponent<Text>();
		if (File.Exists(bookTextPath))
		{
			textContents = FileIOUtil.ReadStringFromFile(bookTextPath);
		}
		else {
			Debug.Log("File path is invalid. Nothing done.");
		}

		maxPageIndex = Mathf.CeilToInt(textContents.ToCharArray().Length / charsPerPage) - pagesText.Length + 1;
		SetPages();
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.T)) {
			TurnPageForward();
		}

		if (Input.GetKeyDown(KeyCode.R)) {
			TurnPageBack();
		}
	}

	public void TurnPageForward () {
		currentPageIndex = Mathf.Clamp(currentPageIndex + 1, 0, maxPageIndex);
		SetPages();
	}

	public void TurnPageBack () {
		currentPageIndex = Mathf.Clamp(currentPageIndex - 1, 0, maxPageIndex);
		SetPages();
	}

	public void SetPages () {
		for(int i = 0; i < pagesText.Length; i++) {
			pagesText[i].text = GetPage(currentPageIndex + i);
		}
	}

	string GetPage (int page) {
		return new string(textContents.ToCharArray(page * charsPerPage, charsPerPage));		
	}
}

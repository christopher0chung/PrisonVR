using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class BookText : MonoBehaviour {

	public string bookTextPath;
	public int charsPerPage = 2000;

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
			Debug.Log("file path is invalid. nothing done");
		}

		maxPageIndex = Mathf.CeilToInt(textContents.ToCharArray().Length / charsPerPage);
		pageText.text = GetPage(currentPageIndex);
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
		pageText.text = GetPage(currentPageIndex);
	}

	public void TurnPageBack () {
		currentPageIndex = Mathf.Clamp(currentPageIndex - 1, 0, maxPageIndex);
		pageText.text = GetPage(currentPageIndex);
	}

	string GetPage (int page) {
		return new string(textContents.ToCharArray(page * charsPerPage, charsPerPage));		
	}
}

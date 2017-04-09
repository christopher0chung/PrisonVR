using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BookText : MonoBehaviour {

	public string bookTextPath;

	public int charsPerPage = 2000;

	string textContents;
	int currentIndex = 0;
	string currentPageText;

	public string NextPageText {
		get {
			SetNextPageText(); 
			return currentPageText;
		}
	}

	void Start () {
		if (File.Exists(bookTextPath))
		{
			textContents = FileIOUtil.ReadStringFromFile(dataPath);
			SetNextPageText();
		}
		else {
			Debug.Log("file path is invalid. nothing done");
		}
	}
	
	void Update () {
		
	}

	public void SetNextPageText () {
		currentPageText = new string(textContents.ToCharArray(currentIndex, charsPerPage));
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreManager : MonoBehaviour {

	public int highScore = 0;

	public Text text;

	// Use this for initialization
	void Start () {
		StartCoroutine(GetHighScore());
	}

	public void SetHighScore(int value) {
		StartCoroutine(UpdateHighScore(value));
	}

	IEnumerator GetHighScore() {
		var www = new WWW("http://localhost:3000");
		yield return www;
		Debug.Log(www.text);
		highScore = int.Parse(www.text);
		text.text = highScore.ToString();
	}

	IEnumerator UpdateHighScore(int score) {
		var form = new WWWForm();
		form.AddField("score", score);
		var www = new WWW("http://localhost:3000", form);
		yield return www;
		Debug.Log(www.text);
		highScore = int.Parse(www.text);
		text.text = highScore.ToString();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Classes.Mobile;

public class GameOver : MonoBehaviour {

	public GameObject resultScreen;
	public Text resultText;
	public float alpha;
	public Spaceship player;
	// Use this for initialization
	void Start () {
		alpha = resultScreen.GetComponent<CanvasGroup> ().alpha;
	}
	
	// Update is called once per frame
	void Update () {
		if (!player.Alive)
			ItsOver ();
	}

	void ItsOver()
	{
		resultText.text = "GAME OVER";
		resultScreen.GetComponent<CanvasGroup>().alpha = 0f;
		resultScreen.SetActive (true);
		//alpha += .1f;
		StartCoroutine("FadeIn");
	}

	IEnumerator FadeIn()
	{
		while (resultScreen.GetComponent<CanvasGroup>().alpha < 1) {
			resultScreen.GetComponent<CanvasGroup>().alpha += Time.deltaTime / 1;
			Debug.Log (resultScreen.GetComponent<CanvasGroup>().alpha);
			yield return null;
		}
	}
		
}

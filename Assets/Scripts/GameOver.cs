using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Classes.Mobile;

public class GameOver : MonoBehaviour {

	public GameObject resultScreen;
	public Text resultText;
	public Color color;
	public Spaceship player;
	// Use this for initialization
	void Start () {
		color = resultScreen.GetComponent<Image> ().color;
	}
	
	// Update is called once per frame
	void Update () {
		if (!player.Alive)
			ItsOver ();
	}

	void ItsOver()
	{
		resultText.text = "GAME OVER";
		color.a = 0f;
		resultScreen.SetActive (true);
		StartCoroutine("FadeIn");
	}

	IEnumerator FadeIn()
	{
		for (float i = 0; i < 1f; i += .01f) {
			color.a = i;
			yield return new WaitForSeconds (.01f);
		}
	}
		
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Classes.Mobile;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

	public GameObject resultScreen;
	public Text resultText;
	public float alpha;
	public Spaceship player;


	private bool isReady = false;
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
		isReady = true;
		Time.timeScale = 1.0f;
		resultText.text = "GAME OVER";
		//resultScreen.GetComponent<CanvasGroup>().alpha = 0f;
		resultScreen.SetActive (true);
		//alpha += .1f;
		if (isReady) {
			StartCoroutine ("FadeIn");
			StartCoroutine ("LoadScene");
		}
	}

	IEnumerator FadeIn()
	{
		isReady = false;
		while (resultScreen.GetComponent<CanvasGroup>().alpha < 1) 
		{
			resultScreen.GetComponent<CanvasGroup> ().alpha += .01f;
			yield return new WaitForSeconds (1);
		}
		//resultScreen.GetComponent<CanvasGroup> ().alpha += .1f;
		yield return new WaitForSeconds (5);

		//Debug.Log (resultScreen.GetComponent<CanvasGroup> ().alpha);
	}

	IEnumerator LoadScene()
	{
		isReady = false;	
		yield return new WaitForSeconds (5);
		SceneManager.LoadScene (0);
	}
		
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Done_GameController : MonoBehaviour
{
	public GameObject[] hazards;
	public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;
	
	public GameObject menu;
	public Text ScoreText;
	
	private bool gameOver;
	private int score;
	
	void Start ()
	{
		gameOver = false;
		menu.SetActive(false);
		score = 0;
		UpdateScore ();
		StartCoroutine (SpawnWaves ());
	}
	
	void Update ()
	{
		
	}
	
	IEnumerator SpawnWaves ()
	{
		yield return new WaitForSeconds (startWait);
		while (true)
		{
			for (int i = 0; i < hazardCount; i++)
			{
				GameObject hazard = hazards [Random.Range (0, hazards.Length)];
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);
			
			if (gameOver)
			{
				break;
			}
		}
	}
	
	public void AddScore (int newScoreValue)
	{
		score += newScoreValue;
		UpdateScore ();
	}
	
	void UpdateScore ()
	{
		ScoreText.text = "Score: " + score;
	}
	
	public void GameOver ()
	{
		menu.SetActive(true);
		gameOver = true;
	}

	public void restart(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
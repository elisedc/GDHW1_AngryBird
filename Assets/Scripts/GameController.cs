using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public GameObject[] liveSprites = new GameObject[3];
    public GameObject[] enemies = new GameObject[1];
    public AstroidController astroid;
    public Transform farRight;
    public CameraController cam;
    public Text statusText;
    public AudioSource successSoundEffect;
    public AudioSource failSoundEffect;
    
    private int lives;
    private bool successSoundPlayed, failSoundPlayed;
    private float nextLevelWaitTime;

	// Use this for initialization
	void Start ()
    {
        lives = 3;
        statusText.gameObject.SetActive(false);
        successSoundPlayed = false;
        failSoundPlayed = false;
        nextLevelWaitTime = 0;
    }
	
	// Update is called once per frame
	void Update () {
        bool killAll = true;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
                killAll = false;
        }

        if (killAll)
        {
            statusText.text = "SUCCESS";
            statusText.gameObject.SetActive(true);
            if (!successSoundEffect.isPlaying && !successSoundPlayed)
            {
                successSoundEffect.Play();
                successSoundPlayed = true;
            }
            nextLevelWaitTime += Time.deltaTime;
            if (nextLevelWaitTime > 3.0f)
            {
                if (SceneManager.GetActiveScene().name == "LevelOne")
                    SceneManager.LoadScene("LevelTwo");
                else if (SceneManager.GetActiveScene().name == "LevelTwo")
                    SceneManager.LoadScene("WelcomeScene");
            }
                
        } else if (!killAll && lives == 0)
        {
            statusText.text = "GAME OVER";
            statusText.color = Color.red;
            statusText.gameObject.SetActive(true);
            if (!failSoundEffect.isPlaying && !failSoundPlayed)
            {
                failSoundEffect.Play();
                failSoundPlayed = true;
            }
            nextLevelWaitTime += Time.deltaTime;
            if (nextLevelWaitTime > 3.0f)
            {
                SceneManager.LoadScene("WelcomeScene");
            }
        } else if (!astroid.gameObject.activeSelf)
        {
            astroid.Reset();
            cam.transform.position = new Vector3(0, 0, -10);
            RemoveLife();
        }
	}

    private void RemoveLife ()
    {
        lives--;
        liveSprites[lives].SetActive(false);
    }
}

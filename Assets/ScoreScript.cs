using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {
    public Text scoreText;
    public Text healthText;
    public Text gameOverText;
    public Text hudText;
    public int score = 0;
    public int health = 100;
    public int shield = 100;
    public string hud = "Targeting system disabled : Strafe navigation engaged";
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        hudText.text = hud;
        scoreText.text = "Score: " + score.ToString();
        healthText.text = "Health: " + health.ToString() + "%\nShield: " + shield.ToString() + "%";
        if (health == 0)
            gameOverText.text = "GAME OVER";
	}
}

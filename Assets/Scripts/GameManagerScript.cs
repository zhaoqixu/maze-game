using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour {

    public Maze mazePrefab;

    private Maze mazeInstance;

    private CharacterController characterInstance;

    public GameObject gameOverPanel;
    public Text gameOverText;


	// Use this for initialization
	void Start () {
        mazeInstance = Instantiate(mazePrefab) as Maze;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Awake()
    {
        gameOverPanel.SetActive(false);
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = "You Win!";
        Time.timeScale = 0;
    }

    public void generateMaze()
    {
        mazeInstance.GenerateOneRow();
    }

    public void finishMaze()
    {
        mazeInstance.GenerateFinalRow();
    }
}

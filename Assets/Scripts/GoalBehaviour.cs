using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GoalBehaviour : MonoBehaviour {

    GameManagerScript GMS;
    private float rotateSpeed = 5f;

    //public Text winText;
    // Use this for initialization
    void Start () {
       GMS = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        //winText.text = "";
    }
	 
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.left * rotateSpeed);
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            GMS.GameOver();       
        }
    }
}

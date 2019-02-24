using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeWall : MonoBehaviour {

    public int numberHitted = 3;
    public bool breakable = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Projectile" )
        {
            numberHitted--;
            if (numberHitted == 0 && breakable == true)
            {
                Destroy(gameObject);
            }
        }
    }
}

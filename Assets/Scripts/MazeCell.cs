using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour {

    public int cellID;

    public MazeWall wallPrefab;

    public Hashtable walls = new Hashtable();

    // Use this for initialization
    void Start () {
        //MazeWall[] walls = new MazeWall[4];
      
    }

    // Update is called once per frame
    void Update () {
		
	}

   // public void Add(int i, MazeWall wallPrefab) // 0 north, 1 west, 2 south, 3 east
    //{
    //    if (i >= 0 && i < 4)
    //        walls[i] = wallPrefab;
    //}
}

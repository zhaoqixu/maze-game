using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour {

    public int z = 0;

    public MazeCell cellPrefab;

    public MazeWall wallPrefab;

    public Trigger triggerPrefab;

    public Projectile projectilePrefab;

    private ArrayList growingMaze = new ArrayList();

    private int globalId = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GenerateOneRow ()
    {
        MazeCell[] cells = new MazeCell[8];
        for (int x = 0; x < 8; x++)
        {

            CreateCell(x, z, cells);
            CreateProjectile(x, z);


        }
        growingMaze.Add(cells);

        eulerUpdate();

        CreateTrigger(z);
        z++;
    }

    public void GenerateFinalRow()
    {
        MazeCell[] cells = new MazeCell[8];
        for (int x = 0; x < 8; x++)
        {

            CreateCell(x, z, cells);
            CreateProjectile(x, z);
        }

        growingMaze.Add(cells);

        int ptr1 = 0;
        int ptr2 = 0;

        MazeCell[] lastRow = (MazeCell[])growingMaze[z - 1];

        // merge with last row
        for (int i = 0; i < cells.Length - 1; i++)
        {
            if (lastRow[i + 1].cellID != lastRow[i].cellID || (lastRow[i + 1].cellID == lastRow[i].cellID && i == 6))
            {
                int randomTime = Random.Range(1, ptr2 - ptr1 + 1);
                for (int j = 0; j < randomTime; j++)
                {
                    int rndNum = Random.Range(ptr1, ptr2 + 1);
                    cells[rndNum].cellID = lastRow[rndNum].cellID;
                }
                ptr1 = i + 1;
                ptr2 = i + 1;
            }
            else
            {
                ptr2++;
            }
        }
        // rightmost column
        if (lastRow[7].cellID != lastRow[6].cellID)
        {
            cells[7].cellID = lastRow[7].cellID;
        }


        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i].cellID == -1)
            {
                cells[i].cellID = globalId;
                globalId++;
            }
        }

        horizontalWallGenerator();

        verticalWallGenerator();
        
        for (int i = 1; i < cells.Length; i++)
        {
            if(cells[i].cellID != cells[i - 1].cellID)
                {
                    cells[i].cellID = cells[i - 1].cellID;
                    string name = "CellWall west" + " " + i + ", " + z;
                    GameObject wall = GameObject.Find(name);
                    if (wall)
                    {
                        Destroy(wall.gameObject);
                    }
            }
        }

        // create final south wall
        for (int i = 0; i < cells.Length; i++)
        {
            CreateWall(i, z, "north", false);
        }
    }

    private void CreateProjectile (int x, int z)
    {
        Projectile newProjectile = Instantiate(projectilePrefab) as Projectile;
        newProjectile.name = "Projectile " + x + ", " + z;
        newProjectile.transform.parent = transform;
        newProjectile.transform.localPosition = new Vector3(-35 + x * 10f, 2f, -25f - z * 10f);
    }

    private MazeCell CreateCell (int x, int z, MazeCell[] cells)
    {
        MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
        cells[x] = newCell;
        newCell.name = "Maze Cell " + x + ", " + z;
        newCell.cellID = -1;
        //globalId++;
        newCell.transform.parent = transform;
        newCell.transform.localPosition = new Vector3(-35 + x*10f , 0f, -25f - z*10f);

        return newCell;
    }

    private void CreateTrigger (int z)
    {
        Trigger newTrigger = Instantiate(triggerPrefab) as Trigger;
        newTrigger.name = "Trigger " + z;
        newTrigger.transform.parent = transform;
        newTrigger.transform.localPosition = new Vector3(0f, 5f, -29f - z * 10f);
    }

    private MazeWall CreateWall (int x, int z, string direction, bool breakable)
    {
        MazeWall newWall = Instantiate(wallPrefab) as MazeWall;
        newWall.breakable = breakable;
        newWall.name = "CellWall " + direction + " " + x + ", " + z;
        if (direction.Equals("east"))
        {
            newWall.transform.parent = transform;
            newWall.transform.localPosition = new Vector3(-30 + x*10f, 2.5f, -25f - z*10f);
        }
        else if (direction.Equals("west"))
        {
            newWall.transform.parent = transform;
            newWall.transform.localPosition = new Vector3(-40 + x * 10f, 2.5f, -25f - z * 10f);
        }
        else if (direction.Equals("north"))
        {
            newWall.transform.parent = transform;
            newWall.transform.Rotate(0f, 90f, 0f);
            newWall.transform.localPosition = new Vector3(-35 + x * 10f, 2.5f, -30f - z * 10f);
        }
        else if (direction.Equals("south"))
        {
            newWall.transform.parent = transform;
            newWall.transform.Rotate(0f, 90f, 0f);
            newWall.transform.localPosition = new Vector3(-35 + x * 10f, 2.5f, -20f - z * 10f);
        }
        return newWall;
    }

    private void eulerUpdate()
    {
        MazeCell[] eightCell = (MazeCell[])growingMaze[z];

        // first row
        if (growingMaze != null && z == 0)
        {
            // initial set
            for (int i = 0; i < eightCell.Length; i++)
            {
                eightCell[i].cellID = globalId;
                globalId++;
            }

            // merge within this row
            for (int i = 1; i< eightCell.Length; i++)
            {
                int randomNumber = UnityEngine.Random.Range(0, 3);
                if (randomNumber == 1)
                {
                    eightCell[i].cellID = eightCell[i - 1].cellID;
                }
            }

            // create vertical walls
            verticalWallGenerator();

        }
        // not first row
        else if (growingMaze != null && z > 0)
        {
            int ptr1 = 0;
            int ptr2 = 0;
            MazeCell[] lastRow = (MazeCell[])growingMaze[z - 1];
            // merge with last row
            for (int i = 0; i < eightCell.Length-1; i++)
            {
                if (lastRow[i+1].cellID != lastRow[i].cellID || (lastRow[i+1].cellID == lastRow[i].cellID && i == 6))
                {
                    int randomTime = Random.Range(1, ptr2 - ptr1 + 1);
                    for (int j = 0; j < randomTime; j++)
                    {
                        int rndNum = Random.Range(ptr1, ptr2 + 1);
                        eightCell[rndNum].cellID = lastRow[rndNum].cellID;
                    }
                    ptr1 = i + 1;
                    ptr2 = i + 1;
                }
                else
                {
                    ptr2++;
                }
            }
            // rightmost column
            if (lastRow[7].cellID != lastRow[6].cellID)
            {
                eightCell[7].cellID = lastRow[7].cellID;
            }


            for (int i = 0; i < eightCell.Length; i++)
            {
                if (eightCell[i].cellID == -1)
                {
                    eightCell[i].cellID = globalId;
                    globalId++;
                }
            }

            horizontalWallGenerator();

            verticalWallGenerator();

            // merge within current row
            for (int i = 1; i < eightCell.Length; i++)
            {
                if (eightCell[i].cellID != eightCell[i - 1].cellID)
                {
                    int randomNumber = UnityEngine.Random.Range(0, 3);
                    if (randomNumber == 0)
                    {
                        eightCell[i].cellID = eightCell[i - 1].cellID;
                        string name = "CellWall west" + " " + i + ", " + z;
                        GameObject wall = GameObject.Find(name);
                        if (wall)
                        {
                            Destroy(wall.gameObject);
                        }
                    }
                }
            }
        }
    }

    private void verticalWallGenerator()
    {
        MazeCell[] eightCell = (MazeCell[])growingMaze[z];

        if (z == 0) {
            CreateWall(0, z, "west", false);
            for (int i = 1; i < eightCell.Length; i++)
            {
                if (eightCell[i].cellID != eightCell[i-1].cellID)
                {
                    CreateWall(i, z, "west", false);
                }
            }
            CreateWall(7, z, "east", false);
        }
        else if (z > 0)
        {
            CreateWall(0, z, "west", false);
           // MazeCell[] lastRow = (MazeCell[])growingMaze[z - 1];
           
            for (int i = 1; i < eightCell.Length; i++)
            {
                if (eightCell[i].cellID != eightCell[i - 1].cellID)
                {
                    CreateWall(i, z, "west", true);
                }
            }
            CreateWall(7, z, "east", false);
        }
    }

    private void horizontalWallGenerator()
    {
        MazeCell[] currentRow = (MazeCell[])growingMaze[z];
        MazeCell[] lastRow = (MazeCell[])growingMaze[z - 1];

        for (int i = 0; i < currentRow.Length; i++)
        {
            if (currentRow[i].cellID != lastRow[i].cellID)
            {
                CreateWall(i, z, "south", true);
            }
        }
    }
}

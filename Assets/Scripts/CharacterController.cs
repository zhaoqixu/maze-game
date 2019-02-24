using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {

    public Projectile projectilePrefab;
    public Transform bulletSpawn;
    public int projectileCount = 0;

    public float speed = 10.0F;
    bool falling = false;
	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () {
        float translation = Input.GetAxis("Vertical") * speed;
        float straffe = Input.GetAxis("Horizontal") * speed;
        translation *= Time.deltaTime;
        straffe *= Time.deltaTime;

        if (falling == false)
        {
            transform.Translate(straffe, 0, translation);
        }

        if (Input.GetKeyDown("escape"))
            Cursor.lockState = CursorLockMode.None;

        if (Input.GetKeyDown("space") && falling == false)
        {
            this.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 800.0f);
        }
      

        if (Input.GetKeyDown(KeyCode.F) && projectileCount != 0 && falling == false)
        {
            Fire();
            projectileCount--;
        }

        falling = true;
    }

    void OnCollisionStay()
    {
        falling = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            projectileCount++;
        }
    }

    void Fire()
    {
        // Create the Projectile from the Bullet Prefab

        Projectile newProjectile = Instantiate(projectilePrefab,
            bulletSpawn.position,
            bulletSpawn.rotation) as Projectile;

        // Add velocity to the bullet
        newProjectile.GetComponent<Rigidbody>().velocity = newProjectile.transform.forward * 20;
        
    }

    public void SetEnable()
    {
        enabled = false;
    }
}

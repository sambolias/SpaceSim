using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotFired : MonoBehaviour {

    public float DPF = 500f;
    public float maxDistance = 2500f;
   // private Vector3 origin;
    // Use this for initialization
	void Start () {
    //    var player = GameObject.FindGameObjectWithTag("Player");
   //     if (player)
    //        DPF += player.GetComponent<Rigidbody>().velocity.magnitude;
     //   origin = player.GetComponent<PlayerStatus>().position;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = (transform.forward * 1f ) * DPF+GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().velocity;
       // rb.velocity = new Vector3(0, 1, -10);
        transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);
        transform.Rotate(new Vector3(1, 0, 0), 90f);

    }
	
	// Update is called once per frame
	void Update () {
        Rigidbody rb = GetComponent<Rigidbody>();

        //var player = GameObject.FindGameObjectWithTag("Player");
        // transform.position = origin + player.GetComponent<PlayerStatus>().position;
        transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);
        transform.Rotate(new Vector3(1, 0, 0), 90f);
        if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) > maxDistance)
        {
                Destroy(gameObject);
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);
        if(collision.gameObject.CompareTag("Asteroid"))
        {
            Destroy(gameObject);
        }
    }
}

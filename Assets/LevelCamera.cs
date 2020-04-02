using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCamera : MonoBehaviour {
    private GameObject player;
   
	// Use this for initialization
	void Start () {
        //set start pos just like camera
        /*  float lookDistance = 5f;
           float aboveDistance = 2f;
           float upDistance = 1f;
           player = GameObject.Find("Player");
           Vector3 lookDist = new Vector3(0, 0, lookDistance);
           Vector3 lookFromAbove = player.transform.up * aboveDistance;
           // var rb = GetComponent<Rigidbody>();
           transform.position = player.transform.position + lookDist + lookFromAbove;
         //  transform.LookAt(player.transform);
         //  GetComponent<Rigidbody>().transform.rotation = transform.rotation;
           transform.LookAt(player.transform.position + (player.transform.up * upDistance));
           GameObject.FindGameObjectWithTag("Move").transform.position = player.transform.position;//new Vector3(0, 0, -lookDistance);
          // GameObject.FindGameObjectWithTag("Move").transform.LookAt(player.transform.position + new Vector3(0, -1, -10));
         //  Quaternion aimup = Quaternion.LookRotation(transform.up, -transform.forward);
         //  GameObject.FindGameObjectWithTag("Move").transform.rotation= Quaternion.RotateTowards(transform.rotation, aimup, 11f);
        */
        //  GameObject.FindGameObjectWithTag("Move").transform.position = transform.position;// - new Vector3(0f, -2f, 5f);
        var pos = GameObject.FindGameObjectWithTag("Move").transform;
        transform.position = pos.position - pos.forward * 5f;
        transform.LookAt(pos.position, pos.up);
    }
	
	// Update is called once per frame
	void Update () {
        //follow imaginary pos in playerstatus
        // transform.position += player.GetComponent<PlayerStatus>().position;
    }

    public void LookEulers(Vector3 euler)
    {
        var rb = GetComponent<Rigidbody>();
        //  transform.position += rb.transform.forward * 5f;
        rb.AddTorque(euler * 1000f);
       // transform.rotation = Quaternion.Euler(euler);
     //   transform.rotation = rb.rotation;
    //    transform.position -= rb.transform.forward * 5f;

        // transform.position = rb.position + euler;
    }

    Vector3 interpolate(Vector3 from, Vector3 to, float mix)
    {
        float x = Mathf.Lerp(from.x, to.x, mix);
        float y = Mathf.Lerp(from.y, to.y, mix);
        float z = Mathf.Lerp(from.z, to.z, mix);
        return new Vector3(x, y, z);
    }
}

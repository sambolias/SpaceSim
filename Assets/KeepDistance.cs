using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepDistance : MonoBehaviour {

    GameObject player;
    public float near;
    public float far;
    private Vector3 lastPos;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        if(player)
            lastPos = player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        if (player)
        {
            Vector3 delta = lastPos - player.transform.position;
            float scale = .85f;

            if(Vector3.Distance(transform.position, player.transform.position) < near)
            {
                transform.position -= delta;
                //Debug.Log("near");
            }
            else if(Vector3.Distance(transform.position, player.transform.position) > far)
            {
                var dir = player.transform.position - transform.position;
                dir.Normalize();
                var dist = Vector3.Distance(transform.position, player.transform.position) - far;
                transform.position += dir * dist;
                //Debug.Log("far");
            }
            else
            transform.position -= delta * scale;
            
            lastPos = player.transform.position;
        }
	}
}

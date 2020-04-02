using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOrigin : MonoBehaviour {
    private GameObject player;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /*private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Moved");
            var dPos = player.transform.position;


            foreach (var obj in GameObject.FindObjectsOfType<GameObject>())
            {
                if (obj.CompareTag("Origin") || obj.CompareTag("Move")) continue;
                //move everything in level back so player is centered in origin
                //var origin = new Vector3(0, 0, 0);
                obj.transform.position -= dPos;
            }
        }
    }

    */
}

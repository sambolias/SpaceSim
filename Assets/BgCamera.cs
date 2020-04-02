using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {

        //set start pos just like camera
        float lookDistance = 5f;
        float aboveDistance = 2f;
        float upDistance = 1f;
        var player = GameObject.Find("Player");
        Vector3 lookDist = new Vector3(0, 0, lookDistance);
        Vector3 lookFromAbove = player.transform.up * aboveDistance;
        transform.position = player.transform.position + lookDist + lookFromAbove;
        //transform.LookAt(player.transform);
        transform.LookAt(player.transform.position + (player.transform.up * upDistance));

    }

    // Update is called once per frame
    void Update () {
        transform.rotation = GameObject.FindGameObjectWithTag("LevelCamera").transform.rotation;
	}
}

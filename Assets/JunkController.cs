﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkController : MonoBehaviour {
    private GameObject player;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        //follow player
        //var pos = player.GetComponent<PlayerStatus>().position;
       // transform.position = pos;// player.transform.position;

    }
}

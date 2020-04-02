using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardPlane : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(new Vector3(1, 0, 0), 90f);
    }
}

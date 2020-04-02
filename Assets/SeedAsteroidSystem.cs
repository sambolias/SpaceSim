using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Space;
public class SeedAsteroidSystem : MonoBehaviour {

    List<SpacePrimitive> asteroids;
    GameObject dust;
	// Use this for initialization
	void Start () {
        //dust = Instantiate(Resources.Load("Dust"), transform) as GameObject;
        asteroids = new List<SpacePrimitive>();
        float rad = 75000;
        float arc = 30000;
        float circ = 2f * Mathf.PI * rad;
        float step = arc / circ * 2 * Mathf.PI;
        int apu = 50; //asteroids per unit
        Vector3 origin = new Vector3(0, 0, 110000);


        for(float t = 0; t < 2*Mathf.PI; t+= step)
        {
            Vector3 pos = origin + new Vector3(rad*Mathf.Cos(t), Random.Range(-arc/2f, arc/2f), rad*Mathf.Sin(t));
            asteroids.Add(new AsteroidSpace(pos, 2f*arc, apu));
            //AsteroidSpace.Instantiate<AsteroidSpace>()
        }

        //test make asteroid belt
  /*      for(float x = -1000000; x < 1000000; x+= 25000)
        {
            Vector3 pos = new Vector3(x, 0, 10000);
            asteroids.Add(new AsteroidSpace(pos, 25000, 100));
        }*/
	}
    public void UpdateOrigins(Vector3 dPos)
    {
        foreach(var asteroid in asteroids)
            asteroid.UpdateOrigins(dPos);
    }

    // Update is called once per frame
    void Update () {
        foreach(var asteroid in asteroids)
        {
            asteroid.Update();
        }
		
	}
}

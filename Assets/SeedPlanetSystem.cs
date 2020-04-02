using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Space;

public class SeedPlanetSystem : MonoBehaviour {

    private PlanetSpace planet;
    private GameObject player;
    public GameObject obj;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        GameObject planetobj = Planet.getPlanet("earth/Earth");
        var pos = new Vector3(0, 0, 110000);
  
        planetobj.transform.localScale = new Vector3(2000, 2000, 2000);
        planetobj.transform.position = pos;
       
        planet = new PlanetSpace(planetobj);

        var radius = 50000f;
        var dir = new Vector3(1, 0, 0);
        dir.Normalize();
    
        GameObject moon = Planet.getPlanet("moon/Moon");
        moon.transform.position = pos;
        moon.transform.localScale = new Vector3(2000, 2000, 2000);

        var moonspace = new SatelliteSpace(moon);

        moonspace.SetOrbit(radius, 25, .01f, 50);
        planet.AddSatellite(moonspace);
	}
	
    public void UpdateOrigins(Vector3 dPos)
    {
        planet.MoveOrigins(dPos);
    }
	// Update is called once per frame
	void Update () {
        planet.Update();
        planet.RenderIfDistant(player.transform.position);
	}
}

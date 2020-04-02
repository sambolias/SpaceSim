using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour {

    public GameObject asteroid;
    public GameObject planet;
    public GameObject factory;
	// Use this for initialization
	void Start () {

        for (int i = 0; i < 20; i++)
        {
            var pos = new Vector3(Random.Range(-1000f, 1000f), Random.Range(-1000f, 1000f), Random.Range(-1000f, 1000f));
            Quaternion rot = new Quaternion(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1), 1f);

            var obj = GameObject.Instantiate(asteroid, pos, rot);
            GameObject.Instantiate(factory, pos, rot);

            setScale(obj, 20f, 50f);
        }

        for (int i = 0; i < 5; i++)
        {
            var pos = new Vector3(Random.Range(500f, 800f)*getRandomNegative(50f), Random.Range(500f, 800f) * getRandomNegative(50f), Random.Range(500f, 800f * getRandomNegative(50f)));
            Quaternion rot = new Quaternion(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1), 1f);

            var obj = GameObject.Instantiate(planet, pos, rot);

            float size = Random.Range(100, 300);
            obj.transform.localScale = new Vector3(size,size,size);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    float setScale(GameObject obj, float min, float max)
    {
        float xscale = Random.Range(min, max);
        float yscale = Random.Range(min, max);
        float zscale = Random.Range(min, max);
        obj.transform.localScale = new Vector3(xscale, yscale, zscale);
        return (xscale + yscale + zscale) / 3f;
    }
    float getRandomNegative(float percent)
    {
        float chance = Random.Range(0f, 1.1f);
        if (chance > (100f - percent) / 100f)
            return -1f;

        return 1f;
    }

}

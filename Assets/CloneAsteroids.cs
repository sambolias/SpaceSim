using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Space;

public class CloneAsteroids : MonoBehaviour {

    public GameObject asteroid;
    public float radius = 150;
    public int maxInPlay = 50;
    public int framesBetweenClones = 20;
    private int counter;
	// Use this for initialization
	void Start () {
        //put initial few close close
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        while (asteroids.Length < maxInPlay / 10)
        {
            asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
            cloneAsteroid(Random.Range(radius / 4f, radius / 2f), 0f);
        }
        counter = framesBetweenClones;
    }
	
	// Update is called once per frame
	void Update () {
        //this might be slow, might need to either do it on a timer
        //or keep a list of them. not sure what happens on list 
        //when objects get deleted though
        counter--;
        if (counter < 0)
        {
            counter = framesBetweenClones;
            GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");

            if (asteroids.Length < maxInPlay)
                cloneAsteroid(radius, 5f);
        }
	}

    void setScale(GameObject obj, float min, float max)
    {
        float xscale = Random.Range(min, max);
        float yscale = Random.Range(min, max);
        float zscale = Random.Range(min, max);
        obj.transform.localScale = new Vector3(xscale, yscale, zscale);
    }

    void setMaxDistance(GameObject obj, float max)
    {
        obj.GetComponent<AsteroidInit>().maxDistance = max;
    }

    void cloneAsteroid(float rad, float bigchance)
    {

        //give asteroid chance to be big
        bool big = (getRandomNegative(bigchance) < 0);
        //needs to be further away
        //TODO make how much further coincide with how much bigger
        if (big) rad *= 4;

        //for those with a chance at big (from update)
        //give them a chance to be guaranteed in front of camera
        Vector3 pos;

        pos = getRandomOnSphere(rad);

        //do not clone if pos too close to others
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        bool proxgood = true;
        while(!proxgood)
        foreach(var a in asteroids)
        {
            if(Vector3.Distance(a.transform.position, pos) < 1.25*rad)
            {
                //put it a little further away
                pos *= 1.5f;
                proxgood = false;

            }
        }

            //size by chace
            float chance = Random.Range(0f, 1.1f);
            if (big)
            {
                Asteroid.getRandom(Asteroid.Sizes.BIG, pos);
            } else if (chance > .2)
            {
                Asteroid.getRandom(Asteroid.Sizes.MEDIUM, pos);
            }
            else
            {
                Asteroid.getRandom(Asteroid.Sizes.SMALL, pos);
            }
        
    }

    Vector3 getRandomOnSphere(float rad)
    {
        float max = rad;
        //center of sphere
        float a = transform.position.x, b = transform.position.y, c = transform.position.z;
        //random x
        float x = Random.Range(-max, max);
        //new max so y isnt outside of xy circle
        max = Mathf.Sqrt(Mathf.Pow(rad, 2) - Mathf.Pow(x, 2));
        //random y
        float y = Random.Range(-max, max);
        //z on sphere
        float z = Mathf.Sqrt(-Mathf.Pow(x, 2) - Mathf.Pow(y, 2) + Mathf.Pow(rad, 2)) * getRandomNegative(50f);
        
        Vector3 position = new Vector3(x + a, y + b, z + c);
        return position;
    }

    bool withinRange()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < radius)
                return true;
        }

        return false;

    }

    //this gives you a chance to put asteroid in front of you
    Vector3 getRandomOnSphere(float rad, float chance)
    {
        Vector3 position;
        if (getRandomNegative(chance) < 0)
        {
          //  Debug.Log("cloning in front");
            //get point in front of you
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                //TODO check for null player (gameover)
                Vector3 front = player.transform.position + (player.transform.forward * rad);

                //these are guessed
                //TODO figure out math
                float halfScreenHt = Random.Range(-100f, 100f);
                float halfScreenWd = Random.Range(-150f, 150f);
                Vector3 perturbUp = player.transform.up * halfScreenHt;
                Vector3 perturbRt = player.transform.right * halfScreenWd;
                front += perturbRt + perturbUp;

                position = front;
            }
            else position = getRandomOnSphere(rad);
        }
        else position = getRandomOnSphere(rad);

        return position;
    }

    //gives percent chance to be -1 else 1
    float getRandomNegative(float percent)
    {
        float chance = Random.Range(0f, 1.1f);
        if (chance >  (100f - percent) / 100f)
            return -1f;

        return 1f;
    }
}

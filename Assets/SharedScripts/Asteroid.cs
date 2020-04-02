using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Space
{

    class Asteroid
    {
        public enum Sizes
        {
            HUGE,
            BIG,
            MEDIUM,
            SMALL
        }

        //if figure out better uv mapping then can warp more using axis scales
        static void setScale(GameObject obj, float min, float max)
        {
            float xscale = Random.Range(min, max);
            float yscale = Random.Range(min, max);
            float zscale = Random.Range(min, max);
            obj.transform.localScale = new Vector3(xscale, yscale, zscale);
        }

        private static void genLODs(GameObject parent, Sizes size)
        {

            float min=1, max=5;

            string sizestr = "";
            //TODO find best C# method
            if(size == Sizes.HUGE)
            {
                sizestr = "big";
                min = 5000; max = 15000;
            }
            if (size == Sizes.BIG)
            {
                sizestr = "big";
                min = 500; max = 1000;
            }
            else if (size == Sizes.MEDIUM)
            {
                sizestr = "medium";
                min = 100;max = 500.1f;
            }
            else
            {
                sizestr = "small";
                min = 1f; max = 50.1f;
            }

            //TODO randomize
            int num = Random.Range(0, 20);

            float scale = Random.Range(min, max);

            int lodnum = (int)scale / 5;
            if (lodnum < 1) lodnum = 1;
            if (lodnum > 5) lodnum = 5;
            LOD[] models = new LOD[lodnum];
            GameObject[] objs = new GameObject[lodnum];

            for (int i = 0; i < lodnum; i++)
            {
                //get primitive model w/ surfaces from imported fbx model
                objs[i] = GameObject.Instantiate(Resources.Load<GameObject>("asteroids/" + sizestr + "/" + sizestr + "asteroid" + num.ToString() + "_LOD" + (i+4-(lodnum-1)).ToString()).transform.GetChild(1).gameObject);

                objs[i].layer = 1;

                objs[i].transform.parent = parent.gameObject.transform;
                objs[i].tag = "Asteroid";
              
                Renderer[] rs = new Renderer[1];
                rs[0] = objs[i].gameObject.GetComponent<Renderer>();
                float srth = (float)(lodnum-1-i) / (float)lodnum;
                models[i] = new LOD(srth, rs);
            }

            foreach(var obj in objs)
            {
                if (obj == null) continue;
                var collider = obj.AddComponent<MeshCollider>();
                collider.sharedMesh = objs[lodnum-1].GetComponent<MeshFilter>().sharedMesh;
                collider.convex = true;
            }
            
            var group = parent.AddComponent<LODGroup>();
            group.SetLODs(models);
            parent.transform.localScale = new Vector3(scale, scale, scale);
            group.RecalculateBounds();
           
        }

        public static GameObject getRandom(Sizes size, Vector3 pos)
        {
            GameObject asteroid = new GameObject("asteroid");
            asteroid.layer = 1;
            genLODs(asteroid, size);
            

            asteroid.transform.position = pos;
            asteroid.tag = "Asteroid";
            var rb = asteroid.AddComponent<Rigidbody>();
            rb.useGravity = false;

            asteroid.AddComponent<AsteroidInit>();


            return asteroid;
        }
    }


    class AsteroidSpace : SpacePrimitive
    {
        private GameObject player;
        private GameObject dust;
        private List<GameObject> asteroids;
        float rad;
        int count;
        public bool unwrapped = false, seeding = false, seeded = false, activating = false;
        float n = 0;
        int i = 0;
        public AsteroidSpace(Vector3 pos, float rad, int count) : base(Unit.Meters, Type.ObjectSpace, pos)
        {
            asteroids = new List<GameObject>();
            player = GameObject.FindGameObjectWithTag("Player");
            dust = GameObject.Instantiate(Resources.Load("Dust"), pos, new Quaternion(0,0,0,1)) as GameObject;
            dust.transform.localScale *= 10f;
            dust.GetComponent<ParticleSystem>().emissionRate = 25f;
           // dust.GetComponent<ParticleSystem>().emission.rateOverTime = 25f;
            //dust.SetActive(false);
            this.rad = rad;
            this.count = count;
        }

        // Use this for initialization
        override
        public void Start()
        {
        }

        // Update is called once per frame
        override
        public void Update()
        {
            //  origin -= player.GetComponent<PlayerStatus>().dPos;
            //   Debug.Log(origin);
            //   Debug.Log(player.GetComponent<PlayerStatus>().position);
            var lookdist = 10000;
            var range = lookdist;//rad*3;
            var dist = Vector3.Distance(player.transform.position, origin);
            if (dist < 5f * range && !seeded && !seeding)
            {
                seeding = true;
                seedAsteroids();
                return;
            }

            if (Vector3.Distance(player.transform.position, origin) < range * 2.5f && seeded && !activating && !unwrapped)
            {
                activating = true;
                activateAsteroids();
                dust.GetComponent<ParticleSystem>().emissionRate = 15f;
                return;
            }
            else
            {
                unwrapped = false;
                dust.GetComponent<ParticleSystem>().emissionRate = 50;
            }
            //TODO research better way. now it will try reactivating occasionally when outside sphere
            //if (dist > range * 3f / 4f) unwrapped = false;

            if (seeding)
                seedAsteroids();
            if (activating)
                activateAsteroids();

        }
        private void activateAsteroids()
        {
             //for partially active systems
          /*   if(asteroids[i].activeSelf)
            {
                i++;
                activateAsteroids();
            }
*/
            if (i >= asteroids.Count - 1)
            {
                unwrapped = true;
                activating = false;
                i = 0;
            }
            else
            {
                if (asteroids[i] != null)
                {
                    asteroids[i].SetActive(true);
                //    asteroids[i].gameObject.GetComponent<Renderer>().enabled = false;
                }
                i++;
            }
        }
        void addAsteroid(GameObject asteroid)
        {
            //  var relative = asteroid.transform.position - origin;
            asteroid.SetActive(false);
            asteroid.transform.parent = transform;
            asteroid.transform.position -= origin;
            asteroids.Add(asteroid);
        }
        private void seedAsteroids()
        {
            //TODO save this model and make a way to create new asteroid with existing model
            //put one big one right in the middle
            if(n==0)
                addAsteroid(Asteroid.getRandom(Asteroid.Sizes.HUGE, origin));
            /*var test = Planet.getPlanet("moon/Moon");//GameObject.Instantiate(Resources.Load("moon/Moon")) as GameObject;
            test.transform.position = origin;
        //    test.AddComponent<Rigidbody>();
            var rb = test.GetComponent<Rigidbody>();
            rb.mass = 100000000000;
            rb.useGravity = false;
            test.transform.localScale = new Vector3(1000, 1000, 1000);
            */
            float step = rad / (float)count;
          //  for(float n = 0; n < rad; n += step)
          //  {
                cloneAsteroid(n, .2f);
                bubbleUp();
            //   }
            n += step;
            if (n >= rad)
            {
                seeded = true;
                seeding = false;
                n = 0;
            }

        }

        //maintain list desc mass - not most efficient TODO implement priority heap perhaps
        private void bubbleUp()
        {
            GameObject temp;
            for(int i = asteroids.Count-1; i > 0; i--)
            {
                if (asteroids[i].GetComponent<Rigidbody>().mass > asteroids[i - 1].GetComponent<Rigidbody>().mass)
                {
                    temp = asteroids[i];
                    asteroids[i] = asteroids[i - 1];
                    asteroids[i - 1] = temp;
                }
                else break; //bubbled to top
            }
        }

        void cloneAsteroid(float rad, float bigchance)
        {

            //give asteroid chance to be big
            bool big = (getRandomNegative(bigchance) < 0);
            //needs to be further away
            //TODO make how much further coincide with how much bigger
            //if (big) rad *= 4;

            //for those with a chance at big (from update)
            //give them a chance to be guaranteed in front of camera
            Vector3 pos;

            pos = getRandomOnSphere(rad);

            //do not clone if pos too close to others
            /* GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
             bool proxgood = true;
             while (!proxgood)
                 foreach (var a in asteroids)
                 {
                     if (Vector3.Distance(a.transform.position, pos) < 1.25 * rad)
                     {
                         //put it a little further away
                         pos *= 1.5f;
                         proxgood = false;

                     }
                 }*/
            GameObject asteroid = null;
            //size by chace
            float chance = Random.Range(0f, 1.1f);
            if (big)
            {
                asteroid = Asteroid.getRandom(Asteroid.Sizes.BIG, pos);
            }
            else if (chance > .2)
            {
                asteroid = Asteroid.getRandom(Asteroid.Sizes.MEDIUM, pos);
            }
            else
            {
                asteroid = Asteroid.getRandom(Asteroid.Sizes.SMALL, pos);
            }
           // asteroid.gameObject.SetActive(false);
            addAsteroid(asteroid);
        }

        Vector3 getRandomOnSphere(float rad)
        {
           // return Random.insideUnitSphere* rad*10f;
            float max = rad;
            //center of sphere
            float a = origin.x, b = origin.y, c = origin.z;
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

        //gives percent chance to be -1 else 1
        float getRandomNegative(float percent)
        {
            float chance = Random.Range(0f, 1.1f);
            if (chance > (100f - percent) / 100f)
                return -1f;

            return 1f;
        }

    }
}

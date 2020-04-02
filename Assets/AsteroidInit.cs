using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidInit : MonoBehaviour {
    public GameObject explosion;
    public GameObject smoke;
    public GameObject dust;
    public GameObject player;
    public float maxVel;
    public float maxDistance = 500000;
    public float minSpeed = 10;
    public float maxSpeed = 100;
    public float hp;
    
	// Use this for initialization
	void Start () {

        player = GameObject.FindGameObjectWithTag("Player");
        dust = Instantiate(Resources.Load<GameObject>("Dust"));
        dust.transform.position = gameObject.transform.position;
        dust.transform.parent = gameObject.transform;
        float scale = gameObject.transform.localScale.magnitude / 15f;
        dust.transform.localScale = new Vector3(scale, scale, scale);
    
        explosion = Resources.Load<GameObject>("Boom");
        smoke = Resources.Load<GameObject>("Smoke");
        explosion.transform.localScale = new Vector3(scale, scale, scale);

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.drag = 0f;
        rb.mass = transform.localScale.magnitude*20f;
        hp = rb.mass;
        //point asteroid at camera
        Vector3 velocity = Vector3.Normalize(Camera.main.transform.position - transform.position);
        //randomly perturb vel
        velocity.x = velocity.x + Random.Range(-1f, 1f);
        velocity.y = velocity.y + Random.Range(-1f, 1f);
        velocity.z = velocity.z + Random.Range(-1f, 1f);
        //set speed
        velocity = Vector3.Normalize(velocity) * Random.Range(minSpeed, maxSpeed);
        rb.velocity = velocity;
    }

    // Update is called once per frame
    void Update()
    {
        
        float max = maxDistance;
        // if (isBehind())
        //     max /= 2f;



        if (Vector3.Distance(transform.position, player.transform.position) > max)
        {
            //Destroy(gameObject);
      //      gameObject.SetActive(false);
        }
      /*  else if (Vector3.Distance(transform.position, player.transform.position) > max/2f)
        {
            var lerp = (Vector3.Distance(transform.position, player.transform.position)-max/2f) / (max/2f);
            //TODO get the one being rendered or make sure this happens only for last lod
            var mat = GetComponent<LODGroup>().GetLODs()[GetComponent<LODGroup>().GetLODs().Length - 1].renderers[0].material;
            //GetComponent<LODGroup>().ForceLOD(GetComponent<LODGroup>().GetLODs().Length - 1);
            var surf = Resources.Load("materials/surface") as Material;
            var black = Resources.Load("materials/Black") as Material;
            mat.Lerp(surf, black, lerp);
        }

        */
    }

    void FixedUpdate()
    {
        //  origin += transform.position - lastPos;
        // lastPos = transform.position;  
        //TODO figure out keep dust inbetween camera and asteroid
        // dust.transform.position = gameObject.transform.position + (gameObject.transform.position - player.transform.forward ).normalized*10f;
      /*  var dp = player.GetComponent<PlayerStatus>().dPos;
        var vel = GetComponent<Rigidbody>().velocity;
        if (vel.magnitude > maxVel)
            vel = vel.normalized * maxVel;
        transform.position -= dp - vel * Time.fixedDeltaTime;*/


    }

    //TODO max xyz maxes using localScale
    float setScale(GameObject obj, float min, float max)
    {
        float xscale = Random.Range(min, max);
        float yscale = Random.Range(min, max);
        float zscale = Random.Range(min, max);
        obj.transform.localScale = new Vector3(xscale, yscale, zscale);
        return (xscale + yscale + zscale) / 3f;
    }

    private void splitApart(int n, Vector3 pos)
    {
       
        float scaleAvg = (transform.localScale.x + transform.localScale.y + transform.localScale.z) / 3f;
        float whole = scaleAvg;
        for(int i = 0; i < n; i++)
        {
            //minimum size to create
            if (whole <= 0.00001) break;

            //make smaller asteroid
            GameObject child = Space.Asteroid.getRandom(Space.Asteroid.Sizes.SMALL, transform.position);
            //sets min and max depending on old scale

            //ensure all of mass is represented
            float min = whole / (n - i);
            float max = whole/1.5f;
            float ds = Random.Range(min, max);
            child.transform.localScale = new Vector3(ds, ds, ds);
            if (ds < 1f)
                Destroy(child, 5);

            whole -= ds;
            var b = i * Mathf.PI / (float)n;
            var a = Random.Range(0,Mathf.PI);
            var offset = new Vector3(Mathf.Sin(b) * Mathf.Cos(a), Mathf.Sin(b) * Mathf.Sin(a), Mathf.Cos(b));
            offset *= scaleAvg * (i/n);
            //50% chance to get reflected
            if (Random.Range(0, 1.1f) > .5)
                offset *= -1;
            child.transform.position = pos + offset;
            Rigidbody rb = child.GetComponent<Rigidbody>();
            //TODO make force dependant on size of piece or whole?
            rb.mass = child.transform.localScale.magnitude * 20f;
         //   if(ds < whole/5f)
         //       rb.AddExplosionForce(/*gameObject.transform.localScale.magnitude*250f / (child.transform.localScale.magnitude * 250f)*/.01f/Mathf.Pow(rb.mass,rb.mass), pos, offset.magnitude);

        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Activator"))
        {
            GetComponent<LODGroup>().enabled = true;
            //gameObject.GetComponent<Renderer>().enabled = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Activator"))
        {
            GetComponent<LODGroup>().enabled = false;
//            gameObject.GetComponent<Renderer>().enabled = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //TODO doing something weird with raycasting 
        //failing on collision with stuff not existing
        //for now just check if null - in future find cleaner way
        if(collision.gameObject.CompareTag("Asteroid"))
        {
            var rb = collision.gameObject.GetComponent<Rigidbody>();
            var atk = rb.mass / 1000f + (rb.velocity - gameObject.GetComponent<Rigidbody>().velocity).magnitude / 1000f;
            //hp -= atk;
            if(dust != null)
                dust.GetComponent<ParticleSystem>().Emit((int)atk/10);
        }

        if (collision.gameObject.CompareTag("Shot"))
        {
            var atk = 100f;

            //var pos = collision.transform.position;
            GameObject hit = Instantiate(smoke, collision.transform.position, transform.rotation) as GameObject;
            hit.transform.localScale *= .05f;
            Destroy(hit, 5);
            hp -= atk;
            dust.GetComponent<ParticleSystem>().Emit(10);
          
        }

        if (hp <= 0 && dust != null)
        {
            if (transform.localScale.magnitude > 100 && Random.Range(0,1.1f) < .2f)
            {
                var pu = Instantiate(Resources.Load("PowerUp")) as GameObject;
                pu.transform.position = gameObject.transform.position;
                pu.transform.parent = null;
            }
            //make explosion
            GameObject e = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
            Destroy(e, 5);
            GameObject s = Instantiate(smoke, transform.position, transform.rotation) as GameObject;
            Destroy(s, 5);

            //make random number of smaller asteroids
            splitApart(Random.Range(4, 9), collision.transform.position);
            dust.transform.parent = null;
            dust.transform.position = gameObject.transform.position;
            float scale = gameObject.transform.localScale.magnitude / 15f;
            dust.transform.localScale = new Vector3(scale, scale, scale); var ps = dust.GetComponent<ParticleSystem>();
            //ps.enableEmission = false;
            ps.Stop();
            Destroy(dust, 10.5f);
            Destroy(gameObject);

            //add points for kill
            float scaleAvg = (transform.localScale.x + transform.localScale.y + transform.localScale.z) / 3f;
            GameObject scoreboard = GameObject.FindGameObjectWithTag("ScoreBoard");
            int points = (int)(1f * scaleAvg);
            scoreboard.GetComponent<ScoreScript>().score += points;
        }
    }

    bool isBehind()
    {
        //DUMMY
        return false;
    }
}

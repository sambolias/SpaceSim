using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Space;

public class PlayerStatus : MonoBehaviour {
    public int health = 100;
    public int sheild = 100;
   // public Vector3 position;
    //private Vector3 lastPos;
   // public Vector3 dPos;
    //public Space.Physics physBody;
    // Use this for initialization
    void Start () {
        /* physBody = new Space.Physics();
         physBody.mass = 1000f;
         physBody.maxAccel = 10f;
         physBody.maxVel = 100f;*/
    }
	
    public void MovePos(Vector3 pos)
    {
        Debug.Log("Being called!?");
    //    dPos = pos;
     //   position += pos;
    }

	// Update is called once per frame
	void FixedUpdate () {

        //dPos = position - lastPos;
     //   position = physBody.pos;
     //   dPos = physBody.dPos;
        updateScore();
     //   physBody.Update();
		
	}
    void updateScore()
    {
        GameObject scoreboard = GameObject.FindGameObjectWithTag("ScoreBoard");
        var script = scoreboard.GetComponent<ScoreScript>();
        script.health = health;
        script.shield = sheild;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PowerUp"))
        {
            sheild += 50;
            Destroy(other.gameObject);
        }

    /*    if(other.gameObject.CompareTag("Asteroid"))
        {
            //var n = other.ClosestPoint(position).normalized;
          //  var n = (other.gameObject.transform.position - other.ClosestPoint(position)).normalized * 100f;
            //Vector3 n = physBody.vel + other.GetComponentInParent<Rigidbody>().velocity;
          //  GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().AddTorque(Vector3.Dot(n, transform.right), Vector3.Dot(n, transform.up), Vector3.Dot(n, transform.forward));
            //Debug.Log("owww!");
            //terrible approx
            var arb = other.gameObject.GetComponentInParent<Rigidbody>();
          /*  physBody.pos -= physBody.dPos;
            position = physBody.pos;
            dPos = -1f * physBody.dPos;
            physBody.dPos *= -1f;
            var temp = physBody.vel;
            physBody.vel = new Vector3();
            physBody.accel = new Vector3();/
           // Vector3 impulse = temp * -10f;
            //physBody.AddImpulse(impulse);
            var fv = physBody.Collide(arb.mass, arb.velocity);
            //arb.AddForce(-physBody.vel * 10f);
            arb.velocity = fv;
            //  physBody.UnMove();
           // physBody.Update();
            position -= dPos;
            dPos = -1 * dPos;
            //dPos = physBody.dPos;
        //physBody.Update();
            // TakeDamage(impulse);
        }*/
    }
    private bool doneOnce = true;
    private void OnTriggerExit(Collider other)
    {
       
        if (other.gameObject.CompareTag("Origin"))
        {
            //TODO find a better way to avoid calling twice (because model has 2 pieces with sep colliders...)
            doneOnce = !doneOnce;
            if (doneOnce) return;
            Debug.Log("Moved");
            var dPos = transform.position;


            foreach (var obj in GameObject.FindObjectsOfType<GameObject>())
            {
                if (obj.CompareTag("Origin") || obj.CompareTag("Move")/* || obj.CompareTag("FwdThrusters") || obj.CompareTag("Target") || obj.CompareTag("ScoreBoard") || obj.CompareTag("TargetInfo")*/) continue;
                //move everything in level back so player is centered in origin
                //var origin = new Vector3(0, 0, 0);
                if (obj.transform.parent != null) continue;
                obj.transform.position -= dPos;
            }
            var planet = GameObject.Find("PlanetSystem");
            planet.GetComponent<SeedPlanetSystem>().UpdateOrigins(dPos);
            var ast = GameObject.Find("AsteroidSystem");
            ast.GetComponent<SeedAsteroidSystem>().UpdateOrigins(dPos);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
         //   TakeDamage(collision.impulse);
        }

   
    }

    private void TakeDamage(Vector3 impulse)
    {
        //TODO make this based on force of hit
        int damage = (int)Vector3.Magnitude(impulse) / 1000;
        int tempSheild = sheild - damage;
        if (tempSheild >= 0)
            sheild = tempSheild;
        else
        {
            sheild = 0;
            health += tempSheild;
        }
        if (health <= 0)
        {
            health = 0;
            updateScore();
            Destroy(gameObject);
        }
    }
}

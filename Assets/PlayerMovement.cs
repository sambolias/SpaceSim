using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour {

    public float dPS = 100f;
    public int fPS = 20;
    private int framesSinceShot;
    private int framesSinceSysChange;
    public float strafeScalar = 10f;
    public float throttleScalar = 20f;
    public GameObject shot;
    private bool targeting = false;
    private Quaternion aim;
    private Vector3 targetpos;
    public const float targetSpriteConst = 5000;
    public float boost = 5f;
    private float boostScalar = 0f;
    private Vector2 targetSpritePos, targetHome;
    // Use this for initialization
    void Start () {
        //shot = (GameObject)Resources.Load("shot");
        framesSinceShot = fPS;
        framesSinceSysChange = fPS;
        targetpos = new Vector3(0f, 0f, 0f);
        targetHome = new Vector2(0, Screen.height / 20f);
        targetSpritePos = targetHome;
        RectTransform target = GameObject.FindGameObjectWithTag("Target").GetComponent<RectTransform>();
        target.anchoredPosition = targetHome;

       // aim = GameObject.FindGameObjectWithTag("LevelCamera").transform.rotation;
    }

    // Update is called once per frame
    void Update () {

        float shoot = Input.GetAxis("RtTrigger");
        // Quaternion rbOffset = Quaternion.Euler(GameObject.FindGameObjectWithTag("LevelCamera").transform.right * 11f);
       // Quaternion aimup = Quaternion.LookRotation(transform.up, -transform.forward);   //.AngleAxis(11f, GameObject.FindGameObjectWithTag("LevelCamera").transform.right);
      
        if (shoot > 0 && framesSinceShot == fPS)
        {
            framesSinceShot--;

            float dist = 15f;
            // float shotspeed = 10f;
            //if (!targeting) aim = GameObject.FindGameObjectWithTag("Move").transform.rotation; //not working right
            if (!targeting) aim = transform.rotation;//Quaternion.RotateTowards(transform.rotation, aimup, 11f);
            Vector3 dir = transform.forward;
            Vector3 up = transform.up;
            //  var pos = GetComponent<PlayerStatus>().position;
            var pos = transform.position + (dir * dist) - up * 2.5f;
            Instantiate(shot, pos, aim);
         //   Debug.Log(GameObject.FindGameObjectWithTag("LevelCamera").transform.rotation.eulerAngles);
        //    Debug.Log(dir);
            /* Debug.Log(pos);
             Debug.Log(dir);
             Debug.Log(up);
             Debug.Log(aim);*/

            // GameObject shotclone = Instantiate(shot, transform.position + (dir * dist), transform.rotation) as GameObject;
            // Destroy(shotclone, 5);
            //shotclone.GetComponent<GameObject>()
            /* Transform s = Instantiate(shot, transform.position + (dir * dist), Camera.main.transform.rotation);
             Rigidbody rb = s.GetComponent<Rigidbody>();
             rb.velocity = dir * shotspeed;*/
        }
        else if (framesSinceShot > 0)
            framesSinceShot--;
        else framesSinceShot = fPS;
	}

    //TODO make these avoid ship with a larger radius underneath
    //use better math!
    
    bool testTargetLimits(Vector3 t, float yoffset)
    {
        /*
        float rad = .3f;
        float xmax = 1.28f;
        float ymax = .59f;
        bool test = true;
        if (t.x > xmax) test = false;
        if (t.x < -xmax) test = false;
        if (t.y - .2 > ymax) test = false;
        if (t.y - .2 < -ymax) test = false;
        // Debug.Log(t);

        //circle at base of screen blocks ship
        Vector2 pos = new Vector2(0, .5f);
        if (Vector2.Distance(t, pos) < rad) test = false;
        */
        bool test = true;
        float rad = .5f;
        //circle in front of ship
        //Vector2 pos = new Vector2(0, -0.1f);
        if ( (Mathf.Pow(t.x, 2) / Mathf.Pow(1.25f, 2) + Mathf.Pow(t.y + .09f + yoffset, 2) / Mathf.Pow(.4f, 2)) > rad) test = false;
       // Debug.Log(t);

        return test;
    }


    void FixedUpdate()
    {
        float step = dPS * Time.fixedDeltaTime;
        float yaw = Input.GetAxis("Horizontal") * step;
        float pitch = Input.GetAxis("Vertical") * step;
        float strafe = Input.GetAxis("RtHorizontal") * step * strafeScalar;
        float lift = Input.GetAxis("RtVertical") * step * strafeScalar;
        float targetSys = Input.GetAxis("TargetSys");
        RectTransform target = GameObject.FindGameObjectWithTag("Target").GetComponent<RectTransform>();

        //TODO find a better way...
        if (targetSys > 0 && framesSinceSysChange == fPS)
        {
            GameObject scoreboard = GameObject.FindGameObjectWithTag("ScoreBoard");
            var script = scoreboard.GetComponent<ScoreScript>();

            targeting = !targeting;
            if (targeting)
            {
                script.hud = "Targeting system engaged : Strafe navigation disabled";
            }
            else
            {
                //reset shot aim
                aim = transform.rotation;
                //reset target sprite pos
                //TODO make origin public - set from editor
                target.anchoredPosition = targetHome;
                targetSpritePos = target.anchoredPosition;
                targetpos = new Vector3(0, 0, 0);
                script.hud = "Targeting system disabled : Strafe navigation engaged";
                
            }
            framesSinceSysChange--;
        }               
        else if (framesSinceSysChange > 0)
            framesSinceSysChange--;
        else framesSinceSysChange = fPS;
        
        // Quaternion rbOffset = Quaternion.Euler(GameObject.FindGameObjectWithTag("LevelCamera").transform.right * 11f);
       // Quaternion aimup = Quaternion.LookRotation(transform.up, -transform.forward);   //.AngleAxis(11f, GameObject.FindGameObjectWithTag("LevelCamera").transform.right);
        //var theta = Quaternion.Angle(aimup, aim);
        Vector3 shotdir = (10f * transform.forward).normalized;// + transform.up).normalized;// Vector3.RotateTowards(GameObject.FindGameObjectWithTag("LevelCamera").transform.forward, GameObject.FindGameObjectWithTag("LevelCamera").transform.up, Mathf.Deg2Rad*theta, 1f);
        if (targeting)
        {
            float targetConst = 100f / (Time.fixedDeltaTime / Screen.width); //23000f;//targetSpriteConst * 1.9f;
            float targetConstx = 100f / (Time.fixedDeltaTime / (Screen.width * 1.6f));
            float targetConsty = 100f / (Time.fixedDeltaTime / (Screen.height * 3.75f));

            //TODO figure out the constants 150 & 5000
            bool valid = testTargetLimits(new Vector3(targetpos.x + (strafe / targetConst), targetpos.y + (lift / targetConst)), Vector2.Distance(targetSpritePos, target.anchoredPosition)/330f);
            //TODO has to be a better way
            /*
            if (!valid)
            {
                //if trying to go past limit this puts it at limit
                float xmax = (targetpos.x > 0) ? 1f : -1f;
                float ymax = (targetpos.y > 0) ? .4f : -.4f;
                
                valid = testTargetLimits(new Vector3(targetpos.x, targetpos.y + (lift / targetConst)));
                if (!valid)
                {
                    valid = testTargetLimits(new Vector3(targetpos.x + (strafe / targetConst), targetpos.y));
                    if (valid)
                        lift = targetConst * (targetpos.y - ymax);
                    if ((lift > 0 && lift < .1) || (lift < 0 && lift > -.1))
                        lift = 0;
                }
                else
                {
                    strafe = targetConst * (targetpos.x - xmax);
                    if ((strafe > 0 && strafe < .1) || (strafe < 0 && strafe > -.1))
                        strafe = 0;
                }
            }
            */
        
            if(valid)
            {
                //update target sprite
                //move target                 
                //TODO do this with anchoredPosition in 2d
                //Vector3 pos = target.position + target.right * (strafe / targetSpriteConst) + target.up * (-lift / targetSpriteConst);
                float targetspeedcoef = .0025025f;
                Vector2 pos = targetSpritePos + new Vector2(1f, 0f ) * (strafe *targetspeedcoef* Time.fixedDeltaTime) + new Vector2(0f, 1f) * (-lift * targetspeedcoef * Time.fixedDeltaTime);
                target.anchoredPosition = pos;
                targetSpritePos = pos;


                //TODO make constant accurate (dependant on screen size?)
                //update aim vector
                targetpos.x += strafe / targetConstx;
                targetpos.y += lift / targetConsty;

                shotdir = transform.forward;
                shotdir += transform.right * targetpos.x;
                shotdir += transform.up * -targetpos.y;
                //cross product doesn't give accurate up, but this doesn't matter because shot is billboard
                //just needs to be orthagonal to fwd
                aim = Quaternion.LookRotation(shotdir, Vector3.Cross(shotdir, transform.up));
                
     
                /*
                aim = transform.rotation;
                Quaternion offset = Quaternion.Euler(transform.right * targetpos.y);
                aim *= offset;
                offset = Quaternion.Euler(transform.up * targetpos.x);
                aim *= offset;
                */
                //attempt to get target position to follow camera interp
                //target.position = transform.position + shotdir * 2f;
            }
            //reset controls
            lift = 0;
            strafe = 0;
        }

        var ti = GameObject.FindGameObjectWithTag("TargetInfo").GetComponent<Text>();
        //check aim
        RaycastHit info;
        Vector3 dir = transform.forward;
        Vector3 up = transform.up;
        //  var pos = GetComponent<PlayerStatus>().position;
        var nose = transform.position + (dir * 10f) - up * 2.5f;
        if (Physics.Raycast(new Ray(nose, shotdir), out info, 75000f))
        {
            if (info.collider.gameObject.transform.parent != null)
            {
                //Debug.Log("Aiming at " + info.collider.gameObject.transform.parent.name.ToString());
                //var center = new Vector2(info.collider.gameObject.transform.position.x, info.collider.gameObject.transform.position.y);
                //target.anchoredPosition = center;
                var scale = (75000f - info.distance) / 75000f;
                if (scale < .25) scale = .25f;
                target.localScale = new Vector3(scale, scale, 1);
                /*    var obj = info.collider.gameObject.GetComponentInParent<GameObject>(); */
                var obj = info.collider.gameObject.transform.parent.gameObject;
                float health = Mathf.Infinity;
                if (obj.tag == "Asteroid")
                    health = obj.GetComponent<AsteroidInit>().hp;
                ti.text = "Targeting: " + obj.name.ToString() + "\n" + "Target Health: " + health.ToString() + "\n" + "Distance: " + info.distance.ToString();
            }
            else ti.text = "how to ignore shot?";
        }
        else
        {
           // target.GetComponent<RectTransform>().transform.localScale = new Vector3(10, 10, 1);
            
            target.localScale = new Vector3(1,1,1);
            ti.text = "Targeting: - \nTarget Health: - \nDistance: -";
        }

        float thrust = Input.GetAxis("LtTrigger") * step * throttleScalar;
        thrust -= Input.GetAxis("Reverse") * step * (throttleScalar / 2);
        float db = Input.GetAxis("Boost");
        float boostlim = 5f;
        if (db > 0 && boostScalar < boostlim) boostScalar += .5f * Time.fixedDeltaTime;
        else if (boostScalar > .9) boostScalar -= 5f * Time.fixedDeltaTime;
        if (boostScalar < .9) boostScalar = .9f;
        if(thrust > 0 && boostScalar > 1)
            thrust *= boost * boostScalar;

        var thrusterlights = GameObject.FindGameObjectsWithTag("FwdThrusters");
        foreach(var t in thrusterlights)
        {
            var light = t.GetComponent<Light>();
            if (thrust > 1)
                if (light.intensity < 20f * Mathf.Sqrt(Mathf.Sqrt(thrust)))
                    light.intensity += .5f;
                else
                    light.intensity -= .75f;
            else
                if (light.intensity > 1) //make >= to pulsate
                    light.intensity -= 1.25f;
                else
                    light.intensity = 1f;
        }

        //TODO only declare t once - in if scope above...
        //TODO target has to move with thrust and lift and strafe (maybe yaw pitch and roll)
        //TODO these should use ship velocity in direction (dot products?) instead of thrust etc
        //Vector2 offset = new Vector2(strafe / -5f, thrust / -40f + lift / -5f);
        Rigidbody rb = GetComponent<Rigidbody>();
       // rb = GameObject.FindGameObjectWithTag("Move").GetComponent<Rigidbody>();

     /*   float a = -Screen.width/600, b = -Screen.height/400;
        float fwd = a * Vector3.Dot(rb.velocity, transform.forward) + b * Vector3.Dot(rb.velocity, transform.up);
        float max = 60f;
        if (fwd > max) fwd = max;
        if (fwd < -max) fwd = -max;
        Vector2 offset = new Vector2(a * Vector3.Dot(rb.velocity, transform.right), fwd);
        
        if(testTargetLimits(targetSpritePos + offset, Vector2.Distance(targetSpritePos, target.anchoredPosition) * Screen.height / Screen.width))
            target.anchoredPosition = targetSpritePos + offset;*/

       // target.anchoredPosition = new Vector2(0, Screen.height / 4f);
        //Debug.Log(boost * boostScalar);
        float roll = 0f;
        roll += Input.GetAxis("LtBumper") * step;
        roll -= Input.GetAxis("RtBumper") * step;

        rb.AddRelativeTorque(pitch/10f, yaw/10f, roll/10f);
        rb.AddRelativeForce(strafe / 10f, -lift / 10f, thrust / 10f);
       //  rb.AddRelativeTorque(pitch*1000, yaw * 1000, roll * 1000);
       // GameObject.FindGameObjectWithTag("LevelCamera").GetComponent<LevelCamera>().LookEulers(MakeRelative(pitch, yaw, roll));
      //  rb.AddRelativeTorque(pitch * 1, yaw * 1, roll * 1);

       // rb.AddForce(MakeRelative(strafe, -lift, thrust));
        
        //rb.AddForce(aim * Quaternion.Euler(strafe, -lift, thrust).eulerAngles);
        //transform.rotation = GameObject.FindGameObjectWithTag("LevelCamera").transform.rotation;
        //var dposfwd = new Vector3(0, 0, thrust / 100f);
       // GetComponent<PlayerStatus>().physBody.AddThrust(thrust * 100f * transform.forward + strafe * 1000f* transform.right - lift * 1000f*transform.up);//Vector3.Dot(dposfwd, transform.forward.normalized) * transform.forward.normalized + Vector3.Dot(dposfwd, transform.up.normalized) * transform.up.normalized + Vector3.Dot(dposfwd, transform.right.normalized) * transform.right.normalized;
        //rb.velocity = new Vector3(rb.velocity.x + strafe, rb.velocity.y - lift, rb.velocity.z + thrust);
       // var rb = GameObject.FindGameObjectWithTag("Move").GetComponent<Rigidbody>();
       // transform.position += rb.position;
        //rb.position = new Vector3(0, 0, 0);
        //transform.localRotation = rb.rotation;
        //rb.rotation = new Quaternion(0,0,0,1);


    }

    private Vector3 MakeRelative(float x, float y, float z)
    {
        var trans = GameObject.FindGameObjectWithTag("LevelCamera").transform;
        var forward = (5f * GameObject.FindGameObjectWithTag("LevelCamera").transform.forward + GameObject.FindGameObjectWithTag("LevelCamera").transform.up).normalized;
        var up = Vector3.Cross(forward, trans.right);
        Vector3 eulers = x * trans.right + y * up + z * forward;
      
        return eulers;
    }
}

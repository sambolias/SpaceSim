using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Space
{
    public class Physics
    {
        public float mass, maxAccel, maxVel;
        public Vector3 pos, accel, vel;
        public Vector3 dPos;
        private Vector3 dPos2;

        public void UnMove()
        {
            pos -= dPos;
            dPos = dPos2;
        }

        public void AddThrust(Vector3 force)
        {
            var a = force / mass;
          //  if (a.magnitude > maxAccel)
           //     a = a.normalized * maxAccel;
            accel = a;
            
        }

        public void AddImpulse(Vector3 impulse)
        {
            vel = impulse;
        }

        public Vector3 Collide(float m2, Vector3 v2)
        {
            accel = new Vector3();
            //little physics refresher, uses law of conservation and formula for ellastic collisions
            var ke = mass * vel + m2 * v2;
         //   Debug.Log(ke);
            //final velocity for this object
            Vector3 fv = (mass - m2)/(mass + m2) * vel + 2f*m2 / (mass + m2) * v2;  
            vel = fv;
        //    Debug.Log(fv);
        //    Debug.Log(ke - fv);
            //return the final velocity of the other object
            return (ke - mass*fv)/m2;
        }

        public void Update()
        {
            vel += accel * Time.fixedDeltaTime;
            //simulate drag (not correct space physics - better for gameplay atm)
            float dc = .00001f;
            vel -= vel.normalized*(.5f*Mathf.Pow(vel.magnitude, 2) * dc);
            accel = new Vector3();  //clear spent accel
                                    //  if (vel.magnitude > maxVel)
                                    //     vel = vel.normalized * maxVel;
            dPos2 = dPos;
            dPos = vel * Time.fixedDeltaTime;
            
          //  RaycastHit info;
          
          /*  if(UnityEngine.Physics.SphereCast(new Ray(new Vector3(), dPos), 10f,  out info, dPos.magnitude))
            {
                if (info.collider.gameObject.CompareTag("Asteroid"))
                {
                 //   var other = info.collider;
                   // var player = GameObject.FindGameObjectWithTag("Player");
                  //  var n = (other.gameObject.transform.position - other.ClosestPoint(player.GetComponent<PlayerStatus>().position)).normalized * 100f;
                    //Vector3 n = physBody.vel + other.GetComponentInParent<Rigidbody>().velocity;
                 //   GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().AddTorque(Vector3.Dot(n, player.transform.right), Vector3.Dot(n, player.transform.up), Vector3.Dot(n, player.transform.forward));
                    var rb = info.collider.GetComponentInParent<Rigidbody>();
                    var fv = Collide(rb.mass, rb.velocity);
                    //  var n = info.normal;
                    //  player.GetComponent<Rigidbody>().AddRelativeTorque(n*100f);
                 //   rb.velocity = fv;
                    //rb.AddForce(-vel * 10f);
                }
            }
            else
            {*/
                pos += dPos;
            //}

        }

    }
}

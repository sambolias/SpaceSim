using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    public GameObject player;
   
    public float followDPS = 20f;
    public float lookDistance = 5f;
    public float aboveDistance = 2f;
    public float upDistance = 1f;
    private float change = 0f;
    public float boostFactor;
    // Use this for initialization
    void Start() {

        //player = GameObject.Find("Player");
        Vector3 lookDist = new Vector3(0, 0, lookDistance);
        Vector3 lookFromAbove = player.transform.up * aboveDistance;
        transform.position = player.transform.position + lookDist + lookFromAbove;
        //transform.LookAt(player.transform);
        transform.LookAt(player.transform.position + (player.transform.up * upDistance));
      
    }
    private bool inited = false;
    private void LateUpdate()
    {
        if(!inited)
        {
            GetComponent<Camera>().nearClipPlane += .01f;
            inited = true;
        }
        else
        {

            GetComponent<Camera>().nearClipPlane -= .01f;
            inited = false;
        }
    }


    //camera follow needs done in physics update to match player movement
    //if they are not in same update then it jitters
    //now it needs to be done late or it can't keep up... TODO figure out 
    public float changepoint = .5f;
    public float changeFactor = 1000f;
    private float s = .05f;
    private float uplim = 2f;
    private float sidelim = 2.5f;
    public float maxboost = 100000;
    public float boostPower = .5f;
    private void FixedUpdate()
    {
        if (player != null)
        {
            //increase follow speed as player speed increases
            Rigidbody rb = player.GetComponent<Rigidbody>();
            float speed = rb.velocity.magnitude;
            float boost = 1f;
            if (change > changepoint) boost = speed/changeFactor;
            if (boost > 5f) boost = 5f;

            if (speed > 0f)
            {
                boost = Mathf.Pow(speed * boostFactor, boostPower);
                //    maxboost = 100 * boost;
            }
            //if (boost > maxboost) boost = maxboost;

            change += Time.fixedDeltaTime * (followDPS + boost);


            Vector3 lookDist = player.transform.forward * (lookDistance/*+boost*/);
            Vector3 lookFromAbove = player.transform.up * aboveDistance;

           // transform.position = Vector3.Lerp(transform.position, player.transform.position - lookDist + lookFromAbove, change/*(followDPS) /* Time.fixedDeltaTime*/);
            change = 0f;
            transform.position = player.transform.position - lookDist + lookFromAbove;
           // transform.LookAt(player.transform, player.transform.up);
            float lb = Input.GetAxis("LookBehind");
            if (lb > 0)
            {
                GameObject.FindGameObjectWithTag("Target").GetComponentInChildren<SpriteRenderer>().enabled = false;
                transform.position = transform.position + lookDist*2.5f;
                //transform.LookAt((player.transform.position - player.transform.forward * lookDistance) + (player.transform.up * upDistance), player.transform.up);
            }
            else
                GameObject.FindGameObjectWithTag("Target").GetComponentInChildren<SpriteRenderer>().enabled = true;

            // GameObject.FindGameObjectWithTag("Target").SetActive(true);

            //  else
            //var s = .1f;
            /* var upTranform = -1f*Vector3.Dot(rb.velocity, player.transform.up)*player.transform.up*s;

            if(Vector3.Dot(upTranform, player.transform.up) < 0f)
                if (upTranform.magnitude > uplim)
                    upTranform = upTranform.normalized * uplim;
            else
                if (upTranform.magnitude > uplim / 2f)
                    upTranform = upTranform.normalized * (uplim / 2f);
            var sideTranform = -1f*Vector3.Dot(rb.velocity, player.transform.right)*player.transform.right*s;
            if (sideTranform.magnitude > sidelim)
                sideTranform = sideTranform.normalized * sidelim;
            transform.LookAt(player.transform.position + (player.transform.up * upDistance) + upTranform + sideTranform, player.transform.up); */
            transform.LookAt(player.transform.position + (player.transform.up * upDistance), player.transform.up);

        }
    }
  
 
    Vector3 interpolate(Vector3 from, Vector3 to, float mix)
    {
        float x = Mathf.Lerp(from.x, to.x, mix);
        float y = Mathf.Lerp(from.y, to.y, mix);
        float z = Mathf.Lerp(from.z, to.z, mix);
        return new Vector3(x, y, z);
    }
}

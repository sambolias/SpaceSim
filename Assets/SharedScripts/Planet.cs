using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Space
{

    public class Planet 
    {
        public static GameObject getRandom()
        {
            GameObject planet = new GameObject("planet");
            planet.layer = 1;
            var model = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            model.layer = 1;
           // model.transform.position = planet.transform.position;
            model.GetComponent<Renderer>().material.color = new Color(0f,.5f,1f);
            model.transform.parent = planet.gameObject.transform;
            //LODGroup lod = new LODGroup();
            Renderer[] renderers = new Renderer[1];
            renderers[0] = model.gameObject.GetComponent<Renderer>();
            LOD big = new LOD(.6f, renderers);
            LOD medium = new LOD(.33f, renderers);
            LOD small = new LOD(.001f, renderers);
            LOD[] models = new LOD[3] { big, medium, small }; 
            //lod.SetLODs(models);
            var group = planet.AddComponent<LODGroup>();
            group.SetLODs(models);
            group.RecalculateBounds();
            //planet.transform.localScale = new Vector3(5000, 5000, 5000);
            
            return planet;
        }
        public static GameObject getPlanet(string path)
        {
            GameObject planet = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("planets/"+path));
            return planet;
        }
    }

    public class PlanetarySpace : SpacePrimitive
    {
        protected SpacePrimitive planet;
        //protected SpacePrimitive[] voidObjects;
        protected List<SpacePrimitive> satellites;

        public PlanetarySpace(Unit unit, Type type, GameObject planet) : base(unit, type, planet.transform.position)
        {

            this.planet = new SpacePrimitive(planet);
          //  this.planet.transform.parent = transform;
          //  this.planet.transform.position = new Vector3(0, 0, 0);
            satellites = new List<SpacePrimitive>();
        }
        public void MoveOrigins(Vector3 dPos)
        {
            foreach(var obj in satellites)
            {
                obj.UpdateOrigins(dPos);
            }
        }

        public void AddSatellite(SpacePrimitive obj)
        {
            satellites.Add(obj);
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
            foreach(var sat in satellites)
            {
                sat.Update();
            }
        }

        //in future want to render distant planets on cubemap
        public void RenderIfDistant(Vector3 playerpos)
        {
            var far = 1500f;
            if(type == Type.Object)
            {
                var pos = Get().transform.position;
                if(Vector3.Distance(pos, playerpos) > far)
                {

                }
            }
        }
    }


    public class PlanetSpace : PlanetarySpace
    {

        public PlanetSpace(GameObject planet) : base(Unit.AstronomicalUnits, Type.PlanetSpace, planet)
        {
            
        }

    }

    public class SatelliteSpace : PlanetarySpace
    {
        float radius;
        float theta;
        float speed;
        float offset;
        float t = 0;

        public SatelliteSpace(GameObject moon) : base(Unit.Meters, Type.SatelliteSpace, moon)
        {

        }

        public void SetOrbit(float radius, float theta, float speed, float offset)
        {
            this.radius = radius;
            this.theta = 90f - theta;
            this.speed = speed/10000;
            this.offset = offset;
        }

        // Update is called once per frame
        override
        public void Update()
        {
            t += speed;
            this.planet.Get().transform.position = GetNewPosition(); 

            foreach (var sat in satellites)
            {
                sat.Update();
            }
        }

        private Vector3 GetNewPosition()
        {
            //circle in xy
            var x = radius * Mathf.Cos(t);
            var y = radius * Mathf.Sin(t);
            //offset and rotate by theta
            var circle = new Vector3(x + offset, y, 0);
            circle = Quaternion.AngleAxis(theta, new Vector3(1, 0, 0)) * circle;

            return origin + circle;
        }

    }
}
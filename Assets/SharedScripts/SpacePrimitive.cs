using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Space
{
    //TODO make this abstract class
    public class SpacePrimitive 
    {
        public enum Unit { Meters, AstronomicalUnits, Parsecs };
        public enum Type { Object, ObjectSpace, SatelliteSpace, PlanetSpace, MinorSystemSpace, MajorSystemSpace };

        public Unit unit;
        public Type type;
        public Transform transform;
        private Hashtable coordinates = null; //only for not Object type
        private GameObject obj = null; //only for Object type
        protected Vector3 origin;

        public void UpdateOrigins(Vector3 dPos)
        {
            origin -= dPos;
            if (coordinates != null)
            {
                foreach (List<SpacePrimitive> obj in coordinates.Values)
                {
                    foreach(var sp in obj)
                        sp.UpdateOrigins(dPos);
                }
            }

        }
        public void GetSprite()
        {
           // var model = obj.GetComponent<LODGroup>().GetLODs()[2];
            //var rend = model.renderers[0];
            
            if (type != Type.Object)
            {
                throw new Exception("Cannot get sprite from Space type");
            }
           // LODGroup lod = 
        }
        public SpacePrimitive(Unit unit, Type type, Vector3 origin) 
        {
            //transform = Transform.Instantiate<Transform>(transform, origin, new Quaternion(0,0,0,1));
            this.unit = unit;
            this.type = type;
            this.origin = origin;
            //coordinates[x][y][z] in units
            coordinates = new Hashtable();
        }

        public SpacePrimitive(GameObject obj) 
        {
            unit = Unit.Meters;
            type = Type.Object;
            this.obj = obj;
            origin = obj.transform.position;
        }

        public List<GameObject> GetObjectsInRadius(Vector3 pos, float radius)
        {
            List<GameObject> list = new List<GameObject>();
            if(type == Type.Object)
            {
                if (Vector3.Distance(obj.transform.position, pos) <= radius)
                {
                    list.Add(obj);
                }
            }
            else
            {
                //look up pos in hashtable - return objects within radius
            }

            return list;
        }


        public List<SpacePrimitive> InSpace(int x, int y, int z)
        {
            if(coordinates.ContainsKey(x))
            {
                Hashtable xindex = (Hashtable) coordinates[x];
                if(xindex.ContainsKey(y))
                {
                    Hashtable yindex = (Hashtable) xindex[y];
                    if(yindex.ContainsKey(z))
                    {
                        Hashtable zindex = (Hashtable)yindex[z];
                        if(zindex.ContainsKey(z))
                        {
                            return (List<SpacePrimitive>) zindex[z];
                        }                   
                    }
                }
            }

            return null;
        }

        public void Set(GameObject obj)
        {
            if(type != Type.Object)
            {
                throw new Exception("Cannot add singular object to Space type");
            }
            this.obj = obj;
        }

        public GameObject Get()
        {
            if (type != Type.Object)
            {
                throw new Exception("Cannot get singular object from Space type");
            }
            return obj;
        }

        public void Add(SpacePrimitive space, int x, int y, int z)
        {
            if (type == Type.Object)
            {
                throw new Exception("Cannot add space to singular Object type");
            }


            if (!coordinates.ContainsKey(x))
            {
                coordinates[x] = new Hashtable();
            }
            Hashtable xindex = (Hashtable)coordinates[x];
            if (!xindex.ContainsKey(y))
            {
                //((Hashtable)coordinates[x])[y] = new Hashtable();
                //hopefully this is a ref and not copy
                xindex[y] = new Hashtable();
            }
            Hashtable yindex = (Hashtable)xindex[y];
            if (!yindex.ContainsKey(z))
            {
                yindex[z] = new Hashtable();
            }
            Hashtable zindex = (Hashtable)coordinates[z];
            if (!zindex.ContainsKey(z))
            {
                zindex[z] = new Hashtable();
            }
            ((List<SpacePrimitive>)zindex[z]).Add(space);                    
            
        }

        // Use this for initialization
        public virtual void Start()
        {

        }

        // Update is called once per frame
        public virtual void Update()
        {

        }
    }
}
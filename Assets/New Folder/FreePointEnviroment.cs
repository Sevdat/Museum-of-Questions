using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreePointEnviroment : MonoBehaviour
{
    public GameObject originalObject;
    public static GameObject dynamicClone;
    public static GameObject staticClone;
    public static GameObject cloneHierarchy;
    public World world = new();

    public class World {
        public int atomsInWorld,maxAtomsInWorld;
        public BodiesInWorld bodiesInWorld = new();
        public WorldGrid worldGrid = new();

        public struct Atom { 
            public int bodyStructureKey;
            public int atomicMeshKey;
            public Vector3 position;
            public Color color;
        }

        public class BodiesInWorld {
            public Dictionary<string,BodyData> allBodies;
            public struct BodyData {
                public string name; 
                public Dictionary<int,JointStructure> bodyStructure;
            }
            public struct JointStructure {
                public List<int> connectedToJointIndex;
                public Vector3 x,y,z; //only for integer 0 in dictionary
                public Dictionary<int,Atom> atomicMesh;
            }
        }
        public class WorldGrid {
            public Dictionary<Vector3Int,AtomicGrid> collisionDetection;
            public struct AtomicGrid {
                public int amountOfUniqueBodies;
                public Color avarageColor;
                public Dictionary<string, Dictionary<int,Atom> > atomsInGrid;
            }
        }
    }
    
    public static void createObject(string name,Vector3 vec){
        GameObject clone = Instantiate(
            staticClone, cloneHierarchy.transform
            );
        clone.name = $"{name}";
        clone.transform.position = vec;   
    }
    void Start()
    {
        cloneHierarchy = new GameObject(){
                name = "cloneHierarchy",
                isStatic = true
            };
        dynamicClone = originalObject;
        staticClone = originalObject;
        staticClone.isStatic = true;
        createObject("lol",new Vector3(10,10,10));
    }

    // Update is called once per frame
    void Update()
    {

                    
    }
}
        // public void addBody(string name,List<BodyData>jointList){
        //     if (!allBodies.ContainsKey(name) && vector3Count<vector3CountLimit){
        //         allBodies[name] = jointList;
        //     }
        // }
        // public Joint joint(
        //         string name,
        //         int indexInWorldList,
        //         int indexInJointList,int connectedTo,
        //         Vector3 position,
        //         Vector3 localX,Vector3 localY,Vector3 localZ,
        //         List<Vector3> meshShape
        //         ){
        //             return new Joint(){
        //                 name = name,
        //                 indexInWorldList = indexInWorldList,
        //                 indexInJointList = indexInJointList,
        //                 connectedTo = connectedTo,
        //                 position = position,
        //                 localX = localX,
        //                 localY = localY,
        //                 localZ = localZ,
        //                 meshShape = meshShape
        //             };
        // }

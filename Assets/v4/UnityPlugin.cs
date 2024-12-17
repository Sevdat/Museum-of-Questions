using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;
using System;
using System.Linq;
using UnityEngine.UI;

public class VertexVisualizer : MonoBehaviour
{
    public GameObject fbx;
    public static GameObject staticTerminal;
    SceneBuilder sceneBuilder;
    public class SceneBuilder:SourceCode{
        public Body body;
        public int decreasePoints = 5;
        List<GameObject> allChildrenInParent(GameObject topParent){
            List<GameObject> allChildren = new List<GameObject>();
            for (int i = 0; i < topParent.transform.childCount; i++){
                allChildren.Add(topParent.transform.GetChild(i).gameObject);
            }
            return allChildren;
        }
        class AssembleJoints{
            public int jointIndex;
            public List<BakedMeshIndex> bakedMeshIndex;
            public List<GameObject> futureConnections; // do comments

            public AssembleJoints(int jointIndex,List<GameObject> futureConnections){
                this.jointIndex = jointIndex;
                bakedMeshIndex = new List<BakedMeshIndex>();
                this.futureConnections = futureConnections;
            }

        }
        public void loadModelToBody(GameObject topParent){
            List<BakedMesh> bakedMeshes = new List<BakedMesh>();
            Dictionary<GameObject,AssembleJoints> dictionary = new Dictionary<GameObject,AssembleJoints>();
            List<GameObject> tree = new List<GameObject>(){topParent};
            int jointIndex = 0;
            for (int i = 0; i < tree.Count; i++){
                GameObject root = tree[i];
                List<GameObject> futureConnections = allChildrenInParent(root);
                tree.AddRange(futureConnections);
                SkinnedMeshRenderer skin = root.GetComponent<SkinnedMeshRenderer>();
                if (!dictionary.ContainsKey(root)) dictionary[root] = new AssembleJoints(jointIndex,futureConnections);
                if (skin) bakedMeshes.Add(new BakedMesh(skin));

                jointIndex++;
            }
            body = new Body(0);
            body.bakedMeshes = bakedMeshes;
            body.arraySizeManager(dictionary.Count);
            for (int i = 0; i<bakedMeshes.Count; i++){
                BakedMesh bakedMesh = bakedMeshes[i];
                for (int j = 0; j < bakedMesh.vertices.Length; j++){
                    dictionary[bakedMesh.getGameObject(j)].bakedMeshIndex.Add(new BakedMeshIndex(i,j));
                }
            }
            foreach (GameObject gameObject in dictionary.Keys){
                AssembleJoints assembleJoints = dictionary[gameObject];
                int indexInBody = assembleJoints.jointIndex;
                Joint joint = new Joint(body,indexInBody,gameObject);
                joint.localAxis.placeAxis(gameObject.transform.position);
                joint.localAxis.alignRotationTo(gameObject, out float angle, out Vector3 axis, out Vector4 quat);
                joint.localAxis.rotate(quat,gameObject.transform.position);
                int pointCloudSize = assembleJoints.bakedMeshIndex.Count;
                joint.pointCloud = new PointCloud(joint,pointCloudSize);
                for (int i = 0;i < pointCloudSize;i++){
                    if (i%decreasePoints == 0){
                    CollisionSphere collisionSphere = new CollisionSphere(joint,i,assembleJoints.bakedMeshIndex[i]);
                    collisionSphere.bakedMeshIndex = assembleJoints.bakedMeshIndex[i];
                    collisionSphere.bakedMeshIndex.updatePoint();
                    joint.pointCloud.collisionSpheres[i] = collisionSphere;
                    }
                }
                body.bodyStructure[indexInBody] = joint;
            }
            foreach (GameObject gameObject in dictionary.Keys){
                AssembleJoints assembleJoints = dictionary[gameObject];
                Joint joint = body.bodyStructure[assembleJoints.jointIndex];
                List<GameObject> list = assembleJoints.futureConnections;
                for (int i = 0; i<list.Count;i++){
                    int index = dictionary[list[i]].jointIndex;
                    Joint future = body.bodyStructure[index];
                    joint.connection.connectThisFutureToPast(future,out _, out _);
                }
            }
            body.optimizeBody();
            for (int i = 0; i<body.bodyStructure.Length;i++){
                body.bodyStructure[i]?.pointCloud.optimizeCollisionSpheres();
            }
        }
    }
    public class Terminal{
        GameObject terminal;
        PathScript path;
        SpecialFolderScript specialFolder;
        FolderScript folders;
        FileScript files;
        public Terminal(){
            terminal = Resources.Load("Terminal") as GameObject;
            terminal = Instantiate(terminal);
            path = getTerminalWindow(0).GetComponent<PathScript>();
            specialFolder = getTerminalWindow(1).GetComponent<SpecialFolderScript>();
            path.specialFolder = specialFolder;
            specialFolder.pathScript = path;

            folders = getTerminalWindow(2).GetComponent<FolderScript>();
            path.folders = folders;
            folders.pathScript = path;

            files = getTerminalWindow(3).GetComponent<FileScript>();
            path.files = files;
            files.pathScript = path;

            specialFolder.specialFolderPaths();
        }
        Transform getTerminalWindow(int index){
            return terminal.transform.GetChild(index).GetChild(0).GetChild(0);
        }
    }
    // Terminal terminal;
    // void Start(){
    //     terminal = new Terminal();
        
    //     // print(terminal.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<ItemButton>());
    //     // string appDataPath = System.Environment.GetEnvironmentVariable("LOCALAPPDATA");
        
    //     // string firefoxPath = System.IO.Path.Combine(appDataPath, @"Mozilla Firefox\firefox.exe");
    //     // // Path to the Firefox executable

    //     // // URL or file you want to open
    //     // string url = "https://www.google.com";
    //     // Process.Start(firefoxPath, url);
    // }

    // void Start() {
    //     sceneBuilder= new SceneBuilder();
    //     sceneBuilder.loadModelToBody(fbx);
    // }
    // void LateUpdate() {
    //     sceneBuilder.body.editor.trackBody();
    // }

}
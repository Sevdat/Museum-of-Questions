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
        class AssembleJoints{
            public int jointIndex;
            public List<BakedMeshIndex> bakedMeshIndex;
            public List<GameObject> futureConnections;
            public List<List<int>> triangles;

            public AssembleJoints(int jointIndex,List<GameObject> futureConnections){
                this.jointIndex = jointIndex;
                bakedMeshIndex = new List<BakedMeshIndex>();
                this.futureConnections = futureConnections;
                triangles = new List<List<int>>();
            }
        }

        List<GameObject> allChildrenInParent(GameObject topParent){
            List<GameObject> allChildren = new List<GameObject>();
            for (int i = 0; i < topParent.transform.childCount; i++){
                allChildren.Add(topParent.transform.GetChild(i).gameObject);
            }
            return allChildren;
        }
        void createHierarchy(List<GameObject> tree,Dictionary<GameObject,AssembleJoints> dictionary,List<BakedMesh> bakedMeshes){
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
            body.bakedMeshes = bakedMeshes;
            body.arraySizeManager(dictionary.Count);
        }
        void createMeshAccessForSpheres(List<BakedMesh> bakedMeshes,Dictionary<GameObject,AssembleJoints> dictionary){
            for (int i = 0; i<bakedMeshes.Count; i++){
                BakedMesh bakedMesh = bakedMeshes[i];
                for (int j = 0; j < bakedMesh.vertices.Length; j++){
                    dictionary[bakedMesh.getGameObject(j)].bakedMeshIndex.Add(new BakedMeshIndex(i,j));
                }
            }
        }
        void createTrianglesForPointClouds(List<BakedMesh> bakedMeshes,Dictionary<GameObject,AssembleJoints> dictionary){
            int count = 0;
            foreach (BakedMesh bakedMesh in bakedMeshes){
                int[] trianglesInMesh = bakedMesh.mesh.triangles;
                for (int i = 0; i < trianglesInMesh.Length; i += 3){
                    int vertexIndex1 = trianglesInMesh[i];
                    int vertexIndex2 = trianglesInMesh[i + 1];
                    int vertexIndex3 = trianglesInMesh[i + 2];
                    GameObject gameObject1 = bakedMesh.getGameObject(vertexIndex1);
                    GameObject gameObject2 = bakedMesh.getGameObject(vertexIndex2);
                    GameObject gameObject3 = bakedMesh.getGameObject(vertexIndex3);
                    if (gameObject1 == gameObject2 && gameObject3 == gameObject2){
                        if (bakedMeshes.Count>dictionary[gameObject1].triangles.Count){
                            for (int j = 0;j<bakedMeshes.Count;j++){
                                dictionary[gameObject1].triangles.Add(new List<int>());
                            }
                        }
                        List<int> trianglesForSphere = dictionary[gameObject1].triangles[count];
                        trianglesForSphere.Add(vertexIndex1);
                        trianglesForSphere.Add(vertexIndex2);
                        trianglesForSphere.Add(vertexIndex3); 
                    }
                }
                count++;
            }
        }
        void renewedKeysForTriangles(PointCloud pointCloud, List<List<int>> triangles){
            CollisionSphere[] collisionspheres = pointCloud.collisionSpheres;
            
            Dictionary<int, Dictionary<int,int>> dictionary = new Dictionary<int, Dictionary<int,int>>();
            int count = 0;
            foreach (CollisionSphere collisionSphere in collisionspheres){
                BakedMeshIndex bakedMesh = collisionSphere.bakedMeshIndex;
                if (!dictionary.ContainsKey(bakedMesh.indexInBakedMesh)) 
                    dictionary[bakedMesh.indexInBakedMesh] = new Dictionary<int,int>();
                
                if (!dictionary[bakedMesh.indexInBakedMesh].ContainsKey(bakedMesh.indexInVertex)){ 
                    dictionary[bakedMesh.indexInBakedMesh][bakedMesh.indexInVertex] = count;
                    count++;
                }
            }
            
            int size = 0;
            foreach (List<int> triangleList in triangles) size += triangleList.Count;
            pointCloud.triangles = new int[size];
            size = 0;
            for (int i = 0; i<triangles.Count;i++){
                List<int> triangleList = triangles[i];
                for (int j = 0; j<triangleList.Count;j++){
                    print(j+size);
                    pointCloud.triangles[j+size] = dictionary[i][triangleList[j]];
                }
                size += triangleList.Count;
            }
        }
        void createPointCloud(Dictionary<GameObject,AssembleJoints> dictionary){
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
                    CollisionSphere collisionSphere = new CollisionSphere(joint,i,assembleJoints.bakedMeshIndex[i]);
                    collisionSphere.bakedMeshIndex = assembleJoints.bakedMeshIndex[i];
                    collisionSphere.bakedMeshIndex.setWorldPoint();
                    joint.pointCloud.collisionSpheres[i] = collisionSphere;
                }
                renewedKeysForTriangles(joint.pointCloud,assembleJoints.triangles);
                body.bodyStructure[indexInBody] = joint;
            }
        }
        void createConnections(Dictionary<GameObject,AssembleJoints> dictionary){
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
        }
        public void loadModelToBody(GameObject topParent){
            body = new Body(0);
            List<BakedMesh> bakedMeshes = new List<BakedMesh>();
            Dictionary<GameObject,AssembleJoints> dictionary = new Dictionary<GameObject,AssembleJoints>();
            List<GameObject> tree = new List<GameObject>(){topParent};
            createHierarchy(tree,dictionary,bakedMeshes);
            createMeshAccessForSpheres(bakedMeshes,dictionary);
            createTrianglesForPointClouds(bakedMeshes,dictionary);
            createPointCloud(dictionary);
            createConnections(dictionary);
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

    void Start() {
        sceneBuilder= new SceneBuilder();
        sceneBuilder.loadModelToBody(fbx);
        sceneBuilder.body.sendToGPU.updateAccaleratedArrays();
        // for (int i = 0; i<sceneBuilder.body.sendToGPU.triangles.Length;i+=3){
        //     print($"{sceneBuilder.body.sendToGPU.vertices[sceneBuilder.body.sendToGPU.triangles[i]]} {sceneBuilder.body.sendToGPU.vertices[sceneBuilder.body.sendToGPU.triangles[i+1]]} {sceneBuilder.body.sendToGPU.vertices[sceneBuilder.body.sendToGPU.triangles[i+2]]}");
        // }
        // foreach(SourceCode.CollisionSphere collisionSphere in sceneBuilder.body.bodyStructure[5].pointCloud.collisionSpheres){
        //     print(collisionSphere.aroundAxis.sphere.origin);
        // }
        cube(sceneBuilder.body.sendToGPU.vertices,sceneBuilder.body.sendToGPU.triangles);
    }
    
    void cube(Vector3[] vertices,int[] triangles){
        GameObject cubeObject = new GameObject("Cube");
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.UploadMeshData(false);
        MeshFilter meshFilter = cubeObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        MeshRenderer meshRenderer = cubeObject.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Standard"));
        cubeObject.transform.position = new Vector3(0, 0, 0);
    }
    // void LateUpdate() {
    //     sceneBuilder.body.editor.trackBody();
    // }

}
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;
using System;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class VertexVisualizer : MonoBehaviour
{
    public GameObject fbx;
    public static GameObject staticTerminal;
    public SceneBuilder sceneBuilder;

    [Serializable]
    public class MeshData {
        public List<Vector3> vertices;
        public Color[] colors;
        
        public MeshData(){}
        public MeshData(List<Vector3> vertices,Color[] colors){
            this.vertices = vertices;
            this.colors = colors;
        }
    }

    public class BakedMesh {
        internal SkinnedMeshRenderer skinnedMeshRenderer;
        internal Mesh mesh;
        Transform transform;
        Transform[] bones;
        BoneWeight[] boneWeights; 
        public MeshData meshData;

        public BakedMesh(SkinnedMeshRenderer skinnedMeshRenderer){
            this.skinnedMeshRenderer=skinnedMeshRenderer;
            mesh = new Mesh();
            skinnedMeshRenderer.BakeMesh(mesh);
            meshData = new MeshData(mesh.vertices.ToList(),new Color[mesh.vertices.Length]);
            bones = skinnedMeshRenderer.bones;
            boneWeights = skinnedMeshRenderer.sharedMesh.boneWeights;
            transform = skinnedMeshRenderer.transform;
            calculateFinalColors();
        }
        public void bakeMesh(){
            skinnedMeshRenderer.BakeMesh(mesh);
            mesh.GetVertices(meshData.vertices);
            bones = skinnedMeshRenderer.bones;
            boneWeights = skinnedMeshRenderer.sharedMesh.boneWeights;
            transform = skinnedMeshRenderer.transform;
        }
        public Vector3 worldPosition(int index){
            return transform.TransformPoint(meshData.vertices[index]);
        }
        public GameObject getGameObject(int index){
            BoneWeight boneWeight = boneWeights[index];
            return bones[boneWeight.boneIndex0].gameObject;
        }
        public void calculateFinalColors(){
            Mesh mesh = skinnedMeshRenderer.sharedMesh;
            Material[] materials = skinnedMeshRenderer.sharedMaterials;
            foreach (Material material in materials){
                Color baseColor = material.HasProperty("_Color") ? material.color : Color.white;
                Texture2D mainTexture = null;
                if (material.HasProperty("_MainTex") && material.mainTexture != null){
                    mainTexture = (Texture2D) material.mainTexture;
                }
                Vector2[] uvs = mesh.uv;
                for (int i = 0; i < mesh.vertexCount; i++){
                    Color finalColor = baseColor;
                    if (mainTexture != null && uvs != null && uvs.Length > i){
                        Vector2 uv = uvs[i];
                        finalColor *= mainTexture.GetPixelBilinear(uv.x, uv.y);
                    }
                    meshData.colors[i] = finalColor;
                }
            }
        }
    }
    public class AxisData{
        public Transform transform;
        public int jointIndex;
        public Vector4 quat;
    
        public AxisData(){}
        public AxisData(Transform transform,int jointIndex){
            this.transform = transform;
            this.jointIndex = jointIndex;
            quat = getQuat();
        }
        public Vector3 getPosition(){
            return transform.position;
        }
        public Vector4 getQuat(){
            return new Vector4(
                transform.rotation.x,
                transform.rotation.y,
                transform.rotation.z,
                transform.rotation.w
            );
        }
    }
    
    public class SceneBuilder:SourceCode{
        public Body body;
        internal List<BakedMesh> bakedMeshes = new List<BakedMesh>();
        internal AxisData globalAxis;
        internal AxisData[] localAxis;
        GameObject fbx;

        public SceneBuilder(GameObject fbxGameObject){ 
            fbx = fbxGameObject;
            loadModelToBody(fbxGameObject);
            drawMesh();
        }

        class AssembleJoints {
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
            body.bodyStructure = new Joint[dictionary.Count];
        }
        void createMeshAccessForSpheres(List<BakedMesh> bakedMeshes,Dictionary<GameObject,AssembleJoints> dictionary){
            for (int i = 0; i<bakedMeshes.Count; i++){
                BakedMesh bakedMesh = bakedMeshes[i];
                for (int j = 0; j < bakedMesh.meshData.vertices.Count; j++){
                    dictionary[bakedMesh.getGameObject(j)].bakedMeshIndex.Add(new BakedMeshIndex(i,j));
                }
            }
        }
        void createTrianglesForPointClouds(List<BakedMesh> bakedMeshes,Dictionary<GameObject,AssembleJoints> dictionary){
            int count = 0;
            foreach (BakedMesh bakedMesh in bakedMeshes){
                int[] trianglesInMesh = bakedMesh.mesh.triangles;
                if (trianglesInMesh.Length>3){
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
                }
                count++;
            }
        }
        void renewedKeysForTriangles(PointCloud pointCloud, List<List<int>> triangles){
            Dictionary<int, Dictionary<int,int>> dictionary = new Dictionary<int, Dictionary<int,int>>();
            int count = 0;
            foreach (BakedMeshIndex bakedMesh in pointCloud.pointCloudData.bakedMeshIndex){
                if (!dictionary.ContainsKey(bakedMesh.indexInBakedMesh)) 
                    dictionary[bakedMesh.indexInBakedMesh] = new Dictionary<int,int>();
                
                if (!dictionary[bakedMesh.indexInBakedMesh].ContainsKey(bakedMesh.indexInVertex)){ 
                    dictionary[bakedMesh.indexInBakedMesh][bakedMesh.indexInVertex] = count;
                    count++;
                }
            }
            
            int size = 0;
            foreach (List<int> triangleList in triangles) size += triangleList.Count;
            pointCloud.pointCloudData.triangles = new int[size];
            size = 0;
            for (int i = 0; i<triangles.Count;i++){
                List<int> triangleList = triangles[i];
                for (int j = 0; j<triangleList.Count;j++){
                    pointCloud.pointCloudData.triangles[j+size] = dictionary[i][triangleList[j]];
                }
                size += triangleList.Count;
            }
        }
        void createPointCloud(Dictionary<GameObject,AssembleJoints> dictionary){
            foreach (GameObject gameObject in dictionary.Keys){
                AssembleJoints assembleJoints = dictionary[gameObject];
                int indexInBody = assembleJoints.jointIndex;
                Transform transform = gameObject.transform;
                Vector4 quat = new Vector4(
                    transform.rotation.x,
                    transform.rotation.y,
                    transform.rotation.z,
                    transform.rotation.w
                    );
                UnityAxis unityAxis = new UnityAxis(transform.position,quat);
                int pointCloudSize = assembleJoints.bakedMeshIndex.Count;
                Joint joint = new Joint(body,indexInBody,pointCloudSize,gameObject.name,unityAxis);
                joint.localAxis.placeAxis(gameObject.transform.position);
                joint.localAxis.rotate(quat,gameObject.transform.position);
                for (int i = 0;i < pointCloudSize;i++){
                    joint.pointCloud.pointCloudData.bakedMeshIndex[i] = assembleJoints.bakedMeshIndex[i];
                    Vector3 vec = joint.pointCloud.getPoint(i);
                    joint.pointCloud.pointCloudData.aroundAxis[i] = new AroundAxis(joint.localAxis, vec);
                    joint.pointCloud.pointCloudData.vertexes[i] = vec;

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
                    Joint futureJoint = body.bodyStructure[index];
                    joint.connection.connectThisFutureToPast(futureJoint);
                }
            }
        }
        public void loadModelToBody(GameObject topParent){
            Vector4 quat = new Vector4(
                topParent.transform.rotation.x,
                topParent.transform.rotation.y,
                topParent.transform.rotation.z,
                topParent.transform.rotation.w
                );
            UnityAxis globalAxis = new UnityAxis(topParent.transform.position,quat);
            body = new Body(0, globalAxis);
            body.globalAxis.placeAxis(globalAxis.origin);
            Dictionary<GameObject,AssembleJoints> dictionary = new Dictionary<GameObject,AssembleJoints>();
            List<GameObject> tree = new List<GameObject>(){topParent};
            createHierarchy(tree,dictionary,bakedMeshes);
            createMeshAccessForSpheres(bakedMeshes,dictionary);
            createTrianglesForPointClouds(bakedMeshes,dictionary);
            createPointCloud(dictionary);
            createConnections(dictionary);
            int size = dictionary.Count;
            localAxis = new AxisData[size];

            foreach (GameObject gameObject in dictionary.Keys){
                AssembleJoints assembleJoints = dictionary[gameObject];
                int index = assembleJoints.jointIndex;
                Transform transform = gameObject.transform;
                localAxis[index] = new AxisData(transform,index);
            }
        }
        public void updateMeshData(){
            for (int i = 0; i< body.bakedMeshes.Count;i++){
                body.bakedMeshes[i].bakeMesh();
            }
        }
        void drawMesh(){
            for (int i = 0;i<body.bodyStructure.Length;i++){
                Joint joint = body.bodyStructure[i];
                if (joint != null&& joint.pointCloud != null){
                    joint.pointCloud.renderPointCloud.drawMesh();
                }
            }
        }
        public void updateBodyPositions(){
            body.unityAxis.origin = fbx.transform.position;
            for (int i = 0; i<localAxis.Length;i++){
                int index = localAxis[i].jointIndex;
                Vector3 origin = localAxis[i].getPosition();
                Vector4 quat = localAxis[i].getQuat();
                UnityAxis unityAxis = body.bodyStructure[index].unityAxis;
                unityAxis.origin = origin;
                unityAxis.quat = quat;
            }
        }
        public void updateUnityData(){
            updateMeshData();
            updateBodyPositions();
        }
        public void updateBody(){
            body.updatePhysics();
        }
        public void drawBody(){
            drawMesh();
        }
    }

    void Start(){
        // terminal = new Terminal();
        
        // print(terminal.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<ItemButton>());
        // string appDataPath = System.Environment.GetEnvironmentVariable("LOCALAPPDATA");
        
        // string firefoxPath = System.IO.Path.Combine(appDataPath, @"Mozilla Firefox\firefox.exe");
        // // Path to the Firefox executable

        // // URL or file you want to open
        // string url = "https://www.google.com";
        // Process.Start(firefoxPath, url);
    }
    MultiThread multiThread;
    static ConcurrentQueue<MultiThread> resultQueue = new ConcurrentQueue<MultiThread>();

    public class MultiThread {
        public bool isRunning, updateBody;
        public SceneBuilder sceneBuilder;
        public MultiThread(SceneBuilder sceneBuilder){
            this.sceneBuilder = sceneBuilder;
            isRunning = true;
            updateBody = true;
            runBackgroundTask();
        }
        public async Task runBackgroundTask(){
            while (isRunning){
                if (updateBody){
                    await Task.Run(() =>{
                        sceneBuilder.updateBody();
                        sceneBuilder.body.editor.writer();
                    });
                    stopUpdate();
                }
                await Task.Delay(30); // Simulate delay
            }
        }
        public void wait(){
            isRunning = false;
        }
        public void StopTask(){
            isRunning = false;
        } 
        public void updateUnityData(){
            sceneBuilder.drawBody();
            sceneBuilder.updateUnityData();
            updateBody = true;
        }
        public void stopUpdate(){
            updateBody = false;
            resultQueue.Enqueue(this);
        }
    }

    void OnDestroy(){
        multiThread?.StopTask();
    }

    void OnApplicationQuit(){
        multiThread?.StopTask();
    }

    // void Start(){
    //     sceneBuilder = new SceneBuilder(fbx);
    //     multiThread = new MultiThread(sceneBuilder);
    // }
    // void LateUpdate() {
    //     DateTime old = DateTime.Now;
    //     while (resultQueue.TryDequeue(out MultiThread result)){
    //         result.updateUnityData();
    //     }
    //     print(DateTime.Now - old);
    // }

    // long memoryBefore;
    // void Start() {
    //     sceneBuilder = new SceneBuilder(fbx);
    //     print(sceneBuilder.body.bodyStructure[3].pointCloud.pointCloudData.triangles.Length);
    //     memoryBefore = Process.GetCurrentProcess().WorkingSet64;
    //     // strt();
    //     // sceneBuilder.body.bakedMeshes = null; 
    // }
    // void LateUpdate() {
    //     DateTime old = DateTime.Now;
    //     sceneBuilder.updateBodyPositions();
    //     sceneBuilder.updateUnityData();
    //     sceneBuilder.updateBody();
    //     sceneBuilder.drawBody();
    //     print(DateTime.Now - old);
    //     sceneBuilder.body.editor.writer();
    // }

    // public class PlayerData {
    //     public string[] playerName;
    //     public int playerLevel;
    //     public float playerHealth;
    //     public bool isAlive;
    // }
    // void strt(){
    //     // Create data
    //     PlayerData player = new PlayerData();
    //     player.playerName = new string[]{"Hero"};
    //     player.playerLevel = 10;
    //     player.playerHealth = 100.0f;
    //     player.isAlive = true;

    //     // Serialize to JSON
    //     string json = JsonUtility.ToJson(sceneBuilder.body,true);
    //     print("JSON: " + json);

    //     // Save JSON to file
    //     string filePath = $"Assets/v4/playerData.json";
    //     File.WriteAllText(filePath, json);
    //     print("JSON saved to: " + filePath);

    //     // Deserialize JSON
    //     PlayerData loadedPlayer = JsonUtility.FromJson<PlayerData>(json);
    //     print("Loaded Player Name: " + loadedPlayer.playerName);
    // }
    // void Start() {
    //     strt();
    // }

}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class SourceCode:MonoBehaviour {

    public class SphericalOctTree {
        public SphericalOctTree sphereOctTree;
        public int depth;
        public Vector3 origin;
        public List<Vector3> root;
        public List<Vector3> a,b,c,d;
        public List<Vector3> e,f,g,h;
    }
    
    public class World {
        public Body[] bodiesInWorld;
        public SphericalOctTree sphereOctTree;
    }

    public class AroundAxis{
        public float angleY,angleX,distance;
        
        public AroundAxis(){}
        public AroundAxis(Axis axis, Vector3 vec){
            angleY = 0; angleX = 0;
            distance = axis.length(vec-axis.origin);
            get(axis,vec);
        }
        public void get(Axis axis,Vector3 vec){
            getPointAroundAxis(axis,vec,out angleY, out angleX);
        }
        public void getPointAroundAxis(Axis axis, Vector3 point,out float angleY, out float angleX){
            float tempY = this.angleY;
            float tempX = this.angleX;
            bool over180 = (this.angleY > Mathf.PI)? true:false;
            axis.getPointAroundOrigin(point,out angleY, out angleX);
            if (!float.IsNaN(angleY) && !float.IsNaN(angleX)){
                if (angleY == 0f || angleY == Mathf.PI) angleX = this.angleX;
                if (over180) {
                    angleY = 2* Mathf.PI - angleY;
                    angleX = (Mathf.PI + angleX)%(2* Mathf.PI);
                    };
            } else {
                angleY = tempY;
                angleX = tempX;
            }
        } 
        
    }

    public class Axis {
        public Vector3 origin,x,y,z;
        public Vector4 quat;
        public Axis(){}
        public Axis(Vector3 origin){
            this.origin = origin;
            x = origin + new Vector3(1,0,0);
            y = origin + new Vector3(0,1,0);
            z = origin + new Vector3(0,0,1);
        }
        
        public void moveAxis(Vector3 add){
            origin += add;
            x += add;
            y += add;
            z += add;
        }
        public Vector3 placeAxis(Vector3 newOrigin){
            Vector3 newPosition = newOrigin-origin;
            moveAxis(newPosition);
            return newPosition;
        }
        public void scaleAxis(float newDistance){
            if (newDistance > 0f){
                x = origin + distanceFromOrigin(x,origin,newDistance);
                y = origin + distanceFromOrigin(y,origin,newDistance);
                z = origin + distanceFromOrigin(z,origin,newDistance);
            }
        }
        public float length(Vector3 vectorDirections){
            float x = vectorDirections.x;
            float y = vectorDirections.y;
            float z = vectorDirections.z;
            return Mathf.Sqrt(x * x + y * y + z * z);
        }
        public Vector3 normalize(Vector3 vec){    
            float radius = length(vec);
            if (radius > 0){
                vec.x /= radius;
                vec.y /= radius;
                vec.z /= radius;
            }
            return vec;
        }
        public float length(Vector4 vectorDirections){
            float x = vectorDirections.x;
            float y = vectorDirections.y;
            float z = vectorDirections.z;
            float w = vectorDirections.w;
            return Mathf.Sqrt(x * x + y * y + z * z + w * w);
        }
        public Vector4 normalize(Vector4 vec){    
            float radius = length(vec);
            if (radius > 0){
                vec.x /= radius;
                vec.y /= radius;
                vec.z /= radius;
                vec.w /= radius;
            }
            return vec;
        }
        internal Vector3 direction(Vector3 point,Vector3 origin){ 
            Vector3 v = point-origin;
            return v/length(v);
        }
        internal Vector3 distanceFromOrigin(Vector3 point,Vector3 origin, float distance){
            return direction(point,origin)*distance;
        }
        float dot(Vector3 vec1,Vector3 vec2){
            return vec1.x*vec2.x+vec1.y*vec2.y+vec1.z*vec2.z;
        }
        internal float angleBetweenLines(Vector3 dir1,Vector3 dir2){
            float dotProduct = dot(dir1,dir2);
            float lengthProduct = length(dir1)*length(dir2);
            float check = (Mathf.Abs(dotProduct)>lengthProduct)? MathF.Sign(dotProduct):dotProduct/lengthProduct;
            return Mathf.Acos(check);
        }
        internal Vector3 perpendicular(Vector3 lineOrigin, Vector3 lineDirection, Vector3 point){
            float amount = dot(point-lineOrigin,lineDirection);
            return lineOrigin+amount*lineDirection;
        }
        public Vector3 setPointAroundOrigin(float angleY,float angleX, float distance){
            Vector3 point = y;
            angleY = convertTo360(angleY);
            angleX = convertTo360(angleX);
            Vector4 rotY = angledAxis(angleY,x);
            Vector4 rotX = angledAxis(angleX,y);
            point = quatRotate(point,origin,rotY);
            point = quatRotate(point,origin,rotX);
            point = origin + distanceFromOrigin(point,origin,Mathf.Abs(distance));
            return point;
        }
        
        public void getPointAroundOrigin(Vector3 point, out float angleY,out float angleX){
            getAngle(point,origin,x,y,z,out angleY,out angleX);
        }
        internal float convertTo360(float angle){
            return (angle<0)? (2*Mathf.PI - (Mathf.Abs(angle) % (2*Mathf.PI))) : Mathf.Abs(angle) % (2*Mathf.PI);
        }

        void getAngle(Vector3 point,Vector3 origin, Vector3 x, Vector3 y, Vector3 z, out float yAngle,out float xAngle){
            Vector3 dirX = direction(x,origin);
            Vector3 dirY = direction(y,origin);
            Vector3 dirZ = direction(z,origin);
            Vector3 dirH = direction(point,origin);
            yAngle = angleBetweenLines(dirY,dirH);

            if (float.IsNaN(yAngle)) xAngle = float.NaN; else {   
                Vector3 perpendicularOrigin = perpendicular(origin,dirY,point);
                float checkLength = length(point -perpendicularOrigin);
                Vector3 dirPerpOrg = (checkLength !=0)?direction(point,perpendicularOrigin):normalize(point);
                float angleSide = angleBetweenLines(dirX,dirPerpOrg);          
                xAngle = (angleSide>Mathf.PI/2)? 
                    2*Mathf.PI-angleBetweenLines(dirZ,dirPerpOrg):
                    angleBetweenLines(dirZ,dirPerpOrg);
            }
        }

        public void rotate(Vector4 quat,Vector3 rotationOrigin){
            origin = quatRotate(origin,rotationOrigin,quat);
            x = quatRotate(x,rotationOrigin,quat);
            y = quatRotate(y,rotationOrigin,quat);
            z = quatRotate(z,rotationOrigin,quat);
        }

        public Vector4 quatMul(Vector4 q1, Vector4 q2) {
            float w = q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z;
            float x = q1.w * q2.x + q1.x * q2.w + q1.y * q2.z - q1.z * q2.y;
            float y = q1.w * q2.y - q1.x * q2.z + q1.y * q2.w + q1.z * q2.x;
            float z = q1.w * q2.z + q1.x * q2.y - q1.y * q2.x + q1.z * q2.w;
            return new Vector4(x, y, z, w);
        }
        public Vector4 angledAxis(float angle,Vector3 point){
            Vector3 normilized = normalize(point - origin); 
            float halfAngle = angle * 0.5f;
            float sinHalfAngle = Mathf.Sin(halfAngle);
            float w = Mathf.Cos(halfAngle);
            float x = normilized.x * sinHalfAngle;
            float y = normilized.y * sinHalfAngle;
            float z = normilized.z * sinHalfAngle;
            return new Vector4(x,y,z,w);
        }
        public void convertAngleAxis(Vector4 q, out float angle, out Vector3 axis){
            q = normalize(q);
            angle = 2.0f * Mathf.Acos(q.w);

            if (Mathf.Approximately(angle, 0.0f)){
                axis = Vector3.zero;
                return;
            }

            float sinHalfAngle = Mathf.Sin(angle / 2.0f);
            axis = new Vector3(
                q.x / sinHalfAngle,
                q.y / sinHalfAngle,
                q.z / sinHalfAngle
            );
            axis = normalize(axis);
        }
        public Vector4 inverseQuat(Vector4 q) {
            return new Vector4(-q.x,-q.y,-q.z,q.w);
        }
        public Vector3 quatRotate(Vector3 point, Vector3 origin, Vector4 angledAxis){
            Vector3 pointDirection = point - origin;     
            Vector4 rotatingVector = new Vector4(pointDirection.x, pointDirection.y, pointDirection.z,0);
            Vector4 rotatedQuaternion = quatMul(quatMul(angledAxis,rotatingVector), inverseQuat(angledAxis));
            return origin + new Vector3(rotatedQuaternion.x,rotatedQuaternion.y,rotatedQuaternion.z);
        }

        public void alignRotationTo(Axis engineAxis, out float angle, out Vector3 axis, out Vector4 quat){
            Vector4 from = getQuat(this);
            Vector4 to = getQuat(engineAxis);
            quat = quatMul(to, inverseQuat(from));
            convertAngleAxis(quat,out angle, out axis);
        }
        public void alignRotationTo(Vector4 to, out float angle, out Vector3 axis, out Vector4 quat){
            Vector4 from = getQuat(this);
            quat = quatMul(to, inverseQuat(from));
            convertAngleAxis(quat,out angle, out axis);
        }
        public Vector4 getQuat(){
            return matrixToQuat(rotationMatrix(this));
        }
        public Vector4 getQuat(Axis engineAxis){
            return matrixToQuat(rotationMatrix(engineAxis));
        }

        Matrix4x4 rotationMatrix(Axis axis){
            Vector3 right = direction(axis.x,axis.origin);
            Vector3 up = direction(axis.y,axis.origin);
            Vector3 forward = direction(axis.z,axis.origin);
            return new Matrix4x4(
                new Vector4(right.x, right.y, right.z, 0),
                new Vector4(up.x, up.y, up.z, 0),
                new Vector4(forward.x, forward.y, forward.z, 0),
                new Vector4(0, 0, 0, 1)
            );
        }
        
        public Vector4 matrixToQuat(Matrix4x4 m){
            float trace = m.m00 + m.m11 + m.m22;
            float w, x, y, z;

            if (trace > 0) {
                float s = Mathf.Sqrt(1 + trace) * 2;
                w = 0.25f * s;
                x = (m.m21 - m.m12) / s;
                y = (m.m02 - m.m20) / s;
                z = (m.m10 - m.m01) / s;
            }
            else if ((m.m00 > m.m11) && (m.m00 > m.m22)) {
                float s = Mathf.Sqrt(1 + m.m00 - m.m11 - m.m22) * 2;
                w = (m.m21 - m.m12) / s;
                x = 0.25f * s;
                y = (m.m01 + m.m10) / s;
                z = (m.m02 + m.m20) / s;
            }
            else if (m.m11 > m.m22) {
                float s = Mathf.Sqrt(1 + m.m11 - m.m00 - m.m22) * 2;
                w = (m.m02 - m.m20) / s;
                x = (m.m01 + m.m10) / s;
                y = 0.25f * s;
                z = (m.m12 + m.m21) / s;
            }
            else {
                float s = Mathf.Sqrt(1 + m.m22 - m.m00 - m.m11) * 2;
                w = (m.m10 - m.m01) / s;
                x = (m.m02 + m.m20) / s;
                y = (m.m12 + m.m21) / s;
                z = 0.25f * s;
            }
            return new Vector4(x, y, z, w);
        }

        public (float yaw, float pitch, float roll) quatToEular(Vector4 quat){
            float x = quat.x;
            float y = quat.y;
            float z = quat.z;
            float w = quat.w;
            return (
                Mathf.Atan2(2 * (x * y + w * z), 1 - 2 * (y * y + z * z)), 
                Mathf.Asin(2 * (w * y - x * z)), 
                Mathf.Atan2(2 * (x * w + y * z), 1 - 2 * (x * x + y * y))
                );
        }
    }

    public class Body {
        public World world;
        public int worldKey;
        public Axis globalAxis;
        public Joint[] bodyStructure;
        public Editor editor;
        public List<VertexVisualizer.BakedMesh> bakedMeshes;
        public string amountOfDigits; 
        public int asyncDelay, time;
        public UnityAxis unityAxis;
        StringBuilder sb = new StringBuilder();

        public Body(){}
        public Body(int worldKey){
            init();
            this.worldKey = worldKey;
        }
        public Body(int worldKey, UnityAxis unityAxis){
            init();
            this.worldKey = worldKey;
            this.unityAxis = unityAxis;
        }
        void init(){
            globalAxis = new Axis(new Vector3(0,0,0));
            bodyStructure = null;
            editor = new Editor(this);
            editor.initilizeBody();
            amountOfDigits = "0.000000";
            asyncDelay = 20;
            time = 0;
        }

        public void newCountStart(int timerStart){
            if (timerStart<1) timerStart = 1;
            asyncDelay = timerStart;    
        }
        public void newAccuracy(int amount){
            string newString;
            if (amount>0){
                newString = "0.0";
                for (int i = 1; i < amount; i++){
                    newString += "0";
                }
                amountOfDigits = newString;
            }
        }

        public string accuracyAmount(float num){
            return num.ToString(amountOfDigits);
        }
        public void saveBodyStructure(StreamWriter writer, bool radianOrAngle,int chunkSize){
            string stringPath = $"{bodyDepth},{worldKey}";
            saveTimeStamp(writer,stringPath);
            saveJointsInBody(writer,radianOrAngle,stringPath,chunkSize);
        }
        public void saveTimeStamp(StreamWriter writer,string stringPath){
            string str = $"{stringPath},{timeStamp},1,1,{time}{Environment.NewLine}";
            writer.Write(str);
            time++;
        }
        void saveJointsInBody(StreamWriter writer, bool radianOrAngle, string stringPath, int chunkSize){
            float convert = radianOrAngle? 180f/Mathf.PI:1;
            writer.Write($"{stringPath},{jointData},{bodyStructure.Length},11");
            if (bodyStructure.Length>0) {
                writer.Write(",");
                int offset = 0;
                while (offset < bodyStructure.Length){
                    int elementsToWrite = Math.Min(bodyStructure.Length-offset, chunkSize);
                    for (int i = 0; i < elementsToWrite; i+=1){
                        Joint joint = bodyStructure[i];
                        if (joint != null) {
                            Vector4 quat = joint.localAxis.getQuat();
                            sb.Append($"{i},{accuracyAmount(joint.localAxis.origin.x)},{accuracyAmount(joint.localAxis.origin.y)},{accuracyAmount(joint.localAxis.origin.z)},{accuracyAmount(joint.fromGlobalAxis.distance)},{accuracyAmount(joint.fromGlobalAxis.angleY*convert)},{accuracyAmount(joint.fromGlobalAxis.angleX*convert)},{accuracyAmount(quat.x)},{accuracyAmount(quat.y)},{accuracyAmount(quat.z)},{accuracyAmount(quat.w)},");
                        }
                    }
                    offset += elementsToWrite;
                    if (offset == bodyStructure.Length) {
                        sb.Remove(sb.Length - 1, 1);
                        sb.Append(Environment.NewLine);
                    };
                    writer.WriteLine(sb.ToString());
                    sb.Clear();
                }
            } else writer.WriteLine("");
        }
        public void saveBodyPosition(StreamWriter writer, bool radianOrDegree){
            Vector3 globalOrigin = globalAxis.origin;
            Vector4 quat = globalAxis.getQuat();
            string stringPath = $"{bodyDepth},{worldKey}";
            writer.Write(
                $"{stringPath},{globalData},1,8,{accuracyAmount(globalOrigin.x)},{accuracyAmount(globalOrigin.y)},{accuracyAmount(globalOrigin.z)}{accuracyAmount(quat.x)},{accuracyAmount(quat.y)},{accuracyAmount(quat.z)},{accuracyAmount(quat.w)},{radianOrDegree}{Environment.NewLine}"
            );
        }

        public void updatePhysics(){
            if (unityAxis != null){          
                globalAxis.placeAxis(unityAxis.origin);
            }
            for(int i = 0;i< bodyStructure.Length; i++){
                bodyStructure[i]?.updatePhysics();
            }     
        }
    }

    public class Connection {
        public bool active = true, used = false;
        public int indexInBody;
        public Joint current;
        public List<Joint> past;
        public List<Joint> future;

        public Connection(){}
        public Connection(Joint joint, int indexInBody){
            current = joint;
            this.indexInBody = indexInBody;
            past = new List<Joint>();
            future = new List<Joint>();
        }

        internal void disconnectFuture(int index){
            future[index].connection.past.RemoveAll(e => e == current);
        } 
        public void disconnectAllFuture(){
            int size = future.Count;
            for (int i =0; i<size;i++){
                disconnectFuture(indexInBody);
            }
            future = new List<Joint>();
        }
        internal void disconnectPast(int index){
            past[index].connection.future.RemoveAll(e => e == current);
        }
        public void disconnectAllPast(){
            int size = past.Count;
            for (int i =0; i<size;i++){
                disconnectPast(indexInBody);
            }
            past = new List<Joint>();
        }
        public void connectThisPastToFuture(Joint joint){
            List<Joint> connectTo = joint.connection.future;
            past.Add(joint);
            connectTo.Add(current);
        }
        public void connectThisFutureToPast(Joint joint){
            List<Joint> connectTo = joint.connection.past;
            future.Add(joint);
            connectTo.Add(current);
        }

        public void savePastConnections(StreamWriter writer, string stringPath){
            writer.Write($"{stringPath},{pastConnectionsInBody},{past.Count},1");
            if (past.Count>0) writer.Write(",");
            for (int i = 0; i<past.Count;i++){
                string str = (i+1 != past.Count)? 
                    $"{past[i].connection.indexInBody},":
                    $"{past[i].connection.indexInBody}";
                writer.Write(str);
            }
        }
        public void saveFutureConnections(StreamWriter writer, string stringPath){
            writer.Write($"{stringPath},{futureConnectionsInBody},{future.Count},1");
            if (future.Count>0) writer.Write(",");
            for (int i = 0; i<future.Count;i++){
                string str = (i+1 != future.Count)? 
                    $"{future[i].connection.indexInBody},":
                    $"{future[i].connection.indexInBody}";
                writer.Write(str);
            }
        }
    }

    public class UnityAxis{
        public Joint joint;
        public Vector3 origin;
        public Vector4 quat;

        public UnityAxis(){}
        public UnityAxis(Vector3 origin, Vector4 quat){
            this.origin = origin;
            this.quat = quat;
        }

        public void updateJoint(){
            Vector3 move = joint.unityAxis.origin - joint.localAxis.origin;
            joint.moveJoint(move);
            joint.localAxis.alignRotationTo(joint.unityAxis.quat, out _, out _, out Vector4 quat);
            joint.rotateJoint(quat);
            joint.localAxis.quat = quat;
            Vector3 jointOrigin = joint.localAxis.origin;
            Vector3 globalOrigin = joint.body.globalAxis.origin;
            joint.fromGlobalAxis.distance = joint.body.globalAxis.length(jointOrigin-globalOrigin);
            joint.fromGlobalAxis.get(joint.localAxis,jointOrigin);
        }
    }

    public class Joint {
        public Body body;
        public Axis localAxis;
        public AroundAxis fromGlobalAxis;
        public Connection connection;
        public PointCloud pointCloud;
        public UnityAxis unityAxis;
        public string jointNameString;
        public Joint(){}
        public Joint(Body body, int indexInBody){
            init(body, indexInBody, 0, "");
        }
        public Joint(Body body, int indexInBody,int pointCloudSize, string jointNameString, UnityAxis unityAxis){
            init(body, indexInBody,pointCloudSize,jointNameString);
            unityAxis.joint = this;
            this.unityAxis = unityAxis;
        }
        void init(Body body, int indexInBody,int pointCloudSize, string jointNameString){
            this.body = body;
            this.jointNameString = jointNameString;
            pointCloud = new PointCloud(this,pointCloudSize);
            localAxis = new Axis(new Vector3(0,0,0));
            connection = new Connection(this,indexInBody);
            fromGlobalAxis = new AroundAxis(body.globalAxis,localAxis.origin);
        }

        public void saveJointPosition(StreamWriter writer){
            string stringPath = $"{jointDepth},{body.worldKey},{connection.indexInBody}";
            writer.Write(
                $"{stringPath},{jointName},{jointNameString.Length},1,{jointNameString}{Environment.NewLine}"
            );
            connection.savePastConnections(writer,stringPath);
            writer.WriteLine("");
            connection.saveFutureConnections(writer,stringPath);
            writer.WriteLine("");
        }

        public void deleteJoint(){
            connection.disconnectAllFuture();
            connection.disconnectAllPast();
            body.bodyStructure[connection.indexInBody] = null;
        }
        public void getDistanceFromGlobalOrigin(float newDistance){
            Vector3 globalOrigin = body.globalAxis.origin;
            Vector3 localOrigin = localAxis.origin;
            float length = localAxis.length(localOrigin-globalOrigin);
            Vector3 direction = (length>0)? localAxis.direction(localOrigin,globalOrigin)*(newDistance-length): localAxis.direction(localAxis.y,globalOrigin)*(newDistance-length);
            moveJoint(direction);
        }

        public void moveJoint(Vector3 add){
            localAxis.moveAxis(add);
            if (pointCloud != null) pointCloud.moveSpheres(add);
        }
        public void rotateJoint(Vector4 quat){
            localAxis.rotate(quat,localAxis.origin);
            if (pointCloud != null) pointCloud.rotateAllSpheres(quat,localAxis.origin);
        }
        public void updatePhysics(){ 
            unityAxis?.updateJoint();
            if (pointCloud != null) pointCloud.updatePhysics();  
        }
    }

    public class RenderPointCloud{
        public PointCloud pointCloud;
        GameObject processedFBX;
        Mesh mesh;
        MeshFilter meshFilter;
        MeshRenderer meshRenderer;
        public RenderPointCloud(){}
        public RenderPointCloud(PointCloud pointCloud, string name){
            this.pointCloud = pointCloud;
            processedFBX = new GameObject(name);
            mesh = new Mesh();
            meshFilter = processedFBX.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;
            meshRenderer = processedFBX.AddComponent<MeshRenderer>();
            meshRenderer.material = Resources.Load<Material>("unlitMaterial");
        }
        public void drawMesh(){
            mesh.vertices = pointCloud.pointCloudData.vertexes;
            mesh.triangles = pointCloud.pointCloudData.triangles;
            mesh.colors = pointCloud.pointCloudData.colors;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.UploadMeshData(false);
        }
    }
    public struct BakedMeshIndex{
        public int indexInBakedMesh;
        public int indexInVertex;
        public BakedMeshIndex(int indexInBakedMesh,int indexInVertex){
            this.indexInBakedMesh = indexInBakedMesh;
            this.indexInVertex = indexInVertex;
        }
    }

    [Serializable]
    public class SaveBody {
        Axis globalAxis;
        SaveJoint[] saveJoints;
    }
    [Serializable]
    public class SaveConnection{
        int current;
        int[] past;
        int[] future;
    }
    [Serializable]
    public class SaveJoint{
        AroundAxis jointAroundGlobalAxis;
        Axis localAxis;
        SaveConnection saveConnection;
        SavePointCloudData pointCloudData;
    }

    public class SavePointCloudData{
        public AroundAxis[] aroundAxis;
        public BakedMeshIndex[] bakedMeshIndex;
        public Vector3[] vertexes;
        public Color[] colors;
        public int[] triangles;

        public SavePointCloudData(){}
        public SavePointCloudData(int size){
            aroundAxis = new AroundAxis[size];
            bakedMeshIndex = new BakedMeshIndex[size];
            vertexes = new Vector3[size];
            colors = new Color[size];
            triangles = new int[0];
        }
    }
    public class PointCloud {
        public Joint joint;
        public SavePointCloudData pointCloudData;
        public RenderPointCloud renderPointCloud;

        StringBuilder sb = new StringBuilder();
        public PointCloud(){}
        public PointCloud(Joint joint, int size){
            this.joint = joint;
            pointCloudData = new SavePointCloudData(size);
            renderPointCloud = new RenderPointCloud(this,joint.jointNameString);
        }

        public void savePointCloud(StreamWriter writer,int chunkSize){
            string stringPath = $"{jointDepth},{joint.body.worldKey},{joint.connection.indexInBody}";
            savePointCloudPositions(writer,stringPath,chunkSize);
            saveTriangles(writer,stringPath, chunkSize);
        }
        void savePointCloudPositions(StreamWriter writer, string stringPath, int chunkSize){
            writer.Write($"{stringPath},{pointCloudSphereDatas},{pointCloudData.vertexes.Length},11");
            if (pointCloudData.vertexes.Length>0) {
                writer.Write(","); 
                int offset = 0;
                while (offset < pointCloudData.vertexes.Length){
                    int elementsToWrite = Math.Min(pointCloudData.vertexes.Length-offset, chunkSize);
                    for (int i = 0; i < elementsToWrite; i+=1){
                        AroundAxis aroundAxis = pointCloudData.aroundAxis[i];
                        if (aroundAxis != null) {
                            Vector3 vec = pointCloudData.vertexes[i];
                            Color col = pointCloudData.colors[i];
                            sb.Append($"{i},{vec.x},{vec.y},{vec.z},{aroundAxis.distance},{aroundAxis.angleY},{aroundAxis.angleX},{col.r},{col.g},{col.b},{col.a},");
                        }
                    }
                    offset += elementsToWrite;
                    if (offset == pointCloudData.vertexes.Length) {
                        sb.Remove(sb.Length - 1, 1);
                        sb.Append(Environment.NewLine);
                        };
                    writer.Write(sb.ToString());
                    sb.Clear();
                }        
            } else writer.WriteLine("");
        }
        void saveTriangles(StreamWriter writer, string stringPath, int chunkSize){
            if (pointCloudData.triangles.Length>3){
                writer.Write($"{stringPath},{trianglesInPointCloud},{pointCloudData.triangles.Length/3},3,");
                int offset = 0;
                while (offset < pointCloudData.triangles.Length){
                    int elementsToWrite = Math.Min(pointCloudData.triangles.Length-offset, chunkSize);
                    elementsToWrite = elementsToWrite / 3 * 3;
                    for (int i = 0; i < elementsToWrite; i+=3){
                        int index1 = pointCloudData.triangles[offset + i];
                        int index2 = pointCloudData.triangles[offset + i + 1];
                        int index3 = pointCloudData.triangles[offset + i + 2];
                        bool check = index1<pointCloudData.triangles.Length && index2<pointCloudData.triangles.Length && index3<pointCloudData.triangles.Length;
                        if (check) {
                            sb.Append($"{index1},{index2},{index3},");
                        };
                    }
                    offset += elementsToWrite;
                    if (offset == pointCloudData.triangles.Length) {
                        sb.Remove(sb.Length - 1, 1);
                        sb.Append(Environment.NewLine);
                    };
                    writer.Write(sb.ToString());
                    sb.Clear();
                }
            } else {
                writer.Write($"{stringPath},{trianglesInPointCloud},{pointCloudData.triangles.Length},3{Environment.NewLine}");
            }
        }
        public void saveBakedMeshes(StreamWriter writer){
            string stringPath = $"{jointDepth},{joint.body.worldKey},{joint.connection.indexInBody}";
            writer.Write($"{stringPath},{bakedMeshIndex},2,{pointCloudData.vertexes.Length/2},");
            if (pointCloudData.vertexes != null){
                for (int i = 0;i<pointCloudData.vertexes.Length;i++){
                    AroundAxis aroundAxis = pointCloudData.aroundAxis[i];
                    if (aroundAxis != null) {
                        BakedMeshIndex bakedMeshIndex = pointCloudData.bakedMeshIndex[i];
                        string str = (i+1 != pointCloudData.vertexes.Length)? 
                            $"{bakedMeshIndex.indexInBakedMesh},{bakedMeshIndex.indexInVertex},":
                            $"{bakedMeshIndex.indexInBakedMesh},{bakedMeshIndex.indexInVertex}{Environment.NewLine}";
                        writer.Write(str);
                    }
                }
            }
        }
        public void rotateAllSpheres(Vector4 quat, Vector3 rotationOrigin){
            for (int i = 0; i<pointCloudData.vertexes.Length; i++){
                AroundAxis aroundAxis = pointCloudData.aroundAxis[i];
                if (aroundAxis != null) pointCloudData.vertexes[i] = joint.localAxis.quatRotate(pointCloudData.vertexes[i],rotationOrigin,quat);
            }
        }
        public void moveSpheres(Vector3 add){
            for (int i = 0; i<pointCloudData.vertexes.Length; i++){
                AroundAxis aroundAxis = pointCloudData.aroundAxis[i];
                if (aroundAxis != null) pointCloudData.vertexes[i] += add;
            }
        }
        public void updatePhysics(){
            for (int i = 0;i<pointCloudData.vertexes.Length; i++){
                setPoint(i);
            }
        }
        public void setPoint(int index){
            AroundAxis aroundAxis = pointCloudData.aroundAxis[index]; 
            if (aroundAxis != null){
                VertexVisualizer.BakedMesh bakedMesh = joint.body.bakedMeshes[pointCloudData.bakedMeshIndex[index].indexInBakedMesh];
                Axis axis = joint.localAxis;
                Color col = bakedMesh.colors[pointCloudData.bakedMeshIndex[index].indexInVertex];
                Vector3 point = joint.body.globalAxis.origin+bakedMesh.vertices[pointCloudData.bakedMeshIndex[index].indexInVertex];
                pointCloudData.vertexes[index] = point;
                pointCloudData.colors[index] = col;
                aroundAxis.distance = joint.localAxis.length(point - axis.origin);
                aroundAxis.getPointAroundAxis(joint.localAxis,point, out float angleY,out float angleX);
                aroundAxis.angleY = angleY;
                aroundAxis.angleX = angleX;
            }
        }
        public Vector3 getPoint(int index){
            VertexVisualizer.BakedMesh bakedMesh = joint.body.bakedMeshes[pointCloudData.bakedMeshIndex[index].indexInBakedMesh];
            return joint.body.globalAxis.origin+bakedMesh.vertices[pointCloudData.bakedMeshIndex[index].indexInVertex];
        }
        
        private static Vector3 ComputeCentroid(List<Vector3> vertices){
            Vector3 centroid = Vector3.zero;
            foreach (Vector3 vertex in vertices){
                centroid += vertex;
            }
            centroid /= vertices.Count;
            return centroid;
        }

        private static List<Vector3> CenterVertices(List<Vector3> vertices, Vector3 centroid){
            List<Vector3> centeredVertices = new List<Vector3>();
            foreach (Vector3 vertex in vertices){
                centeredVertices.Add(vertex - centroid);
            }
            return centeredVertices;
        }
        private static float ComputeScale(List<Vector3> centeredVertices){
            float maxDistance = 0f;
            foreach (Vector3 vertex in centeredVertices){
                float distance = vertex.magnitude; // Distance from origin (centroid)
                if (distance > maxDistance){
                    maxDistance = distance;
                }
            }
            return maxDistance;
        }

        // Scale the vertices to fit within a unit sphere
        private static List<Vector3> ScaleVertices(List<Vector3> centeredVertices, float scale){
            List<Vector3> scaledVertices = new List<Vector3>();
            foreach (Vector3 vertex in centeredVertices){
                scaledVertices.Add(vertex / scale);
            }
            return scaledVertices;
        }
    }

    const string bodyDepth = "2",
        jointData = "JointData",
        globalData = "GlobalData",
        timeStamp = "TimeStamp";

    const string jointDepth = "3",
        jointName = "JointName",
        pastConnectionsInBody = "PastConnectionsInBody",
        futureConnectionsInBody = "FutureConnectionsInBody",
        bakedMeshIndex = "BakedMeshIndex",
        pointCloudSphereDatas = "PointCloudSphereDatas",
        trianglesInPointCloud = "TrianglesInPointCloud";

public class Editor {
        internal bool radianOrDegree = false;
        internal bool initilize;
        internal Body body;
        internal Dictionary<int,int> newJointKeys = new Dictionary<int,int>();
        internal Dictionary<int,int> newSphereKeys = new Dictionary<int,int>();
        internal Dictionary<int,List<int>> deleted = new Dictionary<int, List<int>>();
        internal int count = 0;
        internal string pathToFolder;
        internal int chunkSize = 10000;

        public Editor(){}
        public Editor(Body body){
            this.body = body;
            pathToFolder = $"Assets/v4/{body.worldKey}";
            if (!Directory.Exists(pathToFolder)) {
                Directory.CreateDirectory(pathToFolder);
                }
            if (!Directory.Exists($"{pathToFolder}/VerticesAndTriangles")) {
                Directory.CreateDirectory($"{pathToFolder}/VerticesAndTriangles");
                }
            if (!Directory.Exists($"{pathToFolder}/Datasets")) {
                Directory.CreateDirectory($"{pathToFolder}/Datasets");
                }
        }
        
        void read(int count){
            using(StreamReader readtext = new StreamReader($"{pathToFolder}/Datasets/{count}.txt")){
                string readText;
                while ((readText = readtext.ReadLine()) != null){
                    string[] splitStr = readText.Split(new[] {':'},2);
                    if (splitStr.Length == 2){
                        removeEmpty(splitStr[0].Split("_"), out List<string> instruction);
                    }
                } 
            }
        }
        void write(StreamWriter writetext){
            body.updatePhysics();
            body.saveBodyStructure(writetext,radianOrDegree,chunkSize);
            int size = body.bodyStructure.Length;
            for (int i = 0; i<size; i++){
                Joint joint = body.bodyStructure[i];
                if (joint != null){
                    joint.saveJointPosition(writetext);
                    joint.pointCloud.savePointCloud(writetext,chunkSize);
                    writetext.WriteLine("");
                } 
            }
            body.saveBodyPosition(writetext,radianOrDegree);
        }
        
        internal void writer(){
            using(StreamWriter writeText = new StreamWriter($"{pathToFolder}/Datasets/{count}.txt")) {
                write(writeText);                 
            }
        }
        internal void writerBinary() {
            using (FileStream fs = new FileStream($"{pathToFolder}/VerticesAndTriangles/{count}.txt", FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 65536))
            using (BinaryWriter writer = new BinaryWriter(fs)) {
                foreach (VertexVisualizer.BakedMesh bakedMesh in body.bakedMeshes){
                    writer.Write(bakedMesh.vertices.Count);
                    foreach (Vector3 vec in bakedMesh.vertices) {
                        writer.Write(vec.x);
                        writer.Write(vec.y);
                        writer.Write(vec.z);
                    }
                }
            }
        }
        public void initilizeBody(){
            initilize = true;
            read(0);
            initilize = false;
        }
        internal void removeEmpty(string[] strArray, out List<string> list){
            list = new List<string>();
            int arraySize = strArray.Length;
            for (int i = 0; i < arraySize; i++){
                string str = strArray[i];
                if (str != "") list.Add(str);
            }
        }
        void strt(){
            // Create data
            PlayerData player = new PlayerData();
            player.playerName = "Hero";
            player.playerLevel = 10;
            player.playerHealth = 100.0f;
            player.isAlive = true;

            // Serialize to JSON
            string json = JsonUtility.ToJson(player);
            Debug.Log("JSON: " + json);

            // Save JSON to file
            string filePath = Path.Combine(Application.persistentDataPath, "playerData.json");
            File.WriteAllText(filePath, json);
            Debug.Log("JSON saved to: " + filePath);

            // Deserialize JSON
            PlayerData loadedPlayer = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("Loaded Player Name: " + loadedPlayer.playerName);
        }
    }
    public class PlayerData
{
    public string playerName;
    public int playerLevel;
    public float playerHealth;
    public bool isAlive;
}

}
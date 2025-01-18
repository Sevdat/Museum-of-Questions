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
        public List<CollisionSphere> root;
        public List<CollisionSphere> a,b,c,d;
        public List<CollisionSphere> e,f,g,h;
    }

    public class World {
        public Body[] bodiesInWorld;
        public SphericalOctTree sphereOctTree;
        public KeyGenerator keyGenerator;
    }
    public class AroundAxis{
        public Sphere sphere;
        public Axis axis;
        public float angleY,angleX,distance;
        
        public AroundAxis(){}
        public AroundAxis(Axis axis, Sphere sphere){
            this.sphere = sphere;
            this.axis = axis;
            angleY = 0; angleX = 0;
            distance = axis.length(sphere.origin-axis.origin);
            sphere.setOrigin(axis.origin + new Vector3(0,distance,0));
        }
        public void scale(float newDistance){
            if (newDistance>0){
                distance = Mathf.Abs(newDistance);
                Vector3 add = (sphere.origin != axis.origin) ? axis.distanceFromOrigin(sphere.origin,axis.origin,distance) : new Vector3(0,0,0);
                sphere.origin = axis.origin + add;
            } else {
                distance = 0;
                sphere.origin = axis.origin;
            }
        }

        public void get(){
            getPointAroundAxis(sphere.origin,out angleY, out angleX);
        }
        
        public void getPointAroundAxis(Vector3 point,out float angleY, out float angleX){
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

        void set(float angleY,float angleX){
            sphere.origin = axis.setPointAroundOrigin(angleY,angleX,distance);
            this.angleY = axis.convertTo360(angleY);
            this.angleX = axis.convertTo360(angleX);
        }
        public void resetOrigin(){
            set(angleY,angleX);
        }
        public void getInRadians(out float angleY,out float angleX){
            get();
            angleY = this.angleY;
            angleX = this.angleX;
        }
        public void setInRadians(float angleY,float angleX){
            set(angleY, angleX);
        }
        public void getInDegrees(out float angleY,out float angleX){
            float radianToDegree = 180/Mathf.PI;
            get();
            angleY = this.angleY*radianToDegree;
            angleX = this.angleX*radianToDegree;
        }
        public void setInDegrees(float angleY,float angleX){
            float degreeToRadian = Mathf.PI/180;
            angleY *= degreeToRadian;
            angleX *= degreeToRadian;
            set(angleY, angleX);
        }

        public Vector4 quat(float radian){
            return axis.angledAxis(radian,sphere.origin);
        }
        public Vector4 quatInDegrees(float angle){
            float degreeToRadian = Mathf.PI/180;
            return axis.angledAxis(angle*degreeToRadian,sphere.origin);
        }
         
        public void rotationY(){
            float abs = Mathf.Abs(angleY) % (2*Mathf.PI);
            angleY = axis.convertTo360(abs);
            set(angleY,angleX);
        }

        public void rotationX(){
            angleX = axis.convertTo360(angleX);
            set(angleY,angleX);
        }
    }
    public class Axis {
        public Body body;
        public Vector3 origin,x,y,z;
        public float axisDistance;
        
        public Axis(){}
        public Axis(Body body,Vector3 origin, float distance){
            this.body = body;
            this.origin = origin;
            axisDistance = (distance >0.1f)? distance:1f;
            x = origin + new Vector3(distance,0,0);
            y = origin + new Vector3(0,distance,0);
            z = origin + new Vector3(0,0,distance);
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
                axisDistance = newDistance;
                x = origin + distanceFromOrigin(x,origin,axisDistance);
                y = origin + distanceFromOrigin(y,origin,axisDistance);
                z = origin + distanceFromOrigin(z,origin,axisDistance);
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

    public class KeyGenerator{
        public int availableKeys;

        public KeyGenerator(){}
        public KeyGenerator(int amountOfKeys){
            generateKeys(amountOfKeys);
        }

        public void generateKeys(int increaseKeysBy){
            availableKeys += increaseKeysBy;
        }
        public void getKey(){
            availableKeys -= 1;
        }
        public void returnKey(){
            availableKeys +=1;
        }
        public void resetGenerator(){
            availableKeys = 0;
        }
    }
 
    public class SendToGPU{
        public Body body;
        public Vector3[] vertices;
        public Color[] colors;
        public int[] triangles;

        public SendToGPU(Body body){
            vertices = new Vector3[0];
            colors = new Color[0];
            triangles = new int[0];
            this.body = body;
        }
        public void init(){
            updatePointCloudStart();
            int countTriangleIndex = 0;
            foreach(Joint joint in body.bodyStructure){
                if (joint != null){
                    PointCloud pointCloud = joint.pointCloud;
                    int startIndex = pointCloud.startIndexInArray;
                    CollisionSphere[] collisionSpheres = pointCloud.collisionSpheres;
                    for (int i = 0; i < collisionSpheres.Length;i++){
                        vertices[i+startIndex] = collisionSpheres[i].aroundAxis.sphere.origin;
                    }
                    for (int i = 0; i < pointCloud.triangles.Length;i++){
                        triangles[i+countTriangleIndex] = pointCloud.triangles[i]+startIndex;
                    }
                    countTriangleIndex += pointCloud.triangles.Length;
                }
            }
        }
        public void updatePointCloudStart(){
            int maxVerticesSize = 0;
            int maxTriangleSize = 0;
            for (int i = 0; i<body.bodyStructure.Length;i++){
                Joint joint = body.bodyStructure[i];
                if (joint !=null){
                    int pointCloudSize = joint.pointCloud.collisionSpheres.Length;
                    if (pointCloudSize>0){
                        joint.pointCloud.startIndexInArray = maxVerticesSize;
                        maxVerticesSize += pointCloudSize;
                        maxTriangleSize += joint.pointCloud.triangles.Length;
                    }
                }
            }
            colors = new Color[maxVerticesSize];
            vertices = new Vector3[maxVerticesSize];
            triangles = new int[maxTriangleSize];
        }
        public void updateArray(){
            for (int i = 0; i<body.bodyStructure.Length;i++){
                body.bodyStructure[i]?.pointCloud.updateGPUArray();
            }
        }
    }
    public class Body {
        public World world;
        public int worldKey;
        public Axis globalAxis;
        public Joint[] bodyStructure;
        public KeyGenerator keyGenerator;
        public Editor editor;
        public List<VertexVisualizer.BakedMesh> bakedMeshes;
        public string amountOfDigits; 
        public int asyncDelay, time;
        public SendToGPU sendToGPU;
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
            globalAxis = new Axis(this,new Vector3(0,0,0),1);
            bodyStructure = null;
            keyGenerator = null;
            editor = new Editor(this);
            editor.initilizeBody();
            amountOfDigits = "0.000000";
            asyncDelay = 20;
            sendToGPU = new SendToGPU(this);
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
        public void saveBodyStructure(StreamWriter writer, int chunkSize){
            string stringPath = $"{bodyDepth},{worldKey}";
            saveTimeStamp(writer,stringPath);
            saveJointsInBody(writer,stringPath,chunkSize);
        }
        public void saveTimeStamp(StreamWriter writer,string stringPath){
            string str = $"{stringPath},{timeStamp},1,1,{time}{Environment.NewLine}";
            writer.Write(str);
            time++;
        }
        void saveJointsInBody(StreamWriter writer, string stringPath, int chunkSize){
            writer.Write($"{stringPath},{allJointsInBody},{bodyStructure.Length},1");
            if (bodyStructure.Length>0) {
                writer.Write(",");
                int offset = 0;
                while (offset < bodyStructure.Length){
                    int elementsToWrite = Math.Min(bodyStructure.Length-offset, chunkSize);
                    for (int i = 0; i < elementsToWrite; i+=1){
                        if (bodyStructure[i] != null) sb.Append($"{i},");
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
                $"{stringPath},{globalOriginLocation},1,3,{accuracyAmount(globalOrigin.x)},{accuracyAmount(globalOrigin.y)},{accuracyAmount(globalOrigin.z)}{Environment.NewLine}" + 
                $"{stringPath},{globalAxisQuaternion},1,4,{accuracyAmount(quat.x)},{accuracyAmount(quat.y)},{accuracyAmount(quat.z)},{accuracyAmount(quat.w)}{Environment.NewLine}" + 
                $"{stringPath},{radianOrAngle},1,1,{radianOrDegree}{Environment.NewLine}"
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
        public Dictionary<int,int> arraySizeManager(int amount){
            Dictionary<int,int> newKeys = new Dictionary<int,int>();
            if (bodyStructure != null){
                if (amount > bodyStructure.Length){
                    resizeArray(amount);
                } else if (amount < bodyStructure.Length){
                    newKeys = optimizeBody();
                }
            } else {
                bodyStructure = new Joint[amount];
                keyGenerator = new KeyGenerator(amount);
            }
            return newKeys;
        }
        public void resizeArray(int maxKey){
            int limitCheck = bodyStructure.Length - maxKey;
            if(limitCheck <= 0) {
                int newMax = bodyStructure.Length + Mathf.Abs(limitCheck)+1;
                keyGenerator.generateKeys(Mathf.Abs(limitCheck)+1);
                Joint[] newJointArray = new Joint[newMax];
                for (int i = 0; i<bodyStructure.Length; i++){
                    Joint joint = bodyStructure[i];
                    if (joint != null){
                        newJointArray[i] = joint;
                    }
                }
                bodyStructure = newJointArray;
            }
        }
        public Dictionary<int,int> optimizeBody(){
            int max = bodyStructure.Length;
            int newSize = max - keyGenerator.availableKeys;
            Joint[] orginizedJoints = new Joint[newSize];
            int newIndex = 0;
            Dictionary<int,int> newKeys = new Dictionary<int,int>();
            for (int i = 0; i < max; i++){
                Joint joint = bodyStructure[i];
                if (joint != null){
                    newKeys.Add(joint.connection.indexInBody,newIndex);
                    joint.connection.indexInBody = newIndex;
                    orginizedJoints[newIndex] = joint; 
                    newIndex++;
                }
            }
            bodyStructure = orginizedJoints;
            keyGenerator.resetGenerator();
            return newKeys;
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
        }
    }
    public class Joint {
        public Body body;
        public Axis localAxis;
        public AroundAxis fromGlobalAxis;
        public Connection connection;
        public PointCloud pointCloud;
        public bool movementOptionBool,keepBodyTogetherBool;
        public UnityAxis unityAxis;
        public string jointNameString;
        public Joint(){}
        public Joint(Body body, int indexInBody){
            init(body, indexInBody);
        }
        public Joint(Body body, int indexInBody, UnityAxis unityAxis){
            init(body, indexInBody);
            unityAxis.joint = this;
            this.unityAxis = unityAxis;
        }
        void init(Body body, int indexInBody){
            this.body = body;
            localAxis = new Axis(body,new Vector3(0,0,0),1);
            connection = new Connection(this,indexInBody);
            pointCloud = new PointCloud(this);
            body.keyGenerator.getKey();
            movementOptionBool = false;
            keepBodyTogetherBool = true;
            Sphere sphere = new Sphere();
            sphere.setOrigin(localAxis.origin);
            fromGlobalAxis = new AroundAxis(body.globalAxis,sphere);
            jointNameString = "";
        }

        public void saveJointPosition(StreamWriter writer, bool radianOrAngle){
            float convert = radianOrAngle? 180f/Mathf.PI:1;
            Vector3 jointOrigin = localAxis.origin;
            Vector3 globalOrigin = body.globalAxis.origin;
            Vector4 quat = localAxis.getQuat();
            float distanceFromOrigin = body.globalAxis.length(jointOrigin-globalOrigin);
            fromGlobalAxis.distance = distanceFromOrigin;
            fromGlobalAxis.sphere.setOrigin(localAxis.origin);
            fromGlobalAxis.get();
            string stringPath = $"{jointDepth},{body.worldKey},{connection.indexInBody}";
            writer.Write(
                $"{stringPath},{jointName},{jointNameString.Length},1,{jointNameString}{Environment.NewLine}" +
                $"{stringPath},{distanceFromGlobalOrigin},1,1,{body.accuracyAmount(distanceFromOrigin)}{Environment.NewLine}" +
                $"{stringPath},{YXFromGlobalAxis},1,2,{body.accuracyAmount(fromGlobalAxis.angleY*convert)},{body.accuracyAmount(fromGlobalAxis.angleX*convert)}{Environment.NewLine}" +
                $"{stringPath},{localOriginLocation},1,3,{body.accuracyAmount(localAxis.origin.x)},{body.accuracyAmount(localAxis.origin.y)},{body.accuracyAmount(localAxis.origin.z)}{Environment.NewLine}"+
                $"{stringPath},{localAxisQuaternion},1,4,{body.accuracyAmount(quat.x)},{body.accuracyAmount(quat.y)},{body.accuracyAmount(quat.z)},{body.accuracyAmount(quat.z)}{Environment.NewLine}"
            );
            connection.savePastConnections(writer,stringPath);
            writer.WriteLine("");
            connection.saveFutureConnections(writer,stringPath);
            writer.WriteLine("");
        }

        public void deleteJoint(){
            body.keyGenerator.returnKey();
            connection.disconnectAllFuture();
            connection.disconnectAllPast();
            pointCloud.deleteAllSpheres();
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
            if (pointCloud.collisionSpheres != null) pointCloud.moveSpheres(add);
        }
        public void rotateJoint(Vector4 quat){
            localAxis.rotate(quat,localAxis.origin);
            if (pointCloud.collisionSpheres != null) pointCloud.rotateAllSpheres(quat,localAxis.origin);
        }
        public void updatePhysics(){ 
            unityAxis?.updateJoint();
            if (pointCloud.collisionSpheres != null) pointCloud.updatePhysics();  
        }
    }
    public class PointCloud {
        public Joint joint;
        public KeyGenerator keyGenerator;
        public CollisionSphere[] collisionSpheres;
        public int startIndexInArray;
        public int[] triangles;
        StringBuilder sb = new StringBuilder();
        public PointCloud(){}
        public PointCloud(Joint joint){
            collisionSpheres = null;
            this.joint = joint;
            triangles = new int[0];
        }

        public void savePointCloud(StreamWriter writer,int chunkSize){
            if (collisionSpheres != null){
                string stringPath = $"{jointDepth},{joint.body.worldKey},{joint.connection.indexInBody}";
                savePointCloudPositions(writer,stringPath,chunkSize);
                saveTriangles(writer,stringPath, chunkSize);
            }
        }
        void savePointCloudPositions(StreamWriter writer, string stringPath, int chunkSize){
            writer.Write($"{stringPath},{pointCloudSphereDatas},{collisionSpheres.Length},11");
            if (collisionSpheres.Length>0) {
                writer.Write(","); 
                int offset = 0;
                while (offset < collisionSpheres.Length){
                    int elementsToWrite = Math.Min(collisionSpheres.Length-offset, chunkSize);
                    for (int i = 0; i < elementsToWrite; i+=1){
                        CollisionSphere collisionSphere = collisionSpheres[i];
                        if (collisionSphere != null) {
                            Vector3 vec = collisionSphere.aroundAxis.sphere.origin;
                            Color col = collisionSphere.aroundAxis.sphere.color;
                            AroundAxis aroundAxis = collisionSphere.aroundAxis;
                            sb.Append($"{i},{vec.x},{vec.y},{vec.z},{aroundAxis.distance},{aroundAxis.angleY},{aroundAxis.angleX},{col.r},{col.g},{col.b},{col.a},");
                        }
                    }
                    offset += elementsToWrite;
                    if (offset == collisionSpheres.Length) {
                        sb.Remove(sb.Length - 1, 1);
                        sb.Append(Environment.NewLine);
                        };
                    writer.Write(sb.ToString());
                    sb.Clear();
                }        
            } else writer.WriteLine("");
        }
        void saveTriangles(StreamWriter writer, string stringPath, int chunkSize){
            if (triangles.Length>3){
                writer.Write($"{stringPath},{trianglesInPointCloud},{triangles.Length/3},3,");
                int offset = 0;
                while (offset < triangles.Length){
                    int elementsToWrite = Math.Min(triangles.Length-offset, chunkSize);
                    elementsToWrite = elementsToWrite / 3 * 3;
                    for (int i = 0; i < elementsToWrite; i+=3){
                        int index1 = triangles[offset + i];
                        int index2 = triangles[offset + i + 1];
                        int index3 = triangles[offset + i + 2];
                        bool check = index1<triangles.Length && index2<triangles.Length && index3<triangles.Length;
                        if (check) {
                            sb.Append($"{index1},{index2},{index3},");
                        };
                    }
                    offset += elementsToWrite;
                    if (offset == triangles.Length) {
                        sb.Remove(sb.Length - 1, 1);
                        sb.Append(Environment.NewLine);
                    };
                    writer.Write(sb.ToString());
                    sb.Clear();
                }
            } else {
                writer.Write($"{stringPath},{trianglesInPointCloud},{triangles.Length},3{Environment.NewLine}");
            }
        }
        public void saveBakedMeshes(StreamWriter writer){
            string stringPath = $"{jointDepth},{joint.body.worldKey},{joint.connection.indexInBody}";
            writer.Write($"{stringPath},{bakedMeshIndex},2,{collisionSpheres.Length/2},");
            if (collisionSpheres != null){
                for (int i = 0;i<collisionSpheres.Length;i++){
                    CollisionSphere collisionSphere = collisionSpheres[i];
                    if (collisionSphere != null) {
                        BakedMeshIndex bakedMeshIndex = collisionSphere.bakedMeshIndex;
                        if (bakedMeshIndex != null) {
                            string str = (i+1 != collisionSpheres.Length)? 
                                $"{bakedMeshIndex.indexInBakedMesh},{bakedMeshIndex.indexInVertex},":
                                $"{bakedMeshIndex.indexInBakedMesh},{bakedMeshIndex.indexInVertex}{Environment.NewLine}";
                            writer.Write(str);
                        }
                    }
                }
            }
        }
        public void deleteSphere(int key){
            CollisionSphere remove = collisionSpheres[key];
            if(remove != null){
                keyGenerator.returnKey();
                collisionSpheres[key] = null;
            }
        }
        public void deleteAllSpheres(){
            int size = collisionSpheres.Length;
            for (int i = 0; i<size;i++){
                deleteSphere(i);
            }
        }
        public void resetAllSphereOrigins(){
            int sphereCount = collisionSpheres.Length;
            for (int i = 0; i<sphereCount; i++){
                collisionSpheres[i]?.aroundAxis.resetOrigin();
            }
        }
        public void updateGPUArray(){
            for (int i = 0; i<collisionSpheres.Length;i++){
                CollisionSphere collisionSphere = collisionSpheres[i];
                if (collisionSphere!= null){
                    joint.body.sendToGPU.vertices[i+startIndexInArray] = collisionSphere.aroundAxis.sphere.origin;
                }
            }
        }
        public void rotateAllSpheres(Vector4 quat, Vector3 rotationOrigin){
            int sphereCount = collisionSpheres.Length;
            for (int i = 0; i<sphereCount; i++){
                CollisionSphere collisionSphere = collisionSpheres[i];
                if (collisionSphere != null){
                    Vector3 vec = collisionSphere.aroundAxis.sphere.origin;
                    vec = joint.localAxis.quatRotate(vec,rotationOrigin,quat);
                    collisionSphere.setOrigin(vec);
                }
            }
        }
        public void moveSpheres(Vector3 move){
            int sphereCount = collisionSpheres.Length;
            for (int i = 0; i<sphereCount; i++){
                CollisionSphere collisionSphere = collisionSpheres[i];
                collisionSphere?.moveOrigin(move);
            }
        }
        public void updatePhysics(){
            int sphereCount = collisionSpheres.Length;
            for (int i = 0;i<sphereCount; i++){
                collisionSpheres[i]?.updatePhysics(); 
            }
        }
        public List<CollisionSphere> arrayToList(){
            List<CollisionSphere> list = new List<CollisionSphere>();
            int sphereCount = collisionSpheres.Length;
            for (int i = 0; i<sphereCount; i++){
                CollisionSphere collisionSphere = collisionSpheres[i];
                if (collisionSphere != null){
                    list.Add(collisionSphere);
                }
            }
            return list;
        }
        public Dictionary<int,int> arraySizeManager(int amount){
            Dictionary<int,int> newKeys = new Dictionary<int,int>();
            if (collisionSpheres != null){
                if (amount > collisionSpheres.Length){
                    resizeArray(amount);
                } else if (amount < collisionSpheres.Length){
                    newKeys = optimizeCollisionSpheres();
                }
            } else {
                collisionSpheres = new CollisionSphere[amount];
                keyGenerator = new KeyGenerator(amount);
            }
            return newKeys;
        }
        public void resizeArray(int maxKey){
            int limitCheck = collisionSpheres.Length - maxKey;
            if(limitCheck <= 0) {
                int newMax = collisionSpheres.Length + Mathf.Abs(limitCheck)+1;
                keyGenerator.generateKeys(Mathf.Abs(limitCheck)+1);
                CollisionSphere[] newCollisionSpheresArray = new CollisionSphere[newMax];
                for (int i = 0; i<collisionSpheres.Length; i++){
                    CollisionSphere collisionSphere = collisionSpheres[i];
                    if (collisionSphere != null){
                        newCollisionSpheresArray[i] = collisionSphere;
                    }
                }
                collisionSpheres = newCollisionSpheresArray;
            }
        }
        public Dictionary<int,int> optimizeCollisionSpheres(){
            int maxKeys = collisionSpheres.Length;
            int used = maxKeys - keyGenerator.availableKeys;
            CollisionSphere[] newCollision = new CollisionSphere[used];
            Dictionary<int,int> newKeys = new Dictionary<int,int>();
            int count = 0;
            for (int j = 0; j<maxKeys; j++){
                CollisionSphere collision = collisionSpheres[j];
                if (collision != null){
                    newKeys.Add(collision.path.collisionSphereKey,count);
                    collision.path.setCollisionSphereKey(count);
                    newCollision[count] = collision;
                    count++;
                }
            }
            collisionSpheres = newCollision;
            keyGenerator.resetGenerator();
            return newKeys;
        }
    }

    public class Path {
        public Body body;
        public Joint joint;
        public int collisionSphereKey;

        public Path(){}
        public Path(Body body, Joint joint, int collisionSphereKey){
            this.body=body;
            this.joint=joint;
            this.collisionSphereKey=collisionSphereKey;
        }

        public void setBody(Body body){
            this.body=body;
        }
        public void setJoint(Joint joint){
            this.joint = joint;
        }
        public void setCollisionSphereKey(int collisionSphereKey){
            this.collisionSphereKey = collisionSphereKey;
        }
    }

    public class BakedMeshIndex{
        public CollisionSphere collisionSphere;
        public int indexInBakedMesh;
        public int indexInVertex;
        public BakedMeshIndex(){}
        public BakedMeshIndex(int indexInBakedMesh,int indexInVertex){
            this.indexInBakedMesh = indexInBakedMesh;
            this.indexInVertex = indexInVertex;
        }

        public void setPoint(){
            VertexVisualizer.BakedMesh bakedMesh = collisionSphere.path.body.bakedMeshes[indexInBakedMesh];
            AroundAxis aroundAxis = collisionSphere.aroundAxis;
            Axis axis = aroundAxis.axis;
            Vector3 point = collisionSphere.path.joint.body.globalAxis.origin+bakedMesh.vertices[indexInVertex];
            collisionSphere.setOrigin(point);
            aroundAxis.distance = aroundAxis.axis.length(point - axis.origin);
            aroundAxis.getPointAroundAxis(point, out float angleY,out float angleX);
            aroundAxis.angleY = angleY;
            aroundAxis.angleX = angleX;
        }
    }
    public class CollisionSphere {
        public Path path;
        public AroundAxis aroundAxis;
        public BakedMeshIndex bakedMeshIndex;

        public CollisionSphere(){}
        public CollisionSphere(Joint joint, int sphereIndex){
            init(joint, sphereIndex);
        }
        public CollisionSphere(Joint joint, int sphereIndex,BakedMeshIndex bakedMeshIndex){
            init(joint, sphereIndex);
            this.bakedMeshIndex = bakedMeshIndex;
            bakedMeshIndex.collisionSphere = this;
        }
        void init(Joint joint, int sphereIndex){
            path = new Path(joint.body,joint,sphereIndex);
            aroundAxis = new AroundAxis(joint.localAxis,new Sphere());
            joint.pointCloud.keyGenerator.getKey();
        }
        public void setOrigin(Vector3 newOrigin){
            aroundAxis.sphere.setOrigin(newOrigin);
        }
        public void moveOrigin(Vector3 newOrigin){
            aroundAxis.sphere.moveOrigin(newOrigin);
        }
        public void setRadius(float newRadius){
            aroundAxis.sphere.setRadius(newRadius);
        }
        public void updatePhysics(){
            if (bakedMeshIndex != null && path.body.bakedMeshes != null) bakedMeshIndex.setPoint();
        }
    }
    public class Sphere{
        public float radius;
        public Vector3 origin;
        public Color color;
        
        public Sphere(){
            setOrigin(new Vector3(0,0,0));
            setRadius(0.01f);
            setColor(new Color(1,1,1,1));
        }
        public Sphere(Vector3 origin, float radius, Color color){
            setOrigin(origin);
            setRadius(radius);
            setColor(color);
        }
        public void setOrigin(Vector3 newOrigin){
            origin = newOrigin;
        }
        public void moveOrigin(Vector3 newOrigin){
            origin += newOrigin;
        }
        public void setRadius(float newRadius){
            radius = newRadius;
        }
        public void setColor(Color newColor){
            color = newColor;
        } 
    }

    const string bodyDepth = "2",
        allJointsInBody = "AllJointsInBody",
        globalOriginLocation = "GlobalOriginLocation",
        globalAxisQuaternion = "GlobalAxisQuaternion",
        radianOrAngle = "RadianOrAngle",
        timeStamp = "TimeStamp";

    const string jointDepth = "3",
        jointName = "JointName",
        distanceFromGlobalOrigin = "DistanceFromGlobalOrigin",
        YXFromGlobalAxis = "YXFromGlobalAxis",
        localOriginLocation = "LocalOriginLocation",
        localAxisQuaternion = "LocalAxisQuaternion",
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
            if (!Directory.Exists($"{pathToFolder}/initilize")) {
                Directory.CreateDirectory(pathToFolder);
                }
            if (!Directory.Exists($"{pathToFolder}/initilize")) {
                Directory.CreateDirectory(pathToFolder);
                }
        }
        
        void read(int count){
            using(StreamReader readtext = new StreamReader($"{pathToFolder}/{count}.txt")){
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
            body.saveBodyStructure(writetext,chunkSize);
            int size = body.bodyStructure.Length;
            for (int i = 0; i<size; i++){
                Joint joint = body.bodyStructure[i];
                if (joint != null){
                    joint.saveJointPosition(writetext,radianOrDegree);
                    joint.pointCloud.savePointCloud(writetext,chunkSize);
                    writetext.WriteLine("");
                } 
            }
            body.saveBodyPosition(writetext,radianOrDegree);
        }
        
        internal void writer(){
            using(StreamWriter writeText = new StreamWriter($"{pathToFolder}/{count}.txt")) {
                write(writeText);                 
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
    }

}
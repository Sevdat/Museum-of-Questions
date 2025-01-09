using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class UnityPluginTest : MonoBehaviour
{
    Experiment exp = new Experiment();
    class Experiment:SourceCode{
        public Axis ax;
        public Body lol;
        int count = 0;
        int time = 0;
        public void strt(){
            lol = new Body(0);
        }
        public void readWrite(){
            lol.editor.readWrite();
        }
        public void trackWriter(){
            lol.editor.trackWriter();
        }
        public void readTextFiles(){
            if (time > 1){
                print(count);
                // lol.editor.reader(count);
                count++;
                if (count>35) count = 0;
                time = 0;
            } else time++;
        }
    }
    class MyData {
        public string name;
        public int score;
        public bool isActive;
    }

    void Start(){
        exp.strt();
    }
    void LateUpdate(){
        exp.readWrite();
    }



    // void Start(){
    //     // Create an instance of your data class
    //     MyData data = new MyData {
    //         name = "Player1",
    //         score = 100,
    //         isActive = true
    //     };

    //     string json = JsonUtility.ToJson(data, true);
    //     string path = Path.Combine(Application.dataPath, "MyData.json");
    //     File.WriteAllText(path, json);
    //     print("JSON file created at: " + path);
    // }
}
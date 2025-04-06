using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AssetTerminal : MonoBehaviour
{
    
    public GameObject assetTerminalPrefab;
    public GameObject authorNameMenuContent,authorNamePrefab;
    public List<GameObject> authorNameButtons = new List<GameObject>();

    public GameObject projectNameMenuContent,projectNamePrefab;
    public List<GameObject> projectNameButtons = new List<GameObject>();

    public GameObject prefabIconsMenuContent,prefabIconsPrefab;
    public List<GameObject> prefabIconsButtons = new List<GameObject>();

    string path = @$"{Application.dataPath}/Resources/GeneratedAssets";
    string[][] icons;

    string[] allAuthors;
    string[][] allProjects;

    internal FolderPaths folderPaths;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.gameObject.SetActive(true);
        // createIconButtons(@$"{Application.dataPath}/Resources/GeneratedAssets/Palmov Island/Low Poly Houses Free Pack/Prefab/Icon/city hall.png");
        // createAuthorButtons("Author");
        // createProjectButtons("Project");

        allAuthors = getAllAuthors();
        foreach (string str in allAuthors) createAuthorButtons(Path.GetFileNameWithoutExtension(str));
        
        allProjects = getAllProjects();
        foreach (string[] str in allProjects){
            foreach (string strr in str){
                createProjectButtons(Path.GetFileNameWithoutExtension(strr));
            }
        }

        icons = getAllIcons();
        foreach (string[] str in icons){
            foreach (string strr in str){
                createIconButtons(strr);
            }
        }
    }
 
    // Update is called once per frame
    void Update()
    {
        
    }
    public string[] getAllAuthors(){
        return Directory.GetDirectories(path);
    }
    public string[][] getAllProjects(){
        List<string[]> iconList = new List<string[]>();
        foreach (string str in allAuthors){
            iconList.Add(Directory.GetDirectories(str));
        }
        return iconList.ToArray();
    }
    public string[][] getAllIcons(){
        string[] allDirectories = Directory.GetDirectories(path,"*",SearchOption.AllDirectories);
        string[] allIconPaths = allDirectories.Where(dir => dir.Replace('\\', '/').EndsWith("Prefab/Icon")).ToArray();
        List<string[]> iconList = new List<string[]>();
        foreach (string str in allIconPaths){
            iconList.Add(Directory.GetFiles(str, "*.png"));
        }
        return iconList.ToArray();
    }

    public GameObject createAuthorButtons(string authorName){
        return createItem(authorName,authorNameMenuContent,authorNamePrefab,0);
    }
    public GameObject createProjectButtons(string projectName){
        return createItem(projectName,projectNameMenuContent,projectNamePrefab,1);
    }
    public GameObject createIconButtons(string path){
        string fileName = Path.GetFileNameWithoutExtension(path);
        GameObject icon = createItem(fileName,prefabIconsMenuContent,prefabIconsPrefab,2);
        Sprite sprite = createSpriteFromTexture2D(path);
        icon.name = Path.GetDirectoryName(path).Replace(@"Prefab\Icon",@"Prefab\Content")+$@"\{fileName}"+$@"\{fileName}.gltf";
        icon.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
        return icon;
    }

    public GameObject createItem(string strName, GameObject content,GameObject button, int option = 0){
        GameObject gameObject;
        gameObject = Instantiate(button, Vector3.zero, Quaternion.identity);
        gameObject.transform.SetParent(content.transform);
        gameObject.transform.localScale = Vector3.one;
        gameObject.transform.localPosition = new Vector3();
        gameObject.transform.localRotation = Quaternion.Euler(new Vector3());
        gameObject.name = strName;
        Button clone = gameObject.GetComponent<Button>();
        if (option == 0) clone.onClick.AddListener(() => onClickAuthor(clone)); else 
        if (option == 1) clone.onClick.AddListener(() => onClickProject(clone)); else 
        if (option == 2) clone.onClick.AddListener(() => onClickIcon(clone));
        clone.GetComponentInChildren<TMP_Text>().text = strName;
        return gameObject;
    }

    public void onClickAuthor(Button button){
        print("Author");
    }
    public void onClickProject(Button button){
        print("Project");
    }
    public void onClickIcon(Button button){
        folderPaths.exportImportGLTF.ImportGLTF(button.gameObject.name);
    }

    public static Texture2D LoadPNG(string filePath) {
        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath)) 	{
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }

    public Sprite createSpriteFromTexture2D(string path){
        Texture2D iconTexture = LoadPNG(path);
        return Sprite.Create(
            iconTexture,
            new Rect(0, 0, iconTexture.width, iconTexture.height),
            new Vector2(0.5f, 0.5f) // Pivot point (center)
        );
    }
}

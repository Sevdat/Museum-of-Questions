using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    
    // Start is called before the first frame update
    void Start()
    {
        transform.gameObject.SetActive(true);
        createIconButtons(@$"{Application.dataPath}/Resources/GeneratedAssets/Palmov Island/Low Poly Houses Free Pack/Prefab/Icon/city hall.png");
        createAuthorButtons("Author");
        createProjectButtons("Project");
    }
 
    // Update is called once per frame
    void Update()
    {
        
    }
    
    public GameObject createAuthorButtons(string authorName){
        return createItem(authorName,authorNameMenuContent,authorNamePrefab,0);
    }
    public GameObject createProjectButtons(string projectName){
        return createItem(projectName,projectNameMenuContent,projectNamePrefab,1);
    }
    public GameObject createIconButtons(string path){
        GameObject icon = createItem(Path.GetFileNameWithoutExtension(path),prefabIconsMenuContent,prefabIconsPrefab,2);
        Sprite sprite = createSpriteFromTexture2D(path);
        icon.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
        return icon;
    }


    public Sprite createSpriteFromTexture2D(string path){
        Texture2D iconTexture = LoadPNG(path);
        return Sprite.Create(
            iconTexture,
            new Rect(0, 0, iconTexture.width, iconTexture.height),
            new Vector2(0.5f, 0.5f) // Pivot point (center)
        );
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
        print("Icon");
    }
    public static Texture2D LoadPNG(string filePath) {

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath)) 	{
            print("lol");
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }
}

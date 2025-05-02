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

    public GameObject refreshGameObject;
    public TextMeshProUGUI precenageDoneGameObject;


    string[][] icons;

    string[] allAuthors;
    string[][] allProjects;

    internal Main main;
    internal int maxValue;
    internal float incrementPrecentegeDone = 0;
    internal float precentageDone = 0f;
    
    // Start is called before the first frame update
    void Start(){
        refresh();
        refreshGameObject.GetComponent<Button>().onClick.AddListener(() => StartCoroutine(onRefreshButtonClick()));
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal IEnumerator courutineStart(int amount = 10){

        var gridLayout = GetComponent<GridLayoutGroup>();
        if (gridLayout != null) gridLayout.enabled = false;
        
        int count = 0;
        foreach (string str in allAuthors) {      
            if (count >amount) {count = 0; yield return null;}
            authorNameButtons.Add(createAuthorButtons(Path.GetFileNameWithoutExtension(str)));
            count++;
            precentageDone += incrementPrecentegeDone;
            precenageDoneGameObject.text = precentageDone.ToString("0.0");
        }
        
        foreach (string[] str in allProjects){
            foreach (string strr in str){
                if (count >amount) {count = 0; yield return null;}
                projectNameButtons.Add(createProjectButtons(Path.GetFileNameWithoutExtension(strr)));
                count++;
                precentageDone += incrementPrecentegeDone;
                precenageDoneGameObject.text = precentageDone.ToString("0.0");
            }
        }

        foreach (string[] str in icons){
            foreach (string strr in str){
                if (count >amount) {count = 0; yield return null;}
                prefabIconsButtons.Add(createIconButtons(strr));
                count++;
                precentageDone += incrementPrecentegeDone;
                precenageDoneGameObject.text = precentageDone.ToString("0.0");
            }
        }
        precentageDone = 100f;
        precenageDoneGameObject.text = 100f.ToString("0.0");
        yield break;
    }
    public IEnumerator destroyButtons(int amount = 10) {
        int count = 0;
        int maxLength = Mathf.Max(authorNameButtons.Count, projectNameButtons.Count, prefabIconsButtons.Count);
        
        for (int i = maxLength - 1; i >= 0; i--) {
            if (count >= amount) {
                count = 0;
                yield return null;
            }

            if (i < authorNameButtons.Count) Destroy(authorNameButtons[i]);
            if (i < projectNameButtons.Count) Destroy(projectNameButtons[i]);
            if (i < prefabIconsButtons.Count) Destroy(prefabIconsButtons[i]);

            count++;
            precentageDone -= incrementPrecentegeDone;
            precenageDoneGameObject.text = precentageDone.ToString("0.0");
        }

        authorNameButtons.Clear();
        projectNameButtons.Clear();
        prefabIconsButtons.Clear();
        authorNameButtons.TrimExcess();
        projectNameButtons.TrimExcess();
        prefabIconsButtons.TrimExcess();

        precentageDone = 0;
        precenageDoneGameObject.text = 0f.ToString("0.0");
    }

    public void refresh(){
        var allAuthors = getAllAuthors();
        this.allAuthors = allAuthors.Item1;
        var allProjects = getAllProjects();
        this.allProjects = allProjects.Item1;
        var icons = getAllIcons();
        this.icons = icons.Item1;
        maxValue = allAuthors.Item2 + allProjects.Item2 + icons.Item2;
        if (maxValue != 0.0f) incrementPrecentegeDone = 100f/maxValue;
    }
    public (string[], int) getAllAuthors(){
        string[] str = Directory.GetDirectories(main.orginizePaths.assetPath);
        return (str, str.Length);
    }
    public (string[][], int) getAllProjects(){
        int size = 0;
        List<string[]> iconList = new List<string[]>();
        foreach (string str in allAuthors){
            string[] temp = Directory.GetDirectories(str);
            size += temp.Length;
            iconList.Add(temp);
        }
        return (iconList.ToArray(),size);
    }
    public (string[][], int) getAllIcons(){
        int size = 0;
        string[] allDirectories = Directory.GetDirectories(main.orginizePaths.assetPath,"*",SearchOption.AllDirectories);
        string[] allIconPaths = allDirectories.Where(dir => dir.Replace('\\', '/').EndsWith("Prefab/Icon")).ToArray();
        List<string[]> iconList = new List<string[]>();
        foreach (string str in allIconPaths){
            string[] temp = Directory.GetFiles(str, "*.png");
            size += temp.Length;
            iconList.Add(temp);
        }
        return (iconList.ToArray(), size);
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
        icon.name = Path.GetDirectoryName(path).Replace(@"Prefab\Icon",@"Prefab\Content")+$@"\{fileName}\{fileName}.gltf";
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
        main.loadPrefab(button.gameObject.name);
        transform.gameObject.SetActive(false);
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

    public IEnumerator onRefreshButtonClick(){
        yield return StartCoroutine(destroyButtons());
        refresh();
        yield return null;
        yield return StartCoroutine(courutineStart());
    }

    public void onMenuClick(){
        StartCoroutine(onMenuButtonClick());
    }
    public IEnumerator onMenuButtonClick(){
        if (precentageDone != 100.0f){
            yield return StartCoroutine(destroyButtons());
            yield return null;
            yield return StartCoroutine(courutineStart());
        }
    }

}

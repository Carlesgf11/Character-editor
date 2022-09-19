using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml.Serialization;
using System.IO;

public class AppControl : MonoBehaviour
{
    public enum categorieType { TEXTURES, ACCESORIES };
    public Transform gridCategories, gridSubCategories;
    public GameObject baseBtn;

    //Accessories
    public List<Transform> bones;

    //Sistema de guardado
    public InputField saveWord;
    public List<string> allKeySave;


    //Sistema de camara
    public int currentCameraPos;
    [SerializeField] Camera mainCamera;
    [SerializeField] List<Transform> cameraPos;

    public class SaveParameters
    {
        public int[] textures, accessories;

        public SaveParameters() { }
        public SaveParameters(int[] _texture, int[] _accessories)
        {
            textures = _texture;
            accessories = _accessories;
        }
    }
    private int[] textures = new int[8];
    private int[] accessories = new int[4];


    void Start()
    {
        allKeySave = PlayerPrefsX.GetStringList("AllSaves");
        for (int i = 0; i < accessories.Length; i++)
        {
            accessories[i] = -1;
        }
        PrintCategories();
        currentCameraPos = 0;
    }

    void PrintCategories()
    {
        int totalCategories = Resources.LoadAll<Sprite>("Categories").Length;
        for (int i = 0; i < totalCategories; i++)
        {
            GameObject newBtn = Instantiate(baseBtn, gridCategories);
            newBtn.transform.GetChild(0).GetComponent<Image>().sprite =
                Resources.Load<Sprite>("Categories/" + i);
            int tempIndex = i;
            newBtn.GetComponent<Button>().onClick.AddListener(
                delegate { PrintElements(categorieType.TEXTURES, tempIndex); currentCameraPos = tempIndex; });
            
        }

        int totalCatAccessories = Resources.LoadAll<Sprite>("CatAccessories").Length;
        for (int i = 0; i < totalCatAccessories; i++)
        {
            GameObject newBtn = Instantiate(baseBtn, gridCategories);
            newBtn.transform.GetChild(0).GetComponent<Image>().sprite =
                Resources.Load<Sprite>("CatAccessories/" + i);

            int tempIndex = i;
            newBtn.GetComponent<Button>().onClick.AddListener(
                delegate { PrintElements(categorieType.ACCESORIES, tempIndex); currentCameraPos = tempIndex + 8; });
        }

        //GameObject saveAvatar = Instantiate(baseBtn, gridCategories);
        //saveAvatar.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Save");
        //saveAvatar.GetComponent<Button>().onClick.AddListener(delegate { savePanel.SetActive(true); ; });
        //saveAvatar.transform.SetAsLastSibling();
    }
    void PrintElements(categorieType _type, int _index)
    {
        //print(_type + "           " + _index);
        for (int i = gridSubCategories.childCount - 1; i >= 0; i--)
        {
            Destroy(gridSubCategories.GetChild(i).gameObject);
        }
        switch (_type)
        {
            case categorieType.TEXTURES:
                int texturesCount = Resources.LoadAll<Sprite>("Elements/" + _index).Length;
                for (int i = 0; i < texturesCount; i++)
                {
                    GameObject newTexture = Instantiate(baseBtn, gridSubCategories);
                    newTexture.transform.GetChild(0).GetComponent<Image>().sprite =
                        Resources.Load<Sprite>("Elements/" + _index + "/" + i);

                    int category = _index;
                    int subCategory = i;
                    newTexture.GetComponent<Button>().onClick.AddListener(
                        delegate { SetTexture(category, subCategory); }); 
                }
                break;
            case categorieType.ACCESORIES:
                int accesoriesCount = Resources.LoadAll<Sprite>("ElemAccessories/" + _index).Length;
                for (int i = 0; i < accesoriesCount; i++)
                {
                    GameObject newAccessory = Instantiate(baseBtn, gridSubCategories);
                    newAccessory.transform.GetChild(0).GetComponent<Image>().sprite =
                        Resources.Load<Sprite>("ElemAccessories/" + _index + "/" + i);

                    int category = _index;
                    int subCategory = i;
                    newAccessory.GetComponent<Button>().onClick.AddListener(
                        delegate { SetAccessory(category, subCategory); });
                }

                //Boton quitar accesorio
                GameObject emptyElement = Instantiate(baseBtn, gridSubCategories);
                emptyElement.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("None");
                emptyElement.GetComponent<Button>().onClick.AddListener(
                    delegate { SetAccessory(_index, -1); });
                emptyElement.transform.SetSiblingIndex(0);//para poner el boton al principio de la lista
                break;
        }
    }
    void SetTexture(int _category, int _subCategory)
    {
        //print(_category + "      " + _subCategory);
        textures[_category] = _subCategory;

        Material mat = Resources.Load<Material>("Materials/" + _category);
        Texture2D albedo = Resources.Load<Texture2D>("Textures/" + _subCategory);
        Texture2D normal = Resources.Load<Texture2D>("Textures/" + _subCategory + "/Normal");
        Texture2D metallic = Resources.Load<Texture2D>("Textures/" + _subCategory + "/Metallic");

        mat.SetTexture("_MainTex", albedo);
        mat.SetTexture("_BumpMap", normal);
    }
    void SetAccessory(int _category, int _subCategory)
    {
        //print(_category + "      " + _subCategory);
        accessories[_category] = _subCategory;
    
        for (int i = bones[_category].childCount - 1; i >= 0; i--)
            Destroy(bones[_category].GetChild(i).gameObject);      
        GameObject newAsset = Resources.Load<GameObject>("Accessories/" + _category + "/" + _subCategory);
        if(newAsset != null)
        {
            newAsset = Instantiate(newAsset, bones[_category]);
        }
    }

    //Skin: "position":{"x":0.46799999475479128,"y":1.0180000066757203,"z":-2.8480000495910646}
    //Eyes: "position":{"x":0.23999999463558198,"y":1.7699999809265137,"z":-0.7400000095367432}
    //Helmet: "position":{"x":0.27000001072883608,"y":1.7899999618530274,"z":-0.9399999976158142}
    //Pants: "position":{"x":0.3100000023841858,"y":0.6700000166893005,"z":-0.9399999976158142}
    //Belt: "position":{"x":0.30300000309944155,"y":0.8740000128746033,"z":-0.921999990940094}
    //Gloves: "position":{"x":0.4099999964237213,"y":0.906000018119812,"z":-1.2430000305175782}
    //Socks: "position":{"x":0.31200000643730166,"y":0.5170000195503235,"z":-0.9850000143051148}
    //Boots: "position":{"x":0.3089999854564667,"y":0.24300000071525575,"z":-0.9750000238418579}
    //Bacelet: "position":{"x":-0.09700000286102295,"y":1.2589999437332154,"z":-0.7210000157356262}
    //Hevilla: "position":{"x":0.23600000143051148,"y":0.8360000252723694,"z":-0.625}


    void Update()
    {
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraPos[currentCameraPos].transform.position, 4 * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //SaveSkin();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            LoadSkin();
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            DeleteSave();
        }
    }
    void SaveSkin()
    {
        if(allKeySave.Contains(saveWord.text) == false)
            allKeySave.Add(saveWord.text);
        PlayerPrefsX.SetStringList("AllSaves", allKeySave);
        SaveParameters newSave = new SaveParameters(textures, accessories);
        XmlSerializer serial = new XmlSerializer(typeof(SaveParameters));
        using (StringWriter writer = new StringWriter())
        {
            serial.Serialize(writer, newSave);
            PlayerPrefs.SetString(saveWord.text, writer.ToString());
        }
    }
    void LoadSkin()
    {
        string newLoad = PlayerPrefs.GetString(saveWord.text);
        if(newLoad.Length > 0)
        {
            XmlSerializer serial = new XmlSerializer(typeof(SaveParameters));
            using (StringReader reader = new StringReader(newLoad))
            {
                SaveParameters load = serial.Deserialize(reader) as SaveParameters;
                textures = load.textures;
                accessories = load.accessories;
            }
    
            for (int i = 0; i < textures.Length; i++)
            {
                SetTexture(i, textures[i]);
            }
            for (int i = 0; i < accessories.Length; i++)
            {
                SetAccessory(i, accessories[i]);
            }
        }
    }
    void DeleteSave()
    {
        allKeySave.Remove(saveWord.text);
        PlayerPrefsX.SetStringList("AllSaves", allKeySave);
        PlayerPrefs.DeleteKey(saveWord.text);
    }
}

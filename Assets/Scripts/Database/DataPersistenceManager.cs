using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    // [SerializeField] private bool useEncryption;

    public static DataPersistenceManager dataInstance {get; private set;}
    private List<IDataPersistence> dataPersistenceObjects;
    private List<IDataInitialization> dataInitializations;
    private FileDataHandler dataHandler;

    private GameData gameData;

    // Handle inventory
    private bool isDefaultInventory = true; // false: craft scene
    
    private InventoryManager inventoryManager;
    private bool isNew = true;
    private bool isStart = false;
    private bool isQuit = false;

    private void Awake()
    {
        if (dataInstance != null) {
            Debug.Log("Found one more DataPersistenceManager. Destroy this one.");
            Destroy(this.gameObject);
            return;
        } 
        dataInstance = this;
        DontDestroyOnLoad(this.gameObject);

        //Application.persistentDataPath : 유니티 프로젝트 데이터가 OS에서 기본 저장되는 위치
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
    }

    private void Start() {
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();

        // *TODO* : 정체불명의 코드... 
        // if (isNew) {
        //     isStart = scene.name == "Start" || scene.name == "Dialogue" ? true : false;

        //     if (!isStart) {
        //         inventoryManager = FindObjectOfType<InventoryManager>();
        //         if (inventoryManager == null) {
        //             Debug.LogError("There is no inventoryManager.");
        //         }
        //     }
            
        // }

        // If Scene 'Craft' -> Move inventory to craft
        if (scene.name == "Craft") {
            // Debug.Log("[Test] craft in");

            isDefaultInventory = false;

            inventoryManager.InventoryItemListToDataList();
            GameObject craftInventoryUI = GameObject.FindWithTag("inventory");
            inventoryManager.slotContainers = craftInventoryUI.GetComponent<InventoryUI>().slotContainers;     
            inventoryManager.LoadAndSettingInventoryData();
            // inventoryManager.inventorySlots = craftInventoryUI.transform.Find("ItemSlotContainer").GetComponentsInChildren<InventorySlot>();
        } else {
            if (!isDefaultInventory) { 
                // Debug.Log("[Test] craft out");
                inventoryManager.SetInventorySlotsDefault();
                inventoryManager.LoadAndSettingInventoryData();
                isDefaultInventory = true;
            }
        }

        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene) {
        SaveGame();
    }


    private void OnApplicationQuit()
    {
        isQuit = true;
        
        if (!isDefaultInventory) { 
            inventoryManager.InventoryItemListToDataList();
        }

        SaveGame();
    }

    public void NewGame()
    {
        Debug.Log("[Test] NewGame()");
        isStart = true;
        this.gameData = new GameData();
    }

    // public void LoadStartScene() {
    //     // TODO : Save Initialization Data, dontdestroy ui 처리
    // }

    public void LoadGame() {
        // 데이터 핸들러를 이용해 저장된 데이터 불러오기
        this.gameData = dataHandler.Load();

        // if) Exist Save File -> Load
        // else) NewGame

        if (this.gameData == null) {
            Debug.Log("No Data was found. A New game Start needed.");
            // NewGame();
            return;
        } else {
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) {
                dataPersistenceObj.LoadData(gameData);
            }

            if (isNew || isStart) {
                Debug.Log("[Test] Load game when isNew");
                this.dataInitializations = FindAllDataInitializationObjects();
                foreach (IDataInitialization dataInitialization in dataInitializations) {
                    dataInitialization.LoadData(gameData);
                }
                isStart = false;
            }
        }
    }

    public bool CheckIsGameDataExist() {
        this.gameData = dataHandler.Load();

        if (this.gameData != null) {
            return true;
        } else {
            return false;
        }
    }

    public void SaveGame() {
        if (this.gameData == null) {
            Debug.LogWarning("No Data was found. A New Game needs to be started before save.");
            return;
        }

        // 다른 스크립트에서 데이터 가져옴 -> 저장
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) {
            dataPersistenceObj.SaveData(gameData);
        }

        if (isQuit || isNew || isStart) {
            // if (isDefaultInventory) { // @ check craft scene
            //     inventoryManager.InventoryItemListToDataList();
            // }

            Debug.Log("[Test] SaveGame() ");

            this.dataInitializations = FindAllDataInitializationObjects();
            foreach (IDataInitialization dataInitializationObj in dataInitializations) {
                dataInitializationObj.SaveData(gameData);
            }
            isQuit = false;
            isNew = false;
        }

        // 데이터 핸들러로 파일로 저장
        dataHandler.Save(gameData);
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects() {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    private List<IDataInitialization> FindAllDataInitializationObjects() {
        IEnumerable<IDataInitialization> dataInitializations = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataInitialization>();

        // Debug.Log("=== TEST ===");
        // foreach (IDataInitialization dataInitialization in dataInitializations) {
        //     Debug.Log("dataInitial.. : " + dataInitialization);
        // }

        return new List<IDataInitialization>(dataInitializations);

    }

}

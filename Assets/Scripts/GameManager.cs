using System;
using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //list of cache variables;
    private Coin[] foundCoins;
    private GameObject[] foundHearts01;
    private GameObject[] foundHearts02;
    private Gem[] foundGems;
    private GameObject[] foundSnakesIdle01;
    private GameObject[] foundSnakesIdle02;
    private GameObject[] foundSnakesMover01;
    private GameObject[] foundSnakesMover02;
    private Mashroom[] foundMashrooms;
    //End of list
    //List of important Object's vector3 default positions 
    private Vector3 v3_Human;
    private Vector3[] v3_Coins;
    private Vector3[] v3_Hearts01;
    private Vector3[] v3_Hearts02;
    private Vector3[] v3_SnakesIdle01;
    private Vector3[] v3_SnakesIdle02;
    private Vector3[] v3_SnakesMover01;
    private Vector3[] v3_SnakesMover02;
    private Vector3[] v3_Mashrooms;
    private Vector3[] v3_Gems;
    //End of list
    //List Of Important Prefabs
    public GameObject Owl;
    public GameObject Human;
    public GameObject CoinPrefab;
    public GameObject Heart01Prefab;
    public GameObject Heart02Prefab;
    public GameObject SnakeIdle01Prefab;
    public GameObject SnakeIdle02Prefab;
    public GameObject SnakeMover01Prefab;
    public GameObject SnakeMover02Prefab;
    public GameObject MashroomPrefab;
    public GameObject GemPrefab;
    public GameObject[] characters;
    public GameObject[] Doors;

    public bool isHuman;

    public PlayerStats playerStats;
    
    public Vector3 offset;
    public int activeStatueIndex = 0;
    private int activeCharacterIndex = 0;
    // number of statues needed for door to be activated.
    [HideInInspector]
    public int statueNeeded;
    private Transform _exampleCharacterCameraPos;

    [HideInInspector]
    public List<GameObject> deactivatedGems = new List<GameObject>();

    public void ReactivateGemForStatue()
    {
        if (deactivatedGems.Count > 0)
        {
            GameObject gemToReactivate = deactivatedGems[0];
            gemToReactivate.SetActive(true); // Reactivate the gem
            deactivatedGems.RemoveAt(0); // Remove it from the list
        }
    }

    void Awake()
    {
        // 
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()    
    {
        CacheAllImportantObjectsPositions();
        SaveInitialPositionsOfImportantObjects();
        Owl.SetActive(false);
        Human.SetActive(true);
        isHuman = true;
        _exampleCharacterCameraPos = FindFirstObjectByType<ExampleCharacterCamera>().transform;
    }
    public void Update()    
    {
        if (Input.GetKeyDown(KeyCode.Tab)) // Replace KeyCode.Tab with your preferred key
        {
            SwitchCharacter();
            UIManager.instance.UpdateHealth();
            isHuman = !isHuman;
        }

        if (activeStatueIndex == 3 || activeStatueIndex == 4)
        {
            OpenDoor(1);
            CloseDoor(2);
        }
        else if (activeStatueIndex == 5)
        {
            OpenDoor(2);
        }
        else
        {
            CloseDoor(1);
            CloseDoor(2);
        }

    }
     void SwitchCharacter()
    {
        // Disable the current character's controller
        characters[activeCharacterIndex].SetActive(false);
        
        UpdatePositions();

        // Increment the index and wrap it around if necessary
        activeCharacterIndex = (activeCharacterIndex + 1) % characters.Length;

        // Enable the next character's controller
        characters[activeCharacterIndex].SetActive(true);
    }
    void UpdatePositions()
    {
        Owl.transform.forward = _exampleCharacterCameraPos.transform.forward;
        Owl.transform.SetLocalPositionAndRotation(_exampleCharacterCameraPos.position + offset, _exampleCharacterCameraPos.rotation);
    }

    public void OpenDoor(int doorLevel)
    {
        if (doorLevel == 1) 
        {
            Doors[1 - 1].SetActive(false);
        }
        if (doorLevel == 2) 
        {
            Doors[2 - 1].SetActive(false);
        }
    }
    public bool IsDoorOpen(int doorLevel)
    {
        return Doors[doorLevel - 1].activeInHierarchy;
    }
    public void CloseDoor(int doorLevel)
    {
        if (doorLevel == 1) 
        {
            Doors[1 - 1].SetActive(true);
        }
        if (doorLevel == 2) 
        {
            Doors[2 - 1].SetActive(true);
        }
    }
    public void GameOver()
    {
        if(!isHuman)
        {
            SwitchCharacter();
            isHuman = true;
        }
        Time.timeScale = 0;
        UIManager.instance.isOnUI = true;
        UIManager.instance.DeadMenu.SetActive(true);
    }

    void CacheAllImportantObjectsPositions()
    {
        foundCoins = GameObject.FindObjectsByType<Coin>(FindObjectsSortMode.None);
        foundHearts01 = GameObject.FindGameObjectsWithTag("Heart01");
        foundHearts02 = GameObject.FindGameObjectsWithTag("Heart02");
        foundSnakesIdle01 = GameObject.FindGameObjectsWithTag("SnakeIdle01");
        foundSnakesIdle02 = GameObject.FindGameObjectsWithTag("SnakeIdle02");
        foundSnakesMover01 = GameObject.FindGameObjectsWithTag("SnakeMover01");
        foundSnakesMover02 = GameObject.FindGameObjectsWithTag("SnakeMover02");
        foundMashrooms = GameObject.FindObjectsByType<Mashroom>(FindObjectsSortMode.None);
        foundGems = GameObject.FindObjectsByType<Gem>(FindObjectsSortMode.None);
    }
    void SaveInitialPositionsOfImportantObjects()
    {
        v3_Human = Human.transform.Find("ExampleCharacter").transform.position;
        
        
        v3_Coins = new Vector3[foundCoins.Length];
        for (int i = 0; i < foundCoins.Length; i++)
        {
            v3_Coins[i] = foundCoins[i].transform.position;
        }
        v3_Hearts01 = new Vector3[foundHearts01.Length];
        for (int i = 0; i < foundHearts01.Length; i++)
        {
            v3_Hearts01[i] = foundHearts01[i].transform.position;
        }
        v3_Hearts02 = new Vector3[foundHearts02.Length];
        for (int i = 0; i < foundHearts02.Length; i++)
        {
            v3_Hearts02[i] = foundHearts02[i].transform.position;
        }
        v3_SnakesIdle01 = new Vector3[foundSnakesIdle01.Length];
        for (int i = 0; i < foundSnakesIdle01.Length; i++)
        {
            v3_SnakesIdle01[i] = foundSnakesIdle01[i].transform.position;
        } 

        v3_SnakesIdle02 = new Vector3[foundSnakesIdle02.Length];
        for (int i = 0; i < foundSnakesIdle02.Length; i++)
        {
            v3_SnakesIdle02[i] = foundSnakesIdle02[i].transform.position;
        } 

        v3_SnakesMover01 = new Vector3[foundSnakesMover01.Length];
        for (int i = 0; i < foundSnakesMover01.Length; i++)
        {
            v3_SnakesMover01[i] = foundSnakesMover01[i].transform.position;
        } 

        v3_SnakesMover02 = new Vector3[foundSnakesMover02.Length];
        for (int i = 0; i < foundSnakesMover02.Length; i++)
        {
            v3_SnakesMover02[i] = foundSnakesMover02[i].transform.position;
        } 
        
        v3_Mashrooms = new Vector3[foundMashrooms.Length];
        for (int i = 0; i < foundMashrooms.Length; i++)
        {
            v3_Mashrooms[i] = foundMashrooms[i].transform.position;
        }
        v3_Gems = new Vector3[foundGems.Length];
        for (int i = 0; i < foundGems.Length; i++)
        {
            v3_Gems[i] = foundGems[i].transform.position;
        }
    }
    void DeleteRemainingImportanObjectsInTheScene()
    {
        CacheAllImportantObjectsPositions();
        for (int i = 0; i < foundCoins.Length; i++)
        {
            Destroy(foundCoins[i].gameObject);
        }
        for (int i = 0; i < foundGems.Length; i++)
        {
            Destroy(foundGems[i].gameObject);
        }
        for (int i = 0; i < foundSnakesIdle01.Length; i++)
        {
            Destroy(foundSnakesIdle01[i]);
        }
        for (int i = 0; i < foundSnakesIdle02.Length; i++)
        {
            Destroy(foundSnakesIdle02[i]);
        }
        for (int i = 0; i < foundSnakesMover01.Length; i++)
        {
            Destroy(foundSnakesMover01[i]);
        }
        for (int i = 0; i < foundSnakesMover02.Length; i++)
        {
            Destroy(foundSnakesMover02[i]);
        }
        for (int i = 0; i < foundMashrooms.Length; i++)
        {
            Destroy(foundMashrooms[i].gameObject);
        }
        for (int i = 0; i < foundHearts01.Length; i++)
        {
            Destroy(foundHearts01[i]);
        }
        for (int i = 0; i < foundHearts02.Length; i++)
        {
            Destroy(foundHearts02[i]);
        }
    }
    void InstantiateImportantObjectsToDefaultPositions()
    {
        for (int i = 0; i < v3_Coins.Length; i++)
        {
            GameObject CreatedCoins = Instantiate(CoinPrefab, v3_Coins[i], Quaternion.identity);
            CreatedCoins.layer = LayerMask.NameToLayer("Coin");
        }
        for (int i = 0; i < v3_Gems.Length; i++)
        {
            Instantiate(GemPrefab, v3_Gems[i], Quaternion.identity);
        }
        for (int i = 0; i < v3_SnakesIdle01.Length; i++)
        {
            Instantiate(SnakeIdle01Prefab, v3_SnakesIdle01[i], Quaternion.identity);
        }
        for (int i = 0; i < v3_SnakesIdle02.Length; i++)
        {
            Instantiate(SnakeIdle02Prefab, v3_SnakesIdle02[i], Quaternion.identity);
        }
        for (int i = 0; i < v3_SnakesMover01.Length; i++)
        {
            Instantiate(SnakeMover01Prefab, v3_SnakesMover01[i], Quaternion.identity);
        }
        for (int i = 0; i < v3_SnakesMover02.Length; i++)
        {
            Instantiate(SnakeMover02Prefab, v3_SnakesMover02[i], Quaternion.identity);
        }
        for (int i = 0; i < v3_Hearts01.Length; i++)
        {
            Instantiate(Heart01Prefab, v3_Hearts01[i], Quaternion.identity);
        }
        for (int i = 0; i < v3_Hearts02.Length; i++)
        {
            Instantiate(Heart02Prefab, v3_Hearts02[i], Quaternion.identity);
        }
        for (int i = 0; i < v3_Mashrooms.Length; i++)
        {
            Instantiate(MashroomPrefab, v3_Mashrooms[i], Quaternion.identity);
        }
    }
    public void RestartGame()
    {
        UIManager.instance.isOnUI = false;
        UIManager.instance.isGamePaused = false;
        DeleteRemainingImportanObjectsInTheScene();
        InstantiateImportantObjectsToDefaultPositions();
        playerStats.SetHealth(playerStats.MaxHealth);
        playerStats._ishitBySnake = false;
        playerStats._isJustTookHeart = false;
        FindFirstObjectByType<KinematicCharacterMotor>().SetPosition(v3_Human);
        UIManager.instance.isOnUI = false;
        UIManager.instance.DeadMenu.SetActive(false);
        InventoryManager.instance.NumberofGems = 0;
        InventoryManager.instance.NumberOfGolds = 0;
        UIManager.instance.UpdateGold();
        UIManager.instance.UpdateGems();
        UIManager.instance.UpdateHealth();
        Time.timeScale = 1;
    }
}

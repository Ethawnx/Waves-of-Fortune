using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public TMP_Text Txt_GemCounter;
    public TMP_Text Txt_CoinCounter;
    public Slider HealthCounter;
    public GameObject StatueUI;
    public GameObject PauseUI;
    public GameObject MainMenu;
    public GameObject DeadMenu;
    [Header("Statue Sliders")]
    public GameObject RedFireSlider;
    public GameObject BlueFireSlider;
    public GameObject WhiteFireSlider;

    public PlayerStats playerStats;

    public bool isGamePaused = false;
    public bool isOnUI = true;

    private Statue[] _statues;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        _statues = FindObjectsByType<Statue>(FindObjectsSortMode.None);

    }
    void Start()
    {
        RedFireSlider.SetActive(false);
        BlueFireSlider.SetActive(false);
        WhiteFireSlider.SetActive(false);

        UpdateActivatedStatueUI();
        UpdateHealth();
        UpdateGold();
        UpdateGems();
        MainMenu.SetActive(true);
    }

    void Update()
    {
        UpdateActivatedStatueUI();
        if (Input.GetKeyDown(KeyCode.Escape) && !isOnUI)
        {
            if (!isGamePaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
        if (isOnUI)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    public void UpdateActivatedStatueUI()
    {
        int activeNumbers = 0;
        foreach (Statue _statu in _statues)
        {
            if (_statu.IsActivated)
            {
                if (_statu.Color == "Red")
                {
                    activeNumbers++;
                    Slider _slider = RedFireSlider.GetComponent<Slider>();
                    _slider.maxValue = _statu.ActivatedTime;
                    RedFireSlider.SetActive(true);
                    if (_slider.value <= 0f)
                    {
                        _slider.value = _slider.maxValue;
                        RedFireSlider.SetActive(false);
                    }
                }
                else if (_statu.Color == "Blue")
                {
                    activeNumbers++;
                    Slider _slider = BlueFireSlider.GetComponent<Slider>();
                    _slider.maxValue = _statu.ActivatedTime;
                    BlueFireSlider.SetActive(true);
                    if (_slider.value <= 0f)
                    {
                        _slider.value = _slider.maxValue;
                        BlueFireSlider.SetActive(false);
                    }
                }
                else if (_statu.Color == "Green")
                {
                    activeNumbers++;
                }
                else if (_statu.Color == "White")
                {
                    activeNumbers++;
                }
                else if (_statu.Color == "Black")
                {
                    activeNumbers++;
                }
                else
                {
                    Debug.LogWarning("There is not a suitable statue color match, check the color statu value");
                }
            }
        }
        Debug.Log(_statues.Length);
        Debug.Log(activeNumbers);
    }
    public void UpdateGems()
    {
        Txt_GemCounter.text = "X" + InventoryManager.instance.NumberofGems;
    }
    public void UpdateHealth()
    {
        HealthCounter.value = playerStats.GetCurrentHealth();
    }
    public void UpdateGold()
    {
        Txt_CoinCounter.text = "X" + InventoryManager.instance.NumberOfGolds;
    }
    public IEnumerator UpdateStatuUI(float time, bool isError)
    {
        StatueUI.SetActive(true);
        TMP_Text statuText = StatueUI.GetComponentInChildren<TMP_Text>();
        if (isError)
        {
            statuText.color = Color.red;
            statuText.text = "Need Gem to Activate";
        }
        else
        {
            statuText.color = Color.green;
            statuText.text = "Activated for " + time + " Seconds";

        }
        yield return new WaitForSeconds(2f);
        StatueUI.SetActive(false);
    }
    public void Pause()
    {
        isOnUI = true;
        PauseUI.SetActive(true);
        Time.timeScale = 0;
        isGamePaused = true;
    }
    public void Resume()
    {
        isOnUI = false;
        PauseUI.SetActive(false);
        Time.timeScale = 1;
        isGamePaused = false;
    }
    public void StartGame()
    {
        Resume();
        GameManager.instance.Owl.SetActive(false);
        GameManager.instance.Human.SetActive(true);
    }
    public void Exit()
    {
        Application.Quit();
    }
}

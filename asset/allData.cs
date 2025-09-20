using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class allData : MonoBehaviour
{
    [Header("Time out")]
    public float time;
    public float currentTime;
    [Space(10)]
    public string nameU;
    public static string nameUser;
    public static bool start;
    public string language;
    public string holdSelectWay;
    public string pointStartCity, pointEndCity;
    public string pointSubStartCity, pointSubEndCity;
    [Header("check joy connect")]
    public bool joyConnected;
    public string[] joy_connected, joy_disconnected;
    public GUIStyle guiStyle;
    private string output = "";
    private string stack = "";
    [Header("audio")]
    public GameObject audioCarStart;
    // joy connect
    // joy disconnect
    // Joystick reconnected ("Controller (PXN-V3II)").
    // Joystick disconnected ("Controller (PXN-V3II)").

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        for(int i = 0; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }

        StartCoroutine(checkJoyConnect());
    }

    public void audioStartCar()
    {
        if(GameObject.Find("car start") == null)
        {
            GameObject obj = Instantiate(audioCarStart, transform.localPosition, Quaternion.identity);
            obj.name = "car start";
        }
    }


    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }
    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        output = logString;
        stack = stackTrace;

        if (output == joy_disconnected[0] || output == joy_disconnected[1])
        {
            joyConnected = false;
            Debug.Log(string.Format("{0}", output));
        }
        else if (output == joy_connected[0] || output == joy_connected[1])
        {
            joyConnected = true;
            Debug.Log(string.Format("{0}", output));
        }
    }
    IEnumerator checkJoyConnect()
    {
        while (!joyConnected)
        {
            yield return new WaitForSeconds(2f);


            string[] temp = Input.GetJoystickNames();

            if (temp.Length > 0)
            {
                for (int i = 0; i < temp.Length; i++)
                {
                    if (temp[i] != "")
                    {
                        joyConnected = true;
                        // print("joy connect");
                    }

                    if (!joyConnected)
                    {
                        // print("joy disconnect");
                    }
                }
            }
        }
    }
    private void OnGUI()
    {
        if (!joyConnected)
        {
            GUI.Label(new Rect(20, 20, 100, 20), "joy not connect", guiStyle);
        }
    }





    private void Update()
    {
        nameU = nameUser;
        if (!start)
        {
            if(Input.GetKey(KeyCode.JoystickButton8) &&
                Input.GetKey(KeyCode.JoystickButton9) &&
                Input.GetKey(KeyCode.JoystickButton5))
            {
                print("start");
                getCard.gameStart();
                audioStartCar();
            }

            if (controlScene1.statusPage == null) controlScene1.statusPage = "language";

            //start = getCard.start;
        }
        else
        {
            // time run
            currentTime += Time.deltaTime;

            if (currentTime >= time)
            {
                //resetGame();
                resetAllData();
                //btLeaveGame.SetActive(false);
                //panelLeaveGame.SetActive(false);
                try
                {
                    GameObject.Find("mainScripts").GetComponent<controlScene2>().btSubmitLeaveGame();
                }
                catch
                {
                    Debug.Log("this scene 1");
                }

                SceneManager.LoadScene(0);

                //_controllerPlayer.resetControllerPlayer();
                allData.start = false;
                if(GameObject.Find("mainScripts").GetComponent<GameObject>() != null)
                {
                    controllerPlayer.passQuestion = 0;
                }
            }

            gearFunction();
        }

    }


    public void resetAllData()
    {
        currentTime = 0;
        nameUser = "";
        start = false;
        language = "";
        holdSelectWay = "";
        pointStartCity = "";
        pointSubStartCity = "";
        pointEndCity = "";
        pointSubEndCity = "";
    }


    //public void resetGame()
    //{
    //    leaveGame();

    //    getCard.gameEnd();
    //    start = getCard.start;
    //    //resetObjectScene1.resetCheckCard();
    //}

    public void leaveGame()
    {
        //resetObjectScene1.reset();
        controlScene1.statusPage = "language";

        language = "";
        holdSelectWay = "";
        pointStartCity = "";
        pointSubStartCity = "";
        pointEndCity = "";
        pointSubEndCity = "";
        currentTime = 0;
    }

    void gearFunction()
    {
        /// function touch monitor
        if (Input.GetMouseButtonDown(0)) currentTime = 0;

        /// function press button controller gear
        if (Input.GetKeyDown(KeyCode.JoystickButton0) ||
            Input.GetKeyDown(KeyCode.JoystickButton1) ||
            Input.GetKeyDown(KeyCode.JoystickButton2) ||
            Input.GetKeyDown(KeyCode.JoystickButton3) ||
            Input.GetKeyDown(KeyCode.JoystickButton4) ||
            Input.GetKeyDown(KeyCode.JoystickButton5) ||
            Input.GetKeyDown(KeyCode.JoystickButton6) ||
            Input.GetKeyDown(KeyCode.JoystickButton7) ||
            Input.GetKeyDown(KeyCode.JoystickButton8) || 
            Input.GetKeyDown(KeyCode.JoystickButton9))
            currentTime = 0;

        /// function controller arrow
        Vector3 inputDirection = Vector3.zero;
        inputDirection.x = Input.GetAxis("arrowLeftRight");
        //Debug.Log("arrow left : -1, arrow right : 1,   " + inputDirection.x);
        inputDirection.y = Input.GetAxis("arrowUpDown");
        //Debug.Log("arrow up : 1, arrow down : -1,   " + inputDirection.y);
        if(inputDirection.x != 0 || inputDirection.y != 0)
            currentTime = 0;

        /// function controller turn steering & gas pedal
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        if (v != 0)
            currentTime = 0;

        if (h != 0)
            currentTime = 0;
    }
}

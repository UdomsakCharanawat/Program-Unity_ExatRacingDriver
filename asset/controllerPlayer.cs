using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class controllerPlayer : MonoBehaviour
{
    public controlScene2 _controlScene2;

    public float speed;
    private float _speed, _tempSpeed = 60f;
    public bool tempPlay;
    public static bool play;
    public Transform transformPlayer, turnPlayer;
    public float currentTime;
    public float posXLeft, posXRight;
    public float pressGasPedal, multiplyGasPedal;

     [Header("console")]
    public Text minutesText;
    public Text secondsText;
    public Image fillSpeed;
    public float minFill, maxFill;
    public Text mileSpeed;

    [Header("pass question")]
    public int temp_passQuestion;
    public static int passQuestion;

    [Header("create car enemy")]
    public GameObject prefab_enemyCar;
    public Transform enemyCar;
    [Range(-6.5f,6.5f)]
    public float posX_left, posX_right;
    public int[] posZ;

    [Header("car crash")]
    public GameObject tempCanvas_carCrash;
    public static GameObject canvas_carCrash;
    public static GameObject tempDestroyObj;

    public bool rotateTurnSteering;
    public string rotate;
    public float h = 0;

    private void Start()
    {
        canvas_carCrash = tempCanvas_carCrash;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            play = true;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            play = false;
        }

        if (tempPlay != play) tempPlay = play;
        if (temp_passQuestion != passQuestion) temp_passQuestion = passQuestion;

        if (play)
        {
            var tempSpeed = (speed - minFill) / (maxFill - minFill);
            fillSpeed.fillAmount = tempSpeed;
            mileSpeed.text = speed.ToString("00");

            //audio.volume = (allData.speed) / (maxFill - 20);


            transformPlayer.Translate(Vector3.forward * speed * Time.deltaTime);

            currentTime = currentTime + Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(currentTime);
            minutesText.text = time.Minutes.ToString("00");
            secondsText.text = time.Seconds.ToString("00");

            //collect.timePlay = (int)currentTime;

            

            controlTurnSteeringPlayer();
            controlPressGasPedal();
        }

        //if (currentTime >= 10)
        //{
        //    allData _allData = GameObject.Find("allData").GetComponent<allData>();
        //    _allData.currentTime = 0;
        //    _allData.language = "";
        //    _allData.holdSelectWay = "";
        //    _allData.pointStartCity = "";
        //    _allData.pointSubStartCity = "";
        //    _allData.pointEndCity = "";
        //    _allData.pointSubEndCity = "";
        //    allData.start = false;
        //    //getCard.gameEnd();
        //    //getCard _getCard = GameObject.Find("ScriptsCard").GetComponent<getCard>();
        //    //_getCard.fieldIdCard.ActivateInputField();
        //    UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        //    //GameObject.Find("allData").GetComponent<allData>().resetGame();
        //}
    }

    public void resetControllerPlayer()
    {
        speed = 0;
        play = false;
        currentTime = 0;
        passQuestion = 0;
    }


    void controlTurnSteeringPlayer()
    {
        if (rotateTurnSteering)
        {
            var tempRotate = turnSteering();
            if (tempRotate == "right")
            {
                turnPlayer.transform.localPosition =
                                   Vector3.Lerp(turnPlayer.transform.localPosition,
                                   new Vector3(posXRight, turnPlayer.transform.localPosition.y, turnPlayer.transform.localPosition.z),
                                   3f * h * Time.deltaTime);

                if (Mathf.Abs(turnPlayer.transform.localPosition.x - posXRight) <= 0.01f)
                {
                    turnPlayer.transform.localPosition =
                        new Vector3(posXRight, turnPlayer.transform.localPosition.y, turnPlayer.transform.localPosition.z);
                    //rotate = "stay";
                }
                //print("turn right");
            }
            else if (tempRotate == "left")
            {
                turnPlayer.transform.localPosition =
                                Vector3.Lerp(turnPlayer.transform.localPosition,
                                new Vector3(posXLeft, turnPlayer.transform.localPosition.y, turnPlayer.transform.localPosition.z),
                                3f * -h * Time.deltaTime);

                if (Mathf.Abs(turnPlayer.transform.localPosition.x - posXLeft) <= 0.01f)
                {
                    turnPlayer.transform.localPosition =
                        new Vector3(posXLeft, turnPlayer.transform.localPosition.y, turnPlayer.transform.localPosition.z);
                    //rotate = "stay";
                }
                //print("turn left");
            }
        }
        else
        {
            turnSteering();
        }
    }
    string turnSteering()
    {
        h = Input.GetAxis("Horizontal");

        if (h > 0.01)
        {
            if (!rotateTurnSteering)
            {
                rotateTurnSteering = true;
                rotate = "right";
            }
        }
        else if (h < -0.01)
        {
            if (!rotateTurnSteering)
            {
                rotateTurnSteering = true;
                rotate = "left";
            }
        }
        else
        {
            rotate = "stay";
            rotateTurnSteering = false;
        }

        return rotate;
    }
    void controlPressGasPedal()
    {
        float v = Input.GetAxis("Vertical");
        if (v > 0.1) pressGasPedal = v;
        else pressGasPedal = 0;

        speed = _tempSpeed + (pressGasPedal * multiplyGasPedal);
        if (_speed != speed) _speed = speed;
    }


    public void creatEnemyCar()
    {
        int setMin = 0;
        int setMax = 0;
        if (passQuestion == 0)
        {
            setMin = 0;
            setMax = 5;
        }
        else if (passQuestion == 1)
        {
            setMin = 5;
            setMax = 10;
        }
        else if (passQuestion == 2)
        {
            setMin = 10;
            setMax = 15;
        }
        else if (passQuestion == 3)
        {
            setMin = 15;
            setMax = 20;
        }
        else if (passQuestion == 4)
        {
            setMin = 20;
            setMax = 25;
        }
        else if (passQuestion == 5)
        {
            setMin = 25;
            setMax = 30;
        }
        else if (passQuestion == 6)
        {
            setMin = 30;
            setMax = 35;
        }


        for (int i = setMin; i < setMax; i++)
        {
            var ranPos = UnityEngine.Random.Range(0, 2);
            var tempPos = 0f;
            GameObject enemy = Instantiate(prefab_enemyCar, transform.localPosition, Quaternion.identity);
            enemy.name = "enemy car";
            if (ranPos == 0) tempPos = posX_left;
            else tempPos = posX_right;
            enemy.transform.parent = enemyCar.transform;
            enemy.transform.localPosition = new Vector3(tempPos, 0, posZ[i]);
        }
    }


}

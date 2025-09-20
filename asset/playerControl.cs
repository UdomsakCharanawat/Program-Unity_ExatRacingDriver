using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour
{
    public controllerPlayer _controllerPlayer;
    public createQuestion _createQuestion;
    public controlScene2 _controlScene2;
    public GameObject symCall1543, symEmergency, symAppSos, symRepair;
    public GameObject timeMinus10, timePlus3;
    public GameObject playerWarningSmoke, playerWarningSmokeAllinone;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "create enemy")
        {
            _controllerPlayer.creatEnemyCar();
        }
        else if (other.tag == "create question")
        {
            controllerPlayer.passQuestion++;
            _createQuestion.question(controllerPlayer.passQuestion);
        }
        else if (other.tag == "question")
        {
            controllerPlayer.play = false;
        }
        else if (other.tag == "Finish")
        {
            //_controlScene2.btOptionControlLeaveGame();
            //_controlScene2.btSubmitLeaveGame();
            controllerPlayer.play = false;
            _controlScene2.panelCongreat.SetActive(true);
            controlScene2.statusPage = "congreat";
        }
    }
}

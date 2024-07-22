using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class MultiplayerManager : MonoBehaviour
{
    public List<GameObject> players;
    public List<Transform> spawnPoint;
    public bool settingUpPlayer = true;
    public float timer;
    public TextMeshProUGUI timerC;
    public TextMeshProUGUI centerText;
    public TuileManager tuileManager;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(tuileManager.isPicking)
        {

          
            foreach (GameObject car in players)
            {
                if (car.GetComponent<PlayerInput>().currentActionMap.name != "PickTiles")
                {
                    Debug.Log(car.GetComponent<PlayerInput>().currentActionMap);
                    car.GetComponent<PlayerInput>().SwitchCurrentActionMap("PickTiles");

                }
            }
        
        }
        if(!settingUpPlayer)
        {
            timer += Time.deltaTime;
        }
        timerC.text = (Mathf.Round(timer * 100f) / 100f).ToString() ;
    }

    public void StartRaceM()
    {
        timer = 0;
        foreach(GameObject car in players) 
        {
                car.GetComponent<PlayerInput>().SwitchCurrentActionMap("Car");
        }
        settingUpPlayer = false;
        timerC.gameObject.SetActive(true);
        centerText.text = "";
    }


    public void Victory(GameObject winner)
    {
        settingUpPlayer = true;
        foreach (GameObject car in players)
        {

            car.GetComponent<PlayerInput>().SwitchCurrentActionMap("Vide");
        }

        centerText.text = "Le joueur " + (players.IndexOf(winner)+(int)1) + " a gagné !";

        Invoke("StartPick", 2f);
    }

    void StartPick()
    {
        centerText.text = "";
        tuileManager.StartPicking();
        TPBack();

    }
    public void TPBack()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].transform.position = spawnPoint[i].position;
            players[i].transform.rotation = spawnPoint[i].rotation;
        }

        foreach (GameObject car in players)
        {

            car.GetComponent<PlayerInput>().SwitchCurrentActionMap("BeforeRace");
        }
    }

}

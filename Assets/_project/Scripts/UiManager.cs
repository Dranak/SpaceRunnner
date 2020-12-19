using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{

    public TextMeshProUGUI TextChrono;
    public Image EnergyFilling;
    public Button Button;
    public GameObject Title;
    public GameObject BG;


    public  static UiManager Instance;
  

    // Start is called before the first frame update
    void Start()
    {
        Instance = Instance ?? this;
        TextChrono.transform.parent.gameObject.SetActive(false);
        EnergyFilling.transform.parent.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateChrono(float chrono)
    {
       TextChrono.text=  TimeSpan.FromSeconds(chrono).ToString(@"mm\:ss");
    }

    public void UpdateEnergy(float ratio)
    {
        EnergyFilling.fillAmount = ratio;
    }

    public void LetsGo()
    {
        TextChrono.transform.parent.gameObject.SetActive(true);
        EnergyFilling.transform.parent.gameObject.SetActive(true);
        Button.gameObject.SetActive(false);
        Title.gameObject.SetActive(false);
        BG.gameObject.SetActive(false);
        GameManager.Instance.Player.gameObject.SetActive(true);
        //LevelSpline.Instance.StartSpawnBlock();
        GameManager.Instance.GameState = GameState.Game;
        Time.timeScale =1;
       
    }
}

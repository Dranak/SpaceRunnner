using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState GameState { get; set; }
    public CinemachineVirtualCamera Vcam;

    public GameObject Starfield;
    private float _zStartStarField;
    public SpaceShipController Player;
    public Level Level;
    public LevelSpline LevelSpline;
    public float MaxZStarfield;


    private float _chronoQuest;


    private void Awake()
    {
        Instance = Instance ?? this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Player.OnSpeedUp += Player_OnSpeedUp;
        Player.OnDeath += Player_OnDeath;
        Player.NextLevel += Player_NextLevel;
        _zStartStarField = Starfield.transform.position.z;
        GameState = GameState.MainMenu;
       
    }

    private void Player_NextLevel()
    {
         Debug.Log("YOU PASS THE LEVEL!");
    }

    private void Player_OnDeath()
    {
        Debug.Log("YOU ARE DEAD!");
       // Time.timeScale = 0f;
    }

    private void Player_OnSpeedUp(float _ratio)
    {
        if(Starfield.transform.position.z < Mathf.Lerp(_zStartStarField, MaxZStarfield, _ratio))
             Starfield.transform.Translate(Vector3.forward * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
      
    }







}



public enum GameState
{
    MainMenu,
    Game,
    End
}
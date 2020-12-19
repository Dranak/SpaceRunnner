﻿using SplineMesh;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpaceShipController : MonoBehaviour
{
    public event Action<float> OnSpeedUp;
    public event Action<float> OnSpeedDown;
    public event Action OnDeath;
    public event Action NextLevel;

    public int MinFov;
    public int MaxFov;
    private int _fov;

    public float MaxEnergy;
    private float _actualEnergy = 0;

    public Vector2 SpeedXY;
    public float MinForwardSpeed;
    public float _actualForwardSpeed = 0f;
    public float MaxForwardSpeed;

    Rigidbody _rigidbody = null;
    Vector3 ScreenPos;
    private float _deltaX = 0f;
    private float _deltaY = 0f;
    private float _lastAngle = 0f;

    [Header("Bound")]

    public float rotationMax = 45;
    private Vector3 appliedForce;
    private Vector3 appliedAngularForce;

    private Queue<Vector3> _pointsToFollow;
    private Vector3 _pointFollowing;

    public Vector3 Boundaries { get; set; }



    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

    }

    private void Start()
    {
        _fov = MinFov;
        Camera.main.fieldOfView = _fov;
        _actualForwardSpeed = MinForwardSpeed;
        _pointsToFollow = new Queue<Vector3>();
    }

    void Update()
    {
        if (GameManager.Instance.GameState == GameState.Game)
        {
            _deltaY = Input.GetAxis("Vertical");
            _deltaX = Input.GetAxis("Horizontal");

            if (_pointsToFollow.Count == 0)
            {
                foreach (SplineNode point in LevelSpline.Instance?.ActualBlock.Points)
                {
                 
                    _pointsToFollow.Enqueue(point.Position);
                }

            }

        }

    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.GameState == GameState.Game)
        {
            MoveRotationSpaceshipPhysic();
        }
    }



    void MoveRotationSpaceshipPhysic()
    {

        if (_rigidbody != null)
        {
            appliedForce.x = _deltaX * SpeedXY.x;
            appliedForce.y = _deltaY * SpeedXY.y;
            appliedForce.z = _actualForwardSpeed;

            appliedAngularForce.x += rotationMax * -_deltaY;
            appliedAngularForce.y = 0;
            appliedAngularForce.z += rotationMax * -_deltaX;

            //float angle = 0f;
            if (_pointsToFollow.Count > 0  )
            {
                if(Vector3.Distance(_rigidbody.position, _pointFollowing) <= 0 )
                {
                    _pointFollowing = _pointsToFollow.Dequeue();
                    _rigidbody.rotation = Quaternion.Euler(0,0,Vector3.Angle(_pointFollowing, Vector3.forward));// Quaternion.LookRotation(_pointFollowing-transform.position);
                }
          

            }
            _rigidbody.velocity = appliedForce;
           // _rigidbody.MovePosition(_rigidbody.position+transform.forward * _actualForwardSpeed*Time.fixedDeltaTime);

            _rigidbody.rotation = Quaternion.Euler(rotationMax * -_deltaY, 0, rotationMax * -_deltaX);



        }
    }

    //void RotationShipPhysic()
    //{
    //    transform.rotation = Quaternion.Euler(rotationMax * -_deltaY, 0, rotationMax * -_deltaX);
    //}


    private void OnTriggerEnter(Collider other)
    {
        Element element = other.GetComponent<Element>();
        if (element)
        {
            _actualEnergy += element.EnergyGain;
            float ratio = _actualEnergy;
            Debug.Log("ratio1 " + _actualEnergy);
            UiManager.Instance.UpdateEnergy(_actualEnergy);

            if (element.EnergyGain < 0)
            {
                //if (_actualEnergy < 0)
                //    //OnDeath.Invoke();
            }
            else
            {
                Level.Instance.ActualBlock.SelectedPattern.DesactiveElement(element);
                //if (_actualEnergy >= MaxEnergy)
                //{
                //   // NextLevel.Invoke();
                //}
            }
            Debug.Log("ratio2 " + ratio);
            GameManager.Instance.Vcam.m_Lens.FieldOfView = Mathf.Lerp(MinFov, MaxFov, _actualEnergy);
            _actualForwardSpeed = Mathf.Lerp(MinForwardSpeed, MaxForwardSpeed, _actualEnergy);
            OnSpeedUp.Invoke(_actualEnergy);
        }
    }





}


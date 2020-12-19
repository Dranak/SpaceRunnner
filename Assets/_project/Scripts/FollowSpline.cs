using SplineMesh;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowSpline : MonoBehaviour
{
    public int MinFov;
    public int MaxFov;
    private int _fov;
    public float MinForwardSpeed;
    public float _actualForwardSpeed = 0f;
    public float MaxForwardSpeed;

    SplineNode _splineNodeA;
    SplineNode _splineNodeB;
    private float _distance;
    private Vector3 _pointFollowing;


    private void Start()
    {
        _fov = MinFov;
        Camera.main.fieldOfView = _fov;
        _actualForwardSpeed = MinForwardSpeed;
        _splineNodeA = null;
        _splineNodeB = null;
    }

    private void Update()
    {
       
        if(LevelSpline.Instance.Spline.nodes[0] != _splineNodeA)
        {
            _splineNodeA = LevelSpline.Instance.Spline.nodes[0];
            _splineNodeB = LevelSpline.Instance.Spline.nodes[1];

            _distance = Vector3.Distance(_splineNodeA.Position, _splineNodeB.Position);
            transform.LookAt(_splineNodeB.Position);
        }
       
            float ratio = Vector3.Distance(transform.position, _splineNodeB.Position) / _distance;
            if (ratio<1f)
            {
                transform.position += transform.forward * _actualForwardSpeed * Time.deltaTime;
            }
            else
            {
                LevelSpline.Instance.Spline.RemoveNode(_splineNodeA);   
            }
        

    }

}

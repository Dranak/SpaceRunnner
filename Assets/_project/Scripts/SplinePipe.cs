using SplineMesh;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SplinePipe : MonoBehaviour
{

    public string Name;
    public float PartternSpacing;
    public Vector2Int RangeNumberOfPattern;
    public float MaxSizeY;
    public float NumberOfSplinePoint;
    public bool WorldPosition;
    public AnimationCurve Curve = null;
    //public LineRenderer LineRenderer;

    //public PipeMeshGenerator PipeMeshGenerator;

    //public Spline Spline;
    //public SplineMeshTiling SplineMesh;
    public int NumberOfPattern { get; set; }
    public int NumberOfCollectibles { get; set; }
    // public Collider Volume;
    public Border BorderStart;
    public Border BorderEnd;
    public List<Pattern> Patterns;
    public Pattern SelectedPattern { get; set; }
    float _lengh;
    public Dictionary<string, Pooller> Poolers { get; set; }
    public List<SplineNode> Points { get; set; }
    public Vector3 Offset { get; set; }

    private void Awake()
    {
        Points = new List<SplineNode>();
        Poolers = new Dictionary<string, Pooller>();
        Patterns.ForEach(p => Poolers.Add(p.Name, new Pooller(p.SizePooller, p.gameObject)));
        //GenerateValue();
    }




    public Vector3 GetPositionWorldFromCurve(float input)
    {
        Vector3 pos = transform.position;
        pos.z += Mathf.Lerp(0, _lengh, input);

        pos.y += Curve.Evaluate(input) * MaxSizeY;

        return pos;
    }


    //public Vector3 GetPositionPatternFromCurve(float input)
    //{
    //    Vector3 pos = Vector3.zero;
    //    pos.z = Mathf.Lerp(PartternSpacing, _lengh, input);

    //    pos.y = Curve.Evaluate(input) * MaxSizeY;

    //    return transform.TransformPoint(pos);
    //}


    void GeneratePoints(bool world = false)
    {
        int i = 0;

        foreach (Keyframe key in Curve.keys)
        {
            Vector3 position = GetPositionWorldFromCurve(key.time);
            var worldTangent = Quaternion.Euler(key.outTangent, 0, 0) * Vector3.forward;
            var worldWeight = Mathf.Max(key.outWeight, key.inWeight) * _lengh;

            Points.Add(new SplineNode(position, position + worldTangent * worldWeight));
            ++i;
        }


    }

    public void UpdateLastPoint(SplineNode node)
    {
        //BorderEnd.transform.position = node.Position;
        //Spline.nodes.Last().Position = pos;
        //BorderEnd.transform.rotation = Quaternion.LookRotation(node.Direction);

        //Keyframe key = Curve.keys[Curve.keys.Count() - 1];
        //var worldTangent = Quaternion.Euler(key.outTangent, 0, 0) * Vector3.forward;
        //var worldWeight = key.outWeight * _lengh;


        //SplineMesh.CreateMeshes();
    }


    public void ClearObstacles()
    {
        List<Pattern> gos = new List<Pattern>();
        for (int i = 0; i < transform.childCount; ++i)
        {
            gos.Add(transform.GetChild(i).GetComponent<Pattern>());
        }
        gos = gos.Where(go => go != null).ToList();
        gos.ForEach(go => Poolers[gos[0].Name].ReturnToPool(go.gameObject));
        Curve = null;
    }

    public void FillVolume()
    {
        CalculatePostionPattern();
    }

    void CalculatePostionPattern()
    {
        Physics.SyncTransforms();


        Vector3 lastPosition = Vector3.zero;
        float offset = _lengh / NumberOfPattern;
        for (int index = 0; index <= NumberOfPattern; ++index)
        {
            SelectedPattern = Poolers[Patterns[Random.Range(0, Patterns.Count)].Name].GetObject().GetComponent<Pattern>();
            SelectedPattern.transform.parent = transform;
            //SelectedPattern.transform.position = GetPositionPatternFromCurve(index / NumberOfPattern);
            lastPosition = SelectedPattern.transform.position;
            SelectedPattern.LoadCollectibles();
        }

    }

    public void SetCurve(AnimationCurve _curve)
    {

        Curve = _curve;
        GenerateValue();
        GeneratePoints(WorldPosition);
        BorderStart.transform.position = Points[0].Position;
        BorderEnd.transform.position = Points.Last().Position;

    }

    void GenerateValue()
    {
        NumberOfPattern = Random.Range(RangeNumberOfPattern.x, RangeNumberOfPattern.y);
        _lengh = PartternSpacing * NumberOfPattern /*+ 2 * PartternSpacing*/;
        MaxSizeY = _lengh / 2;

    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    public string Name;
    public float PartternSpacing;
    public bool CanRotate;
    public MeshRenderer Pipe;
    //[MinMaxSlider(5, 100)]
    //public Vector2Int RangeNumberOfParttern;
    public int NumberOfPattern { get; set; }
    public int NumberOfCollectibles { get; set; }
    // public Collider Volume;
    public Border BorderStart;
    public Border BorderEnd ;
    public List<Pattern> Patterns;
    public Pattern SelectedPattern { get; set; }
    float _lengh;
    public Dictionary<string, Pooller> Poolers { get; set; }




    public void Awake()
    {
        Poolers = new Dictionary<string, Pooller>();
        Patterns.ForEach(p => Poolers.Add(p.Name, new Pooller(p.SizePooller, p.gameObject)));
        _lengh = Mathf.Abs( Vector3.Distance(BorderStart.transform.position, BorderEnd.transform.position));
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
    }

    public void FillVolume()
    {
       
        CalculatePostionPattern();
   

    }

    void CalculatePostionPattern()
    {
        Physics.SyncTransforms();
        NumberOfPattern = (int)(_lengh/ PartternSpacing);
      
        Vector3 lastPosition = Vector3.zero;
        float offset = _lengh / NumberOfPattern;
        for (int index = 0; index < NumberOfPattern; ++index)
        {

            SelectedPattern = Poolers[Patterns[Random.Range(0, Patterns.Count)].Name].GetObject().GetComponent<Pattern>();
            SelectedPattern.transform.parent = transform;
            if(lastPosition == Vector3.zero)
            {
                SelectedPattern.transform.position = new Vector3(0, 0, BorderStart.transform.position.z+ PartternSpacing); 
            }
            else
            {
                SelectedPattern.transform.position = new Vector3(0, 0, lastPosition.z + PartternSpacing);
            }

            lastPosition = SelectedPattern.transform.position;
            SelectedPattern.LoadCollectibles();
        }

    }




    //List<int> RepartCristal()
    //{
    //    int numberRate = (int)(CristalRate * NumberOfElement);
    //    List<int> cristalPoints = new List<int>();
    //    for (int index = 0; index < numberRate; ++index)
    //    {
    //        int randomIndex = 0;
    //        do
    //        {
    //            randomIndex = Random.Range(0, NumberOfElement);

    //        } while (cristalPoints.Contains(randomIndex));

    //        cristalPoints.Add(randomIndex);

    //    }

    //    return cristalPoints;
    //}


}

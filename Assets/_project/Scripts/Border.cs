using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BorderType
{
    Start,
    End
}


public class Border : MonoBehaviour
{

    public BorderType BorderType;
    public Collider Collider { get; set; }
    // Start is called before the first frame update

    private void Start()
    {
        Collider = GetComponent<Collider>();
    }

    private void OnTriggerExit(Collider other)
    {

        if (BorderType == BorderType.End)
        {
            if (other.GetComponent<SpaceShipController>())
            {
                
                //LevelSpline.Instance.PoolersBlocks[LevelSpline.Instance.ActualBlock.Name].ReturnToPool(LevelSpline.Instance.ActualBlock.gameObject);
              
            }
        }
        if (BorderType == BorderType.Start)
        {
            if (other.GetComponent<SpaceShipController>())
            {
                LevelSpline.Instance.LoadedBlock.Dequeue();
                LevelSpline.Instance.LoadBlock();
                LevelSpline.Instance.ActualBlock = transform.parent.GetComponent<SplinePipe>();
                LevelSpline.Instance.ActualBlock.FillVolume();
            }

        }
      
    }


}

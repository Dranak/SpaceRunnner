using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayVolume : MonoBehaviour
{
    private void OnTriggerExit(Collider collision)
    {
        if (collision.GetComponent<SpaceShipController>())
        {
            Debug.Log("WESH");
            Level.Instance.PoolersBlocks[Level.Instance.ActualBlock.Name].ReturnToPool(Level.Instance.ActualBlock.gameObject);
            Level.Instance.LoadBlock();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.GetComponent<SpaceShipController>())
        {
            Level.Instance.ActualBlock = transform.parent.GetComponent<BlockGenerator>();
            Debug.Log("BN");
        }
       
    }


}

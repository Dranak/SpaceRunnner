using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pattern : MonoBehaviour
{
    public string Name;
    public int SizePooller;
 
    public List<Element> Elements;
    List<Element> _elements;
    List<Element> _collectibles;
    // Start is called before the first frame update
    void Start()
    {
        for(int index=0;index <Elements.Count-1;++index)
        {
            if(Elements[index].Name == Elements[index+1].Name)
            {
                Elements[index + 1].Name += index.ToString();
            }
        }

        if (_elements == null || _collectibles == null)
        {
            _elements = Elements.Where(e => e.EnergyGain < 0).ToList();
            _collectibles = Elements.Where(e => e.EnergyGain > 0).ToList();
        }
        DesactiveAll();
    }

    public void LoadCollectibles()
    {
        if (_collectibles == null)
        {
            _collectibles = Elements.Where(e => e.EnergyGain >0).ToList();
        }

        int collectibleToActivate = Random.Range(1, _collectibles.Count);
        for (int index = 0; index < collectibleToActivate; ++index)
        {
             int indexToActivate = Random.Range(0, _collectibles.Count-1);
            _collectibles[indexToActivate].gameObject.SetActive(true);
            _collectibles[indexToActivate].transform.rotation = Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f));
        }
        

    }

    public void LoadElements()
    {
        if (_elements == null)
        {
            _elements = Elements.Where(e => e.EnergyGain < 0).ToList();

        }

        //int _elementsToActivate = Random.Range(1, _elements.Count - 1);

        for (int index = 0; index < _elements.Count; ++index)
        {
            //int indexRamdom = Random.Range(0, _elements.Count);
            _elements[index].gameObject.SetActive(true);
        }
        
    }

    private void OnEnable()
    {
        //LoadElements();
        //LoadCollectibles();
    }

    private void OnDisable()
    {
        //DesactiveAll();
    }

    public void DesactiveElement(Element element)
    {
        Elements.Where(e => e.Name == element.Name).FirstOrDefault().gameObject.SetActive(false);
    }

    public void DesactiveAll()
    {
        if (_elements == null || _collectibles == null)
        {
            _elements = Elements.Where(e => e.EnergyGain < 0).ToList();
            _collectibles = Elements.Where(e => e.EnergyGain > 0).ToList();
        }
        _collectibles.ForEach(c => c.gameObject.SetActive(false));
        _elements.ForEach(c => c.gameObject.SetActive(false));

    }
}

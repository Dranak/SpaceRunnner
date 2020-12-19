using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public static Level Instance { get; set; }
    public BlockGenerator ActualBlock;
    public Queue<BlockGenerator> LoadedBlock { get; set; }
    public List<BlockGenerator> ListBlocks;
    public Dictionary<string, Pooller> PoolersBlocks { get; set; }
    public int SizePooller;
    public GameObject Pool;
    public Vector3 spawnOrigin;
    [Tooltip("First number is for straight or not , second one is for up or not")]
    public Vector2 InitialProbaRotate;
    private Vector2 _actualProbaRotate;
    public int blockToSpawn = 10;
    public List<Material> Materials;
    private Material _selectedMaterial;
    

    void Awake()
    {
        Instance = Instance ?? this;
        PoolersBlocks = new Dictionary<string, Pooller>();
        foreach (BlockGenerator block in ListBlocks)
        {
            PoolersBlocks.Add(block.Name, new Pooller(SizePooller, block.gameObject));
        }

        LoadedBlock = new Queue<BlockGenerator>();

    }

    private void Start()
    {
        _selectedMaterial = SelectMaterial();
        _actualProbaRotate = InitialProbaRotate;
        for (int index = 0; index < blockToSpawn; ++index)
        {
            LoadBlock();
        }
        Time.timeScale = 0;

       

    }

    public void LoadBlock()
    {
        string randomBlockName = ListBlocks[Random.Range(0, ListBlocks.Count)].Name;
        BlockGenerator loadedBlock = PoolersBlocks[randomBlockName].GetObject().GetComponent<BlockGenerator>();
        loadedBlock.Pipe.material = _selectedMaterial;
        loadedBlock.transform.parent = transform;
        if (LoadedBlock.Count > 0)
        {
            Vector3 posToSpawn = LoadedBlock.ToArray()[LoadedBlock.Count - 1].BorderEnd.transform.position;
            loadedBlock.transform.position = posToSpawn;
            if(Random.Range(0,100) <_actualProbaRotate.x)
            {
                loadedBlock.transform.rotation = Quaternion.Euler(Vector3.zero);
                _actualProbaRotate.x = Mathf.Max(0, _actualProbaRotate.x - 5f);
                if (_actualProbaRotate.x == 0)
                    _actualProbaRotate.x = InitialProbaRotate.x;
            }
            else
            {
                if(Random.Range(0,100) < _actualProbaRotate.y)
                {
                    loadedBlock.transform.rotation = Quaternion.Euler(Vector3.left * 45);
                    _actualProbaRotate.y = Mathf.Max(0, _actualProbaRotate.y - 5f);
                    if (_actualProbaRotate.y == 0)
                        _actualProbaRotate.y = InitialProbaRotate.y;
                }
                else
                {
                    loadedBlock.transform.rotation = Quaternion.Euler(Vector3.left * -45);
                    _actualProbaRotate.y = Mathf.Min(100, _actualProbaRotate.y + 5f);
                    if (_actualProbaRotate.y == 100)
                        _actualProbaRotate.y = InitialProbaRotate.y;
                }

            }
        }
        else
        {
            loadedBlock.transform.position = spawnOrigin;
        }
        loadedBlock.FillVolume();
        LoadedBlock.Enqueue(loadedBlock);

    }


  


    public void StartSpawnBlock()
    {

        for (int i = 0; i < blockToSpawn; ++i)
        {
            LoadBlock();
        }

    }


    public Material SelectMaterial()
    {
        int randomIndexMaterials = Random.Range(0, Materials.Count);

        return Materials[randomIndexMaterials];

    }
}

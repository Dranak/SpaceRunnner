using SplineMesh;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelSpline : MonoBehaviour
{
    public static LevelSpline Instance { get; set; }
    public Spline Spline;
    public SplineMeshTiling SplineMesh;

    public SplinePipe ActualBlock;
    public List<AnimationCurve> AllCurves;
    public Queue<SplinePipe> LoadedBlock { get; set; }
    public List<SplinePipe> ListBlocks;
    public Dictionary<string, Pooller> PoolersBlocks { get; set; }
    public int SizePooller;
    public GameObject Pool;
    public Vector3 spawnOrigin;

    public int blockToSpawn = 10;
    public List<Material> Materials;
    private Material _selectedMaterial;


    void Awake()
    {
        Instance = Instance ?? this;
        PoolersBlocks = new Dictionary<string, Pooller>();
        foreach (SplinePipe block in ListBlocks)
        {
            PoolersBlocks.Add(block.Name, new Pooller(SizePooller, block.gameObject));
        }
        LoadedBlock = new Queue<SplinePipe>();
    }

    private void Start()
    {
        _selectedMaterial = SelectMaterial();

        StartSpawnBlock();
         ActualBlock = LoadedBlock.Peek();
        Time.timeScale = 0;
    }

    public void LoadBlock()
    {

        string randomBlockName = ListBlocks[UnityEngine.Random.Range(0, ListBlocks.Count)].Name;
        SplinePipe loadedBlock = PoolersBlocks[randomBlockName].GetObject().GetComponent<SplinePipe>();

        loadedBlock.transform.parent = transform;
        if (LoadedBlock.Count > 0)
        {
            loadedBlock.transform.position = LoadedBlock.ToArray()[LoadedBlock.Count - 1].BorderEnd.transform.position+ Vector3.forward * 35;
            loadedBlock.SetCurve(AllCurves[UnityEngine.Random.Range(0, AllCurves.Count)]);
        }
        else
        {
          
            loadedBlock.transform.position = spawnOrigin;
            //loadedBlock.Offset = spawnOrigin;
            loadedBlock.SetCurve(AnimationCurve.Linear(0, 0, 1, 0));
        }

        //loadedBlock.PipeMeshGenerator.pipeMaterial = _selectedMaterial;
        loadedBlock.FillVolume();
        LoadedBlock.Enqueue(loadedBlock);
        AddPointsBlock(loadedBlock.Points);

    }

    private void AddPointsBlock(List<SplineNode> nodes)
    {

        foreach (SplineNode node in nodes)
        {
            Spline.AddNode(node);
        }
    }

    public void StartSpawnBlock()
    {
        Spline.Clear();
        for (int i = 0; i < blockToSpawn; ++i)
        {
            LoadBlock();
        }
        //foreach (SplinePipe pipe in LoadedBlock.ToList())
        //{
        //    AddPointsBlock(pipe.Points);
        //}
        SplineMesh.CreateMeshes();
        //LoadedBlock.Peek().FillVolume();

    }


    public Material SelectMaterial()
    {
        int randomIndexMaterials = UnityEngine.Random.Range(0, Materials.Count);

        return Materials[randomIndexMaterials];

    }
}

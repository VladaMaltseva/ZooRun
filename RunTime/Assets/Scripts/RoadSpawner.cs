﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    public GameObject[] RoadBlockPrefabs;
    public GameObject StartBlock;


    float startBlockXPos;
    int blocksCount = 7;
    float blockLength = 0;
    //int safeZone = 50;

    public Transform PlayerTransf;
    List<GameObject> CurrentBlocks = new List<GameObject>();

    Vector3 startPlayerPos;

    void Start()
    {
        startBlockXPos = PlayerTransf.position.x + 15;
        blockLength = 31;

        StartGame();
    }
    public void StartGame()
    {
        PlayerTransf.GetComponent<PlayerMovement>().ResetPosition();

        foreach(var go in CurrentBlocks)

            Destroy(go);
        
        CurrentBlocks.Clear();

        for (int i = 0; i < blocksCount; i++)
        {
            SpawnBlock();
        }
    }

    void LateUpdate()
    {
        CheckForSpawn();
    }
    void CheckForSpawn()
    {
        if (CurrentBlocks[0].transform.position.x - PlayerTransf.position.x< -25) 
        {
            SpawnBlock();
            DectroyBlock();
        }
    }

    void SpawnBlock() {
        GameObject block = Instantiate(RoadBlockPrefabs[Random.Range(0, RoadBlockPrefabs.Length)], transform);
        Vector3 blockPos;

        if (CurrentBlocks.Count > 0)
            blockPos = CurrentBlocks[CurrentBlocks.Count - 1].transform.position + new Vector3(blockLength, 0, 0);
        else
            blockPos = new Vector3(startBlockXPos, 0, 0);
        block.transform.position = blockPos;
        CurrentBlocks.Add(block);
    }
    void DectroyBlock()
    {
        Destroy(CurrentBlocks[0]);
        CurrentBlocks.RemoveAt(0);
    }
}

using System;
using UnityEngine;
using System.Collections.Generic;

public class BackgroundManager : MonoBehaviour
{
    // インスペクターで設定するフィールド
    [SerializeField] private GameObject prefab; // 生成するPrefab
    [SerializeField] private int numberOfChildren = 5; // 生成するPrefabの数

    [Header("Background Parameters")]
    [Tooltip("The amount of movement power applied when scrolling.")]
    //[Range(0f, 20f)]
    [SerializeField] private float scrollSpeed = 2f;  // 背景のスクロール速度
    [SerializeField] private Vector3 scrollDirection = Vector3.left + Vector3.up;

    private float spacing; // Prefab間の間隔

    private List<Transform> children = new List<Transform>();
    private BackgroundGraphicsInfo childInfo;

    private void Awake()
    {
        GetChildren();
        if (children.Count != numberOfChildren)
        {
            SpawnPrefabs(numberOfChildren - children.Count);
        }

        childInfo = children[0].GetComponent<BackgroundGraphicsInfo>();
    }

    void Start()
    {
        SetChildrenPosition(children);
        SetChildrenScrollParametaer();
    }

    private void GetChildren()
    {
        // 子オブジェクトをリストに追加
        foreach (Transform child in transform)
        {
            children.Add(child);
        }
    }

    private void SpawnPrefabs(int numberOfPrefabs)
    {
        // 親Transform
        Transform parentTransform = transform;
        for (int i = 0; i < numberOfPrefabs; i++)
        {
            // 生成位置を計算
            Vector3 spawnPosition = new Vector3(i * spacing, 0, 0);
            // Prefabを生成
            GameObject child = Instantiate(prefab, spawnPosition, Quaternion.identity, parentTransform);
            children.Add(child.transform);
        }
    }

    private void SetChildrenPosition(List<Transform> children)
    {
        spacing = childInfo.spriteSize.x;
        Debug.Log(spacing);
        for (int i = 0; i < children.Count; i++)
        {
            children[i].position = new Vector3(i * spacing, 0, 0);
        }
    }

    private void SetChildrenScrollParametaer()
    {
        for (int i = 0; i < children.Count; i++)
        {
            children[i].GetComponent<BackgroundViewController>().SetNewParametare(scrollSpeed, scrollDirection);
        }
    }
}

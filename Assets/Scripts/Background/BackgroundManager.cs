using System;
using UnityEngine;
using System.Collections.Generic;

public class BackgroundManager : MonoBehaviour
{
    // �C���X�y�N�^�[�Őݒ肷��t�B�[���h
    [SerializeField] private GameObject prefab; // ��������Prefab
    [SerializeField] private int numberOfChildren = 5; // ��������Prefab�̐�

    [Header("Background Parameters")]
    [Tooltip("The amount of movement power applied when scrolling.")]
    //[Range(0f, 20f)]
    [SerializeField] private float scrollSpeed = 2f;  // �w�i�̃X�N���[�����x
    [SerializeField] private Vector3 scrollDirection = Vector3.left + Vector3.up;

    private float spacing; // Prefab�Ԃ̊Ԋu

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
        // �q�I�u�W�F�N�g�����X�g�ɒǉ�
        foreach (Transform child in transform)
        {
            children.Add(child);
        }
    }

    private void SpawnPrefabs(int numberOfPrefabs)
    {
        // �eTransform
        Transform parentTransform = transform;
        for (int i = 0; i < numberOfPrefabs; i++)
        {
            // �����ʒu���v�Z
            Vector3 spawnPosition = new Vector3(i * spacing, 0, 0);
            // Prefab�𐶐�
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

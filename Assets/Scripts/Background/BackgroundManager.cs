using System;
using UnityEngine;
using System.Collections.Generic;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class BackgroundManager : MonoBehaviour
{
    // インスペクターで設定するフィールド
    [Header("Alignment Parameters")]
    [SerializeField] private GameObject backgroundPrefab; // 生成するPrefab
    [SerializeField] private AlignmentType alignment = AlignmentType.Horizontal;
    [SerializeField] private bool autoGeneratePrefabs = false; // 自動生成を制御するフラグ
    [SerializeField] private int prefabCount = 5; // 生成するPrefabの数
    

    [Header("Background Parameters")]
    [Tooltip("The amount of movement power applied when scrolling.")]
    [Range(0f, 20f)]
    [SerializeField] private float scrollSpeed = 2f;  // 背景のスクロール速度
    [SerializeField] private Vector3 scrollDirection = Vector3.left + Vector3.up;

    private List<Transform> prefabInstances = new List<Transform>();
    private BackgroundGraphicsInfo prefabInfo;
    private List<BackgroundViewController> prefabControllers = new List<BackgroundViewController>();

    public enum AlignmentType
    {
        Horizontal,
        Vertical,
        Diagonal
    }

    private void Awake()
    {
        InitializePrefabs();
        if (autoGeneratePrefabs)
        {
            prefabCount = CalculatePrefabCount(prefabInfo.spriteSize);
        }
        if (prefabInstances.Count != prefabCount)
        {
            int difference = prefabCount - prefabInstances.Count;
            for (int i = 0; i < difference; i++)
            {
                CreatePrefab(backgroundPrefab);
            }
        }
    }

    private void Start()
    {
        SetPositionPrefabs(prefabInstances);
        SetPrefabControllers();
    }

    private void Update()
    {
        SetPrefabScrollParameters(scrollSpeed, scrollDirection);
    }

    private void InitializePrefabs()
    {
        foreach (Transform child in transform)
        {
            prefabInstances.Add(child);
        }

        if (prefabInstances.Count == 0)
        {
            CreatePrefab(backgroundPrefab);
        }
        prefabInfo = prefabInstances[0].GetComponent<BackgroundGraphicsInfo>();
    }

    private void CreatePrefab(GameObject prefab)
    {
        GameObject child = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
        prefabInstances.Add(child.transform);
    }

    private int CalculatePrefabCount(Vector2 spriteSize)
    {
        // 画面サイズに基づいて生成するPrefabの数を計算
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found.");
            return prefabCount;
        }

        float screenWidth = 2f * mainCamera.orthographicSize * mainCamera.aspect;
        float screenHeight = 2f * mainCamera.orthographicSize;

        switch (alignment)
        {
            case AlignmentType.Horizontal:
                return Mathf.CeilToInt(screenWidth / spriteSize.x) + 1;
            case AlignmentType.Vertical:
                return Mathf.CeilToInt(screenHeight / spriteSize.y) + 1;
            case AlignmentType.Diagonal:
                return Mathf.CeilToInt(Mathf.Max(screenWidth / spriteSize.x, screenHeight / spriteSize.y)) + 1;
            default:
                return prefabCount;
        }
    }

    private void SetPositionPrefabs(List<Transform> prefabTransforms)
    {
        Vector3 prefabSpacing = new Vector3(prefabInfo.spriteSize.x, prefabInfo.spriteSize.y, 0);
        Vector3 startPosition = CalculateStartPosition(prefabInfo);

        for (int i = 0; i < prefabTransforms.Count; i++)
        {
            Vector3 position = alignment switch
            {
                AlignmentType.Horizontal => new Vector3(startPosition.x + (i * prefabSpacing.x), 0, 0),
                AlignmentType.Vertical => new Vector3(0, -(startPosition.y + (i * prefabSpacing.y)), 0),
                AlignmentType.Diagonal => new Vector3(startPosition.x + (i * prefabSpacing.x), -(startPosition.y + (i * prefabSpacing.y)), 0),
                _ => Vector3.zero,
            };
            prefabTransforms[i].position = position;
        }
    }

    private Vector3 CalculateStartPosition(BackgroundGraphicsInfo graphicsInfo)
    {
        return (graphicsInfo.spriteSize - graphicsInfo.viewSize) / 2;
    }

    private void SetPrefabControllers()
    {
        foreach (Transform prefabInstance in prefabInstances)
        {
            prefabControllers.Add(prefabInstance.GetComponent<BackgroundViewController>());
        }
    }

    private void SetPrefabScrollParameters(float speed, Vector3 direction)
    {
        foreach (BackgroundViewController prefavController in prefabControllers)
        {
            prefavController.SetNewParameters(speed, direction);
        }
    }
}

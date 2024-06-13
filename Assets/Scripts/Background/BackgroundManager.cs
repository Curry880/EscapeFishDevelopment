using System;
using UnityEngine;
using System.Collections.Generic;

public class BackgroundManager : MonoBehaviour
{
    [Header("Alignment Parameters")]
    [Tooltip("The prefab to generate for the background.")]
    [SerializeField] private GameObject backgroundPrefab;
    [Tooltip("The alignment type for the background prefabs.")]
    [SerializeField] private AlignmentType alignment = AlignmentType.Horizontal;
    [Tooltip("Flag to control automatic prefab generation.")]
    [SerializeField] private bool autoGeneratePrefabs = false;
    [Tooltip("The number of prefabs to generate.")]
    [SerializeField] private int prefabCount = 5;


    [Header("Background Parameters")]
    [Tooltip("The amount of movement power applied when scrolling.")]
    [Range(0f, 20f)]
    [SerializeField] private float scrollSpeed = 2f;
    [Tooltip("The direction in which the background will scroll.")]
    [SerializeField] private Vector3 scrollDirection = Vector3.left + Vector3.up;

    // List to store instances of the background prefabs
    private List<Transform> prefabInstances = new List<Transform>();

    // Information about the graphics of the prefabs
    private BackgroundGraphicsInfo prefabInfo;

    // List to store controllers of the background prefabs
    private List<BackgroundViewController> prefabControllers = new List<BackgroundViewController>();

    /// <summary>
    /// Types of alignment for background prefabs.
    /// </summary>
    public enum AlignmentType
    {
        Horizontal,
        Vertical,
        Diagonal
    }

    /// <summary>
    /// Initializes the background manager. Called when the script instance is being loaded.
    /// </summary>
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

    /// <summary>
    /// Called before the first frame update. Sets the initial positions and controllers for the prefabs.
    /// </summary>
    private void Start()
    {
        SetPositionPrefabs(prefabInstances);
        SetPrefabControllers();
    }

    /// <summary>
    /// Called once per frame. Updates the scrolling parameters for the prefabs.
    /// </summary>
    private void Update()
    {
        SetPrefabScrollParameters(scrollSpeed, scrollDirection);
    }

    /// <summary>
    /// Initializes the list of prefab instances from existing children or creates new ones if needed.
    /// </summary>
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

    /// <summary>
    /// Creates a new instance of the given prefab and adds it to the list of instances.
    /// </summary>
    /// <param name="prefab">The prefab to create.</param>
    private void CreatePrefab(GameObject prefab)
    {
        GameObject child = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
        prefabInstances.Add(child.transform);
    }

    /// <summary>
    /// Calculates the number of prefabs to generate based on the screen size and alignment type.
    /// </summary>
    /// <param name="spriteSize">The size of the sprite in the prefab.</param>
    /// <returns>The number of prefabs to generate.</returns>
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

    /// <summary>
    /// Sets the initial positions of the prefabs based on the alignment type.
    /// </summary>
    /// <param name="prefabTransforms">The list of prefab transforms.</param>
    private void SetPositionPrefabs(List<Transform> prefabTransforms)
    {
        Vector3 prefabSpacing = new Vector3(prefabInfo.spriteSize.x, prefabInfo.spriteSize.y, 0);
        Vector3 startPosition = CalculateStartPosition(prefabInfo);

        for (int i = 0; i < prefabTransforms.Count; i++)
        {
            Vector3 position = alignment switch
            {
                AlignmentType.Horizontal => new Vector3(startPosition.x + (i * prefabSpacing.x), transform.position.y, transform.position.z),
                AlignmentType.Vertical => new Vector3(transform.position.x, -(startPosition.y + (i * prefabSpacing.y)), transform.position.z),
                AlignmentType.Diagonal => new Vector3(startPosition.x + (i * prefabSpacing.x), -(startPosition.y + (i * prefabSpacing.y)), transform.position.z),
                _ => Vector3.zero,
            };
            prefabTransforms[i].position = position;
        }
    }

    /// <summary>
    /// Calculates the starting position for the prefabs based on the graphics info.
    /// </summary>
    /// <param name="graphicsInfo">The graphics info of the prefabs.</param>
    /// <returns>The starting position for the prefabs.</returns>
    private Vector3 CalculateStartPosition(BackgroundGraphicsInfo graphicsInfo)
    {
        return (graphicsInfo.spriteSize - graphicsInfo.viewSize) / 2;
    }

    /// <summary>
    /// Adds controllers to the prefabs for managing their behavior.
    /// </summary>
    private void SetPrefabControllers()
    {
        foreach (Transform prefabInstance in prefabInstances)
        {
            prefabControllers.Add(prefabInstance.GetComponent<BackgroundViewController>());
        }
    }

    /// <summary>
    /// Sets the scrolling parameters for the prefabs.
    /// </summary>
    /// <param name="speed">The scrolling speed.</param>
    /// <param name="direction">The scrolling direction.</param>
    private void SetPrefabScrollParameters(float speed, Vector3 direction)
    {
        foreach (BackgroundViewController prefabController in prefabControllers)
        {
            prefabController.SetNewParameters(speed, direction);
        }
    }
}

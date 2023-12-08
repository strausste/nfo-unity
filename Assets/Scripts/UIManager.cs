using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // ====================================================================================
    // Class attributes
    // ====================================================================================

    public enum Algorithms
    {
        DIJKSTRA,
        A_STAR_EUCLIDEAN,
        A_STAR_MANHATTAN,
        A_STAR_CHEBYSHEV,
        A_STAR_OCTILE
    }

    [SerializeField] private TextMeshProUGUI gridSizeText;
    [SerializeField] private TextMeshProUGUI algorithmText;
    [SerializeField] private TextMeshProUGUI pathCostText;
    [SerializeField] private TextMeshProUGUI numberOfStepsText;
    [SerializeField] private TextMeshProUGUI executionTimeText;

    // Static reference to the instance (singleton pattern)
    private static UIManager _instance;
    
    // ====================================================================================
    
    
    // ====================================================================================
    // Class methods
    // ====================================================================================
    
    
    // Access the instance
    public static UIManager GetInstance()
    {
        // If the instance doesn't exist, find or create it
        if (_instance == null)
        {
            _instance = FindObjectOfType<UIManager>();

            // If no instance exists in the scene, create a new GameObject and add the script
            if (_instance == null)
            {
                GameObject singletonObject = new GameObject(nameof(UIManager));
                _instance = singletonObject.AddComponent<UIManager>();
            }
        }

        return _instance;
    }
    
    public void SetGridSizeText()
    {
        var grid = GridManager.GetInstance();

        if (grid)
        {
            gridSizeText.SetText("Grid size: " + "(" + grid.GetGridSize().x + "," + grid.GetGridSize().y + ")");
        }
        else
        {
            gridSizeText.SetText("Grid size: ");
        }
    }
    
    public void SetAlgorithmText(Algorithms algorithm)
    {
        switch (algorithm)
        {
            case Algorithms.DIJKSTRA:
                algorithmText.SetText("Algorithm: Dijkstra");
                break;
            case Algorithms.A_STAR_EUCLIDEAN:
                algorithmText.SetText("Algorithm: A* - Euclidean distance");
                break;
            case Algorithms.A_STAR_MANHATTAN:
                algorithmText.SetText("Algorithm: A* - Manhattan distance");
                break;
            case Algorithms.A_STAR_CHEBYSHEV:
                algorithmText.SetText("Algorithm: A* - Chebyshev's distance");
                break;
            case Algorithms.A_STAR_OCTILE:
                algorithmText.SetText("Algorithm: A* - Octile distance");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public void SetPathCostText(int cost)
    {
        pathCostText.SetText("SP cost: " + cost);
    }
    
    public void SetNumberOfStepsText(int steps)
    {
        numberOfStepsText.SetText("Number of steps: " + steps);
    }
    
    public void SetExecutionTimeText(int time)
    {
        executionTimeText.SetText("Execution time: " + time + "ms");
    }

    public void RestoreTexts()
    {
        algorithmText.SetText("Algorithm: ");
        pathCostText.SetText("SP cost: ");
        numberOfStepsText.SetText("Number of steps: ");
        executionTimeText.SetText("Execution time: ");
    }
    
    // ====================================================================================
    
    
    // ====================================================================================
    // MonoBehaviour methods
    // ====================================================================================
    
    private void Awake()
    {
        // Ensure there's only one instance, and persist it between scenes
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If another instance already exists, destroy this one
            Destroy(gameObject);
        }
    }

    // ====================================================================================
}

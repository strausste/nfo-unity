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

    [SerializeField] private TextMeshProUGUI algorithmText;
    [SerializeField] private TextMeshProUGUI pathCostText;
    [SerializeField] private TextMeshProUGUI executionTimeText;

    // Static reference to the instance (singleton pattern)
    private static UIManager _instance;
    
    // ====================================================================================
    
    
    // ====================================================================================
    // Class methods
    // ====================================================================================

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
    
    public void SetExecutionTimeText(int time)
    {
        executionTimeText.SetText("Execution time: " + time + "ms");
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
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // ====================================================================================
}

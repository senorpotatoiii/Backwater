using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarManager : MonoBehaviour
{
    public AStarManager instance;
    [SerializeField] Node testNode;

    void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        
    }
}

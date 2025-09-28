using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpriteAssets : MonoBehaviour
{
    private Sprite[] _sprites;
    private static SpriteAssets s_i;
    public Dictionary<string, Sprite> Sprites = new();
    public static SpriteAssets s_Instance
    {
        get
        {
            if (s_i == null)
            s_i = (Instantiate(Resources.Load("SpriteAssets")) as GameObject).GetComponent<SpriteAssets>();
            return s_i;
        }
    }
    
    void Awake()
    {
        _sprites = Resources.LoadAll("Portraits", typeof(Sprite)).Cast<Sprite>().ToArray();
        foreach (Sprite s in _sprites)
        {
            Sprites.Add(s.name, s);
        }
    }
}

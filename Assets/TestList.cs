using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class TestList : MonoBehaviour
{
    public string world;

    [Button]
    public void DestroyRepeatWorld()
    {
        IEnumerable<char> distinctList = world.Distinct(); 
        for (int i = 0; i < world.Length; i++) 
        { 
            while (world.IndexOf(world.Substring(i, 1), StringComparison.Ordinal) != world.LastIndexOf(world.Substring(i, 1), StringComparison.Ordinal)) 
            { 
                world = world.Remove(world.LastIndexOf(world.Substring(i, 1), StringComparison.Ordinal), 1); 
            } 
        } 
    }
}

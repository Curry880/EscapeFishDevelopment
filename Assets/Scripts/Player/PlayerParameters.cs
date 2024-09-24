using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParameters : MonoBehaviour
{
    public Dictionary<string, float> parameters = new Dictionary<string, float>()
    {
        { "Health", 100f },
        { "Speed", 5f },
        { "Stamina", 50f }
    };

    public void AddParameter(string name, float value)
    {
        if (!parameters.ContainsKey(name))
        {
            parameters.Add(name, value);
        }
    }

    public void RemoveParameter(string name)
    {
        if (parameters.ContainsKey(name))
        {
            parameters.Remove(name);
        }
    }
}

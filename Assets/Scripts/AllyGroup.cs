using UnityEngine;
using System.Collections.Generic;

public class AllyGroup : MonoBehaviour
{
    public List<GameObject> allies = new List<GameObject>();

    void Start()
    {
        foreach (Transform child in transform)
        {
            allies.Add(child.gameObject);
        }
    }

    public void RemoveAlly(GameObject ally)
    {
        allies.Remove(ally);
    }
}

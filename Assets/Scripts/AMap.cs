using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AMap : MonoBehaviour
{
    public MapTimeTableSO timeTable;
    protected List<int> eventTable = new();
    public virtual void Initialize()
    {
        foreach (var t in timeTable.set)
        {
            if (!eventTable.Contains(t))
            {
                eventTable.Add(t);
            }
        }
        eventTable.Sort();
    }

    public void CheckTime(int time)
    {
        if (eventTable.Count > 0 && time >= eventTable[0])
        {
            Execute(eventTable[0]);
            eventTable.RemoveAt(0);
        }
    }

    public virtual void Execute(int time) {}
    

}

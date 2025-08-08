using System;
using Unity.Collections;
using UnityEngine;

[Serializable]
public class RuntimePlayerMovementData
{
    public int maxDashCount;
}

public class PlayerMovementData : MonoBehaviour
{
    public PlayerMovementSO playerMovementSO;
    public RuntimePlayerMovementData runtimePlayerMovementData;
}

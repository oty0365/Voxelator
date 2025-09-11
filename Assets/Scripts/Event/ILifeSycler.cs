using System;
using UnityEngine;

public interface ILifeSycler
{
    public event Action OnDeath;
}

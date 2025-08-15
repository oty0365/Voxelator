using UnityEngine;
using System.Collections.Generic;

public class Fsm : MonoBehaviour
{
    private Dictionary<string, IState> states = new Dictionary<string, IState>();
    private IState currentState;
    void Update()
    {
        currentState?.Execute();
    }

    void FixedUpdate()
    {
        currentState?.FixedExecute();
    }
    public void RegisterState(string stateName, IState state)
    {
        if (!states.ContainsKey(stateName))
        {
            states.Add(stateName, state);
        }
    }
    public void ChangeState(string stateName)
    {
        if (states.ContainsKey(stateName)&&currentState!=states[stateName])
        {
            currentState?.Exit();
            currentState = states[stateName];
            currentState?.Enter();
        }
    }
    public IState GetCurrentState()
    {
        return currentState;
    }
}
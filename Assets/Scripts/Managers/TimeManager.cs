using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : SceneSingletonMonoBehaviour<TimeManager>
{
    public int gameTime;

    private Coroutine _currentClockFlow;
    private float _elapsed;

    private Queue<int> _eventQueue = new Queue<int>();
    private HashSet<int> _queuedTimes = new HashSet<int>();
    
    public void StartGame()
    {
        if (_currentClockFlow != null)
            StopGame();

        gameTime = 0;
        ContinueGame();
    }
    
    public void StopGame()
    {
        if (_currentClockFlow != null)
        {
            StopCoroutine(_currentClockFlow);
            _currentClockFlow = null;
        }
    }
    public void ContinueGame()
    {
        if (_currentClockFlow == null)
        {
            _currentClockFlow = StartCoroutine(ClockFlow());
        }
    }
    private void EnqueueEvent(int time)
    {
        if (_queuedTimes.Add(time)) 
            _eventQueue.Enqueue(time);
    }
    private void ProcessEventQueue()
    {
        if (_eventQueue.Count == 0) return;

        int time = _eventQueue.Dequeue();
        _queuedTimes.Remove(time);

        EventManager.Instance.Invoke(EventKey.OnClocked, time);
    }
    private IEnumerator ClockFlow()
    {
        var wait = new WaitForEndOfFrame();
        while (true)
        {
            _elapsed += Time.deltaTime;

            if (_elapsed >= 1f)
            {
                gameTime++;
                EnqueueEvent(gameTime);
                _elapsed -= 1f;
            }

            yield return wait;
        }
    }
    private void Update()
    {
        ProcessEventQueue();
    }
}

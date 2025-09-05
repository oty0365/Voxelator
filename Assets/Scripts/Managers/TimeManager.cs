using System.Collections;
using UnityEngine;

public class TimeManager : SceneSingletonMonoBehaviour<TimeManager>
{
    public int gameTime;
    private Coroutine _currentClockFlow;

    public void StartGame()
    {
        if (_currentClockFlow != null)
        {
            StopGame();
        }
        gameTime = 0;
        ContinueGame();
    }

    public void StopGame()
    {
        StopCoroutine(_currentClockFlow);
        _currentClockFlow = null;
    }
    
    public void ContinueGame()
    {
        _currentClockFlow = StartCoroutine(ClockFlow());
    }

    private IEnumerator ClockFlow()
    {
        yield return new WaitForSeconds(1);
        gameTime++;
    }
    
    
}

using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private PlayerMovementData playerMovementData;
    public float currentDashSpeed;
    private bool isDashing = false;
    private int currentDashCount;
    private Coroutine dashRechargeCoroutine;

    public int CurrentDashCount => currentDashCount;
    public int MaxDashCount => playerMovementData.runtimePlayerMovementData.maxDashCount;
    public bool IsDashing => isDashing;

    private void Start()
    {
        currentDashCount = playerMovementData.runtimePlayerMovementData.maxDashCount;

        if (dashRechargeCoroutine == null)
        {
            dashRechargeCoroutine = StartCoroutine(DashRechargeFlow());
        }
    }
    public void Dash(Vector2 dir)
    {
        if (isDashing || currentDashCount <= 0)
        {
            return;
        }

        currentDashCount--;
        StartCoroutine(DashFlow(dir));
    }

    private IEnumerator DashFlow(Vector2 dir)
    {
        isDashing = true;
        controller.playerMoves = PlayerMoves.Dash;
        controller.currentDir = dir;
        currentDashSpeed = playerMovementData.playerMovementSO.dashSpeed;
        yield return new WaitForSeconds(playerMovementData.playerMovementSO.dashTime);
        currentDashSpeed = 1;
        controller.currentDir = Vector2.zero;
        controller.playerMoves = PlayerMoves.None;
        isDashing = false;
    }

    private IEnumerator DashRechargeFlow()
    {
        while (true)
        {
            yield return new WaitForSeconds(playerMovementData.playerMovementSO.dashCooldown);

            if (currentDashCount < playerMovementData.runtimePlayerMovementData.maxDashCount)
            {
                currentDashCount++;
            }
        }
    }
}
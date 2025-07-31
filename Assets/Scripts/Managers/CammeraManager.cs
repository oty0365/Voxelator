using UnityEngine;
using System.Collections;

public class CammeraManager : HalfSingleMono<CammeraManager>
{
    [SerializeField] private GameObject player;
    [SerializeField] private float followSpeed = 10f;
    [Header("Screen Shake Settings")]
    [SerializeField] private AnimationCurve shakeCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
    [SerializeField] private float defaultShakeIntensity = 1f;
    [SerializeField] private float defaultShakeDuration = 0.5f;

    private Transform _currentTarget;
    private Coroutine _followRoutine;
    private Coroutine _shakeRoutine;
    
    private Vector3 _originalPosition;
    private bool _isShaking = false;

    void Start()
    {
        SetTarget(player.transform);
    }

    public void SetTarget(Transform newTarget)
    {
        if (newTarget == null) return;

        _currentTarget = newTarget;
        
        if (_followRoutine != null)
            StopCoroutine(_followRoutine);

        _followRoutine = StartCoroutine(FollowTarget());
    }

    private IEnumerator FollowTarget()
    {
        while (true)
        {
            if (_currentTarget != null)
            {
                Vector3 targetPos = _currentTarget.position;
                targetPos.z = transform.position.z;
                
                if (!_isShaking)
                {
                    transform.position = Vector3.Lerp(transform.position, targetPos, Time.fixedDeltaTime * followSpeed);
                }
                else
                {
                    _originalPosition = Vector3.Lerp(_originalPosition, targetPos, Time.fixedDeltaTime * followSpeed);
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public void ShakeCamera()
    {
        ShakeCamera(defaultShakeIntensity, defaultShakeDuration);
    }

    public void ShakeCamera(float intensity, float duration)
    {
        if (_shakeRoutine != null)
            StopCoroutine(_shakeRoutine);

        _shakeRoutine = StartCoroutine(ShakeCoroutine(intensity, duration));
    }

    public void ShakeCamera(Vector2 direction, float intensity, float duration)
    {
        if (_shakeRoutine != null)
            StopCoroutine(_shakeRoutine);

        _shakeRoutine = StartCoroutine(ShakeCoroutine(intensity, duration, direction.normalized));
    }

    private IEnumerator ShakeCoroutine(float intensity, float duration, Vector2? direction = null)
    {
        _isShaking = true;
        _originalPosition = transform.position;
        
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float strength = intensity * shakeCurve.Evaluate(elapsed / duration);
            
            Vector3 shakeOffset;
            
            if (direction.HasValue)
            {
                shakeOffset = new Vector3(
                    direction.Value.x * strength * Random.Range(-1f, 1f),
                    direction.Value.y * strength * Random.Range(-1f, 1f),
                    0f
                );
            }
            else
            {
                shakeOffset = new Vector3(
                    Random.Range(-1f, 1f) * strength,
                    Random.Range(-1f, 1f) * strength,
                    0f
                );
            }

            transform.position = _originalPosition + shakeOffset;
            yield return null;
        }

        transform.position = _originalPosition;
        _isShaking = false;
    }

    public void StopShake()
    {
        if (_shakeRoutine != null)
        {
            StopCoroutine(_shakeRoutine);
            _shakeRoutine = null;
        }
        
        if (_isShaking)
        {
            transform.position = _originalPosition;
            _isShaking = false;
        }
    }

    public void ExplosionShake()
    {
        ShakeCamera(2f, 0.8f);
    }

    public void HitShake()
    {
        ShakeCamera(0.5f, 0.3f);
    }

    public void EarthquakeShake()
    {
        ShakeCamera(Vector2.up, 0.3f, 2f);
    }
}
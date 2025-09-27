using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class CameraManager : SceneSingletonMonoBehaviour<CameraManager>
{
    [SerializeField] private GameObject player;
    [SerializeField] private float followSpeed = 10f;
    
    [Header("Screen Shake Settings")]
    [SerializeField] private AnimationCurve shakeCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
    [SerializeField] private float defaultShakeIntensity = 1f;
    [SerializeField] private float defaultShakeDuration = 0.5f;
    
    [Header("Health Vignette Settings")]
    [SerializeField] private AnimationCurve healthVignetteCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
    [SerializeField] private float healthVignetteDuration = 0.8f;
    [SerializeField] private float healthVignetteIntensity = 0.4f;
    [SerializeField] private Color decreaseColor = Color.red;
    [SerializeField] private Color increaseColor = Color.green;
    
    [SerializeField] private Volume volume;
    private UnityEngine.Rendering.Universal.Vignette _vignette;
    private UnityEngine.Rendering.Universal.ChromaticAberration _aberration;
    [NonSerialized] public UnityEngine.Rendering.Universal.ColorAdjustments colorAdjustments;

    private Transform _currentTarget;
    private Coroutine _followRoutine;
    private Coroutine _shakeRoutine;
    private Coroutine _healthVignetteRoutine;
    
    private Vector3 _originalPosition;
    private bool _isShaking = false;
    
    private Color _originalVignetteColor;
    private float _originalVignetteIntensity;

    private float _currentHealth;

    void Start()
    {
        if (volume.profile.TryGet(out UnityEngine.Rendering.Universal.Vignette vignette))
        {
            _vignette = vignette;
            _originalVignetteColor = _vignette.color.value;
            _originalVignetteIntensity = _vignette.intensity.value;
        }
        if (volume.profile.TryGet(out UnityEngine.Rendering.Universal.ColorAdjustments colorAdj))
        {
            colorAdjustments = colorAdj;
        }
        if (volume.profile.TryGet(out UnityEngine.Rendering.Universal.ChromaticAberration colorAber))
        {
            _aberration = colorAber;
        }

        //PlayerStatus.Instance.playerHp.OnChanged += OnHealthChange;
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

    public void OnHealthChange(float current, float max)
    {
        if (_currentHealth > current)
        {
            OnHealthDecrease(1, 0.5f);
        }
        else
        {
            OnHealthIncrease(1, 0.5f);
        }

        if (max / 3 > current)
        {
            SetGlitch(1);
        }
        else
        {
            SetGlitch(0);
        }
        _currentHealth = current;
    }

    public void SetGlitch(float val)
    {
        _aberration.intensity.value = val;
    }
    
    public void OnHealthDecrease()
    {
        OnHealthDecrease(healthVignetteIntensity, healthVignetteDuration);
    }

    public void OnHealthDecrease(float intensity, float duration)
    {
        if (_healthVignetteRoutine != null)
            StopCoroutine(_healthVignetteRoutine);

        _healthVignetteRoutine = StartCoroutine(HealthVignetteCoroutine(decreaseColor, intensity, duration));
    }

    public void OnHealthIncrease()
    {
        OnHealthIncrease(healthVignetteIntensity, healthVignetteDuration);
    }

    public void OnHealthIncrease(float intensity, float duration)
    {
        if (_healthVignetteRoutine != null)
            StopCoroutine(_healthVignetteRoutine);

        _healthVignetteRoutine = StartCoroutine(HealthVignetteCoroutine(increaseColor, intensity, duration));
    }

    private IEnumerator HealthVignetteCoroutine(Color effectColor, float intensity, float duration)
    {
        if (_vignette == null) yield break;

        float elapsed = 0f;
        Color startColor = _originalVignetteColor;
        float startIntensity = _originalVignetteIntensity;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;
            float curveValue = healthVignetteCurve.Evaluate(progress);
            
            Color currentColor = Color.Lerp(startColor, effectColor, curveValue);
            _vignette.color.value = currentColor;
            
            float currentIntensity = Mathf.Lerp(startIntensity, intensity, curveValue);
            _vignette.intensity.value = currentIntensity;

            yield return null;
        }
        
        _vignette.color.value = _originalVignetteColor;
        _vignette.intensity.value = _originalVignetteIntensity;
        _healthVignetteRoutine = null;
    }

    public void StopHealthVignette()
    {
        if (_healthVignetteRoutine != null)
        {
            StopCoroutine(_healthVignetteRoutine);
            _healthVignetteRoutine = null;
            
            if (_vignette != null)
            {
                _vignette.color.value = _originalVignetteColor;
                _vignette.intensity.value = _originalVignetteIntensity;
            }
        }
    }

    public void OnDestroy()
    {
        PlayerStatus.Instance.playerHp.OnChanged -= OnHealthChange;
    }
}
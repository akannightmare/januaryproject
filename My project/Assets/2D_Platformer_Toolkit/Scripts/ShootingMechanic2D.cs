using UnityEngine;

public class ShootingMechanic2D : MonoBehaviour
{
    [Header("Mode")]
    [SerializeField] private bool _isEnemy = false;

    [Header("Settings")]
    [SerializeField] private Transform _firePoint;      // Точка вылета магии
    [SerializeField] private GameObject _magicPrefab;   // Префаб снаряда/магии
    [SerializeField] private AudioSource _shootSound;

    [Header("Cooldown")]
    [SerializeField] private float _fireCooldown = 0.5f; // Задержка между выстрелами (сек)
    private float _nextFireTime = 0f; // Время, когда можно стрелять снова

    private void Update()
    {
        if (Time.time < _nextFireTime) return;

        if (_isEnemy)
        {
            // --- ЛОГИКА ВРАГА ---
            // Враг стреляет автоматически, как только откатился кулдаун
            Shoot();
            _shootSound.pitch = Random.Range(0.6f, 1.1f);
            _shootSound.Play();
        }
        else
        {
            // --- ЛОГИКА ИГРОКА ---
            // Игрок стреляет только по кнопке
            if (Input.GetKeyDown(KeyCode.E))
            {
                Shoot();
                _shootSound.pitch = Random.Range(0.6f, 1.1f);
                _shootSound.Play();
            }
        }
    }

    private void Shoot()
    {
        // 1. Обновляем время следующего выстрела
        _nextFireTime = Time.time + _fireCooldown;

        // 2. Создаем снаряд
        Instantiate(_magicPrefab, _firePoint.position, _firePoint.rotation);

        // Опционально: звук выстрела или анимация каста
        // AudioSource.PlayClipAtPoint(_castSound, transform.position);
    }
}

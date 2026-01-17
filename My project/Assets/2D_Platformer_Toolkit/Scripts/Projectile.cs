using UnityEditor.UIElements;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [Header("Характеристики")]
    [SerializeField] private float _speed = 20f;
    [SerializeField] private float _lifeTime = 2f;
    [SerializeField] private int _damage = 10;
    [SerializeField] private string _tag;

    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        // 1. ОТКЛЮЧАЕМ ГРАВИТАЦИЮ КОДОМ
        // Теперь снаряд никогда не будет падать вниз
        _rb.gravityScale = 0;

        // 2. ЗАДАЕМ ПОЛЕТ В СТОРОНУ ПОВОРОТА
        // transform.right — это вектор, указывающий "вправо" относительно поворота самого снаряда.
        // Если снаряд повернут на 180 градусов, transform.right будет указывать влево.
        _rb.linearVelocity = transform.right * _speed;

        // Уничтожение через время
        Destroy(gameObject, _lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D hitInfo)
    {
        // Игнорируем игрока (если у него тег Player)
        if (hitInfo.gameObject.CompareTag(_tag)) return;

        IDamageable damageable = hitInfo.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(_damage);
        }

        Destroy(gameObject);
    }
}
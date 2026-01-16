using UnityEngine;

public class Attack2D : MonoBehaviour
{

    [SerializeField] private int _meleeDamage;
    [SerializeField] private float _damageRadius;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private Transform _attackPos;

    [Header("Cooldown")]
    [SerializeField] private float _meleeAttackCooldown = 0.5f; // Время задержки в секундах
    private float _nextAttackTime = 0f;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= _nextAttackTime) // Левая кнопка мыши или Ctrl
        {
            Attack();

            _nextAttackTime = Time.time + _meleeAttackCooldown;
        }

    }

    void Attack()
    {
        // 1. Запуск анимации (если есть)
        // _animator.SetTrigger("Attack");

        // 2. Обнаружение врагов в радиусе атаки
        // Создаем круг в точке _attackPos радиусом _attackRange и ищем коллайдеры на слое _enemyLayers
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPos.position, _damageRadius, _enemyLayer);

        // 3. Нанесение урона
        foreach (Collider2D enemy in hitEnemies)
        {
            // Проверяем, есть ли на объекте компонент, реализующий IDamageable
            IDamageable damageable = enemy.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(_meleeDamage);
            }
        }
    }

    public void TryAttack()
    {
        if (Time.time >= _nextAttackTime)
        {
            Attack();
            Debug.Log("hit");
            _nextAttackTime = Time.time + _meleeAttackCooldown;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_attackPos == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_attackPos.position, _damageRadius);
    }
}

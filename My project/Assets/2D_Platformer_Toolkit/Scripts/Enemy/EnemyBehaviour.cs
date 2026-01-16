using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Attack2D))]
public class EnemyBehaviour : MonoBehaviour
{
    [Header("Настройки поведения")]
    [SerializeField] private bool _isPatrolling = true;
    [SerializeField] private float _moveSpeed = 2f;

    [Header("Точки патрулирования")]
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;

    [Header("Обнаружение и Атака")]
    [SerializeField] private float _aggroRange = 5f; // Дальность зрения
    [SerializeField] private float _attackDistance = 1f; // Дистанция для удара

    [Header("Проверка обрыва (Ledge Check)")]
    [SerializeField] private Transform _groundCheck; // Точка у ног (чуть впереди)
    [SerializeField] private LayerMask _groundLayer;

    // Ссылки
    private Transform _player;
    private Rigidbody2D _rb;
    private Attack2D _combatModule; // Ссылка на наш скрипт атаки
    private Transform _currentTarget; // Куда идем сейчас (А или Б)

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _combatModule = GetComponent<Attack2D>();

        // Находим игрока автоматически по тегу (убедитесь, что у игрока тег "Player")
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) _player = playerObj.transform;

        // Начинаем патруль с точки Б
        _currentTarget = _pointB;
    }

    private void Update()
    {
        if (_player == null) return; // Если игрока нет, ничего не делаем

        float distToPlayer = Vector2.Distance(transform.position, _player.position);

        // 1. ПРОВЕРКА: Если игрок близко (в зоне агрессии)
        if (distToPlayer <= _aggroRange)
        {
            // Если мы уже совсем близко - АТАКУЕМ
            if (distToPlayer <= _attackDistance)
            {
                StopMoving();
                _combatModule.TryAttack(); // Дергаем соседний компонент
            }
            // Иначе - ПРЕСЛЕДУЕМ
            else
            {
                ChasePlayer();
            }
        }
        // 2. Иначе - ПАТРУЛИРУЕМ (или стоим)
        else
        {
            if (_isPatrolling)
            {
                Patrol();
            }
            else
            {
                StopMoving(); // Просто стоим
            }
        }
    }

    private void ChasePlayer()
    {
        // Идем в сторону игрока, но проверяем обрыв
        MoveToTarget(_player.position);
    }

    private void Patrol()
    {
        // Идем к текущей точке (А или Б)
        MoveToTarget(_currentTarget.position);

        // Если дошли до точки (погрешность 0.5f)
        if (Vector2.Distance(transform.position, _currentTarget.position) < 0.5f)
        {
            // Меняем цель
            _currentTarget = (_currentTarget == _pointA) ? _pointB : _pointA;
        }
    }

    private void MoveToTarget(Vector3 target)
    {
        // Определяем направление (-1 влево, 1 вправо)
        float direction = (target.x > transform.position.x) ? 1 : -1;

        // Поворачиваем спрайт (можно вызвать ваш метод FlipThePlayer, здесь упрощенно)
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * direction, transform.localScale.y, 1);

        // ПРОВЕРКА ОБРЫВА
        // Пускаем луч вниз от точки проверки (перед ногами)
        bool isGroundAhead = Physics2D.Raycast(_groundCheck.position, Vector2.down, 1f, _groundLayer);

        if (isGroundAhead)
        {
            // Земля есть - идем
            _rb.linearVelocity = new Vector2(direction * _moveSpeed, _rb.linearVelocity.y);
        }
        else
        {
            // Земли нет (обрыв) - стоим
            StopMoving();
        }
    }

    private void StopMoving()
    {
        _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
    }

    // Рисуем зоны в редакторе для удобства
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _aggroRange); // Зона агро

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackDistance); // Зона атаки

        if (_pointA != null && _pointB != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_pointA.position, _pointB.position); // Линия патруля
        }
    }

}

using UnityEngine;

public class EnemyTargeting : MonoBehaviour
{
    [Header("Настройки обнаружения")]
    [SerializeField] private float _detectionRange = 10f; // Радиус видимости игрока
    [SerializeField] private bool _faceRightInitially = true; // Куда смотрит спрайт по умолчанию (обычно вправо)

    [Header("Объект для вращения")]
    [SerializeField] private Transform _partToRotate; // Если пусто, будет вращать весь объект врага

    private Transform _player;

    private void Start()
    {
        // 1. Автоматически ищем игрока по тегу
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            _player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Игрок не найден! Проверьте, стоит ли тег 'Player' на игроке.");
        }

        // Если не назначили, что вращать, вращаем сам объект, на котором висит скрипт
        if (_partToRotate == null)
            _partToRotate = transform;
    }

    private void Update()
    {
        if (_player == null) return;

        // 2. Считаем дистанцию до игрока
        float distanceToPlayer = Vector2.Distance(transform.position, _player.position);

        // Если игрок в радиусе видимости
        if (distanceToPlayer <= _detectionRange)
        {
            FacePlayer();
        }
    }

    private void FacePlayer()
    {
        // Определяем, где игрок относительно нас: справа или слева?
        // Если X игрока больше нашего X, значит он справа.
        bool isPlayerToTheRight = _player.position.x > transform.position.x;

        // Определяем нужный угол Y (0 или 180)
        float targetYRotation = 0f;

        if (isPlayerToTheRight)
        {
            // Если спрайт изначально смотрит вправо, то для поворота вправо угол 0
            targetYRotation = _faceRightInitially ? 0f : 180f;
        }
        else
        {
            // Игрок слева
            targetYRotation = _faceRightInitially ? 180f : 0f;
        }

        // Применяем поворот
        // Мы меняем только ось Y, сохраняя X и Z текущими
        Vector3 currentRot = _partToRotate.localEulerAngles;
        _partToRotate.localEulerAngles = new Vector3(currentRot.x, targetYRotation, currentRot.z);
    }

    // Рисуем радиус в редакторе, чтобы было удобно настраивать
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);
    }
}
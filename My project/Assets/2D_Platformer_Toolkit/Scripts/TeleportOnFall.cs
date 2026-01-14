using UnityEngine;

namespace Peykarimeh.PlatformerToolkit
{
    public class TeleportOnFall : MonoBehaviour
    {
        [SerializeField] Transform teleportPos;


        private void Awake()
        {
            teleportPos = transform.GetChild(0).transform;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                collision.transform.position = teleportPos.position;
                Camera.main.transform.position = teleportPos.position;
            }
        }
    }
}
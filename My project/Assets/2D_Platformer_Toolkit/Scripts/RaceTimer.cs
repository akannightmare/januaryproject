using TMPro;
using UnityEngine;

namespace Peykarimeh.PlatformerToolkit
{
    public class RaceTimer : MonoBehaviour
    {
        [SerializeField] bool isStarted;
        [SerializeField] float timer;
        [SerializeField] float hScore;
        [SerializeField] TextMeshProUGUI TM_pro;
        [SerializeField] TextMeshProUGUI H_S_TM_pro;

        public static RaceTimer instance;
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);

            }
            instance.ResetTimer();

        }
        private void Start()
        {
            timer = 0f;
            hScore = 0f;
            TM_pro.SetText(TimeToText(timer));
            H_S_TM_pro.SetText(TimeToText(hScore));

        }
        public void ResetTimer()
        {
            isStarted = false;
            timer = 0f;
            TM_pro.SetText(TimeToText(timer));
        }
        private void Update()
        {
            if (isStarted)
            {
                timer += Time.deltaTime;
                TM_pro.SetText(TimeToText(timer));

            }
        }

        public void StartRace()
        {
            if (!isStarted)
            {
                timer = 0;
                isStarted = true;
            }
        }

        string TimeToText(float _time)
        {
            return _time.ToString("0.000").Replace(",", ".");
        }

        void CheckHighScore()
        {
            if (timer < hScore || hScore == 0f)
            {
                hScore = timer;
                H_S_TM_pro.SetText(TimeToText(hScore));
            }
        }
        public void FinishRace()
        {
            isStarted = false;
            CheckHighScore();
        }

    }
}
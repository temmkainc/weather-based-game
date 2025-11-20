using UnityEngine;

namespace GameLoop
{
    public class RoundTimer
    {
        public float TimeLeft { get; private set; }
        public bool IsExpired => TimeLeft <= 0;

        public void Start(float time)
        {
            TimeLeft = time;
        }

        public void Tick(float delta)
        {
            TimeLeft -= delta;
        }
    }
}

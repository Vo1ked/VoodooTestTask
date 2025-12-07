using UnityEngine;

namespace Gameplay.PowerUps
{
    public class PowerUp_SpeedBoost : PowerUp
    {
        public AnimationCurve SpeedCurve;
        public float Speed;
        public override void OnPlayerTouched (Player _Player)
        {
            base.OnPlayerTouched (_Player);
            _Player.AddSpeedUp(Speed,m_Duration,SpeedCurve);
        }
    }
}
using UnityEngine;

namespace Com.SamuelHOARAU.Common.Shakes
{
    [CreateAssetMenu(menuName = "Shakes/Shake setting")]
    public class ShakeSetting : ScriptableObject
    {
        private const float MINIMUN_VALUE = 0.0001f;

        [SerializeField] private Vector2 magnitude = default;
        [Min(MINIMUN_VALUE)][SerializeField] private float duration = 1;
        [Min(MINIMUN_VALUE)][SerializeField] private float frequency = 1;
        [Min(MINIMUN_VALUE)][SerializeField] private bool bidirectionnal = true;
        [SerializeField] private AnimationCurve curve = AnimationCurve.Linear(1,1,0,0);

        public Vector2 Magnitude => magnitude;
        public float Duration => duration;
        public float Frequency => frequency;
        public bool Bidirectionnal => bidirectionnal;

        /// <summary>
        /// Returns a ratio depending on a given ratio
        /// </summary>
        /// <param name="ratio">Input ratio</param>
        /// <returns>Return the curve affected ratio</returns>
        public float CurvePosition(float ratio) => curve.Evaluate(ratio);
    }
}

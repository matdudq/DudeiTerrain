using Unity.Collections;
using UnityEngine;

namespace Procedural.Utilities
{
    public static class AnimationCurveUtility
    {
        #region Public methods

        public static void GetSampledCurve(this AnimationCurve animationCurve, ref NativeArray<float> arrayToFill)
        {
            int samplesCount = arrayToFill.Length;
            
            for (int j = 0; j < samplesCount; j++)
            {
                arrayToFill[j] = animationCurve.Evaluate(j / (float)samplesCount);            
            }
        }

        #endregion Public methods
    }
}
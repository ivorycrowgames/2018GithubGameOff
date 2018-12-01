using IvoryCrow.Extensions;
using System;

namespace IvoryCrow.Utilities
{
    public abstract class BaseFloatGenerator
    {
        protected Random rng;

        protected BaseFloatGenerator() : this(new Random())
        {
        }

        protected BaseFloatGenerator(Random random)
        {
            this.rng = random;
        }

        protected float GetNextRandomValue()
        {
            return (float)(rng.NextDouble());
        }

        protected float GetNextRandomValue(float min, float max)
        {
            return GetNextRandomValue().Remap(0, 1.0f, min, max);
        }
    }

    [System.Serializable]
    public class RandomFloatValueRange : BaseFloatGenerator
    {
        public float Max;
        public float Min;

        public RandomFloatValueRange() : this(0, 0)
        {
        }

        public RandomFloatValueRange(float min, float max) : base()
        {
            Min = min;
            Max = max;
        }

        public float GetNext()
        {
            return base.GetNextRandomValue(Min, Max);
        }
    }

    [System.Serializable]
    public class RandomFloatValueRangeScaling : BaseFloatGenerator
    {
        public float InitialMin;
        public float InitialMax;
        public float FinalMin;
        public float FinalMax;

        public float InitialTimeDelay = 0;
        public float ScaleDuration;

        public RandomFloatValueRangeScaling()
        {
        }

        public float GetNext(float currentTime)
        {
            float currentRangeMin = InitialMin;
            float currentRangeMax = InitialMax;

            // We need to scale if we're passed the initial
            if (currentTime > InitialTimeDelay)
            {
                // Check if we are maxed, if not scale the values
                if (currentTime >= ScaleDuration + InitialTimeDelay)
                {
                    currentRangeMin = FinalMin;
                    currentRangeMax = FinalMax;
                }
                else
                {
                    float percentageComplete = (currentTime - InitialTimeDelay) / (ScaleDuration);
                    currentRangeMin = percentageComplete.Remap(0, 1.0f, InitialMin, FinalMin);
                    currentRangeMax = percentageComplete.Remap(0, 1.0f, InitialMax, FinalMax);
                }
            }

            return base.GetNextRandomValue(currentRangeMin, currentRangeMax);
        }
    }

    [System.Serializable]
    public class InfiniteScalingFloat : BaseFloatGenerator
    {
        public float InitialValue;

        public float InitialTimeDelay = 0;
        public float IncreasePerMinute = 1.0f;

        public InfiniteScalingFloat()
        {
        }

        public float GetNext(float currentTime)
        {
            if (currentTime <= InitialTimeDelay)
            {
                return InitialValue;
            }

            float timeAfterInitialDelay = currentTime - InitialTimeDelay;
            float amountToScale = timeAfterInitialDelay / 60.0f;
            return InitialValue + (amountToScale * IncreasePerMinute);
        }
    }
}

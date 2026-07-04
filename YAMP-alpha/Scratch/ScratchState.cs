// YAMP/Scratch/ScratchState.cs  
using System;

namespace YAMP.Scratch
{
    public enum FrictionMode : byte
    {
        None = 0,   // No friction — velocity holds until explicitly changed  
        Exponential = 1,  // Natural decay: vel *= (1 - k/sr)  
        Linear = 2,   // Constant drag: vel -= sign(vel) * k/sr  
        Combined = 3    // Both applied together (most realistic)  
    }

    public sealed class ScratchState
    {
        public static readonly ScratchState Default = new ScratchState(
            isScratching: false,
            velocity: 1.0f,
            targetPosition: -1.0,
            allowReverse: true,
            frictionMode: FrictionMode.None,
            frictionCoeff: 0f,
            backspinActive: false,
            backspinVelocity: 0f,
            returnToPlayback: true);

        public readonly bool IsScratching;
        public readonly float Velocity;
        public readonly double TargetPosition;
        public readonly bool AllowReverse;

        // ── Friction / Backspin fields ────────────────────────────────────  
        public readonly FrictionMode FrictionMode;
        public readonly float FrictionCoeff;     // 0.0 – 50.0 typical range  
        public readonly bool BackspinActive;    // true = backspin was just triggered  
        public readonly float BackspinVelocity;  // initial reverse velocity  
        public readonly bool ReturnToPlayback;  // resume normal speed after stop?  

        public ScratchState(
            bool isScratching, float velocity, double targetPosition,
            bool allowReverse, FrictionMode frictionMode, float frictionCoeff,
            bool backspinActive, float backspinVelocity, bool returnToPlayback)
        {
            IsScratching = isScratching;
            Velocity = velocity;
            TargetPosition = targetPosition;
            AllowReverse = allowReverse;
            FrictionMode = frictionMode;
            FrictionCoeff = frictionCoeff;
            BackspinActive = backspinActive;
            BackspinVelocity = backspinVelocity;
            ReturnToPlayback = returnToPlayback;
        }
    }
}
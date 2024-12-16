using System;
using UnityEngine;

namespace Lottie.Sys
{
    internal enum TvgEngine
    {
        SW = 1 << 1
    }
    
    internal enum Result
    {
        Success = 0,
        InvalidArguments,
        InsufficientCondition,
        FailedAllocation,
        MemoryCorruption,
        NonSupport,
        Unknown = 255
    }

    internal enum ColorSpace
    {   
        Abgr8888 = 0
    }
    
    public static class TvgSys
    {
        public static bool Initialized { get; private set; }
        
        public static void Init()
        {
            if (Initialized) return;
            Check(TvgLib.tvg_engine_init(TvgEngine.SW, 2), "Engine Init");
            Initialized = true;
        }

        private static void Term()
        {
            Debug.Log("TERMINATED");
            TvgLib.tvg_engine_term(TvgEngine.SW);
            Initialized = false;
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitScene()
        {
            Init();
            Application.quitting += TermScene;
        }

        private static void TermScene()
        {
            Term();
            Application.quitting -= TermScene;
        }
        
        internal static void Check(int code, string msg)
        {
            switch ((Result)code)
            {
                case Result.Success:
                    return;
                case Result.InvalidArguments:
                    msg += " (Invalid Arguments)";
                    break;
                case Result.InsufficientCondition:
                    msg += " (Insufficient Condition)";
                    break;
                case Result.FailedAllocation:
                    msg += " (Failed Allocation)";
                    break;
                case Result.MemoryCorruption:
                    msg += " (Memory Corruption)";
                    break;
                case Result.NonSupport:
                    msg += " (Non Support)";
                    break;
                case Result.Unknown:
                    msg += " (Unknown)";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(code), code, null);
            }
            throw new Exception("ThorVG: " + msg);
        }
    }
}
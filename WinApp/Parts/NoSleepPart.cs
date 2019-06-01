﻿using System;
using System.Runtime.InteropServices;

namespace PlayPauser.Parts
{
    public class NoSleepPart : IPart
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern ExecutionState SetThreadExecutionState(ExecutionState esFlags);

        [Flags]
        private enum ExecutionState : uint
        {
            EsAwaymodeRequired = 0x00000040,
            EsContinuous = 0x80000000,
            EsDisplayRequired = 0x00000002,
            EsSystemRequired = 0x00000001
        }

        public void Start(Options options)
        {
            SetThreadExecutionState(ExecutionState.EsContinuous | ExecutionState.EsSystemRequired);
        }

        public void Stop()
        {
            SetThreadExecutionState(ExecutionState.EsContinuous);
        }
    }
}
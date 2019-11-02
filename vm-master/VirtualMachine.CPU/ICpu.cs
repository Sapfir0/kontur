using System;
using VirtualMachine.Core;

namespace VirtualMachine.CPU
{
    public interface ICpu : IDevice
    {
        TimeSpan StepDelay { get; set; }
        Word InstructionPointer { get; set; }
        void Exit();
        void Sleep(int delayTime);
    }
}
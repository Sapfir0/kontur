using System;
using VirtualMachine.Core;

namespace VirtualMachine.CPU.InstructionSet.Instructions
{
    public class exit : InstructionBase
    {
        public exit() : base(8, OperandType.Ignored, OperandType.Ignored, OperandType.Ignored)
        { }

        protected override void ExecuteInternal(ICpu proc, IMemory __, Operand src, Operand dst, Operand ___) {
            proc.Exit();
        }

        public override string Name => "exit";
    }
}


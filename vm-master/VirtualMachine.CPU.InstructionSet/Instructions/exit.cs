using VirtualMachine.Core;

namespace VirtualMachine.CPU.InstructionSet.Instructions
{
    public class exit : InstructionBase
    {
        public exit() : base(1, OperandType.Ignored, OperandType.Ignored, OperandType.Ignored)
        { }

        protected override void ExecuteInternal(ICpu _, IMemory __, Operand src, Operand dst, Operand ___)
        {
            
        }
    }
}


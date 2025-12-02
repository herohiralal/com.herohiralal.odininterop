using System;

namespace OdinInterop
{
    public class GenerateOdinInteropAttribute : Attribute
    {
        public GenerateOdinInteropAttribute() { }

        public string odinSrcAppend { get; set; } = "";
    }
}

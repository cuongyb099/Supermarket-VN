using UnityEngine;

namespace Core.Constant
{
    public static class MaterialConstant
    {
        //Outline
        public static readonly int EnableOutline = Shader.PropertyToID("_Outline");
        public static readonly int OutlineWidth = Shader.PropertyToID("_OutlineWidth");
        public static readonly int OutlineColor = Shader.PropertyToID("_OutlineColor");
        public static readonly string SRPDefaultUnlit = "SRPDefaultUnlit";
        public static readonly string OutlinePass = "OUTLINE_PASS_DISABLE";
    }
}

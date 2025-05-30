using UnityEngine;

namespace Core.Constant
{
    public static class MaterialConstant
    {
        //Outline
        public static readonly int SurfaceID = Shader.PropertyToID("_Preset");
        public static readonly int ZWrite = Shader.PropertyToID("_ZWrite");
        public static readonly int SourceBlend = Shader.PropertyToID("_SourceBlend");
        public static readonly int DestBlend = Shader.PropertyToID("_DestBlend");
        public static readonly int Cutout = Shader.PropertyToID("_Cutout");
        public static readonly string CutoutKeyword = "CUTOUT";
        
        public static readonly string SRPDefaultUnlit = "SRPDefaultUnlit";
        public static readonly string OutlinePass = "OUTLINE_PASS_DISABLE";
    }
}

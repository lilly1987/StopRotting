using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Lilly.StopRotting
{
    [StaticConstructorOnStartup]
    internal static class StopRottingStartup
    {
        static StopRottingStartup()
        {
            new Harmony("Lilly.StopRotting").PatchAll(Assembly.GetExecutingAssembly());
        }
    }
 
    [HarmonyPatch(typeof(CompRottable), "TickInterval")]
    public static class CompRottable_TickInterval_Patch
    {
        // TickInterval은 private이므로 Harmony가 이름으로 찾도록 지정
        public static bool Prefix(CompRottable __instance)
        {
            if (__instance == null)
                return false;

            // 이미 부패한 대상도 신선한 상태로 복구
            if (__instance.RotProgress != 0f)
            {
                __instance.RotProgress = 0f;
            }

            __instance.disabled = true; // TickInterval 실행 차단

            // 원본 TickInterval 실행 차단:
            // 부패 진행, 부패 피해, 부패에 의한 파괴를 모두 방지
            return false;
        }
    }



}

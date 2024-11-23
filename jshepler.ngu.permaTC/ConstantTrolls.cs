using HarmonyLib;
using UnityEngine;

namespace jshepler.ngu.permaTC
{
    [HarmonyPatch]
    internal class ConstantTrolls
    {
        private const float TROLL_SECONDS = 120f;

        private static TrollChallengeController _controller;
        private static bool _challengesUnlocked => _controller.character.challenges.unlocked;
        private static bool _running = false;

        private static float _timer
        {
            get => mods.ModSave.Data.PTC_Timer;
            set => mods.ModSave.Data.PTC_Timer = value;
        }

        private static int _counter
        {
            get => mods.ModSave.Data.PTC_TrollCount;
            set => mods.ModSave.Data.PTC_TrollCount = value;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(TrollChallengeController), "Start")]
        private static void TrollChallengeController_Start_postfix(TrollChallengeController __instance)
        {
            _controller = __instance;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(Character), "Update")]
        private static void Character_Start_update()
        {
            if (!_running)
                return;

            _timer += Time.deltaTime;
            if (_timer >= TROLL_SECONDS)
            {
                _timer = 0f;
                _counter++;

                if (_counter % 5 == 0)
                    _controller.doBigTroll();
                else
                    _controller.doSmallTroll();
            }
        }

        [HarmonyPrefix, HarmonyPatch(typeof(Character), "addOfflineProgress")]
        private static bool Character_addOfflineProgress_prefix(Character __instance)
        {
            __instance.splashScreen.message = "No Offline Progress for PermaTC";
            __instance.splashScreen.openScreen();
            _running = false;

            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(OfflineProgressSplashScreen), "closeScreen")]
        private static void OfflineProgressSplashScreen_closeScreen_prefix()
        {
            _running = true;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(RebirthButtonHover), "updateText")]
        private static void RebirthButtonHover_updateText_postfix(RebirthButtonHover __instance)
        {
            if (!_running)
                return;

            var nextTrollSeconds = TROLL_SECONDS - _timer;
            var text = $"\n[PTC] troll #{_counter + 1} in {NumberOutput.timeOutput(nextTrollSeconds)}";

            __instance.rebirthTimeText.text += text;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(Rebirth), "engage", [typeof(bool)])]
        private static void Rebirth_engate_postfix(bool hardReset, Rebirth __instance)
        {
            __instance.character.allChallenges.trollChallenge.resetTrolls();

            if (hardReset)
            {
                _timer = 0;
                _counter = 0;
            }
        }
    }
}

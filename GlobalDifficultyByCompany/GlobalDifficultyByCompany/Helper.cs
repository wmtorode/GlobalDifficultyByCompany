using BattleTech;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace GlobalDifficultyByCompany {
    public class Helper {

        public static Settings LoadSettings() {
            try {
                using (StreamReader r = new StreamReader($"{GlobalDifficultyByCompany.ModDirectory}/settings.json")) {
                    string json = r.ReadToEnd();
                    return JsonConvert.DeserializeObject<Settings>(json);
                }
            }
            catch (Exception ex) {
                Logger.LogError(ex);
                return null;
            }
        }

        public static float CalculateCBillValue(MechDef mech) {
            float currentCBillValue = 0f;
            float num = 10000f;
            currentCBillValue = (float)mech.Chassis.Description.Cost;
            float num2 = 0f;
            num2 += mech.Head.CurrentArmor;
            num2 += mech.CenterTorso.CurrentArmor;
            num2 += mech.CenterTorso.CurrentRearArmor;
            num2 += mech.LeftTorso.CurrentArmor;
            num2 += mech.LeftTorso.CurrentRearArmor;
            num2 += mech.RightTorso.CurrentArmor;
            num2 += mech.RightTorso.CurrentRearArmor;
            num2 += mech.LeftArm.CurrentArmor;
            num2 += mech.RightArm.CurrentArmor;
            num2 += mech.LeftLeg.CurrentArmor;
            num2 += mech.RightLeg.CurrentArmor;
            num2 *= UnityGameInstance.BattleTechGame.MechStatisticsConstants.CBILLS_PER_ARMOR_POINT;
            currentCBillValue += num2;
            for (int i = 0; i < mech.Inventory.Length; i++) {
                MechComponentRef mechComponentRef = mech.Inventory[i];
                currentCBillValue += (float)mechComponentRef.Def.Description.Cost;
            }
            currentCBillValue = Mathf.Round(currentCBillValue / num) * num;
            return currentCBillValue;
        }

        // Capitals by faction
        private static Dictionary<string, string> capitalsByFaction = new Dictionary<string, string> {
            { "Kurita", "Luthien" },
            { "Davion", "New Avalon" },
            { "Liao", "Sian" },
            { "Marik", "Atreus (FWL)" },
            { "Rasalhague", "Rasalhague" },
            { "Ives", "St. Ives" },
            { "Oberon", "Oberon" },
            { "TaurianConcordat", "Taurus" },
            { "MagistracyOfCanopus", "Canopus" },
            { "Outworld", "Alpheratz" },
            { "Circinus", "Circinus" },
            { "Marian", "Alphard (MH)" },
            { "Lothian", "Lothario" },
            { "AuriganRestoration", "Coromodir" },
            { "Steiner", "Tharkad" },
            { "ComStar", "Terra" },
            { "Castile", "Asturias" },
            { "Chainelane", "Far Reach" },
            { "ClanBurrock", "Albion (Clan)" },
            { "ClanCloudCobra", "Zara (Homer 2850+)" },
            { "ClanCoyote", "Tamaron" },
            { "ClanDiamondShark", "Strato Domingo" },
            { "ClanFireMandrill", "Shadow" },
            { "ClanGhostBear", "Arcadia (Clan)" },
            { "ClanGoliathScorpion", "Dagda (Clan)" },
            { "ClanHellsHorses", "Kirin" },
            { "ClanIceHellion", "Hector" },
            { "ClanJadeFalcon", "Ironhold" },
            { "ClanNovaCat", "Barcella" },
            { "ClansGeneric", "Strana Mechty" },
            { "ClanSmokeJaguar", "Huntress" },
            { "ClanSnowRaven", "Lum" },
            { "ClanStarAdder", "Sheridan (Clan)" },
            { "ClanSteelViper", "New Kent" },
            { "ClanWolf", "Tiber (Clan)" },
            { "Delphi", "New Delphi" },
            { "Elysia", "Blackbone (Nyserta 3025+)" },
            { "Hanse", "Bremen (HL)" },
            { "JarnFolk", "Trondheim (JF)" },
            { "Tortuga", "Tortuga Prime" },
            { "Valkyrate", "Gotterdammerung" },
            { "Axumite", "Thala" },
            { "WordOfBlake", "EC3040-B42A" }
        };

        private static ILookup<string, string> capitalsBySystemName = capitalsByFaction.ToLookup(pair => pair.Value, pair => pair.Key);
        public static bool IsCapital(StarSystem system) {
            bool isCapital = false;
            try {
                if (capitalsBySystemName.Contains(system.Name)) {
                    isCapital = true;
                }
            }
            catch (Exception ex) {
                Logger.LogError(ex);
            }
            return isCapital;
        }

        public static string GetCapital(string faction) {
            try {
                if (capitalsByFaction.Keys.Contains(faction)) {
                    return capitalsByFaction[faction];
                }
            }
            catch (Exception ex) {
                Logger.LogError(ex);
            }
            return null;
        }

        public static string GetFactionForCapital(StarSystem system) {
            try {
                if (capitalsByFaction.Values.Contains(system.Name)) {
                    return capitalsByFaction.FirstOrDefault(x => x.Value == system.Name).Key;
                }
            }
            catch (Exception ex) {
                Logger.LogError(ex);
            }
            return FactionEnumeration.GetInvalidUnsetFactionValue().Name;
        }

    }
}
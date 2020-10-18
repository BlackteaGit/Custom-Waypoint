using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using CoOpSpRpG;
using System.Reflection;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WTFModLoader;
using WTFModLoader.Manager;
using Microsoft.Xna.Framework.Content;


namespace CustomWaypoint
{
    public class CustomWaypoint : IWTFMod
    {
        public ModLoadPriority Priority => ModLoadPriority.Normal;
        public void Initialize()
        {
            Harmony harmony = new Harmony("blacktea.customwaypoint");
            harmony.PatchAll();
        }
    }
 
    [HarmonyPatch(typeof(UNavigationRev2), "Draw")]
        public class UNavigationRev2_Draw
        {

        public static Texture2D[] MenuArt;

        [HarmonyPrefix]
            private static void Prefix(ref Dictionary<string, GreenArrow> __state)
            {
            __state = new Dictionary<string, GreenArrow>();
            String SteamModsDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), System.IO.Path.Combine(@"..\..\workshop\content\392080")));
            String ModsDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), System.IO.Path.Combine(@"Mods")));
            
            List<string> Folders = new List<string>();

            if (System.IO.Directory.Exists(SteamModsDirectory))
            {
                var dirs = from dir in
                 System.IO.Directory.EnumerateDirectories(SteamModsDirectory, "*",
                    System.IO.SearchOption.AllDirectories)
                           select dir;

                foreach (var dir in dirs)
                {
                    Folders.Add(dir);
                }
            }

            if (System.IO.Directory.Exists(ModsDirectory))
            {
                Folders.Add(ModsDirectory);
                var dirs = from dir in
                 System.IO.Directory.EnumerateDirectories(ModsDirectory, "*",
                    System.IO.SearchOption.AllDirectories)
                           select dir;

                foreach (var dir in dirs)
                {
                    Folders.Add(dir);
                }
            }

            foreach (var folder in Folders)
            {
                if (System.IO.File.Exists(System.IO.Path.Combine(folder, "CustomWaypoint.xnb")))
                {
                    Game1.instance.Content.RootDirectory = folder;
                    Texture2D[] array = new Texture2D[]
                    {
                        null,
                        Game1.instance.Content.Load<Texture2D>("CustomWaypoint")
                    };

                    MenuArt = array;
                }
            }

            Game1.instance.Content.RootDirectory = "Content";

            Dictionary<string, GreenArrow> greenArrows = CoOpSpRpG.PLAYER.greenArrows;
                foreach (var newArrow in greenArrows)
                    __state.Add(newArrow.Key, newArrow.Value);
                CoOpSpRpG.PLAYER.greenArrows.Clear();
            }

            [HarmonyPostfix]
            private static void Postfix(Dictionary<string, GreenArrow> __state, ref Rectangle ___greenArrowIcon, ref float ___constantScale, SpriteBatch batch, ref Rectangle ___homeIcon, ref Vector3 ___cameraPos)
            {

            
            CoOpSpRpG.PLAYER.greenArrows = __state;

                bool flag9 = ___cameraPos.Z <= 64000f;
                checked
                {
                    if (flag9)
                    {
                        SCREEN_MANAGER.spriteBasic.CurrentTechnique = SCREEN_MANAGER.spriteBasicPixelNO;
                        SCREEN_MANAGER.spriteBasicIntensity.SetValue(1.85f);
                        batch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, SCREEN_MANAGER.spriteBasic, null);
                        bool flag10 = PLAYER.currentWorld != null;
                        if (flag10)
                        {
                            Vector2 vector = new Vector2((float)(PLAYER.currentSession.grid.X * 256), (float)(PLAYER.currentSession.grid.Y * 256));
                            foreach (KeyValuePair<string, GreenArrow> entry in PLAYER.greenArrows)
                            {
                                vector.X = (float)(entry.Value.grid.X * 256);
                                vector.Y = (float)(entry.Value.grid.Y * 256);
                                if (entry.Key == "player_assigned")
                                {
                                    if (MenuArt[1] != null)
                                    { 
                                    batch.Draw(MenuArt[1], vector + entry.Value.position / 781.25f, new Rectangle?(___greenArrowIcon), Color.White, 0f, UNavigationRev2.iconEconOffset, ___constantScale, SpriteEffects.None, 0f);
                                    }
                                    else
                                    {
                                    batch.Draw(SCREEN_MANAGER.MenuArt[225], vector + entry.Value.position / 781.25f, new Rectangle?(___greenArrowIcon), Color.White, 0f, UNavigationRev2.iconEconOffset, ___constantScale, SpriteEffects.None, 0f);
                                    }
                                }
                                else
                                {
                                    batch.Draw(SCREEN_MANAGER.MenuArt[225], vector + entry.Value.position / 781.25f, new Rectangle?(___greenArrowIcon), Color.White, 0f, UNavigationRev2.iconEconOffset, ___constantScale, SpriteEffects.None, 0f);

                                }
                            }
                        }
                        batch.End();
                    }
                }
            }
        }

    [HarmonyPatch(typeof(VNavigationRev3), "Draw")]
    public class VNavigationRev3_Draw
    {


        [HarmonyPrefix]
        private static void Prefix(ref Dictionary<string, GreenArrow> __state)
        {
            __state = new Dictionary<string, GreenArrow>();
            Dictionary<string, GreenArrow> greenArrows = CoOpSpRpG.PLAYER.greenArrows;
            foreach (var newArrow in greenArrows)
                __state.Add(newArrow.Key, newArrow.Value);
            CoOpSpRpG.PLAYER.greenArrows.Clear();
        }

        [HarmonyPostfix]
        private static void Postfix(Dictionary<string, GreenArrow> __state, SpriteBatch batch, ref Vector2 ___screenCenter, ref int ___screenHeight, ref ScaleBox ___distanceBox, ref Vector2 ___distBoxTextOffset, ref Vector2 ___arrowUnderOffset, ref float ___uiAlphaPhase, ref float ___constantScale)
        {
            Color arrowColor = new Color(175, 98, 199);
            CoOpSpRpG.PLAYER.greenArrows = __state;

            bool flag58 = ___uiAlphaPhase > 0f;
            if (flag58)
            {
                batch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, SCREEN_MANAGER.spriteBasic, null);


                bool flag = PLAYER.greenArrows.Count > 0;
                checked
                {
                    if (flag)
                    {
                        foreach (KeyValuePair<string, GreenArrow> greenArrow in PLAYER.greenArrows)
                        {
                            bool flag2 = greenArrow.Value.source == null || (greenArrow.Value.source != null && greenArrow.Value.source.tracked);
                            if (flag2)
                            {
                                bool flag3 = greenArrow.Value.position != PLAYER.currentShip.position;
                                if (flag3)
                                {
                                    bool flag4 = greenArrow.Value.grid == PLAYER.currentSession.grid;
                                    if (flag4)
                                    {
                                        float distance = Vector2.Distance(PLAYER.currentShip.position, greenArrow.Value.position);
                                        string text = SCREEN_MANAGER.formatDistanceString(distance);
                                        Vector2 vector = ___screenCenter + Vector2.Normalize(greenArrow.Value.position - PLAYER.currentShip.position) * (float)(___screenHeight / 3 - 15);
                                        ___distanceBox.position = vector;
                                        ___distBoxTextOffset.X = (float)((0 - text.Length) * 4);
                                        batch.Draw(SCREEN_MANAGER.GameArt[131], vector - ___arrowUnderOffset, Color.LightGreen * ___uiAlphaPhase);
                                        try
                                        {
                                            batch.DrawString(SCREEN_MANAGER.FF14reg, text, ___distanceBox.position + ___distBoxTextOffset, Color.LightGreen * ___uiAlphaPhase, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                                        }
                                        catch
                                        {
                                        }
                                        bool flag5 = SCREEN_MANAGER.questJournal != null;
                                        if (flag5)
                                        {
                                            SCREEN_MANAGER.questJournal.AssignDistance(text, distance, greenArrow.Value.source);
                                        }
                                    }
                                    else
                                    {
                                        if (greenArrow.Key == "player_assigned")
                                            {
                                            Vector2 vector2 = new Vector2((float)(greenArrow.Value.grid.X - PLAYER.currentShip.grid.X), (float)(greenArrow.Value.grid.Y - PLAYER.currentShip.grid.Y)) * 200000f;
                                            vector2 += greenArrow.Value.position;
                                            float distance2 = Vector2.Distance(PLAYER.currentShip.position, vector2);
                                            string text2 = SCREEN_MANAGER.formatDistanceString(distance2);
                                            Vector2 vector3 = ___screenCenter + Vector2.Normalize(vector2 - PLAYER.currentShip.position) * (float)(___screenHeight / 3 - 15);
                                            ___distanceBox.position = vector3;
                                            ___distBoxTextOffset.X = (float)((0 - text2.Length) * 4);
                                            batch.Draw(SCREEN_MANAGER.GameArt[131], vector3 - ___arrowUnderOffset, arrowColor * ___uiAlphaPhase);
                                            try
                                            {
                                                batch.DrawString(SCREEN_MANAGER.FF14reg, text2, ___distanceBox.position + ___distBoxTextOffset, arrowColor * ___uiAlphaPhase, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                                            }
                                            catch
                                            {
                                            }
                                            bool flag6 = SCREEN_MANAGER.questJournal != null;
                                            if (flag6)
                                            {
                                                SCREEN_MANAGER.questJournal.AssignDistance(text2, distance2, greenArrow.Value.source);
                                            }

                                        }
                                        else
                                            { 
                                            Vector2 vector2 = new Vector2((float)(greenArrow.Value.grid.X - PLAYER.currentShip.grid.X), (float)(greenArrow.Value.grid.Y - PLAYER.currentShip.grid.Y)) * 200000f;
                                            vector2 += greenArrow.Value.position;
                                            float distance2 = Vector2.Distance(PLAYER.currentShip.position, vector2);
                                            string text2 = SCREEN_MANAGER.formatDistanceString(distance2);
                                            Vector2 vector3 = ___screenCenter + Vector2.Normalize(vector2 - PLAYER.currentShip.position) * (float)(___screenHeight / 3 - 15);
                                            ___distanceBox.position = vector3;
                                            ___distBoxTextOffset.X = (float)((0 - text2.Length) * 4);
                                            batch.Draw(SCREEN_MANAGER.GameArt[131], vector3 - ___arrowUnderOffset, CONFIG.textColor * ___uiAlphaPhase);
                                            try
                                            {
                                                batch.DrawString(SCREEN_MANAGER.FF14reg, text2, ___distanceBox.position + ___distBoxTextOffset, CONFIG.textColor * ___uiAlphaPhase, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                                            }
                                            catch
                                            {
                                            }
                                            bool flag6 = SCREEN_MANAGER.questJournal != null;
                                            if (flag6)
                                            {
                                                SCREEN_MANAGER.questJournal.AssignDistance(text2, distance2, greenArrow.Value.source);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                batch.End();
                batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, SCREEN_MANAGER.spriteBasic, null);
                bool flag27 = ___uiAlphaPhase > 0f;
                if (flag27)
                {
                    bool flag56 = PLAYER.greenArrows.Count > 0;
                    if (flag56)
                    {
                        foreach (GreenArrow greenArrow in PLAYER.greenArrows.Values)
                        {
                            bool flag57 = greenArrow.grid == PLAYER.currentSession.grid;
                            if (flag57)
                            {
                                batch.Draw(SCREEN_MANAGER.GameArt[32], greenArrow.position, null, Color.LightGreen, 0f, UNavigation.iconHomeffset, ___constantScale, SpriteEffects.None, 0.3f);
                            }
                        }
                    }
                }
                batch.End();
            }

        }
    }
}

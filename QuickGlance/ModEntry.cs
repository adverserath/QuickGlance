using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using System;
using System.Linq;

namespace QuickGlance
{
    public class ModEntry : Mod
    {
        private ModConfig Config;
        private GMCMAPI GMCM;
        private PerScreen<float> zoomMemory = new();

        public override void Entry(IModHelper helper)
        {
            this.Config = this.Helper.ReadConfig<ModConfig>();

            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            helper.Events.Input.ButtonReleased += this.OnButtonReleased;
            helper.Events.GameLoop.GameLaunched += this.OnLaunch;
        }

        private void OnLaunch(object sender, GameLaunchedEventArgs e)
        {
            if (Helper.ModRegistry.IsLoaded("spacechase0.GenericModConfigMenu"))
            {
                if (Helper.ModRegistry.Get("spacechase0.GenericModConfigMenu").Manifest.Version.IsOlderThan("1.5.0"))
                {
                    Monitor.Log(Helper.Translation.Get("config.warn", new {v = "1.5.0"}), LogLevel.Warn);
                } else
                {
                    GMCM = Helper.ModRegistry.GetApi<GMCMAPI>("spacechase0.GenericModConfigMenu");
                    Config.Register(GMCM, Helper, ModManifest);
                }
            }
        }

        private void OnButtonReleased(object sender, ButtonReleasedEventArgs e)
        {
            if (!Config.ToggleZoom && Config.ZoomKeys.GetState()==SButtonState.Released && Config.ZoomKeys.Keybinds.Any(x => x.Buttons.Contains(e.Button)))
            {
                    float zoom = zoomMemory.Value;
                    if (zoom is not 0f)
                    {
                        Game1.options.desiredBaseZoomLevel = zoom;
                        zoomMemory.Value = 0f;
                    }
                }
            }

        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (Config.ZoomKeys.JustPressed() && Config.ZoomKeys.Keybinds.Any(x => x.Buttons.Contains(e.Button)))
            {
                if (Config.ToggleZoom)
                {
                    float zoom = zoomMemory.Value;
                    if (zoom is not 0f)
                    {
                        Game1.options.desiredBaseZoomLevel = zoom;
                        zoomMemory.Value = 0f;
                    }
                    else
                    {
                        zoomMemory.Value = Game1.options.desiredBaseZoomLevel;
                        Game1.options.desiredBaseZoomLevel = Config.ZoomLevel;
                    }
                }
                else
                {
                    zoomMemory.Value = Game1.options.desiredBaseZoomLevel;
                    Game1.options.desiredBaseZoomLevel = Config.ZoomLevel;
                }
            }
        }
    }
}
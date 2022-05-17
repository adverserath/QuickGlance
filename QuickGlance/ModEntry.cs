using HarmonyLib;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace QuickGlance
{
    public class ModEntry : Mod
    {
        private ModConfig Config;
        public override void Entry(IModHelper helper)
        {
            this.Config = this.Helper.ReadConfig<ModConfig>();

            Harmony harmony = new Harmony(this.ModManifest.UniqueID);

            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            helper.Events.Input.ButtonReleased += this.OnButtonReleased;
            helper.Events.GameLoop.OneSecondUpdateTicked += GameLoop_OneSecondUpdateTicked;
        }


        private float zoomMemory = Game1.options.desiredBaseZoomLevel;
        private bool zoomedOut = false;
        private void OnButtonReleased(object sender, ButtonReleasedEventArgs e)
        {
            if (zoomedOut && (e.Button == SButton.LeftStick || e.Button == SButton.Home))
            {
                zoomedOut = false;
                Game1.options.desiredBaseZoomLevel = zoomMemory;
            }
        }

        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (!zoomedOut && ((int)e.Button == Config.ZoomKey1 || (int)e.Button == Config.ZoomKey2))
            {
                zoomedOut = true;
                if (Game1.options.desiredBaseZoomLevel != Config.ZoomLevel)
                    zoomMemory = Game1.options.desiredBaseZoomLevel;
                Game1.options.desiredBaseZoomLevel = Config.ZoomLevel;
            }
        }

        private void GameLoop_OneSecondUpdateTicked(object sender, OneSecondUpdateTickedEventArgs e)
        {
            if (zoomedOut && (!Helper.Input.IsDown((SButton)Config.ZoomKey1) && !Helper.Input.IsDown((SButton)Config.ZoomKey2)))
            {
                zoomedOut = false;
                Game1.options.desiredBaseZoomLevel = zoomMemory;
            }
        }

    }
}
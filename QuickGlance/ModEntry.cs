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
        }

        private float zoomMemory = Game1.options.desiredBaseZoomLevel;

        private void OnButtonReleased(object sender, ButtonReleasedEventArgs e)
        {
            if (e.Button == SButton.LeftStick || e.Button == SButton.Home)
            {
                Game1.options.desiredBaseZoomLevel = zoomMemory;
            }
        }

        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if ((int)e.Button == Config.ZoomKey1 || (int)e.Button == Config.ZoomKey2)
            {
                zoomMemory = Game1.options.desiredBaseZoomLevel;
                Game1.options.desiredBaseZoomLevel = Config.ZoomLevel;
            }
        }
    }
}
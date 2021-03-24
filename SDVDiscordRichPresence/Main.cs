//#define EHTRUE
// ^ allows exception handling
using System;
using System.Reflection;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using static SDVDiscordRichPresence.Globals;
// DISCORD CLIENT ID:
namespace SDVDiscordRichPresence
{

    public static class Globals
    {
        public static Discord.Discord discord = null;
    }
    public class ModEntry : Mod
    {
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
#if EHTRUE
            try
            {
                discord = new Discord.Discord(824160913908039729, (UInt64)Discord.CreateFlags.NoRequireDiscord);
            } catch (System.DllNotFoundException)
            {
                this.Monitor.Log($"Discord DLL not found", LogLevel.Error);
            }
#else
            discord = new Discord.Discord(824160913908039729, (UInt64)Discord.CreateFlags.NoRequireDiscord);
#endif
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            helper.Events.GameLoop.DayStarted += this.NewDay;
            helper.Events.GameLoop.OneSecondUpdateTicked += this.Update;
            if (Game1.IsMultiplayer)
                this.Monitor.Log($"Multiplayer Mode", LogLevel.Info);
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;

            this.Monitor.Log($"{Game1.player.Name} pressed {e.Button}", LogLevel.Debug);
        }
        /// <summary>
        /// Runs at the start of every new day to update rich presence
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void NewDay(object sender, DayStartedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;
            this.Monitor.Log($"{Game1.player.Name} on {Game1.whichFarm} farm is on day {Game1.dayOfMonth}", LogLevel.Info);
        }
        private void Update(object sender, OneSecondUpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;
#if EHTRUE
            try
            {
                discord.RunCallbacks();
            } catch (System.NullReferenceException)
            {
                this.Monitor.Log($"Discord DLL not found, null reference exception", LogLevel.Warn);
            }
#else
            discord.RunCallbacks();
#endif
        }
    }
}

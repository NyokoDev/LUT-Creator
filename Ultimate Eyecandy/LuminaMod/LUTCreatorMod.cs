namespace Lumina
{
    using AlgernonCommons;
    using AlgernonCommons.Notifications;
    using AlgernonCommons.Patching;
    using AlgernonCommons.Translation;
    using ICities;
    using System;
    using System.Reflection;


    /// <summary>
    /// The base mod class for instantiation by the game.
    /// </summary>
    public sealed class LUTCreatorMod : PatcherMod<OptionsPanel, PatcherBase>, IUserMod
    {
        /// <summary>
        /// Gets the mod's base display name (name only).
        /// </summary>
          /// 



        public override string BaseName => "LUTCreator";

        /// <summary>
        /// Gets the mod's unique Harmony identfier.
        /// </summary>
        public override string HarmonyID => "com.nyoko.lutcreator.patch";


       

        /// <summary>
        /// Gets the mod's description for display in the content manager.
        /// </summary>
        public string Description => Translations.Translate(LuminaTR.TranslationID.MOD_DESCRIPTION);

        /// <summary>
        /// Saves settings file.
        
        /// </summary>
        public override void SaveSettings() => ModSettings.Save();

        /// <summary>
        /// Loads settings file.
        /// WhatsNewMessage message = new WhatsNewMessage



        /// </summary>
        public override void LoadSettings()
        {
            ModSettings.Load();
            
            

            // Enable detailed logging.
            Logging.DetailLogging = true;
        }
    }
}
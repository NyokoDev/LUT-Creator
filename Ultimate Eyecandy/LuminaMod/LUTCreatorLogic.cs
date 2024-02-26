namespace LUTCreator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;
    using AlgernonCommons;
    using AlgernonCommons.Translation;
    using AlgernonCommons.UI;
    using ColossalFramework;
    using ColossalFramework.UI;
    using Lumina;
    using UnifiedUI.Helpers;
    using UnityEngine;

    /// <summary>
    /// LUTCreator logic class.
    /// </summary>
    internal sealed class LUTCreatorLogic
    {
        // Shadow values.
        private static bool s_disableSmoothing;
        private static float s_shadowIntensity = 1f;

        // UUI button.
        internal UUICustomButton _uuiButton;

        // Instance reference.
        private static LUTCreatorLogic s_instance;

        /// <summary>
        /// Gets the active instance.
        /// </summary>
        internal static LUTCreatorLogic Instance => s_instance;

        /// <summary>
        /// Gets the UUI button reference.
        /// </summary>
        internal UUICustomButton UUIButton => _uuiButton;

        internal static void OnLoad()
        {
            // Set instance reference.
            if (s_instance == null)
            {
                s_instance = new LUTCreatorLogic();
            }

            // Add UUI button.
            s_instance._uuiButton = UUIHelpers.RegisterCustomButton(
                name: LUTCreatorMod.Instance.Name,
                groupName: null, // default group
                tooltip: Translations.Translate("MOD_NAME"),
                icon: UUIHelpers.LoadTexture(UUIHelpers.GetFullPath<Lumina.LUTCreatorMod>("Resources", "UUI.png")),
                onToggle: (value) =>
                {
                    if (value)
                    {
                        StandalonePanelManager<LUTCreatorPanel>.Create();
                    }
                    else
                    {
                        StandalonePanelManager<LUTCreatorPanel>.Panel?.Close();
                    }
                },
                hotkeys: new UUIHotKeys { ActivationKey = ModSettings.ToggleKey });


        }



        /// <summary>
        /// Destroys the active instance.
        /// </summary>
        internal static void Destroy() => s_instance = null;

        /// <summary>
        /// Resets the UUI button to the non-pressed state.
        /// </summary>
        internal void ResetButton() => _uuiButton.IsPressed = false;



        //ColorCorrection

        internal int SelectedLut { get; set; }

        public string[] _lutnames;
        public bool _disableEvents;

        public string[] Names
        {
            get
            {
                if (_lutnames == null)
                {
                    _lutnames = SingletonResource<ColorCorrectionManager>.instance.items.ToArray();
                }

                return _lutnames;
            }
        }

        public static bool ShowButton = true;
        public static float ButtonPositionX { get; set; }
        public static float ButtonPositionY { get; set; }
        public static string BackgroundStyle { get; set; }

        public void ApplyLut(string name)
        {
            try
            {
                SingletonResource<ColorCorrectionManager>.instance.currentSelection = IndexOf(name);
            }
            catch (Exception e)
            {
                Debug.Log("[LUTCreator] Exception" + e.Message);
            }
        }

        public int IndexOf(string name)
        {
            try
            {
                int index = Array.FindIndex(Names, x => x.Equals(name));

                return index != -1 ? index : 0;
            }
            catch (Exception e)
            {
                Debug.Log("LUTCreator IndexOf failed" + e.Message);
                return 0;
            }
        }





        internal void OnSelectedIndexChanged(UIComponent component, int value)
        {
            if (_disableEvents) return;
            ColorCorrectionManager.instance.currentSelection = value;
        }
    }
}

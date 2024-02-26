namespace Lumina
{
    using AlgernonCommons.Keybinding;
    using AlgernonCommons.Translation;
    using AlgernonCommons.UI;
    using ColossalFramework.UI;
    using LUTCreator;
    using LUTCreator.UI;
    using System.Diagnostics;
    using UnityEngine;

    /// <summary>
    /// The mod's settings options panel.
    /// </summary>
    public sealed class OptionsPanel : OptionsPanelBase
    {
        // Layout constants.
        private const float Margin = 5f;
        private const float LeftMargin = 24f;
        private const float GroupMargin = 40f;

        string[] UIStyles = new string[] { "Transparent", "Normal" };

        public string[] VisibilityStatus = new string[] { "Both", "Only UUI" };

        public object LuminaLogic { get; private set; }

        /// <summary>
        /// Performs on-demand panel setup.
        /// </summary>
        protected override void Setup()
        {
            autoLayout = false;
            float currentY = Margin;

            UISprite image2Sprite = this.AddUIComponent<UISprite>();

            image2Sprite.height = 1000f;
            image2Sprite.relativePosition = new Vector3(0f, -50f);
            image2Sprite.width = 1000f;
            image2Sprite.atlas = UITextures.LoadSingleSpriteAtlas("..\\Resources\\bck");
            image2Sprite.spriteName = "normal";
            image2Sprite.zOrder = 1;

            // Language choice.
            UIDropDown languageDropDown = UIDropDowns.AddPlainDropDown(this, LeftMargin, currentY, Translations.Translate("LANGUAGE_CHOICE"), Translations.LanguageList, Translations.Index);
            languageDropDown.eventSelectedIndexChanged += (c, index) =>
            {
                Translations.Index = index;
                OptionsPanelManager<OptionsPanel>.LocaleChanged();
            };
            currentY += languageDropDown.parent.height + GroupMargin;

            // Hotkey control.
            OptionsKeymapping uuiKeymapping = OptionsKeymapping.AddKeymapping(this, LeftMargin, currentY, Translations.Translate("HOTKEY"), ModSettings.ToggleKey.Keybinding);
            currentY += uuiKeymapping.Panel.height + GroupMargin;

            UIDropDown UIStyleDropdown = UIDropDowns.AddLabelledDropDown(this, LeftMargin, currentY, Translations.Translate(LuminaTR.TranslationID.UISTYLE));
            UIStyleDropdown.items = UIStyles;
            currentY += 80f;
            if (LUTCreatorLogic.BackgroundStyle == "LuminaNormal")
            {
                UIStyleDropdown.selectedValue = "Transparent";
            }
            else if (LUTCreatorLogic.BackgroundStyle == "UnlockingItemBackground")
            {
                UIStyleDropdown.selectedValue = "Normal";
            }
            UIStyleDropdown.eventSelectedIndexChanged += (component, value) =>
            {
                int index = UIStyleDropdown.selectedIndex;
                if (UIStyles[index] == "Transparent")
                {
                    LUTCreatorLogic.BackgroundStyle = "LuminaNormal";
                    ModSettings.Save();
                }
                else if (UIStyles[index] == "Normal")
                {
                    LUTCreatorLogic.BackgroundStyle = "UnlockingItemBackground";
                    ModSettings.Save();
                }
            };
            currentY += 30f;

            /// Button Visibility Status dropdown
            UIDropDown ButtonVisibleToggle = UIDropDowns.AddLabelledDropDown(this, LeftMargin, currentY, Translations.Translate(LuminaTR.TranslationID.VISIBILITY_STATUS));
            ButtonVisibleToggle.items = VisibilityStatus;
            if (LUTCreatorLogic.ShowButton == false)
            {
                ButtonVisibleToggle.selectedValue = "Only UUI";
            }
            else if (LUTCreatorLogic.ShowButton == true)
            {
                ButtonVisibleToggle.selectedValue = "Both";
            }
            ButtonVisibleToggle.eventSelectedIndexChanged += (component, value) =>
            {
                int index = ButtonVisibleToggle.selectedIndex;
                if (VisibilityStatus[index] == "Only UUI")
                {
                    LUTCreatorLogic.ShowButton = false;
                    ModSettings.Save();

                }
                else if (VisibilityStatus[index] == "Both")
                {
                    LUTCreatorLogic.ShowButton = true;
                    ModSettings.Save();
                }
            };
            currentY += 50f;



        }

        /// <summary>
        /// Opens the LUT editor.
        /// </summary>
        private void OpenLUTEditor()
        {
            // TODO: fix to use package path.
            string lutEditorPath = @"C:\Program Files (x86)\Steam\steamapps\workshop\content\255710\2983036781\LUT Editor\";

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = lutEditorPath;
            startInfo.UseShellExecute = true;

            Process.Start(startInfo);
        }
    }
}
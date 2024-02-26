namespace Lumina
{
    using AlgernonCommons.Translation;
    using AlgernonCommons.UI;
    using ColossalFramework.UI;
    using Lumina.CompatibilityPolice;
    using Lumina.CompChecker;
    using LUTCreator;
    using System.Collections.Generic;

    /// <summary>
    /// Lumina panel tab for setting lighting options.
    /// </summary>
    internal sealed class LightingTab : PanelTabBase
    {
        // Panel components.
        private UISlider _luminositySlider;
        private UISlider _gammaSlider;
        private UISlider _contrastSlider;
        private UISlider _hueSlider;
        private UISlider _tintSlider;
        private UISlider _sunTempSlider;
        private UISlider _sunTintSlider;
        private UISlider _skyTempSlider;
        private UISlider _skyTintSlider;
        private UISlider _moonTempSlider;
        private UISlider _moonTintSlider;
        private UISlider _moonLightSlider;
        private UISlider _twilightTintSlider;
        private UICheckBox _skyTonemappingCheck;
        private UILabel _disabledLabel;
        private UILabel _causeLabel;
        private UILabel LUTLabel;
        private UIDropDown _lutdropdown;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightingTab"/> class.
        /// </summary>
        /// <param name="tabStrip">Tab strip to add to.</param>
        /// <param name="tabIndex">Index number of tab.</param>
        internal LightingTab(UITabstrip tabStrip, int tabIndex)
        {
            // Add tab.
            UIPanel panel = UITabstrips.AddTextTab(tabStrip, Translations.Translate(LuminaTR.TranslationID.LIGHTING_TEXT), tabIndex, out UIButton _);
            float currentY = Margin * 2f;



                UILabels.AddLabel(panel, Margin, currentY, Translations.Translate(LuminaTR.TranslationID.LUT_TEXT), panel.width - (Margin * 2f), alignment: UIHorizontalAlignment.Center);
                currentY += 30f;


                _lutdropdown = UIDropDowns.AddLabelledDropDown(panel, Margin, currentY, Translations.Translate(LuminaTR.TranslationID.LUT_TEXT), itemTextScale: 0.7f, width: panel.width - (Margin * 2f));
                currentY += 30f;

                // Define a dictionary to hold the mapping of lowercased names
                Dictionary<string, string> nameMapping = new Dictionary<string, string>
{
    { "LUTSunny", "Temperate" },
    { "lutnorth", "Boreal" },
    { "luttropical", "Tropical" },
    { "luteurope", "European" },
    { "lutcold", "Cold" },
    { "lutdark", "Dark" },
    { "lutfaded", "Faded" },
    { "lutneutral", "Neutral" },
    { "lutvibrant", "Vibrant" },
    { "lutwarm", "Warm" }
};

                // Create a List<string> to hold the modified items
                List<string> modifiedItems = new List<string>();

                foreach (var item in ColorCorrectionManager.instance.items)
                {
                    // Check if the item name matches any lowercased name in the mapping
                    string lowercasedName = item.ToLower();
                    if (nameMapping.ContainsKey(lowercasedName))
                    {
                        // If so, add the mapped value to the modified list
                        modifiedItems.Add(nameMapping[lowercasedName]);
                    }
                    else
                    {
                        // If not, process the item name according to the dot-separated rule
                        int dotIndex = item.LastIndexOf('.');
                        if (dotIndex >= 0 && dotIndex < item.Length - 1)
                        {
                            modifiedItems.Add(item.Substring(dotIndex + 1));
                        }
                    }
                }

                // Set the modified items to the dropdown
                _lutdropdown.items = modifiedItems.ToArray(); // Convert back to array if necessary

                _lutdropdown.selectedIndex = ColorCorrectionManager.instance.lastSelection;
                _lutdropdown.eventSelectedIndexChanged += LUTCreatorLogic.Instance.OnSelectedIndexChanged;
                _lutdropdown.localeID = LocaleID.BUILTIN_COLORCORRECTION;
            




        }
    }
}

   

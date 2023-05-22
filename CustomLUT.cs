using System;
using UnityEngine;
using ColossalFramework.UI;
using ColossalFramework.IO;
using ICities;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

namespace CustomLUTMod
{
    public class CustomLUTMod : IUserMod
    {
        public string Name => "LUT Creator";
        public string Description => "Allows players to create and edit their own LUTs within the game.";

        public void OnEnabled()
        {
            
        
    





    // Add a button to the game's main menu for opening the LUT editor panel
    UIComponent button = UIView.GetAView().AddUIComponent(typeof(UIButton)) as UIButton;
            button.name = "CustomLUTModButton";
            button.tooltip = "Custom LUT";
            button.width = 100;
            button.height = 30;
            button.relativePosition = new Vector3(80, 80);

            // Subscribe to the button's click event
            button.eventClick += OnButtonClick;
        }

        private void OnButtonClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            // Create a new LUT editor panel
            CustomLUTPanel panel = UIView.GetAView().AddUIComponent(typeof(CustomLUTPanel)) as CustomLUTPanel;
            panel.name = "CustomLUTPanel";
            panel.Show();
        }
    }

    public class CustomLUTPanel : UIPanel
    {
        private UISlider contrastSlider;
        private UISlider brightnessSlider;
        private UISlider saturationSlider;
        // Add more sliders for other LUT parameters as desired

        public override void Start()
        {
            base.Start();

        // Set up the panel's layout
             size = new Vector2(300, 200);
            backgroundSprite = "MenuPanel2";
            color = new Color32(255, 255, 255, 255);
            isVisible = false;

            // Add a label for the panel title
            UILabel titleLabel = AddUIComponent<UILabel>();
            titleLabel.text = "Custom LUT Editor";
            titleLabel.textScale = 1.2f;
            titleLabel.relativePosition = new Vector3(10, 10);

            // Add a contrast slider
            UILabel contrastLabel = AddUIComponent<UILabel>();
            contrastLabel.text = "Contrast";
            contrastLabel.relativePosition = new Vector3(10, 40);

            contrastSlider = AddUIComponent<UISlider>();
            contrastSlider.width = 280;
            contrastSlider.height = 20;
            contrastSlider.relativePosition = new Vector3(10, 60);
            contrastSlider.minValue = -1f;
            contrastSlider.maxValue = 1f;
            contrastSlider.value = 0f;
            contrastSlider.eventValueChanged += OnSliderValueChanged;

            // Add a brightness slider
            UILabel brightnessLabel = AddUIComponent<UILabel>();
            brightnessLabel.text = "Brightness";
            brightnessLabel.relativePosition = new Vector3(10, 90);

            brightnessSlider = AddUIComponent<UISlider>();
            brightnessSlider.width = 280;
            brightnessSlider.height = 20;
            brightnessSlider.relativePosition = new Vector3(10, 110);
            brightnessSlider.minValue = -1f;
            brightnessSlider.maxValue = 1f;
            brightnessSlider.value = 0f;
            brightnessSlider.eventValueChanged += OnSliderValueChanged;

            // Add a saturation slider
            UILabel saturationLabel = AddUIComponent<UILabel>();
            saturationLabel.text = "Saturation";
            saturationLabel.relativePosition = new Vector3(10, 140);

            saturationSlider = AddUIComponent<UISlider>();
            saturationSlider.width = 280;
            saturationSlider.height = 20;        saturationSlider.relativePosition = new Vector3(10, 160);
            saturationSlider.minValue = -1f;
            saturationSlider.maxValue = 1f;
            saturationSlider.value = 0f;
            saturationSlider.eventValueChanged += OnSliderValueChanged;

            // Add a button for applying the LUT changes
            UIButton applyButton = AddUIComponent<UIButton>();
            applyButton.text = "Apply";
            applyButton.width = 100;
            applyButton.height = 30;
            applyButton.relativePosition = new Vector3(180, 180);
            applyButton.eventClick += OnApplyButtonClick;
        }

        private void OnSliderValueChanged(UIComponent component, float value)
        {
            // Update the LUT with the new slider values
           
        }

        private void OnApplyButtonClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            // Apply the new LUT to the game
            ApplyLUT();
        }

        private void ApplyLUT()
        {
            // Apply the current LUT to the game permanently
            Camera.main.GetComponent<ColorCorrectionLookup>().enabled = false;
            Camera.main.GetComponent<ColorCorrectionLookup>().enabled = true;
        }
    }

    public class LoadingExtension : LoadingExtensionBase
    {
        public override void OnLevelLoaded(LoadMode mode)
        {
            if (mode != LoadMode.LoadGame && mode != LoadMode.NewGame)
                return;

            // Display warning if player has installed LUT Creator Mod (currently a testing environment)
            UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage(
                "LUT Creator Mod",
                "You've installed the LUT Creator Mod, which is currently a testing environment and has no functionality. If you prefer to uninstall it, please do. Expect instability.",
                false);
        }
    }
}


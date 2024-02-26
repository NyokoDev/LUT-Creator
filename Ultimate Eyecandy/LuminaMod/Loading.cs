namespace Lumina
{
    using System.Collections.Generic;
    using AlgernonCommons.Patching;
    using ICities;
    using LUTCreator;
    using LUTCreator.UI;
    using UnityEngine;


    /// <summary>
    /// Main loading class: the mod runs from here.
    /// </summary>
    public sealed class Loading : PatcherLoadingBase<OptionsPanel, PatcherBase>
    {
        /// <summary>
        /// Gets a list of permitted loading modes.
        /// </summary>
        /// 
        public ColorCorrectionManager colorCorrectionManager;
        private GameObject LUTCreatorButton;

        protected override List<AppMode> PermittedModes => new List<AppMode> { AppMode.Game, AppMode.MapEditor, AppMode.AssetEditor, AppMode.ThemeEditor, AppMode.ScenarioEditor };

        /// <summary>
        /// Called by the game when exiting a level.
        /// </summary>
        public override void OnLevelUnloading()
        {
            base.OnLevelUnloading();

            // Destroy any existing Lumina logic.
            LUTCreatorLogic.Destroy();
        }

        private UnityEngine.GameObject _gameObject;
        /// <summary>
        /// Performs any actions upon successful level loading completion.
        /// </summary>
        /// <param name="mode">Loading mode (e.g. game, editor, scenario, etc.).</param>
        protected override void LoadedActions(LoadMode mode)
        {
            base.LoadedActions(mode);

            // Create logic instance.
            LUTCreatorLogic.OnLoad();

            // Inititalizes the in-game button.
            LUTCreatorButton = new GameObject("LUTCreatorButtonGameObject");
            LUTCreatorButton.AddComponent<ButtonInitializer>();
        }
    }
}

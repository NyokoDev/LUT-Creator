using AlgernonCommons.UI;
using ColossalFramework.UI;
using Lumina;
using LUTCreator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace LUTCreator.UI
{
    internal class ButtonInitializer : MonoBehaviour
    {
        private UIPanel _buttonPanel;

        private UITextureAtlas _LUTCreatorAtlas;

        private UIButton _button;

        /// <summary>
        /// Creates the button.
        /// </summary>
        public void Start()
        {
            _LUTCreatorAtlas = LoadResources();
            CreateUI();

        }

        public static UITextureAtlas CreateTextureAtlas(string atlasName, string[] spriteNames, string assemblyPath)
        {
            int maxSize = 1024;
            Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            Texture2D[] textures = new Texture2D[spriteNames.Length];
            Rect[] regions = new Rect[spriteNames.Length];

            for (int i = 0; i < spriteNames.Length; i++)
            {
                textures[i] = LoadTextureFromAssembly(assemblyPath + spriteNames[i] + ".png");
            }

            regions = texture2D.PackTextures(textures, 2, maxSize);

            UITextureAtlas textureAtlas = ScriptableObject.CreateInstance<UITextureAtlas>();
            Material material = UnityEngine.Object.Instantiate(UIView.GetAView().defaultAtlas.material);
            material.mainTexture = texture2D;
            textureAtlas.material = material;
            textureAtlas.name = atlasName;

            for (int i = 0; i < spriteNames.Length; i++)
            {
                UITextureAtlas.SpriteInfo item = new UITextureAtlas.SpriteInfo
                {
                    name = spriteNames[i],
                    texture = textures[i],
                    region = regions[i],
                };

                textureAtlas.AddSprite(item);
            }

            return textureAtlas;
        }

        public static Texture2D LoadTextureFromAssembly(string path)
        {
            Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);

            byte[] array = new byte[manifestResourceStream.Length];
            manifestResourceStream.Read(array, 0, array.Length);

            Texture2D texture2D = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            texture2D.LoadImage(array);

            return texture2D;
        }

        public static UITextureAtlas GetAtlas(string name)
        {
            UITextureAtlas[] atlases = Resources.FindObjectsOfTypeAll(typeof(UITextureAtlas)) as UITextureAtlas[];
            for (int i = 0; i < atlases.Length; i++)
            {
                if (atlases[i].name == name)
                    return atlases[i];
            }

            return UIView.GetAView().defaultAtlas;
        }

        public static void AddTexturesInAtlas(UITextureAtlas atlas, Texture2D[] newTextures, bool locked = false)
        {
            Texture2D[] textures = new Texture2D[atlas.count + newTextures.Length];

            for (int i = 0; i < atlas.count; i++)
            {
                Texture2D texture2D = atlas.sprites[i].texture;

                if (locked)
                {
                    RenderTexture renderTexture = RenderTexture.GetTemporary(texture2D.width, texture2D.height, 0);
                    Graphics.Blit(texture2D, renderTexture);

                    RenderTexture active = RenderTexture.active;
                    texture2D = new Texture2D(renderTexture.width, renderTexture.height);
                    RenderTexture.active = renderTexture;
                    texture2D.ReadPixels(new Rect(0f, 0f, renderTexture.width, renderTexture.height), 0, 0);
                    texture2D.Apply();
                    RenderTexture.active = active;

                    RenderTexture.ReleaseTemporary(renderTexture);
                }

                textures[i] = texture2D;
                textures[i].name = atlas.sprites[i].name;
            }

            for (int i = 0; i < newTextures.Length; i++)
            {
                textures[atlas.count + i] = newTextures[i];
            }

            Rect[] regions = atlas.texture.PackTextures(textures, atlas.padding, 4096, false);

            atlas.sprites.Clear();

            for (int i = 0; i < textures.Length; i++)
            {
                UITextureAtlas.SpriteInfo spriteInfo = atlas[textures[i].name];
                atlas.sprites.Add(new UITextureAtlas.SpriteInfo
                {
                    texture = textures[i],
                    name = textures[i].name,
                    border = (spriteInfo != null) ? spriteInfo.border : new RectOffset(),
                    region = regions[i]
                });
            }

            atlas.RebuildIndexes();
        }

        private UITextureAtlas LoadResources()
        {
            try
            {
                if (_LUTCreatorAtlas == null)
                {
                    string[] spriteNames = new string[]
                    {
                        "ADV"
                    };

                    _LUTCreatorAtlas = CreateTextureAtlas("LUTCreatorAtlas", spriteNames, "LUTCreator.Resources.");

                    UITextureAtlas defaultAtlas = GetAtlas("Ingame");
                    Texture2D[] textures = new Texture2D[]
                    {
                        defaultAtlas["OptionLandscapingDisabled"].texture,
                        defaultAtlas["OptionBaseFocused"].texture,
                        defaultAtlas["OptionLandscapingHovered"].texture,
                        defaultAtlas["OptionLandscapingPressed"].texture,
                        defaultAtlas["LandscapingOptionBrushLargeDisabled"].texture
                    };

                    AddTexturesInAtlas(_LUTCreatorAtlas, textures);
                }

                return _LUTCreatorAtlas;
            }
            catch (Exception e)
            {
                Debug.Log("[LUTCreator] ModManager:LoadResources -> Exception: " + e.Message);
                return null;
            }
        }


        
        public static UIPanel CreatePanel(string name)
        {
            UIPanel panel = UIView.GetAView()?.AddUIComponent(typeof(UIPanel)) as UIPanel;
            panel.name = name;

            return panel;
        }

        public static UIButton CreateButton(UIComponent parent, string name, UITextureAtlas atlas, string spriteName)
        {
            UIButton button = parent.AddUIComponent<UIButton>();
            button.name = name;
            button.atlas = atlas;

            button.normalBgSprite = "OptionLandscapingDisabled";
            button.hoveredBgSprite = "OptionLandscapingHovered";
            button.pressedBgSprite = "OptionLandscapingPressed";
            button.disabledBgSprite = "OptionBaseDisabled";

            button.foregroundSpriteMode = UIForegroundSpriteMode.Stretch;
            button.normalFgSprite = spriteName;
            button.hoveredFgSprite = spriteName;
            button.pressedFgSprite = spriteName;
            button.disabledFgSprite = spriteName;

            return button;
        }



        private void UpdateUI()
        {
            try
            {
                _buttonPanel.isVisible = LUTCreatorLogic.ShowButton;
                _buttonPanel.absolutePosition = new Vector3(LUTCreatorLogic.ButtonPositionX, LUTCreatorLogic.ButtonPositionY);
            }
            catch (Exception e)
            {
                Debug.Log("[LUTCreator] ModManager:UpdateUI -> Exception: " + e.Message);
            }
        }

        private void CreateUI()
        {
            try
            {
                _buttonPanel = CreatePanel("LUTCreatorButtonPanel");
                _buttonPanel.isVisible = LUTCreatorLogic.ShowButton;
                _buttonPanel.zOrder = 25;
                _buttonPanel.size = new Vector2(36f, 36f);
                _buttonPanel.eventMouseMove += (component, eventParam) =>
                {
                    if (eventParam.buttons.IsFlagSet(UIMouseButton.Right))
                    {
                        var ratio = UIView.GetAView().ratio;
                        component.position = new Vector3(component.position.x + (eventParam.moveDelta.x * ratio), component.position.y + (eventParam.moveDelta.y * ratio), component.position.z);
                        LUTCreatorLogic.ButtonPositionX = component.absolutePosition.x;
                        LUTCreatorLogic.ButtonPositionY = component.absolutePosition.y;
                        ModSettings.Save();
                    }
                };

                _button = CreateButton(_buttonPanel, "LUTCreatorInButton", _LUTCreatorAtlas, "ADV");
                _button.tooltip = "LUTCreator";
                _button.size = new Vector2(36f, 36f);
                _button.relativePosition = new Vector3(0f, 0f);
                _button.eventClick += (component, eventParam) =>
                {
                    if (!eventParam.used)
                    {


                        StandalonePanelManager<LUTCreatorPanel>.Create();
                    }
                    else
                    {
                        StandalonePanelManager<LUTCreatorPanel>.Panel?.Close();
                    }


                    eventParam.Use();
                };


            }
            catch (Exception e)
            {

                Debug.Log("[LUTCreator] ModManager:CreateUI -> Error: " + e.Message);
            }
        }
    }
}

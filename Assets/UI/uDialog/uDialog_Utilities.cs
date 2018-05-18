using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace UI.Dialogs
{ 
    public static class uDialog_Utilities
    {
        static Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();        
        public static Dictionary<string, uDialog_ColorScheme> ColorSchemes = new Dictionary<string, uDialog_ColorScheme>();
        public static List<string> DefaultColorSchemeNames = new List<string>() { "Light", "Dark", "Plain" };

        public static uDialog_Config Config { get; private set; }

        public static string[] ColorSchemeNames
        {
            get
            {
                var names = ColorSchemes.Keys.ToList();
                names.Add("Custom");

                return names.ToArray();
            }
        }

        static uDialog_Utilities()
        {
            Config = Resources.Load("Configuration/uDialog_Config") as uDialog_Config;
            LoadColorSchemes();
            InitializeDefaultThemes();
        }

        public static void InitializeDefaultThemes()
        {
            bool changed = false;

            if (!ColorSchemes.ContainsKey("Light"))
            {
                ColorSchemes.Add("Light", new uDialog_ColorScheme()
                {
                    Name = "Light",
                    TitleBackground = new Color(0.5f, 0.5f, 0.5f),
                    TitleText = new Color(1, 1, 1),
                    TitleTextEffect = new Color(0,0,0),
                    ViewportBackground = new Color(1, 1, 1),
                    ViewportText = new Color(0, 0, 0),
                    ViewportTextEffect = new Color(0.5f, 0.5f, 0.5f),
                    ButtonBackground = new Color(0.9f, 0.9f, 0.9f),
                    ButtonText = new Color(0, 0, 0),
                    ButtonTextEffect = new Color(0, 0, 0),
                    Glow = new Color(1, 1, 1),
                    Shadow = new Color(0, 0, 0),
                    ScreenOverlay = new Color(0f, 0f, 0f, 0.25f),
                    Icon_Information = new Color(1, 1, 1),
                    Icon_Question = new Color(1, 0.75f, 0.75f),
                    Icon_Warning = new Color(0.9f, 0.75f, 0.5f)
                });

                changed = true;
            }

            if (!ColorSchemes.ContainsKey("Dark"))
            {
                ColorSchemes.Add("Dark", new uDialog_ColorScheme()
                {
                    Name = "Dark",
                    TitleBackground = new Color(0.125f, 0.125f, 0.125f),
                    TitleText = new Color(1, 1, 1),
                    TitleTextEffect = new Color(0, 0, 0),
                    ViewportBackground = new Color(0.25f, 0.25f, 0.25f),
                    ViewportText = new Color(1, 1, 1),
                    ViewportTextEffect = new Color(0f, 0f, 0f),
                    ButtonBackground = new Color(0.1f, 0.1f, 0.1f),
                    ButtonText = new Color(1, 1, 1),
                    ButtonTextEffect = new Color(0, 0, 0),
                    Glow = new Color(0, 0, 0),
                    Shadow = new Color(0, 0, 0),
                    ScreenOverlay = new Color(0f, 0f, 0f, 0.25f),
                    Icon_Information = new Color(1, 1, 1),
                    Icon_Question = new Color(1, 0.75f, 0.75f),
                    Icon_Warning = new Color(0.9f, 0.75f, 0.5f)
                });

                changed = true;
            }

            if (!ColorSchemes.ContainsKey("Plain"))
            {
                ColorSchemes.Add("Plain", new uDialog_ColorScheme()
                {
                    Name = "Plain",
                    TitleBackground = new Color(1,1,1),
                    TitleText = new Color(0, 0, 0),
                    TitleTextEffect = new Color(0.5f, 0.5f, 0.5f),
                    ViewportBackground = new Color(1,1,1),
                    ViewportText = new Color(0, 0, 0),
                    ViewportTextEffect = new Color(0.5f, 0.5f, 0.5f),
                    ButtonBackground = new Color(1,1,1),
                    ButtonHighlight = new Color(0.9f, 0.9f, 0.9f),
                    ButtonText = new Color(0, 0, 0),
                    ButtonTextEffect = new Color(0, 0, 0),
                    Glow = new Color(1,1,1),
                    Shadow = new Color(0, 0, 0),
                    ScreenOverlay = new Color(0f, 0f, 0f, 0.25f),
                    Icon_Information = new Color(1, 1, 1),
                    Icon_Question = new Color(1, 0.75f, 0.75f),
                    Icon_Warning = new Color(0.9f, 0.75f, 0.5f)
                });

                changed = true;
            }

            if (changed)
            {
                SaveColorSchemes();
            }
        }

        /// <summary>
        /// Load a sprite, and store it in memory in case we need it again.
        /// If the sprite has already been loaded, this function will return it without reloading it.
        /// </summary>
        /// <param name="spriteName"></param>
        /// <returns></returns>
        public static Sprite GetSprite(string spriteName)
        {
            Sprite sprite = null;

            if (!sprites.ContainsKey(spriteName))
            {
                sprite = Resources.Load<Sprite>(spriteName);

                if (sprite != null)
                {
                    sprites.Add(spriteName, sprite);
                }
            }
            else
            {
                sprite = sprites[spriteName];
            }

            return sprite;
        }

        public static string AddColorScheme(string name, uDialog_ColorScheme scheme)
        {
            if (ColorSchemes.ContainsKey(name)) ColorSchemes.Remove(name);

            ColorSchemes.Add(name, scheme);

            return SaveColorSchemes();
        }

        public static void DeleteColorScheme(string name)
        {
            if (ColorSchemes.ContainsKey(name))
            {
                ColorSchemes.Remove(name);

                SaveColorSchemes();
            }
        }

        public static string SaveColorSchemes()
        {            
            Config.ColorSchemes = ColorSchemes.Values.ToList();
                        
#if UNITY_EDITOR            
            UnityEditor.AssetDatabase.SaveAssets();
#endif

            LoadColorSchemes();

            // Success
            return "";
        }

        public static void LoadColorSchemes()
        {
            ColorSchemes = Config.ColorSchemes.ToDictionary(
                    k => k.Name,
                    v => v);

            UpdateAllDialogColorSchemes();
        }

        public static void UpdateAllDialogColorSchemes()
        {
            var allDialogs = GameObject.FindObjectsOfType<uDialog>();
            foreach (var dialog in allDialogs)
            {
                dialog.ColorSchemeChanged();
            }   
        }

        public static void FixInstanceTransform(RectTransform baseTransform, RectTransform instanceTransform)
        {
            instanceTransform.localPosition = baseTransform.localPosition;
            instanceTransform.position = baseTransform.position;
            instanceTransform.rotation = baseTransform.rotation;
            instanceTransform.localScale = baseTransform.localScale;
            instanceTransform.anchoredPosition = baseTransform.anchoredPosition;
            instanceTransform.sizeDelta = baseTransform.sizeDelta;
        }

        public static GameObject InstantiatePrefab(string name, bool playMode = false, bool generateUndo = true)
        {
            var prefab = Resources.Load<GameObject>("Prefabs/" + name);

            if (prefab == null)
            {
                throw new UnityException(String.Format("Could not find prefab '{0}'!", name));
            }

            Transform parent = null;

#if UNITY_EDITOR
            if(!playMode) parent = UnityEditor.Selection.activeTransform;            
#endif
            var gameObject = GameObject.Instantiate(prefab) as GameObject;
            gameObject.name = name;

            if (parent == null || !(parent is RectTransform))
            {
                parent = GetCanvasTransform();
            }

            gameObject.transform.SetParent(parent);

            var transform = (RectTransform)gameObject.transform;
            var prefabTransform = (RectTransform)prefab.transform;

            FixInstanceTransform(prefabTransform, transform);

#if UNITY_EDITOR
            if (generateUndo)
            {                
                UnityEditor.Undo.RegisterCreatedObjectUndo(gameObject, "Created " + name);
            }
#endif

            return gameObject;
        }

        public static Transform GetCanvasTransform()
        {
            Canvas canvas = null;
#if UNITY_EDITOR
            // Attempt to locate a canvas object parented to the currently selected object
            if (!Application.isPlaying && UnityEditor.Selection.activeGameObject != null)
            {
                canvas = FindParentOfType<Canvas>(UnityEditor.Selection.activeGameObject);
                //canvas = UnityEditor.Selection.activeTransform.GetComponentInParent<Canvas>();                
            }
#endif

            if (canvas == null)
            {
                // Attempt to find a canvas anywhere
                canvas = UnityEngine.Object.FindObjectOfType<Canvas>();                

                if (canvas != null) return canvas.transform;                
            }

            // if we reach this point, we haven't been able to locate a canvas
            // ...So I guess we'd better create one
            

            GameObject canvasGameObject = new GameObject("Canvas");
            canvasGameObject.layer = LayerMask.NameToLayer("UI");
            canvas = canvasGameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGameObject.AddComponent<CanvasScaler>();
            canvasGameObject.AddComponent<GraphicRaycaster>();

#if UNITY_EDITOR
            UnityEditor.Undo.RegisterCreatedObjectUndo(canvasGameObject, "Create Canvas");
#endif

            var eventSystem = UnityEngine.Object.FindObjectOfType<EventSystem>();

            if (eventSystem == null)
            {
                GameObject eventSystemGameObject = new GameObject("EventSystem");
                eventSystem = eventSystemGameObject.AddComponent<EventSystem>();
                eventSystemGameObject.AddComponent<StandaloneInputModule>();

#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
                eventSystemGameObject.AddComponent<TouchInputModule>();
#endif

#if UNITY_EDITOR
                UnityEditor.Undo.RegisterCreatedObjectUndo(eventSystemGameObject, "Create EventSystem");
#endif
            }

            return canvas.transform;
        }

        public static T FindParentOfType<T>(GameObject childObject)
            where T : UnityEngine.Object
        {
            Transform t = childObject.transform;
            while (t.parent != null)
            {
                var component = t.parent.GetComponent<T>();

                if (component != null) return component;

                t = t.parent.transform;
            }

            // We didn't find anything
            return null;
        }

#if UNITY_EDITOR
        public static void WrapSelectedContent()
        {
            var selectedContent = UnityEditor.Selection.activeTransform as RectTransform;
            if (selectedContent == null)
            {
                UnityEditor.EditorUtility.DisplayDialog("No RectTransform Selected!", "Only content with a RectTransform can be wrapped!", "Okay");
                return;
            }

            var parent = selectedContent.parent;
            if ((RectTransform)parent.transform != null)
            {
                UnityEditor.Selection.activeTransform = parent.transform;
            }
            
            //UnityEditor.Undo.RecordObject(selectedContent, "uDialog - Wrap Content " + selectedContent.name);

            var windowObject = InstantiatePrefab("uDialog_Window");            
            var uDialogComponent = windowObject.GetComponent<uDialog>();            
                        
            uDialogComponent.SetContent(selectedContent);

            UnityEditor.Selection.activeTransform = windowObject.transform;
            
            //UnityEditor.Undo.RegisterCreatedObjectUndo(windowObject, "uDialog - Wrap Content " + selectedContent.name);
        }
#endif
    }
}

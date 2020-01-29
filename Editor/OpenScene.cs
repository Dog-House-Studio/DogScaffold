#if UNITY_EDITOR
#if ODIN_INSPECTOR
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
#endif
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEditor;
using UnityEditor.SceneManagement;

using UnityEngine;


namespace DogScaffold
{
    /// <summary>
    /// Opens an interactive window that allows for OSX Spotlight-esque scene switching.
    /// </summary>
    public class OpenScene : EditorWindow
    {
        protected const string kFocusName = "SceneOpenerWindow.SceneFilterControl";

        protected string searchText = "";
        protected Vector2 scrollPosition;
        protected string selected = null;
        protected int selectedIndex = 0;

        protected static List<SceneInformation> m_scenes;

        protected static List<SceneInformation> Scenes
        {
            get
            {
                if (m_scenes == null)
                {
                    CacheProjectSceneList();
                }

                return m_scenes;
            }
        }


        [MenuItem("DogHouse/(Editor) Open Scene Quickly... %t", priority = -10)]
        protected static void OpenWindow()
        {
            CacheProjectSceneList();

#if ODIN_INSPECTOR
            // Position the window in the middle of the editor.
            var window = GetWindow(
                typeof(OpenScene),
                true,
                "Open Scene"
            );

            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 700);
#else
            GetWindow(
                typeof(OpenScene),
                true,
                "Open Scene"
            );
#endif
        }


        protected void OnGUI()
        {
            if (Event.current.type == EventType.KeyDown)
            {
                switch (Event.current.keyCode)
                {
                    case KeyCode.Escape:
                        Event.current.Use();
                        SafeCloseWindow();
                        break;
                    case KeyCode.UpArrow:
                        Event.current.Use();
                        --selectedIndex;
                        selectedIndex = Mathf.Max(0, selectedIndex);
                        break;
                    case KeyCode.DownArrow:
                        Event.current.Use();
                        ++selectedIndex;
                        break;
                    case KeyCode.KeypadEnter:
                    case KeyCode.Return:
                        Event.current.Use();
                        if (selected != null)
                        {
                            CloseWindowAndOpenScene(selected);
                        }
                        break;
                }
            }

            GUI.SetNextControlName(kFocusName);
            string newSearchText = EditorGUILayout.TextField(searchText);
            bool textChanged = newSearchText != searchText;
            searchText = newSearchText;
            EditorGUI.FocusTextInControl(kFocusName);

            string[] tokenizedSearch = searchText.ToLower().Split(' ');

            int resultIndex = 0;

            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            foreach (
                string scenePath in
                    from scenePath in GetProjectRelativeScenePaths()
                    where tokenizedSearch.All(
                        x => Path.GetFileNameWithoutExtension(scenePath)
                            .ToLower().Contains(x))
                    select scenePath)
            {
                if (textChanged && resultIndex == 0)
                {
                    selected = scenePath;
                    selectedIndex = resultIndex;
                }
                else if (resultIndex <= selectedIndex)
                {
                    selected = scenePath;
                }

                string sceneName = Path.GetFileNameWithoutExtension(scenePath);

                Rect elementRect = GUILayoutUtility.GetRect(
                    new GUIContent(sceneName),
                    EditorStyles.boldLabel
                );

                bool hover = elementRect.Contains(Event.current.mousePosition);

                if (Event.current.type == EventType.Repaint)
                {
                    EditorStyles.boldLabel.Draw(
                        elementRect,
                        sceneName,
                        hover,
                        false,
                        resultIndex == selectedIndex,
                        false
                    );
                }

                if (hover && Event.current.type == EventType.MouseDown)
                {
                    Event.current.Use();
                    CloseWindowAndOpenScene(scenePath);
                }
                ++resultIndex;
            }

            EditorGUILayout.EndScrollView();

            if (resultIndex == 0)
            {
                GUILayout.Label("No results");
                GUILayout.FlexibleSpace();
                selected = null;
            }
            else if (selectedIndex == resultIndex)
            {
                selectedIndex = resultIndex - 1;
            }
        }


        protected void OnLostFocus()
        {
            SafeCloseWindow();
        }


        protected void CloseWindowAndOpenScene(string scenePath)
        {
            if (scenePath == null)
            {
                Debug.LogError("SceneOpener told to open null scene! Ignoring.");
                return;
            }

            SafeCloseWindow();

            EditorApplication.delayCall += () =>
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    EditorSceneManager.OpenScene(scenePath);
                }
            };
        }


        protected void SafeCloseWindow()
        {
            EditorApplication.delayCall += Close;
        }


        protected static IEnumerable<string> GetProjectRelativeScenePaths()
        {
            return from SceneInformation s in Scenes select s.ProjectRelativePath;
        }


        protected static void CacheProjectSceneList()
        {
            m_scenes = new List<SceneInformation>();

            string[] sceneGUIDs = AssetDatabase.FindAssets("t:scene");
            string scenePath;

            foreach (var sceneGUID in sceneGUIDs)
            {
                scenePath = AssetDatabase.GUIDToAssetPath(sceneGUID);
                SceneInformation sceneInfo = new SceneInformation(scenePath);
                m_scenes.Add(sceneInfo);
            }
        }
    }
}
#endif

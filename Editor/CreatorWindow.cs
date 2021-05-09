using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GUA.Editor
{
    public class CreatorWindow : EditorWindow
    {
        private const string StarterTemplate = "Starter.cs.txt";
        private const string StarterEditorTemplate = "StarterEditor.cs.txt";

        private static bool _showWillBeGenerated;
        private static bool _createEditorScript;
        private static bool _createFolders;
        private static bool _createScriptsFolder;
        private static string _rootNamespace = string.Empty;
        private static string _selectedPath = string.Empty;
        private static string _starterName = "Starter";
        
        private static CreatorWindow _window;

        [MenuItem("Assets/Create/GUA/Creator", false, 0)]
        private static void ShowWindow()
        {
            _window = GetWindow<CreatorWindow>(true, "GUA Creator");
            _window.minSize = new Vector2(350, 320);
            _rootNamespace = EditorSettings.projectGenerationRootNamespace.Trim();
            _window.Show();
        }

        private void OnGUI()
        {
            if(_selectedPath == string.Empty) _selectedPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            
            _rootNamespace = EditorGUILayout.TextField("Root Namespace", _rootNamespace);
            _starterName = EditorGUILayout.TextField("Starter Name", _starterName);
            _createScriptsFolder = EditorGUILayout.Toggle("Create Script Folder", _createScriptsFolder);
            _createEditorScript = EditorGUILayout.Toggle("Create Editor Script", _createEditorScript);
            _createFolders = EditorGUILayout.Toggle("Create Folders", _createFolders);
            
            if (GUILayout.Button("Change Path"))
            {
                var path = EditorUtility.SaveFolderPanel("Select Folder", _selectedPath, string.Empty);
                if (CheckPath(path)) _selectedPath = TrimPath(path);
                else if (path != string.Empty) EditorUtility.DisplayDialog("Error", "Invalid path", "OK");
            }

            GUILayout.Space(10);

            _showWillBeGenerated = EditorGUILayout.Foldout(_showWillBeGenerated, "Will be generated");

            if (_showWillBeGenerated)
            {
                GUILayout.Label($"{_selectedPath}/{(_createScriptsFolder ? "Scripts/" : string.Empty)}{(_createFolders ? "Starters/" : string.Empty)}{_starterName}.cs");
                if (_createEditorScript) GUILayout.Label($"{_selectedPath}/{(_createScriptsFolder ? "Scripts/" : string.Empty)}Editor/{_starterName}Editor.cs");
                if (_createFolders)
                {
                    GUILayout.Label($"{_selectedPath}/{(_createScriptsFolder ? "Scripts/" : string.Empty)}Data");
                    GUILayout.Label($"{_selectedPath}/{(_createScriptsFolder ? "Scripts/" : string.Empty)}Components");
                    GUILayout.Label($"{_selectedPath}/{(_createScriptsFolder ? "Scripts/" : string.Empty)}Managers");
                    GUILayout.Label($"{_selectedPath}/{(_createScriptsFolder ? "Scripts/" : string.Empty)}Systems");
                }
            }

            GUILayout.Space(10);

            if (!GUILayout.Button("Create", GUILayout.Height(30))) return;
            
            Generate();
            AssetDatabase.Refresh();
            Close();
        }

        private static void Generate()
        {
            EditorSettings.projectGenerationRootNamespace = _rootNamespace.Trim();

            if (_createFolders)
            {
                CreateEmptyFolder($"{_selectedPath}/{(_createScriptsFolder ? "Scripts/" : string.Empty)}Editor");
                CreateEmptyFolder($"{_selectedPath}/{(_createScriptsFolder ? "Scripts/" : string.Empty)}Data");
                CreateEmptyFolder($"{_selectedPath}/{(_createScriptsFolder ? "Scripts/" : string.Empty)}Components");
                CreateEmptyFolder($"{_selectedPath}/{(_createScriptsFolder ? "Scripts/" : string.Empty)}Managers");
                CreateEmptyFolder($"{_selectedPath}/{(_createScriptsFolder ? "Scripts/" : string.Empty)}Systems");
                CreateEmptyFolder($"{_selectedPath}/{(_createScriptsFolder ? "Scripts/" : string.Empty)}Starters");
            }

            var starterPath = $"{_selectedPath}/{(_createScriptsFolder ? "Scripts/" : string.Empty)}{(_createFolders ? "Starters/" : string.Empty)}";
            var starterNamespace = CreateScript(starterPath, StarterTemplate, _starterName);

            if (!_createEditorScript) return;
            var starterEditorPath = $"{_selectedPath}/{(_createScriptsFolder ? "Scripts/" : string.Empty)}Editor/";
            _ = CreateScript(starterEditorPath, StarterEditorTemplate, $"{_starterName}Editor", starterNamespace);
        }

        private static bool CheckPath(string path)
        {
            return path.Contains("Assets");
        }

        private static string TrimPath(string path)
        {
            var startIndex = path.IndexOf("Assets", StringComparison.Ordinal);
            path = path.Substring(startIndex);
            return path;
        }

        private static void CreateEmptyFolder(string path)
        {
            if (Directory.Exists(path)) return;
            
            Directory.CreateDirectory(path);
            File.Create($"{path}/.gitkeep");
        }

        private static string CreateScript(string path, string templateName, string scriptName, string starterNamespace = null)
        {
            var templatePath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(_window)));
            var template = File.ReadAllText($"{templatePath}/{templateName}");
            var scriptNamespace = GetNamespace(path);

            var script = template.Replace("#SCRIPT_NAME#", scriptName);
            script = script.Replace("#STARTER_NAME#", _starterName);
            script = script.Replace("#NAMESPACE#", scriptNamespace);
            script = script.Replace("#STARTER_NAMESPACE#", starterNamespace);

            File.WriteAllText($"{path}/{scriptName}.cs", script);

            return scriptNamespace;
        }

        private static string GetNamespace(string path)
        {
            var result = string.Empty;
            
            var rootNamespace = _rootNamespace.Trim();
            if (!string.IsNullOrEmpty(rootNamespace)) result += rootNamespace;

            var pathsSplit = path.Split('/');
            var writeNamespace = false;
            foreach (var pathPart in pathsSplit)
            {
                if (string.IsNullOrEmpty(pathPart)) continue;
                if (pathPart == "Assets" || pathPart == "Scripts")
                {
                    writeNamespace = true;
                    continue;
                }
                if (writeNamespace) result += $"{(string.IsNullOrEmpty(result) ? string.Empty : ".")}{pathPart}";
            }
            
            return result;
        }
    }
}
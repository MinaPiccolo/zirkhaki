using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateAssetEditor
{
    private const string PathToFComponentTemplate = "FComponentTemplate";
    private const string PathToFClassTemplate = "FClassTemplate";
    private const string PathToGameSystemTemplate = "GameSystemTemplate";
    private const string PathToGameManagerTemplate = "GameManagerTemplate";
    private const string PathToSubsystemTemplate = "SubsystemTemplate";
    private const string PathToInterfaceTemplate = "InterfaceTemplate";

    [MenuItem("Assets/Create/Revy-Framework/FComponent")]
    [MenuItem("Revy/Create/FComponent")]
    private static void CreateNewFComponent()
    {
        CreateFrameworkScriptsFromTemplate(PathToFComponentTemplate,"FComponent",true);
    }

    [MenuItem("Assets/Create/Revy-Framework/FClass")]
    [MenuItem("Revy/Create/FClass")]
    private static void CreateNewFClass()
    {
        CreateFrameworkScriptsFromTemplate(PathToFComponentTemplate,"FClass",true);
    }


    [MenuItem("Assets/Create/Revy-Framework/GameSystem")]
    [MenuItem("Revy/Create/GameSystem")]
    private static void CreateNewGameSystem()
    {
        CreateFrameworkScriptsFromTemplate(PathToGameSystemTemplate,"GameSystem", true);
    }

    [MenuItem("Assets/Create/Revy-Framework/GameManager")]
    [MenuItem("Revy/Create/GameManager")]
    private static void CreateNewGameManager()
    {
        CreateFrameworkScriptsFromTemplate(PathToGameManagerTemplate,"GameManager", true);
    }

    [MenuItem("Assets/Create/Revy-Framework/Subsystem")]
    [MenuItem("Revy/Create/Subsystem")]
    private static void CreateNewSubsystem()
    {
        CreateFrameworkScriptsFromTemplate(PathToSubsystemTemplate,"Subsystem", true);
    }

    private static void CreateFrameworkScriptsFromTemplate(string templatePath,string templateTitle, bool hasInterface = false)
    {
        TextAsset template = Resources.Load<TextAsset>(templatePath);
        if (template == null)
        {
            EditorUtility.DisplayDialog("Error", "Template does not found!", "OK");
            return;
        }
        string templateString = template.text;
        string activeObjectPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (!AssetDatabase.IsValidFolder(activeObjectPath))
        {
            activeObjectPath = Path.GetDirectoryName(activeObjectPath);
        }

        string newFilePath = EditorUtility.SaveFilePanelInProject($"Create new {templateTitle}", $"New{templateTitle}", "cs", "Create", activeObjectPath);
        if (string.IsNullOrEmpty(newFilePath))
        {
            return;
        }

        string newFileName = Path.GetFileName(newFilePath);
        newFilePath = _renameFileName($"{_normalizeScriptName(newFileName)}", newFilePath);
        templateString = templateString.Replace("#ClassName#", Path.GetFileNameWithoutExtension(newFilePath));
        if (_createFile(templateString, newFilePath)) 
        {
            if (hasInterface)
            {
                _createInterface(PathToInterfaceTemplate, newFilePath);
            }
            AssetDatabase.ImportAsset(newFilePath);
            AssetDatabase.Refresh();
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(newFilePath);
        }
    }

    private static void _createInterface(string pathToTemplate, string newFilePath)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(pathToTemplate);
        if (textAsset == null)
        {
            EditorUtility.DisplayDialog("Error", "Interface template does not found!", "OK");
            return;
        }
        string templateString = textAsset.text;
        string scriptName = Path.GetFileNameWithoutExtension(newFilePath);
        string interfaceName = $"I{scriptName}";
        templateString = templateString.Replace("#InterfaceName#", interfaceName);
        string changedFilePath = _renameFileName($"{interfaceName}.cs", newFilePath);
        _createFile(templateString, changedFilePath);
    }

    private static bool _createFile(string file, string path)
    {
        bool result = true;
        try
        {
            StreamWriter writer = new StreamWriter(path);
            writer.Write(file);
            writer.Close();
        }
        catch
        {
            result = false;
        }
        return result;
    }

    private static string _renameFileName(string newFileName, string path)
    {
        string oldfileName = Path.GetFileName(path);
        string renamedPath = path.Remove(path.Length - oldfileName.Length);
        renamedPath = $"{renamedPath}{newFileName}";
        return renamedPath; 
    }

    //todo ideen: find better way to check script name.
    private static string _normalizeScriptName(string scriptName)
    {
        scriptName = scriptName.Replace(" ", string.Empty);
        scriptName = scriptName.Replace("-", "_");
        scriptName = scriptName.Replace(";", string.Empty);
        scriptName = scriptName.Replace(",", string.Empty);
        scriptName = scriptName.Replace("]", string.Empty);
        scriptName = scriptName.Replace("[", string.Empty);
        scriptName = scriptName.Replace("\\", string.Empty);
        scriptName = scriptName.Replace("/", string.Empty);
        scriptName = scriptName.Replace("@", string.Empty);
        scriptName = scriptName.Replace("#", string.Empty);
        scriptName = scriptName.Replace("$", string.Empty);
        scriptName = scriptName.Replace("%", string.Empty);
        scriptName = scriptName.Replace("^", string.Empty);
        scriptName = scriptName.Replace("&", string.Empty);
        scriptName = scriptName.Replace("*", string.Empty);
        scriptName = scriptName.Replace("(", string.Empty);
        scriptName = scriptName.Replace(")", string.Empty);
        scriptName = scriptName.Replace("+", string.Empty);
        scriptName = scriptName.Replace("-", string.Empty);
        scriptName = scriptName.Replace("~", string.Empty);

        return scriptName;
    }
}

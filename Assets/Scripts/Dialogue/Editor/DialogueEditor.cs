using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace RPG.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor", true);
        }

        [OnOpenAssetAttribute(1)]
        // This is called whenever any kind of asset is opened.
        public static bool OnOpenAsset(int instanceID, int line)
        {
            // "as" keyword will return null if this is not a Dialogue object.
            Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
            if (dialogue != null)
            {
                ShowEditorWindow();
                return true;
            }
            return false;
        }
    }
}
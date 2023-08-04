using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace RPG.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        Dialogue selectedDialogue = null;

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor", true);
        }

        [OnOpenAsset(1)]
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

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;
        }

        //  We can bypass subscribing to the event altogether, by naming the method
        //  OnSelectionChange() (no "d") and it will be picked up by the EditorWindow callbacks.
        private void OnSelectionChanged()
        {
            // Try to cast the active object to a Dialogue object
            Dialogue newDialogue = Selection.activeObject as Dialogue;
            
            // Set selectedDialogue or clear it to null if we aren't selecting a Dialogue
            selectedDialogue = newDialogue;

            // Redraw the GUI
            Repaint();
        }

        private void OnGUI()
        {
            if(selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue Selected.");
            }
            else
            {
                EditorGUILayout.LabelField(selectedDialogue.name);
            }
        }
    }
}
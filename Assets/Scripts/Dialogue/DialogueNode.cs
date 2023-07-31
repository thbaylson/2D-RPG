using System;

namespace RPG.Dialogue
{
    [Serializable]
    public class DialogueNode
    {
        public string ID;
        public string message;
        // An array of ID strings
        public string[] children;
    }
}
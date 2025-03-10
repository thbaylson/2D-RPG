using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string saveFile)
        {
            print($"Saving to {saveFile}");
        }

        public void Load(string saveFile)
        {
            print($"Loading to {saveFile}");
        }
    }
}
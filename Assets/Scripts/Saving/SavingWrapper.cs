using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save1";

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                GetComponent<JsonSavingSystem>().Save(defaultSaveFile);
            }
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                GetComponent<JsonSavingSystem>().Load(defaultSaveFile);
            }
        }
    }
}
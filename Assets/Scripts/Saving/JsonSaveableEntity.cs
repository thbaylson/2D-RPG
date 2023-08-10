using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class JsonSaveableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";
        [SerializeField] bool capturePosition = false;

        // Cached state
        static Dictionary<string, JsonSaveableEntity> globalLookup = new Dictionary<string, JsonSaveableEntity>();

        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public JToken CaptureAsJToken()
        {
            // JObject is a special container class that allows us to store a collection of Key/Value entries
            JObject state = new JObject();
            // state and stateDict are the same object, but this lets us treat it as a Dict before returning JObject
            IDictionary<string, JToken> stateDict = state;
            // Get a list of everything that is saveable
            foreach (IJsonSaveable jsonSaveable in GetComponents<IJsonSaveable>())
            {
                // Convert the saveable object into a JToken
                JToken token = jsonSaveable.CaptureAsJToken();
                string component = jsonSaveable.GetType().ToString();
                // StateDictionary[component] = token
                stateDict[jsonSaveable.GetType().ToString()] = token;
                Debug.Log($"{name} Capture {component} => {token.ToString()}");
            }

            if (capturePosition)
            {
                // Create a JToken of the position
                JToken posToken = JsonStatics.ToToken(transform.position);
                // Save the dict key under the Transform type
                string posKey = typeof(Transform).ToString();
                stateDict[posKey] = posToken;
                Debug.Log($"{name} Capture {posKey} => {posToken.ToString()}");
            }

            return state;
        }

        public void RestoreFromJToken(JToken s)
        {
            JObject state = s.ToObject<JObject>();
            IDictionary<string, JToken> stateDict = state;
            // Get a list of everything that is saveable
            foreach (IJsonSaveable jsonSaveable in GetComponents<IJsonSaveable>())
            {
                // Figure out the type
                string component = jsonSaveable.GetType().ToString();
                // If the dictionary contains that type
                if (stateDict.ContainsKey(component))
                {
                    // Restore the object from the JToken
                    jsonSaveable.RestoreFromJToken(stateDict[component]);
                    Debug.Log($"{name} Restore {component} => {stateDict[component]}");
                }
            }

            if (capturePosition)
            {
                string posKey = typeof(Transform).ToString();
                if (stateDict.ContainsKey(posKey))
                {
                    transform.position = JsonStatics.ToVector3(stateDict[posKey]);
                    Debug.Log($"{name} Restore {posKey} => {stateDict[posKey]}");
                }
            }
        }

#if UNITY_EDITOR
        private void Update()
        {
            // We want these serializations to be set pre-runtime
            if (Application.IsPlaying(gameObject)) { return; }
            if (string.IsNullOrEmpty(gameObject.scene.path)) { return; }

            // Find and assign the serialization of this monobehavior 
            SerializedObject serializedObject = new SerializedObject(this);
            // Find the "uniqueIdentifier" property
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");
            //Debug.Log($"Update: !IsUnique: {!IsUnique(property.stringValue)}");
            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            globalLookup[property.stringValue] = this;
        }
#endif

        private bool IsUnique(string candidate)
        {
            if (!globalLookup.ContainsKey(candidate)) { return true; }
            if (globalLookup[candidate] == this) { return true; }

            if (globalLookup[candidate] == null)
            {
                globalLookup.Remove(candidate);
                return true;
            }

            if (globalLookup[candidate].GetUniqueIdentifier() != candidate)
            {
                globalLookup.Remove(candidate);
                return true;
            }

            return false;
        }
    }
}
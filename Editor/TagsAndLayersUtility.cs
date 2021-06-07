using RedOwl.Engine;
using UnityEditor;

namespace RedOwl.Editor
{
    public static class TagsAndLayersUtility
    {
        private static int maxTags = 10000;
        private static int maxLayers = 31;

        private static SerializedObject Manager => new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);

        /// <summary>
        /// Checks if the value exists in the property.
        /// </summary>
        /// <returns><c>true</c>, if property exists, <c>false</c> otherwise.</returns>
        /// <param name="property">Property.</param>
        /// <param name="start">Start.</param>
        /// <param name="end">End.</param>
        /// <param name="value">Value.</param>
        private static bool PropertyExists(SerializedProperty property, int start, int end, string value)
        {
            for (int i = start; i < end; i++)
            {
                SerializedProperty t = property.GetArrayElementAtIndex(i);
                if (t.stringValue.Equals(value))
                {
                    return true;
                }
            }
            return false;
        }

        #region Tags

        /// <summary>
        /// Checks to see if tag exists.
        /// </summary>
        /// <returns><c>true</c>, if tag exists, <c>false</c> otherwise.</returns>
        /// <param name="tagName">Tag name.</param>
        public static bool TagExists(string tagName)
        {
            SerializedObject tagManager = Manager;
            SerializedProperty tagsProp = tagManager.FindProperty("tags");
            return PropertyExists(tagsProp, 0, maxTags, tagName);
        }
        
        /// <summary>
        /// Adds the tag.
        /// </summary>
        /// <returns><c>true</c>, if tag was added, <c>false</c> otherwise.</returns>
        /// <param name="tagName">Tag name.</param>
        public static bool CreateTag(string tagName)
        {
            SerializedObject tagManager = Manager;
            SerializedProperty tagsProp = tagManager.FindProperty("tags");
            if (tagsProp.arraySize >= maxTags)
            {
                Log.Warn($"No more tags can be added to the Tags property. You have {tagsProp.arraySize} tags already.");
                return false;
            }
            // if not found, add it
            if (!PropertyExists(tagsProp, 0, tagsProp.arraySize, tagName))
            {
                int index = tagsProp.arraySize;
                tagsProp.InsertArrayElementAtIndex(index);
                SerializedProperty sp = tagsProp.GetArrayElementAtIndex(index);
                sp.stringValue = tagName;
                Log.Info($"Tag: {tagName} has been added");
                // Save settings
                tagManager.ApplyModifiedProperties();
 
                return true;
            }

            Log.Debug($"Tag: {tagName} already exists");
            return false;
        }
        
        /// <summary>
        /// Removes the tag.
        /// </summary>
        /// <returns><c>true</c>, if tag was removed, <c>false</c> otherwise.</returns>
        /// <param name="tagName">Tag name.</param>
        public static bool RemoveTag(string tagName)
        {
            SerializedObject tagManager = Manager;
            SerializedProperty tagsProp = tagManager.FindProperty("tags");
 
            if (PropertyExists(tagsProp, 0, tagsProp.arraySize, tagName))
            {
                SerializedProperty sp;
                for (int i = 0, j = tagsProp.arraySize; i < j; i++)
                {
 
                    sp = tagsProp.GetArrayElementAtIndex(i);
                    if (sp.stringValue == tagName)
                    {
                        tagsProp.DeleteArrayElementAtIndex(i);
                        Log.Info($"Tag: {tagName} has been removed");
                        // Save settings
                        tagManager.ApplyModifiedProperties();
                        return true;
                    }
                }
            }
 
            return false;
        }

        #endregion
        
        #region Layers
        
        /// <summary>
        /// Checks to see if layer exists.
        /// </summary>
        /// <returns><c>true</c>, if layer exists, <c>false</c> otherwise.</returns>
        /// <param name="layerName">Layer name.</param>
        public static bool LayerExists(string layerName)
        {
            SerializedObject tagManager = Manager;
            SerializedProperty layersProp = tagManager.FindProperty("layers");
            return PropertyExists(layersProp, 0, maxLayers, layerName);
        }
        
        /// <summary>
        /// Adds the layer.
        /// </summary>
        /// <returns><c>true</c>, if layer was added, <c>false</c> otherwise.</returns>
        /// <param name="layerName">Layer name.</param>
        public static bool CreateLayer(string layerName)
        {
            SerializedObject tagManager = Manager;
            SerializedProperty layersProp = tagManager.FindProperty("layers");
            if (!PropertyExists(layersProp, 0, maxLayers, layerName))
            {
                SerializedProperty sp;
                // TODO: we could also use 4th layer (3 index)
                // Start at layer 7th index -> 6 (zero based) => first 6 reserved for unity / greyed out
                for (int i = 6, j = maxLayers; i < j; i++)
                {
                    sp = layersProp.GetArrayElementAtIndex(i);
                    if (sp.stringValue == "")
                    {
                        sp.stringValue = layerName;
                        Log.Info($"Layer: {layerName} has been added");
                        // Save settings
                        tagManager.ApplyModifiedProperties();
                        return true;
                    }
                    if (i == j)
                        Log.Info("All allowed layers have been filled");
                }
            }
            else
            {
                Log.Debug($"Layer: {layerName} already exists");
            }
            return false;
        }

        /// <summary>
        /// Removes the layer.
        /// </summary>
        /// <returns><c>true</c>, if layer was removed, <c>false</c> otherwise.</returns>
        /// <param name="layerName">Layer name.</param>
        public static bool RemoveLayer(string layerName)
        {
            SerializedObject tagManager = Manager;
            SerializedProperty layersProp = tagManager.FindProperty("layers");
 
            if (PropertyExists(layersProp, 0, layersProp.arraySize, layerName))
            {
                SerializedProperty sp;
                for (int i = 0, j = layersProp.arraySize; i < j; i++)
                {
                    sp = layersProp.GetArrayElementAtIndex(i);
                    if (sp.stringValue == layerName)
                    {
                        sp.stringValue = "";
                        Log.Info($"Layer: {layerName} has been removed");
                        // Save settings
                        tagManager.ApplyModifiedProperties();
                        return true;
                    }
                }
            }
 
            return false;
        }
        
        #endregion
    }
}
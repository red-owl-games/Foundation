#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;

namespace RedOwl.Core
{
	public abstract	class UnitySettingsEditor<T> : MonoBehaviour where T : Database<T>, new()
	{
		private static PropertyTree _tree;
		
		private static T GetInstance()
		{
			return Database<T>.Instance;
		}
		
		protected static SettingsProvider CreateCustomSettingsProvider()
		{
			return CreateCustomSettingsProvider(typeof(T).Name);
		}
		
		protected static SettingsProvider CreateCustomSettingsProvider(string name)
		{
			_tree = PropertyTree.Create(GetInstance());
			// First parameter is the path in the Settings window.
			// Second parameter is the scope of this setting: it only appears in the Project Settings window.
			SettingsProvider provider = new SettingsProvider($"Project/RedOwl/{name}", SettingsScope.Project)
			{
				// Create the SettingsProvider and initialize its drawing (IMGUI) function in place:
				guiHandler = searchContext =>
				{
					InspectorUtilities.BeginDrawPropertyTree(_tree, false);
					foreach (InspectorProperty property in _tree.EnumerateTree(false))
					{
						property.Draw(property.Label);
					}
					InspectorUtilities.EndDrawPropertyTree(_tree);
				},
	
				// Populate the search keywords to enable smart search filtering and label highlighting:
				keywords = SettingsProvider.GetSearchKeywordsFromSerializedObject(new SerializedObject(GetInstance()))
	    	};
	
			return provider;
		}
	}
}
#endif

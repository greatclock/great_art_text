using UnityEngine;
using UnityEditor;

namespace GreatClock.Common.UI {

	[CustomEditor(typeof(GreatArtText)), CanEditMultipleObjects]
	public class GreatArtTextEditor : Editor {

		private SerializedObject mPreset;
		private GreatArtTextStyleDrawer mDrawer;

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
			SerializedProperty pRectWrapAll = serializedObject.FindProperty("m_RectWrapAll");
			EditorGUILayout.PropertyField(pRectWrapAll);
			SerializedProperty pContent = serializedObject.FindProperty("m_Text");
			EditorGUILayout.LabelField("Text");
			string content = GUILayout.TextArea(pContent.stringValue, GUILayout.MinHeight(80f));
			if (content != pContent.stringValue) {
				Undo.RegisterCompleteObjectUndo(targets, "Text");
				pContent.stringValue = content;
			}
			SerializedProperty pPresetStyle = serializedObject.FindProperty("m_PresetStyle");
			EditorGUILayout.PropertyField(pPresetStyle);
			if (pPresetStyle.objectReferenceValue == null) {
				if (mPreset != null) {
					mPreset = null;
					mDrawer = null;
				}
			} else {
				if (mPreset == null || mPreset.targetObject != pPresetStyle.objectReferenceValue) {
					mPreset = new SerializedObject(pPresetStyle.objectReferenceValue);
					mDrawer = null;
				}
			}
			EditorGUILayout.BeginHorizontal();
			EditorGUI.BeginDisabledGroup(pPresetStyle.objectReferenceValue == null);
			GreatArtTextStyleDrawer.eOperation op = GreatArtTextStyleDrawer.eOperation.None;
			if (GUILayout.Button("Clear Overrides")) {
				op = GreatArtTextStyleDrawer.eOperation.ClearOverrides;
			}
			if (GUILayout.Button("Reset Overrides")) {
				op = GreatArtTextStyleDrawer.eOperation.ResetOverrides;
			}
			EditorGUI.EndDisabledGroup();
			if (GUILayout.Button("Save to Preset")) {
				if (pPresetStyle.objectReferenceValue != null) {
					op = GreatArtTextStyleDrawer.eOperation.SaveToPreset;
				} else {
					string path = EditorUtility.SaveFilePanelInProject("New Preset Style", "NewGreatArtTextStyle", "asset", null);
					if (!string.IsNullOrEmpty(path)) {
						GreatArtTextStyleAsset ps = AssetDatabase.LoadAssetAtPath<GreatArtTextStyleAsset>(path);
						bool isnew = false;
						if (ps == null) {
							ps = ScriptableObject.CreateInstance<GreatArtTextStyleAsset>();
							isnew = true;
						}
						SerializedObject so = new SerializedObject(ps);
						so.CopyFromSerializedProperty(serializedObject.FindProperty("m_Style"));
						so.ApplyModifiedProperties();
						if (isnew) {
							AssetDatabase.CreateAsset(ps, path);
						} else {
							AssetDatabase.SaveAssets();
						}
					}
				}
			}
			EditorGUILayout.EndHorizontal();
			if (mDrawer == null) {
				SerializedProperty preset = mPreset == null ? null : mPreset.FindProperty("m_Style");
				mDrawer = new GreatArtTextStyleDrawer(serializedObject, preset);
			}
			mDrawer.DrawStyleGUI(this, false, op);
			bool save = false;
			if (serializedObject.ApplyModifiedProperties()) { save = true; }
			if (mPreset != null && mPreset.ApplyModifiedProperties()) { save = true; }
			if (save) { AssetDatabase.SaveAssets(); }
		}

	}

}
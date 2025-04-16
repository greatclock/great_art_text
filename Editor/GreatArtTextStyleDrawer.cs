using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace GreatClock.Common.UI {

	public class GreatArtTextStyleDrawer {

		private SerializedObject mSerializedObject;

		private StyleSerializedProperty mPropFonts;
		private ReorderableList mFontsListCurrent;
		private ReorderableList mFontsListPreset;
		private StyleSerializedProperty mPropFontSize;
		private StyleSerializedProperty mPropFontStyle;
		private StyleSerializedProperty mPropLineHeight;
		private StyleSerializedProperty mPropLineSpace;
		private StyleSerializedProperty mPropColorType;
		private StyleColorProperty mPropFontColorCurrent;
		private StyleColorProperty mPropFontColorPreset;
		private StyleSerializedProperty mPropCharSpace;
		private StyleSerializedProperty mPropCharScale;
		private StyleSerializedProperty mPropCharIncline;
		private StyleSerializedProperty mPropTextBlend;
		private StyleSerializedProperty mPropHasCharTrapezoid;
		private StyleSerializedProperty mPropCharTrapezoidAxis;
		private StyleSerializedProperty mPropCharTrapezoidRatio;
		private StyleSerializedProperty mPropCharTrapezoidConcaveConvex;
		private StyleSerializedProperty mPropSpacialMode;
		private StyleSerializedProperty mPropShadowOffset;
		private StyleSerializedProperty mPropShadowExtend;
		private StyleSerializedProperty mPropShadowAngleBlur;
		private StyleSerializedProperty mPropShadowBlurDirection;
		private StyleSerializedProperty mPropShadowBlurAlongAndCross;
		private StyleSerializedProperty mPropShadowBlur;
		private StyleFxColorProperty mPropShadowColor;
		private StyleSerializedProperty mPropShadowIntensity;
		private StyleSerializedProperty mPropSpacialLight360;
		private StyleSerializedProperty mPropSpacialLightPhase;
		private StyleSerializedProperty mPropSpacialLightSoft;
		private StyleSerializedProperty mPropPerspective3dEndPoint;
		private StyleSerializedProperty mPropPerspective3dStretch;
		private StyleSerializedProperty mPropOrthographic3dDirection;
		private StyleSerializedProperty mPropOrthographic3dLength;
		private StyleSerializedProperty mPropSpacialFaded;
		private StyleSerializedProperty mPropHasOutline;
		private StyleSerializedProperty mPropOutlineSize;
		private StyleFxColorProperty mPropOutlineColor;
		private StyleSerializedProperty mPropOutlineFillText;
		private StyleSerializedProperty mPropInnerFxMode;
		private StyleSerializedProperty mPropInnerFxBlur;
		private StyleSerializedProperty mPropInnerFxBlurDirection;
		private StyleFxColorProperty mPropInnerFxColor;
		private StyleSerializedProperty mPropInnerFxPower;
		private StyleSerializedProperty mPropInnerFxBlend;
		private StyleSerializedProperty mPropInnerFxMultiply;
		private StyleSerializedProperty mPropInnerEmbossLightColor;
		private StyleSerializedProperty mPropInnerEmbossLightDir;
		private StyleSerializedProperty mPropInnerEmbossIntensity;
		private StyleSerializedProperty mPropInnerEmbossBevel;
		private StyleSerializedProperty mPropHasGlow;
		private StyleSerializedProperty mPropGlowExtend;
		private StyleSerializedProperty mPropGlowAngleBlur;
		private StyleSerializedProperty mPropGlowBlurDirection;
		private StyleSerializedProperty mPropGlowBlurAlongAndCross;
		private StyleSerializedProperty mPropGlowBlur;
		private StyleFxColorProperty mPropGlowColor;
		private StyleSerializedProperty mPropGlowPower;
		private StyleSerializedProperty mPropGlowIntensity;

		public GreatArtTextStyleDrawer(SerializedObject serializedObject, SerializedProperty preset) {
			mSerializedObject = serializedObject;
			SerializedProperty current = serializedObject.FindProperty("m_Style");
			ReorderableList.HeaderCallbackDelegate drawheader = (Rect rect) => {
				EditorGUI.LabelField(rect, "Fonts");
			};
			mPropFonts = StyleSerializedProperty.Get("m_Fonts", "m_FontsOverride", mSerializedObject, current, preset,
				() => { mFontsListCurrent.DoLayoutList(); },
				() => { if (mFontsListPreset != null) { mFontsListPreset.DoLayoutList(); } }
			);
			mFontsListCurrent = new ReorderableList(serializedObject, mPropFonts.propCurrent, true, true, true, true);
			mFontsListPreset = preset == null ? null : new ReorderableList(preset.serializedObject, mPropFonts.propPreset, true, true, true, true);
			mFontsListCurrent.drawHeaderCallback = drawheader;
			mFontsListCurrent.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => { DrawFontElement(rect, mPropFonts.propCurrent, index, isActive, isFocused); };
			if (mFontsListPreset != null) {
				mFontsListPreset.drawHeaderCallback = drawheader;
				mFontsListPreset.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => { DrawFontElement(rect, mPropFonts.propPreset, index, isActive, isFocused); };
			}
			mPropFontSize = StyleSerializedProperty.Get("m_FontSize", "m_FontSizeOverride", mSerializedObject, current, preset, null, null);
			mPropFontStyle = StyleSerializedProperty.Get("m_FontStyle", "m_FontStyleOverride", mSerializedObject, current, preset, null, null);
			mPropLineHeight = StyleSerializedProperty.Get("m_LineHeight", "m_LineHeightOverride", mSerializedObject, current, preset, null, null);
			mPropLineSpace = StyleSerializedProperty.Get("m_LineSpace", "m_LineSpaceOverride", mSerializedObject, current, preset, null, null);
			mPropColorType = StyleSerializedProperty.Get("m_ColorType", "m_ColorTypeOverride", mSerializedObject, current, preset, null, null);
			mPropFontColorCurrent = new StyleColorProperty();
			mPropFontColorCurrent.propColor = current.FindPropertyRelative("m_FontColor");
			mPropFontColorCurrent.propGradient = current.FindPropertyRelative("m_ColorGradient");
			mPropFontColorCurrent.propGradientAngle = current.FindPropertyRelative("m_ColorGradientAngle");
			mPropFontColorCurrent.propTexture = current.FindPropertyRelative("m_ColorTexture");
			mPropFontColorCurrent.propTextureTiling = current.FindPropertyRelative("m_ColorTextureTiling");
			mPropFontColorCurrent.propTextureOffset = current.FindPropertyRelative("m_ColorTextureOffset");
			mPropFontColorPreset = new StyleColorProperty();
			if (preset != null) {
				mPropFontColorPreset.propColor = preset.FindPropertyRelative("m_FontColor");
				mPropFontColorPreset.propGradient = preset.FindPropertyRelative("m_ColorGradient");
				mPropFontColorPreset.propGradientAngle = preset.FindPropertyRelative("m_ColorGradientAngle");
				mPropFontColorPreset.propTexture = preset.FindPropertyRelative("m_ColorTexture");
				mPropFontColorPreset.propTextureTiling = preset.FindPropertyRelative("m_ColorTextureTiling");
				mPropFontColorPreset.propTextureOffset = preset.FindPropertyRelative("m_ColorTextureOffset");
			}
			mPropCharSpace = StyleSerializedProperty.Get("m_CharSpace", "m_CharSpaceOverride", mSerializedObject, current, preset, null, null);
			mPropCharScale = StyleSerializedProperty.Get("m_CharScale", "m_CharScaleOverride", mSerializedObject, current, preset, null, null);
			mPropCharIncline = StyleSerializedProperty.Get("m_CharIncline", "m_CharInclineOverride", mSerializedObject, current, preset, null, null);
			mPropTextBlend = StyleSerializedProperty.Get("m_TextBlend", "m_TextBlendOverride", mSerializedObject, current, preset, null, null);
			mPropHasCharTrapezoid = StyleSerializedProperty.Get("m_HasCharTrapezoid", "m_HasCharTrapezoidOverride", mSerializedObject, current, preset, null, null);
			mPropCharTrapezoidAxis = StyleSerializedProperty.Get("m_CharTrapezoidAxis", "m_CharTrapezoidAxisOverride", mSerializedObject, current, preset, null, null);
			mPropCharTrapezoidRatio = StyleSerializedProperty.Get("m_CharTrapezoidRatio", "m_CharTrapezoidRatioOverride", mSerializedObject, current, preset, null, null);
			mPropCharTrapezoidConcaveConvex = StyleSerializedProperty.Get("m_CharTrapezoidConcaveConvex", "m_CharTrapezoidConcaveConvexOverride", mSerializedObject, current, preset, null, null);
			mPropSpacialMode = StyleSerializedProperty.Get("m_SpacialMode", "m_SpacialModeOverride", mSerializedObject, current, preset, null, null);
			mPropShadowOffset = StyleSerializedProperty.Get("m_ShadowOffset", "m_ShadowOffsetOverride", mSerializedObject, current, preset, null, null);
			mPropShadowExtend = StyleSerializedProperty.Get("m_ShadowExtend", "m_ShadowExtendOverride", mSerializedObject, current, preset, null, null);
			mPropShadowAngleBlur = StyleSerializedProperty.Get("m_ShadowAngleBlur", "m_ShadowAngleBlurOverride", mSerializedObject, current, preset, null, null);
			mPropShadowBlurDirection = StyleSerializedProperty.Get("m_ShadowBlurDirection", "m_ShadowBlurDirectionOverride", mSerializedObject, current, preset, null, null);
			mPropShadowBlurAlongAndCross = StyleSerializedProperty.Get("m_ShadowBlurAlongAndCross", "m_ShadowBlurAlongAndCrossOverride", mSerializedObject, current, preset, null, null);
			mPropShadowBlur = StyleSerializedProperty.Get("m_ShadowBlur", "m_ShadowBlurOverride", mSerializedObject, current, preset, null, null);
			mPropShadowColor = StyleFxColorProperty.Get("m_ShadowColor", mSerializedObject, current, preset);
			mPropShadowIntensity = StyleSerializedProperty.Get("m_ShadowIntensity", "m_ShadowIntensityOverride", mSerializedObject, current, preset, null, null);
			mPropSpacialLight360 = StyleSerializedProperty.Get("m_SpacialLight360", "m_SpacialLight360Override", mSerializedObject, current, preset, null, null);
			mPropSpacialLightPhase = StyleSerializedProperty.Get("m_SpacialLightPhase", "m_SpacialLightPhaseOverride", mSerializedObject, current, preset, null, null);
			mPropSpacialLightSoft = StyleSerializedProperty.Get("m_SpacialLightSoft", "m_SpacialLightSoftOverride", mSerializedObject, current, preset, null, null);
			mPropPerspective3dEndPoint = StyleSerializedProperty.Get("m_Perspective3dEndPoint", "m_Perspective3dEndPointOverride", mSerializedObject, current, preset, null, null);
			mPropPerspective3dStretch = StyleSerializedProperty.Get("m_Perspective3dStretch", "m_Perspective3dStretchOverride", mSerializedObject, current, preset, null, null);
			mPropOrthographic3dDirection = StyleSerializedProperty.Get("m_Orthographic3dDirection", "m_Orthographic3dDirectionOverride", mSerializedObject, current, preset, null, null);
			mPropOrthographic3dLength = StyleSerializedProperty.Get("m_Orthographic3dLength", "m_Orthographic3dLengthOverride", mSerializedObject, current, preset, null, null);
			mPropSpacialFaded = StyleSerializedProperty.Get("m_SpacialFaded", "m_SpacialFadedOverride", mSerializedObject, current, preset, null, null);
			mPropHasOutline = StyleSerializedProperty.Get("m_HasOutline", "m_HasOutlineOverride", mSerializedObject, current, preset, null, null);
			mPropOutlineSize = StyleSerializedProperty.Get("m_OutlineSize", "m_OutlineSizeOverride", mSerializedObject, current, preset, null, null);
			mPropOutlineColor = StyleFxColorProperty.Get("m_OutlineColor", mSerializedObject, current, preset);
			mPropOutlineFillText = StyleSerializedProperty.Get("m_OutlineFillText", "m_OutlineFillTextOverride", mSerializedObject, current, preset, null, null);
			mPropInnerFxMode = StyleSerializedProperty.Get("m_InnerFxMode", "m_InnerFxModeOverride", mSerializedObject, current, preset, null, null);
			mPropInnerFxBlur = StyleSerializedProperty.Get("m_InnerBlur", "m_InnerBlurOverride", mSerializedObject, current, preset, null, null);
			mPropInnerFxBlurDirection = StyleSerializedProperty.Get("m_InnerShadowDirection", "m_InnerShadowDirectionOverride", mSerializedObject, current, preset, null, null);
			mPropInnerFxColor = StyleFxColorProperty.Get("m_InnerFxColor", mSerializedObject, current, preset);
			mPropInnerFxPower = StyleSerializedProperty.Get("m_InnerBlurFix", "m_InnerBlurFixOverride", mSerializedObject, current, preset, null, null);
			mPropInnerFxBlend = StyleSerializedProperty.Get("m_InnerFxBlend", "m_InnerFxBlendOverride", mSerializedObject, current, preset, null, null);
			mPropInnerFxMultiply = StyleSerializedProperty.Get("m_InnerMultiply", "m_InnerMultiplyOverride", mSerializedObject, current, preset, null, null);
			mPropInnerEmbossLightColor = StyleSerializedProperty.Get("m_EmbossLightColor", "m_EmbossLightColorOverride", mSerializedObject, current, preset, null, null);
			mPropInnerEmbossLightDir = StyleSerializedProperty.Get("m_EmbossLightDirection", "m_EmbossLightDirectionOverride", mSerializedObject, current, preset, null, null);
			mPropInnerEmbossIntensity = StyleSerializedProperty.Get("m_EmbossIntensity", "m_EmbossIntensityOverride", mSerializedObject, current, preset, null, null);
			mPropInnerEmbossBevel = StyleSerializedProperty.Get("m_EmbossBevel", "m_EmbossBevelOverride", mSerializedObject, current, preset, null, null);
			mPropHasGlow = StyleSerializedProperty.Get("m_HasGlow", "m_HasGlowOverride", mSerializedObject, current, preset, null, null);
			mPropGlowExtend = StyleSerializedProperty.Get("m_GlowExtend", "m_GlowExtendOverride", mSerializedObject, current, preset, null, null);
			mPropGlowAngleBlur = StyleSerializedProperty.Get("m_GlowAngleBlur", "m_GlowAngleBlurOverride", mSerializedObject, current, preset, null, null);
			mPropGlowBlurDirection = StyleSerializedProperty.Get("m_GlowBlurDirection", "m_GlowBlurDirectionOverride", mSerializedObject, current, preset, null, null);
			mPropGlowBlurAlongAndCross = StyleSerializedProperty.Get("m_GlowBlurAlongAndCross", "m_GlowBlurAlongAndCrossOverride", mSerializedObject, current, preset, null, null);
			mPropGlowBlur = StyleSerializedProperty.Get("m_GlowBlur", "m_GlowBlurOverride", mSerializedObject, current, preset, null, null);
			mPropGlowColor = StyleFxColorProperty.Get("m_GlowColor", mSerializedObject, current, preset);
			mPropGlowPower = StyleSerializedProperty.Get("m_GlowBlurFix", "m_GlowBlurFixOverride", mSerializedObject, current, preset, null, null);
			mPropGlowIntensity = StyleSerializedProperty.Get("m_GlowIntensity", "m_GlowIntensityOverride", mSerializedObject, current, preset, null, null);
		}

		private void DrawFontElement(Rect rect, SerializedProperty list, int index, bool isActive, bool isFocused) {
			const float label_width = 50f;
			const float toggle_width = 120f;
			Rect pos = rect;
			SerializedProperty element = list.GetArrayElementAtIndex(index);
			SerializedProperty pUseFontAsset = element.FindPropertyRelative("m_UseFontAsset");
			EditorGUI.LabelField(new Rect(pos.x, pos.y, label_width, pos.height), $"Font {index}");
			pos.x += label_width;
			pos.width -= label_width;
			EditorGUI.BeginChangeCheck();
			bool useFontAsset = EditorGUI.ToggleLeft(new Rect(pos.x, pos.y, toggle_width, pos.height), "Use Font Asset", pUseFontAsset.boolValue);
			pos.x += toggle_width;
			pos.width -= toggle_width;
			if (EditorGUI.EndChangeCheck()) {
				pUseFontAsset.boolValue = useFontAsset;
			}
			Rect objrect = new Rect(pos.x, pos.y, pos.width, pos.height);
			SerializedProperty pa = element.FindPropertyRelative("m_FontAsset");
			SerializedProperty pf = element.FindPropertyRelative("m_Font");
			SerializedProperty pUsing = useFontAsset ? pa : pf;
			SerializedProperty pFree = useFontAsset ? pf : pa;
			EditorGUI.ObjectField(objrect, pUsing, GUIContent.none);
			if (pFree.objectReferenceValue != null) { pFree.objectReferenceValue = null; }
		}

		public enum eOperation { None, ClearOverrides, ResetOverrides, SaveToPreset }

		private enum eOP { None, Reset, Save }

		private struct StyleSerializedProperty {

			public SerializedProperty propCurrent;
			public SerializedProperty propPreset;
			public SerializedProperty propOverride;
			public System.Action onDrawCurrent;
			public System.Action onDrawPreset;

			public static StyleSerializedProperty Get(string property, string pOverride, SerializedObject serializedObject, SerializedProperty current, SerializedProperty preset,
				System.Action onDrawCurrent, System.Action onDrawPreset) {
				StyleSerializedProperty ret = new StyleSerializedProperty();
				ret.propCurrent = current.FindPropertyRelative(property);
				ret.propPreset = preset == null ? null : preset.FindPropertyRelative(property);
				ret.propOverride = string.IsNullOrEmpty(pOverride) ? null : serializedObject.FindProperty(pOverride);
				ret.onDrawCurrent = onDrawCurrent;
				ret.onDrawPreset = onDrawPreset;
				return ret;
			}
		}

		private struct StyleColorProperty {

			public SerializedProperty propColor;
			public SerializedProperty propGradient;
			public SerializedProperty propGradientAngle;
			public SerializedProperty propTexture;
			public SerializedProperty propTextureTiling;
			public SerializedProperty propTextureOffset;

			public static StyleColorProperty Get(SerializedProperty property) {
				StyleColorProperty ret = new StyleColorProperty();
				if (property != null) {
					ret.propColor = property.FindPropertyRelative("m_Color");
					ret.propGradient = property.FindPropertyRelative("m_Gradient");
					ret.propGradientAngle = property.FindPropertyRelative("m_GradientAngle");
					ret.propTexture = property.FindPropertyRelative("m_Texture");
					ret.propTextureTiling = property.FindPropertyRelative("m_TextureTiling");
					ret.propTextureOffset = property.FindPropertyRelative("m_TextureOffset");
				}
				return ret;
			}

		}

		private struct StyleFxColorProperty {

			public SerializedProperty propCurrent;
			public SerializedProperty propPreset;
			public StyleSerializedProperty propColorType;
			public StyleColorProperty colorCurrent;
			public StyleColorProperty colorPreset;

			public static StyleFxColorProperty Get(string property, SerializedObject serializedObject, SerializedProperty current, SerializedProperty preset) {
				StyleFxColorProperty ret = new StyleFxColorProperty();
				ret.propCurrent = current.FindPropertyRelative(property);
				ret.propPreset = preset == null ? null : preset.FindPropertyRelative(property);
				ret.propColorType = StyleSerializedProperty.Get("m_ColorType", property + "Override", serializedObject, ret.propCurrent, ret.propPreset, null, null);
				ret.colorCurrent = StyleColorProperty.Get(ret.propCurrent);
				ret.colorPreset = StyleColorProperty.Get(ret.propPreset);
				return ret;
			}
		}

		private struct StylePropertyData {
			// public SerializedProperty style_object;
			public SerializedProperty property;
			public bool enabled;
			public eOP op;
		}

		private static StylePropertyData DrawStyleProperty(StyleSerializedProperty styleProp, bool styleOnly, eOperation operation, GUIContent title = null) {
			StylePropertyData ret = new StylePropertyData();
			bool ov = styleProp.propPreset != null && !styleOnly;
			bool bOV = false;
			if (ov) {
				EditorGUILayout.BeginHorizontal();
				if (operation == eOperation.ClearOverrides && styleProp.propOverride.boolValue) { styleProp.propOverride.boolValue = false; }
				bOV = styleProp.propOverride.boolValue;
			}
			System.Action onDraw = null;
			if (!ov || bOV) {
				ret.property = styleProp.propCurrent;
				ret.enabled = true;
				onDraw = styleProp.onDrawCurrent;
			} else {
				ret.property = styleProp.propPreset;
				ret.enabled = false;
				onDraw = styleProp.onDrawPreset;
			}
			ret.op = eOP.None;
			EditorGUI.BeginDisabledGroup(!ret.enabled);
			if (onDraw != null) {
				if (ov) { EditorGUILayout.BeginVertical(); }
				onDraw();
				if (ov) { EditorGUILayout.EndVertical(); }
			} else if (title == null) {
				EditorGUILayout.PropertyField(ret.property);
			} else {
				EditorGUILayout.PropertyField(ret.property, title);
			}
			EditorGUI.EndDisabledGroup();
			if (ov) {
				Color cachedColor = GUI.backgroundColor;
				if (bOV) {
					GUI.backgroundColor = Color.green;
				}
				if (bOV != GUILayout.Toggle(bOV, "O", GUI.skin.button, s_mini_btn_width)) {
					bOV = !bOV;
					styleProp.propOverride.boolValue = bOV;
				}
				GUI.backgroundColor = cachedColor;
				EditorGUI.BeginDisabledGroup(!bOV);
				bool save = false;
				if (Event.current.shift) {
					if (GUILayout.Button("S", s_mini_btn_width)) {
						save = true;
					}
				} else {
					if (GUILayout.Button("R", s_mini_btn_width) || (bOV && operation == eOperation.ResetOverrides)) {
						CopyProperty(styleProp.propPreset, styleProp.propCurrent);
						ret.op = eOP.Reset;
					}
				}
				if (save || (bOV && operation == eOperation.SaveToPreset)) {
					CopyProperty(styleProp.propCurrent, styleProp.propPreset);
					ret.op = eOP.Save;
				}
				EditorGUI.EndDisabledGroup();
				EditorGUILayout.EndHorizontal();
			}
			return ret;
		}

		public void DrawStyleGUI(Editor win, bool styleOnly, eOperation operation) {
			Init();
			SerializedProperty current = mSerializedObject.FindProperty("m_Style");
			Color cachedGUIColor = GUI.color;
			GUI.color = new Color(cachedGUIColor.r * 0.6f, cachedGUIColor.g * 0.6f, cachedGUIColor.b * 0.6f, cachedGUIColor.a);
			EditorGUILayout.BeginVertical(GUI.skin.box);
			GUI.color = cachedGUIColor;
			DrawStyleProperty(mPropFonts, styleOnly, operation);
			DrawStyleProperty(mPropFontSize, styleOnly, operation);
			DrawStyleProperty(mPropFontStyle, styleOnly, operation);
			DrawStyleProperty(mPropLineHeight, styleOnly, operation);
			DrawStyleProperty(mPropLineSpace, styleOnly, operation);
			StylePropertyData colorData = DrawStyleProperty(mPropColorType, styleOnly, operation);
			EditorGUI.indentLevel++;
			EditorGUI.BeginDisabledGroup(!colorData.enabled);
			StyleColorProperty fontColor = colorData.enabled ? mPropFontColorCurrent : mPropFontColorPreset;
			if (colorData.op == eOP.Reset) {
				CopyProperty(mPropFontColorPreset.propTexture, mPropFontColorCurrent.propTexture);
				CopyProperty(mPropFontColorPreset.propTextureTiling, mPropFontColorCurrent.propTextureTiling);
				CopyProperty(mPropFontColorPreset.propTextureOffset, mPropFontColorCurrent.propTextureOffset);
			} else if (colorData.op == eOP.Save) {
				CopyProperty(mPropFontColorCurrent.propTexture, mPropFontColorPreset.propTexture);
				CopyProperty(mPropFontColorCurrent.propTextureTiling, mPropFontColorPreset.propTextureTiling);
				CopyProperty(mPropFontColorCurrent.propTextureOffset, mPropFontColorPreset.propTextureOffset);
			}
			EditorGUILayout.PropertyField(fontColor.propTexture);
			if (fontColor.propTexture.objectReferenceValue != null) {
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(fontColor.propTextureTiling);
				EditorGUILayout.PropertyField(fontColor.propTextureOffset);
				EditorGUI.indentLevel--;
			}
			switch ((GreatArtTextStyle.eColorType)colorData.property.enumValueIndex) {
				case GreatArtTextStyle.eColorType.Color:
					if (colorData.op == eOP.Reset) {
						CopyProperty(mPropFontColorPreset.propColor, mPropFontColorCurrent.propColor);
					} else if (colorData.op == eOP.Save) {
						CopyProperty(mPropFontColorCurrent.propColor, mPropFontColorPreset.propColor);
					}
					EditorGUILayout.PropertyField(fontColor.propColor);
					break;
				case GreatArtTextStyle.eColorType.Gradient:
				case GreatArtTextStyle.eColorType.GradientPerChar:
					if (colorData.op == eOP.Reset) {
						CopyProperty(mPropFontColorPreset.propGradient, mPropFontColorCurrent.propGradient);
						CopyProperty(mPropFontColorPreset.propGradientAngle, mPropFontColorCurrent.propGradientAngle);
					} else if (colorData.op == eOP.Save) {
						CopyProperty(mPropFontColorCurrent.propGradient, mPropFontColorPreset.propGradient);
						CopyProperty(mPropFontColorCurrent.propGradientAngle, mPropFontColorPreset.propGradientAngle);
					}
					EditorGUILayout.PropertyField(fontColor.propGradient);
					EditorGUILayout.PropertyField(fontColor.propGradientAngle);
					break;
			}
			EditorGUI.EndDisabledGroup();
			EditorGUI.indentLevel--;
			DrawStyleProperty(mPropCharSpace, styleOnly, operation);
			DrawStyleProperty(mPropCharScale, styleOnly, operation);
			DrawStyleProperty(mPropCharIncline, styleOnly, operation);
			DrawStyleProperty(mPropTextBlend, styleOnly, operation);
			EditorGUILayout.EndVertical();
			StylePropertyData trapezoidData = DrawStyleProperty(mPropHasCharTrapezoid, styleOnly, operation);
			EditorGUI.BeginDisabledGroup(!trapezoidData.property.boolValue);
			EditorGUI.indentLevel++;
			cachedGUIColor = GUI.color;
			GUI.color = new Color(cachedGUIColor.r * 0.6f, cachedGUIColor.g * 0.6f, cachedGUIColor.b * 0.6f, cachedGUIColor.a);
			EditorGUILayout.BeginVertical(GUI.skin.box);
			GUI.color = cachedGUIColor;
			DrawStyleProperty(mPropCharTrapezoidAxis, styleOnly, operation, s_label_trapezoid_axis);
			DrawStyleProperty(mPropCharTrapezoidRatio, styleOnly, operation, s_label_trapezoid_ratio);
			DrawStyleProperty(mPropCharTrapezoidConcaveConvex, styleOnly, operation, s_label_trapezoid_concave_convex);
			EditorGUILayout.EndVertical();
			EditorGUI.indentLevel--;
			EditorGUI.EndDisabledGroup();
			StylePropertyData spacialData = DrawStyleProperty(mPropSpacialMode, styleOnly, operation);
			GreatArtTextStyle.eSpacialMode spacialMode = (GreatArtTextStyle.eSpacialMode)spacialData.property.enumValueIndex;
			EditorGUI.BeginDisabledGroup(spacialMode == GreatArtTextStyle.eSpacialMode.None);
			EditorGUI.indentLevel++;
			cachedGUIColor = GUI.color;
			GUI.color = new Color(cachedGUIColor.r * 0.6f, cachedGUIColor.g * 0.6f, cachedGUIColor.b * 0.6f, cachedGUIColor.a);
			EditorGUILayout.BeginVertical(GUI.skin.box);
			GUI.color = cachedGUIColor;
			switch (spacialMode) {
				case GreatArtTextStyle.eSpacialMode.Perspective3D:
					DrawStyleProperty(mPropSpacialLight360, styleOnly, operation);
					DrawStyleProperty(mPropSpacialLightPhase, styleOnly, operation);
					DrawStyleProperty(mPropSpacialLightSoft, styleOnly, operation);
					DrawStyleProperty(mPropPerspective3dEndPoint, styleOnly, operation);
					DrawStyleProperty(mPropPerspective3dStretch, styleOnly, operation);
					DrawStyleProperty(mPropSpacialFaded, styleOnly, operation);
					break;
				case GreatArtTextStyle.eSpacialMode.Orthographic3D:
					DrawStyleProperty(mPropSpacialLight360, styleOnly, operation);
					DrawStyleProperty(mPropSpacialLightPhase, styleOnly, operation);
					DrawStyleProperty(mPropSpacialLightSoft, styleOnly, operation);
					DrawStyleProperty(mPropOrthographic3dDirection, styleOnly, operation);
					DrawStyleProperty(mPropOrthographic3dLength, styleOnly, operation);
					DrawStyleProperty(mPropSpacialFaded, styleOnly, operation);
					break;
				default:
					DrawStyleProperty(mPropShadowOffset, styleOnly, operation);
					DrawStyleProperty(mPropShadowExtend, styleOnly, operation);
					StylePropertyData shadowAngleBlurData = DrawStyleProperty(mPropShadowAngleBlur, styleOnly, operation);
					EditorGUI.indentLevel++;
					if (shadowAngleBlurData.property.boolValue) {
						DrawStyleProperty(mPropShadowBlurDirection, styleOnly, operation);
						DrawStyleProperty(mPropShadowBlurAlongAndCross, styleOnly, operation);
					} else {
						DrawStyleProperty(mPropShadowBlur, styleOnly, operation);
					}
					EditorGUI.indentLevel--;
					DrawFxColor(mPropShadowColor, styleOnly, operation);
					DrawStyleProperty(mPropShadowIntensity, styleOnly, operation);
					break;
			}
			EditorGUILayout.EndVertical();
			EditorGUI.indentLevel--;
			EditorGUI.EndDisabledGroup();
			StylePropertyData outlineData = DrawStyleProperty(mPropHasOutline, styleOnly, operation);
			EditorGUI.BeginDisabledGroup(!outlineData.property.boolValue);
			EditorGUI.indentLevel++;
			cachedGUIColor = GUI.color;
			GUI.color = new Color(cachedGUIColor.r * 0.6f, cachedGUIColor.g * 0.6f, cachedGUIColor.b * 0.6f, cachedGUIColor.a);
			EditorGUILayout.BeginVertical(GUI.skin.box);
			GUI.color = cachedGUIColor;
			DrawStyleProperty(mPropOutlineSize, styleOnly, operation);
			DrawFxColor(mPropOutlineColor, styleOnly, operation);
			DrawStyleProperty(mPropOutlineFillText, styleOnly, operation);
			EditorGUILayout.EndVertical();
			EditorGUI.indentLevel--;
			EditorGUI.EndDisabledGroup();
			StylePropertyData innerFxData = DrawStyleProperty(mPropInnerFxMode, styleOnly, operation);
			GreatArtTextStyle.eInnerFxMode innerFxMode = (GreatArtTextStyle.eInnerFxMode)innerFxData.property.enumValueIndex;
			EditorGUI.BeginDisabledGroup(innerFxMode == GreatArtTextStyle.eInnerFxMode.None);
			EditorGUI.indentLevel++;
			cachedGUIColor = GUI.color;
			GUI.color = new Color(cachedGUIColor.r * 0.6f, cachedGUIColor.g * 0.6f, cachedGUIColor.b * 0.6f, cachedGUIColor.a);
			EditorGUILayout.BeginVertical(GUI.skin.box);
			GUI.color = cachedGUIColor;
			bool hasInnerColor = true;
			switch (innerFxMode) {
				case GreatArtTextStyle.eInnerFxMode.Light:
					DrawStyleProperty(mPropInnerFxBlur, styleOnly, operation);
					break;
				case GreatArtTextStyle.eInnerFxMode.Shadow:
					DrawStyleProperty(mPropInnerFxBlurDirection, styleOnly, operation);
					DrawStyleProperty(mPropInnerFxBlur, styleOnly, operation);
					break;
				case GreatArtTextStyle.eInnerFxMode.Emboss:
					DrawStyleProperty(mPropInnerEmbossLightColor, styleOnly, operation);
					DrawStyleProperty(mPropInnerEmbossLightDir, styleOnly, operation);
					DrawStyleProperty(mPropInnerEmbossIntensity, styleOnly, operation);
					DrawStyleProperty(mPropInnerEmbossBevel, styleOnly, operation);
					hasInnerColor = false;
					break;
			}
			if (hasInnerColor) {
				DrawFxColor(mPropInnerFxColor, styleOnly, operation);
				DrawStyleProperty(mPropInnerFxPower, styleOnly, operation);
				StylePropertyData innerFxBlend = DrawStyleProperty(mPropInnerFxBlend, styleOnly, operation);
				if ((GreatArtTextStyle.eInnerFxBlend)innerFxBlend.property.enumValueIndex == GreatArtTextStyle.eInnerFxBlend.Additive) {
					DrawStyleProperty(mPropInnerFxMultiply, styleOnly, operation);
				}
			}
			EditorGUILayout.EndVertical();
			EditorGUI.indentLevel--;
			EditorGUI.EndDisabledGroup();
			StylePropertyData glowData = DrawStyleProperty(mPropHasGlow, styleOnly, operation);
			EditorGUI.BeginDisabledGroup(!glowData.property.boolValue);
			EditorGUI.indentLevel++;
			cachedGUIColor = GUI.color;
			GUI.color = new Color(cachedGUIColor.r * 0.6f, cachedGUIColor.g * 0.6f, cachedGUIColor.b * 0.6f, cachedGUIColor.a);
			EditorGUILayout.BeginVertical(GUI.skin.box);
			GUI.color = cachedGUIColor;
			DrawStyleProperty(mPropGlowExtend, styleOnly, operation);
			StylePropertyData glowBlurData = DrawStyleProperty(mPropGlowAngleBlur, styleOnly, operation);
			EditorGUI.indentLevel++;
			if (glowBlurData.property.boolValue) {
				DrawStyleProperty(mPropGlowBlurDirection, styleOnly, operation);
				DrawStyleProperty(mPropGlowBlurAlongAndCross, styleOnly, operation);
			} else {
				DrawStyleProperty(mPropGlowBlur, styleOnly, operation);
			}
			EditorGUI.indentLevel--;
			DrawFxColor(mPropGlowColor, styleOnly, operation);
			DrawStyleProperty(mPropGlowPower, styleOnly, operation);
			DrawStyleProperty(mPropGlowIntensity, styleOnly, operation);
			EditorGUILayout.EndVertical();
			EditorGUI.indentLevel--;
			EditorGUI.EndDisabledGroup();
			if (win != null) {
				Event evt = Event.current;
				if ((evt.type == EventType.KeyUp || evt.type == EventType.KeyDown) &&
					(evt.keyCode == KeyCode.LeftShift || evt.keyCode == KeyCode.RightShift)) {
					win.Repaint();
				}
			}
		}

		private static Dictionary<string, GUIContent> s_color_field_labels = new Dictionary<string, GUIContent>();

		private static void DrawFxColor(StyleFxColorProperty colorProp, bool styleOnly, eOperation operation) {
			GUIContent label;
			if (!s_color_field_labels.TryGetValue(colorProp.propCurrent.displayName, out label)) {
				label = new GUIContent(colorProp.propCurrent.displayName);
				s_color_field_labels.Add(colorProp.propCurrent.displayName, label);
			}
			SerializedProperty pPreset = !styleOnly ? colorProp.propPreset : null;
			StylePropertyData colorData = DrawStyleProperty(colorProp.propColorType, styleOnly, operation, label);
			EditorGUI.indentLevel++;
			EditorGUI.BeginDisabledGroup(!colorData.enabled);
			StyleColorProperty colorProps = colorData.enabled ? colorProp.colorCurrent : colorProp.colorPreset;
			if (colorData.op == eOP.Reset) {
				CopyProperty(colorProp.colorPreset.propTexture, colorProp.colorCurrent.propTexture);
				CopyProperty(colorProp.colorPreset.propTextureTiling, colorProp.colorCurrent.propTextureTiling);
				CopyProperty(colorProp.colorPreset.propTextureOffset, colorProp.colorCurrent.propTextureOffset);
			} else if (colorData.op == eOP.Save) {
				CopyProperty(colorProp.colorCurrent.propTexture, colorProp.colorPreset.propTexture);
				CopyProperty(colorProp.colorCurrent.propTextureTiling, colorProp.colorPreset.propTextureTiling);
				CopyProperty(colorProp.colorCurrent.propTextureOffset, colorProp.colorPreset.propTextureOffset);
			}
			EditorGUILayout.PropertyField(colorProps.propTexture);
			if (colorProps.propTexture.objectReferenceValue != null) {
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(colorProps.propTextureTiling);
				EditorGUILayout.PropertyField(colorProps.propTextureOffset);
				EditorGUI.indentLevel--;
			}
			switch ((GreatArtTextStyle.eFxColorType)colorData.property.enumValueIndex) {
				case GreatArtTextStyle.eFxColorType.Gradient:
					if (colorData.op == eOP.Reset) {
						CopyProperty(colorProp.colorPreset.propGradient, colorProp.colorCurrent.propGradient);
						CopyProperty(colorProp.colorPreset.propGradientAngle, colorProp.colorCurrent.propGradientAngle);
					} else if (colorData.op == eOP.Save) {
						CopyProperty(colorProp.colorCurrent.propGradient, colorProp.colorPreset.propGradient);
						CopyProperty(colorProp.colorCurrent.propGradientAngle, colorProp.colorPreset.propGradientAngle);
					}
					EditorGUILayout.PropertyField(colorProps.propGradient);
					EditorGUILayout.PropertyField(colorProps.propGradientAngle);
					break;
				default:
					if (colorData.op == eOP.Reset) {
						CopyProperty(colorProp.colorPreset.propColor, colorProp.colorCurrent.propColor);
					} else if (colorData.op == eOP.Save) {
						CopyProperty(colorProp.colorCurrent.propColor, colorProp.colorPreset.propColor);
					}
					EditorGUILayout.PropertyField(colorProps.propColor);
					break;
			}
			EditorGUI.EndDisabledGroup();
			EditorGUI.indentLevel--;
		}

		private static bool s_inited = false;
		private static GUIContent s_label_trapezoid_axis;
		private static GUIContent s_label_trapezoid_ratio;
		private static GUIContent s_label_trapezoid_concave_convex;
		private static GUILayoutOption s_mini_btn_width;

		private static void Init() {
			if (s_inited) { return; }
			s_inited = true;
			s_label_trapezoid_axis = new GUIContent("Axis");
			s_label_trapezoid_ratio = new GUIContent("Ratio");
			s_label_trapezoid_concave_convex = new GUIContent("Convace&Convex");
			s_mini_btn_width = GUILayout.Width(20f);
		}

		public static bool CopyProperty(SerializedProperty from, SerializedProperty to) {
			if (from.propertyType != to.propertyType) { return false; }
			switch (from.propertyType) {
				case SerializedPropertyType.Generic:
				case SerializedPropertyType.Gradient:
					SerializedProperty copy = from.Copy();
					bool first = true;
					int depth = 0;
					while (copy.Next(first)) {
						if (first) {
							depth = copy.depth;
							first = false;
						} else {
							if (copy.depth != depth) { break; }
						}
						if (!CopyProperty(copy, to.FindPropertyRelative(copy.name))) { return false; }
					}
					break;
				case SerializedPropertyType.Integer:
					to.intValue = from.intValue;
					break;
				case SerializedPropertyType.Boolean:
					to.boolValue = from.boolValue;
					break;
				case SerializedPropertyType.Float:
					to.floatValue = from.floatValue;
					break;
				case SerializedPropertyType.String:
					to.stringValue = from.stringValue;
					break;
				case SerializedPropertyType.Color:
					to.colorValue = from.colorValue;
					break;
				case SerializedPropertyType.ObjectReference:
					to.objectReferenceValue = from.objectReferenceValue;
					break;
				// case SerializedPropertyType.LayerMask:
				case SerializedPropertyType.Enum:
					to.enumValueIndex = from.enumValueIndex;
					break;
				case SerializedPropertyType.Vector2:
					to.vector2Value = from.vector2Value;
					break;
				case SerializedPropertyType.Vector3:
					to.vector3Value = from.vector3Value;
					break;
				case SerializedPropertyType.Vector4:
					to.vector4Value = from.vector4Value;
					break;
				case SerializedPropertyType.Rect:
					to.rectValue = from.rectValue;
					break;
				// case SerializedPropertyType.ArraySize:
				// case SerializedPropertyType.Character:
				case SerializedPropertyType.AnimationCurve:
					to.animationCurveValue = from.animationCurveValue;
					break;
				case SerializedPropertyType.Bounds:
					to.boundsValue = from.boundsValue;
					break;
				case SerializedPropertyType.Quaternion:
					to.quaternionValue = from.quaternionValue;
					break;
				case SerializedPropertyType.ExposedReference:
					to.exposedReferenceValue = from.exposedReferenceValue;
					break;
				// case SerializedPropertyType.FixedBufferSize:
				case SerializedPropertyType.Vector2Int:
					to.vector2IntValue = from.vector2IntValue;
					break;
				case SerializedPropertyType.Vector3Int:
					to.vector3IntValue = from.vector3IntValue;
					break;
				case SerializedPropertyType.RectInt:
					to.rectIntValue = from.rectIntValue;
					break;
				case SerializedPropertyType.BoundsInt:
					to.boundsIntValue = from.boundsIntValue;
					break;
				// case SerializedPropertyType.ManagedReference:
				default:
					return false;
			}
			return true;
		}

	}

}

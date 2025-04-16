using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GreatClock.Common.UI.GreatArtTextStyle;
using static GreatClock.Common.UI.TextToRenderTexture;

namespace GreatClock.Common.UI {

	[RequireComponent(typeof(RawImage)), ExecuteAlways]
	public class GreatArtText : MonoBehaviour, IMeshModifier {

		public static Func<Font, Font> FontReplacementDynamic = null;
		public static Func<BaseFontAsset, BaseFontAsset> FontReplacementAsset = null;

		public string text {
			get { return m_Text; }
			set { if (m_Text == value) { return; } m_Text = value; mDirty = true; }
		}

		public GreatArtTextStyleAsset presetStyle {
			get { return m_PresetStyle; }
			set { if (m_PresetStyle == value) { return; } m_PresetStyle = value; mDirty = true; }
		}

		public FontConfig[] fonts {
			get {
				return m_PresetStyle != null && !m_FontsOverride ? m_PresetStyle.style.fonts : m_Style.fonts;
			}
			set {
				if (m_PresetStyle != null && !m_FontsOverride) { m_FontsOverride = true; mDirty = true; }
				while (true) {
					FontConfig[] prev = m_Style.fonts;
					if (prev.Length != value.Length) { break; }
					bool flag = false;
					for (int i = prev.Length - 1; i >= 0; i--) {
						FontConfig pf = prev[i];
						FontConfig vf = value[i];
						if (pf.useFontAsset != vf.useFontAsset) {
							flag = true;
							break;
						}
						if (pf.useFontAsset) {
							if (pf.fontAsset != vf.fontAsset) {
								flag = true;
								break;
							}
						} else {
							if (pf.font != vf.font) {
								flag = true;
								break;
							}
						}
					}
					if (flag) { break; }
					return;
				}
				if (m_Style.fonts == value) { return; }
				m_Style.fonts = value; mDirty = true;
			}
		}

		public int fontSize {
			get {
				return m_PresetStyle != null && !m_FontSizeOverride ? m_PresetStyle.style.fontSize : m_Style.fontSize;
			}
			set {
				if (m_PresetStyle != null && !m_FontSizeOverride) { m_FontSizeOverride = true; mDirty = true; }
				if (m_Style.fontSize == value) { return; }
				m_Style.fontSize = value; mDirty = true;
			}
		}

		public FontStyle fontStyle {
			get {
				return m_PresetStyle != null && !m_FontStyleOverride ? m_PresetStyle.style.fontStyle : m_Style.fontStyle;
			}
			set {
				if (m_PresetStyle != null && !m_FontStyleOverride) { m_FontStyleOverride = true; mDirty = true; }
				if (m_Style.fontStyle == value) { return; }
				m_Style.fontStyle = value; mDirty = true;
			}
		}

		public float lineHeight {
			get {
				return m_PresetStyle != null && !m_LineHeightOverride ? m_PresetStyle.style.lineHeight : m_Style.lineHeight;
			}
			set {
				if (m_PresetStyle != null && !m_LineHeightOverride) { m_LineHeightOverride = true; mDirty = true; }
				if (m_Style.lineHeight == value) { return; }
				m_Style.lineHeight = value; mDirty = true;
			}
		}

		public float lineSpace {
			get {
				return m_PresetStyle != null && !m_LineSpaceOverride ? m_PresetStyle.style.lineSpace : m_Style.lineSpace;
			}
			set {
				if (m_PresetStyle != null && !m_LineSpaceOverride) { m_LineSpaceOverride = true; mDirty = true; }
				if (m_Style.lineSpace == value) { return; }
				m_Style.lineSpace = value; mDirty = true;
			}
		}

		public eColorType colorType { get { return m_PresetStyle != null && !m_ColorTypeOverride ? m_PresetStyle.style.colorType : m_Style.colorType; } }

		public Color fontColor { get { return m_PresetStyle != null && !m_ColorTypeOverride ? m_PresetStyle.style.fontColor : m_Style.fontColor; } }

		public Gradient colorGradient { get { return m_PresetStyle != null && !m_ColorTypeOverride ? m_PresetStyle.style.colorGradient : m_Style.colorGradient; } }

		public float colorGradientAngle { get { return m_PresetStyle != null && !m_ColorTypeOverride ? m_PresetStyle.style.colorGradientAngle : m_Style.colorGradientAngle; } }

		public Texture colorTexture { get { return m_PresetStyle != null && !m_ColorTypeOverride ? m_PresetStyle.style.colorTexture : m_Style.colorTexture; } }

		public Vector2 colorTextureTiling { get { return m_PresetStyle != null && !m_ColorTypeOverride ? m_PresetStyle.style.colorTextureTiling : m_Style.colorTextureTiling; } }

		public Vector2 colorTextureOffset { get { return m_PresetStyle != null && !m_ColorTypeOverride ? m_PresetStyle.style.colorTextureOffset : m_Style.colorTextureOffset; } }

		public bool colorTexturePerChar { get { return m_PresetStyle != null && !m_ColorTypeOverride ? m_PresetStyle.style.colorTexturePerChar : m_Style.colorTexturePerChar; } }

		public bool SetFontColor(Color color) {
			if (m_PresetStyle != null && !m_ColorTypeOverride) { m_ColorTypeOverride = true; mDirty = true; }
			if (m_Style.SetFontColor(color)) {
				mDirty = true;
				return true;
			}
			return false;
		}

		public bool SetColorGradient(Gradient gradient, float angle, bool perChar) {
			if (m_PresetStyle != null && !m_ColorTypeOverride) { m_ColorTypeOverride = true; mDirty = true; }
			if (m_Style.SetColorGradient(gradient, angle, perChar)) {
				mDirty = true;
				return true;
			}
			return false;
		}

		public bool SetFontTexture(Texture texture, bool perChar) {
			if (m_PresetStyle != null && !m_ColorTypeOverride) { m_ColorTypeOverride = true; mDirty = true; }
			if (m_Style.SetFontTexture(texture, perChar)) {
				mDirty = true;
				return true;
			}
			return false;
		}

		public bool SetFontTexture(Texture texture, Vector2 tiling, Vector2 offset, bool perChar) {
			if (m_PresetStyle != null && !m_ColorTypeOverride) { m_ColorTypeOverride = true; mDirty = true; }
			if (m_Style.SetFontTexture(texture, tiling, offset, perChar)) {
				mDirty = true;
				return true;
			}
			return false;
		}

		public float charSpace {
			get {
				return m_PresetStyle != null && !m_CharSpaceOverride ? m_PresetStyle.style.charSpace : m_Style.charSpace;
			}
			set {
				if (m_PresetStyle != null && !m_CharSpaceOverride) { m_CharSpaceOverride = true; mDirty = true; }
				if (m_Style.charSpace == value) { return; }
				m_Style.charSpace = value; mDirty = true;
			}
		}

		public Vector2 charScale {
			get {
				return m_PresetStyle != null && !m_CharScaleOverride ? m_PresetStyle.style.charScale : m_Style.charScale;
			}
			set {
				if (m_PresetStyle != null && !m_CharScaleOverride) { m_CharScaleOverride = true; mDirty = true; }
				if (m_Style.charScale == value) { return; }
				m_Style.charScale = value; mDirty = true;
			}
		}

		public Vector2 charIncline {
			get {
				return m_PresetStyle != null && !m_CharInclineOverride ? m_PresetStyle.style.charIncline : m_Style.charIncline;
			}
			set {
				if (m_PresetStyle != null && !m_CharInclineOverride) { m_CharInclineOverride = true; mDirty = true; }
				if (m_Style.charIncline == value) { return; }
				m_Style.charIncline = value; mDirty = true;
			}
		}

		public float textBlend {
			get {
				return m_PresetStyle != null && !m_TextBlendOverride ? m_PresetStyle.style.textBlend : m_Style.textBlend;
			}
			set {
				if (m_PresetStyle != null && !m_TextBlendOverride) { m_TextBlendOverride = true; mDirty = true; }
				if (m_Style.textBlend == value) { return; }
				m_Style.textBlend = value; mDirty = true;
			}
		}

		public bool hasCharTrapezoid { get { return m_PresetStyle != null && !m_HasCharTrapezoidOverride ? m_PresetStyle.style.hasCharTrapezoid : m_Style.hasCharTrapezoid; } }

		public eAxis charTrapezoidAxis { get { return m_PresetStyle != null && !m_CharTrapezoidAxisOverride ? m_PresetStyle.style.charTrapezoidAxis : m_Style.charTrapezoidAxis; } }

		public float charTrapezoidRatio { get { return m_PresetStyle != null && !m_CharTrapezoidRatioOverride ? m_PresetStyle.style.charTrapezoidRatio : m_Style.charTrapezoidRatio; } }

		public float charTrapezoidConcaveConvex { get { return m_PresetStyle != null && !m_CharTrapezoidConcaveConvexOverride ? m_PresetStyle.style.charTrapezoidConcaveConvex : m_Style.charTrapezoidConcaveConvex; } }

		public bool SetCharTrapezoid(eAxis axis, float ratio, float concave_convex) {
			if (m_PresetStyle != null) {
				if (!m_HasCharTrapezoidOverride) { m_HasCharTrapezoidOverride = true; mDirty = true; }
				if (!m_CharTrapezoidAxisOverride) { m_CharTrapezoidAxisOverride = true; mDirty = true; }
				if (!m_CharTrapezoidRatioOverride) { m_CharTrapezoidRatioOverride = true; mDirty = true; }
				if (!m_CharTrapezoidConcaveConvexOverride) { m_CharTrapezoidConcaveConvexOverride = true; mDirty = true; }
			}
			if (m_Style.SetCharTrapezoid(axis, ratio, concave_convex)) {
				mDirty = true;
				return true;
			}
			return false;
		}

		public bool ResetCharTrapezoid() {
			if (m_PresetStyle != null && !m_HasCharTrapezoidOverride) { m_HasCharTrapezoidOverride = true; mDirty = true; }
			if (m_Style.ResetCharTrapezoid()) {
				mDirty = true;
				return true;
			}
			return false;
		}

		public eSpacialMode spacialMode { get { return m_PresetStyle != null && !m_SpacialModeOverride ? m_PresetStyle.style.spacialMode : m_Style.spacialMode; } }

		public Vector2 shadowOffset { get { return m_PresetStyle != null && !m_ShadowOffsetOverride ? m_PresetStyle.style.shadowOffset : m_Style.shadowOffset; } }

		public float shadowExtend { get { return m_PresetStyle != null && !m_ShadowExtendOverride ? m_PresetStyle.style.shadowExtend : m_Style.shadowExtend; } }

		public bool shadowAngleBlur { get { return m_PresetStyle != null && !m_ShadowAngleBlurOverride ? m_PresetStyle.style.shadowAngleBlur : m_Style.shadowAngleBlur; } }

		public float shadowBlur { get { return m_PresetStyle != null && !m_ShadowBlurOverride ? m_PresetStyle.style.shadowBlur : m_Style.shadowBlur; } }

		public float shadowBlurDirection { get { return m_PresetStyle != null && !m_ShadowBlurDirectionOverride ? m_PresetStyle.style.shadowBlurDirection : m_Style.shadowBlurDirection; } }

		public Vector2 shadowBlurAlongAndCross { get { return m_PresetStyle != null && !m_ShadowBlurAlongAndCrossOverride ? m_PresetStyle.style.shadowBlurAlongAndCross : m_Style.shadowBlurAlongAndCross; } }

		public FxColorData shadowColor { get { return m_PresetStyle != null && !m_ShadowColorOverride ? m_PresetStyle.style.shadowColor : m_Style.shadowColor; } }

		public float shadowIntensity { get { return m_PresetStyle != null && !m_ShadowIntensityOverride ? m_PresetStyle.style.shadowIntensity : m_Style.shadowIntensity; } }
		
		public Color spacialFaded { get { return m_PresetStyle != null && !m_SpacialFadedOverride ? m_PresetStyle.style.spacialFaded : m_Style.spacialFaded; } }

		public Gradient spacialLight360 { get { return m_PresetStyle != null && !m_SpacialLight360Override ? m_PresetStyle.style.spacialLight360 : m_Style.spacialLight360; } }

		public float spacialLightPhase { get { return m_PresetStyle != null && !m_SpacialLightPhaseOverride ? m_PresetStyle.style.spacialLightPhase : m_Style.spacialLightPhase; } }

		public float spacialLightSoft { get { return m_PresetStyle != null && !m_SpacialLightSoftOverride ? m_PresetStyle.style.spacialLightSoft : m_Style.spacialLightSoft; } }

		public Vector2 perspective3dEndPoint { get { return m_PresetStyle != null && !m_Perspective3dEndPointOverride ? m_PresetStyle.style.perspective3dEndPoint : m_Style.perspective3dEndPoint; } }

		public float perspective3dStretch { get { return m_PresetStyle != null && !m_Perspective3dStretchOverride ? m_PresetStyle.style.perspective3dStretch : m_Style.perspective3dStretch; } }

		public float orthographic3dDirection { get { return m_PresetStyle != null && !m_Orthographic3dDirectionOverride ? m_PresetStyle.style.orthographic3dDirection : m_Style.orthographic3dDirection; } }

		public float orthographic3dLength { get { return m_PresetStyle != null && !m_Orthographic3dLengthOverride ? m_PresetStyle.style.orthographic3dLength : m_Style.orthographic3dLength; } }

		public bool SetSpacialShadow(Vector2 offset, float extend, float blur_angle, Vector2 blur, FxColorData color, float intensity) {
			if (m_PresetStyle != null) {
				if (!m_SpacialModeOverride) { m_SpacialModeOverride = true; mDirty = true; }
				if (!m_ShadowOffsetOverride) { m_ShadowOffsetOverride = true; mDirty = true; }
				if (!m_ShadowExtendOverride) { m_ShadowExtendOverride = true; mDirty = true; }
				if (!m_ShadowAngleBlurOverride) { m_ShadowAngleBlurOverride = true; mDirty = true; }
				if (!m_ShadowBlurDirectionOverride) { m_ShadowBlurDirectionOverride = true; mDirty = true; }
				if (!m_ShadowBlurAlongAndCrossOverride) { m_ShadowBlurAlongAndCrossOverride = true; mDirty = true; }
				if (!m_ShadowColorOverride) { m_ShadowColorOverride = true; mDirty = true; }
				if (!m_ShadowIntensityOverride) { m_ShadowIntensityOverride = true; mDirty = true; }
			}
			if (m_Style.SetSpacialShadow(offset, extend, blur_angle, blur, color, intensity)) {
				mDirty = true;
				return true;
			}
			return false;
		}

		public bool SetSpacialShadow(Vector2 offset, float extend, float blur, FxColorData color, float intensity) {
			if (m_PresetStyle != null) {
				if (!m_SpacialModeOverride) { m_SpacialModeOverride = true; mDirty = true; }
				if (!m_ShadowOffsetOverride) { m_ShadowOffsetOverride = true; mDirty = true; }
				if (!m_ShadowExtendOverride) { m_ShadowExtendOverride = true; mDirty = true; }
				if (!m_ShadowAngleBlurOverride) { m_ShadowAngleBlurOverride = true; mDirty = true; }
				if (!m_ShadowBlurOverride) { m_ShadowBlurOverride = true; mDirty = true; }
				if (!m_ShadowColorOverride) { m_ShadowColorOverride = true; mDirty = true; }
				if (!m_ShadowIntensityOverride) { m_ShadowIntensityOverride = true; mDirty = true; }
			}
			if (m_Style.SetSpacialShadow(offset, extend, blur, color, intensity)) {
				mDirty = true;
				return true;
			}
			return false;
		}

		public bool ResetSpacial() {
			if (m_PresetStyle != null && !m_SpacialModeOverride) { m_SpacialModeOverride = true; mDirty = true; }
			if (m_Style.ResetSpacial()) {
				mDirty = true;
				return true;
			}
			return false;
		}

		public bool hasOutline { get { return m_PresetStyle != null && !m_HasOutlineOverride ? m_PresetStyle.style.hasOutline : m_Style.hasOutline; } }

		public float outlineSize { get { return m_PresetStyle != null && !m_OutlineSizeOverride ? m_PresetStyle.style.outlineSize : m_Style.outlineSize; } }

		public FxColorData outlineColor { get { return m_PresetStyle != null && !m_OutlineColorOverride ? m_PresetStyle.style.outlineColor : m_Style.outlineColor; } }

		public bool outlineFillText { get { return m_PresetStyle != null && !m_OutlineFillTextOverride ? m_PresetStyle.style.outlineFillText : m_Style.outlineFillText; } }

		public bool SetOutline(float size, FxColorData color, bool fillText) {
			if (m_PresetStyle != null) {
				if (!m_HasOutlineOverride) { m_HasOutlineOverride = true; mDirty = true; }
				if (!m_OutlineSizeOverride) { m_OutlineSizeOverride = true; mDirty = true; }
				if (!m_OutlineColorOverride) { m_OutlineColorOverride = true; mDirty = true; }
				if (!m_OutlineFillTextOverride) { m_OutlineFillTextOverride = true; mDirty = true; }
			}
			if (m_Style.SetOutline(size, color, fillText)) {
				mDirty = true;
				return true;
			}
			return false;
		}

		public bool ResetOutline() {
			if (m_PresetStyle != null && !m_HasOutlineOverride) { m_HasOutlineOverride = true; mDirty = true; }
			if (m_Style.ResetOutline()) {
				mDirty = true;
				return true;
			}
			return false;
		}

		public eInnerFxMode innerFxMode { get { return m_PresetStyle != null && !m_InnerFxModeOverride ? m_PresetStyle.style.innerFxMode : m_Style.innerFxMode; } }

		public FxColorData innerFxColor { get { return m_PresetStyle != null && !m_InnerFxColorOverride ? m_PresetStyle.style.innerFxColor : m_Style.innerFxColor; } }

		public float innerBlurFix { get { return m_PresetStyle != null && !m_InnerBlurFixOverride ? m_PresetStyle.style.innerBlurFix : m_Style.innerBlurFix; } }

		public float innerBlur { get { return m_PresetStyle != null && !m_InnerBlurOverride ? m_PresetStyle.style.innerBlur : m_Style.innerBlur; } }

		public float innerShadowDirection { get { return m_PresetStyle != null && !m_InnerShadowDirectionOverride ? m_PresetStyle.style.innerShadowDirection : m_Style.innerShadowDirection; } }

		public eInnerFxBlend innerFxBlend { get { return m_PresetStyle != null && !m_InnerFxBlendOverride ? m_PresetStyle.style.innerFxBlend : m_Style.innerFxBlend; } }

		public float innerMultiply { get { return m_PresetStyle != null && !m_InnerMultiplyOverride ? m_PresetStyle.style.innerMultiply : m_Style.innerMultiply; } }

		public Color embossLightColor { get { return m_PresetStyle != null && !m_EmbossLightColorOverride ? m_PresetStyle.style.embossLightColor : m_Style.embossLightColor; } }

		public float embossLightDirection { get { return m_PresetStyle != null && !m_EmbossLightDirectionOverride ? m_PresetStyle.style.embossLightDirection : m_Style.embossLightDirection; } }

		public float embossIntensity { get { return m_PresetStyle != null && !m_EmbossIntensityOverride ? m_PresetStyle.style.embossIntensity : m_Style.embossIntensity; } }

		public float embossBevel { get { return m_PresetStyle != null && !m_EmbossBevelOverride ? m_PresetStyle.style.embossBevel : m_Style.embossBevel; } }

		public bool SetInnerAlphaBlend(float blur, FxColorData color, float power) {
			if (m_PresetStyle != null) {
				if (!m_InnerFxModeOverride) { m_InnerFxModeOverride = true; mDirty = true; }
				if (!m_InnerFxBlendOverride) { m_InnerFxBlendOverride = true; mDirty = true; }
				if (!m_InnerBlurOverride) { m_InnerBlurOverride = true; mDirty = true; }
				if (!m_InnerFxColorOverride) { m_InnerFxColorOverride = true; mDirty = true; }
				if (!m_InnerBlurFixOverride) { m_InnerBlurFixOverride = true; mDirty = true; }
			}
			if (m_Style.SetInnerAlphaBlend(blur, color, power)) {
				mDirty = true;
				return true;
			}
			return false;
		}

		public bool SetInnerShadowAlphaBlend(float blur_angle, float blur, FxColorData color, float power) {
			if (m_PresetStyle != null) {
				if (!m_InnerFxModeOverride) { m_InnerFxModeOverride = true; mDirty = true; }
				if (!m_InnerFxBlendOverride) { m_InnerFxBlendOverride = true; mDirty = true; }
				if (!m_InnerShadowDirectionOverride) { m_InnerShadowDirectionOverride = true; mDirty = true; }
				if (!m_InnerBlurOverride) { m_InnerBlurOverride = true; mDirty = true; }
				if (!m_InnerFxColorOverride) { m_InnerFxColorOverride = true; mDirty = true; }
				if (!m_InnerBlurFixOverride) { m_InnerBlurFixOverride = true; mDirty = true; }
			}
			if (m_Style.SetInnerShadowAlphaBlend(blur_angle, blur, color, power)) {
				mDirty = true;
				return true;
			}
			return false;
		}

		public bool SetInnerAdditive(float blur, FxColorData color, float power, float multiply) {
			if (m_PresetStyle != null) {
				if (!m_InnerFxModeOverride) { m_InnerFxModeOverride = true; mDirty = true; }
				if (!m_InnerFxBlendOverride) { m_InnerFxBlendOverride = true; mDirty = true; }
				if (!m_InnerBlurOverride) { m_InnerBlurOverride = true; mDirty = true; }
				if (!m_InnerFxColorOverride) { m_InnerFxColorOverride = true; mDirty = true; }
				if (!m_InnerBlurFixOverride) { m_InnerBlurFixOverride = true; mDirty = true; }
				if (!m_InnerMultiplyOverride) { m_InnerMultiplyOverride = true; mDirty = true; }
			}
			if (m_Style.SetInnerAdditive(blur, color, power, multiply)) {
				mDirty = true;
				return true;
			}
			return false;
		}

		public bool SetInnerShadowAdditive(float blur_angle, float blur, FxColorData color, float power, float multiply) {
			if (m_PresetStyle != null) {
				if (!m_InnerFxModeOverride) { m_InnerFxModeOverride = true; mDirty = true; }
				if (!m_InnerFxBlendOverride) { m_InnerFxBlendOverride = true; mDirty = true; }
				if (!m_InnerShadowDirectionOverride) { m_InnerShadowDirectionOverride = true; mDirty = true; }
				if (!m_InnerBlurOverride) { m_InnerBlurOverride = true; mDirty = true; }
				if (!m_InnerFxColorOverride) { m_InnerFxColorOverride = true; mDirty = true; }
				if (!m_InnerBlurFixOverride) { m_InnerBlurFixOverride = true; mDirty = true; }
				if (!m_InnerMultiplyOverride) { m_InnerMultiplyOverride = true; mDirty = true; }
			}
			if (m_Style.SetInnerShadowAdditive(blur_angle, blur, color, power, multiply)) {
				mDirty = true;
				return true;
			}
			return false;
		}

		public bool SetInnerEmboss(Color light_color, float light_angle, float intensity, float bevel) {
			if (m_PresetStyle != null) {
				if (!m_InnerFxModeOverride) { m_InnerFxModeOverride = true; mDirty = true; }
				if (!m_EmbossLightColorOverride) { m_EmbossLightColorOverride = true; mDirty = true; }
				if (!m_EmbossLightDirectionOverride) { m_EmbossLightDirectionOverride = true; mDirty = true; }
				if (!m_EmbossIntensityOverride) { m_EmbossIntensityOverride = true; mDirty = true; }
				if (!m_EmbossBevelOverride) { m_EmbossBevelOverride = true; mDirty = true; }
			}
			if (m_Style.SetInnerEmboss(light_color, light_angle, intensity, bevel)) {
				mDirty = true;
				return true;
			}
			return false;
		}

		public bool ResetInner() {
			if (m_PresetStyle != null && !m_InnerFxModeOverride) { m_InnerFxModeOverride = true; mDirty = true; }
			if (m_Style.ResetInner()) {
				mDirty = true;
				return true;
			}
			return false;
		}

		public bool hasGlow { get { return m_PresetStyle != null && !m_HasGlowOverride ? m_PresetStyle.style.hasGlow : m_Style.hasGlow; } }

		public float glowExtend { get { return m_PresetStyle != null && !m_GlowExtendOverride ? m_PresetStyle.style.glowExtend : m_Style.glowExtend; } }

		public bool glowAngleBlur { get { return m_PresetStyle != null && !m_GlowAngleBlurOverride ? m_PresetStyle.style.glowAngleBlur : m_Style.glowAngleBlur; } }

		public float glowBlur { get { return m_PresetStyle != null && !m_GlowBlurOverride ? m_PresetStyle.style.glowBlur : m_Style.glowBlur; } }

		public float glowBlurDirection { get { return m_PresetStyle != null && !m_GlowBlurDirectionOverride ? m_PresetStyle.style.glowBlurDirection : m_Style.glowBlurDirection; } }

		public Vector2 glowBlurAlongAndCross { get { return m_PresetStyle != null && !m_GlowBlurAlongAndCrossOverride ? m_PresetStyle.style.glowBlurAlongAndCross : m_Style.glowBlurAlongAndCross; } }

		public FxColorData glowColor { get { return m_PresetStyle != null && !m_GlowColorOverride ? m_PresetStyle.style.glowColor : m_Style.glowColor; } }

		public float glowBlurFix { get { return m_PresetStyle != null && !m_GlowBlurFixOverride ? m_PresetStyle.style.glowBlurFix : m_Style.glowBlurFix; } }

		public float glowIntensity { get { return m_PresetStyle != null && !m_GlowIntensityOverride ? m_PresetStyle.style.glowIntensity : m_Style.glowIntensity; } }

		public bool SetGlow(float extend, float blur_angle, Vector2 blur, FxColorData color, float power, float intensity) {
			if (m_PresetStyle != null) {
				if (!m_HasGlowOverride) { m_HasGlowOverride = true; mDirty = true; }
				if (!m_GlowAngleBlurOverride) { m_GlowAngleBlurOverride = true; mDirty = true; }
				if (!m_GlowExtendOverride) { m_GlowExtendOverride = true; mDirty = true; }
				if (!m_GlowBlurDirectionOverride) { m_GlowBlurDirectionOverride = true; mDirty = true; }
				if (!m_GlowBlurAlongAndCrossOverride) { m_GlowBlurAlongAndCrossOverride = true; mDirty = true; }
				if (!m_GlowColorOverride) { m_GlowColorOverride = true; mDirty = true; }
				if (!m_GlowBlurFixOverride) { m_GlowBlurFixOverride = true; mDirty = true; }
				if (!m_GlowIntensityOverride) { m_GlowIntensityOverride = true; mDirty = true; }
			}
			if (m_Style.SetGlow(extend, blur_angle, blur, color, power, intensity)) {
				mDirty = true;
				return true;
			}
			return false;
		}

		public bool SetGlow(float extend, float blur, FxColorData color, float power, float intensity) {
			if (m_PresetStyle != null) {
				if (!m_HasGlowOverride) { m_HasGlowOverride = true; mDirty = true; }
				if (!m_GlowAngleBlurOverride) { m_GlowAngleBlurOverride = true; mDirty = true; }
				if (!m_GlowExtendOverride) { m_GlowExtendOverride = true; mDirty = true; }
				if (!m_GlowBlurOverride) { m_GlowBlurOverride = true; mDirty = true; }
				if (!m_GlowColorOverride) { m_GlowColorOverride = true; mDirty = true; }
				if (!m_GlowBlurFixOverride) { m_GlowBlurFixOverride = true; mDirty = true; }
				if (!m_GlowIntensityOverride) { m_GlowIntensityOverride = true; mDirty = true; }
			}
			if (m_Style.SetGlow(extend, blur, color, power, intensity)) {
				mDirty = true;
				return true;
			}
			return false;
		}

		public bool ResetGlow() {
			if (m_Style.ResetGlow()) {
				mDirty = true;
				return true;
			}
			return false;
		}

		void IMeshModifier.ModifyMesh(Mesh mesh) { }

		void IMeshModifier.ModifyMesh(VertexHelper verts) {
			if (mRT == null || m_RectWrapAll) { return; }
			Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
			Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
			UIVertex vert = new UIVertex();
			int n = verts.currentVertCount;
			for (int i = 0; i < n; i++) {
				verts.PopulateUIVertex(ref vert, i);
				min = Vector3.Min(vert.position, min);
				max = Vector3.Max(vert.position, max);
			}
			Vector3 min1 = min - (Vector3)mTextRect.min;
			Vector3 max1 = max + new Vector3(mRT.width, mRT.height) - (Vector3)mTextRect.max;
			float tx, ty;
			for (int i = 0; i < n; i++) {
				verts.PopulateUIVertex(ref vert, i);
				tx = Mathf.InverseLerp(min.x, max.x, vert.position.x);
				ty = Mathf.InverseLerp(min.y, max.y, vert.position.y);
				vert.position.x = Mathf.LerpUnclamped(min1.x, max1.x, tx);
				vert.position.y = Mathf.LerpUnclamped(min1.y, max1.y, ty);
				verts.SetUIVertex(vert, i);
			}
		}

		[SerializeField, HideInInspector]
		private bool m_RectWrapAll = false;
		[SerializeField, HideInInspector]
		private string m_Text;
		[SerializeField, HideInInspector]
		private GreatArtTextStyle m_Style = new GreatArtTextStyle();
		[SerializeField, HideInInspector]
		private GreatArtTextStyleAsset m_PresetStyle;
		[SerializeField, HideInInspector]
		private bool m_FontsOverride;
		[SerializeField, HideInInspector]
		private bool m_FontSizeOverride;
		[SerializeField, HideInInspector]
		private bool m_FontStyleOverride;
		[SerializeField, HideInInspector]
		private bool m_LineHeightOverride;
		[SerializeField, HideInInspector]
		private bool m_LineSpaceOverride;
		[SerializeField, HideInInspector]
		private bool m_ColorTypeOverride;
		[SerializeField, HideInInspector]
		private bool m_CharSpaceOverride;
		[SerializeField, HideInInspector]
		private bool m_CharScaleOverride;
		[SerializeField, HideInInspector]
		private bool m_CharInclineOverride;
		[SerializeField, HideInInspector]
		private bool m_TextBlendOverride;
		[SerializeField, HideInInspector]
		private bool m_HasCharTrapezoidOverride;
		[SerializeField, HideInInspector]
		private bool m_CharTrapezoidAxisOverride;
		[SerializeField, HideInInspector]
		private bool m_CharTrapezoidRatioOverride;
		[SerializeField, HideInInspector]
		private bool m_CharTrapezoidConcaveConvexOverride;
		[SerializeField, HideInInspector]
		private bool m_SpacialModeOverride;
		[SerializeField, HideInInspector]
		private bool m_ShadowOffsetOverride;
		[SerializeField, HideInInspector]
		private bool m_ShadowExtendOverride;
		[SerializeField, HideInInspector]
		private bool m_ShadowAngleBlurOverride;
		[SerializeField, HideInInspector]
		private bool m_ShadowBlurOverride;
		[SerializeField, HideInInspector]
		private bool m_ShadowBlurDirectionOverride;
		[SerializeField, HideInInspector]
		private bool m_ShadowBlurAlongAndCrossOverride;
		[SerializeField, HideInInspector]
		private bool m_ShadowColorOverride;
		[SerializeField, HideInInspector]
		private bool m_ShadowIntensityOverride;
		[SerializeField, HideInInspector]
		private bool m_SpacialFadedOverride;
		[SerializeField, HideInInspector]
		private bool m_SpacialLight360Override;
		[SerializeField, HideInInspector]
		private bool m_SpacialLightPhaseOverride;
		[SerializeField, HideInInspector]
		private bool m_SpacialLightSoftOverride;
		[SerializeField, HideInInspector]
		private bool m_Perspective3dEndPointOverride;
		[SerializeField, HideInInspector]
		private bool m_Perspective3dStretchOverride;
		[SerializeField, HideInInspector]
		private bool m_Orthographic3dDirectionOverride;
		[SerializeField, HideInInspector]
		private bool m_Orthographic3dLengthOverride;
		[SerializeField, HideInInspector]
		private bool m_HasOutlineOverride;
		[SerializeField, HideInInspector]
		private bool m_OutlineSizeOverride;
		[SerializeField, HideInInspector]
		private bool m_OutlineColorOverride;
		[SerializeField, HideInInspector]
		private bool m_OutlineFillTextOverride;
		[SerializeField, HideInInspector]
		private bool m_InnerFxModeOverride;
		[SerializeField, HideInInspector]
		private bool m_InnerFxColorOverride;
		[SerializeField, HideInInspector]
		private bool m_InnerBlurFixOverride;
		[SerializeField, HideInInspector]
		private bool m_InnerBlurOverride;
		[SerializeField, HideInInspector]
		private bool m_InnerShadowDirectionOverride;
		[SerializeField, HideInInspector]
		private bool m_InnerFxBlendOverride;
		[SerializeField, HideInInspector]
		private bool m_InnerMultiplyOverride;
		[SerializeField, HideInInspector]
		private bool m_EmbossLightColorOverride;
		[SerializeField, HideInInspector]
		private bool m_EmbossLightDirectionOverride;
		[SerializeField, HideInInspector]
		private bool m_EmbossIntensityOverride;
		[SerializeField, HideInInspector]
		private bool m_EmbossBevelOverride;
		[SerializeField, HideInInspector]
		private bool m_HasGlowOverride;
		[SerializeField, HideInInspector]
		private bool m_GlowExtendOverride;
		[SerializeField, HideInInspector]
		private bool m_GlowAngleBlurOverride;
		[SerializeField, HideInInspector]
		private bool m_GlowBlurOverride;
		[SerializeField, HideInInspector]
		private bool m_GlowBlurDirectionOverride;
		[SerializeField, HideInInspector]
		private bool m_GlowBlurAlongAndCrossOverride;
		[SerializeField, HideInInspector]
		private bool m_GlowColorOverride;
		[SerializeField, HideInInspector]
		private bool m_GlowBlurFixOverride;
		[SerializeField, HideInInspector]
		private bool m_GlowIntensityOverride;

		private RectTransform mTrans;
		private RawImage mImage;
		private RenderTexture mRT;
		private Rect mTextRect;

		private bool mDirty = false;

		void Awake() {
			mTrans = transform as RectTransform;
			mImage = GetComponent<RawImage>();
			mImage.enabled = false;
		}

		void OnEnable() {
			mDirty = true;
		}

		void LateUpdate() {
			if (mDirty || (mRT != null && !mRT.IsCreated())) {
				mDirty = false;
				UpdateContent();
			}
		}

		void OnDisable() {
			if (mImage != null && !mImage.Equals(null)) {
				mImage.texture = null;
				mImage.enabled = false;
			}
			if (mRT != null) {
				if (RenderTexture.active == mRT) {
					RenderTexture.active = null;
				}
#if UNITY_EDITOR
				DestroyImmediate(mRT);
#else
				Destroy(mRT);
#endif
			}
			mRT = null;
		}

		private IFontInternal[] mCachedFonts;

		private Dictionary<Font, FontDynamic> mFontDict = null;

		private void UpdateContent() {
			Parameter paras = Parameter.Default;
			paras.SetLineHeightAndSpace(lineHeight, lineSpace);
			paras.SetTextTexture(colorTexture, colorTextureTiling, colorTextureOffset, colorTexturePerChar);
			switch (colorType) {
				case eColorType.Color:
					paras.SetTextColor(fontColor);
					break;
				case eColorType.Gradient:
					paras.SetTextGradient(colorGradient, colorGradientAngle, false);
					break;
				case eColorType.GradientPerChar:
					paras.SetTextGradient(colorGradient, colorGradientAngle, true);
					break;
			}
			paras.SetCharSpace(charSpace * fontSize * 0.5f);
			paras.SetScale(charScale);
			paras.SetIncline(charIncline);
			paras.SetTextBlend(textBlend);
			if (hasCharTrapezoid && charTrapezoidRatio != 0f) {
				if (charTrapezoidAxis == eAxis.Horizontal) {
					paras.SetCharTrapezoidHorizontal(charTrapezoidRatio, charTrapezoidConcaveConvex);
				} else {
					paras.SetCharTrapezoidVertical(charTrapezoidRatio, charTrapezoidConcaveConvex);
				}
			}
			switch (spacialMode) {
				case eSpacialMode.Shadow:
					if (shadowAngleBlur) {
						paras.SetSpacialShadow(shadowOffset, shadowExtend, shadowBlurDirection, shadowBlurAlongAndCross, shadowColor.ToPara(), shadowIntensity);
					} else {
						paras.SetSpacialShadow(shadowOffset, shadowExtend, shadowBlur, shadowColor.ToPara(), shadowIntensity);
					}
					break;
				case eSpacialMode.Perspective3D:
					paras.SetSpacialPerspective3D(perspective3dEndPoint, perspective3dStretch, spacialFaded, spacialLight360, spacialLightPhase, spacialLightSoft);
					break;
				case eSpacialMode.Orthographic3D:
					paras.SetSpacialOrthographic3D(orthographic3dDirection, orthographic3dLength, spacialFaded, spacialLight360, spacialLightPhase, spacialLightSoft);
					break;
			}
			if (hasOutline) {
				paras.SetOutline(outlineSize, outlineColor.ToPara(), outlineFillText);
			}
			switch (innerFxMode) {
				case eInnerFxMode.Light:
					if (innerFxBlend == eInnerFxBlend.AlphaBlend) {
						paras.SetInnerAlphaBlend(innerBlur, innerFxColor.ToPara(), innerBlurFix);
					} else if (innerMultiply != 0f) {
						paras.SetInnerAdditive(innerBlur, innerFxColor.ToPara(), innerBlurFix, innerMultiply);
					}
					break;
				case eInnerFxMode.Shadow:
					if (innerFxBlend == eInnerFxBlend.AlphaBlend) {
						paras.SetInnerShadowAlphaBlend(innerShadowDirection, innerBlur, innerFxColor.ToPara(), innerBlurFix);
					} else if (innerMultiply != 0f) {
						paras.SetInnerShadowAdditive(innerShadowDirection, innerBlur, innerFxColor.ToPara(), innerBlurFix, innerMultiply);
					}
					break;
				case eInnerFxMode.Emboss:
					paras.SetInnerEmboss(embossLightColor, embossLightDirection, embossIntensity, embossBevel);
					break;
			}
			if (hasGlow) {
				if (glowAngleBlur) {
					paras.SetGlow(glowExtend, glowBlurDirection, glowBlurAlongAndCross, glowColor.ToPara(), glowBlurFix, glowIntensity);
				} else {
					paras.SetGlow(glowExtend, glowBlur, glowColor.ToPara(), glowBlurFix, glowIntensity);
				}
			}
			FontConfig[] fnts = fonts;
			int length = fnts.Length;
			if (mCachedFonts == null || mCachedFonts.Length != length) {
				mCachedFonts = new IFontInternal[length];
			}
			for (int i = 0; i < length; i++) {
				FontConfig fnt = fnts[i];
				if (fnt.useFontAsset) {
					BaseFontAsset f = fnt.fontAsset;
					if (f != null && !f.Equals(null) && FontReplacementAsset != null && Application.isPlaying) {
						f = FontReplacementAsset(f);
					}
					mCachedFonts[i] = f;
				} else {
					Font f = fnt.font;
					FontDynamic fd = null;
					if (f != null && !f.Equals(null)) {
						if (FontReplacementDynamic != null && Application.isPlaying) {
							f = FontReplacementDynamic(f);
						}
						if (mFontDict == null) {
							mFontDict = new Dictionary<Font, FontDynamic>();
							fd = new FontDynamic(f);
							mFontDict.Add(f, fd);
						} else {
							if (!mFontDict.TryGetValue(f, out fd)) {
								fd = new FontDynamic(f);
								mFontDict.Add(f, fd);
							}
						}
					}
					mCachedFonts[i] = fd;
				}
			}
			ArtTextTexture at = RenderText(mCachedFonts, m_Text, fontSize, fontStyle, paras, eRenderTextureAllocType.Alloc);
			mRT = at.texture;
			mImage.texture = mRT;
			if (m_RectWrapAll) {
				mImage.SetNativeSize();
			} else if (mRT != null) {
				mTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, at.rect.width);
				mTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, at.rect.height);
			}
			mImage.enabled = mRT != null;
			mTextRect = at.rect;
		}

#if UNITY_EDITOR
		void OnValidate() {
			mDirty = true;
		}

		[ContextMenu("Save Art Text Image")]
		void SaveToPng() {
			if (mRT == null) { return; }
			string fn = name;
			if (string.IsNullOrEmpty(fn)) { fn = "art_text"; }
			string path = UnityEditor.EditorUtility.SaveFilePanel("Save Art Text Image", ".", fn, "png");
			if (string.IsNullOrEmpty(path)) { return; }
			Texture2D tex = new Texture2D(mRT.width, mRT.height, TextureFormat.ARGB32, false, false);
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = mRT;
			tex.ReadPixels(new Rect(0f, 0f, mRT.width, mRT.height), 0, 0, false);
			RenderTexture.active = active;
			tex.Apply(false, false);
			System.IO.File.WriteAllBytes(path, tex.EncodeToPNG());
			DestroyImmediate(tex);
			UnityEditor.EditorUtility.OpenWithDefaultApp(System.IO.Path.GetDirectoryName(path));
		}
#endif

	}

}
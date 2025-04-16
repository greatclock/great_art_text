using System;
using System.Net;
using UnityEngine;

namespace GreatClock.Common.UI {

	[Serializable]
	public class GreatArtTextStyle {

		[Serializable]
		public class FontConfig {
			[SerializeField]
			private bool m_UseFontAsset = false;
			[SerializeField]
			private Font m_Font;
			[SerializeField]
			private BaseFontAsset m_FontAsset;
			public bool useFontAsset { get { return m_UseFontAsset; } }
			public Font font { get { return m_Font; } }
			public BaseFontAsset fontAsset { get { return m_FontAsset; } }
			public FontConfig(Font font) { m_UseFontAsset = false; m_Font = font; }
			public FontConfig(BaseFontAsset font) { m_UseFontAsset = true; m_FontAsset = font; }
		}

		public enum eColorType {
			Color, Gradient, GradientPerChar
		}

		public enum eAxis {
			Horizontal, Vertical
		}

		public enum eSpacialMode {
			None, Shadow, Perspective3D, Orthographic3D
		}

		public enum eInnerFxMode {
			None, Light, Shadow, Emboss
		}

		public enum eInnerFxBlend {
			AlphaBlend, Additive
		}

		public enum eFxColorType {
			Color, Gradient
		}

		[Serializable]
		public class FxColorData {
			[SerializeField]
			private eFxColorType m_ColorType;
			[SerializeField]
			private Color m_Color = Color.gray;
			[SerializeField]
			private Gradient m_Gradient;
			[SerializeField, Range(-360f, 360f)]
			private float m_GradientAngle;
			[SerializeField]
			private Texture m_Texture;
			[SerializeField]
			private Vector2 m_TextureTiling = Vector2.one;
			[SerializeField]
			private Vector2 m_TextureOffset = Vector2.zero;

			private bool mDirty = true;

			public FxColorData SetColor(Color color) {
				if (m_ColorType == eFxColorType.Color && m_Texture == null && m_Color == color) { return this; }
				m_ColorType = eFxColorType.Color; m_Texture = null; m_Color = color;
				mDirty = true;
				return this;
			}

			public FxColorData SetColor(Texture texture, Color color) {
				if (m_ColorType == eFxColorType.Color && m_Texture == texture && m_Color == color) { return this; }
				m_ColorType = eFxColorType.Color; m_Texture = texture; m_TextureTiling = Vector2.one; m_TextureOffset = Vector2.zero; m_Color = color;
				mDirty = true;
				return this;
			}

			public FxColorData SetColor(Texture texture, Vector2 tiling, Vector2 offset, Color color) {
				if (m_ColorType == eFxColorType.Color && m_Texture == texture && m_TextureTiling == Vector2.one && m_TextureOffset == Vector2.zero && m_Color == color) { return this; }
				m_ColorType = eFxColorType.Color; m_Texture = texture; m_TextureTiling = tiling; m_TextureOffset = offset; m_Color = color;
				mDirty = true;
				return this;
			}

			public FxColorData SetGradient(Gradient gradient, float angle) {
				if (m_ColorType == eFxColorType.Gradient && m_Texture == null && m_Gradient == gradient && m_GradientAngle == angle) { return this; }
				m_ColorType = eFxColorType.Gradient; m_Texture = null; m_Gradient = gradient; m_GradientAngle = angle;
				mDirty = true;
				return this;
			}

			public FxColorData SetGradient(Texture texture, Gradient gradient, float angle) {
				if (m_ColorType == eFxColorType.Gradient && m_Texture == texture && m_TextureTiling == Vector2.one && m_TextureOffset == Vector2.zero && m_Gradient == gradient && m_GradientAngle == angle) { return this; }
				m_ColorType = eFxColorType.Gradient; m_Texture = texture; m_TextureTiling = Vector2.one; m_TextureOffset = Vector2.zero; m_Gradient = gradient; m_GradientAngle = angle;
				mDirty = true;
				return this;
			}

			public FxColorData SetGradient(Texture texture, Vector2 tiling, Vector2 offset, Gradient gradient, float angle) {
				if (m_ColorType == eFxColorType.Gradient && m_Texture == texture && m_TextureTiling == tiling && m_TextureOffset == offset && m_Gradient == gradient && m_GradientAngle == angle) { return this; }
				m_ColorType = eFxColorType.Gradient; m_Texture = texture; m_TextureTiling = tiling; m_TextureOffset = offset; m_Gradient = gradient; m_GradientAngle = angle;
				mDirty = true;
				return this;
			}

			public bool dirty { get { return mDirty; } }

			public TextToRenderTexture.FxColor ToPara() {
				mDirty = false;
				switch (m_ColorType) {
					case eFxColorType.Gradient:
						return TextToRenderTexture.FxColor.Gradient(m_Gradient, m_GradientAngle)
							.SetTexture(m_Texture, m_TextureTiling, m_TextureOffset);
				}
				return TextToRenderTexture.FxColor.Color(m_Color).SetTexture(m_Texture, m_TextureTiling, m_TextureOffset);
			}

		}

		[SerializeField, HideInInspector]
		private FontConfig[] m_Fonts;
		[SerializeField, HideInInspector, Min(8)]
		private int m_FontSize = 32;
		[SerializeField, HideInInspector]
		private FontStyle m_FontStyle;
		[SerializeField, HideInInspector]
		private float m_LineHeight = 1f;
		[SerializeField, HideInInspector]
		private float m_LineSpace = 0f;
		[SerializeField, HideInInspector]
		private eColorType m_ColorType = eColorType.Color;
		[SerializeField, HideInInspector]
		private Color m_FontColor = Color.white;
		[SerializeField, HideInInspector]
		private Gradient m_ColorGradient;
		[SerializeField, HideInInspector, Range(-360f, 360f)]
		private float m_ColorGradientAngle = -90f;
		[SerializeField, HideInInspector]
		private Texture m_ColorTexture;
		[SerializeField, HideInInspector]
		private Vector2 m_ColorTextureTiling = Vector2.one;
		[SerializeField, HideInInspector]
		private Vector2 m_ColorTextureOffset = Vector2.zero;
		[SerializeField, HideInInspector]
		private bool m_ColorTexturePerChar = false;
		[SerializeField, HideInInspector, Range(-4f, 4f)]
		private float m_CharSpace = 0f;
		[SerializeField, HideInInspector]
		private Vector2 m_CharScale = Vector2.one;
		[SerializeField, HideInInspector]
		private Vector2 m_CharIncline = Vector2.zero;
		[SerializeField, HideInInspector, Range(0f, 1f)]
		private float m_TextBlend = 1f;
		[SerializeField, HideInInspector]
		private bool m_HasCharTrapezoid = false;
		[SerializeField, HideInInspector]
		private eAxis m_CharTrapezoidAxis = eAxis.Vertical;
		[SerializeField, HideInInspector, Range(-1f, 1f)]
		private float m_CharTrapezoidRatio = 0.2f;
		[SerializeField, HideInInspector, Range(-5f, 5f)]
		private float m_CharTrapezoidConcaveConvex = 0f;
		[SerializeField, HideInInspector]
		private eSpacialMode m_SpacialMode;
		[SerializeField, HideInInspector]
		private Vector2 m_ShadowOffset;
		[SerializeField, HideInInspector, Range(0f, 100f)]
		private float m_ShadowExtend;
		[SerializeField, HideInInspector]
		private bool m_ShadowAngleBlur;
		[SerializeField, HideInInspector, Range(0f, 100f)]
		private float m_ShadowBlur;
		[SerializeField, HideInInspector, Range(-360f, 360f)]
		private float m_ShadowBlurDirection;
		[SerializeField, HideInInspector]
		private Vector2 m_ShadowBlurAlongAndCross;
		[SerializeField, HideInInspector]
		private FxColorData m_ShadowColor = new FxColorData().SetColor(Color.gray);
		[SerializeField, HideInInspector, Range(0f, 10f)]
		private float m_ShadowIntensity = 1f;
		[SerializeField, HideInInspector]
		private Gradient m_SpacialLight360;
		[SerializeField, HideInInspector, Range(-360f, 360f)]
		private float m_SpacialLightPhase;
		[SerializeField, HideInInspector, Range(0f, 8f)]
		private float m_SpacialLightSoft = 4f;
		[SerializeField, HideInInspector]
		private Color m_SpacialFaded = Color.clear;
		[SerializeField, HideInInspector]
		private Vector2 m_Perspective3dEndPoint;
		[SerializeField, HideInInspector, Range(0f, 1f)]
		private float m_Perspective3dStretch = 0.1f;
		[SerializeField, HideInInspector, Range(-360f, 360f)]
		private float m_Orthographic3dDirection;
		[SerializeField, HideInInspector]
		private float m_Orthographic3dLength = 5f;
		[SerializeField, HideInInspector]
		private bool m_HasOutline;
		[SerializeField, HideInInspector, Range(0f, 50f)]
		private float m_OutlineSize = 1f;
		[SerializeField, HideInInspector]
		private FxColorData m_OutlineColor = new FxColorData().SetColor(Color.black);
		[SerializeField, HideInInspector]
		private bool m_OutlineFillText = true;
		[SerializeField, HideInInspector]
		private eInnerFxMode m_InnerFxMode = eInnerFxMode.None;
		[SerializeField, HideInInspector]
		private FxColorData m_InnerFxColor = new FxColorData().SetColor(Color.red);
		[SerializeField, HideInInspector, Range(-5f, 5f)]
		private float m_InnerBlurFix = 0f;
		[SerializeField, HideInInspector, Range(0f, 32f)]
		private float m_InnerBlur = 2f;
		[SerializeField, HideInInspector, Range(-360f, 360f)]
		private float m_InnerShadowDirection = 0f;
		[SerializeField, HideInInspector]
		private eInnerFxBlend m_InnerFxBlend = eInnerFxBlend.AlphaBlend;
		[SerializeField, HideInInspector, Range(-20f, 20f)]
		private float m_InnerMultiply = 1f;
		[SerializeField, HideInInspector]
		private Color m_EmbossLightColor = Color.white;
		[SerializeField, HideInInspector, Range(-360f, 360f)]
		private float m_EmbossLightDirection = 60f;
		[SerializeField, HideInInspector, Range(0f, 32f)]
		private float m_EmbossBevel = 2f;
		[SerializeField, HideInInspector, Range(0f, 5f)]
		private float m_EmbossIntensity = 1f;
		[SerializeField, HideInInspector]
		private bool m_HasGlow;
		[SerializeField, HideInInspector, Range(0f, 100f)]
		private float m_GlowExtend = 8;
		[SerializeField, HideInInspector]
		private bool m_GlowAngleBlur;
		[SerializeField, HideInInspector, Range(0f, 100f)]
		private float m_GlowBlur;
		[SerializeField, HideInInspector, Range(-360f, 360f)]
		private float m_GlowBlurDirection;
		[SerializeField, HideInInspector]
		private Vector2 m_GlowBlurAlongAndCross;
		[SerializeField, HideInInspector]
		private FxColorData m_GlowColor = new FxColorData().SetColor(new Color(1f, 1f, 0f, 0.3f));
		[SerializeField, HideInInspector, Range(-20f, 20f)]
		private float m_GlowBlurFix = 0f;
		[SerializeField, HideInInspector, Range(-10f, 10f)]
		private float m_GlowIntensity = 0.3f;



		public FontConfig[] fonts { get { return m_Fonts ?? new FontConfig[0]; } set { m_Fonts = value; } }

		public int fontSize { get { return m_FontSize; } set { m_FontSize = value; } }

		public FontStyle fontStyle { get { return m_FontStyle; } set { m_FontStyle = value; } }

		public float lineHeight { get { return m_LineHeight; } set { m_LineHeight = value; } }

		public float lineSpace { get { return m_LineSpace; } set { m_LineSpace = value; } }

		public eColorType colorType { get { return m_ColorType; } }

		public Color fontColor { get { return m_FontColor; } }

		public Gradient colorGradient { get { return m_ColorGradient; } }

		public float colorGradientAngle { get { return m_ColorGradientAngle; } }

		public Texture colorTexture { get { return m_ColorTexture; } }

		public Vector2 colorTextureTiling { get { return m_ColorTextureTiling; } }

		public Vector2 colorTextureOffset { get { return m_ColorTextureOffset; } }

		public bool colorTexturePerChar { get { return m_ColorTexturePerChar; } }

		public bool SetFontColor(Color color) {
			if (m_ColorType != eColorType.Color || m_FontColor != color) {
				m_ColorType = eColorType.Color; m_FontColor = color;
				return true;
			}
			return false;
		}

		public bool SetColorGradient(Gradient gradient, float angle, bool perChar) {
			eColorType colorType = perChar ? eColorType.GradientPerChar : eColorType.Gradient;
			if (m_ColorType != colorType || !CheckEqual(m_ColorGradient, gradient) || m_ColorGradientAngle != angle) {
				m_ColorType = colorType; m_ColorGradient = gradient; m_ColorGradientAngle = angle;
				return true;
			}
			return false;
		}

		public bool SetFontTexture(Texture texture, bool perChar) {
			if (!CheckEqual(m_ColorTexture, texture) || m_ColorTextureTiling != Vector2.one || m_ColorTextureOffset != Vector2.zero) {
				m_ColorTexture = texture; m_ColorTextureTiling = Vector2.one; m_ColorTextureOffset = Vector2.zero;
				return true;
			}
			return false;
		}

		public bool SetFontTexture(Texture texture, Vector2 tiling, Vector2 offset, bool perChar) {
			if (!CheckEqual(m_ColorTexture, texture) || m_ColorTextureTiling != tiling || m_ColorTextureOffset != offset) {
				m_ColorTexture = texture; m_ColorTextureTiling = tiling; m_ColorTextureOffset = offset;
				return true;
			}
			return false;
		}

		public float charSpace { get { return m_CharSpace; } set { m_CharSpace = value; } }

		public Vector2 charScale { get { return m_CharScale; } set { m_CharScale = value; } }

		public Vector2 charIncline { get { return m_CharIncline; } set { m_CharIncline = value; } }

		public float textBlend { get { return m_TextBlend; } set { m_TextBlend = value; } }

		public bool hasCharTrapezoid { get { return m_HasCharTrapezoid; } }

		public eAxis charTrapezoidAxis { get { return m_CharTrapezoidAxis; } }

		public float charTrapezoidRatio { get { return m_CharTrapezoidRatio; } }

		public float charTrapezoidConcaveConvex { get { return m_CharTrapezoidConcaveConvex; } }

		public bool SetCharTrapezoid(eAxis axis, float ratio, float concave_convex) {
			if (!m_HasCharTrapezoid || m_CharTrapezoidAxis != axis || m_CharTrapezoidRatio != ratio || m_CharTrapezoidConcaveConvex != concave_convex) {
				m_HasCharTrapezoid = true; m_CharTrapezoidAxis = axis; m_CharTrapezoidRatio = ratio; m_CharTrapezoidConcaveConvex = concave_convex;
			}
			return false;
		}

		public bool ResetCharTrapezoid() {
			if (m_HasCharTrapezoid) {
				m_HasCharTrapezoid = false;
			}
			return false;
		}

		public eSpacialMode spacialMode { get { return m_SpacialMode; } }

		public Vector2 shadowOffset { get { return m_ShadowOffset; } }

		public float shadowExtend { get { return m_ShadowExtend; } }

		public bool shadowAngleBlur { get { return m_ShadowAngleBlur; } }

		public float shadowBlur { get { return m_ShadowBlur; } }

		public float shadowBlurDirection { get { return m_ShadowBlurDirection; } }

		public Vector2 shadowBlurAlongAndCross { get { return m_ShadowBlurAlongAndCross; } }

		public FxColorData shadowColor { get { return m_ShadowColor; } }

		public float shadowIntensity { get { return m_ShadowIntensity; } }

		public Color spacialFaded { get { return m_SpacialFaded; } }

		public Gradient spacialLight360 { get { return m_SpacialLight360; } }

		public float spacialLightPhase { get { return m_SpacialLightPhase; } }

		public float spacialLightSoft { get { return m_SpacialLightSoft; } }

		public Vector2 perspective3dEndPoint { get { return m_Perspective3dEndPoint; } }

		public float perspective3dStretch { get { return m_Perspective3dStretch; } }

		public float orthographic3dDirection { get { return m_Orthographic3dDirection; } }

		public float orthographic3dLength { get { return m_Orthographic3dLength; } }

		public bool SetSpacialShadow(Vector2 offset, float extend, float blur_angle, Vector2 blur, FxColorData color, float intensity) {
			if (m_SpacialMode != eSpacialMode.Shadow || !m_ShadowAngleBlur || m_ShadowOffset != offset || m_ShadowExtend != extend || m_ShadowBlurDirection != blur_angle || m_ShadowBlurAlongAndCross != blur || m_ShadowColor != color || m_ShadowColor.dirty || m_ShadowIntensity != intensity) {
				m_SpacialMode = eSpacialMode.Shadow; m_ShadowAngleBlur = true; m_ShadowOffset = offset; m_ShadowExtend = extend; m_ShadowBlurDirection = blur_angle; m_ShadowBlurAlongAndCross = blur; m_ShadowColor = color; m_ShadowIntensity = intensity;
				return true;
			}
			return false;
		}

		public bool SetSpacialShadow(Vector2 offset, float extend, float blur, FxColorData color, float intensity) {
			if (m_SpacialMode != eSpacialMode.Shadow || m_ShadowAngleBlur || m_ShadowOffset != offset || m_ShadowExtend != extend || m_ShadowBlur != blur || m_ShadowColor != color || m_ShadowColor.dirty || m_ShadowIntensity != intensity) {
				m_SpacialMode = eSpacialMode.Shadow; m_ShadowAngleBlur = false; m_ShadowOffset = offset; m_ShadowExtend = extend; m_ShadowBlur = blur; m_ShadowColor = color; m_ShadowIntensity = intensity;
				return true;
			}
			return false;
		}

		public bool SetSpacialPerspective3D(Vector2 endPoint, float stretch, Color faded, Gradient light360, float lightPhase) {
			if (m_SpacialMode != eSpacialMode.Perspective3D || m_Perspective3dEndPoint != endPoint || m_Perspective3dStretch != stretch || m_SpacialFaded != faded || m_SpacialLight360 != light360 || m_SpacialLightPhase != lightPhase) {
				m_SpacialMode = eSpacialMode.Perspective3D; m_Perspective3dEndPoint = endPoint; m_Perspective3dStretch = stretch; m_SpacialFaded = faded; m_SpacialLight360 = light360; m_SpacialLightPhase = lightPhase;
			}
			return false;
		}

		public bool SetSpacialOrthographic3D(float direction, float length, Color faded, Gradient light360, float lightPhase) {
			if (m_SpacialMode != eSpacialMode.Orthographic3D || m_Orthographic3dDirection != direction || m_Orthographic3dLength != length || m_SpacialFaded != faded || m_SpacialLight360 != light360 || m_SpacialLightPhase != lightPhase) {
				m_SpacialMode = eSpacialMode.Orthographic3D; m_Orthographic3dDirection = direction; m_Orthographic3dLength = length; m_SpacialFaded = faded; m_SpacialLight360 = light360; m_SpacialLightPhase = lightPhase;
			}
			return false;
		}

		public bool ResetSpacial() {
			if (m_SpacialMode != eSpacialMode.None) {
				m_SpacialMode = eSpacialMode.None;
				return true;
			}
			return false;
		}

		public bool hasOutline { get { return m_HasOutline; } }

		public float outlineSize { get { return m_OutlineSize; } }

		public FxColorData outlineColor { get { return m_OutlineColor; } }

		public bool outlineFillText { get { return m_OutlineFillText; } }

		public bool SetOutline(float size, FxColorData color, bool fillText) {
			if (!m_HasOutline || m_OutlineSize != size || m_OutlineColor != color || m_OutlineFillText != fillText || m_OutlineColor.dirty) {
				m_HasOutline = true; m_OutlineSize = size; m_OutlineColor = color; m_OutlineFillText = fillText;
				return true;
			}
			return false;
		}

		public bool ResetOutline() {
			if (m_HasOutline) {
				m_HasOutline = false;
				return true;
			}
			return false;
		}

		public eInnerFxMode innerFxMode { get { return m_InnerFxMode; } }

		public FxColorData innerFxColor { get { return m_InnerFxColor; } }

		public float innerBlurFix { get { return m_InnerBlurFix; } }

		public float innerBlur { get { return m_InnerBlur; } }

		public float innerShadowDirection { get { return m_InnerShadowDirection; } }

		public eInnerFxBlend innerFxBlend { get { return m_InnerFxBlend; } }

		public float innerMultiply { get { return m_InnerMultiply; } }

		public Color embossLightColor { get { return m_EmbossLightColor; } }

		public float embossLightDirection { get { return m_EmbossLightDirection; } }

		public float embossIntensity { get { return m_EmbossIntensity; } }

		public float embossBevel { get { return m_EmbossBevel; } }

		public bool SetInnerAlphaBlend(float blur, FxColorData color, float blur_fix) {
			if (m_InnerFxMode != eInnerFxMode.Light || m_InnerFxBlend != eInnerFxBlend.AlphaBlend || m_InnerBlur != blur || m_InnerFxColor != color || m_InnerFxColor.dirty || m_InnerBlurFix != blur_fix) {
				m_InnerFxMode = eInnerFxMode.Light; m_InnerFxBlend = eInnerFxBlend.AlphaBlend; m_InnerBlur = blur; m_InnerFxColor = color; m_InnerBlurFix = blur_fix;
				return true;
			}
			return false;
		}

		public bool SetInnerShadowAlphaBlend(float shadow_angle, float blur, FxColorData color, float blur_fix) {
			if (m_InnerFxMode != eInnerFxMode.Shadow || m_InnerFxBlend != eInnerFxBlend.AlphaBlend || m_InnerShadowDirection != shadow_angle || m_InnerBlur != blur || m_InnerFxColor != color || m_InnerBlurFix != blur_fix || m_InnerFxColor.dirty) {
				m_InnerFxMode = eInnerFxMode.Shadow; m_InnerFxBlend = eInnerFxBlend.AlphaBlend; m_InnerShadowDirection = shadow_angle; m_InnerBlur = blur; m_InnerFxColor = color; m_InnerBlurFix = blur_fix;
				return true;
			}
			return false;
		}

		public bool SetInnerAdditive(float blur, FxColorData color, float blur_fix, float multiply) {
			if (m_InnerFxMode != eInnerFxMode.Light || m_InnerFxBlend != eInnerFxBlend.Additive || m_InnerBlur != blur || m_InnerFxColor != color || m_InnerFxColor.dirty || m_InnerBlurFix != blur_fix || m_InnerMultiply != multiply) {
				m_InnerFxMode = eInnerFxMode.Light; m_InnerFxBlend = eInnerFxBlend.Additive; m_InnerBlur = blur; m_InnerFxColor = color; m_InnerBlurFix = blur_fix; m_InnerMultiply = multiply;
				return true;
			}
			return false;
		}

		public bool SetInnerShadowAdditive(float shadow_angle, float blur, FxColorData color, float blur_fix, float multiply) {
			if (m_InnerFxMode != eInnerFxMode.Shadow || m_InnerFxBlend != eInnerFxBlend.Additive || m_InnerShadowDirection != shadow_angle || m_InnerBlur != blur || m_InnerFxColor != color || m_InnerFxColor.dirty || m_InnerBlurFix != blur_fix || m_InnerMultiply != multiply) {
				m_InnerFxMode = eInnerFxMode.Shadow; m_InnerFxBlend = eInnerFxBlend.Additive; m_InnerShadowDirection = shadow_angle; m_InnerBlur = blur; m_InnerFxColor = color; m_InnerBlurFix = blur_fix; m_InnerMultiply = multiply;
				return true;
			}
			return false;
		}

		public bool SetInnerEmboss(Color light_color, float light_angle, float intensity, float bevel) {
			if (m_InnerFxMode != eInnerFxMode.Emboss || m_EmbossLightColor != light_color || m_EmbossLightDirection != light_angle || m_EmbossIntensity != intensity || m_EmbossBevel != bevel) {
				m_InnerFxMode = eInnerFxMode.Emboss; m_EmbossLightColor = light_color; m_EmbossLightDirection = light_angle; m_EmbossIntensity = intensity; m_EmbossBevel = bevel;
				return true;
			}
			return false;
		}

		public bool ResetInner() {
			if (m_InnerFxMode != eInnerFxMode.None) {
				m_InnerFxMode = eInnerFxMode.None;
				return true;
			}
			return false;
		}

		public bool hasGlow { get { return m_HasGlow; } }

		public float glowExtend { get { return m_GlowExtend; } }

		public bool glowAngleBlur { get { return m_GlowAngleBlur; } }

		public float glowBlur { get { return m_GlowBlur; } }

		public float glowBlurDirection { get { return m_GlowBlurDirection; } }

		public Vector2 glowBlurAlongAndCross { get { return m_GlowBlurAlongAndCross; } }

		public FxColorData glowColor { get { return m_GlowColor; } }

		public float glowBlurFix { get { return m_GlowBlurFix; } }

		public float glowIntensity { get { return m_GlowIntensity; } }

		public bool SetGlow(float extend, float blur_angle, Vector2 blur, FxColorData color, float blur_fix, float intensity) {
			if (!m_HasGlow || !m_GlowAngleBlur || m_GlowExtend != extend || m_GlowBlurDirection != blur_angle || m_GlowBlurAlongAndCross != blur || m_GlowColor != color || m_GlowColor.dirty || m_GlowBlurFix != blur_fix || m_GlowIntensity != intensity) {
				m_HasGlow = true; m_GlowAngleBlur = true; m_GlowExtend = extend; m_GlowBlurDirection = blur_angle; m_GlowBlurAlongAndCross = blur; m_GlowColor = color; m_GlowBlurFix = blur_fix; m_GlowIntensity = intensity;
				return true;
			}
			return false;
		}

		public bool SetGlow(float extend, float blur, FxColorData color, float blur_fix, float intensity) {
			if (!m_HasGlow || m_GlowAngleBlur || m_GlowExtend != extend || m_GlowBlur != blur || m_GlowColor != color || m_GlowColor.dirty || m_GlowBlurFix != blur_fix || m_GlowIntensity != intensity) {
				m_HasGlow = true; m_GlowAngleBlur = false; m_GlowExtend = extend; m_GlowBlur = blur; m_GlowColor = color; m_GlowBlurFix = blur_fix; m_GlowIntensity = intensity;
				return true;
			}
			return false;
		}

		public bool ResetGlow() {
			if (m_HasGlow) {
				m_HasGlow = false;
				return true;
			}
			return false;
		}

		private static bool CheckEqual(object a, object b) {
			if (a == b) { return true; }
			if (a == null) { return false; }
			return a.Equals(b);
		}

	}

}

using UnityEditor;
using UnityEngine;

namespace GreatClock.Common.UI {

	[CustomEditor(typeof(GreatArtTextStyleAsset)), CanEditMultipleObjects]
	public class GreatArtTextStyleAssetEditor : Editor {

		private GreatArtTextStyleDrawer mDrawer;
		private RenderTexture mPreviewTex;

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
			mDrawer.DrawStyleGUI(this, true, GreatArtTextStyleDrawer.eOperation.None);
			if (serializedObject.ApplyModifiedProperties()) {
				AssetDatabase.SaveAssets();
				UpdatePreviewTexture();
			}
		}

		public override bool HasPreviewGUI() {
			GreatArtTextStyle style = (target as GreatArtTextStyleAsset).style;
			return style != null && style.fonts != null && style.fonts.Length > 0 && style.fonts[0] != null;
		}

		public override void OnPreviewGUI(Rect r, GUIStyle background) {
			base.OnPreviewGUI(r, background);
			if (mPreviewTex == null) { return; }
			Vector2 center = (r.min + r.max) * 0.5f;
			Vector2 size = r.size;
			if (mPreviewTex.width * size.y > size.x * mPreviewTex.height) {
				size.y = mPreviewTex.height * size.x / mPreviewTex.width;
			} else {
				size.x = mPreviewTex.width * size.y / mPreviewTex.height;
			}
			GUI.DrawTexture(new Rect(center - size * 0.5f, size), mPreviewTex);
		}

		void OnEnable() {
			mDrawer = new GreatArtTextStyleDrawer(serializedObject, null);
			UpdatePreviewTexture();
		}

		void OnDisable() {
			if (mPreviewTex != null) { RenderTexture.ReleaseTemporary(mPreviewTex); }
			mPreviewTex = null;
		}

		private void UpdatePreviewTexture() {
			if (mPreviewTex != null) { RenderTexture.ReleaseTemporary(mPreviewTex); }
			mPreviewTex = null;
			GreatArtTextStyle style = (target as GreatArtTextStyleAsset).style;
			TextToRenderTexture.Parameter paras = TextToRenderTexture.Parameter.Default;
			paras.SetLineHeightAndSpace(style.lineHeight, style.lineSpace);
			paras.SetTextTexture(style.colorTexture, style.colorTextureTiling, style.colorTextureOffset, style.colorTexturePerChar);
			switch (style.colorType) {
				case GreatArtTextStyle.eColorType.Color:
					paras.SetTextColor(style.fontColor);
					break;
				case GreatArtTextStyle.eColorType.Gradient:
					paras.SetTextGradient(style.colorGradient, style.colorGradientAngle, false);
					break;
				case GreatArtTextStyle.eColorType.GradientPerChar:
					paras.SetTextGradient(style.colorGradient, style.colorGradientAngle, true);
					break;
			}
			paras.SetCharSpace(style.charSpace * style.fontSize * 0.5f);
			paras.SetScale(style.charScale);
			paras.SetIncline(style.charIncline);
			paras.SetTextBlend(style.textBlend);
			if (style.hasCharTrapezoid && style.charTrapezoidRatio != 0f) {
				if (style.charTrapezoidAxis == GreatArtTextStyle.eAxis.Horizontal) {
					paras.SetCharTrapezoidHorizontal(style.charTrapezoidRatio, style.charTrapezoidConcaveConvex);
				} else {
					paras.SetCharTrapezoidVertical(style.charTrapezoidRatio, style.charTrapezoidConcaveConvex);
				}
			}
			switch (style.spacialMode) {
				case GreatArtTextStyle.eSpacialMode.Shadow:
					if (style.shadowAngleBlur) {
						paras.SetSpacialShadow(style.shadowOffset, style.shadowExtend, style.shadowBlurDirection, style.shadowBlurAlongAndCross, style.shadowColor.ToPara(), style.shadowIntensity);
					} else {
						paras.SetSpacialShadow(style.shadowOffset, style.shadowExtend, style.shadowBlur, style.shadowColor.ToPara(), style.shadowIntensity);
					}
					break;
				case GreatArtTextStyle.eSpacialMode.Perspective3D:
					paras.SetSpacialPerspective3D(style.perspective3dEndPoint, style.perspective3dStretch, style.spacialFaded, style.spacialLight360, style.spacialLightPhase, style.spacialLightSoft);
					break;
				case GreatArtTextStyle.eSpacialMode.Orthographic3D:
					paras.SetSpacialOrthographic3D(style.orthographic3dDirection, style.orthographic3dLength, style.spacialFaded, style.spacialLight360, style.spacialLightPhase, style.spacialLightSoft);
					break;
			}
			if (style.hasOutline) {
				paras.SetOutline(style.outlineSize, style.outlineColor.ToPara(), style.outlineFillText);
			}
			switch (style.innerFxMode) {
				case GreatArtTextStyle.eInnerFxMode.Light:
					if (style.innerFxBlend == GreatArtTextStyle.eInnerFxBlend.AlphaBlend) {
						paras.SetInnerAlphaBlend(style.innerBlur, style.innerFxColor.ToPara(), style.innerBlurFix);
					} else if (style.innerMultiply != 0f) {
						paras.SetInnerAdditive(style.innerBlur, style.innerFxColor.ToPara(), style.innerBlurFix, style.innerMultiply);
					}
					break;
				case GreatArtTextStyle.eInnerFxMode.Shadow:
					if (style.innerFxBlend == GreatArtTextStyle.eInnerFxBlend.AlphaBlend) {
						paras.SetInnerShadowAlphaBlend(style.innerShadowDirection, style.innerBlur, style.innerFxColor.ToPara(), style.innerBlurFix);
					} else if (style.innerMultiply != 0f) {
						paras.SetInnerShadowAdditive(style.innerShadowDirection, style.innerBlur, style.innerFxColor.ToPara(), style.innerBlurFix, style.innerMultiply);
					}
					break;
				case GreatArtTextStyle.eInnerFxMode.Emboss:
					paras.SetInnerEmboss(style.embossLightColor, style.embossLightDirection, style.embossIntensity, style.embossBevel);
					break;
			}
			if (style.hasGlow) {
				if (style.glowAngleBlur) {
					paras.SetGlow(style.glowExtend, style.glowBlurDirection, style.glowBlurAlongAndCross, style.glowColor.ToPara(), style.glowBlurFix, style.glowIntensity);
				} else {
					paras.SetGlow(style.glowExtend, style.glowBlur, style.glowColor.ToPara(), style.glowBlurFix, style.glowIntensity);
				}
			}
			GreatArtTextStyle.FontConfig[] cfgs = style.fonts;
			IFontInternal[] fonts = new IFontInternal[cfgs.Length];
			bool flag = true;
			for (int i = 0; i < cfgs.Length; i++) {
				GreatArtTextStyle.FontConfig cfg = cfgs[i];
				fonts[i] = cfg.fontAsset;
				if (!cfg.useFontAsset) {
					if (cfg.font == null) {
						flag = false;
						break;
					}
					fonts[i] = new FontDynamic(cfg.font);
				}
			}
			if (flag) {
				mPreviewTex = TextToRenderTexture.RenderText(fonts, "S<size=x0.7>AM<voffset=0.14em>P</voffset><rot=-10>L</rot><rot=7>E</rot></size> Text", style.fontSize, style.fontStyle, paras, TextToRenderTexture.eRenderTextureAllocType.Temporary).texture;
			} else {
				if (mPreviewTex != null) {
					RenderTexture.ReleaseTemporary(mPreviewTex);
					mPreviewTex = null;
				}
			}
		}

	}

}

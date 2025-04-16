using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;

namespace GreatClock.Common.UI {

	/// <summary>
	/// The core class of drawing art text texture.
	/// </summary>
	public static class TextToRenderTexture {

		/// <summary>
		/// This struct is used for specifying how the text and FXs are colored.<br />
		/// Pure color, gradient color and texture are supported.
		/// </summary>
		public struct FxColor {

			public eFxColorType _type;
			public Color _color;
			public Gradient _gradient;
			public float _gradient_angle;
			public Texture _texture;
			public Vector4 _tiling_offset;

			/// <summary>
			/// Set the texture used for coloring
			/// </summary>
			/// <param name="tex">Texture for coloring fx.</param>
			/// <returns>The FxColor object itself.</returns>
			public FxColor SetTexture(Texture tex) {
				_texture = tex;
				_tiling_offset = GetVector4(Vector2.one, Vector2.zero);
				return this;
			}

			/// <summary>
			/// Set the texture used for coloring fx with its tiling and offset.
			/// </summary>
			/// <param name="tex">Texture for coloring fx.</param>
			/// <param name="tiling">the placement scale of the texture</param>
			/// <param name="offset">the placement offset of the texture</param>
			/// <returns>The FxColor object itself.</returns>
			public FxColor SetTexture(Texture tex, Vector2 tiling, Vector2 offset) {
				_texture = tex;
				_tiling_offset = GetVector4(tiling, offset);
				return this;
			}

			/// <summary>
			/// Create a color data object with a pure color.
			/// </summary>
			/// <param name="color">pure color value</param>
			/// <returns>the FXColor object</returns>
			public static FxColor Color(Color color) {
				return new FxColor() {
					_type = eFxColorType.Color,
					_tiling_offset = GetVector4(Vector2.one, Vector2.zero),
					_color = color
				};
			}

			/// <summary>
			/// Create a color data object with a gradient color.
			/// </summary>
			/// <param name="gradient">the Unity gradient object</param>
			/// <param name="angle">the direction that gradient go along, 0: left-right, 90: bottom-top</param>
			/// <returns>the FXColor object</returns>
			public static FxColor Gradient(Gradient gradient, float angle) {
				return new FxColor() {
					_type = eFxColorType.Gradient,
					_tiling_offset = GetVector4(Vector2.one, Vector2.zero),
					_gradient = gradient,
					_gradient_angle = angle
				};
			}

		}

		/// <summary>
		/// Data struct that defining all art text FXs.
		/// </summary>
		public struct Parameter {

			public float _line_height;
			public float _line_space;
			public eTextColorType _color_type;
			public Color _color;
			public Gradient _gradient;
			public float _gradient_angle;
			public Texture _texture;
			public Vector4 _texture_tiling_offset;
			public bool _texture_per_char;
			public float _char_space;
			public Vector2 _incline;
			public float _text_blend;
			public Vector2 _scale;
			public int _char_trapezoid;
			public float _char_trapezoid_ratio;
			public float _char_trapezoid_concave_convex;
			public float _outline;
			public FxColor _outline_color;
			public float _outline_fill_text;
			public Vector2 _shadow_offset;
			public float _shadow_extend;
			public float _shadow_blur_angle;
			public Vector2 _shadow_blur;
			public FxColor _shadow_color;
			public float _shadow_intensity;
			public Vector2 _3d_endpoint;
			public float _3d_length;
			public Color _3d_faded;
			public Gradient _3d_light;
			public float _3d_light_phase;
			public float _3d_light_soft;
			public float _inner_blur;
			public float _inner_blur_angle;
			public FxColor _inner_color;
			public float _inner_blur_fix;
			public float _inner_multiply;
			public Color _emboss_light;
			public float _glow_extend;
			public float _glow_blur_angle;
			public Vector2 _glow_blur;
			public FxColor _glow_color;
			public float _glow_blur_fix;
			public float _glow_intensity;

			/// <summary>
			/// Set line height (new line vertical offset)<br />
			/// Final line height = height * fontSize + space
			/// </summary>
			/// <param name="height">Font-size-relative part of final line height. 1 by default which means 1 multiplies font size.</param>
			/// <param name="space">Increment pixels part of final line height.</param>
			/// <returns>The parameter object itself.</returns>
			public Parameter SetLineHeightAndSpace(float height, float space) {
				_line_height = height;
				_line_space = space;
				return this;
			}

			/// <summary>
			/// Specify a pure color for coloring text.
			/// </summary>
			/// <param name="color">Font color value.</param>
			/// <returns>The parameter object itself.</returns>
			public Parameter SetTextColor(Color color) {
				_color_type = eTextColorType.Color;
				_color = color;
				return this;
			}

			/// <summary>
			/// Specify a gradient color and its direction for coloring text.
			/// </summary>
			/// <param name="gradient">How colors changed.</param>
			/// <param name="angle">The direction that gradient goes along with.</param>
			/// <param name="perChar">True for gradient in every single char, false for gradient in whole text.</param>
			/// <returns>The parameter object itself.</returns>
			public Parameter SetTextGradient(Gradient gradient, float angle, bool perChar) {
				_color_type = perChar ? eTextColorType.GradientPerChar : eTextColorType.Gradient;
				_gradient = gradient;
				_gradient_angle = angle;
				return this;
			}

			/// <summary>
			/// Specify the texture for coloring text.
			/// </summary>
			/// <param name="tex">Texture for coloring text.</param>
			/// <param name="texPerChar">True when the texture is applied for individual characters.</param>
			/// <returns>The parameter object itself.</returns>
			public Parameter SetTextTexture(Texture tex, bool texPerChar) {
				_texture = tex;
				_texture_tiling_offset = GetVector4(Vector2.one, Vector2.zero);
				_texture_per_char = texPerChar;
				return this;
			}

			/// <summary>
			/// Specify the texture togegher with tiling and offset for coloring text.
			/// </summary>
			/// <param name="tex">Texture for coloring text.</param>
			/// <param name="tiling">The placement scale of the texture.</param>
			/// <param name="offset">The placement offset of the texture.</param>
			/// <param name="texPerChar">True when the texture is applied for individual characters.</param>
			/// <returns>The parameter object itself.</returns>
			public Parameter SetTextTexture(Texture tex, Vector2 tiling, Vector2 offset, bool texPerChar) {
				_texture = tex;
				_texture_tiling_offset = GetVector4(tiling, offset);
				_texture_per_char = texPerChar;
				return this;
			}

			/// <summary>
			/// Set the space between sequential characters in pixel.
			/// </summary>
			/// <param name="space">The character space value.</param>
			/// <returns>The parameter object itself.</returns>
			public Parameter SetCharSpace(float space) {
				_char_space = space;
				return this;
			}

			/// <summary>
			/// Set incline transform to make chars outer rect transformed into parallelogram.
			/// </summary>
			/// <param name="incline">incline.x: The horizontal offset between bottom and top edges.<br />incline.y: The vertical offset between left and right edges.</param>
			/// <returns>The parameter object itself.</returns>
			public Parameter SetIncline(Vector2 incline) {
				_incline = incline;
				return this;
			}

			/// <summary>
			/// Set opaque value of original colored text.
			/// </summary>
			/// <param name="blend">Opaque value when blending.</param>
			/// <returns>The parameter object itself.</returns>
			public Parameter SetTextBlend(float blend) {
				_text_blend = blend;
				return this;
			}

			/// <summary>
			/// Set character default scale along x and y axis.
			/// </summary>
			/// <param name="scale">Scale value.</param>
			/// <returns>The parameter object itself.</returns>
			public Parameter SetScale(Vector2 scale) {
				_scale = scale;
				return this;
			}

			/// <summary>
			/// Set trapezoid transform of which unparallel edges are horizontal edges.
			/// </summary>
			/// <param name="ratio">Length ratio of parallel edges.</param>
			/// <param name="concave_convex">The unparallel edges will not be straight if this value is not 0, the two edges will be curved into concave or convex.</param>
			/// <returns>The parameter object itself.</returns>
			public Parameter SetCharTrapezoidHorizontal(float ratio, float concave_convex) {
				_char_trapezoid = -1;
				_char_trapezoid_ratio = ratio;
				_char_trapezoid_concave_convex = concave_convex;
				return this;
			}

			/// <summary>
			/// Set trapezoid transform of which unparallel edges are vertical edges.
			/// </summary>
			/// <param name="ratio">Length ratio of parallel edges.</param>
			/// <param name="concave_convex">The unparallel edges will not be straight if this value is not 0, the two edges will be curved into concave or convex.</param>
			/// <returns>The parameter object itself.</returns>
			public Parameter SetCharTrapezoidVertical(float ratio, float concave_convex) {
				_char_trapezoid = 1;
				_char_trapezoid_ratio = ratio;
				_char_trapezoid_concave_convex = concave_convex;
				return this;
			}

			/// <summary>
			/// Set offline FX of text.
			/// </summary>
			/// <param name="size">The size of outline in pixels.</param>
			/// <param name="color">How the outline is colored.</param>
			/// <param name="fill_text">Outline FX will fill original text area if true, culled if false.<br />When 0 is passed into 'SetTextBlend' method, outline area will be shown as an extend of original text area.</param>
			/// <returns>The parameter object itself.</returns>
			public Parameter SetOutline(float size, FxColor color, bool fill_text) {
				if (size <= 0) { return this; }
				_outline = size;
				_outline_color = color;
				_outline_fill_text = fill_text ? 1f : 0f;
				return this;
			}

			/// <summary>
			/// Clear and turn off outline FX.
			/// </summary>
			/// <returns>The parameter object itself.</returns>
			public Parameter ResetOutline() {
				_outline = 0f;
				return this;
			}

			/// <summary>
			/// Set directional shadow FX.
			/// </summary>
			/// <param name="offset">Placement offset between shadow and original text.</param>
			/// <param name="extend">Region extend in pixels from original text area before calculating shadow blur.</param>
			/// <param name="blur_angle">Blur direction in degree.</param>
			/// <param name="blur">Blur size in pixels along blur direction and its perpendicular direction.</param>
			/// <param name="color">How the shadow is colored.</param>
			/// <param name="intensity">The value multiplied to colored shadow blur results before added to final texture.</param>
			/// <returns>The parameter object itself.</returns>
			public Parameter SetSpacialShadow(Vector2 offset, float extend, float blur_angle, Vector2 blur, FxColor color, float intensity) {
				_shadow_offset = offset;
				_shadow_extend = extend;
				_shadow_blur_angle = blur_angle;
				_shadow_blur = blur;
				_shadow_color = color;
				_shadow_intensity = Mathf.Max(0f, intensity);
				return this;
			}

			/// <summary>
			/// Set shadow FX.
			/// </summary>
			/// <param name="offset">Placement offset between shadow and original text.</param>
			/// <param name="extend">Region extend in pixels from original text area before calculating shadow blur.</param>
			/// <param name="blur">Shadow blur size in pixels.</param>
			/// <param name="color">How the shadow is colored.</param>
			/// <param name="intensity">The value multiplied to colored shadow blur results before added to final texture.</param>
			/// <returns>The parameter object itself.</returns>
			public Parameter SetSpacialShadow(Vector2 offset, float extend, float blur, FxColor color, float intensity) {
				_shadow_offset = offset;
				_shadow_extend = extend;
				_shadow_blur_angle = 0f;
				_shadow_blur = new Vector2(blur, blur);
				_shadow_color = color;
				_shadow_intensity = Mathf.Max(0f, intensity);
				return this;
			}

			/// <summary>
			/// Set Perspective 3D Text
			/// </summary>
			/// <param name="endpoint">The point that the 3D text disappears in perspective view.</param>
			/// <param name="stretch">The depth of 3D text, linear scale from 0-depth to full infinit-depth.</param>
			/// <param name="faded">How color faded along depth. RGB: the distant color, Alpha: the value that RGB blends.</param>
			/// <param name="light360">The circle light that intowards lit the 3D text. RGB: light color, Alpha: darken or enlighten the origin text color.</param>
			/// <param name="phase">The circle light angle offset.</param>
			/// <param name="lightSoft">Text edge smoothness when calculating lights.</param>
			/// <returns>The parameter object itself.</returns>
			public Parameter SetSpacialPerspective3D(Vector2 endpoint, float stretch, Color faded, Gradient light360, float phase, float lightSoft) {
				_shadow_intensity = -1f;
				_3d_endpoint = endpoint;
				_3d_length = stretch;
				_3d_faded = faded;
				_3d_light = light360;
				_3d_light_phase = phase;
				_3d_light_soft = lightSoft;
				return this;
			}

			/// <summary>
			/// Set Orthographic 3D Text
			/// </summary>
			/// <param name="dir">The direction in degree that text extends in depth.</param>
			/// <param name="length">The depth extended size in pixels.</param>
			/// <param name="faded">How color faded along depth. RGB: the distant color, Alpha: the value that RGB blends.</param>
			/// <param name="light360">The circle light that intowards lit the 3D text. RGB: light color, Alpha: darken or enlighten from origin text color.</param>
			/// <param name="phase">The circle light angle offset.</param>
			/// <param name="lightSoft">Text edge smoothness when calculating lights.</param>
			/// <returns>The parameter object itself.</returns>
			public Parameter SetSpacialOrthographic3D(float dir, float length, Color faded, Gradient light360, float phase, float lightSoft) {
				_shadow_intensity = -2f;
				_3d_endpoint = new Vector2(dir, dir);
				_3d_length = length;
				_3d_faded = faded;
				_3d_light = light360;
				_3d_light_phase = phase;
				_3d_light_soft = lightSoft;
				return this;
			}

			/// <summary>
			/// Clear and turn off Shadow, Perspective3D or Orthographic3D FX.
			/// </summary>
			/// <returns>The parameter object itself.</returns>
			public Parameter ResetSpacial() {
				_shadow_intensity = 0f;
				return this;
			}

			/// <summary>
			/// Set blur FX inwards from text edge using additive blend.
			/// </summary>
			/// <param name="blur">Blur size in pixels inwards from text edge.</param>
			/// <param name="color">How the inner FX is colored.</param>
			/// <param name="blur_fix">Curve fix for blur result to strengthen or weaken FX color.</param>
			/// <returns>The parameter object itself.</returns>
			public Parameter SetInnerAlphaBlend(float blur, FxColor color, float blur_fix) {
				if (blur <= 0f) {
					_inner_blur_angle = 0f;
					return this;
				}
				_inner_blur = blur;
				_inner_blur_angle = 720f;
				_inner_color = color;
				_inner_blur_fix = blur_fix;
				_inner_multiply = 0f;
				return this;
			}

			/// <summary>
			/// Set shadow in original text area using alpha blend.<br />
			/// This FX is similar to inner directional blur, but blur goes in just one side.
			/// </summary>
			/// <param name="blur_angle">The direction that shodow blur goes along.</param>
			/// <param name="blur">The blur size of the shadow in pixels inwards from text edge.</param>
			/// <param name="color">How the inner shadow is colored.</param>
			/// <param name="blur_fix">Curve fix for blur result to strengthen or weaken FX color.</param>
			/// <returns>The parameter object itself.</returns>
			public Parameter SetInnerShadowAlphaBlend(float blur_angle, float blur, FxColor color, float blur_fix) {
				if (blur <= 0f) {
					_inner_blur_angle = 0f;
					return this;
				}
				blur_angle %= 360f;
				if (blur_angle >= 0f) { blur_angle -= 360f; }
				_inner_blur = blur;
				_inner_blur_angle = blur_angle;
				_inner_color = color;
				_inner_blur_fix = blur_fix;
				_inner_multiply = 0f;
				return this;
			}

			/// <summary>
			/// Set blur FX inwards from text edge using additive blend.
			/// </summary>
			/// <param name="blur">Blur size in pixels inwards from text edge.</param>
			/// <param name="color">How the inner FX is colored.</param>
			/// <param name="blur_fix">Curve fix for blur result to strengthen or weaken FX color.</param>
			/// <param name="multiply">Value multiplied to colored blur result before additive blend. The value may be negative.</param>
			/// <returns>The parameter object itself.</returns>
			public Parameter SetInnerAdditive(float blur, FxColor color, float blur_fix, float multiply) {
				if (blur <= 0f) {
					_inner_blur_angle = 0f;
					return this;
				}
				_inner_blur = blur;
				_inner_blur_angle = 720f;
				_inner_color = color;
				_inner_blur_fix = blur_fix;
				_inner_multiply = multiply;
				return this;
			}

			/// <summary>
			/// Set shadow in original text area using additive blend.<br />
			/// This FX is similar to inner directional blur, but blur goes in just one side.
			/// </summary>
			/// <param name="blur_angle">The direction in degree that shodow blur goes along.</param>
			/// <param name="blur">The blur size of the shadow in pixels inwards from text edge.</param>
			/// <param name="color">How the inner shadow is colored.</param>
			/// <param name="blur_fix">Curve fix for blur result to strengthen or weaken FX color.</param>
			/// <param name="multiply">Value multiplied to colored blur result before additive blend, may be negative.</param>
			/// <returns>The parameter object itself.</returns>
			public Parameter SetInnerShadowAdditive(float blur_angle, float blur, FxColor color, float blur_fix, float multiply) {
				if (blur <= 0f) {
					_inner_blur_angle = 0f;
					return this;
				}
				blur_angle %= 360f;
				if (blur_angle >= 0f) { blur_angle -= 360f; }
				_inner_blur = blur;
				_inner_blur_angle = blur_angle;
				_inner_color = color;
				_inner_blur_fix = blur_fix;
				_inner_multiply = multiply;
				return this;
			}

			/// <summary>
			/// Set Emboss FX in original text area.
			/// </summary>
			/// <param name="light_angle">Where the light comes from in degree.</param>
			/// <param name="light_intensity">The intensity of shines and shadows.</param>
			/// <param name="bevel">The bevel size in pixel inwards from text edge.</param>
			/// <returns>The parameter object itself.</returns>
			public Parameter SetInnerEmboss(Color light_color, float light_angle, float light_intensity, float bevel) {
				if (bevel <= 0f || light_intensity == 0f) {
					_inner_blur_angle = 0f;
				}
				light_angle %= 360f;
				if (light_angle <= 0f) { light_angle += 360f; }
				_emboss_light = light_color;
				_inner_blur_angle = light_angle;
				_inner_blur = bevel;
				_inner_multiply = light_intensity;
				return this;
			}

			/// <summary>
			/// Clear and turn off inner FX.
			/// </summary>
			/// <returns>The parameter object itself.</returns>
			public Parameter ResetInner() {
				_inner_blur_angle = 0f;
				return this;
			}

			/// <summary>
			/// Set glow FX by using single-direction blur.<br />
			/// Glow FX works on text and all previous FXs.
			/// </summary>
			/// <param name="extend">Region extend in pixels from original text area before calculating blur.</param>
			/// <param name="blur_angle">Blur direction in degree.</param>
			/// <param name="blur">Blur size in pixels along blur direction and its perpendicular direction.</param>
			/// <param name="color">How the glow FX is colored.</param>
			/// <param name="blur_fix">Curve fix for blur result to strengthen or weaken FX color.</param>
			/// <param name="intensity">The value multiplied to colored blur results before added to final texture.</param>
			/// <returns>The parameter object itself.</returns>
			public Parameter SetGlow(float extend, float blur_angle, Vector2 blur, FxColor color, float blur_fix, float intensity) {
				_glow_extend = extend;
				_glow_blur_angle = blur_angle;
				_glow_blur = blur;
				_glow_color = color;
				_glow_blur_fix = blur_fix;
				_glow_intensity = intensity;
				return this;
			}

			/// <summary>
			/// Set glow FX by using all-direction blur.<br />
			/// Glow FX works on text and all previous FXs.
			/// </summary>
			/// <param name="extend">Region extend in pixels from original text area before calculating blur.</param>
			/// <param name="blur">Blur size in pixels when calculating glow.</param>
			/// <param name="color">How the glow FX is colored.</param>
			/// <param name="blur_fix">Curve fix for blur result to strengthen or weaken FX color.</param>
			/// <param name="intensity">The value multiplied to colored blur results before added to final texture.</param>
			/// <returns>The parameter object itself.</returns>
			public Parameter SetGlow(float extend, float blur, FxColor color, float blur_fix, float intensity) {
				_glow_extend = extend;
				_glow_blur_angle = 0f;
				_glow_blur = new Vector2(blur, blur);
				_glow_color = color;
				_glow_blur_fix = blur_fix;
				_glow_intensity = intensity;
				return this;
			}

			/// <summary>
			/// Clear and turn off glow FX.
			/// </summary>
			/// <returns>The parameter object itself.</returns>
			public Parameter ResetGlow() {
				_glow_intensity = 0f;
				return this;
			}

			/// <summary>
			/// Get data object that defining default art text parameters.
			/// </summary>
			public static Parameter Default {
				get {
					Parameter para = new Parameter();
					para._line_height = 1f;
					para._line_space = 0f;
					para._color_type = eTextColorType.Color;
					para._color = Color.white;
					para._char_space = 0f;
					para._incline = Vector2.zero;
					para._text_blend = 1f;
					para._scale = Vector2.one;
					para._char_trapezoid = 0;
					para._outline = 0f;
					para._shadow_intensity = 0f;
					para._inner_blur_angle = 0f;
					para._emboss_light = Color.white;
					para._glow_intensity = 0f;
					return para;
				}
			}

		}

		/// <summary>
		/// This struct is used for specifing how RenderTexture is allocated.
		/// </summary>
		public enum eRenderTextureAllocType {
			/// <summary>
			/// RenderTexture instance is created via 'new RenderTexture()'.<br />
			/// The instance should be released via 'UnityEngine.Object.Destroy()'.'
			/// </summary>
			Alloc = 0,
			/// <summary>
			/// RenderTexture instance is created via 'RenderTexture.GetTemporary()'.<br />
			/// The instance should be released via 'RenderTexture.ReleaseTemporary()'.
			/// </summary>
			Temporary = 1
		}

		/// <summary>
		/// Data type that 'RenderText' method returns.
		/// </summary>
		public struct ArtTextTexture {
			/// <summary>
			/// Texture containing art text.
			/// </summary>
			public RenderTexture texture;
			/// <summary>
			/// Zero point of text in texture coordinate (lower-left based).
			/// </summary>
			public Vector2 origin;
			/// <summary>
			/// Text rect area in texture coordinate (lower-left based).
			/// </summary>
			public Rect rect;
		}

		/// <summary>
		/// Draw text and its FXs into a RenderTexture.
		/// </summary>
		/// <param name="font">Dynamic font asset, should not be null.</param>
		/// <param name="content">Text content, rich text is supported.</param>
		/// <param name="fontSize">Default font size.</param>
		/// <param name="style">Specify font style, bold or not and italic or not.</param>
		/// <param name="paras">All parameters of art text FXs.</param>
		/// <param name="allocType">Specify how returned rendertexture is created.</param>
		/// <returns>Art text texture together with zero point position and text rect.</returns>
		public static ArtTextTexture RenderText(Font font, string content, int fontSize, FontStyle style, Parameter paras, eRenderTextureAllocType allocType) {
			temp_fonts[0] = new FontDynamic(font);
			return RenderText(temp_fonts, content, fontSize, style, paras, allocType);
		}

		/// <summary>
		/// Draw text and its FXs into a RenderTexture.
		/// </summary>
		/// <param name="fonts">Dynamic font array. Size of the array must be greater then 0, and all fonts used by content should not be null.</param>
		/// <param name="content">Text content, rich text is supported.</param>
		/// <param name="fontSize">Default font size.</param>
		/// <param name="style">Specify font style, bold or not and italic or not.</param>
		/// <param name="paras">All parameters of art text FXs.</param>
		/// <param name="allocType">Specify how returned rendertexture is created.</param>
		/// <returns>Art text texture together with zero point position and text rect.</returns>
		public static ArtTextTexture RenderText(Font[] fonts, string content, int fontSize, FontStyle style, Parameter paras, eRenderTextureAllocType allocType) {
			if (string.IsNullOrEmpty(content) || fonts == null) { return new ArtTextTexture() { texture = null }; }
			for (int i = fonts.Length - 1; i >= 0; i--) {
				temp_fonts[i] = new FontDynamic(fonts[i]);
			}
			return RenderText(temp_fonts, content, fontSize, style, paras, allocType);
		}

		/// <summary>
		/// Draw text and its FXs into a RenderTexture.
		/// </summary>
		/// <param name="fonts">Custom  font array. Size of the array must be greater then 0, and all fonts used my content should not be null.</param>
		/// <param name="content">Text content, rich text is supported.</param>
		/// <param name="fontSize">Default font size.</param>
		/// <param name="style">Specify font style, bold or not and italic or not.</param>
		/// <param name="paras">All parameters of art text FXs.</param>
		/// <param name="allocType">Specify how returned rendertexture is created.</param>
		/// <returns>Art text texture together with zero point position and text rect.</returns>
		public static ArtTextTexture RenderText(IFontInternal[] fonts, string content, int fontSize, FontStyle style, Parameter paras, eRenderTextureAllocType allocType) {
			if (string.IsNullOrEmpty(content) || fonts == null) { return new ArtTextTexture() { texture = null }; }
			s_chars.Clear();
			GetCharInfos(content, fontSize, style, paras._char_space, paras._incline, paras._scale, 0f, s_chars);
			float lineHeight = paras._line_height * fontSize + paras._line_space;
			s_temp_invalids.Clear();
			Mesh mesh;
			IFontInternal[] fnts;
			TryGetTextMesh(fonts, lineHeight, s_chars, paras._color_type, paras._gradient_angle, s_temp_invalids, out mesh, out fnts);
			if (s_temp_invalids.Count > 0) {
				RequestCharsFromFont(fonts, s_temp_invalids);
				s_temp_invalids.Clear();
				TryGetTextMesh(fonts, lineHeight, s_chars, paras._color_type, paras._gradient_angle, s_temp_invalids, out mesh, out fnts);
			}
			if (mesh == null) { return new ArtTextTexture() { texture = null }; }
			Vector3 origin = -mesh.bounds.min;
			Rect rect;
			RenderTexture tex = DrawTextMesh(fnts, mesh, paras, allocType, out rect);
			for (int i = mesh.subMeshCount - 1; i >= 0; i--) {
				s_submesh_fonts[i] = null;
			}
			origin.x += rect.x;
			origin.y += rect.y;
			for (int i = fonts.Length - 1; i >= 0; i--) {
				IFontInternal font = fonts[i];
				if (font != null) { font.Reset(); }
			}
			return new ArtTextTexture() { texture = tex, origin = new Vector2(origin.x, origin.y), rect = rect };
		}

		private static IFontInternal[] temp_fonts = new IFontInternal[100];
		private static List<CharInfo> s_chars = new List<CharInfo>(128);

		public enum eTextColorType { Color, Gradient, GradientPerChar }

		public enum eFxColorType { Color, Gradient }

		private enum eCharColorOverride { None = 0, Override = 1, Multiply = 2, Add = 3, Minus = 4 }

		#region char analysis

		private struct CharInfo {
			public int font;
			public char chr;
			public int size;
			public FontStyle style;
			public eCharColorOverride colorOverrided;
			public Color color1;
			public Color color2;
			public float gradientAngle;
			public float rotation;
			public Vector2 offset;
			public float voffset;
			public Vector2 border;
			public float cspace;
			public Vector2 incline;
			public Vector2 scale;
			public int locate;
			public Vector2 pos;
		}

		private static Stack<CharInfo> s_temp_char_info = new Stack<CharInfo>();
		private static Stack<StringSegment> s_temp_tags = new Stack<StringSegment>();

		private static void GetCharInfos(string content, int defaultSize, FontStyle defaultStyle, float defaultCharSpace,
			Vector2 defaultIncline, Vector2 defaultScale, float defaultRotation, List<CharInfo> infos) {
			s_temp_char_info.Clear();
			CharInfo current = new CharInfo();
			current.font = 0;
			current.size = defaultSize;
			current.style = defaultStyle;
			current.colorOverrided = eCharColorOverride.None;
			current.color1 = Color.white;
			current.gradientAngle = -1f;
			current.rotation = defaultRotation;
			current.offset = Vector2.zero;
			current.voffset = 0f;
			current.border = Vector2.zero;
			current.cspace = defaultCharSpace;
			current.incline = defaultIncline;
			current.scale = defaultScale;
			current.locate = 0;
			current.pos = Vector2.zero;

			CharInfo prevCharInfo = current;

			int index = 0;
			int length = content.Length;
			int tag_start = 0;
			int type = 0; // 1:tag 2:tag_end, 3:value_ready_read 4:read_end_tag 5:read_end
			int block_start = 0;

			s_temp_tags.Clear();
			StringSegment currentTag = StringSegment.Empty;

			while (index < length) {
				int pi = index;
				int si;
				int t = ReadWord(content, ref index, out si);
				int len = index - si;
				switch (type) {
					case 1:
						if (t == 1) {
							type = 2;
							// Debug.LogWarning("tag:" + content.Substring(si, index - si));
							currentTag = StringSegment.Get(content, si, index - si);
						} else if (len == 1 && content[si] == '/') {
							type = 4;
							// Debug.LogWarning("ready end tag");
						} else {
							// Debug.LogErrorFormat("'/' or tag expected at {0}!", si);
							type = 0;
							// Debug.LogFormat("UNnormal content : '{0}'", content.Substring(tag_start, index - tag_start));
							prevCharInfo = AppendChars(content, tag_start, index, current, infos);
							block_start = index;
						}
						break;
					case 2:
						char chr = len == 1 ? content[si] : ' ';
						if (chr == '=') {
							type = 3;
							block_start = index;
						} else if (chr == '>') {
							type = 0;
							block_start = index;
							bool pair;
							CharInfo charinfo = UpdateCharStyle(ref prevCharInfo, current, defaultSize, currentTag, StringSegment.Empty, out pair);
							if (pair) {
								s_temp_tags.Push(currentTag);
								s_temp_char_info.Push(current);
								current = charinfo;
							} else if (infos.Count > 0) {
								infos[infos.Count - 1] = prevCharInfo;
							}
						} else {
							// Debug.LogErrorFormat("'>' or '=' expected at {0}!", si);
							type = 0;
							index--;
							// Debug.LogFormat("UNnormal content : '{0}'", content.Substring(tag_start, index - tag_start));
							prevCharInfo = AppendChars(content, tag_start, index, current, infos);
							block_start = index;
						}
						break;
					case 3:
						if (len == 1 && content[si] == '>') {
							// Debug.LogWarningFormat("para:{0}", content.Substring(block_start, index - 1 - block_start));
							type = 0;
							bool pair;
							CharInfo charinfo = UpdateCharStyle(ref prevCharInfo, current, defaultSize, currentTag, StringSegment.Get(content, block_start, index - 1 - block_start), out pair);
							if (pair) {
								s_temp_tags.Push(currentTag);
								s_temp_char_info.Push(current);
								current = charinfo;
							} else if (infos.Count > 0) {
								infos[infos.Count - 1] = prevCharInfo;
							}
							block_start = index;
						}
						break;
					case 4:
						if (t == 1) {
							type = 5;
							// Debug.LogWarning("tag end : " + content.Substring(si, index - si));
							if (s_temp_tags.Count > 0 && s_temp_tags.Peek().Equals(content, si, index - si)) {
								s_temp_tags.Pop();
								current = s_temp_char_info.Pop();
							} else {
								type = 0;
								// Debug.LogFormat("UNnormal content : '{0}'", content.Substring(tag_start, index - tag_start));
								prevCharInfo = AppendChars(content, tag_start, index, current, infos);
								block_start = index;
							}
						} else {
							// Debug.LogErrorFormat("tag name expected at {0}!", si);
							type = 0;
							// Debug.LogFormat("UNnormal content : '{0}'", content.Substring(tag_start, index - tag_start));
							prevCharInfo = AppendChars(content, tag_start, index, current, infos);
							block_start = index;
						}
						break;
					case 5:
						if (len == 1 && content[si] == '>') {
							// Debug.LogWarningFormat("para:{0}", content.Substring(block_start, index - block_start));
							type = 0;
							block_start = index;
						} else {
							// Debug.LogErrorFormat("'>' expected at {0}!", si);
							type = 0;
							block_start = index;
							// Debug.LogFormat("UNnormal content : '{0}'", content.Substring(tag_start, index - tag_start));
							prevCharInfo = AppendChars(content, tag_start, index, current, infos);
						}
						break;
					default:
						if (len == 1 && content[si] == '<') {
							type = 1;
							tag_start = pi;
							// Debug.LogFormat("normal content : '{0}'", content.Substring(block_start, index - 1 - block_start));
							prevCharInfo = AppendChars(content, block_start, index - 1, current, infos);
						}
						break;


				}
				// Debug.LogFormat("\"{0}\" - \"{1}\"", content.Substring(pi, index - pi), content.Substring(si, index - si));
			}
			if (type == 0) {
				if (index > block_start) {
					// Debug.LogFormat("normal content : '{0}'", content.Substring(block_start, index - block_start));
					AppendChars(content, block_start, index, current, infos);
				}
			} else {
				if (index > tag_start) {
					// Debug.LogFormat("UNnormal content : '{0}'", content.Substring(tag_start, index - tag_start));
					AppendChars(content, tag_start, index, current, infos);
				}
			}
		}

		private static CharInfo AppendChars(string content, int start, int end, CharInfo info, List<CharInfo> infos) {
			if (start >= end && infos.Count > 0) { return infos[infos.Count - 1]; }
			CharInfo last = info;
			for (int i = start; i < end; i++) {
				info.chr = content[i];
				if (info.chr == '\r') { continue; }
				infos.Add(info);
				last = info;
			}
			return last;
		}

		private static List<StringSegment> s_temp_splits_1 = new List<StringSegment>(8);
		private static List<StringSegment> s_temp_splits_2 = new List<StringSegment>(8);
		private static List<StringSegment> s_temp_splits_3 = new List<StringSegment>(8);

		private static CharInfo UpdateCharStyle(ref CharInfo pi, CharInfo info, int fontsize, StringSegment tag, StringSegment para, out bool pair) {
			pair = true;
			switch (tag.ToString()) {
				case "font":
					int fi;
					if (int.TryParse(para.ToString(), out fi) && fi >= 0) {
						info.font = fi;
					} else {
						// Debug.LogError("Font index should be a positive integer value !");
					}
					break;
				case "size":
					switch (para[0]) {
						case '+':
						case '-':
							int sign = para[0] == '+' ? 1 : -1;
							if (para[para.length - 1] == '%') {
								float percentage;
								if (float.TryParse(para.Substring(1, para.length - 2), out percentage)) {
									info.size = info.size + sign * Mathf.RoundToInt(info.size * percentage * 0.01f);
								}
							} else {
								int delta;
								if (int.TryParse(para.ToString(), out delta)) {
									info.size += delta;
								}
							}
							break;
						case '*':
						case 'x':
							float ratio;
							if (float.TryParse(para.Substring(1, para.length - 1), out ratio)) {
								info.size = Mathf.RoundToInt(info.size * ratio);
							}
							break;
						default:
							int size;
							if (int.TryParse(para.ToString(), out size)) {
								info.size = size;
							} else {
								// Debug.LogError("Font size should be an integer value !");
							}
							break;
					}
					break;
				case "color":
					Color color;
					StringSegment colorSeg = StringSegment.Get(para.content, para.index + 1, para.length - 1);
					switch (para[0]) {
						case '*':
						case 'x':
							info.colorOverrided = eCharColorOverride.Multiply;
							break;
						case '+':
							info.colorOverrided = eCharColorOverride.Add;
							break;
						case '-':
							info.colorOverrided = eCharColorOverride.Minus;
							break;
						default:
							colorSeg = para;
							info.colorOverrided = eCharColorOverride.Override;
							break;
					}
					if (ParseColor(colorSeg, out color)) {
						info.color1 = color;
						info.gradientAngle = -1;
					} else {
						info.colorOverrided = eCharColorOverride.None;
						// Debug.LogError("Invalid color parameter : " + para);
					}
					break;
				case "gradient":
					StringSegment gradientSeg = StringSegment.Get(para.content, para.index + 1, para.length - 1);
					switch (para[0]) {
						case '*':
						case 'x':
							info.colorOverrided = eCharColorOverride.Multiply;
							break;
						case '+':
							info.colorOverrided = eCharColorOverride.Add;
							break;
						case '-':
							info.colorOverrided = eCharColorOverride.Minus;
							break;
						default:
							gradientSeg = para;
							info.colorOverrided = eCharColorOverride.Override;
							break;
					}
					bool success = false;
					while (true) {
						s_temp_splits_1.Clear();
						if (gradientSeg.Split(',', s_temp_splits_1) != 2) { break; }
						float angle;
						if (!float.TryParse(s_temp_splits_1[1].Trim().ToString(), out angle)) { break; }
						s_temp_splits_2.Clear();
						if (s_temp_splits_1[0].Split('-', s_temp_splits_2) != 2) { break; }
						Color color1, color2;
						if (!ParseColor(s_temp_splits_2[0].Trim(), out color1) || !ParseColor(s_temp_splits_2[1].Trim(), out color2)) { break; }
						success = true;
						info.color1 = color1;
						info.color2 = color2;
						angle = angle % 360f;
						while (angle >= 360f) { angle -= 360f; }
						while (angle < 0f) { angle += 360f; }
						info.gradientAngle = angle;
						break;
					}
					if (!success) {
						info.colorOverrided = eCharColorOverride.None;
						// Debug.LogError("Invalid gradient parameter : " + para + " ! 'ColorFrom-ColorTo,Angle' is required !");
					}
					break;
				case "b":
					info.style = info.style == FontStyle.Italic ? FontStyle.BoldAndItalic : FontStyle.Bold;
					break;
				case "i":
					info.style = info.style == FontStyle.Bold ? FontStyle.BoldAndItalic : FontStyle.Italic;
					break;
				case "bi":
				case "ib":
					info.style = FontStyle.BoldAndItalic;
					break;
				case "n":
					info.style = FontStyle.Normal;
					break;
				case "rot":
				case "rotate":
				case "rotation":
					float rot;
					if (float.TryParse(para.ToString(), out rot)) {
						info.rotation = rot;
					} else {
						// Debug.LogError("Char Rotation should be an float value !");
					}
					break;
				case "offset":
					Vector2 offset;
					if (ParseVector2(para, info.size, out offset)) {
						info.offset = offset;
					} else {
						// Debug.LogError("Offset should be an Vector2 value such as '12,23' !");
					}
					break;
				case "voffset":
					float voffset;
					if (ParseFloat(para, info.size, out voffset)) {
						info.voffset = voffset;
					}
					break;
				case "cspace":
					float cspace;
					if (ParseFloat(para, info.size, out cspace)) {
						info.cspace = cspace;
					}
					break;
				case "space":
					pair = false;
					float space;
					if (ParseFloat(para, info.size, out space)) {
						pi.border += new Vector2(0f, space);
					}
					break;
				case "scale":
					float scale;
					Vector2 scale2;
					if (ParseVector2(para, out scale2)) {
						info.scale = scale2;
					} else if (float.TryParse(para.ToString(), out scale)) {
						info.scale = new Vector2(scale, scale);
					}
					break;
				case "incline":
					Vector2 incline;
					if (ParseVector2(para, out incline)) {
						info.incline = incline;
					}
					break;
				case "pos":
				case "position":
				case "locate":
					pair = false;
					Vector2 pos;
					if (ParseVector2(para, fontsize, out pos)) {
						pi.locate = 1;
						pi.pos = pos;
					}
					break;
				case "move":
					pair = false;
					Vector2 move;
					if (ParseVector2(para, fontsize, out move)) {
						pi.locate = 2;
						pi.pos += move;
					}
					break;
				default:
					// Debug.LogError("Invalid tag : " + tag);
					break;
			}
			return info;
		}

		private static float _255 = 1f / 255f;
		private static float _15 = 1f / 15f;
		private static bool ParseColor(StringSegment seg, out Color color) {
			color = Color.clear;
			int len = seg.length;
			if (len <= 0) { return false; }
			if (seg[0] == '#' && (len == 4 || len == 5 || len == 7 || len == 9)) {
				float r, g, b;
				float a = 1f;
				try {
					int ri, gi, bi, ai;
					if (len == 4 || len == 5) {
						seg.HexToInt(1, 1, out ri);
						seg.HexToInt(2, 1, out gi);
						seg.HexToInt(3, 1, out bi);
						r = ri * _15;
						g = gi * _15;
						b = bi * _15;
						if (len == 5) {
							seg.HexToInt(4, 1, out ai);
							a = ai * _15;
						}
					} else {
						seg.HexToInt(1, 2, out ri);
						seg.HexToInt(3, 2, out gi);
						seg.HexToInt(5, 2, out bi);
						r = ri * _255;
						g = gi * _255;
						b = bi * _255;
						if (len == 9) {
							seg.HexToInt(7, 2, out ai);
							a = ai * _255;
						}
					}
					color = new Color(r, g, b, a);
					return true;
				} catch (Exception) {
					return false;
				}
			}
			switch (seg.ToString()) {
				case "red": color = Color.red; return true;
				case "green": color = Color.green; return true;
				case "blue": color = Color.blue; return true;
				case "white": color = Color.white; return true;
				case "black": color = Color.black; return true;
				case "yellow": color = Color.yellow; return true;
				case "cyan": color = Color.cyan; return true;
				case "magenta": color = Color.magenta; return true;
				case "gray": color = Color.gray; return true;
				case "grey": color = Color.grey; return true;
				case "clear": color = Color.clear; return true;
				default:
					break;
			}
			return false;
		}

		private static bool ParseFloat(StringSegment str, int size, out float val) {
			if (str.IsNullOrEmpty()) {
				val = 0f;
				return false;
			}
			int len = str.length;
			int iem = str.IndexOf("em");
			if (iem > 0) {
				val = 0f;
				if (iem + 2 < len) {
					string sub = str.Substring(iem + 2, len - iem - 2).Trim();
					if (sub[0] != '+' && sub[0] != '-') { return false; }
					if (!float.TryParse(sub, out val)) { return false; }
				}
				float relative;
				if (!float.TryParse(str.Substring(0, iem).Trim(), out relative)) { return false; }
				val += relative * size;
				return true;
			}
			if (!float.TryParse(str.ToString(), out val)) { return false; }
			return true;
		}

		private static bool ParseVector2(StringSegment str, int size, out Vector2 v2) {
			v2 = Vector2.zero;
			s_temp_splits_3.Clear();
			if (str.Split(',', s_temp_splits_3) != 2) { return false; }
			float x, y;
			if (ParseFloat(s_temp_splits_3[0].Trim(), size, out x) && ParseFloat(s_temp_splits_3[1].Trim(), size, out y)) {
				v2.x = x;
				v2.y = y;
				return true;
			}
			return false;
		}

		private static bool ParseVector2(StringSegment str, out Vector2 v2) {
			v2 = Vector2.zero;
			s_temp_splits_3.Clear();
			if (str.Split(',', s_temp_splits_3) != 2) { return false; }
			float x, y;
			if (float.TryParse(s_temp_splits_3[0].Trim().ToString(), out x) && float.TryParse(s_temp_splits_3[1].Trim().ToString(), out y)) {
				v2.x = x;
				v2.y = y;
				return true;
			}
			return false;
		}

		private static int ReadWord(string content, ref int index, out int si) {
			si = index;
			int len = content.Length;
			int type = 0; // 1:word 2:other
			while (index < len) {
				char chr = content[index];
				if (chr == '_' || (chr >= 'A' && chr <= 'Z') || (chr >= 'a' && chr <= 'z') || (chr >= '0' && chr <= '9') || (int)chr > 127) {
					bool end = false;
					switch (type) {
						case 0: type = 1; break;
						case 1: break;
						default:
							end = true;
							break;
					}
					if (end) { return type; }
					index++;
				} else if (chr == ' ' || chr == '\t' || chr == '\n' || chr == '\r') {
					if (type != 0) { return type; }
					si++;
					index++;
				} else {
					if (type == 0) { index++; }
					return type == 0 ? 2 : type;
				}
			}
			return type;
		}

		#endregion char analysis

		private static List<CharData> s_temp_invalids = new List<CharData>(64);

		private static Mesh s_temp_mesh;
		private static List<Vector3> s_temp_vertices = new List<Vector3>(64);
		private static List<Vector4> s_temp_uv1s = new List<Vector4>(64);
		private static List<Vector4> s_temp_uv2s = new List<Vector4>(64);
		private static List<Vector4> s_temp_uv3s = new List<Vector4>(64);
		private static List<Vector4> s_temp_uv4s = new List<Vector4>(64);
		private static List<Color> s_temp_colors = new List<Color>(64);
		private static List<List<int>> s_temp_triangles = new List<List<int>>();
		private static Queue<List<int>> s_cached_triangles = new Queue<List<int>>();
		private static Vector4[] s_temp_alpha_keys = new Vector4[8];
		private static Vector4[] s_temp_color_keys = new Vector4[8];
		private static Vector2[] s_temp_offsets1 = new Vector2[16];
		private static Vector2[] s_temp_offsets2 = new Vector2[16];
		private static Vector4[] s_temp_offsets = new Vector4[16];
		private static Vector4[] s_temp_extend_offsets = new Vector4[32];
		private static List<MaterialPropertyBlock> s_submesh_blocks = new List<MaterialPropertyBlock>();

		private static CommandBuffer s_command_buffer;
		private static Material s_material_text_mesh;
		private static int s_prop_text_pattern;
		private static int s_prop_text_region;
		private static int s_prop_text_per_char;
		private static int s_prop_text_alpha_keys_count;
		private static int s_prop_text_alpha_keys;
		private static int s_prop_text_color_keys_count;
		private static int s_prop_text_color_keys;
		private static int s_prop_text_gradient_direction;
		private static int s_prop_text_trapezoid;

		private static Material s_material_draw_edge;
		private static Material s_material_draw_3d_perspective;
		private static Material s_material_draw_3d_orthographic;
		private static Material s_material_draw_offset;
		private static Material s_material_draw_blurdir;
		private static Material s_material_draw_extend;
		private static Material s_material_draw_applymask;
		private static Material s_material_draw_blend;
		private static int s_prop_main_tex;
		private static int s_prop_size;
		private static int s_prop_region_text;
		private static int s_prop_offsets1_count;
		private static int s_prop_offsets2_count;
		private static int s_prop_offsets;
		private static int s_prop_blur_dir;
		private static int s_prop_blur_color_mask;
		private static int s_prop_blur_color_level_min;
		private static int s_prop_txt_tex;
		private static int s_prop_txt_3d_tex;
		private static int s_prop_extend_color_mask;
		private static int s_prop_extend_count;
		private static int s_prop_channel_mask;
		private static int s_prop_phase;
		private static int s_prop_3d_endpoint;
		private static int s_prop_3d_faded;
		private static int s_prop_light_360_alpha_keys_count;
		private static int s_prop_light_360_alpha_keys;
		private static int s_prop_light_360_color_keys_count;
		private static int s_prop_light_360_color_keys;
		private static int s_prop_fx_inner_gradient_alpha_keys_count;
		private static int s_prop_fx_inner_gradient_alpha_keys;
		private static int s_prop_fx_inner_gradient_color_keys_count;
		private static int s_prop_fx_inner_gradient_color_keys;
		private static int s_prop_fx_inner_gradient_direction;
		private static int s_prop_fx_inner_texture;
		private static int s_prop_fx_outer_gradient_alpha_keys_count;
		private static int s_prop_fx_outer_gradient_alpha_keys;
		private static int s_prop_fx_outer_gradient_color_keys_count;
		private static int s_prop_fx_outer_gradient_color_keys;
		private static int s_prop_fx_outer_gradient_direction;
		private static int s_prop_fx_outer_texture;
		private static int s_prop_fx_outline_gradient_alpha_keys_count;
		private static int s_prop_fx_outline_gradient_alpha_keys;
		private static int s_prop_fx_outline_gradient_color_keys_count;
		private static int s_prop_fx_outline_gradient_color_keys;
		private static int s_prop_fx_outline_gradient_direction;
		private static int s_prop_fx_outline_texture;
		private static int s_prop_fx_glow_gradient_alpha_keys_count;
		private static int s_prop_fx_glow_gradient_alpha_keys;
		private static int s_prop_fx_glow_gradient_color_keys_count;
		private static int s_prop_fx_glow_gradient_color_keys;
		private static int s_prop_fx_glow_gradient_direction;
		private static int s_prop_fx_glow_texture;
		private static int s_prop_emboss;
		private static int s_prop_emboss_light_color;
		private static int s_prop_fx_blend;
		private static int s_prop_glow_blend;

		private static Dictionary<int, StringBuilder> s_temp_chars = new Dictionary<int, StringBuilder>();
		private static Queue<StringBuilder> s_cached_stringbuilder = new Queue<StringBuilder>();
		private static IFontInternal[] s_submesh_fonts = new IFontInternal[100];
		private static int[] s_font_to_submesh = new int[100];

		private static float half_square_2 = 0.7071068f;

		private static void TryGetTextMesh(IFontInternal[] fonts, float lineHeight, List<CharInfo> content, eTextColorType colorType, float charGradientAngle,
			List<CharData> invalids, out Mesh mesh, out IFontInternal[] fnts) {
			int fontsCount = fonts.Length;
			if (fontsCount <= 0 || fonts[0] == null) { mesh = null; fnts = null; return; }
			if (s_temp_mesh == null) {
				s_temp_mesh = new Mesh();
				s_temp_mesh.name = "[TextToRenderTexture] temp mesh";
			}
			Vector2 charGradientDir = Vector2.right;
			if (colorType == eTextColorType.GradientPerChar && charGradientAngle != 0f) {
				float rad = charGradientAngle * Mathf.Deg2Rad;
				charGradientDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
			}
			s_temp_mesh.Clear(true);
			s_temp_vertices.Clear();
			s_temp_uv1s.Clear();
			s_temp_uv2s.Clear();
			s_temp_uv3s.Clear();
			s_temp_uv4s.Clear();
			s_temp_colors.Clear();
			for (int i = s_temp_triangles.Count - 1; i >= 0; i--) {
				List<int> triangles = s_temp_triangles[i];
				triangles.Clear();
				s_cached_triangles.Enqueue(triangles);
			}
			s_temp_triangles.Clear();
			Vector2 basePos = Vector2.zero;
			Vector2 pos = Vector2.zero;
			bool flag = true;
			for (int i = 0; i < fontsCount; i++) {
				s_font_to_submesh[i] = -1;
			}
			int submeshCount = 0;
			foreach (CharInfo ci in content) {
				if (ci.chr == '\n') {
					pos.x = ci.border.y;
					pos.y -= lineHeight;
					continue;
				}
				if (ci.chr == '\r') { continue; }
				IFontInternal font = null;
				if (ci.font >= 0 && ci.font < fontsCount) {
					font = fonts[ci.font];
				}
				bool fakeFont = false;
				if (font == null) { font = fonts[0]; fakeFont = true; }
				CharacterInfo info;
				if (!font.GetCharacterInfo(ci.chr, out info, ci.size, ci.style)) {
					invalids.Add(new CharData() { index = ci.font, chr = ci.chr, size = ci.size, style = ci.style });
					flag = false;
					continue;
				}
				// Debug.LogWarningFormat("char : {0}, size : {1}x{2}, x from to : {3}-{4}, y from to : {5}-{6}, bearing : {7}, advance : {8}",
				// 	ci.chr, info.glyphWidth, info.glyphHeight, info.minX, info.maxX, info.minY, info.maxY, info.bearing, info.advance);
				pos.x += ci.border.x + ci.cspace;
				int vertex_index = s_temp_vertices.Count;
				Vector2 scale = new Vector2(Mathf.Abs(ci.scale.x), Mathf.Abs(ci.scale.y));
				Vector2 charMin = new Vector2(info.minX * scale.x, info.minY * scale.y);
				Vector2 charMax = new Vector2(info.maxX * scale.x, info.maxY * scale.y);
				if (ci.scale.x < 0f) {
					float tx = charMin.x;
					charMin.x = charMax.x;
					charMax.x = tx;
				}
				if (ci.scale.y < 0f) {
					float ty = charMin.y;
					charMin.y = charMax.y;
					charMax.y = ty;
				}
				Vector2 lb = basePos + pos + new Vector2(charMin.x, charMin.y);
				Vector2 lt = basePos + pos + new Vector2(charMin.x, charMax.y);
				Vector2 rt = basePos + pos + new Vector2(charMax.x, charMax.y);
				Vector2 rb = basePos + pos + new Vector2(charMax.x, charMin.y);
				if (ci.incline != Vector2.zero) {
					Vector2 pivot = (lb + rt) * 0.5f;
					Vector2 plb = lb - pivot;
					Vector2 plt = lt - pivot;
					Vector2 prt = rt - pivot;
					Vector2 prb = rb - pivot;
					lb = pivot + new Vector2(plb.x + plb.y * ci.incline.x, plb.y + plb.x * ci.incline.y);
					lt = pivot + new Vector2(plt.x + plt.y * ci.incline.x, plt.y + plt.x * ci.incline.y);
					rt = pivot + new Vector2(prt.x + prt.y * ci.incline.x, prt.y + prt.x * ci.incline.y);
					rb = pivot + new Vector2(prb.x + prb.y * ci.incline.x, prb.y + prb.x * ci.incline.y);
				}
				Vector2 currentCharGradientDir = charGradientDir;
				if (ci.rotation != 0f) {
					Vector2 pivot = (lb + rt) * 0.5f;
					Vector2 plb = lb - pivot;
					Vector2 plt = lt - pivot;
					Vector2 prt = rt - pivot;
					Vector2 prb = rb - pivot;
					float rad = ci.rotation * Mathf.Deg2Rad;
					float cos = Mathf.Cos(rad);
					float sin = Mathf.Sin(rad);
					lb = pivot + new Vector2(plb.x * cos - plb.y * sin, plb.y * cos + plb.x * sin);
					lt = pivot + new Vector2(plt.x * cos - plt.y * sin, plt.y * cos + plt.x * sin);
					rt = pivot + new Vector2(prt.x * cos - prt.y * sin, prt.y * cos + prt.x * sin);
					rb = pivot + new Vector2(prb.x * cos - prb.y * sin, prb.y * cos + prb.x * sin);
					currentCharGradientDir = new Vector2(charGradientDir.x * cos - charGradientDir.y * sin, charGradientDir.y * cos + charGradientDir.x * sin);
				}
				Vector2 offset = ci.offset + new Vector2(0f, ci.voffset);
				lb += offset;
				lt += offset;
				rt += offset;
				rb += offset;
				s_temp_vertices.Add(new Vector3(lb.x, lb.y, 0f));
				s_temp_vertices.Add(new Vector3(lt.x, lt.y, 0f));
				s_temp_vertices.Add(new Vector3(rt.x, rt.y, 0f));
				s_temp_vertices.Add(new Vector3(rb.x, rb.y, 0f));
				float gLB = 0f;
				float gLT = 0f;
				float gRT = 0f;
				float gRB = 0f;
				if (colorType == eTextColorType.GradientPerChar) {
					float gMin = float.MaxValue;
					float gMax = float.MinValue;
					gLB = Vector2.Dot(currentCharGradientDir, lb);
					gLT = Vector2.Dot(currentCharGradientDir, lt);
					gRT = Vector2.Dot(currentCharGradientDir, rt);
					gRB = Vector2.Dot(currentCharGradientDir, rb);
					gMin = Mathf.Min(gMin, gLB);
					gMin = Mathf.Min(gMin, gLT);
					gMin = Mathf.Min(gMin, gRT);
					gMin = Mathf.Min(gMin, gRB);
					gMax = Mathf.Max(gMax, gLB);
					gMax = Mathf.Max(gMax, gLT);
					gMax = Mathf.Max(gMax, gRT);
					gMax = Mathf.Max(gMax, gRB);
					gLB = Mathf.InverseLerp(gMin, gMax, gLB);
					gLT = Mathf.InverseLerp(gMin, gMax, gLT);
					gRT = Mathf.InverseLerp(gMin, gMax, gRT);
					gRB = Mathf.InverseLerp(gMin, gMax, gRB);
				}
				float applyGlobalColor = (float)ci.colorOverrided;
				s_temp_uv1s.Add(GetVector4(info.uvBottomLeft, applyGlobalColor, gLB));
				s_temp_uv1s.Add(GetVector4(info.uvTopLeft, applyGlobalColor, gLT));
				s_temp_uv1s.Add(GetVector4(info.uvTopRight, applyGlobalColor, gRT));
				s_temp_uv1s.Add(GetVector4(info.uvBottomRight, applyGlobalColor, gRB));
				s_temp_uv2s.Add(new Vector4(0f, 0f, lb.x, lb.y));
				s_temp_uv2s.Add(new Vector4(0f, 1f, lt.x, lt.y));
				s_temp_uv2s.Add(new Vector4(1f, 1f, rt.x, rt.y));
				s_temp_uv2s.Add(new Vector4(1f, 0f, rb.x, rb.y));
				Vector4 uv3 = GetVector4(info.uvBottomLeft, info.uvTopLeft);
				Vector4 uv4 = GetVector4(info.uvTopRight, info.uvBottomRight);
				s_temp_uv3s.Add(uv3);
				s_temp_uv3s.Add(uv3);
				s_temp_uv3s.Add(uv3);
				s_temp_uv3s.Add(uv3);
				s_temp_uv4s.Add(uv4);
				s_temp_uv4s.Add(uv4);
				s_temp_uv4s.Add(uv4);
				s_temp_uv4s.Add(uv4);
				if (ci.gradientAngle >= 0f) {
					float rad = Mathf.Deg2Rad * ci.gradientAngle;
					Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
					float dirMin = float.MaxValue;
					float dirMax = float.MinValue;
					Vector2 pivot = (lb + rt) * 0.5f;
					float dlb = Vector2.Dot(dir, lb - pivot);
					float dlt = Vector2.Dot(dir, lt - pivot);
					float drt = Vector2.Dot(dir, rt - pivot);
					float drb = Vector2.Dot(dir, rb - pivot);
					dirMin = Mathf.Min(dirMin, dlb);
					dirMin = Mathf.Min(dirMin, dlt);
					dirMin = Mathf.Min(dirMin, drt);
					dirMin = Mathf.Min(dirMin, drb);
					dirMax = Mathf.Max(dirMax, dlb);
					dirMax = Mathf.Max(dirMax, dlt);
					dirMax = Mathf.Max(dirMax, drt);
					dirMax = Mathf.Max(dirMax, drb);
					s_temp_colors.Add(Color.Lerp(ci.color1, ci.color2, Mathf.InverseLerp(dirMin, dirMax, dlb)));
					s_temp_colors.Add(Color.Lerp(ci.color1, ci.color2, Mathf.InverseLerp(dirMin, dirMax, dlt)));
					s_temp_colors.Add(Color.Lerp(ci.color1, ci.color2, Mathf.InverseLerp(dirMin, dirMax, drt)));
					s_temp_colors.Add(Color.Lerp(ci.color1, ci.color2, Mathf.InverseLerp(dirMin, dirMax, drb)));
				} else {
					s_temp_colors.Add(ci.color1);
					s_temp_colors.Add(ci.color1);
					s_temp_colors.Add(ci.color1);
					s_temp_colors.Add(ci.color1);
				}
				int si = s_font_to_submesh[fakeFont ? 0 : ci.font];
				if (si < 0) {
					si = submeshCount;
					submeshCount++;
					s_font_to_submesh[fakeFont ? 0 : ci.font] = si;
					s_submesh_fonts[si] = font;
				}
				if (si >= s_temp_triangles.Count) {
					List<int> tris = s_cached_triangles.Count > 0 ? s_cached_triangles.Dequeue() : new List<int>(128);
					s_temp_triangles.Add(tris);
				}
				List<int> triangles = s_temp_triangles[si];
				triangles.Add(vertex_index);
				triangles.Add(vertex_index + 1);
				triangles.Add(vertex_index + 2);
				triangles.Add(vertex_index + 2);
				triangles.Add(vertex_index + 3);
				triangles.Add(vertex_index);
				pos.x += info.advance * scale.x + ci.border.y;
				switch (ci.locate) {
					case 1:
						basePos = ci.pos;
						pos = Vector2.zero;
						break;
					case 2:
						basePos += pos + ci.pos;
						pos = Vector2.zero;
						break;
				}
			}
			if (!flag) { mesh = null; fnts = null; return; }
			s_temp_mesh.SetVertices(s_temp_vertices);
			s_temp_mesh.SetUVs(0, s_temp_uv1s);
			s_temp_mesh.SetUVs(1, s_temp_uv2s);
			s_temp_mesh.SetUVs(2, s_temp_uv3s);
			s_temp_mesh.SetUVs(3, s_temp_uv4s);
			s_temp_mesh.SetColors(s_temp_colors);
			int subMeshCount = s_temp_triangles.Count;
			s_temp_mesh.subMeshCount = subMeshCount;
			for (int i = 0; i < subMeshCount; i++) {
				s_temp_mesh.SetTriangles(s_temp_triangles[i], i);
			}
			mesh = s_temp_mesh;
			fnts = s_submesh_fonts;
		}

		private static bool RequestCharsFromFont(IFontInternal[] fonts, List<CharData> invalids) {
			foreach (CharData cd in invalids) {
				int key = cd.GetKey();
				StringBuilder sb;
				if (!s_temp_chars.TryGetValue(key, out sb)) {
					sb = s_cached_stringbuilder.Count > 0 ? s_cached_stringbuilder.Dequeue() : new StringBuilder();
					s_temp_chars.Add(key, sb);
				}
				sb.Append(cd.chr);
			}
			bool ret = true;
			foreach (KeyValuePair<int, StringBuilder> kv in s_temp_chars) {
				s_cached_stringbuilder.Enqueue(kv.Value);
				if (kv.Value.Length <= 0) { continue; }
				string str = kv.Value.ToString();
				kv.Value.Clear();
				int fi;
				int size;
				FontStyle style;
				CharData.GetSizeAndStyle(kv.Key, out fi, out size, out style);
				if (fi < 0 || fi >= fonts.Length) { ret = false; }
				if (ret) { fonts[fi].RequestCharactersInTexture(str, size, style); }
			}
			s_temp_chars.Clear();
			return ret;
		}

		private static bool Not(object obj) {
			return obj == null || obj.Equals(null);
		}

		private static RenderTexture DrawTextMesh(IFontInternal[] fonts, Mesh mesh, Parameter paras, eRenderTextureAllocType allocType, out Rect rect) {
			if (Not(s_command_buffer) || Not(s_material_text_mesh)) {
				s_command_buffer = new CommandBuffer();
				s_material_text_mesh = new Material(Shader.Find("Hidden/TextToRenderTexture/RenderTextMesh"));
				s_prop_text_pattern = Shader.PropertyToID("_PatternTex");
				s_prop_text_region = Shader.PropertyToID("_TextRegion");
				s_prop_text_per_char = Shader.PropertyToID("_PerCharPattern");
				s_prop_text_alpha_keys_count = Shader.PropertyToID("_AlphaKeysCount");
				s_prop_text_alpha_keys = Shader.PropertyToID("_AlphaKeys");
				s_prop_text_color_keys_count = Shader.PropertyToID("_ColorKeysCount");
				s_prop_text_color_keys = Shader.PropertyToID("_ColorKeys");
				s_prop_text_gradient_direction = Shader.PropertyToID("_GradientDirection");
				s_prop_text_trapezoid = Shader.PropertyToID("_Trapezoid");
				s_material_draw_edge = new Material(Shader.Find("Hidden/TextToRenderTexture/RenderShader_Edge"));
				s_material_draw_3d_perspective = new Material(Shader.Find("Hidden/TextToRenderTexture/RenderShader_Perspective3D"));
				s_material_draw_3d_orthographic = new Material(Shader.Find("Hidden/TextToRenderTexture/RenderShader_Orthographic3D"));
				s_material_draw_offset = new Material(Shader.Find("Hidden/TextToRenderTexture/RenderShader_Offset"));
				s_material_draw_blurdir = new Material(Shader.Find("Hidden/TextToRenderTexture/RenderShader_BlurDir"));
				s_material_draw_extend = new Material(Shader.Find("Hidden/TextToRenderTexture/RenderShader_Extend"));
				s_material_draw_applymask = new Material(Shader.Find("Hidden/TextToRenderTexture/RenderShader_ApplyMask"));
				s_material_draw_blend = new Material(Shader.Find("Hidden/TextToRenderTexture/RenderShader_Blend"));
				s_prop_main_tex = Shader.PropertyToID("_MainTex");
				s_prop_size = Shader.PropertyToID("_Size");
				s_prop_region_text = Shader.PropertyToID("_RegionText");
				s_prop_offsets1_count = Shader.PropertyToID("_Offsets1Count");
				s_prop_offsets2_count = Shader.PropertyToID("_Offsets2Count");
				s_prop_offsets = Shader.PropertyToID("_Offsets");
				s_prop_blur_dir = Shader.PropertyToID("_BlurDir");
				s_prop_blur_color_mask = Shader.PropertyToID("_BlurColorMask");
				s_prop_blur_color_level_min = Shader.PropertyToID("_ColorLevelMin");
				s_prop_txt_tex = Shader.PropertyToID("_TextTex");
				s_prop_txt_3d_tex = Shader.PropertyToID("_Text3D");
				s_prop_extend_color_mask = Shader.PropertyToID("_ExtendColorMask");
				s_prop_extend_count = Shader.PropertyToID("_ExtendCount");
				s_prop_channel_mask = Shader.PropertyToID("_ChannelMask");
				s_prop_phase = Shader.PropertyToID("_Phase");
				s_prop_3d_endpoint = Shader.PropertyToID("_3DEndPoint");
				s_prop_3d_faded = Shader.PropertyToID("_3DFaded");
				s_prop_light_360_alpha_keys_count = Shader.PropertyToID("_Light360AlphaKeysCount");
				s_prop_light_360_alpha_keys = Shader.PropertyToID("_Light360AlphaKeys");
				s_prop_light_360_color_keys_count = Shader.PropertyToID("_Light360ColorKeysCount");
				s_prop_light_360_color_keys = Shader.PropertyToID("_Light360ColorKeys");
				s_prop_fx_inner_gradient_alpha_keys_count = Shader.PropertyToID("_InnerAlphaKeysCount");
				s_prop_fx_inner_gradient_alpha_keys = Shader.PropertyToID("_InnerAlphaKeys");
				s_prop_fx_inner_gradient_color_keys_count = Shader.PropertyToID("_InnerColorKeysCount");
				s_prop_fx_inner_gradient_color_keys = Shader.PropertyToID("_InnerColorKeys");
				s_prop_fx_inner_gradient_direction = Shader.PropertyToID("_InnerGradientDirection");
				s_prop_fx_inner_texture = Shader.PropertyToID("_InnerTex");
				s_prop_fx_outer_gradient_alpha_keys_count = Shader.PropertyToID("_OuterAlphaKeysCount");
				s_prop_fx_outer_gradient_alpha_keys = Shader.PropertyToID("_OuterAlphaKeys");
				s_prop_fx_outer_gradient_color_keys_count = Shader.PropertyToID("_OuterColorKeysCount");
				s_prop_fx_outer_gradient_color_keys = Shader.PropertyToID("_OuterColorKeys");
				s_prop_fx_outer_gradient_direction = Shader.PropertyToID("_OuterGradientDirection");
				s_prop_fx_outer_texture = Shader.PropertyToID("_OuterTex");
				s_prop_fx_outline_gradient_alpha_keys_count = Shader.PropertyToID("_OutlineAlphaKeysCount");
				s_prop_fx_outline_gradient_alpha_keys = Shader.PropertyToID("_OutlineAlphaKeys");
				s_prop_fx_outline_gradient_color_keys_count = Shader.PropertyToID("_OutlineColorKeysCount");
				s_prop_fx_outline_gradient_color_keys = Shader.PropertyToID("_OutlineColorKeys");
				s_prop_fx_outline_gradient_direction = Shader.PropertyToID("_OutlineGradientDirection");
				s_prop_fx_outline_texture = Shader.PropertyToID("_OutlineTex");
				s_prop_fx_glow_gradient_alpha_keys_count = Shader.PropertyToID("_GlowAlphaKeysCount");
				s_prop_fx_glow_gradient_alpha_keys = Shader.PropertyToID("_GlowAlphaKeys");
				s_prop_fx_glow_gradient_color_keys_count = Shader.PropertyToID("_GlowColorKeysCount");
				s_prop_fx_glow_gradient_color_keys = Shader.PropertyToID("_GlowColorKeys");
				s_prop_fx_glow_gradient_direction = Shader.PropertyToID("_GlowGradientDirection");
				s_prop_fx_glow_texture = Shader.PropertyToID("_GlowTex");
				s_prop_emboss = Shader.PropertyToID("_Emboss");
				s_prop_emboss_light_color = Shader.PropertyToID("_EmbossLightColor");
				s_prop_fx_blend = Shader.PropertyToID("_FxBlend");
				s_prop_glow_blend = Shader.PropertyToID("_GlowBlend");
			}

			GradientAlphaKey[] alphaKeys;
			GradientColorKey[] colorKeys;

			Bounds bounds = mesh.bounds;
			Vector3 min = bounds.min;
			Vector3 max = bounds.max;
			Vector2 meshSize = max - min;

			Vector4 extend = Vector4.zero;
			Vector2 s_min = Vector2.zero;
			Vector2 s_max = Vector2.zero;
			Vector4 spacialExtend = Vector4.zero;
			if (paras._shadow_intensity > 0f) {
				float blur = paras._shadow_extend + paras._shadow_blur.magnitude;
				s_min = paras._shadow_offset - new Vector2(blur, blur);
				s_max = paras._shadow_offset + new Vector2(blur, blur);
				extend.x = Mathf.Max(extend.x, s_max.x);
				extend.y = Mathf.Max(extend.y, s_max.y);
				extend.z = Mathf.Max(extend.z, -s_min.x);
				extend.w = Mathf.Max(extend.w, -s_min.y);
			} else if (paras._shadow_intensity < -1.5f) {
				float rad = -Mathf.Deg2Rad * paras._3d_endpoint.x;
				Vector2 offset = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * paras._3d_length;
				spacialExtend.x = Mathf.Max(0f, offset.x);
				spacialExtend.y = Mathf.Max(0f, -offset.y);
				spacialExtend.z = Mathf.Max(0f, -offset.x);
				spacialExtend.w = Mathf.Max(0f, offset.y);
			} else if (paras._shadow_intensity < -0.5f) {
				// perspective
				Vector2 endpoint = new Vector2(paras._3d_endpoint.x * meshSize.x, paras._3d_endpoint.y * meshSize.y);
				spacialExtend.x = Mathf.Max(0f, (endpoint.x - max.x) * paras._3d_length);
				spacialExtend.y = Mathf.Max(0f, (endpoint.y - max.y) * paras._3d_length);
				spacialExtend.z = Mathf.Max(0f, (min.x - endpoint.x) * paras._3d_length);
				spacialExtend.w = Mathf.Max(0f, (min.y - endpoint.y) * paras._3d_length);
			}
			extend.x = Mathf.Max(extend.x, spacialExtend.x + paras._outline);
			extend.y = Mathf.Max(extend.y, spacialExtend.y + paras._outline);
			extend.z = Mathf.Max(extend.z, spacialExtend.z + paras._outline);
			extend.w = Mathf.Max(extend.w, spacialExtend.w + paras._outline);
			extend.x = Mathf.Ceil(extend.x);
			extend.y = Mathf.Ceil(extend.y);
			extend.z = Mathf.Ceil(extend.z);
			extend.w = Mathf.Ceil(extend.w);
			Vector2 size = new Vector2(meshSize.x + extend.x + extend.z, meshSize.y + extend.y + extend.w);
			int width = Mathf.CeilToInt(size.x);
			int height = Mathf.CeilToInt(size.y);
			rect = new Rect(extend.z, extend.w, width - extend.x - extend.z, height - extend.y - extend.w);
			RenderTextureDescriptor desc = new RenderTextureDescriptor(width, height, RenderTextureFormat.ARGB32, 0, 0);
			desc.sRGB = true;
			RenderTexture brt = RenderTexture.GetTemporary(desc);
			s_command_buffer.Clear();
			s_command_buffer.SetRenderTarget(brt);
			s_command_buffer.ClearRenderTarget(true, true, Color.clear);
			s_command_buffer.SetViewMatrix(Matrix4x4.identity);
			Matrix4x4 proj = Matrix4x4.Ortho(min.x - extend.z, max.x + extend.x, min.y - extend.w, max.y + extend.y, -1f, 1f);
			s_command_buffer.SetProjectionMatrix(proj);
			s_command_buffer.SetViewport(new Rect(0f, 0f, size.x, size.y));
			s_material_text_mesh.SetVector(s_prop_text_region, GetVector4(min, meshSize));
			float gradientPerChar = paras._color_type == eTextColorType.GradientPerChar ? 1f : 0f;
			float texturePerChar = paras._texture_per_char ? 1f : 0f;
			s_material_text_mesh.SetVector(s_prop_text_per_char, new Vector4(gradientPerChar, texturePerChar));
			Texture pattern = paras._texture;
			if (pattern == null || pattern.Equals(null)) {
				pattern = GetWhiteTexture();
			}
			s_material_text_mesh.SetTexture(s_prop_text_pattern, pattern);
			s_material_text_mesh.SetTextureScale(s_prop_text_pattern, new Vector2(paras._texture_tiling_offset.x, paras._texture_tiling_offset.y));
			s_material_text_mesh.SetTextureOffset(s_prop_text_pattern, new Vector2(paras._texture_tiling_offset.z, paras._texture_tiling_offset.w));
			switch (paras._color_type) {
				case eTextColorType.Gradient:
				case eTextColorType.GradientPerChar:
					alphaKeys = paras._gradient.alphaKeys;
					colorKeys = paras._gradient.colorKeys;
					for (int i = 0; i < alphaKeys.Length; i++) {
						GradientAlphaKey key = alphaKeys[i];
						s_temp_alpha_keys[i] = new Vector4(key.alpha, key.alpha, key.alpha, key.time);
					}
					for (int i = 0; i < colorKeys.Length; i++) {
						GradientColorKey key = colorKeys[i];
						s_temp_color_keys[i] = new Vector4(key.color.r, key.color.g, key.color.b, key.time);
					}
					float rad = Mathf.Deg2Rad * paras._gradient_angle;
					Vector2 gradientDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
					float gmin = float.MaxValue;
					float gmax = float.MinValue;
					float glb = Vector2.Dot(gradientDir, new Vector2(min.x, min.y));
					float glt = Vector2.Dot(gradientDir, new Vector2(min.x, max.y));
					float grt = Vector2.Dot(gradientDir, new Vector2(max.x, max.y));
					float grb = Vector2.Dot(gradientDir, new Vector2(max.x, min.y));
					gmin = Mathf.Min(gmin, glb);
					gmin = Mathf.Min(gmin, glt);
					gmin = Mathf.Min(gmin, grt);
					gmin = Mathf.Min(gmin, grb);
					gmax = Mathf.Max(gmax, glb);
					gmax = Mathf.Max(gmax, glt);
					gmax = Mathf.Max(gmax, grt);
					gmax = Mathf.Max(gmax, grb);
					s_material_text_mesh.SetVector(s_prop_text_gradient_direction, new Vector4(gradientDir.x, gradientDir.y, gmin, gmax));
					s_material_text_mesh.SetInt(s_prop_text_alpha_keys_count, alphaKeys.Length);
					s_material_text_mesh.SetVectorArray(s_prop_text_alpha_keys, s_temp_alpha_keys);
					s_material_text_mesh.SetInt(s_prop_text_color_keys_count, colorKeys.Length);
					s_material_text_mesh.SetVectorArray(s_prop_text_color_keys, s_temp_color_keys);
					break;
				default:
					s_temp_alpha_keys[0] = new Vector4(paras._color.a, paras._color.a, paras._color.a, 1);
					s_temp_color_keys[0] = new Vector4(paras._color.r, paras._color.g, paras._color.b, 1);
					s_material_text_mesh.SetInt(s_prop_text_alpha_keys_count, 1);
					s_material_text_mesh.SetVectorArray(s_prop_text_alpha_keys, s_temp_alpha_keys);
					s_material_text_mesh.SetInt(s_prop_text_color_keys_count, 1);
					s_material_text_mesh.SetVectorArray(s_prop_text_color_keys, s_temp_color_keys);
					break;
			}
			s_material_text_mesh.SetVector(s_prop_text_trapezoid, new Vector4(paras._char_trapezoid, paras._char_trapezoid_ratio, paras._char_trapezoid_concave_convex));
			int subMeshCount = mesh.subMeshCount;
			while (s_submesh_blocks.Count < subMeshCount) {
				s_submesh_blocks.Add(new MaterialPropertyBlock());
			}
			for (int i = 0; i < subMeshCount; i++) {
				MaterialPropertyBlock block = s_submesh_blocks[i];
				block.SetTexture(s_prop_main_tex, fonts[i].texture);
				s_command_buffer.DrawMesh(mesh, Matrix4x4.identity, s_material_text_mesh, i, 0, block);
			}
			Graphics.ExecuteCommandBuffer(s_command_buffer);
			for (int i = 0; i < subMeshCount; i++) { s_submesh_blocks[i].Clear(); }

			RenderTexture b3d = RenderTexture.GetTemporary(desc);

			Vector4 propSize = new Vector4(width, height, paras._outline, paras._outline);
			Vector4 propRegion = new Vector4(extend.z, extend.w, meshSize.x, meshSize.y);
			s_material_draw_offset.SetVector(s_prop_size, propSize);
			s_material_draw_offset.SetVector(s_prop_region_text, propRegion);
			s_material_draw_blurdir.SetVector(s_prop_size, propSize);
			s_material_draw_blurdir.SetVector(s_prop_region_text, propRegion);
			s_material_draw_extend.SetVector(s_prop_size, propSize);
			s_material_draw_extend.SetVector(s_prop_region_text, propRegion);
			s_material_draw_applymask.SetVector(s_prop_size, propSize);
			s_material_draw_applymask.SetVector(s_prop_region_text, propRegion);
			s_material_draw_blend.SetVector(s_prop_size, propSize);
			s_material_draw_blend.SetVector(s_prop_region_text, propRegion);

			float _w = 1f / size.x;
			float _h = 1f / size.y;

			if (paras._shadow_intensity < 0f && paras._3d_length > 0f) {
				s_material_draw_edge.SetVector(s_prop_size, propSize);
				s_material_draw_edge.SetVector(s_prop_region_text, propRegion);
				alphaKeys = paras._3d_light.alphaKeys;
				colorKeys = paras._3d_light.colorKeys;
				for (int i = 0; i < alphaKeys.Length; i++) {
					GradientAlphaKey key = alphaKeys[i];
					s_temp_alpha_keys[i] = new Vector4(key.alpha, key.alpha, key.alpha, key.time);
				}
				for (int i = 0; i < colorKeys.Length; i++) {
					GradientColorKey key = colorKeys[i];
					s_temp_color_keys[i] = new Vector4(key.color.r, key.color.g, key.color.b, key.time);
				}
				s_material_draw_edge.SetInt(s_prop_light_360_alpha_keys_count, alphaKeys.Length);
				s_material_draw_edge.SetVectorArray(s_prop_light_360_alpha_keys, s_temp_alpha_keys);
				s_material_draw_edge.SetInt(s_prop_light_360_color_keys_count, colorKeys.Length);
				s_material_draw_edge.SetVectorArray(s_prop_light_360_color_keys, s_temp_color_keys);

				RenderTexture drt = RenderTexture.GetTemporary(desc);

				if (paras._3d_light_soft > 0f) {
					s_material_draw_blurdir.SetFloat(s_prop_blur_color_level_min, 0f);
					s_material_draw_blurdir.SetVector(s_prop_blur_color_mask, new Vector4(0f, 0f, 0f, 1f));
					s_material_draw_blurdir.SetVector(s_prop_blur_dir, new Vector4(_w * paras._3d_light_soft, _h * paras._3d_light_soft));
					Graphics.Blit(brt, drt, s_material_draw_blurdir);
					s_material_draw_blurdir.SetVector(s_prop_blur_dir, new Vector4(_w * paras._3d_light_soft, -_h * paras._3d_light_soft));
					Graphics.Blit(drt, b3d, s_material_draw_blurdir);
				} else {
					Graphics.Blit(brt, b3d);
				}
				s_material_draw_edge.SetTexture(s_prop_txt_tex, brt);
				s_material_draw_edge.SetFloat(s_prop_phase, paras._3d_light_phase * Mathf.Deg2Rad);
				Graphics.Blit(b3d, drt, s_material_draw_edge);

				if (paras._shadow_intensity < -1.5f) {
					s_material_draw_3d_orthographic.SetVector(s_prop_size, propSize);
					s_material_draw_3d_orthographic.SetVector(s_prop_region_text, propRegion);
					float deg = paras._3d_endpoint.x * Mathf.Deg2Rad;
					s_material_draw_3d_orthographic.SetVector(s_prop_3d_endpoint, new Vector4(-Mathf.Cos(deg), -Mathf.Sin(deg), 0, paras._3d_length));
					s_material_draw_3d_orthographic.SetColor(s_prop_3d_faded, paras._3d_faded);
					Graphics.Blit(drt, b3d, s_material_draw_3d_orthographic);
				} else if (paras._shadow_intensity < -0.5f) {
					s_material_draw_3d_perspective.SetVector(s_prop_size, propSize);
					s_material_draw_3d_perspective.SetVector(s_prop_region_text, propRegion);
					Vector2 endpoint = new Vector2(paras._3d_endpoint.x * meshSize.x + min.x, paras._3d_endpoint.y * meshSize.y + min.y);
					float d1 = Vector2.Distance(new Vector2(min.x, min.y), endpoint);
					float d2 = Vector2.Distance(new Vector2(min.x, max.y), endpoint);
					float d3 = Vector2.Distance(new Vector2(max.x, max.y), endpoint);
					float d4 = Vector2.Distance(new Vector2(max.x, max.y), endpoint);
					float pxs = Mathf.Ceil(Mathf.Max(Mathf.Max(d1, d2), Mathf.Max(d3, d4)) * paras._3d_length);
					s_material_draw_3d_perspective.SetVector(s_prop_3d_endpoint, new Vector4(endpoint.x, endpoint.y, paras._3d_length, pxs * 2f));
					s_material_draw_3d_perspective.SetColor(s_prop_3d_faded, paras._3d_faded);
					Graphics.Blit(drt, b3d, s_material_draw_3d_perspective);
				}
			} else {
				Graphics.Blit(brt, b3d);
			}

			s_material_text_mesh.SetTexture(s_prop_text_pattern, null);

			desc.sRGB = false;
			RenderTexture trt1 = RenderTexture.GetTemporary(desc);
			RenderTexture trt2 = RenderTexture.GetTemporary(desc);

			// draw offsets and colors
			Vector4 shadowMainOffset = new Vector4(paras._shadow_offset.x * _w, paras._shadow_offset.y * _h);
			if (paras._shadow_intensity > 0f && paras._shadow_extend > 0f) {
				Vector2 ex3 = new Vector2(paras._shadow_extend * _w, paras._shadow_extend * _h);
				Vector2 ex0 = -ex3;
				Vector2 ex1 = Vector2.Lerp(ex0, ex3, 1f / 3f);
				Vector2 ex2 = (ex1 + ex3) * 0.5f;
				s_temp_offsets1[0] = new Vector4(ex0.x, ex0.y) * 0.8f + shadowMainOffset;
				s_temp_offsets1[1] = new Vector4(ex1.x * 0.8f, ex0.y) + shadowMainOffset;
				s_temp_offsets1[2] = new Vector4(ex2.x * 0.8f, ex0.y) + shadowMainOffset;
				s_temp_offsets1[3] = new Vector4(ex3.x, ex0.y) * 0.8f + shadowMainOffset;
				s_temp_offsets1[4] = new Vector4(ex0.x, ex1.y * 0.8f) + shadowMainOffset;
				s_temp_offsets1[5] = new Vector4(ex1.x, ex1.y) + shadowMainOffset;
				s_temp_offsets1[6] = new Vector4(ex2.x, ex1.y) + shadowMainOffset;
				s_temp_offsets1[7] = new Vector4(ex3.x, ex1.y * 0.8f) + shadowMainOffset;
				s_temp_offsets1[8] = new Vector4(ex0.x, ex2.y * 0.8f) + shadowMainOffset;
				s_temp_offsets1[9] = new Vector4(ex1.x, ex2.y) + shadowMainOffset;
				s_temp_offsets1[10] = new Vector4(ex2.x, ex2.y) + shadowMainOffset;
				s_temp_offsets1[11] = new Vector4(ex3.x, ex2.y * 0.8f) + shadowMainOffset;
				s_temp_offsets1[12] = new Vector4(ex0.x, ex3.y) * 0.8f + shadowMainOffset;
				s_temp_offsets1[13] = new Vector4(ex1.x * 0.8f, ex3.y) + shadowMainOffset;
				s_temp_offsets1[14] = new Vector4(ex2.x * 0.8f, ex3.y) + shadowMainOffset;
				s_temp_offsets1[15] = new Vector4(ex3.x, ex3.y) * 0.8f + shadowMainOffset;
				s_material_draw_offset.SetInt(s_prop_offsets1_count, 16);
			} else {
				s_temp_offsets1[0] = shadowMainOffset;
				s_material_draw_offset.SetInt(s_prop_offsets1_count, 1);
			}
			if (paras._glow_intensity != 0f && paras._glow_extend > 0f) {
				Vector2 ex3 = new Vector2(paras._glow_extend * _w, paras._glow_extend * _h);
				Vector2 ex0 = -ex3;
				Vector2 ex1 = Vector2.Lerp(ex0, ex3, 1f / 3f);
				Vector2 ex2 = (ex1 + ex3) * 0.5f;
				s_temp_offsets2[0] = new Vector4(ex0.x, ex0.y) * 0.8f;
				s_temp_offsets2[1] = new Vector4(ex1.x * 0.8f, ex0.y);
				s_temp_offsets2[2] = new Vector4(ex2.x * 0.8f, ex0.y);
				s_temp_offsets2[3] = new Vector4(ex3.x, ex0.y) * 0.8f;
				s_temp_offsets2[4] = new Vector4(ex0.x, ex1.y * 0.8f);
				s_temp_offsets2[5] = new Vector4(ex1.x, ex1.y);
				s_temp_offsets2[6] = new Vector4(ex2.x, ex1.y);
				s_temp_offsets2[7] = new Vector4(ex3.x, ex1.y * 0.8f);
				s_temp_offsets2[8] = new Vector4(ex0.x, ex2.y * 0.8f);
				s_temp_offsets2[9] = new Vector4(ex1.x, ex2.y);
				s_temp_offsets2[10] = new Vector4(ex2.x, ex2.y);
				s_temp_offsets2[11] = new Vector4(ex3.x, ex2.y * 0.8f);
				s_temp_offsets2[12] = new Vector4(ex0.x, ex3.y) * 0.8f;
				s_temp_offsets2[13] = new Vector4(ex1.x * 0.8f, ex3.y);
				s_temp_offsets2[14] = new Vector4(ex2.x * 0.8f, ex3.y);
				s_temp_offsets2[15] = new Vector4(ex3.x, ex3.y) * 0.8f;
				s_material_draw_offset.SetInt(s_prop_offsets2_count, 16);
			} else {
				s_temp_offsets2[0] = Vector2.zero;
				s_material_draw_offset.SetInt(s_prop_offsets2_count, 1);
			}
			for (int i = s_temp_offsets.Length - 1; i >= 0; i--) {
				s_temp_offsets[i] = GetVector4(s_temp_offsets1[i], s_temp_offsets2[i]);
			}
			s_material_draw_offset.SetVectorArray(s_prop_offsets, s_temp_offsets);
			s_material_draw_offset.SetTexture(s_prop_txt_3d_tex, b3d);
			Graphics.Blit(brt, trt1, s_material_draw_offset);
			if (paras._outline > 0f) {
				s_material_draw_blurdir.SetFloat(s_prop_blur_color_level_min, 0f);
				float fade = 1f;
				float outline = paras._outline;
				if (outline < 0.75f) {
					fade *= outline / 0.75f;
					outline = 0.75f;
				}
				float outlineblend = (1.5f * outline + 1f) / (outline + 1f);
				float tc = Mathf.Pow(outline * 0.7f, 4f);
				float outlineclip = (0.04f * tc + 1f) / (tc + 1f);
				s_material_draw_blurdir.SetVector(s_prop_blur_color_mask, new Vector4(outlineblend, 0f, 0f, 0f)); // 原，外，内，无
				float soft = Mathf.Min(Mathf.Pow(outline, 2f), Mathf.Pow(Mathf.Abs(Mathf.Max(outline, 1) - 1f), 0.25f) + 1f);
				float outline_blur = (outline - soft * 0.8f) * 0.7f;
				s_material_draw_blurdir.SetVector(s_prop_blur_dir, new Vector4(outline_blur * _w, 0f));
				Graphics.Blit(trt1, trt2, s_material_draw_blurdir);
				s_material_draw_blurdir.SetVector(s_prop_blur_dir, new Vector4(0f, outline_blur * _h));
				Graphics.Blit(trt2, trt1, s_material_draw_blurdir);
				outline_blur *= half_square_2;
				s_material_draw_blurdir.SetVector(s_prop_blur_dir, new Vector4(outline_blur * _w, outline_blur * _h));
				Graphics.Blit(trt1, trt2, s_material_draw_blurdir);
				s_material_draw_blurdir.SetVector(s_prop_blur_dir, new Vector4(outline_blur * _w, -outline_blur * _h));
				s_material_draw_blurdir.SetFloat(s_prop_blur_color_level_min, outlineclip);
				s_material_draw_blurdir.SetVector(s_prop_blur_color_mask, new Vector4(200f, 0f, 0f, 0f)); // 原，外，内，无
				Graphics.Blit(trt2, trt1, s_material_draw_blurdir);
				s_material_draw_blurdir.SetFloat(s_prop_blur_color_level_min, 0f);
				s_material_draw_blurdir.SetVector(s_prop_blur_color_mask, new Vector4(1f, 0f, 0f, 0f)); // 原，外，内，无
				s_material_draw_blurdir.SetVector(s_prop_blur_dir, new Vector4(soft * _w, soft * _h));
				Graphics.Blit(trt1, trt2, s_material_draw_blurdir);
				s_material_draw_blurdir.SetVector(s_prop_blur_dir, new Vector4(soft * _w, -soft * _h));
				Graphics.Blit(trt2, trt1, s_material_draw_blurdir);
				Rect r = new Rect(extend.z - outline + min.x, extend.w - outline + min.y,
					meshSize.x + outline + outline, meshSize.y + outline + outline);
				SetMaterialFxColor(s_material_draw_blend, FxColorWithFade.Get(paras._outline_color, fade), r, s_prop_fx_outline_gradient_direction,
					s_prop_fx_outline_gradient_alpha_keys_count, s_prop_fx_outline_gradient_alpha_keys,
					s_prop_fx_outline_gradient_color_keys_count, s_prop_fx_outline_gradient_color_keys,
					s_prop_fx_outline_texture);
			} else {
				ClearMaterialFxColor(s_material_draw_blend, s_prop_fx_outline_gradient_direction,
					s_prop_fx_outline_gradient_alpha_keys_count, s_prop_fx_outline_gradient_alpha_keys,
					s_prop_fx_outline_gradient_color_keys_count, s_prop_fx_outline_gradient_color_keys,
					s_prop_fx_outline_texture);
			}

			if (paras._shadow_intensity > 0f) {
				float intensity = paras._shadow_intensity > 1f ? Mathf.Pow(paras._shadow_intensity, 0.25f) : 1f;
				s_material_draw_blurdir.SetFloat(s_prop_blur_color_level_min, 0f);
				s_material_draw_blurdir.SetVector(s_prop_blur_color_mask, new Vector4(0f, intensity, 0f, 0f)); // 原，外，内，无
				float rad = Mathf.Deg2Rad * paras._shadow_blur_angle;
				float cos = Mathf.Cos(rad);
				float sin = Mathf.Sin(rad);
				if (paras._shadow_blur.x != 0f) {
					Vector2 blur = new Vector2(paras._shadow_blur.x * cos, paras._shadow_blur.x * sin);
					s_material_draw_blurdir.SetVector(s_prop_blur_dir, new Vector4(blur.x * 0.1f * _w, blur.y * 0.1f * _h));
					Graphics.Blit(trt1, trt2, s_material_draw_blurdir);
					s_material_draw_blurdir.SetVector(s_prop_blur_dir, new Vector4(blur.x * 0.2f * _w, blur.y * 0.2f * _h));
					Graphics.Blit(trt2, trt1, s_material_draw_blurdir);
					s_material_draw_blurdir.SetVector(s_prop_blur_dir, new Vector4(blur.x * 0.5f * _w, blur.y * 0.5f * _h));
					Graphics.Blit(trt1, trt2, s_material_draw_blurdir);
					s_material_draw_blurdir.SetVector(s_prop_blur_dir, new Vector4(blur.x * _w, blur.y * _h));
					Graphics.Blit(trt2, trt1, s_material_draw_blurdir);
				}
				if (paras._shadow_blur.y != 0f) {
					Vector2 blur = new Vector2(-paras._shadow_blur.y * sin, paras._shadow_blur.y * cos);
					s_material_draw_blurdir.SetVector(s_prop_blur_dir, new Vector4(blur.x * 0.1f * _w, blur.y * 0.1f * _h));
					Graphics.Blit(trt1, trt2, s_material_draw_blurdir);
					s_material_draw_blurdir.SetVector(s_prop_blur_dir, new Vector4(blur.x * 0.2f * _w, blur.y * 0.2f * _h));
					Graphics.Blit(trt2, trt1, s_material_draw_blurdir);
					s_material_draw_blurdir.SetVector(s_prop_blur_dir, new Vector4(blur.x * 0.5f * _w, blur.y * 0.5f * _h));
					Graphics.Blit(trt1, trt2, s_material_draw_blurdir);
					s_material_draw_blurdir.SetVector(s_prop_blur_dir, new Vector4(blur.x * _w, blur.y * _h));
					Graphics.Blit(trt2, trt1, s_material_draw_blurdir);
				}
				Rect r = new Rect(extend.z + s_min.x, extend.w + s_min.y, meshSize.x + s_max.x - s_min.x, meshSize.y + s_max.y - s_min.y);
				SetMaterialFxColor(s_material_draw_blend, FxColorWithFade.Get(paras._shadow_color), r, s_prop_fx_outer_gradient_direction,
					s_prop_fx_outer_gradient_alpha_keys_count, s_prop_fx_outer_gradient_alpha_keys,
					s_prop_fx_outer_gradient_color_keys_count, s_prop_fx_outer_gradient_color_keys,
					s_prop_fx_outer_texture);
			} else {
				ClearMaterialFxColor(s_material_draw_blend, s_prop_fx_outer_gradient_direction,
					s_prop_fx_outer_gradient_alpha_keys_count, s_prop_fx_outer_gradient_alpha_keys,
					s_prop_fx_outer_gradient_color_keys_count, s_prop_fx_outer_gradient_color_keys,
					s_prop_fx_outer_texture);
			}
			if (paras._glow_intensity != 0f) {
				float abs = Mathf.Abs(paras._glow_intensity);
				float intensity = Mathf.Sign(paras._glow_intensity) * (abs > 1f ? Mathf.Pow(abs, 0.25f) : 1f);
				s_material_draw_blurdir.SetFloat(s_prop_blur_color_level_min, 0f);
				s_material_draw_blurdir.SetVector(s_prop_blur_color_mask, new Vector4(0f, 0f, 0f, intensity)); // 原，外，内，无
				float rad = Mathf.Deg2Rad * paras._glow_blur_angle;
				float cos = Mathf.Cos(rad);
				float sin = Mathf.Sin(rad);
				if (paras._glow_blur.x != 0f) {
					Vector2 blur = new Vector2(paras._glow_blur.x * cos, paras._glow_blur.x * sin);
					s_material_draw_blurdir.SetVector(s_prop_blur_dir, new Vector4(blur.x * 0.1f * _w, blur.y * 0.1f * _h));
					Graphics.Blit(trt1, trt2, s_material_draw_blurdir);
					s_material_draw_blurdir.SetVector(s_prop_blur_dir, new Vector4(blur.x * 0.2f * _w, blur.y * 0.2f * _h));
					Graphics.Blit(trt2, trt1, s_material_draw_blurdir);
					s_material_draw_blurdir.SetVector(s_prop_blur_dir, new Vector4(blur.x * 0.5f * _w, blur.y * 0.5f * _h));
					Graphics.Blit(trt1, trt2, s_material_draw_blurdir);
					s_material_draw_blurdir.SetVector(s_prop_blur_dir, new Vector4(blur.x * _w, blur.y * _h));
					Graphics.Blit(trt2, trt1, s_material_draw_blurdir);
				}
				if (paras._glow_blur.y != 0f) {
					Vector2 blur = new Vector2(-paras._glow_blur.y * sin, paras._glow_blur.y * cos);
					s_material_draw_blurdir.SetVector(s_prop_blur_dir, new Vector4(blur.x * 0.1f * _w, blur.y * 0.1f * _h));
					Graphics.Blit(trt1, trt2, s_material_draw_blurdir);
					s_material_draw_blurdir.SetVector(s_prop_blur_dir, new Vector4(blur.x * 0.2f * _w, blur.y * 0.2f * _h));
					Graphics.Blit(trt2, trt1, s_material_draw_blurdir);
					s_material_draw_blurdir.SetVector(s_prop_blur_dir, new Vector4(blur.x * 0.5f * _w, blur.y * 0.5f * _h));
					Graphics.Blit(trt1, trt2, s_material_draw_blurdir);
					s_material_draw_blurdir.SetVector(s_prop_blur_dir, new Vector4(blur.x * _w, blur.y * _h));
					Graphics.Blit(trt2, trt1, s_material_draw_blurdir);
				}
				SetMaterialFxColor(s_material_draw_blend, FxColorWithFade.Get(paras._glow_color), new Rect(0f, 0f, size.x, size.y), s_prop_fx_glow_gradient_direction,
					s_prop_fx_glow_gradient_alpha_keys_count, s_prop_fx_glow_gradient_alpha_keys,
					s_prop_fx_glow_gradient_color_keys_count, s_prop_fx_glow_gradient_color_keys,
					s_prop_fx_glow_texture);
			} else {
				ClearMaterialFxColor(s_material_draw_blend, s_prop_fx_glow_gradient_direction,
					s_prop_fx_glow_gradient_alpha_keys_count, s_prop_fx_glow_gradient_alpha_keys,
					s_prop_fx_glow_gradient_color_keys_count, s_prop_fx_glow_gradient_color_keys,
					s_prop_fx_glow_texture);
			}
			float innerAlphaBlend = 0f;
			float innerMultiply = 0f;
			float emboss = 0f;
			Vector2 embossDir = Vector2.zero;
			if (paras._inner_blur_angle != 0f) {
				float rad = Mathf.Deg2Rad * paras._inner_blur_angle;
				float cos = Mathf.Cos(rad);
				float sin = Mathf.Sin(rad);
				s_material_draw_extend.SetVector(s_prop_extend_color_mask, new Vector4(0f, 0f, 1f, 0f));
				if (paras._inner_blur_angle > 0f) {
					s_material_draw_extend.SetInt(s_prop_extend_count, 12);
					int n = Mathf.CeilToInt(paras._inner_blur);
					float _blur = 1f / paras._inner_blur;
					for (int i = 0; i < n; i++) {
						float val1 = 1f - i * _blur;
						float val2 = 1f - (i + 1) * _blur;
						float t = val1 / (val1 - val2);
						float val = (val1 + Mathf.Max(val2, 0f)) * 0.5f * Mathf.Min(t, 1f);
						s_temp_extend_offsets[0] = new Vector4(_w, 0f, 0.0005f, val);
						s_temp_extend_offsets[1] = new Vector4(_w * 0.87f, _h * 0.5f, 0.0005f, val);
						s_temp_extend_offsets[2] = new Vector4(_w * 0.5f, _h * 0.87f, 0.0005f, val);
						s_temp_extend_offsets[3] = new Vector4(0f, _h, 0.0005f, val);
						s_temp_extend_offsets[4] = new Vector4(-_w * 0.5f, _h * 0.87f, 0.0005f, val);
						s_temp_extend_offsets[5] = new Vector4(-_w * 0.87f, _h * 0.5f, 0.0005f, val);
						s_temp_extend_offsets[6] = new Vector4(-_w, 0f, 0.0005f, val);
						s_temp_extend_offsets[7] = new Vector4(-_w * 0.87f, -_h * 0.5f, 0.0005f, val);
						s_temp_extend_offsets[8] = new Vector4(-_w * 0.5f, -_h * 0.87f, 0.0005f, val);
						s_temp_extend_offsets[9] = new Vector4(0f, -_h, 0.0005f, val);
						s_temp_extend_offsets[10] = new Vector4(_w * 0.5f, -_h * 0.87f, 0.0005f, val);
						s_temp_extend_offsets[11] = new Vector4(_w * 0.87f, -_h * 0.5f, 0.0005f, val);
						s_material_draw_extend.SetVectorArray(s_prop_offsets, s_temp_extend_offsets);
						Graphics.Blit(trt1, trt2, s_material_draw_extend);
						(trt1, trt2) = (trt2, trt1);
					}
				} else {
					Vector2 dir = new Vector2(cos * _w, sin * _h);
					float blur = Mathf.Min(paras._inner_blur, 32f);
					int n = Mathf.CeilToInt(blur);
					float _blur = 1f / blur;
					s_material_draw_extend.SetInt(s_prop_extend_count, n);
					for (int i = 0; i < n; i++) {
						float val1 = 1f - i * _blur;
						float val2 = 1f - (i + 1) * _blur;
						float t = val1 / (val1 - val2);
						float val = (val1 + Mathf.Max(val2, 0f)) * 0.5f * Mathf.Min(t, 1f);
						s_temp_extend_offsets[i] = new Vector4(dir.x * (i + 1), dir.y * (i + 1), 0.0005f, val);
					}
					s_material_draw_extend.SetVectorArray(s_prop_offsets, s_temp_extend_offsets);
					Graphics.Blit(trt1, trt2, s_material_draw_extend);
					s_material_draw_applymask.SetTexture(s_prop_txt_tex, brt);
					s_material_draw_applymask.SetVector(s_prop_channel_mask, new Vector4(0f, 0f, 1f, 0f));
					Graphics.Blit(trt2, trt1, s_material_draw_applymask);
				}
				s_material_draw_blurdir.SetFloat(s_prop_blur_color_level_min, 0f);
				s_material_draw_blurdir.SetVector(s_prop_blur_color_mask, new Vector4(0f, 0f, 1f, 0f)); // 原，外，内，无
				s_material_draw_blurdir.SetVector(s_prop_blur_dir, new Vector4(_w, _h));
				Graphics.Blit(trt1, trt2, s_material_draw_blurdir);
				s_material_draw_blurdir.SetVector(s_prop_blur_dir, new Vector4(_w, -_h));
				Graphics.Blit(trt2, trt1, s_material_draw_blurdir);
				if (paras._inner_blur_angle > 0f && paras._inner_blur_angle <= 360f) {
					s_material_draw_blurdir.SetVector(s_prop_blur_dir, new Vector4(_w + _w, _h + _h));
					Graphics.Blit(trt1, trt2, s_material_draw_blurdir);
					s_material_draw_blurdir.SetVector(s_prop_blur_dir, new Vector4(_w + _w, -_h - _h));
					Graphics.Blit(trt2, trt1, s_material_draw_blurdir);
					emboss = paras._inner_multiply;
					embossDir = new Vector2(cos, sin);
				} else {
					innerAlphaBlend = paras._inner_multiply == 0f ? 1f : 0f;
					innerMultiply = paras._inner_multiply;
				}
				SetMaterialFxColor(s_material_draw_blend, FxColorWithFade.Get(paras._inner_color), new Rect(extend.z, extend.w, meshSize.x, meshSize.y), s_prop_fx_inner_gradient_direction,
					s_prop_fx_inner_gradient_alpha_keys_count, s_prop_fx_inner_gradient_alpha_keys,
					s_prop_fx_inner_gradient_color_keys_count, s_prop_fx_inner_gradient_color_keys,
					s_prop_fx_inner_texture);
			} else {
				ClearMaterialFxColor(s_material_draw_blend, s_prop_fx_inner_gradient_direction,
					s_prop_fx_inner_gradient_alpha_keys_count, s_prop_fx_inner_gradient_alpha_keys,
					s_prop_fx_inner_gradient_color_keys_count, s_prop_fx_inner_gradient_color_keys,
					s_prop_fx_inner_texture);
			}
			s_material_draw_blend.SetVector(s_prop_emboss, new Vector4(emboss, paras._inner_blur, embossDir.x, embossDir.y));
			s_material_draw_blend.SetColor(s_prop_emboss_light_color, paras._emboss_light);
			float innerpower = paras._inner_blur_fix < 0f ? 1f / (1f - paras._inner_blur_fix) : (paras._inner_blur_fix + 1f);
			s_material_draw_blend.SetVector(s_prop_fx_blend, new Vector4(paras._shadow_intensity, innerpower, innerAlphaBlend, innerMultiply));
			float glowpower = paras._glow_blur_fix < 0f ? 1f / (1f - paras._glow_blur_fix) : (paras._glow_blur_fix + 1f);
			s_material_draw_blend.SetVector(s_prop_glow_blend, new Vector4(glowpower, Mathf.Clamp(paras._glow_intensity, -1f, 1f), paras._text_blend, paras._outline_fill_text));
			s_material_draw_blend.SetTexture(s_prop_txt_tex, brt);
			s_material_draw_blend.SetTexture(s_prop_txt_3d_tex, b3d);
			desc.sRGB = true;
			RenderTexture rt = allocType == eRenderTextureAllocType.Temporary ? RenderTexture.GetTemporary(desc) : new RenderTexture(desc);
			Graphics.Blit(trt1, rt, s_material_draw_blend);

			RenderTexture.ReleaseTemporary(brt);
			RenderTexture.ReleaseTemporary(trt1);
			RenderTexture.ReleaseTemporary(trt2);

			return rt;
		}

		private struct FxColorWithFade {
			public FxColor color;
			public float fade;
			public static FxColorWithFade Get(FxColor color) {
				return new FxColorWithFade() { color = color, fade = 1 };
			}
			public static FxColorWithFade Get(FxColor color, float fade) {
				return new FxColorWithFade() { color = color, fade = fade };
			}
		}

		private static bool SetMaterialFxColor(Material material, FxColorWithFade color_with_fade, Rect rect, int gradient_direction,
				int gradient_alpha_keys_count, int gradient_alpha_keys, int gradient_color_keys_count, int gradient_color_keys, int texture) {
			bool ret = true;
			FxColor color = color_with_fade.color;
			switch (color._type) {
				case eFxColorType.Gradient:
					if (color._gradient == null) {
						ClearMaterialFxColor(material, gradient_direction,
							gradient_alpha_keys_count, gradient_alpha_keys, gradient_color_keys_count, gradient_color_keys, texture);
						ret = false;
						break;
					}
					GradientAlphaKey[] alphaKeys = color._gradient.alphaKeys;
					GradientColorKey[] colorKeys = color._gradient.colorKeys;
					for (int i = 0; i < alphaKeys.Length; i++) {
						GradientAlphaKey key = alphaKeys[i];
						float keyalpha = key.alpha * color_with_fade.fade;
						s_temp_alpha_keys[i] = new Vector4(keyalpha, keyalpha, keyalpha, key.time);
					}
					for (int i = 0; i < colorKeys.Length; i++) {
						GradientColorKey key = colorKeys[i];
						s_temp_color_keys[i] = new Vector4(key.color.r, key.color.g, key.color.b, key.time);
					}
					float rad = Mathf.Deg2Rad * color._gradient_angle;
					Vector2 gradientDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
					float gmin = float.MaxValue;
					float gmax = float.MinValue;
					Vector2 min = rect.min;
					Vector2 max = rect.max;
					float glb = Vector2.Dot(gradientDir, new Vector2(min.x, min.y));
					float glt = Vector2.Dot(gradientDir, new Vector2(min.x, max.y));
					float grt = Vector2.Dot(gradientDir, new Vector2(max.x, max.y));
					float grb = Vector2.Dot(gradientDir, new Vector2(max.x, min.y));
					gmin = Mathf.Min(gmin, glb);
					gmin = Mathf.Min(gmin, glt);
					gmin = Mathf.Min(gmin, grt);
					gmin = Mathf.Min(gmin, grb);
					gmax = Mathf.Max(gmax, glb);
					gmax = Mathf.Max(gmax, glt);
					gmax = Mathf.Max(gmax, grt);
					gmax = Mathf.Max(gmax, grb);
					material.SetVector(gradient_direction, new Vector4(gradientDir.x, gradientDir.y, gmin, gmax));
					material.SetInt(gradient_alpha_keys_count, alphaKeys.Length);
					material.SetVectorArray(gradient_alpha_keys, s_temp_alpha_keys);
					material.SetInt(gradient_color_keys_count, colorKeys.Length);
					material.SetVectorArray(gradient_color_keys, s_temp_color_keys);
					break;
				default:
					float alpha = color._color.a * color_with_fade.fade;
					s_temp_alpha_keys[0] = new Vector4(alpha, alpha, alpha, 1f);
					s_temp_color_keys[0] = new Vector4(color._color.r, color._color.g, color._color.b, 1f);
					material.SetInt(gradient_alpha_keys_count, 1);
					material.SetVectorArray(gradient_alpha_keys, s_temp_alpha_keys);
					material.SetInt(gradient_color_keys_count, 1);
					material.SetVectorArray(gradient_color_keys, s_temp_color_keys);
					material.SetVector(gradient_direction, new Vector4(0f, 0f, 0f, 1f));
					break;
			}
			Texture tex = color._texture;
			if (tex == null || tex.Equals(null)) {
				tex = GetWhiteTexture();
			}
			material.SetTexture(texture, tex);
			material.SetTextureScale(texture, new Vector2(color._tiling_offset.x, color._tiling_offset.y));
			material.SetTextureOffset(texture, new Vector2(color._tiling_offset.z, color._tiling_offset.w));
			return ret;
		}

		private static void ClearMaterialFxColor(Material material, int gradient_direction,
				int gradient_alpha_keys_count, int gradient_alpha_keys, int gradient_color_keys_count, int gradient_color_keys, int texture) {
			s_temp_alpha_keys[0] = new Vector4(0f, 0f, 0f, 1f);
			s_temp_color_keys[0] = new Vector4(0f, 0f, 0f, 1f);
			material.SetInt(gradient_alpha_keys_count, 1);
			material.SetVectorArray(gradient_alpha_keys, s_temp_alpha_keys);
			material.SetInt(gradient_color_keys_count, 1);
			material.SetVectorArray(gradient_color_keys, s_temp_color_keys);
			material.SetVector(gradient_direction, new Vector4(0f, 0f, 0f, 1f));
			material.SetTexture(texture, null);
		}

		private static Vector4 GetVector4(Vector2 xy, Vector2 zw) {
			return new Vector4(xy.x, xy.y, zw.x, zw.y);
		}

		private static Vector4 GetVector4(Vector2 xy, float z, float w) {
			return new Vector4(xy.x, xy.y, z, w);
		}

		private static Texture2D s_white_texture;

		private static Texture2D GetWhiteTexture() {
			if (s_white_texture == null || s_white_texture.Equals(null)) {
				s_white_texture = new Texture2D(2, 2, TextureFormat.RGBA32, false, false);
				s_white_texture.SetPixel(0, 0, Color.white);
				s_white_texture.SetPixel(0, 1, Color.white);
				s_white_texture.SetPixel(1, 0, Color.white);
				s_white_texture.SetPixel(1, 1, Color.white);
				s_white_texture.Apply(false, true);
			}
			return s_white_texture;
		}

		private static string empty_chars = " \t\r\n";

		private struct CharData {
			public int index;
			public char chr;
			public int size;
			public FontStyle style;
			public int GetKey() {
				return size * 1000 + index * 10 + (int)style;
			}
			public static void GetSizeAndStyle(int key, out int index, out int size, out FontStyle style) {
				size = key / 1000;
				key -= size * 1000;
				index = key / 10;
				style = (FontStyle)(key - index * 10);
			}
		}

		private struct StringFindData {
			public int search_index;
			public int match_index;
		}

		private static Queue<StringFindData> s_str_find_todo = new Queue<StringFindData>(8);

		private struct StringSegment {
			public string content;
			public int index;
			public int length;
			public char this[int i] { get { return content[index + i]; } }
			public bool IsNullOrEmpty() { return content == null || length <= 0; }
			public bool Equals(string str) {
				if (str == null) { return content == null; }
				if (content == null) { return false; }
				if (str.Length != length) { return false; }
				for (int i = 0; i < length; i++) {
					if (content[index + i] != str[i]) { return false; }
				}
				return true;
			}
			public bool Equals(string str, int si, int len) {
				if (str == null) { return content == null; }
				if (content == null) { return false; }
				if (len != length) { return false; }
				if (si < 0 || si + len >= str.Length) { return false; }
				for (int i = 0; i < length; i++) {
					if (content[index + i] != str[si + i]) { return false; }
				}
				return true;
			}
			public int IndexOf(string str) {
				if (content == null || length <= 0) { return -1; }
				if (string.IsNullOrEmpty(str)) { return -1; }
				int sl = str.Length;
				int ei = index + length + sl - 1;
				s_str_find_todo.Clear();
				s_str_find_todo.Enqueue(new StringFindData() { search_index = index, match_index = 0 });
				while (s_str_find_todo.Count > 0) {
					StringFindData todo = s_str_find_todo.Dequeue();
					if (content[todo.search_index + todo.match_index] == str[todo.match_index]) {
						int matchlen = todo.match_index + 1;
						if (matchlen >= sl) { return todo.search_index - index; }
						s_str_find_todo.Enqueue(new StringFindData() { search_index = todo.search_index, match_index = matchlen });
					}
					if (todo.search_index < ei) {
						s_str_find_todo.Enqueue(new StringFindData() { search_index = todo.search_index + 1, match_index = 0 });
					}
				}
				return -1;
			}
			public string Substring(int si, int len) {
				if (content == null) { return null; }
				if (len > length) { return null; }
				return content.Substring(index + si, len);
			}
			public int Split(char chr, List<StringSegment> splits) {
				if (content == null) { return 0; }
				int ei = index + length;
				int pi = index;
				int ret = 1;
				for (int i = index; i < ei; i++) {
					if (content[i] == chr) {
						splits.Add(StringSegment.Get(content, pi, i - pi));
						pi = i + 1;
						ret++;
					}
				}
				splits.Add(StringSegment.Get(content, pi, ei - pi));
				return ret;
			}
			public StringSegment Trim() {
				int si = index;
				int ei = index + length;
				while (si < ei && empty_chars.IndexOf(content[si]) >= 0) { si++; }
				while (si < ei && empty_chars.IndexOf(content[ei]) >= 0) { ei--; }
				return StringSegment.Get(content, si, ei - si);
			}
			public bool HexToInt(int si, int len, out int ret) {
				ret = 0;
				if (content == null || si < 0 || len > length) {
					return false;
				}
				for (int i = 0; i < len; i++) {
					char chr = content[index + si + i];
					int temp = 0;
					if (chr >= '0' && chr <= '9') {
						temp = chr - '0';
					} else if (chr >= 'A' && chr <= 'F') {
						temp = chr - 'A' + 10;
					} else if (chr >= 'a' && chr <= 'f') {
						temp = chr - 'a' + 10;
					} else {
						ret = 0;
						return false;
					}
					ret = (ret << 4) | temp;
				}
				return true;
			}
			public override string ToString() {
				if (content == null) { return null; }
				return content.Substring(index, length);
			}
			public static StringSegment Empty { get { return new StringSegment(); } }
			public static StringSegment Get(string content, int index, int length) {
				StringSegment ret = new StringSegment();
				ret.content = content;
				ret.index = index;
				ret.length = length;
				return ret;
			}
		}

	}

}

using System;
using UnityEngine;

namespace GreatClock.Common.UI {

	public class FontDynamic : IFontInternal {

		private Font mFont;

		public FontDynamic(Font font) {
			if (font == null) {
				throw new ArgumentNullException(nameof(font));
			}
			mFont = font;
		}

		public Texture texture { get { return mFont?.material.mainTexture; } }

		public bool GetCharacterInfo(char chr, out CharacterInfo info, int size, FontStyle style) {
			if (mFont == null) { info = new CharacterInfo(); return false; }
			return mFont.GetCharacterInfo(chr, out info, size, style);
		}

		public void RequestCharactersInTexture(string characters, int size, FontStyle style) {
			if (mFont == null) { return; }
			mFont.RequestCharactersInTexture(characters, size, style);
		}

		public void Reset() { }

	}

}

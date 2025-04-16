using UnityEngine;

namespace GreatClock.Common.UI {

	public abstract class BaseFontAsset : ScriptableObject, IFontInternal {

		public abstract Texture texture { get; }

		public abstract bool GetCharacterInfo(char chr, out CharacterInfo info, int size, FontStyle style);

		public abstract void RequestCharactersInTexture(string characters, int size, FontStyle style);

		public abstract void Reset();

	}

}
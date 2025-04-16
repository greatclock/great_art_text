using UnityEngine;

namespace GreatClock.Common.UI {

	public interface IFontInternal {

		bool GetCharacterInfo(char chr, out CharacterInfo info, int size, FontStyle style);

		void RequestCharactersInTexture(string characters, int size, FontStyle style);

		Texture texture { get; }

		void Reset();

	}

}
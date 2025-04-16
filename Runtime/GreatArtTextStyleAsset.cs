using UnityEngine;

namespace GreatClock.Common.UI {

	[CreateAssetMenu(fileName = "NewGreatArtTextStyle", menuName = "Great Art Text/New Art Text Style", order = 500)]
	public class GreatArtTextStyleAsset : ScriptableObject {

		[SerializeField, HideInInspector]
		private GreatArtTextStyle m_Style = new GreatArtTextStyle();

		public GreatArtTextStyle style { get { return replacement == null || !Application.isPlaying ? m_Style : replacement.style; } }

		public GreatArtTextStyleAsset replacement { get; set; } = null;

	}

}

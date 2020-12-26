﻿using VkNet.Utils;

namespace VkNet.Enums.SafetyEnums
{
	/// <summary>
	/// Тип кнопки сообщений.
	/// Содержит "text"
	/// </summary>
	public class KeyboardButtonActionType : SafetyEnum<KeyboardButtonActionType>
	{
		/// <summary>
		/// Text
		/// </summary>
		[DefaultValue]
		public static readonly KeyboardButtonActionType Text = RegisterPossibleValue(value: "text");
	}
}

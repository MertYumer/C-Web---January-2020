﻿namespace SIS.HTTP.Extensions
{
    public class StringExtensions
    {
        public static string Capitalize(string text) 
            => char.ToUpper(text[0]) + text.Substring(1).ToLower();
    }
}

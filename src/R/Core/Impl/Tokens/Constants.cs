﻿using System;

namespace Microsoft.R.Core.Tokens
{
    public static class Constants
    {
        public static bool IsConstant(string candidate)
        {
            // R is case sensitive language
             return Array.BinarySearch<string>(_constants, candidate) >= 0;
        }

        public static string[] ConstantsList
        {
            get { return _constants; }
        }

        // must be sorted
        internal static string[] _constants = new string[]
        {
            "Inf",
            "letters",
            "month.abb",
            "month.name",
            "NA",
            "NA_character_",
            "NA_complex_",
            "NA_integer_",
            "NA_real_",
            "NaN",
            "NULL",
            "pi"
        };
    }
}

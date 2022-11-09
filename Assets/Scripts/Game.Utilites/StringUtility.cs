﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public static class StringUtility
{
    private static StringBuilder builder_;

    private static StringBuilder GetCachedBuilder()
    {
        if (builder_ == null)
        {
            builder_ = new StringBuilder();
        }
        else
        {
            builder_.Clear();
        }

        return builder_;
    }

    public static string SplitPascalCase(object obj)
    {
        return SplitPascalCase(obj.ToString());
    }

    public static string SplitPascalCase(string sourceString)
    {
        StringBuilder sb = GetCachedBuilder();
        foreach (char c in sourceString)
        {
            if (char.IsUpper(c))
            {
                sb.Append(" ");
            }

            sb.Append(c);
        }

        if (sourceString != null && sourceString.Length > 0 && char.IsUpper(sourceString[0]))
        {
            sb.Remove(0, 1);
        }

        return sb.ToString();
    }

    public static string ToScreamingCaps(string sourceString)
    {
        string split = Regex.Replace(sourceString, @"(?<=[A-Za-z])(?=[A-Z][a-z])|(?<=[a-z0-9])(?=[0-9]?[A-Z])", " ");
        split = ValidatePropertyName(split, "_");

        return split.ToUpper();
    }

    public static string GetCompositeString<T>(IEnumerable<T> collection, bool vertical = true, Func<T, string> getter = null, bool numerate = true)
    {
        if (collection == null) return string.Empty;

        return GetCompositeString(new List<T>(collection), vertical, getter, numerate);
    }

    public static string GetCompositeString<T>(List<T> items, bool vertical = true, Func<T, string> getter = null, bool numerate = true)
    {
        if (items == null || items.Count == 0) return string.Empty;

        StringBuilder builder = GetCachedBuilder();
        for (int i = 0; i < items.Count; i++)
        {
            T item = items[i];

            string value = 
                item == null ? "NULL" : 
                getter != null ?
                getter.Invoke(item) :
                item.ToString();

            string prefix = numerate ? $"{i + 1}. " : null;
            string next = vertical ?
                $"\n{prefix}{value}" :
                $"{prefix}{value} ";

            builder.Append(next);
        }

        return builder.ToString();
    }

    public static string[] StringToArrayOfStrings(string str, char delimiter = ',')
    {
        return str.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
    }

    public static int[] StringToArrayOfIntegers(string str, char delimiter = ',')
    {
        var parts = str.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
        return parts.Select(p => int.Parse(p)).ToArray();
    }

    public static string ValidatePropertyName(string name, string replacement = "")
    {
        return Regex.Replace(name, "^[^a-zA-Z_]+|[^a-zA-Z_0-9]+", replacement);
    }

    public static int StringToHash(string str)
    {
        int hash = 5381;

        if (string.IsNullOrEmpty(str))
            return hash;

        for (int i = 0; i < str.Length; i++)
            hash = ((hash << 5) + hash) + str[i]; /* hash * 33 + c */

        return hash;
    }
}
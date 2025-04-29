﻿using UnityEditor;

namespace Editor
{
    public static class ClearConsole
    {
        [MenuItem("Shortcuts/Clear Console")]
        public static void Clear()
        {
            var logEntries = System.Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");
            var clearMethod = logEntries?.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            clearMethod?.Invoke(null, null);
        }
    }
}
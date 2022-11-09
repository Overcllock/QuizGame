using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Game.Content.Main
{
    [CreateAssetMenu(menuName = "Content/Main/Settings", fileName = "MainSettings")]
    public class MainSettingsScrobject : BaseSettingsScrobject
    {
        private const RegexOptions _regexOptions = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.RightToLeft;
        
        public int minWordLength = 3;
        public int maxWordLength = 15;

        [Space]
        public int defaultAttemptsCount = 10;

        [Space]
        public List<string> wordsDatabase;
        
        [Space]
        [InlineButton(nameof(ExportWords), "Export", ShowIf = nameof(ValidateExportAsset))]
        public TextAsset wordsSourceAsset;
        
        private bool ValidateExportAsset()
        {
            if (wordsSourceAsset == null)
                return false;

            return true;
        }

        private void ExportWords()
        {
#if UNITY_EDITOR
            var assetPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), AssetDatabase.GetAssetPath(wordsSourceAsset));

            var text = string.Empty;

            try
            {
                text = File.ReadAllText(assetPath);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return;
            }

            wordsDatabase = new List<string>();
            
            if (text.IsNullOrEmpty())
                return;

            var tempDatabase = new Dictionary<int, string>();

            var matches = Regex.Matches(text, @"\w+", _regexOptions);
            foreach (Match match in matches)
            {
                var word = match.Value.ToLower();
                
                if (word.Length < minWordLength || word.Length > maxWordLength)
                    continue;
                
                var hash = StringUtility.StringToHash(word);
                if (tempDatabase.ContainsKey(hash))
                    continue;
                
                tempDatabase.Add(hash, word);
                wordsDatabase.Add(word);
            }
            
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
#endif
        }
    }
}
using System;
using System.Collections.Generic;
using DG.Tweening;
using Game.Content.UI;
using Game.Quiz;
using UnityEngine;

namespace Game.UI.Windows
{
    public class GameFieldWindow : UIWindow<GameFieldLayout, GameFieldWindowArgs>
    {
        public override string id => WindowType.GAME_FIELD_WINDOW;
        
        private QuizService _quizService;

        private Sequence _swapWordSequence;

        private bool _alphabetIsBlocked;

        //NOTE: use UI pool instead this
        private List<GameFieldItemWidget> _wordWigdets = new List<GameFieldItemWidget>();
        private List<GameFieldItemWidget> _alphabetWidgets = new List<GameFieldItemWidget>();

        protected override void OnInitialize()
        {
            ServiceLocator.Get(out _quizService);
        }

        protected override void OnShow(GameFieldWindowArgs args)
        {
            base.OnShow(args);
            
            _quizService.letterUsed += HandleLetterUsed;
            _quizService.wordGenerated += HandleWordGenerated;

            if (_layout != null)
            {
                CreateWordWidgets();
                CreateAlphabetWidgets();
                
                if (!_entry.useFader)
                {
                    _layout.rootObject.SetActive(true);
                }
            }
        }

        protected override void OnHide()
        {
            base.OnHide();

            _quizService.letterUsed -= HandleLetterUsed;
            _quizService.wordGenerated -= HandleWordGenerated;
            
            if (_layout != null && !_entry.useFader)
            {
                _layout.rootObject.SetActive(false);
            }
            
            foreach (var widget in _wordWigdets)
            {
                widget?.Dispose();
            }
            
            foreach (var widget in _alphabetWidgets)
            {
                widget?.Dispose();
            }
        }

        private void CreateWordWidgets()
        {
            _wordWigdets.Clear();

            var data = _quizService.GetData();
            var word = _quizService.GetWord(data.currentWordHash);

            for (int i = 0; i < word.Length; i++)
            {
                var letter = word[i];
                
                var widget = CreateWidget(_layout.wordItemTemplate, letter, false, false);

                _wordWigdets.Add(widget);
            }
        }

        private void HideWordWidgets()
        {
            foreach (var widget in _wordWigdets)
            {
                widget?.Hide();
            }
        }

        private void UpdateWordWidgets()
        {
            var data = _quizService.GetData();
            var word = _quizService.GetWord(data.currentWordHash);

            if (word.Length > _wordWigdets.Count)
            {
                for (int i = _wordWigdets.Count; i < word.Length; i++)
                {
                    var widget = CreateWidget(_layout.wordItemTemplate, Char.MinValue, false, false, false);
                    
                    _wordWigdets.Add(widget);
                }
            }
            else if (word.Length < _wordWigdets.Count)
            {
                for (int i = _wordWigdets.Count - 1; i >= word.Length; i--)
                {
                    var widget = _wordWigdets[i];
                    widget?.Dispose();
                    
                    _wordWigdets.RemoveAt(i);
                }
            }

            for (int i = 0; i < word.Length; i++)
            {
                var letter = word[i];
                var widget = _wordWigdets[i];
                
                widget.ForceSetActive(false);
                widget.SetLetter(letter);
                
                widget.Show();
            }
        }

        private void BlockAlphabet()
        {
            _alphabetIsBlocked = true;
        }
        
        private void UnblockAlphabet()
        {
            _alphabetIsBlocked = false;
        }

        private void ResetAlphabetWidgets()
        {
            foreach (var widget in _alphabetWidgets)
            {
                if (!widget.isActive)
                {
                    widget.Activate();
                }
            }
        }

        private void CreateAlphabetWidgets()
        {
            _alphabetWidgets.Clear();

            for (char letter = 'a'; letter <= 'z'; letter++)
            {
                var widget = CreateWidget(_layout.alphabetItemTemplate, letter, true, true);
                
                _alphabetWidgets.Add(widget);
            }

            //NOTE: 27th empty alphabet widget for symmetry
            {
                var widget = CreateWidget(_layout.alphabetItemTemplate, Char.MinValue, true, false);
                
                _alphabetWidgets.Add(widget);
            }
        }

        private GameFieldItemWidget CreateWidget(GameObject template, char letter, bool isActive, bool hasButton, bool show = true)
        {
            var layout = _layoutFactory.Create<GameFieldItemWidgetLayout>(template, template.transform.parent);
            var widget = new GameFieldItemWidget(layout, isActive);
                
            widget.SetLetter(letter);
            
            if (hasButton)
            {
                widget.clicked += HandleLetterClick;
            }

            return widget;
        }

        private void HandleLetterClick(char letter)
        {
            if (_alphabetIsBlocked)
                return;
            
            _quizService.UseLetter(letter);
        }
        
        private void HandleWordGenerated()
        {
            if (_layout == null)
                return;
            
            _swapWordSequence = GetSwapWordSequence();
            _swapWordSequence.Play();

            ResetAlphabetWidgets();
        }

        private void HandleLetterUsed(char letter)
        {
            if (_layout == null)
                return;
            
            foreach (var widget in _wordWigdets)
            {
                if (widget.GetLetter() == letter && !widget.isActive)
                {
                    widget.Activate();
                }
            }
            
            foreach (var widget in _alphabetWidgets)
            {
                if (widget.GetLetter() == letter && widget.isActive)
                {
                    widget.Deactivate();
                }
            }
        }

        private Sequence GetSwapWordSequence()
        {
            return DOTween.Sequence()
                .PrependCallback(BlockAlphabet)
                .PrependInterval(0.6f)
                .AppendCallback(HideWordWidgets)
                .AppendInterval(0.75f)
                .AppendCallback(UpdateWordWidgets)
                .AppendCallback(UnblockAlphabet)
                .SetAutoKill(false);
        }
    }

    public class GameFieldWindowArgs : IWindowArgs
    {
    }
}
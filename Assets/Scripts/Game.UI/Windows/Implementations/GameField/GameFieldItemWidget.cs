using System;
using DG.Tweening;
using Object = UnityEngine.Object;

namespace Game.UI.Windows
{
    public class GameFieldItemWidget : IDisposable
    {
        private GameFieldItemWidgetLayout _layout;

        private Sequence _activateSequence;
        private Sequence _deactivateSequence;
        private Sequence _showSequence;
        private Sequence _hideSequence;
        
        private char _letter;

        private bool _active;

        public bool isActive => _active;

        public Action<char> clicked;

        public GameFieldItemWidget(GameFieldItemWidgetLayout layout, bool active = true, bool show = true)
        {
            _layout = layout;
            _active = active;
            
            _layout.inactiveImage.SetActive(!active);
            _layout.activeImage.SetActive(active);

            if (_layout.button != null)
            {
                _layout.button.onClick.AddListener(HandleClick);
            }

            _layout.canvasGroup.alpha = 0f;
            _layout.rootObject.SetActive(true);

            if (show)
            {
                Show();
            }
        }

        public void SetLetter(char letter)
        {
            _letter = letter;
            
            _layout.text.text = letter.ToString();
        }

        public char GetLetter()
        {
            return _letter;
        }
        
        public void Dispose()
        {
            _activateSequence?.Kill();
            _activateSequence = null;
            
            _deactivateSequence?.Kill();
            _deactivateSequence = null;
            
            _showSequence?.Kill();
            _showSequence = null;
            
            _hideSequence?.Kill();
            _hideSequence = null;

            if (_layout != null && _layout.button != null)
            {
                _layout.button.onClick.RemoveAllListeners();
            }

            if (_layout != null)
            {
                Object.Destroy(_layout.rootObject);
            }
        }

        public void Activate()
        {
            _activateSequence = GetActivateSequence();
            _activateSequence.Play();

            _active = true;
        }

        public void Deactivate()
        {
            _deactivateSequence = GetDeactivateSequence();
            _deactivateSequence.Play();

            _active = false;
        }

        public void ForceSetActive(bool active)
        {
            _active = active;
            _layout.inactiveImage.SetActive(!active);
            _layout.activeImage.SetActive(active);
        }

        public void Show()
        {
            _showSequence = GetShowSequence();
            _showSequence.Play();
        }

        public void Hide()
        {
            if (_activateSequence != null && _activateSequence.IsPlaying())
            {
                _activateSequence.Kill();
                _activateSequence = null;
            }
            
            if (_deactivateSequence != null && _deactivateSequence.IsPlaying())
            {
                _deactivateSequence.Kill();
                _deactivateSequence = null;
            }
            
            _hideSequence = GetHideSequence();
            _hideSequence.Play();
        }

        private void ShowInactiveLayer()
        {
            _layout.inactiveImage.SetActive(true);
            _layout.activeImage.SetActive(false);
        }
        
        private void HideInactiveLayer()
        {
            _layout.inactiveImage.SetActive(false);
            _layout.activeImage.SetActive(true);
        }

        private void HandleClick()
        {
            clicked?.Invoke(_letter);
        }

        private Sequence GetActivateSequence()
        {
            return DOTween.Sequence()
                .Append(_layout.canvasGroup.DOFade(0, 0.5f))
                .AppendCallback(HideInactiveLayer)
                .Append(_layout.canvasGroup.DOFade(1, 0.3f))
                .SetEase(Ease.InOutQuint)
                .SetAutoKill(false);
        }
        
        private Sequence GetDeactivateSequence()
        {
            return DOTween.Sequence()
                .Append(_layout.canvasGroup.DOFade(0, 0.5f))
                .AppendCallback(ShowInactiveLayer)
                .Append(_layout.canvasGroup.DOFade(1, 0.3f))
                .SetEase(Ease.InOutQuint)
                .SetAutoKill(false);
        }

        private Sequence GetShowSequence()
        {
            return DOTween.Sequence()
                .Append(_layout.canvasGroup.DOFade(1, 0.5f))
                .SetEase(Ease.InOutQuint)
                .SetAutoKill(false);
        }
        
        private Sequence GetHideSequence()
        {
            return DOTween.Sequence()
                .Append(_layout.canvasGroup.DOFade(0, 0.5f))
                .SetEase(Ease.InOutQuint)
                .SetAutoKill(false);
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using DG.Tweening;

namespace GraviZoo
{
    public class ReloadTilesButton : MonoBehaviour
    {
        private GamePresenter _gamePresenter;
        private List<SpriteRenderer> _spriteRenderers;
        private BoxCollider2D _collider;
        private Sequence _tweenSequence;

        [Inject]
        public void Construct(GamePresenter gamePresenter)
        {
            _gamePresenter = gamePresenter;
        }

        private void Awake()
        {
            _spriteRenderers = new List<SpriteRenderer>();
            _spriteRenderers.AddRange(GetComponentsInChildren<SpriteRenderer>());
            _collider = GetComponent<BoxCollider2D>();
        }
        private void OnMouseDown()
        {
            _tweenSequence = DOTween.Sequence();

            _tweenSequence
                .Append(transform.DOScale(0.8f, 0.2f))
                .Append(transform.DOScale(1.0f, 0.2f))
                .OnComplete(() => _gamePresenter.OnRestartClicked());
        }

        public void Hide()
        {
            _collider.enabled = false;
            foreach (SpriteRenderer spriteRenderer in _spriteRenderers)
                spriteRenderer.DOFade(0.0f, 0.5f);
        }

        public void Show()
        {
            _collider.enabled = true;
            foreach (SpriteRenderer spriteRenderer in _spriteRenderers)
                spriteRenderer.DOFade(1.0f, 1.0f);
        }

        private void OnDestroy()
        {
            _tweenSequence.Kill(true);
        }
    }
}

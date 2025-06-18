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

        // This animation option for the button is made in the animator, for example
        private Animator _animator;

        [Inject]
        public void Construct(GamePresenter gamePresenter)
        {
            _gamePresenter = gamePresenter;
        }

        private void Awake()
        {
            _spriteRenderers = new List<SpriteRenderer>();
            _spriteRenderers.AddRange(GetComponentsInChildren<SpriteRenderer>());
            _animator = GetComponentInChildren<Animator>();
            _collider = GetComponent<BoxCollider2D>();
        }
        private void OnMouseDown()
        {
            _animator.SetTrigger("isClick");
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

        // Call from Animations, for Example
        public void ButtonClick()
        {
            _gamePresenter.OnRestartClicked();
        }
    }
}

using UnityEngine;
using DG.Tweening;
using Zenject;
using Shapes2D;

namespace GraviZoo
{
    public class Tile : MonoBehaviour
    {
        private TileModel _tileModel;

        public TileModel TileModel { get => _tileModel; set => _tileModel = value; }

        private Transform _transform;
        public Transform Transform { get => _transform; }

        private int _topPanelSlotIndex;
        public int TopPanelSlotIndex { set => _topPanelSlotIndex = value; }

        [SerializeField] private Transform _shapeParent;
        [SerializeField] private SpriteRenderer _animalsSprite;
        protected Shape Shape;
        protected Collider2D Collider2D;
        protected SpriteRenderer SpriteRenderer;
        protected Rigidbody2D Rigidbody2D;
        private FixedJoint2D _fixedJoint;

        private float _startRigidbodyGravityScale;
        private int _startSortingOrderShapeSprite;
        private int _startSortingOrderAnimalsSprite;

        protected Tween Tween;
        private Sequence _tweenSequence;
        private SignalBus _signalBus;

        private bool _isGameplayActive = false;

        private Vector3 _scaleOnTopPanel = new Vector3(0.9f, 0.9f, 0.9f);

        public virtual void Init(TileModel tileModel, Shape shape, Sprite animals, SignalBus signalBus = null)
        {
            _tileModel = tileModel;
            Shape = Instantiate(shape, _shapeParent);
            Collider2D = Shape.gameObject.GetComponent<Collider2D>();
            SpriteRenderer = Shape.gameObject.GetComponent<SpriteRenderer>();
            _animalsSprite.sprite = animals;            

            _transform = gameObject.transform;
            Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();

            _startRigidbodyGravityScale = Rigidbody2D.gravityScale;
            _startSortingOrderShapeSprite = SpriteRenderer.sortingOrder;
            _startSortingOrderAnimalsSprite = _animalsSprite.sortingOrder;

            if (signalBus != null)
            {
                _signalBus = signalBus;
                _signalBus.Subscribe<IsGameplayActiveSignal>(SwitchBoolIsGameplay);
            }
        }

        public void MoveToTopPanel(Vector3 endPosition, float duration)
        {
            _tweenSequence = DOTween.Sequence();

            _tweenSequence
                .Append(_transform.DOMove(endPosition, duration).SetEase(Ease.OutSine))
                .Insert(0, _transform.DOScale(_scaleOnTopPanel, duration / 5))
                .Insert(0, _transform.DORotate(Vector3.zero, duration))
                .OnComplete(() => _signalBus.Fire(new TileOnTopPanelSignal(this, _topPanelSlotIndex)));
        }

        public virtual void MoveToFinish(Vector3 finishPosition, float duration)
        {
            _tweenSequence = DOTween.Sequence();

            _tweenSequence
                .Append(_transform.DOMove(finishPosition, duration).SetEase(Ease.InOutElastic))
                .Append(_transform.DOShakeScale(0.2f))
                .Append(_transform.DOScale(Vector3.zero, duration / 5))
                .OnComplete(() =>
                {
                    SetDefaultState();
                    _signalBus.Fire(new TileOnFinishSignal(this));
                });
        }

        public void DestroyFromGamefield()
        {            
            _tweenSequence = DOTween.Sequence();

            _tweenSequence
                .Append(_transform.DOShakeScale(UnityEngine.Random.Range(0.2f, 0.5f), strength: 0.2f))
                .Append(_transform.DOScale(Vector3.zero, UnityEngine.Random.Range(0.2f, 0.5f)))
                .OnComplete(() =>
                {
                    SetDefaultState();
                });
        }

        private void OnMouseDown()
        {
            OnTileClick();
        }

        public virtual void OnTileClick()
        {
            if (_isGameplayActive)
                _signalBus.Fire(new ClickOnTileSignal(this));
        }

        public virtual void SwitchOffRigidbodyAndCollider()
        {
            Rigidbody2D.simulated = false;
            Collider2D.enabled = false;
            SpriteRenderer.sortingOrder++;
            _animalsSprite.sortingOrder++;
        }

        public void StopFalling()
        {
            Rigidbody2D.velocity = Vector2.zero;
        }

        protected virtual void SetDefaultState()
        {
            _transform.position = new Vector3(0.0f, -10.0f, 0.0f);
            _transform.localScale = Vector3.one;
            Rigidbody2D.gravityScale = _startRigidbodyGravityScale;
            SpriteRenderer.sortingOrder = _startSortingOrderShapeSprite;
            _animalsSprite.sortingOrder = _startSortingOrderAnimalsSprite;
            DeattachTile();
            Destroy(Shape.gameObject);
        }

        private void SwitchBoolIsGameplay(IsGameplayActiveSignal startStopGameplay)
        {
            _isGameplayActive = startStopGameplay.IsActive;
        }

        public void AttachTile(Rigidbody2D rigidbody2D)
        {
            _fixedJoint = gameObject.AddComponent<FixedJoint2D>();
            _fixedJoint.connectedBody = rigidbody2D;
            _fixedJoint.autoConfigureConnectedAnchor = true;
            _fixedJoint.enableCollision = false;
        }

        public void DeattachTile()
        {
            if (_fixedJoint != null)
                Destroy(_fixedJoint);
        }

        private void OnDestroy()
        {
            Tween.Kill(true);
            _tweenSequence.Kill(true);
        }
    }
}

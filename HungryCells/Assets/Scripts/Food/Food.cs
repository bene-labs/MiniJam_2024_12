using UnityEngine;

namespace Food
{
    public class Food : MonoBehaviour
    {
        public float Size { get; private set; }
        public float energyValue;
        public float eatSizeValue;
        public float damageValue;
        
        
        private SpriteRenderer _spriteRenderer;
        private Collider2D _collider;
        private FoodSpawner _parentSpawner;
        
        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();
            var sprite = _spriteRenderer.sprite;
            var localScale = transform.localScale;
            Size = sprite.bounds.size.x * localScale.x * sprite.bounds.size.y * localScale.y;
        }

        public void Initialize(FoodSpawner parentSpawner)
        {
            _parentSpawner = parentSpawner;
        }

        public void OnEaten()
        {
            _collider.enabled = false;
            if (_parentSpawner) _parentSpawner.OnFoodEaten();

            Destroy(gameObject);
        }
    }
}
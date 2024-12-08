using System;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        public Player.Player target;
        public GameObject spriteObject;
        public SpriteRenderer spriteRenderer;
        public float spriteRotationOffset = 180;
        public float rotationSpeed = 20;
        private float moveSpeed;
        public float minSpeed = 7;
        public float maxSpeed = 9;
        public float energyDrain;
        public float Size { get; private set; }
        public float energyValue;
        public float eatSizeValue;
        
        [SerializeField] private VisualEffect biggerVfx;
        [SerializeField] private VisualEffect smallerVfx;
        private bool smallerThanPlayer;
        
        private Collider2D _collider;
        
        private void Start()
        {
            moveSpeed = Random.Range(minSpeed, maxSpeed);
            _collider = GetComponent<Collider2D>();
            target.SizeChanged +=OnPlayerSizeChanged;
            var sprite = spriteRenderer.sprite;
            var localScale = transform.localScale;
            Size = sprite.bounds.size.x * localScale.x * sprite.bounds.size.y * localScale.y;
            OnPlayerSizeChanged();
        }

        private void OnDisable()
        {
            target.SizeChanged -= OnPlayerSizeChanged;
        }

        private void OnPlayerSizeChanged()
        {
            smallerThanPlayer = target.Size > Size;
            if (smallerThanPlayer)
            {
                biggerVfx.Stop();
                smallerVfx.Play();
            }
            else
            {
                biggerVfx.Play();
                smallerVfx.Stop();
            }
        }
        
        public void OnEaten()
        {
            EnemyLeftovers.Create(transform.position);
            _collider.enabled = false;
            Destroy(gameObject);
        }
        
        private void Update()
        {
            if (!target.enabled)
                return;
            
            Vector3 playerDist = target.transform.position - transform.position;
            playerDist.z = 0;

            transform.position += playerDist.normalized * (moveSpeed * Time.deltaTime)
                * (!target.enabled || (smallerThanPlayer && playerDist.magnitude < 3) ? -1f : 1);
            
            // rotate enemy to face the movement direction
            float angle = Mathf.Atan2(playerDist.y, playerDist.x) * Mathf.Rad2Deg;
            if (!target.enabled)
                angle += 180;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle + spriteRotationOffset));
            spriteObject.transform.rotation = Quaternion.Slerp(spriteObject.transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
    }
}
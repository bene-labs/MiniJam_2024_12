using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class Player : MonoBehaviour
    {
        // Exposed Fields -----------------------------------------------------
        [Header("General")] 
        public float maxEnergy = 100f;
        public float energyOnSpawn = 80f;
        public float energyLoseSpeed = 5f;
        public float moveSpeed = 8;
        public float mouseDistForMaxSpeed = 5;
        public float rotationSpeed = 20;
        public float size;

        [Header("Animation Timings")]
        public float invincibilityTime = 0.1f;
        public float damageFlashTime = 0.1f;

        [Header("Player Visuals")] 
        public GameObject spriteObject;
        public SpriteRenderer spriteRenderer;
        public float spriteRotationOffset = -90;
        
        // Member Fields -----------------------------------------------------
        private float _collectedEnergy = 0f;
        private bool _isInvincible = false;
        private bool _canGetInput = true;
        private Camera _camera;
        private Vector3 _startSize;
        
        // Methods -----------------------------------------------------
        private void CalcSize()
        { 
            var sprite = spriteRenderer.sprite; 
            var localScale = transform.localScale; 
            size = sprite.bounds.size.x * localScale.x * sprite.bounds.size.y * localScale.y;
        }

        // Start is called before the first frame update
        void Awake()
        {
            _camera = Camera.main;
            _startSize = transform.localScale;
        }

        private void Start()
        {
            Debug.Log("START");
            _collectedEnergy = energyOnSpawn;
            UpdateEnergyBar();
            CalcSize();
        }

        // Update is called once per frame
        void Update()
        {
            _collectedEnergy -= energyLoseSpeed * Time.deltaTime;
            UpdateEnergyBar();
            if (!_canGetInput || !Input.GetMouseButton(0)) return;
            // move player
            Vector3 movementDir = _camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            movementDir.z = 0;

            // calculate a multiplier for the speed based on the distance the mouse has to the player.
            // if its closer than the threshold the player's speed is reduced.
            float mouseDistMultiplier = Math.Clamp(movementDir.magnitude / mouseDistForMaxSpeed, 0, 1);

            transform.position += movementDir.normalized * (mouseDistMultiplier * (moveSpeed * Time.deltaTime));


            // rotate player to face the movement direction
            float angle = Mathf.Atan2(movementDir.y, movementDir.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle + spriteRotationOffset));
            spriteObject.transform.rotation = Quaternion.Slerp(spriteObject.transform.rotation, rotation,
                rotationSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Enemy.Enemy enemy))
            {
                TryEat(enemy);
            }
        }

        private void TakeDamage(float damage)
        {
            if (_isInvincible)
                return;

            _collectedEnergy -= damage;
            StartCoroutine(DamageAnimation(damage));
        }

        IEnumerator DamageAnimation(float damage)
        {
            _isInvincible = true;

            if (_collectedEnergy < 0)
                _collectedEnergy = 0;
            UpdateEnergyBar();
            var defaultColor = spriteRenderer.color;

            DOVirtual.Color(Color.red, defaultColor, damageFlashTime, value => spriteRenderer.color = value)
                .SetEase(Ease.OutCirc);

            yield return new WaitForSeconds(invincibilityTime);
            _isInvincible = false;
            // Add Inviciblity Frames and Red flash animation here

            if (_collectedEnergy <= 0)
                OnDeath();
        }

        private void OnDeath()
        {
            UIManager.SetDeathScreenVisibility(true);
        }
        
        private bool TryEat(Enemy.Enemy enemy)
        {
            return true;
            Debug.Log($"Trying to eat Enemy of size {enemy.Size} while at size {size}");
            
            if (enemy.Size > size)
            {
                transform.localScale -= new Vector3(enemy.eatSizeValue / 2, enemy.eatSizeValue / 2, 0);
                if (transform.localScale.x < _startSize.x && transform.localScale.y < _startSize.y)
                {
                    transform.localScale = _startSize;
                }
                TakeDamage(enemy.energyDrain);
                return true;
            }
            transform.localScale += new Vector3(enemy.eatSizeValue, enemy.eatSizeValue, 0);
            CalcSize();
            
            _collectedEnergy += enemy.energyValue;
            if (_collectedEnergy > maxEnergy)
                _collectedEnergy = maxEnergy;
            UpdateEnergyBar();
            enemy.OnEaten();
            return true;
        }

        private void UpdateEnergyBar()
        {
            UIManager.SetEnergyBar(_collectedEnergy, maxEnergy);
        }
    }
}
using System;
using UnityEditor;
using UnityEngine;

namespace Lottie
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(SpriteRenderer))]
    public class LottieSprite : MonoBehaviour
    {
        private Sys.TvgTexture _texture;
        
        [SerializeField] public string file = "";
        [SerializeField] public uint width = 32;
        [SerializeField] public uint height = 32;
        
        private SpriteRenderer _spriteRenderer;
        private Sprite _sprite;
        
        private bool _loaded;
        
        private void OnEnable()
        {
            if (Application.isPlaying) return;
            EditorApplication.update += Play;
        }

        private void OnDisable()
        {
            if (Application.isPlaying) return;
            EditorApplication.update -= Play;
        }

        public void OnValidate()
        {
            // Get the sprite renderer
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            // Check if the file is empty
            if (string.IsNullOrEmpty(file) || !System.IO.File.Exists(file) || width == 0 || height == 0)
            {
                _loaded = false;
                _spriteRenderer.sprite = null;
                return;
            }
            
            // Load the texture
            _texture = new Sys.TvgTexture(file, width, height);
            UpdateSprite();
            
            // Set the flag
            _loaded = true;
        }

        private void UpdateSprite()
        {
            var texture = _texture.Texture();
            _sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            _spriteRenderer.sprite = _sprite;
        }

        private void Update()
        {
            if (!Application.isPlaying) return;
            Play();
        }

        private void Play()
        {
            if (!_loaded) return;
            if (!PlayAnimation) return;
            _texture.Play();
            UpdateSprite();
        }

        public bool PlayAnimation { get; set; } = true;

        public float Frame
        {
            get => _texture.Frame;
            set => _texture.Frame = value;
        }
    }
}
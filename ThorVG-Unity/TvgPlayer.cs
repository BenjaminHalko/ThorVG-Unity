using System;
using UnityEditor;
using UnityEngine;

namespace Tvg
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(SpriteRenderer))]
    public class TvgPlayer : MonoBehaviour
    {   
        [SerializeField] public string file = "";
        [SerializeField] public float speed = 1.0f;

        private TvgTexture __texture;
        
        private SpriteRenderer __spriteRenderer;
        
        private bool __loaded;
        private bool __isAnimated;
        
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
            __spriteRenderer = GetComponent<SpriteRenderer>();
            
            // Check if the file is empty
            if (string.IsNullOrEmpty(file) || !System.IO.File.Exists(file))
            {
                __loaded = false;
                __spriteRenderer.sprite = null;
                return;
            }

            // Read the file
            var data = System.IO.File.ReadAllBytes(file);
            var dataString = System.Text.Encoding.UTF8.GetString(data);

            // Load the texture
            __texture = new TvgTexture(dataString);
            __isAnimated = __texture.totalFrames > 1;
            UpdateSprite();
            
            // Set the flag
            __loaded = true;
        }

        private void UpdateSprite()
        {
            __spriteRenderer.sprite =
                Sprite.Create(
                    __texture.Texture(),
                    new Rect(0, 0, __texture.width, __texture.height),
                    new Vector2(0.5f, 0.5f));
        }

        private void Play()
        {
            if (!__loaded) return;
            if (!__isAnimated) return;
            if (Mathf.Approximately(speed, 0.0f)) return;

            __texture.frame += Time.deltaTime * __texture.fps * speed;
            UpdateSprite();
        }

        private void Update()
        {
            Play();
        }
    }
}

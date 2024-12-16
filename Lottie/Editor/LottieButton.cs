using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

namespace Lottie.Editor
{
    internal class LottieButton 
    {
        // Animation data
        private Sys.TvgCanvas _tvgCanvas;
        private Sys.TvgAnimation _tvgAnimation;

        // Data
        private readonly string _url;
        private bool _requested;
        private readonly VisualElement _buttonList;

        // Sizing
        private const uint Size = 120;

        private void InitCanvas(string imageData)
        {
            Data = imageData;
            
            _tvgAnimation = new Sys.TvgAnimation();
            try
            {
                _tvgAnimation.Picture.LoadData(Data, "lottie");
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
                _buttonList.Remove(Element.parent);
                return;
            }
            
            _tvgCanvas = new Sys.TvgCanvas(Size, Size);
            _tvgAnimation.Picture.GetSize(out var width, out var height);
            // Resize to fit
            var scale = Size / Mathf.Max(width, height);
            _tvgAnimation.Picture.SetSize(width * scale, height * scale);
            _tvgAnimation.Frame = _tvgAnimation.TotalFrame / 2;
            _tvgCanvas.AddPaint(_tvgAnimation.Picture);
            _tvgCanvas.Update();
            Loaded = true;
            var texture = Texture();
            Element.style.backgroundImage = texture;
            if (texture == null)
            {
                // Clear Element from Button List
                _buttonList.Remove(Element.parent);
                Loaded = false;
            }
            EditorWindow.focusedWindow.Repaint();
        }
        
        public void Request()
        {
            if (_requested) return;
            _requested = true;
            var req = UnityWebRequest.Get(_url);
            req.SendWebRequest().completed += operation =>
            {
                if (req.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Failed to download Lottie file: {_url}");
                    return;
                }
                
                InitCanvas(req.downloadHandler.text);
            };
        }

        public LottieButton(string url, string name, string author, VisualElement lottieButtons)
        {
            Name = name;
            Author = author;
            _url = url;
            _buttonList = lottieButtons;
        }

        public void Play()
        {
            if (!Loaded) return;
            
            _tvgAnimation.Frame = _tvgAnimation.Frame = ((float)EditorApplication.timeSinceStartup * _tvgAnimation.TotalFrame / _tvgAnimation.Duration) % _tvgAnimation.TotalFrame;
            _tvgCanvas.Update();
        }
        
        public Texture2D Texture()
        {
            return !Loaded ? null : _tvgCanvas.Texture();
        }
        
        public string Name { get; }
        
        public string Author { get; private set; }
        
        public string Data { get; private set; }
        
        public bool Loaded { get; private set; }

        public VisualElement Element { get; set; }

        public void GetSize(out float width, out float height)
        {
            _tvgAnimation.Picture.GetSize(out width, out height);
        }
    }
}
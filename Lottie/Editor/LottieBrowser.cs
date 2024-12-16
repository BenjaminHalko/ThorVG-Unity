using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace Lottie.Editor
{
    internal class LottieBrowser : EditorWindow
    {
        private int _selectedTab = 0;
        private List<LottieButton> _animationItems = new();
        private readonly List<LottieButton> _emojiButtons = new();

        private LottieButton _selectedAnimation;
        private static LottieButton _hoveredAnimation;

        private Sys.TvgCanvas _canvas;
        private Sys.TvgAnimation _animation;

        private static LottieSprite _targetTexture;

        private const float WindowWidth = 408f;
        private const float WindowHeight = 772f;

        private VisualTreeAsset _visualTree;
        private VisualElement _root;
        private VisualElement _gridView;
        private VisualElement _detailView;
        private VisualElement _thumbnailGrid;
        private TextField _searchField;
        private VisualElement _previewImage;
        private Label _detailTitle;
        private Label _detailAuthor;
        
        private CancellationTokenSource _searchTokenSource;

        public static void ShowWindow(LottieSprite target)
        {
            var window = GetWindow<LottieBrowser>("Animation Browser");
            window.minSize = new Vector2(WindowWidth, WindowHeight);
            window.maxSize = new Vector2(WindowWidth, WindowHeight);
            _targetTexture = target;
        }

        private void OnEnable()
        {
            // Load the UXML file
            _visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Lottie/Editor/UI/LottieBrowser.uxml");

            // Clone the VisualTreeAsset to the root VisualElement
            _root = rootVisualElement;
            _visualTree.CloneTree(_root);

            _gridView = _root.Q<VisualElement>("GridView");
            _detailView = _root.Q<VisualElement>("DetailView");
            _thumbnailGrid = _root.Q<VisualElement>("ThumbnailGrid");
            _searchField = _root.Q<TextField>("SearchField");
            _previewImage = _root.Q<VisualElement>("PreviewImage");

            _detailTitle = _root.Q<Label>("TitleLabel");
            _detailAuthor = _root.Q<Label>("AuthorLabel");
            
            // Load emojis
            var emojis = new EmojiGetter().GetAllEmojis();
            foreach (var emoji in emojis)
            {
                _emojiButtons.Add(new LottieButton(
                    $"https://fonts.gstatic.com/s/e/notoemoji/latest/{emoji.code}/lottie.json", emoji.name, "Google", _thumbnailGrid));
            }

            SetupEventHandlers();
            SwitchTab(0);
        }

        private void SetupEventHandlers()
        {
            _root.Q<Button>("TabNotto").clicked += () => SwitchTab(0);
            _root.Q<Button>("TabLottie").clicked += () => SwitchTab(1);

            _searchField.RegisterValueChangedCallback(OnSearchChanged);

            _root.Q<Button>("BackButton").clicked += ShowGridView;
            _root.Q<Button>("InsertButton").clicked += InsertSelectedAnimation;
        }

        private void SwitchTab(int tabIndex)
        {
            _searchTokenSource?.Cancel();
            _selectedTab = tabIndex;
            if (_selectedTab == 0)
            {
                _animationItems = _emojiButtons;
            }
            else
            {
                LottieSearch("");
            }

            _searchField.SetValueWithoutNotify("");

            var tabButtons = _root.Query<Button>(className: "tab-button").ToList();
            foreach (var tab in tabButtons)
            {
                tab.RemoveFromClassList("active");
            }

            tabButtons[tabIndex].AddToClassList("active");
            
            UpdateThumbnailGrid();
        }

        private void OnSearchChanged(ChangeEvent<string> evt)
        {
            switch (_selectedTab)
            {
                case 0:
                    _animationItems = string.IsNullOrWhiteSpace(evt.newValue) ? 
                        _emojiButtons : _emojiButtons.FindAll(item => item.Name.ToLower().Contains(evt.newValue.ToLower()));
                    UpdateThumbnailGrid();
                    break;
                case 1:
                    LottieSearch(evt.newValue);
                    break;
            }
        }

        private void ShowGridView()
        {
            _selectedAnimation = null;
            _gridView.style.display = DisplayStyle.Flex;
            _detailView.style.display = DisplayStyle.None;
        }

        private void ShowDetailView(LottieButton animation)
        {
            _selectedAnimation = animation;
            _canvas = new Sys.TvgCanvas(384, 384);
            _animation = new Sys.TvgAnimation();
            _animation.Picture.LoadData(animation.Data, "lottie");
            _animation.Picture.GetSize(out var width, out var height);
            var scale = 384 / Mathf.Max(width, height);
            _animation.Picture.SetSize(width * scale, height * scale);
            _canvas.AddPaint(_animation.Picture);
            _detailTitle.text = animation.Name;
            _detailAuthor.text = animation.Author;

            _previewImage.style.backgroundImage = _canvas.Texture();
            _gridView.style.display = DisplayStyle.None;
            _detailView.style.display = DisplayStyle.Flex;
        }

        private async void LottieSearch(string search)
        {
            try
            {
                _searchTokenSource?.Cancel();
                _searchTokenSource = new CancellationTokenSource();
                var token = _searchTokenSource.Token;
                var result = await LottieSearchWrapper(search, token);
                
                _animationItems.Clear();
                foreach (var edge in result.edges)
                {
                    // TODO: REMOVE THIS LATER
                    var str = edge.node.jsonUrl.Replace("https://assets-v2.lottiefiles.com/https", "https");
                    _animationItems.Add(new LottieButton(str, edge.node.name, edge.node.createdBy.firstName, _thumbnailGrid));
                }

                UpdateThumbnailGrid();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Operation was canceled: {e}");
            }
        }

        private static async Task<LottieAPI.SearchPublicAnimationsResponse> LottieSearchWrapper(string search,
            CancellationToken token)
        {
            if (!string.IsNullOrWhiteSpace(search))
                await Task.Delay(500, token);
            var result = await LottieAPI.Search(search, 12);
            return result;
        }

    private void UpdateThumbnailGrid()
        {
            // Clear existing thumbnails
            _thumbnailGrid.Clear();

            // Add new thumbnails
            foreach (var item in _animationItems)
            {
                var thumbnail = new VisualElement();
                thumbnail.AddToClassList("thumbnail");

                var thumbnailImage = new VisualElement();
                thumbnailImage.AddToClassList("thumbnail-image");
                // Set the background image or other properties of thumbnailImage here
                thumbnail.Add(thumbnailImage);

                var thumbnailTitle = new Label(item.Name);
                thumbnailTitle.AddToClassList("thumbnail-title");
                thumbnail.Add(thumbnailTitle);
                
                // Add event listener to thumbnail
                thumbnail.RegisterCallback<MouseEnterEvent>(evt => StartAnimation(item));
                thumbnail.RegisterCallback<MouseLeaveEvent>(evt => StopAnimation(item));
                thumbnail.RegisterCallback<MouseDownEvent>(evt => ShowDetailView(item));
                
                // If the item is in view, request the animation
                // Only if the item is in the scroll frame
                _thumbnailGrid.Add(thumbnail);
                item.Element = thumbnailImage;
                if (item.Loaded)
                {
                    thumbnailImage.style.backgroundImage = item.Texture();
                }
            }
        }

        private void Update()
        {
            if (_selectedAnimation == null)
            {
                foreach (var item in _animationItems.Where(item => IsElementVisible(item.Element)))
                {
                    item.Request();
                    
                    // Check to see if the item is being hovered
                    if (item != _hoveredAnimation) continue;
                    if (!item.Loaded) continue;
                    item.Play();
                    item.Element.style.backgroundImage = item.Texture();
                    Repaint();
                }
            }
            else
            {
                _animation.Play();
                _canvas.Update();
                _previewImage.style.backgroundImage = _canvas.Texture();
                Repaint();
            }
        }
        
        private bool IsElementVisible(VisualElement element)
        {
            var scrollView = _thumbnailGrid.parent;
            var elementWorldBound = element.worldBound;
            var scrollViewWorldBound = scrollView.worldBound;

            return elementWorldBound.Overlaps(scrollViewWorldBound);
        }

        private static void StartAnimation(LottieButton animation)
        {
            _hoveredAnimation = animation;
        }
        
        private static void StopAnimation(LottieButton animation)
        {
            if (_hoveredAnimation == animation)
            {
                _hoveredAnimation = null;
            }
        }
        
        private void InsertSelectedAnimation()
        {
            if (_selectedAnimation == null) return;
            if (!System.IO.Directory.Exists("Assets/LottieFiles"))
            {
                System.IO.Directory.CreateDirectory("Assets/LottieFiles");
            }
            var path = $"Assets/LottieFiles/{string.Join("", _selectedAnimation.Name.Split(System.IO.Path.GetInvalidFileNameChars()))}.json";
            System.IO.File.WriteAllText(path, _selectedAnimation.Data);
            _selectedAnimation.GetSize(out var w, out var h);
            _targetTexture.file = path;
            _targetTexture.width = (uint)w;
            _targetTexture.height = (uint)h;
            _targetTexture.OnValidate();
            Close();
        }
    }
}

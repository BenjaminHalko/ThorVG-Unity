using UnityEditor;
using UnityEngine;

namespace Lottie.Editor
{
    [CustomEditor(typeof(LottieSprite))]
    internal class TvgAnimationEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var output = GUILayout.Button("Download Lottie File");

            if (output)
            {
                // Open LottieFiles Importer window
                LottieBrowser.ShowWindow((LottieSprite)target);
                
            }

            // To keep the default inspector properties (optional)
            DrawDefaultInspector();
        }
    }
}

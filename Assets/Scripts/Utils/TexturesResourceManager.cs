using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace CirclesGame
{
    public interface ITextureResourceManager
    {
        Texture GetTexture(CircleSizes size, CircleColors color);
        void GenerateNewSet(Action onTexturesReady);
    }

    public class TexturesResourceManager : MonoBehaviour, ITextureResourceManager
    {
        public Color GradientColor { set; get; }
        public event Action onSetIsReady;
        private readonly Dictionary<TextureKey, Texture> texturesCache = new Dictionary<TextureKey, Texture>();

        public Texture GetTexture(CircleSizes size, CircleColors color)
        {
            var key = new TextureKey(size, color);

            return texturesCache.ContainsKey(key) ? texturesCache[key] : null;
        }

        public void GenerateNewSet(Action onTexturesReady)
        {
            foreach (var texture in texturesCache)
            {
                DestroyImmediate(texture.Value);
            }

            texturesCache.Clear();
            Resources.UnloadUnusedAssets();
            StartPrepareTexturesSet();
            onSetIsReady += onTexturesReady;
        }

        public void StartPrepareTexturesSet()
        {
            StartCoroutine(TexturesSetPrepare());
        }

        private IEnumerator TexturesSetPrepare()
        {
            foreach (CircleSizes size in Enum.GetValues(typeof(CircleSizes)))
            {
                foreach (CircleColors color in Enum.GetValues(typeof(CircleColors)))
                {
                    var key = new TextureKey(size, color);
                    Texture texture = null;

                    yield return StartCoroutine(CreateTexture(key, (t) =>
                    {
                        texture = t;
                    }));

                    if (!texturesCache.ContainsKey(key))
                    {
                        texturesCache.Add(key, texture);
                    }
                    else
                    {
                        Destroy(texture);
                        texture = null;
                    }
                }
            }
            yield return null;

            if (onSetIsReady != null)
            {
                onSetIsReady();
            }
        }

        private IEnumerator CreateTexture(TextureKey key, Action<Texture> callback)
        {
            var sizeValue = GameModel.SIZE_BASE*(int) key.size;
            var texture = new Texture2D(sizeValue, sizeValue, TextureFormat.ARGB32, false)
            {
                filterMode = FilterMode.Trilinear,
                anisoLevel = 20
            };

            Color32[] gradient = null;
            var colorFiller = new GradientColorGenerator(() =>
            {
                gradient = new Color32[sizeValue * sizeValue];

                for (int i = 0; i < sizeValue; i++)
                {
                    for (int j = 0; j < sizeValue; j++)
                    {
                        gradient[j + i * sizeValue] = Color32.Lerp(GetColorValue(key.color), GradientColor, (float)j / (sizeValue / 1.8f));
                    }
                }

            });

            yield return colorFiller.WaitForColorFill();

            texture.SetPixels32(gradient, 0);
            texture.Apply(false, true);

            callback(texture);
        }

        private Color GetColorValue(CircleColors color)
        {
            switch (color)
            {
                case CircleColors.Red:
                    return Color.red;
                case CircleColors.Green:
                    return Color.green;
                case CircleColors.Blue:
                    return Color.blue;
                case CircleColors.Yellow:
                    return Color.yellow;
                case CircleColors.Magenta:
                    return Color.magenta;
                case CircleColors.Cyan:
                    return Color.cyan;
                default:
                    throw new ArgumentOutOfRangeException("color", color, null);
            }
        }

        private class TextureKey : IEquatable<TextureKey>
        {
            public readonly CircleSizes size;
            public readonly CircleColors color;

            public TextureKey(CircleSizes size, CircleColors color)
            {
                this.size = size;
                this.color = color;
            }

            public override int GetHashCode()
            {
                return (int) color + (int) size;
            }

            public bool Equals(TextureKey other)
            {
                return color == other.color && size == other.size;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as TextureKey);
            }
        }

        public class GradientColorGenerator
        {
            private bool textureFilled;

            public GradientColorGenerator(Action fillTask)
            {
                var thread = new Thread(() =>
                {
                    if (fillTask != null)
                        fillTask();
                    textureFilled = true;
                });
                thread.Start();
            }

            public IEnumerator WaitForColorFill()
            {
                while (!textureFilled)
                    yield return null;
            }
        }
    }
}
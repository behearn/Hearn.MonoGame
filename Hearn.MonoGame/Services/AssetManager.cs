using System;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Hearn.MonoGame
{
    public class AssetManager
    {
        
        static Dictionary<Type, Dictionary<string, object>> _cache;
        static ContentManager _content;

        public static void Initialise(ContentManager content)
        {
            if (content == null)
            {
                throw new Exception("content cannot be null");
            }

            _content = content;
            _cache = new Dictionary<Type, Dictionary<string, object>>();
        }

        public static void Load<T>(string assetName)
        {
            if (_cache == null)
            {
                throw new Exception("AssetManager not initialised");
            }

            var asset = _content.Load<T>(assetName);

            if (!_cache.ContainsKey(typeof(T)))
            {
                _cache.Add(typeof(T), new Dictionary<string, object>());
            }

            var typeCache = _cache[typeof(T)];

            if (typeCache.ContainsKey(assetName))
            {
                throw new Exception($"Asset {assetName} of type {typeof(T).ToString()} already loaded");
            }

            typeCache.Add(assetName, asset);

            TraceAsset(assetName, asset);

        }

        private static void TraceAsset<T>(string assetName, T asset)
        {
            var assetDetails = typeof(T).ToString(); ;

            if (typeof(T) == typeof(Texture2D))
            {
                var texture = asset as Texture2D;
                assetDetails = string.Concat(assetDetails, $" ({texture.Width} x {texture.Height})"); 
            }
            
            Trace.TraceInformation($"Loaded {assetName} as {assetDetails}");
        }

        public static T Get<T>(string assetName)
        {
            if (_cache == null)
            {
                throw new Exception("AssetManager not initialised");
            }

            if (!_cache.ContainsKey(typeof(T)))
            {
                throw new Exception($"No assets of type {typeof(T).ToString()} loaded.");
            }

            var typeCache = _cache[typeof(T)];

            if (!typeCache.ContainsKey(assetName))
            {
                throw new Exception($"asset {assetName} of type {typeof(T).ToString()} not loaded.");
            }

            return (T)typeCache[assetName];
        }
        
    }
}

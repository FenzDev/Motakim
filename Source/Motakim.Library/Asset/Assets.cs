using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Motakim
{
    public static class Assets
    {
        public static string assetsPath = "Assets";

        internal static Dictionary<string, IAsset> AssetsBank = new Dictionary<string, IAsset>();

        public static bool IsLoaded(string assetName)
        {
            return AssetsBank.ContainsKey(assetName);
        }

        public static T Get<T>(string assetName) where T : IAsset, new()
        {
            T asset;
            
            if (!AssetsBank.ContainsKey(assetName))
            {
                asset = new T();
                var stream = TitleContainer.OpenStream(Path.Combine(assetsPath, assetName));
                asset.Load(stream);
                asset.Name = assetName;
                AssetsBank.Add(assetName, asset);
            }
            else
            {
                asset = (T)AssetsBank[assetName];
            }

            return asset;
        }

        public static void Unload(string assetName)
        {
            var asset = AssetsBank[assetName];
            asset.Dispose();
            AssetsBank.Remove(assetName);
        }

        public static void UnloadAll()
        {
            foreach (var asset in AssetsBank)
            {
                asset.Value.Dispose();
            }

            AssetsBank.Clear();
        }
    }
}
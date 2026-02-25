using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Mokus2D.Content
{
    public class ReferenceCountingContentManager(IServiceProvider serviceProvider) : ContentManager(serviceProvider)
    {
        public bool Exists(string assetName)
        {
            return assetReferences.ContainsKey(assetName);
        }

        public override T Load<T>(string assetName)
        {
            if (Exists(assetName))
            {
                return (T)assetReferences[assetName].Asset;
            }
            T t = ReadAsset<T>(assetName, new Action<IDisposable>(RecordDisposableResource));
            AssetReference assetReference = new(t);
            assetReferences[assetName] = assetReference;
            if (t is Texture2D)
            {
                (t as Texture2D).Name = assetName;
            }
            return t;
        }

        public void IncreaseReferenceCount(string assetName)
        {
            ChangeCount(assetName, 1);
        }

        public void DecreaseReferenceCount(string assetName)
        {
            ChangeCount(assetName, -1);
        }

        private void ChangeCount(string assetName, int change)
        {
            AssetReference assetReference = assetReferences[assetName];
            assetReference.ReferencesCount += change;
            if (assetReference.ReferencesCount < 0)
            {
                throw new InvalidOperationException("Reference count can't be negative");
            }
        }

        private void RecordDisposableResource(IDisposable resource)
        {
        }

        public void UnloadUnused()
        {
            foreach (KeyValuePair<string, AssetReference> keyValuePair in assetReferences)
            {
                if (keyValuePair.Value.ReferencesCount == 0)
                {
                    toRemove.Add(keyValuePair.Key);
                    keyValuePair.Value.Dispose();
                }
            }
            foreach (string text in toRemove)
            {
                _ = assetReferences.Remove(text);
                Util.DebugLog.infoFmt("Unloading unused ressource {0}", [text]);
            }
            toRemove.Clear();
        }

        private readonly Dictionary<string, AssetReference> assetReferences = new();

        private List<string> toRemove = new(64);
    }
}


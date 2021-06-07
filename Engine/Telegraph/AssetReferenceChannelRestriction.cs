using UnityEngine;

namespace RedOwl.Engine
{
    public class AssetReferenceChannelRestriction : AssetReferenceUIRestriction
    {
        public override bool ValidateAsset(Object obj)
        {
            return obj is ChannelBase;
        }
    }
}
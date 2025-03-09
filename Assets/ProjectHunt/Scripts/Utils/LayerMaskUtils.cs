using UnityEngine;

namespace MadDuck.Scripts.Utils
{
    public abstract class LayerMaskUtils
    {
        public static bool IsInLayerMask(int layerToCheck, LayerMask layerMask)
        {
            return layerMask == (layerMask | (1 << layerToCheck));
        }
    }
}
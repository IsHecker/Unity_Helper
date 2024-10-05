using UnityEngine.EventSystems;
using UnityEngine;

namespace UnityHelper.Utilities
{
    public static class UIUtils
    {
        public static bool IsOverUI(Touch touch)
        {
            if (touch.phase != TouchPhase.Began)
                return false;

            return !EventSystem.current.IsPointerOverGameObject(touch.fingerId);
        }

        public static GameObject GetPressedUI() => EventSystem.current.currentSelectedGameObject;


        public static Vector3 CanvasPositionToWorldPosition(RectTransform canvas)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, canvas.position, UnityUtils.Camera, out var result);
            return result;
        }
    }
}

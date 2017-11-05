using UnityEngine;

namespace Assets.Scripts.CubeWalls
{
    public abstract class CubeBaseWall : MonoBehaviour
    {
        protected Texture2D Texture2D;
        protected float Width;
        protected float Height;

        private float rotatingSpeed = 5;
        private SpriteRenderer _wallSprite;
        private Transform _wallParent;

        private void Start()
        {
            Texture2D = new Texture2D(100, 100);
            Width = Texture2D.width;
            Height = Texture2D.height;
            _wallSprite = GetComponent<SpriteRenderer>();
            _wallSprite.sprite = RgbPartSprite();
            _wallParent = transform.parent;
        }

        private void OnMouseDrag()
        {
            var rotX = Input.GetAxis("Mouse X") * rotatingSpeed * Mathf.Deg2Rad;
            var rotY = Input.GetAxis("Mouse Y") * rotatingSpeed * Mathf.Deg2Rad;

            _wallParent.RotateAround(Vector3.up, -rotX);
            _wallParent.RotateAround(Vector3.right, rotY);
        }

        protected abstract Sprite RgbPartSprite();

        protected Sprite CreateSprite()
        {
            Texture2D.Apply();
            return Sprite.Create(Texture2D, new Rect(0, 0, Width, Height), new Vector2(0.5f, 0.5f));
        }
    }
}

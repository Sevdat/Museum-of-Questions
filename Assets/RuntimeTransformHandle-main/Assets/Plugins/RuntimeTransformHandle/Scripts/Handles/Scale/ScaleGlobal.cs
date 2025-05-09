using System.IO;
using UnityEngine;

namespace RuntimeHandle
{
    /**
     * Created by Peter @sHTiF Stefcek 20.10.2020
     */
    public class ScaleGlobal : HandleBase
    {
        protected Vector3 _axis;
        protected Vector3 _startScale;

        private Vector3 initialWorldPoint;  // A fixed point in world space (e.g., 1m ahead)
        private Vector2 initialScreenCenter;
        
        public ScaleGlobal Initialize(RuntimeTransformHandle p_parentTransformHandle, Vector3 p_axis, Color p_color)
        {
            _parentTransformHandle = p_parentTransformHandle;
            _axis = p_axis;
            _defaultColor = p_color;
            
            InitializeMaterial();

            transform.SetParent(p_parentTransformHandle.transform, false);

            GameObject o = new GameObject();
            o.transform.SetParent(transform, false);
            MeshRenderer mr = o.AddComponent<MeshRenderer>();
            mr.material = _material;
            MeshFilter mf = o.AddComponent<MeshFilter>();
            mf.mesh = MeshUtils.CreateBox(.35f, .35f, .35f);
            MeshCollider mc = o.AddComponent<MeshCollider>();

            return this;
        }

        public override void Interact(Vector3 p_previousPosition)
        {
            _parentTransformHandle.target.localScale = _startScale + _startScale * GetScreenDeviation();
            
            base.Interact(p_previousPosition);
        }
        // Get current screen deviation (in pixels)
        public float GetScreenDeviation()
        {
            // Re-project the SAME world point after camera rotation
            Vector2 currentScreenPos = Camera.main.WorldToScreenPoint(initialWorldPoint);
            Vector2 vec = currentScreenPos - initialScreenCenter;
            float magnitude = (vec.y>0)?-vec.magnitude:vec.magnitude;
            return magnitude/300;
        }
        public override void StartInteraction(Vector3 p_hitPoint)
        {
            initialWorldPoint = Camera.main.transform.position + Camera.main.transform.forward * 1f;
            initialScreenCenter = Camera.main.WorldToScreenPoint(initialWorldPoint);

            base.StartInteraction(p_hitPoint);
            _startScale = _parentTransformHandle.target.localScale;
        }
    }
}
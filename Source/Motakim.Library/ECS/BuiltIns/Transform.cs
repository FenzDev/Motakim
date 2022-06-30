using System;
using Microsoft.Xna.Framework;

namespace Motakim {
    public class Transform : Component {
        public Vector2 Position;
        public Vector2 Scale = new Vector2(1f);
        public float Rotation;
    }
}

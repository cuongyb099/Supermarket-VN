using UnityEngine;

namespace Core.Interact
{
    public interface IThrowable
    {
        public void Throw(Vector3 direction, float force);
    }
}

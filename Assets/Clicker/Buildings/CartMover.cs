using UnityEngine;

namespace Clicker.Buildings
{
    // A moving obstacle for step 4: it shuttles between two points and stops at each end, which is
    // when carving is worth it.
    public sealed class CartMover : MonoBehaviour
    {
        [SerializeField] private Vector3 pointA = new Vector3(0f, 0.5f, -6f);
        [SerializeField] private Vector3 pointB = new Vector3(0f, 0.5f, 6f);
        [SerializeField] private float speed = 2f;
        [SerializeField] private float pauseDuration = 3f;

        private Vector3 target;
        private float paused;

        private void Start()
        {
            transform.position = pointA;
            target = pointB;
        }

        private void Update()
        {
            if (transform.position == target)
            {
                paused += Time.deltaTime;
                if (paused < pauseDuration) return;
                paused = 0f;
                target = target == pointA ? pointB : pointA;
            }
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
    }
}

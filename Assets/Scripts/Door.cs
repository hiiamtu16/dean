    using UnityEngine;


    public class Door : MonoBehaviour
    {
        private Animator animator;
        private bool isOpened = false;

        void Start()
        {
            animator = GetComponent<Animator>();
            animator.enabled = false; // Tắt Animator lúc đầu để nó không chạy tự động
        }

        public void Open()
        {
            if (!isOpened) // Tránh mở cửa nhiều lần
            {
                isOpened = true;
                gameObject.tag = "OpenedDoor"; // Đổi tag
                if (animator != null)
                {
                    animator.SetBool("HaveKey", true); // Chạy animation mở cửa
                }
                Debug.Log("Cửa đã mở!");
            }
        }

    
    }

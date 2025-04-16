    using UnityEngine;


    public class Door : MonoBehaviour
    {
        private Animator animator;
        private bool isOpened = false;

        void Start()
        {
            animator = GetComponent<Animator>();
            animator.enabled = false; 
        }

        public void Open()
        {
            if (!isOpened) 
            {
                isOpened = true;
                gameObject.tag = "OpenedDoor"; 
                if (animator != null)
                {
                    animator.SetBool("HaveKey", true); 
                }
                Debug.Log("Cửa đã mở!");
            }
        }

    
    }

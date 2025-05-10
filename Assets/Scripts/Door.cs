using Unity.VisualScripting;
using UnityEngine;


public class Door : MonoBehaviour
{
    private static Door instance;
    public static Door Instance => instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            return;
        }

        if (instance.gameObject.GetInstanceID() != this.gameObject.GetInstanceID())
        {
            Destroy(this.gameObject);
        }
    
        
    }





    private Animator animator;
    private bool isOpened = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }



    public void Open()
    {
        Debug.Log("Goi ham Open Door");
        if (!isOpened)
        {
            isOpened = true;
            gameObject.tag = "OpenedDoor";

            if (animator != null)
            {
                animator.SetBool("HaveKey", true);
                Debug.Log("Da havekey xong");
            }

            Debug.Log("Cửa đã mở!");
        }
    }


}

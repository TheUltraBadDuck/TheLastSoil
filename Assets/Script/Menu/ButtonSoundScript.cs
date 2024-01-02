using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundScript : MonoBehaviour
{
    public AudioClip buttonClickSound; // Âm thanh khi nút được nhấn
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();

        // Đảm bảo nút đã được gán
        if (button != null)
        {
            // Gán sự kiện cho nút
            button.onClick.AddListener(PlayButtonClickSound);
        }
        else
        {
            Debug.LogWarning("Không tìm thấy Button component trên đối tượng chứa script.");
        }
    }

    void PlayButtonClickSound()
    {
        // Phát âm thanh khi nút được nhấn
        if (buttonClickSound != null)
        {
            AudioSource.PlayClipAtPoint(buttonClickSound, Camera.main.transform.position);
        }
    }
}

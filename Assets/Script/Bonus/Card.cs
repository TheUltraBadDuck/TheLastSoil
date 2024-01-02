using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public RectTransform[] buttons;
    public Vector2[] positions;
    public Button randomizeButton;

    void Start()
    {
        randomizeButton.onClick.AddListener(RandomizeButtons);
        RandomizeButtons(); // Gọi ngay khi bắt đầu để hiển thị trạng thái ban đầu
    }

    void RandomizeButtons()
    {
        // Ẩn tất cả các button ban đầu
        foreach (RectTransform button in buttons)
        {
            button.gameObject.SetActive(false);
        }

        // Chọn ngẫu nhiên 3 button để hiển thị
        RectTransform[] selectedButtons = SelectRandomButtons(buttons, 3);

        // Hiển thị 3 button được chọn ở các vị trí cố định
        for (int i = 0; i < selectedButtons.Length; i++)
        {
            RectTransform button = selectedButtons[i];
            // Đặt vị trí của button tại vị trí đã chọn
            button.anchoredPosition = positions[i];
            button.gameObject.SetActive(true);
        }
    }

    RectTransform[] SelectRandomButtons(RectTransform[] buttons, int count)
    {
        // Tạo một mảng chứa các button được chọn
        RectTransform[] selectedButtons = new RectTransform[count];

        // Trộn mảng button
        for (int i = 0; i < buttons.Length; i++)
        {
            RectTransform temp = buttons[i];
            int randomIndex = Random.Range(i, buttons.Length);
            buttons[i] = buttons[randomIndex];
            buttons[randomIndex] = temp;
        }

        // Lấy 'count' button đầu tiên từ mảng đã trộn
        System.Array.Copy(buttons, selectedButtons, count);

        return selectedButtons;
    }
}

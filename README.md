# 🥊 Boxing Arena: Multi Mode Combat

## 🎮 Giới thiệu

**Boxing** là một game đối kháng 3D màn hình dọc được phát triển bằng Unity.  
Bạn sẽ điều khiển võ sĩ của mình để chiến đấu qua nhiều chế độ chơi khác nhau với độ khó tăng dần rõ rệt, AI thông minh, và hiệu năng tối ưu hóa nhờ Object Pooling.

---

## 🚀 Tính năng nổi bật

- ✅ **3 chế độ chơi hấp dẫn**:
  - `1 vs 1`: Solo đối kháng tay đôi.
  - `1 vs Many`: Một mình chống lại nhiều kẻ địch.
  - `Many vs Many`: Tổ đội đấu tổ đội, bạn chỉ điều khiển 1 player, các nhân vật còn lại do AI điều khiển.

- 🔁 **Tự sinh 10 level / chế độ**:
  - Mỗi level có độ khó tăng dần.
  - Enemy tăng máu, sát thương, tốc độ theo cấp số mũ.
  - Player cũng tăng chỉ số nhẹ để giữ cân bằng.

- 🤖 **AI tự động**:
  - Enemy AI tự tìm đúng player để tấn công.
  - Player AI (đồng đội) tự tìm enemy gần nhất để hỗ trợ.

- 🧠 **Object Pooling**:
  - Tái sử dụng object thay vì Instantiate/Destroy.
  - Chạy mượt với hơn 50 model có animation đồng thời.
  - Tối ưu hóa cho mobile và PC.

- 🎥 **Camera động**:
  - Camera follow player chính tự động trong mọi chế độ.

---

## 🛠️ Kỹ thuật sử dụng

| Công nghệ             | Mục đích                                      |
|----------------------|-----------------------------------------------|
| Unity (2021.3+)      | Game Engine                                   |
| C# + OOP             | Viết code game logic theo hướng đối tượng     |
| MVC Pattern          | Model - View - Controller                     |
| Animator Controller  | Quản lý animation Walk, Idle, Punch           |
| Floating Joystick    | Điều khiển mobile/touch                       |
| Object Pooling       | Quản lý hiệu quả nhiều player/enemy           |
| Simple AI            | Di chuyển + tấn công tự động thông minh       |

---

## 🧪 Hệ thống tự sinh level

- Tăng level sau mỗi lần tiêu diệt hết enemy.

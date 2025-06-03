/// <summary>
/// giao diện cung cấp các phương thức để lấy dữ liệu đầu vào từ người dùng.
/// </summary>
/// <returns>
/// <para><see cref="GetMoveInput"/> trả về giá trị float biểu thị hướng di chuyển.</para>
/// <para><see cref="GetMouseX"/> trả về giá trị float biểu thị chuyển động chuột theo trục X.</para>
/// </returns>
public interface IInputProvider
{
    float GetMoveInput();
    float GetMouseX();
}

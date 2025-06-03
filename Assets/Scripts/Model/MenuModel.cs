public class MenuModel
{
    // enum đại diện cho các trạng thái của menu: chính, tùy chọn, không có
    public enum MenuState { Main, Options, None }

    // thuộc tính lưu trạng thái hiện tại của menu
    public MenuState CurrentState { get; private set; }

    // phương thức để đặt trạng thái menu
    public void SetMenu(MenuState state) => CurrentState = state;
}

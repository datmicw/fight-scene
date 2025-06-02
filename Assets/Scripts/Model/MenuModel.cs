public class MenuModel
{
    public enum MenuState { Main, Options, None }
    public MenuState CurrentState { get; private set; }

    public void SetMenu(MenuState state) => CurrentState = state;
}

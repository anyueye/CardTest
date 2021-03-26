namespace CardGame
{
    public enum UIFormId:byte
    {
        Undefined = 0,

        /// <summary>
        /// 弹出框。
        /// </summary>
        DialogForm = 1,

        /// <summary>
        /// 主菜单。
        /// </summary>
        MenuForm = 100,

        /// <summary>
        /// 设置。
        /// </summary>
        SettingForm = 101,

        /// <summary>
        /// 选择角色。
        /// </summary>
        SelectCharactorForm = 102,
        /// <summary>
        /// 游戏。
        /// </summary>
        GameForm = 103,
    }
}
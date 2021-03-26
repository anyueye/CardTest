namespace CardGame
{
    public class MenuForm : UGuiForm
    {

        private object _procedureMenu;
        public void EnterSelectCharactor()
        {
            GameEntry.UI.OpenUIForm(UIFormId.SelectCharactorForm,_procedureMenu);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            _procedureMenu = userData;
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            _procedureMenu = null;
            base.OnClose(isShutdown, userData);
        }
    }
}


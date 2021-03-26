namespace CardGame
{
    public class SelectCharactorForm:UGuiForm
    {
        private ProcedureMenu m_ProcedureMenu=null;

        public void OnStartButtonClick()
        {
            m_ProcedureMenu.StartGame();
        }

        public void OnBackStart()
        {
            Close();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            m_ProcedureMenu = (ProcedureMenu) userData;
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            m_ProcedureMenu = null;
        }
    }
}
using System;
using System.Web.UI;

namespace WebFormsApp
{
    public partial class SessionPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SessionTextBox.Text = Session[SystemWebAdapterConfig.SessionValue]?.ToString() ?? string.Empty;
            }
        }

        protected void OnSubmitSessionButtonClick(object sender, EventArgs e)
        {
            Session[SystemWebAdapterConfig.SessionValue] = SessionTextBox.Text;
        }
    }
}
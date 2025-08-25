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
                SessionTextBox.Text = Session["test-value"]?.ToString() ?? string.Empty;
            }
        }

        protected void OnSubmitSessionButtonClick(object sender, EventArgs e)
        {
            Session["test-value"] = SessionTextBox.Text;
        }
    }
}
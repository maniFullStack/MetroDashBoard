using SharedClasses;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace GCC_Web_Portal.Admin
{
    public partial class MessageTimeline : System.Web.UI.UserControl
    {
        public DataTable Messages { get; set; }
        public bool HideReplyBox { get; set; }

        public bool IsGuestVersion { get; set; }

        public GCCPropertyShortCode PropertyShortCode { get; set; }

        public Action<TextBox, MessageManager> OnReply { get; set; }
        public Action<TextBox, MessageManager, DropDownList> OnAddNote { get; set; }

        public bool NoteTabActive
        {
            get
            {
                return hdnLastTab.Value.Equals("note");
            }
            set
            {
                if (value)
                {
                    hdnLastTab.Value = "note";
                }
                else
                {
                    hdnLastTab.Value = "reply";
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsGuestVersion)
            {
                btnSendReply.Text = "Send reply to staff";
            }
            else
            {
                btnSendReply.Text = "Send reply to guest";
            }
        }

        protected void btnSendReply_Click(object sender, EventArgs e)
        {
            if (OnReply != null)
            {
                OnReply(txtReplyMessage, MessageManager);
            }
        }

        protected void btnStaffNote_Click(object sender, EventArgs e)
        {
            if (OnAddNote != null)
            {
                OnAddNote(txtStaffNote, MessageManager, ddlGuestInteraction);
            }
        }
    }
}
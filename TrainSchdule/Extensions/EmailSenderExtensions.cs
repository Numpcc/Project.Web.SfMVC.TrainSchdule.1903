using System.Text.Encodings.Web;
using System.Threading.Tasks;
using BLL.Interfaces;

namespace TrainSchdule.Extensions
{
    public static class EmailSenderExtensions
    {
        #region Logic

        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "��֤����",
                $"��֤�ʼ��ѷ�����ע������<a href='{HtmlEncoder.Default.Encode(link)}'>����˴�</a>������֤");
        }

        #endregion
    }
}

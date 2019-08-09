using eGradeBook.Models;
using eGradeBook.Models.Dtos.Grades;
using eGradeBook.Repositories;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace eGradeBook.Services
{
    public class EmailsService : IEmailsService
    {
        private IUnitOfWork db;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public EmailsService(IUnitOfWork db)
        {
            this.db = db;
        }

        private void NotifyParent(GradeNotificationDto details)
        {
            string subject = string.Format("Grade assigned for {0} {1}", details.StudentFirstName, details.StudentLastName);

            StringBuilder sb = new StringBuilder();
            sb.Append("<html><head><title>" + subject + "</title></head><body>");
            sb.Append(string.Format("<p>Dear <strong>{0} {1}</strong></p>", details.ParentFirstName, details.ParentLastName));
            sb.Append(string.Format("<p>Student <strong>{0} {1}</strong> from Classroom <strong>{2}</strong> ", details.StudentFirstName, details.StudentLastName, details.ClassRoom));
            sb.Append(string.Format("has got a Grade <strong>{0}</strong> for Course <strong>{1}</strong>, ", details.GradePoint, details.Course));
            sb.Append(string.Format("assigned by Teacher <strong>{0} {1}</strong> at <strong>{2}</strong>.</p>", details.TeacherFirstName, details.TeacherLastName, details.Assigned));
            sb.Append(string.Format("<p>Best wishes from <a href=\"{0}\">The School</a></p>", @"http://brains-akademija.forumotion.com/"));
            sb.Append(" </body></html>");

            string body = sb.ToString();
            string FromMail = ConfigurationManager.AppSettings["from"];
            string emailTo = details.ParentEmail;
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(ConfigurationManager.AppSettings["smtpServer"]);
            mail.From = new MailAddress(FromMail);
            mail.To.Add(emailTo);
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            mail.Body = body;
            SmtpServer.Port = int.Parse(ConfigurationManager.AppSettings["smtpPort"]);
            SmtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["from"], ConfigurationManager.AppSettings["password"]);
            SmtpServer.EnableSsl = bool.Parse(ConfigurationManager.AppSettings["smtpSsl"]);
            SmtpServer.Send(mail);
        }

        /// <summary>
        /// Notify parents of student through email
        /// </summary>
        /// <param name="grade"></param>
        public void NotifyParents(Grade grade)
        {
            // Get parent and email...

            var parents = grade.Taking.Student.StudentParents.Select(sp => sp.Parent);

            foreach (var parent in parents)
            {
                var notification = Converters.GradesConverter.GradeToGradeNotificationDto(grade, parent);

                try
                {
                    NotifyParent(notification);
                }
                catch
                {
                    logger.Error("Grade notification {@gradeNotification} failure", notification);
                }

                logger.Info("Grade notification {@gradeNotification} success", notification);
            }
        }
    }
}
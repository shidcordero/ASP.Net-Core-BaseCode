namespace Data.Utilities
{
    public static class Constants
    {
        public static class Common
        {
            public const string OrderBy = "OrderBy";
            public const string ThenBy = "ThenBy";
            public const string ModelStateErrors = "ModelStateErrors";
            public const string IdentityErrors = "IdentityErrors";
            public const string Xoauth2 = "XOAUTH2";
            public const string ModalMessage = "ModalMessage";
            public const string ModalTitle = "ModalTitle";
            public const string ModalSize = "ModalSize";
            public const string Previous = "prev";
            public const string Next = "next";
            public const string Value = "Value";
            public const string Text = "Text";
        }

        public static class AppConfig
        {
            public const string ConnectionString = "DefaultConnection";
            public const string MigrationAssembly = "Data";
        }

        public static class Message
        {
            //Modal Title
            public const string Info = "Info";
            public const string Warning = "Warning";
            public const string Error = "Error";
            public const string Confirm = "Confirm";

            //Common message
            public const string DeletePrompt = "Are you sure you want to delete the '{0}' record?";

            //Success message
            public const string RecordSuccessAdd = "Record successfully added.";
            public const string RecordSuccessUpdate = "Record successfully updated.";
            public const string RecordSuccessDelete = "Record successfully deleted.";

            //Error message
            public const string ErrorProcessing = "An error was encountered while processing the request.";
            public const string ErrorRecordExists = "Record already exists.";
            public const string ErrorRecordNotExists = "Record does not exist.";
            public const string ErrorRecordInUse = "This record is currently in use.";
            public const string ErrorRecordInvalid = "Record is not valid.";

            //Exception Error message
            public const string ModelException = "A model issue occured while processing your request. Please contact administrator.";
            public const string NetworkTransportException = "A network transport issue occured while processing your request. Please contact administrator.";
            public const string DatabaseException = "A database connection issue occured while processing your request. Please contact administrator.";
            public const string DirectoryNotFoundException = "A directory is not found while processing your request. Please contact administrator.";
            public const string FileNotFoundException = "A file is not found while processing your request. Please contact administrator.";
            public const string NullException = "A null exception found while processing your request. Please contact administrator.";
            public const string OutOfRangeException = "An out of range exception found while processing your request. Please contact administrator.";
        }

        public static class EmailTemplate
        {
            public static class Exception
            {
                public const string TemplateName = "Exception Email";
                public const string Subject = @"Exception - {0}";
                public const string Body = @"<span>Hi Team,</span><br /><br />
									<span>This is a message service to notify you that there is an exception occurs. Please see below the details.</span><br /><br />
									<b>Exception Details:</b> <br /> 
                                    <pre>{0}</pre><br />
                                    <br /><br />
                                    Thank you, <br /><b>ASP BaseCode</b><br /><br />
                                    This is a system-generated email. Please do not reply to this email.<br /><br />
                                    <b>LEGAL DISCLAIMER</b><br />
                                    This email including any attachments transmitted with it, may contain
                                    legally privileged, confidential or proprietary information. If you are not
                                    the intended recipient or the employee responsible for delivery of this
                                    message and its attachments to the intended recipient/s, YOU ARE HEREBY
                                    NOTIFIED that any use, distribution, dissemination, or copying of this email
                                    or any of the information contained herein is strictly prohibited. If you
                                    have received this message in error, please immediately notify the sender
                                    and delete this email message from your system.
									";
            }
            public static class ForgotPassword
            {
                public const string TemplateName = "ForgotPassword";
                public const string Subject = "Forgot Password";
                public const string Body = @"<span>Hi {0},</span><br /><br />
                                            <span>Please click the link to reset your password {1}.</span><br />
                                            Thank you, <br /><b>ASP BaseCode</b><br /><br />
                                            This is a system-generated email. Please do not reply to this email.<br /><br />
                                            <b>LEGAL DISCLAIMER</b><br />
                                            This email including any attachments transmitted with it, may contain
                                            legally privileged, confidential or proprietary information. If you are not
                                            the intended recipient or the employee responsible for delivery of this
                                            message and its attachments to the intended recipient/s, YOU ARE HEREBY
                                            NOTIFIED that any use, distribution, dissemination, or copying of this email
                                            or any of the information contained herein is strictly prohibited. If you
                                            have received this message in error, please immediately notify the sender
                                            and delete this email message from your system.";
            }

        }

        public static class ConfigurationString
        {
            public const string Email = "Email";
            public const string ExceptionEmail = "ExceptionEmail";
        }

        #region Sorting

        public static class Sort
        {
            public const string SortBy = "SortBy";
            public const string SortOrder = "SortOrder";
            public const string Page = "Page";
        }

        public static class SortDirection
        {
            public const string Ascending = "Ascending";
            public const string Descending = "Descending";
        }

        public static class SortIcon
        {
            public const string Unsorted = "fa-sort";
            public const string SortAscending = "fa-sort-asc";
            public const string SortDescending = "fa-sort-desc";
        }

        public static class Region
        {
            public const string RegionId = "RegionId";
            public const string RegionName = "RegionName";
            public const string RegionCode = "RegionCode";
            public const string RegionKey = "RegionKey";
            public const string Description = "Description";
            public const string RowVersion = "RowVersion";
        }

        #endregion Sorting
    }
}
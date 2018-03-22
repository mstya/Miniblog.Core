using System;
using System.Text;

namespace Miniblog.Core.Web.Models
{
    public class CommentViewModel
    {
        public string Author { get; set; }

        public string Content { get; set; }

        public string Id { get; set; }

        public string Email { get; set; }

        public DateTime PubDate { get; set; }

        public bool IsAdmin { get; set; }

        public string GetGravatar()
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(Email.Trim().ToLowerInvariant());
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                var sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                return $"https://www.gravatar.com/avatar/{sb.ToString().ToLowerInvariant()}?s=60&d=blank";
            }
        }

        public string RenderContent()
        {
            return Content;
        }
    }
}
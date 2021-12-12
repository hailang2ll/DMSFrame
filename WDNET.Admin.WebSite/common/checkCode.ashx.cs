using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace WDNET.Admin.WebSite.common
{
    /// <summary>
    /// checkCode 的摘要说明
    /// </summary>
    public class checkCode : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        /// <summary>
        /// 验证码类型(0-字母数字混合,1-数字,2-字母)
        /// </summary>
        private string validateCodeType = "1";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            context.Response.BufferOutput = true;
            context.Response.Cache.SetExpires(DateTime.Now.AddMilliseconds(-1));
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.AppendHeader("Pragma", "No-Cache");
            this.CreateCheckCodeImage(context, GenerateCheckCode());
        }
        private void CreateCheckCodeImage(HttpContext context, string checkCode)
        {
            context.Session["vcheckCode"] = checkCode; //将字符串保存到Session中，以便需要时进行验证
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(70, 22);
            Graphics g = System.Drawing.Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(System.Drawing.Color.White);
                // 画图片的背景噪音线
                int i;
                for (i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }

                Font font = new System.Drawing.Font("Arial", 12, (System.Drawing.FontStyle.Bold));
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2F, true);
                g.DrawString(checkCode, font, brush, 2, 2);
                //画图片的前景噪音点
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                context.Response.ClearContent();
                context.Response.ContentType = "image/Gif";
                context.Response.BinaryWrite(ms.ToArray());
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }
        private string GenerateCheckCode()
        {
            int number;
            char code;
            string checkCode = String.Empty;
            System.Random random = new Random();
            for (int i = 0; i < 4; i++)
            {
                number = random.Next();
                code = (char)('0' + (char)(number % 10));
                if (number % 2 == 0)
                    code = (char)('0' + (char)(number % 10));
                else
                    code = (char)('A' + (char)(number % 26));
                //去掉0，Modify By KennyPeng On 2008-11-10
                if (code.ToString() == "O" || code.ToString() == "I" || code.ToString() == "o" || code.ToString() == "i")
                {
                    i--;
                    continue;
                }
                //要求全为数字
                if (validateCodeType == "1")
                {
                    if ((int)code < 48 || (int)code > 57)
                    {
                        i--;
                        continue;
                    }
                }

                //要求全为字母
                if (validateCodeType == "2")
                {
                    if ((int)code > 47 || (int)code < 58)
                    {
                        i--;
                        continue;
                    }
                }
                checkCode += code.ToString();
            }
            return checkCode;
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
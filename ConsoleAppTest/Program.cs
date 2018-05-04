using ConsoleAppTest.Model;
using DMSFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    class Program
    {
        static void Main(string[] args)
        {
            InsertTest();
        }

        public static void InsertTest()
        {
            string body = "-你是谁fdfd😚😎😋🤥🤗😋fdfdf😉🙂🤗fdfdfd";

            PostMST entity = new PostMST
            {
                MemberName = "17704007627",
                CurrentType = 30,
                Image = string.Empty,
                Body = body,
                PostKey = Guid.NewGuid(),
                CreateTime = DateTime.Now,
                Remark = "谁😚😎😋🤥",
            };

            int flag = DMS.Create<PostMST>()
                .Insert(entity);


            //for (int i = 0; i < 2; i++)
            //{
            //    pro_PostMST_Insert param = new pro_PostMST_Insert()
            //    {
            //        MemberName = "17704007627",
            //        CurrentType = 30,
            //        Image = "201712/201712941238110228975616.jpg",
            //        Body = i + body,
            //        PostKey = Guid.NewGuid(),
            //        CreateTime = DateTime.Now,
            //    };
            //    DMSStoredProcedureHandler s = new DMSStoredProcedureHandler();
            //    string errMsg = string.Empty;
            //    bool result = s.ExecuteNoQuery(param, ref errMsg);

            //    if (result)
            //    {
            //    }
            //}


        }
    }
}

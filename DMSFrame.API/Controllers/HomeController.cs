using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DMSFrame.API.Controllers
{
    /// <summary>  
    /// 手动设计一个Person类。用于放到List泛型中  
    /// </summary>  
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; private set; }
        public string Sex { get; set; }

        public Person(string name, string sex, int age)
        {
            Name = name;
            Age = age;
            Sex = sex;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            List<Person> list = new List<Person>();
            list.Add(new Person("张三", "男", 20));
            list.Add(new Person("王成", "男", 32));
            list.Add(new Person("李丽", "女", 19));
            list.Add(new Person("何英", "女", 35));
            list.Add(new Person("何英", "女", 18));

            var ls = list.GroupBy(a => a.Name).Select(g => (new { name = g.Key, count = g.Count(), ageC = g.Sum(item => item.Age), sexc = g.Select(q => q.Sex).FirstOrDefault() }));

            return Content("OK");
        }
    }
}
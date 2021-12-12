using System.Collections.Generic;

namespace Admin.Entity.Sys.ExtEntity
{


    public class Province : AddressEntity
    {
        public Province()
        {
            this.City = new List<City>();
        }
        public List<City> City { get; set; }

    }


    public class City : AddressEntity
    {
        public City()
        {
            this.Areas = new List<AddressEntity>();
        }
        public List<AddressEntity> Areas { get; set; }
    }

    public class AddressEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public int LevelType { get; set; }
        public string CityCode { get; set; }
        public string ZipCode { get; set; }
    }




}

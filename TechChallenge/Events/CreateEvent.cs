using Core.Entities;
using Prometheus;
using System.Security;

namespace TechChallengeApi.Events
{
    public class CreateEvent
    {
        public Guid ID { get; set; }
        public   string Name { get; set; }
        public  string Email { get; set; }
        public  string Phone { get; set; }
        public  string Ddd { get; set; }

        public CreateEvent(Guid id,string name, string email, string phone,string ddd)
        {
            this.ID = id;
            this.Name = name;
            this.Email = email;
            this.Phone =phone;
            this.Ddd = ddd;
        }
    }
}

﻿namespace WebApi.Models
{
    public class CustomerVM
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool Active { get; set; }
        public string PanNo { get; set; }
        public string GSTNo { get; set; }
        public string CreatedDate { get; set; }
        public string UpdtateDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdtateBy { get; set; }
        public string UserId { get; set;}
    }
}

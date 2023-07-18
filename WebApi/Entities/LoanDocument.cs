﻿namespace WebApi.Entities
{
    public class LoanDocument
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int LoanNumber { get; set; }
        public Loan Loan { get; set; }
    }
}

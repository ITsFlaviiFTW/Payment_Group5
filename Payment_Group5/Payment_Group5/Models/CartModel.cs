﻿namespace Payment_Group5.Models
{
    public class CartModel
    {
        public int[] products { get; set; }
        public int customerID { get; set; }
        public decimal total { get; set; }
    }
}
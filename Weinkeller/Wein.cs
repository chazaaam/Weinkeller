﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weinkeller
{
    class Wein
    {
        public string barcode;
        public string name;
        public string detailname;
        public string vendor;
        public string origin;
        public string descr;
        public int quantity;

        public Wein()
        {
            barcode = "";
            name = "";
            detailname = "";
            vendor = "";
            origin = "";
            descr = "";
            quantity = 0;
        }

        public Wein(string e_barcode, string e_name, string e_detailname, string e_vendor, string e_origin, string e_descr, int e_quantity)
        {
            barcode = e_barcode;
            name = e_name;
            detailname = e_detailname;
            vendor = e_vendor;
            origin = e_origin;
            descr = e_descr;
            quantity = e_quantity;
        }

        public void setData(string e_barcode, string e_name, string e_detailname, string e_vendor, string e_origin, string e_descr, int e_quantity)
        {
            barcode = e_barcode;
            name = e_name;
            detailname = e_detailname;
            vendor = e_vendor;
            origin = e_origin;
            descr = e_descr;
            quantity = e_quantity;
        }

        public string getBarcode()
        {
            return barcode;
        }

        public string getName()
        {
            return name;
        }

        public string getDetailname()
        {
            return detailname;
        }

        public string getVendor()
        {
            return vendor;
        }

        public string getOrigin()
        {
            return origin;
        }

        public string getDescr()
        {
            return descr;
        }

        public int getQuantity()
        {
            return quantity;
        }

        public void addBottle()
        {
            quantity++;
        }

        public void removeBottle()
        {
            if (quantity > 0)
                quantity--;
        }
    }
}

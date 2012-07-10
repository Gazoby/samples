﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Collections;
using System.Reflection;
using PayTrace.Integration.Interfaces;

namespace PayTrace.Integration.API
{
    public class Request : IRequest
    {
        public Uri Destination{get;set;}
        
        public Request(Uri destination)
        {
            Destination = destination;
        }

        public Request() { }


        private Dictionary<string, string> APIAttributeValues = new Dictionary<string, string>();

        public Dictionary<string, string> GetAPIDictionary()
        {
            return APIAttributeValues.Select(x => new { x.Key, x.Value }).ToDictionary(x => x.Key, x => x.Value);
        }
        
        public void Add(string attribute, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                APIAttributeValues.Add(attribute, value);
            }
        }

        public void Add(string attribute, bool value)
        {
            if(value)
            {
                APIAttributeValues.Add(attribute, "Y");
            }
        }

        public Dictionary<string, string> ToAPI()
        {
            return APIAttributeValues;
        }

        public void AppendDictionary(Dictionary<string, string> dictionary)
        {
            foreach (var item in dictionary)
            {
                APIAttributeValues.Add(item.Key, item.Value);
            }
        }

        public Response Send()
        {
            Client client = new Client();
            return client.SendRequest(this);
        }


        public void AddCreditCardInfo(CreditCard cc)
        {
            AddressInfo billing_address = cc.BillingAddress;

            APIAttributeValues.Add(Keys.CC, cc.Number);
            APIAttributeValues.Add(Keys.EXPMNTH, cc.ExperationDate.Value.Month.ToString());
            APIAttributeValues.Add(Keys.EXPYR, cc.ExperationDate.Value.Year.ToString());
            APIAttributeValues.Add(Keys.CSC, cc.CSC);
            APIAttributeValues.Add(Keys.SADDRESS, billing_address.Street);
            APIAttributeValues.Add(Keys.SADDRESS2, billing_address.Street2);
            APIAttributeValues.Add(Keys.SCITY, billing_address.City);
            APIAttributeValues.Add(Keys.SSTATE, billing_address.Region);
            APIAttributeValues.Add(Keys.SZIP, billing_address.PostalCode);
            APIAttributeValues.Add(Keys.SCOUNTY, billing_address.County);
            APIAttributeValues.Add(Keys.SCOUNTRY, billing_address.Country);
        }

        public void AddShippingAddress(AddressInfo address)
        {
            APIAttributeValues.Add(Keys.SADDRESS, address.Street);
            APIAttributeValues.Add(Keys.SADDRESS2, address.Street2);
            APIAttributeValues.Add(Keys.SCITY, address.City);
            APIAttributeValues.Add(Keys.SSTATE, address.Region);
            APIAttributeValues.Add(Keys.SZIP, address.PostalCode);
            APIAttributeValues.Add(Keys.SCOUNTY, address.County);
            APIAttributeValues.Add(Keys.SCOUNTRY, address.Country);
        }
    }
}

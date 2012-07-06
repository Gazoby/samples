﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;
using System.IO;
using PayTrace.Integration;
using PayTrace.Integration.API;

namespace SecureCheckout
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnStartOrder_OnClick(object sender, EventArgs e)
        {
            //format parameters for request 
            // to get an approval amount set: AMOUNT~1.00
            // to get a declined amount set: AMOUNT~1.12
            // to get amount error set: AMOUNT~0.00

            APIRequestBuilder apiBuilder = new APIRequestBuilder(Destinations.Validation);

            apiBuilder.Add(Keys.UN, "demo123");
            apiBuilder.Add(Keys.PSWD, "demo123");
            apiBuilder.Add(Keys.TERMS, "Y");
            apiBuilder.Add(Keys.TRANXTYPE, "Sale");
            apiBuilder.Add(Keys.ORDERID, "1234");
            apiBuilder.Add(Keys.AMOUNT, "1.00");

            if (txtSilentPost.Text.Length > 0)
            {
                apiBuilder.Add(Keys.RETURNURL,Server.UrlEncode(txtSilentPost.Text));
            }

            string return_url = @"http://" + Request.Url.Authority;
            
            apiBuilder.Add(Keys.APPROVEURL, return_url + "/Approved.aspx");
            apiBuilder.Add(Keys.DECLINEURL, return_url + "/Declined.aspx");

            Client client = new Client();
            Response response = client.SendRequest(apiBuilder);
            
            BindeData(response);

        }


        private void BindeData(Response response)
        {
            // if we have errors output to ui
            ClearPanel();
            if (response.HasError)
            {
                lblResponce.Text = response.Error.Message;
            }
            else
            {
                // just so we can look at the raw response
                lblResponce.Text = response.Raw;

                lblOrderID.Text = response.ResponseValues[Keys.ORDERID];
                lblAUTHKEY.Text = response.ResponseValues[Keys.AUTHKEY];
                string url = "https://paytrace.com/api/checkout.pay?parmList=orderID~{0}|AuthKey~{1}";

                lnkSendToBilling.NavigateUrl = string.Format(url, lblOrderID.Text, lblAUTHKEY.Text);
            }

            pnl_response.Visible = true;

        }

        private void ClearPanel()
        {
            lblResponce.Text = string.Empty;
            lblOrderID.Text = string.Empty;
            lblAUTHKEY.Text = string.Empty;
            lnkSendToBilling.NavigateUrl = string.Empty;
        }

       
    }

}

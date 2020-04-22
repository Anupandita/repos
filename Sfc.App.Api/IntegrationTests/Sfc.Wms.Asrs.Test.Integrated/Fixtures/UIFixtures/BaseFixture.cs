﻿using RestSharp;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Sfc.Core.OnPrem.Result;
using System.Data;
using System.Reflection;
using System.Collections.Generic;
using System.Configuration;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures.UIFixtures
{
    public class BaseFixture
    {

        LoginCredentials loginCredentials;
        protected void CreateLoginDto()
        {
            loginCredentials = new LoginCredentials()
            {
                UserName = "PSI",
                Password = "WOLF"
            };
        }

        public void LoginToFetchToken()
        {
            CreateLoginDto();
            var client = new RestClient(UIConstants.LoginUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", Content.ContentType);
            request.AddJsonBody(loginCredentials);
            request.RequestFormat = DataFormat.Json;
            var response = client.Execute(request);
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content);
            Assert.AreEqual(ResultType.Ok, result.ResultType.ToString());
            UIConstants.BearerToken = response.Headers[1].Value.ToString();
        }

        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }

            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;

        }
        public IRestResponse CallGetApi(string url)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("content-type", Content.ContentType);
            request.AddHeader("Authorization", UIConstants.BearerToken);
            return client.Execute(request);
        }

        public IRestResponse CallPutApi(string url, string inp)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.PUT);
            request.AddHeader("content-type", Content.ContentType);
            request.AddHeader("Authorization", UIConstants.BearerToken);
            request.AddJsonBody(JsonConvert.DeserializeObject(inp));
            request.RequestFormat = DataFormat.Json;
            return client.Execute(request);
        }

        public IRestResponse CallPostApi(string url, string inp)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", Content.ContentType);
            request.AddHeader("Authorization", UIConstants.BearerToken);
            //  request.AddJsonBody(JsonConvert.DeserializeObject(inp));
            request.AddJsonBody(inp);
            request.RequestFormat = DataFormat.Json;
            return client.Execute(request);
        }
        public void VerifyOkResultAndStoreBearerToken(IRestResponse response)
        {
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content);
            Assert.AreEqual(ResultType.Ok, result.ResultType.ToString());
            UIConstants.BearerToken = response.Headers[1].Value.ToString();

        }

        public void VerifyCreatedResultAndStoreBearerToken(IRestResponse response)
        {
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content);
            Assert.AreEqual(ResultType.Created, result.ResultType.ToString());
            UIConstants.BearerToken = response.Headers[1].Value.ToString();

        }
        public void VerifyApiOutputAgainstDbOutput(DataTable queryDt, DataTable ApiDt)
        { var i = -1;

            foreach (DataRow dr in queryDt.Rows)
            {
                i = i+1;
                foreach (DataColumn dc in queryDt.Columns)
                   
                    Assert.AreEqual(dr[dc].ToString(), ApiDt.Rows[i][dc.ColumnName],dc.ColumnName+" : Values are not equal");
        }
}

        public void CreateUrlAndInputParamForApiUsing(string criteria)
        {
            switch (criteria)
            {
                case "Item":
                    UIConstants.ItemAttributeSearchUrl = ConfigurationManager.AppSettings["BaseUrl"] + UIConstants.ItemAttributes + UIConstants.Search + UIConstants.SearchInputItemId + UIConstants.ItemNumber;
                    return;
                case "ItemDescription":
                    UIConstants.ItemAttributeSearchUrl = ConfigurationManager.AppSettings["BaseUrl"] + UIConstants.ItemAttributes + UIConstants.Search + UIConstants.SearchInputItemDescription + UIConstants.ItemDescription;
                    return;
                case "VendorItemNumber":
                    UIConstants.ItemAttributeSearchUrl = ConfigurationManager.AppSettings["BaseUrl"] + UIConstants.ItemAttributes + UIConstants.Search + UIConstants.SearchInputVendorItemNumber + UIConstants.VendorItemNumber;
                    return;
                case "TempZone":
                    UIConstants.ItemAttributeSearchUrl = ConfigurationManager.AppSettings["BaseUrl"] + UIConstants.ItemAttributes + UIConstants.Search + UIConstants.SearchInputTempZone + UIConstants.TempZone;
                    return;
                case "ItemDetails":
                    UIConstants.ItemAttributeDetailsUrl = ConfigurationManager.AppSettings["BaseUrl"] + UIConstants.ItemAttributes + UIConstants.ItemNumber;
                    return;
            }
        }
    }
}

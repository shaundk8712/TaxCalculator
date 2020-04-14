using NUnit.Framework;
using System.Net.Http;
using System.Configuration;
using System.Net;
using System.Net.Http.Headers;
using System;

namespace TaxCalculator.NUnitTests.Tests
{
    public class HttpResponseTests
    {
        private HttpClient client;

        private HttpResponseMessage response;

        [SetUp]
        public void SetUP()
        {
            client = new HttpClient();

            client.BaseAddress = new Uri("https://localhost:44324/api/incometaxapi");
            response = client.GetAsync("IncomeTaxApi").Result;
        }

        [Test]
        public void GetResponseIsSuccess()
        {
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }


        [Test]
        public void GetResponseIsJson()
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Assert.AreEqual("application/json", response.Content.Headers.ContentType.MediaType);
        }

        [Test]
        public void GetAuthenticationStatus()
        {
            Assert.AreNotEqual(HttpStatusCode.Unauthorized,response.StatusCode);
        }
    }
}
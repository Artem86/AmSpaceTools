﻿using System;
using System.Net;
using AmSpaceClient;
using Moq;
using NUnit;
using NUnit.Framework;
using System.Net.Http;
using System.Security;
using System.Threading.Tasks;
using AmSpaceModels;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Globalization;

namespace AmspaceClientUnitTests
{
    [TestFixture]
    public class AmSpaceClientTests
    {
        private IAmSpaceClient _amSpaceClient;
        private Mock<IRequestsWrapper> _requestsWrapper;

        [SetUp]
        public void SetUp()
        {
            _requestsWrapper = new Mock<IRequestsWrapper>();
            _amSpaceClient = new AmSpaceClient.AmSpaceClient(_requestsWrapper.Object, new ApiEndpoints("http://a.b"));
        }

        [Test]
        public void LoginRequestAsync_WhenCalledWithOkCredentials_ReturnsTrue()
        {
            var loginResult = new LoginResult();
            _requestsWrapper
                .Setup(rw => rw.PostAsyncWrapper<LoginResult>(It.IsAny<string>(), It.IsAny<FormUrlEncodedContent>()))
                .Returns(Task.FromResult(loginResult));
            var secureString = new SecureString();
            var amspaceEnv = new AmSpaceEnvironment {BaseAddress = "http://a.b"};

            var result = _amSpaceClient.LoginRequestAsync("a", secureString, amspaceEnv);

            Assert.IsTrue(result.Result);
        }

        [Test]
        public void LoginRequestAsync_WhenCalledWithWrongCredentials_Throws()
        {
            _requestsWrapper
                .Setup(rw => rw.PostAsyncWrapper<LoginResult>(It.IsAny<string>(), It.IsAny<FormUrlEncodedContent>()))
                .Throws(new Exception());
            var secureString = new SecureString();
            var amspaceEnv = new AmSpaceEnvironment { BaseAddress = "http://a.b" };

            AsyncTestDelegate call = () => _amSpaceClient.LoginRequestAsync("a", secureString, amspaceEnv);

            Assert.ThrowsAsync<Exception>(call);
        }

        [Test]
        public void LoginRequestAsync_WhenCalledWithOkCredentials_CallsAddAuthCookiesAndAddAuthHeaders()
        {
            var loginResult = new LoginResult();
            _requestsWrapper
                .Setup(rw => rw.PostAsyncWrapper<LoginResult>(It.IsAny<string>(), It.IsAny<FormUrlEncodedContent>()))
                .Returns(Task.FromResult(loginResult));
            var secureString = new SecureString();
            var amspaceEnv = new AmSpaceEnvironment { BaseAddress = "http://a.b" };

            _amSpaceClient.LoginRequestAsync("a", secureString, amspaceEnv);

            _requestsWrapper.Verify( rw => rw.AddAuthCookies(It.IsAny<Uri>(), It.IsAny<Cookie>()), Times.AtLeastOnce);
            _requestsWrapper.Verify(rw => rw.AddAuthHeaders(It.IsAny<AuthenticationHeaderValue>()), Times.AtLeastOnce);
        }

        [Test]
        public void GetAvatarAsync_WhenCalledWithoutPriorAuth_Throws()
        {
            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.Content = new StringContent("a");
            var converterMock = new Mock<IImageConverter>();
            converterMock.Setup(cm => cm.ConvertFromByteArray(It.IsAny<byte[]>())).Returns(new BitmapImage());

            _requestsWrapper
                .Setup(rw => rw.GetAsyncWrapper(It.IsAny<string>()))
                .Returns(Task.FromResult(response));

            AsyncTestDelegate call = () => _amSpaceClient.GetAvatarAsync("a", converterMock.Object);

            Assert.ThrowsAsync(Is.TypeOf(typeof(Exception)), call);
        }

        [Test]
        public void GetAvatarAsync_WhenCalledNotFromProductionEnvironment_ReturnsDefaultAvatar()
        {
            var response1 = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response1.Content = new StringContent("a");
            var response2 = new HttpResponseMessage(HttpStatusCode.OK);
            response1.Content = new StringContent("b");
            _requestsWrapper
                .SetupSequence(rw => rw.GetAsyncWrapper(It.IsAny<string>()))
                .Returns(Task.FromResult(response1))
                .Returns(Task.FromResult(response2));
            var converterMock = new Mock<IImageConverter>();
            converterMock.Setup(cm => cm.ConvertFromByteArray(It.IsAny<byte[]>())).Returns(new BitmapImage());

            var result = _amSpaceClient.GetAvatarAsync("a", converterMock.Object);

            _requestsWrapper.Verify(rw => rw.GetAsyncWrapper(It.IsAny<string>()), Times.Exactly(2));
        }

        [Test]
        public void GetAvatarAsync_WhenCalled_ReturnsBitmapSourceObject()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            _requestsWrapper
                .SetupSequence(rw => rw.GetAsyncWrapper(It.IsAny<string>()))
                .Returns(Task.FromResult(response));

            var result = _amSpaceClient.GetAvatarAsync("a");

            Assert.That(result, Is.TypeOf(typeof(Task<BitmapSource>)));
        }


    }
}

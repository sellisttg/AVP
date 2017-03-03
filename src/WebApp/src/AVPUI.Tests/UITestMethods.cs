using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
using Microsoft.VisualStudio.TestTools.UITesting.HtmlControls;


namespace AVPUI.Tests
{
    /// <summary>
    /// Summary description for CodedUITest1
    /// </summary>
    [CodedUITest]
    public class UITestMethods : TestMethods
    {
        public UITestMethods()
        {
        }

        [TestMethod]
        public void UserLoginUITest()
        {
            TestMethods test = new TestMethods();
            var broswer = BrowserWindow.Launch("http://localhost:31419/Thunderstruck.html");
            test.EnterText(broswer, "username", "WebTest");
            test.EnterText(broswer, "password", "password");
            test.ClickButton(broswer, "loginBtn");
            test.ClickButton(broswer, "saveUserInfoBtn");
            test.ClickLabel(broswer, "savedUserInfoLabel");
            test.ClickLabel(broswer, "logoutBtn");
            test.ClickButton(broswer, "loginBtn");
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        }

        [TestMethod]
        public void UpdateUserInfoUITest()
        {
            TestMethods test = new TestMethods();
            var broswer = BrowserWindow.Launch("http://localhost:31419/Thunderstruck.html");
            test.EnterText(broswer, "username", "WebTest");
            test.EnterText(broswer, "password", "password");
            test.ClickButton(broswer, "loginBtn");
            test.EnterText(broswer, "resetPassword", "password");
            test.EnterText(broswer, "confirmPassword", "password");
            test.EnterText(broswer, "userDisplayName", "WebTest");
            test.EnterText(broswer, "streetAddress", "2015 J Street");
            test.EnterText(broswer, "city", "Sacramento");
            test.ClickDropDownList(broswer, "selectState");
            test.ClickDropDownListElement(broswer, "CA");
            test.EnterText(broswer, "zipCode", "95819");
            test.EnterText(broswer, "emailAddress", "mloller@trinitytg.com");
            test.EnterText(broswer, "phoneNumber", "9167984676");
            test.ClickButton(broswer, "saveUserInfoBtn");
            test.ClickLabel(broswer, "savedUserInfoLabel");
            test.ClickLabel(broswer, "logoutBtn");
            test.ClickButton(broswer, "loginBtn");
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        }

        [TestMethod]
        public void UpdateUserProfileRandomOptInOutUITest()
        {
            TestMethods test = new TestMethods();
            var broswer = BrowserWindow.Launch("http://localhost:31419/Thunderstruck.html");
            test.EnterText(broswer, "username", "WebTest");
            test.EnterText(broswer, "password", "password");
            test.ClickButton(broswer, "loginBtn");
            test.EnterText(broswer, "resetPassword", "password");
            test.EnterText(broswer, "confirmPassword", "password");
            test.EnterText(broswer, "userDisplayName", "WebTest");
            test.EnterText(broswer, "streetAddress", "2015 J Street");
            test.EnterText(broswer, "city", "Sacramento");
            test.ClickDropDownList(broswer, "selectState");
            test.ClickDropDownListElement(broswer, "CA");
            test.EnterText(broswer, "zipCode", "95819");
            test.EnterText(broswer, "emailAddress", "mloller@trinitytg.com");
            test.EnterText(broswer, "phoneNumber", "9167984676");
            test.ClickRandomCheckBox(broswer, "emailNotifications", "pushNotifications", "smsNotification");
            test.ClickButton(broswer, "saveUserInfoBtn");
            test.ClickLabel(broswer, "savedUserInfoLabel");
            test.ClickLabel(broswer, "logoutBtn");
            test.ClickButton(broswer, "loginBtn");
        }

        [TestMethod]
        public void RegisterUserUITest()
        {
            TestMethods test = new TestMethods();
            var broswer = BrowserWindow.Launch("http://localhost:31419/Thunderstruck.html");
            test.ClickLabel(broswer, "registerLabel2");
            test.EnterText(broswer, "regUsername", "Test");
            test.EnterText(broswer, "resetPassword", "Test");
            test.EnterText(broswer, "confirmPassword", "Test");
            test.EnterText(broswer, "userDisplayName", "Test");
            test.ClickButton(broswer, "registerBtn");
            var broswer2 = BrowserWindow.Launch("http://localhost:31419/Thunderstruck.html");
            test.EnterText(broswer2, "username", "Test");
            test.EnterText(broswer2, "password", "Test");
            test.ClickButton(broswer2, "loginBtn");
            test.ClickButton(broswer2, "saveUserInfoBtn");
            test.ClickLabel(broswer2, "logoutBtn");
            test.ClickButton(broswer2, "loginBtn");
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        }

        [TestMethod]
        public void OptInOutSMSUITest()
        {
            TestMethods test = new TestMethods();
            var broswer = BrowserWindow.Launch("http://localhost:31419/Thunderstruck.html");
            test.EnterText(broswer, "username", "WebTest");
            test.EnterText(broswer, "password", "password");
            test.ClickButton(broswer, "loginBtn");
            test.EnterText(broswer, "resetPassword", "password");
            test.EnterText(broswer, "confirmPassword", "password");
            test.EnterText(broswer, "userDisplayName", "WebTest");
            test.EnterText(broswer, "streetAddress", "2015 J Street");
            test.EnterText(broswer, "city", "Sacramento");
            test.ClickDropDownList(broswer, "selectState");
            test.ClickDropDownListElement(broswer, "CA");
            test.EnterText(broswer, "zipCode", "95819");
            test.EnterText(broswer, "emailAddress", "mloller@trinitytg.com");
            test.EnterText(broswer, "phoneNumber", "9167984676");
            test.ClickCheckBox(broswer, "smsNotification");
            test.ClickButton(broswer, "saveUserInfoBtn");
            test.ClickLabel(broswer, "savedUserInfoLabel");
            test.ClickLabel(broswer, "logoutBtn");
            test.ClickButton(broswer, "loginBtn");
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        }

        [TestMethod]
        public void OptInOutPushUITest()
        {
            TestMethods test = new TestMethods();
            var broswer = BrowserWindow.Launch("http://localhost:31419/Thunderstruck.html");
            test.EnterText(broswer, "username", "WebTest");
            test.EnterText(broswer, "password", "password");
            test.ClickButton(broswer, "loginBtn");
            test.EnterText(broswer, "resetPassword", "password");
            test.EnterText(broswer, "confirmPassword", "password");
            test.EnterText(broswer, "userDisplayName", "WebTest");
            test.EnterText(broswer, "streetAddress", "2015 J Street");
            test.EnterText(broswer, "city", "Sacramento");
            test.ClickDropDownList(broswer, "selectState");
            test.ClickDropDownListElement(broswer, "CA");
            test.EnterText(broswer, "zipCode", "95819");
            test.EnterText(broswer, "emailAddress", "mloller@trinitytg.com");
            test.EnterText(broswer, "phoneNumber", "9167984676");
            test.ClickCheckBox(broswer, "pushNotifications");
            test.ClickButton(broswer, "saveUserInfoBtn");
            test.ClickLabel(broswer, "savedUserInfoLabel");
            test.ClickLabel(broswer, "logoutBtn");
            test.ClickButton(broswer, "loginBtn");
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        }

        [TestMethod]
        public void OptInOutEmailUITest()
        {
            TestMethods test = new TestMethods();
            var broswer = BrowserWindow.Launch("http://localhost:31419/Thunderstruck.html");
            test.EnterText(broswer, "username", "WebTest");
            test.EnterText(broswer, "password", "password");
            test.ClickButton(broswer, "loginBtn");
            test.EnterText(broswer, "resetPassword", "password");
            test.EnterText(broswer, "confirmPassword", "password");
            test.EnterText(broswer, "userDisplayName", "WebTest");
            test.EnterText(broswer, "streetAddress", "2015 J Street");
            test.EnterText(broswer, "city", "Sacramento");
            test.ClickDropDownList(broswer, "selectState");
            test.ClickDropDownListElement(broswer, "CA");
            test.EnterText(broswer, "zipCode", "95819");
            test.EnterText(broswer, "emailAddress", "mloller@trinitytg.com");
            test.EnterText(broswer, "phoneNumber", "9167984676");
            test.ClickCheckBox(broswer, "emailNotifications");
            test.ClickButton(broswer, "saveUserInfoBtn");
            test.ClickLabel(broswer, "savedUserInfoLabel");
            test.ClickLabel(broswer, "logoutBtn");
            test.ClickButton(broswer, "loginBtn");
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        }

        [TestMethod]
        public void HeaderElementsUITest()
        {
            TestMethods test = new TestMethods();
            var broswer = BrowserWindow.Launch("http://localhost:31419/Thunderstruck.html");
            test.EnterText(broswer, "username", "Test");
            test.EnterText(broswer, "password", "Test");
            test.ClickButton(broswer, "loginBtn");
            test.ClickLabel(broswer, "dashboard");
            test.ClickLabel(broswer, "incidents");
            test.ClickLabel(broswer, "editProfile");
            test.ClickLabel(broswer, "switchRole");
            test.ClickButton(broswer, "okBtn");
            test.ClickLabel(broswer, "logoutBtn");
            test.ClickButton(broswer, "loginBtn");
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        }




        #region Additional test attributes

        // You can use the following additional attributes as you write your tests:

        ////Use TestInitialize to run code before running each test 
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{        
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //}

        ////Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{        
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //}

        #endregion

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        private TestContext testContextInstance;
    }
}

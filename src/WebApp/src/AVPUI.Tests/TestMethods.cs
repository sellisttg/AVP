using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using System.Windows.Automation;

namespace AVPUI.Tests
{
    public class TestMethods
    {
        public void EnterText(UITestControl parent, string id, string value)
        {
            var edit = new HtmlEdit(parent);
            edit.SearchProperties.Add(HtmlEdit.PropertyNames.Id, id);
            edit.Text = value;

        }

        public void ClickRandomCheckBox(UITestControl parent, string value1, string value2, string value3)
        {
            var checkBox1 = new HtmlCheckBox(parent);
            var checkBox2 = new HtmlCheckBox(parent);
            var checkBox3 = new HtmlCheckBox(parent);
            Random rand = new Random();
            int checkNum = rand.Next(1, 3);
            switch (checkNum)
            {
                case '1':
                    checkBox1.SearchProperties.Add(HtmlEdit.PropertyNames.Id, value1);
                    Mouse.Click(checkBox1);
                    break;
                case '2':
                    checkBox1.SearchProperties.Add(HtmlEdit.PropertyNames.Id, value2);
                    Mouse.Click(checkBox1);
                    break;
                case '3':
                    checkBox1.SearchProperties.Add(HtmlEdit.PropertyNames.Id, value3);
                    Mouse.Click(checkBox1);
                    break;
                default:
                    checkBox1.SearchProperties.Add(HtmlEdit.PropertyNames.Id, value1);
                    checkBox2.SearchProperties.Add(HtmlEdit.PropertyNames.Id, value2);
                    checkBox3.SearchProperties.Add(HtmlEdit.PropertyNames.Id, value3);
                    Mouse.Click(checkBox1);
                    Mouse.Click(checkBox2);
                    Mouse.Click(checkBox3);
                    break;
            }
        }

        public void ClickCheckBox(UITestControl parent, string value1)
        {
            var checkBox1 = new HtmlCheckBox(parent);
            checkBox1.SearchProperties.Add(HtmlEdit.PropertyNames.Id, value1);
            Mouse.Click(checkBox1);           
        }

        public void ClickButton(UITestControl parent, string value)
        {
            //var button = new HtmlInputButton(parent);
            var button = new HtmlButton(parent);
            button.SearchProperties.Add(HtmlButton.PropertyNames.Id, value);
            Mouse.Click(button);

        }

        public void ClickButtonVal(UITestControl parent, string value)
        {
            //var button = new HtmlInputButton(parent);
            var button = new HtmlButton(parent);
            button.SearchProperties.Add(HtmlButton.PropertyNames.ValueAttribute, value);
            Mouse.Click(button);

        }

        public void ClickDropDownList(UITestControl parent, string value)
        {
            var button = new HtmlComboBox(parent);
            button.SearchProperties.Add(HtmlComboBox.PropertyNames.Id, value);
            Mouse.Click(button);
        }

        public void ClickDropDownListElement(UITestControl parent, string value)
        {
            HtmlListItem button = new HtmlListItem(parent);
            button.SearchProperties.Add(HtmlListItem.PropertyNames.ValueAttribute, value);
            button.Select();


            //HtmlListItem html_listItem = new HtmlListItem(HtmlComboBox);
            //html_listItem.SearchProperties.Add(HtmlListItem.PropertyNames.ValueAttribute, str_Value);
            //html_listItem.Select();
        }

        public void ClickLink(UITestControl parent, string innerText)
        {
            var link = new HtmlHyperlink(parent);
            link.SearchProperties.Add(HtmlHyperlink.PropertyNames.InnerText, innerText);
            Mouse.Click(link);
        }

        public void ClickHeader(UITestControl parent, string value)
        {
            var link = new HtmlHyperlink(parent);
            link.SearchProperties.Add(HtmlHyperlink.PropertyNames.ValueAttribute, value);
            Mouse.Click(link);
        }

        public void ClickLabel(UITestControl parent, string value)
        {
            var link = new HtmlLabel(parent);
            link.SearchProperties.Add(HtmlHyperlink.PropertyNames.Id, value);

            Mouse.Click(link);
        }

        public void ClickHead(UITestControl parent, string value)
        {
            var link = new HtmlListItem(parent);
            link.SearchProperties.Add(HtmlListItem.PropertyNames.Id, value);

            Mouse.Click(link);
        }

        public void ClickableLabel(UITestControl parent, string value)
        {
            var link = new HtmlAreaHyperlink(parent);
            link.SearchProperties.Add(HtmlHyperlink.PropertyNames.Id, value);
            Mouse.Click(link);
        }
    }
}

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Data;
using OpenQA.Selenium.Interactions;

namespace AUi
{
    public static class AUi
    {
        private static string selectorPath;  // field
        public static string SelectorPath    // property
        {
            get { return selectorPath; }
            set { selectorPath = value; }
        }

        private static string selectorConfig;  // field
        public static string SelectorConfig    // property
        {
            get { return selectorConfig; }
            set { selectorConfig = value; }
        }

        private static IWebDriver webDriver;  // field
        public static IWebDriver WebDriver   // property
        {
            get { return webDriver; }
            set { webDriver = value; }
        }

        private static ChromeDriver chDriver;  // field
        public static ChromeDriver ChDriver   // property
        {
            get { return chDriver; }
            set { chDriver = value; }
        }
       
        public static string GetCSS(string SelectorName)
        {
            string Selector = "";
            string Jsonstring = "";
            if (String.IsNullOrEmpty(SelectorConfig))
            {
                Jsonstring = File.ReadAllText(SelectorPath);
            }
            else
            {
                Jsonstring = SelectorConfig;
            }
            
            JObject Jobj = JObject.Parse(Jsonstring);
            IDictionary<string, JToken> Jsondata = JObject.Parse(Jsonstring);
            foreach (KeyValuePair<string, JToken> element in Jsondata)
            {
                string innerKey = element.Key;                
                JArray a = (JArray)Jobj[element.Key];
                foreach (var sel in a)
                {
                    if (sel["selectorname"].ToString() == SelectorName)
                    {                        
                        Selector = sel["selector-css"].ToString();
                        break;
                    }

                }
                break;
            }
            if(String.IsNullOrEmpty(Selector))
            {
                throw (new System.Exception("Selector file missing or invalid file name"));
            }
            return(Selector);
        }

        public static string GetXpath(string SelectorName)
        {
            string Selector = "";
            string Jsonstring = "";
            if (String.IsNullOrEmpty(SelectorConfig))
            {
                Jsonstring = File.ReadAllText(SelectorPath);
            }
            else
            {
                Jsonstring = SelectorConfig;
            }

            JObject Jobj = JObject.Parse(Jsonstring);
            IDictionary<string, JToken> Jsondata = JObject.Parse(Jsonstring);
            foreach (KeyValuePair<string, JToken> element in Jsondata)
            {
                string innerKey = element.Key;
                JArray a = (JArray)Jobj[element.Key];
                foreach (var sel in a)
                {
                    if (sel["selectorname"].ToString() == SelectorName)
                    {
                        Selector = sel["selector-xpath"].ToString();
                        break;
                    }

                }
                break;
            }
            if (String.IsNullOrEmpty(Selector))
            {
                throw (new System.Exception("Selector file missing or invalid file name"));
            }
            return (Selector);
        }

        public static string GetID(string SelectorName)
        {
            string Selector = "";
            string Jsonstring = "";
            if (String.IsNullOrEmpty(SelectorConfig))
            {
                Jsonstring = File.ReadAllText(SelectorPath);
            }
            else
            {
                Jsonstring = SelectorConfig;
            }

            JObject Jobj = JObject.Parse(Jsonstring);
            IDictionary<string, JToken> Jsondata = JObject.Parse(Jsonstring);
            foreach (KeyValuePair<string, JToken> element in Jsondata)
            {
                string innerKey = element.Key;
                JArray a = (JArray)Jobj[element.Key];
                foreach (var sel in a)
                {
                    if (sel["selectorname"].ToString() == SelectorName)
                    {
                        Selector = sel["selector-id"].ToString();
                        break;
                    }

                }
                break;
            }
            if (String.IsNullOrEmpty(Selector))
            {
                throw (new System.Exception("Selector file missing or invalid file name"));
            }
            return (Selector);
        }

        public static string GetClass(string SelectorName)
        {
            string Selector = "";
            string Jsonstring = "";
            if (String.IsNullOrEmpty(SelectorConfig))
            {
                Jsonstring = File.ReadAllText(SelectorPath);
            }
            else
            {
                Jsonstring = SelectorConfig;
            }

            JObject Jobj = JObject.Parse(Jsonstring);
            IDictionary<string, JToken> Jsondata = JObject.Parse(Jsonstring);
            foreach (KeyValuePair<string, JToken> element in Jsondata)
            {
                string innerKey = element.Key;
                JArray a = (JArray)Jobj[element.Key];
                foreach (var sel in a)
                {
                    if (sel["selectorname"].ToString() == SelectorName)
                    {
                        Selector = sel["selector-classname"].ToString();
                        break;
                    }

                }
                break;
            }
            if (String.IsNullOrEmpty(Selector))
            {
                throw (new System.Exception("Selector file missing or invalid file name"));
            }
            return (Selector);
        }

        public static string GetTag(string SelectorName)
        {
            string Selector = "";
            string Jsonstring = "";
            if (String.IsNullOrEmpty(SelectorConfig))
            {
                Jsonstring = File.ReadAllText(SelectorPath);
            }
            else
            {
                Jsonstring = SelectorConfig;
            }

            JObject Jobj = JObject.Parse(Jsonstring);
            IDictionary<string, JToken> Jsondata = JObject.Parse(Jsonstring);
            foreach (KeyValuePair<string, JToken> element in Jsondata)
            {
                string innerKey = element.Key;
                JArray a = (JArray)Jobj[element.Key];
                foreach (var sel in a)
                {
                    if (sel["selectorname"].ToString() == SelectorName)
                    {
                        Selector = sel["selector-tagname"].ToString();
                        break;
                    }

                }
                break;
            }
            if (String.IsNullOrEmpty(Selector))
            {
                throw (new System.Exception("Selector file missing or invalid file name"));
            }
            return (Selector);
        }
        
        public static IWebElement FindElementByXpath(string xpath,IWebDriver driver = null)
        {
            if(driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {
                    
                    element = driver.FindElement(By.XPath(GetXpath(xpath)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(30))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }
       
        public static IWebElement FindElementByCss(string css, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetCSS(css)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement FindElementById(string id, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.Id(GetID(id)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement FindElementByClassName(string classname, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.ClassName(GetClass(classname)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement FindElementByTagName(string tag, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.TagName(GetTag(tag)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }
       
        public static IWebElement FindElementByLinkText(string linkText, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.LinkText(linkText));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement FindElementByPartialLinkText(string partialLinkText, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.PartialLinkText(partialLinkText));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static bool ElementExistByXpath(string xpath, int timeout = 3, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            bool ElementExistFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetXpath(xpath)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                    ElementExistFlag = true;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        FindElementFlag = false;
                        ElementExistFlag = false;

                    }

                }
            }
            
            return ElementExistFlag;
        }
                
        public static bool ElementExistByCss(string css, int timeout = 3, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            bool ElementExistFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetCSS(css)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                    ElementExistFlag = true;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        FindElementFlag = false;
                        ElementExistFlag = false;
                    }
                }
            }

            return ElementExistFlag;
        }

        public static bool ElementExistById(string id, int timeout = 3, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            bool ElementExistFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetID(id)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                    ElementExistFlag = true;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        FindElementFlag = false;
                        ElementExistFlag = false;
                    }
                }
            }

            return ElementExistFlag;
        }

        public static bool ElementExistByClassName(string classname, int timeout = 3, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            bool ElementExistFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetClass(classname)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                    ElementExistFlag = true;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        FindElementFlag = false;
                        ElementExistFlag = false;
                    }
                }
            }

            return ElementExistFlag;
        }

        public static bool ElementExistByTagName(string tag, int timeout = 3, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            bool ElementExistFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetTag(tag)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                    ElementExistFlag = true;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        FindElementFlag = false;
                        ElementExistFlag = false;
                    }
                }
            }

            return ElementExistFlag;
        }

        public static bool ElementExistByLinkText(string linkText, int timeout = 3, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            bool ElementExistFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.LinkText(linkText));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                    ElementExistFlag = true;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        FindElementFlag = false;
                        ElementExistFlag = false;
                    }
                }
            }

            return ElementExistFlag;
        }
        
        public static bool ElementExistByPartialLinkText(string partalLinkText, int timeout = 3, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            bool ElementExistFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.PartialLinkText(partalLinkText));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                    ElementExistFlag = true;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        FindElementFlag = false;
                        ElementExistFlag = false;
                    }
                }
            }

            return ElementExistFlag;
        }

        public static IWebElement ClickElementByXpath(string xpath, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetXpath(xpath)));
                    element.Click();
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement ClickElementByCss(string css, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetCSS(css)));
                    element.Click();
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement ClickElementById(string id, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.Id(GetID(id)));
                    element.Click();
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement ClickElementByClassName(string classname, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.ClassName(GetClass(classname)));
                    element.Click();
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement ClickElementByTagName(string tag, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.TagName(GetTag(tag)));
                    element.Click();
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }
        
        public static IWebElement ClickElementByLinkText(string linkText, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.LinkText(linkText));
                    element.Click();
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement ClickElementByPartialLinkText(string partialLinkText, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.PartialLinkText(partialLinkText));
                    element.Click();
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }
        
        public static IWebElement ClearElementByXpath(string xpath, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetXpath(xpath)));
                    element.Clear();
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement ClearElementByCss(string css, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetCSS(css)));
                    element.Clear();
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement ClearElementById(string id, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.Id(GetID(id)));
                    element.Clear();
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement ClearElementByClassName(string classname, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.ClassName(GetClass(classname)));
                    element.Clear();
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement ClearElementByTagName(string tag, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.TagName(GetTag(tag)));
                    element.Clear();
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }
       
        public static IWebElement ClearElementByLinkText(string linkText, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.LinkText(linkText));
                    element.Clear();
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }
        
        public static IWebElement ClearElementByPartialLinkText(string partialLinkText, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.PartialLinkText(partialLinkText));
                    element.Clear();
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }
        
        public static IWebElement TypeIntoElementByXpath(string xpath,string text, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetXpath(xpath)));
                    element.SendKeys(text);
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement TypeIntoElementByCss(string css, string text, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetCSS(css)));
                    element.SendKeys(text);
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement TypeIntoElementById(string id, string text, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.Id(GetID(id)));
                    element.SendKeys(text);
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement TypeIntoElementByClassName(string classname, string text, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.ClassName(GetClass(classname)));
                    element.SendKeys(text);
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement TypeIntoElementByTagName(string tag, string text, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.TagName(GetTag(tag)));
                    element.SendKeys(text);
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }
        
        public static IWebElement TypeIntoElementByLinkText(string linkText, string text, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.LinkText(linkText));
                    element.SendKeys(text);
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }
        
        public static IWebElement TypeIntoElementByPartialLinkText(string partialLinkText, string text, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.PartialLinkText(partialLinkText));
                    element.SendKeys(text);
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }
        
        public static bool IsEnabledByXpath(string xpath, int timeout = 3, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            bool ElementExistFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetXpath(xpath)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                    ElementExistFlag = element.Enabled;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        FindElementFlag = false;
                        ElementExistFlag = true;

                    }
                }
            }

            return ElementExistFlag;
        }

        public static bool IsEnabledByCss(string css, int timeout = 3, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            bool ElementExistFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetCSS(css)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                    ElementExistFlag = element.Enabled;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        FindElementFlag = false;
                        ElementExistFlag = true;
                    }
                }
            }

            return ElementExistFlag;
        }

        public static bool IsEnabledById(string id, int timeout = 3, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            bool ElementExistFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetID(id)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                    ElementExistFlag = element.Enabled;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        FindElementFlag = false;
                        ElementExistFlag = true;
                    }
                }
            }

            return ElementExistFlag;
        }

        public static bool IsEnabledByClassName(string classname, int timeout = 3, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            bool ElementExistFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetClass(classname)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                    ElementExistFlag = element.Enabled;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        FindElementFlag = false;
                        ElementExistFlag = true;
                    }
                }
            }

            return ElementExistFlag;
        }

        public static bool IsEnabledByTagName(string tag, int timeout = 3, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            bool ElementExistFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetTag(tag)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                    ElementExistFlag = element.Enabled;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        FindElementFlag = false;
                        ElementExistFlag = true;
                    }
                }
            }

            return ElementExistFlag;
        }
        
        public static bool IsEnabledByLinkText(string linkText, int timeout = 3, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            bool ElementExistFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.LinkText(linkText));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                    ElementExistFlag = element.Enabled;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        FindElementFlag = false;
                        ElementExistFlag = true;
                    }
                }
            }

            return ElementExistFlag;
        }
        
        public static bool IsEnabledByPartialLinkText(string partialLinkText, int timeout = 3, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            bool ElementExistFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.PartialLinkText(partialLinkText));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                    ElementExistFlag = element.Enabled;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        FindElementFlag = false;
                        ElementExistFlag = true;
                    }
                }
            }

            return ElementExistFlag;
        }

        public static bool IsSelectedByXpath(string xpath, int timeout = 3, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            bool ElementExistFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetXpath(xpath)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                    ElementExistFlag = element.Selected;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        FindElementFlag = false;
                        ElementExistFlag = true;

                    }
                }
            }

            return ElementExistFlag;
        }

        public static bool IsSelectedByCss(string css, int timeout = 3, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            bool ElementExistFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetCSS(css)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                    ElementExistFlag = element.Selected;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        FindElementFlag = false;
                        ElementExistFlag = true;
                    }
                }
            }

            return ElementExistFlag;
        }

        public static bool IsSelectedById(string id, int timeout = 3, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            bool ElementExistFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetID(id)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                    ElementExistFlag = element.Selected;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        FindElementFlag = false;
                        ElementExistFlag = true;
                    }
                }
            }

            return ElementExistFlag;
        }

        public static bool IsSelectedByClassName(string classname, int timeout = 3, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            bool ElementExistFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetClass(classname)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                    ElementExistFlag = element.Selected;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        FindElementFlag = false;
                        ElementExistFlag = true;
                    }
                }
            }

            return ElementExistFlag;
        }

        public static bool IsSelectedByTagName(string tag, int timeout = 3, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            bool ElementExistFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetTag(tag)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                    ElementExistFlag = element.Selected;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        FindElementFlag = false;
                        ElementExistFlag = true;
                    }
                }
            }

            return ElementExistFlag;
        }
       
        public static bool IsSelectedByLinkText(string linkText, int timeout = 3, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            bool ElementExistFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.LinkText(linkText));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                    ElementExistFlag = element.Selected;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        FindElementFlag = false;
                        ElementExistFlag = true;
                    }
                }
            }

            return ElementExistFlag;
        }
        
        public static bool IsSelectedByPartialLinkText(string partialLinkText, int timeout = 3, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            bool ElementExistFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.PartialLinkText(partialLinkText));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                    ElementExistFlag = element.Selected;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        FindElementFlag = false;
                        ElementExistFlag = true;
                    }
                }
            }

            return ElementExistFlag;
        }
        
        public static IWebElement SubmitByXpath(string xpath, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetXpath(xpath)));
                    element.Submit();
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement SubmitByCss(string css, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetCSS(css)));
                    element.Submit();
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement SubmitById(string id, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.Id(GetID(id)));
                    element.Submit();
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement SubmitByClassName(string classname, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.ClassName(GetClass(classname)));
                    element.Submit();
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement SubmitByTagName(string tag, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.TagName(GetTag(tag)));
                    element.Submit();
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }
        
        public static IWebElement SubmitByLinkText(string linkText, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.LinkText(linkText));
                    element.Submit();
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }
        
        public static IWebElement SubmitByPartialLinkText(string partialLinkText, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.PartialLinkText(partialLinkText));
                    element.Submit();
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }
        
        public static string GetTextByXpath(string xpath, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetXpath(xpath)));                    
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element.Text;
        }

        public static string GetTextByCss(string css, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetCSS(css)));                    
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element.Text;
        }

        public static string GetTextById(string id, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.Id(GetID(id)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element.Text;
        }

        public static string GetTextClassName(string classname, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.ClassName(GetClass(classname)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element.Text;
        }

        public static string GetTextByTagName(string tag, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.TagName(GetTag(tag)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element.Text;
        }
        
        public static string GetTextByLinkText(string linkText, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.LinkText(linkText));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element.Text;
        }
        
        public static string GetTextByPartialLinkText(string partialLinkText, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.PartialLinkText(partialLinkText));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element.Text;
        }

        public static string GetTagNameByXpath(string xpath, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetXpath(xpath)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element.TagName;
        }

        public static string GetTagNameByCss(string css, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetCSS(css)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element.TagName;
        }

        public static string GetTagNameById(string id, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.Id(GetID(id)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element.TagName;
        }

        public static string GetTagNameByClassName(string classname, int timeout = 30,  IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.ClassName(GetClass(classname)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element.TagName;
        }

        public static string GetTagNameByTagName(string tag, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.TagName(GetTag(tag)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element.TagName;
        }

        public static string GetCssByXpath(string xpath, string property, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetXpath(xpath)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element.GetCssValue(property);
        }

        public static string GetCssByCss(string css, string property, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetCSS(css)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element.GetCssValue(property);
        }

        public static string GetCssById(string id, string property, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.Id(GetID(id)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element.GetCssValue(property);
        }

        public static string GetCssByClassName(string classname, string property, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.ClassName(GetClass(classname)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element.GetCssValue(property);
        }

        public static string GetCssByTagName(string tag, string property, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.TagName(GetTag(tag)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element.GetCssValue(property);
        }

        public static string GetAttributeByXpath(string xpath, string attribute, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetXpath(xpath)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element.GetAttribute(attribute);
        }

        public static string GetAttributeByCss(string css, string attribute, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetCSS(css)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element.GetAttribute(attribute);
        }

        public static string GetAttributeById(string id, string attribute, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.Id(GetID(id)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element.GetAttribute(attribute);
        }

        public static string GetAttributeByClassName(string classname, string attribute, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.ClassName(GetClass(classname)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element.GetAttribute(attribute);
        }

        public static string GetAttributeByTagName(string tag, string attribute, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.TagName(GetTag(tag)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element.GetAttribute(attribute);
        }

        public static string GetPropertyByXpath(string xpath, string property, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetXpath(xpath)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element.GetDomProperty(property);
        }

        public static string GetPropertyByCss(string css, string property, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetCSS(css)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element.GetDomProperty(property);
        }

        public static string GetPropertyById(string id, string property, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.Id(GetID(id)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element.GetDomProperty(property);
        }

        public static string GetPropertyByClassName(string classname, string property, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.ClassName(GetClass(classname)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element.GetDomProperty(property);
        }

        public static string GetPropertyByTagName(string tag, string property, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.TagName(GetTag(tag)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element.GetDomProperty(property);
        }

        public static IWebElement SelectTextByXpath(string xpath, string text, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetXpath(xpath)));
                    SelectElement select = new SelectElement(element);
                    select.SelectByText(text);
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement SelectTextByCss(string css, string text, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetCSS(css)));
                    SelectElement select = new SelectElement(element);
                    select.SelectByText(text);
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement SelectTextById(string id, string text, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.Id(GetID(id)));
                    SelectElement select = new SelectElement(element);
                    select.SelectByText(text);
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement SelectTextByClassName(string classname, string text, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.ClassName(GetClass(classname)));
                    SelectElement select = new SelectElement(element);
                    select.SelectByText(text);
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement SelectTextByTagName(string tag, string text, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.TagName(GetTag(tag)));
                    SelectElement select = new SelectElement(element);
                    select.SelectByText(text);
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement SelectByIndexByXpath(string xpath, int index, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {
                    element = driver.FindElement(By.XPath(GetXpath(xpath)));
                    SelectElement select = new SelectElement(element);
                    select.SelectByIndex(index);
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement SelectByIndexByCss(string css, int index, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetCSS(css)));
                    SelectElement select = new SelectElement(element);
                    select.SelectByIndex(index);
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement SelectByIndexById(string id, int index, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.Id(GetID(id)));
                    SelectElement select = new SelectElement(element);
                    select.SelectByIndex(index);
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement SelectByIndexByClassName(string classname, int index, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.ClassName(GetClass(classname)));
                    SelectElement select = new SelectElement(element);
                    select.SelectByIndex(index);
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement SelectByIndexByTagName(string tag, int index, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.TagName(GetTag(tag)));
                    SelectElement select = new SelectElement(element);
                    select.SelectByIndex(index);
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> FindElementsByXpath(string xpath, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElements(By.XPath(GetXpath(xpath)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> FindElementsByCss(string css, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElements(By.XPath(GetCSS(css)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> FindElementsById(string id, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElements(By.Id(GetID(id)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> FindElementsByClassName(string classname, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElements(By.ClassName(GetClass(classname)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                    
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> FindElementsByTagName(string tag, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElements(By.TagName(GetTag(tag)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static System.Data.DataTable ScrapeByXpath(string[] selector, string[] columns = null, ChromeDriver cdriver = null)
        {

            if (cdriver == null)
            {
                cdriver = ChDriver;
            }

            DataTable dt = new DataTable();
            if (columns != null)
            {
                foreach (var col in columns)
                {
                    dt.Columns.Add(col.ToString());
                }
            }
            else
            {
                foreach (var col in selector)
                {
                    dt.Columns.Add(col.ToString());
                }
            }
            int count = cdriver.FindElements(By.XPath(GetXpath(selector[0]))).Count;

            foreach (int i in Enumerable.Range(1, count))
            {
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
            }

            int colindex = 0;
            foreach (var col in selector)
            {
                int rowindex = 0;
                foreach (var ele in cdriver.FindElements(By.XPath(GetXpath(col))))
                {
                    try
                    {
                        dt.Rows[rowindex][colindex] = ele.Text;
                        rowindex = rowindex + 1;
                    }
                    catch (Exception e)
                    {

                    }

                }
                colindex = colindex + 1;
            }
            return dt;
        }

        public static System.Data.DataTable ScrapeElementByXpath(string[] selector, string[] columns = null, ChromeDriver cdriver = null)
        {

            if (cdriver == null)
            {
                cdriver = ChDriver;
            }

            DataTable dt = new DataTable();
            if (columns != null)
            {
                foreach (var col in columns)
                {
                    dt.Columns.Add(col.ToString(), typeof(IWebElement));
                }
            }
            else
            {
                foreach (var col in selector)
                {
                    dt.Columns.Add(col.ToString(), typeof(IWebElement));
                }
            }
            int count = cdriver.FindElements(By.XPath(GetXpath(selector[0]))).Count;

            foreach (int i in Enumerable.Range(1, count))
            {
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
            }

            int colindex = 0;
            foreach (var col in selector)
            {
                int rowindex = 0;
                foreach (var ele in cdriver.FindElements(By.XPath(GetXpath(col))))
                {
                    try
                    {
                        dt.Rows[rowindex][colindex] = ele;
                        rowindex = rowindex + 1;
                    }
                    catch (Exception e)
                    {

                    }

                }
                colindex = colindex + 1;
            }
            return dt;
        }

        public static IWebElement WaitForElementAppearByXpath(string xpath,int timeout=30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;           
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetXpath(xpath)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement WaitForElementAppearByClass(string classname, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.ClassName(GetClass(classname)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement WaitForElementAppearByCss(string css, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.CssSelector(GetCSS(css)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement WaitForElementAppearById(string id, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.Id(GetID(id)));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }
        
        public static IWebElement WaitForElementAppearLinkText(string linkText, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.LinkText(linkText));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }
        
        public static IWebElement WaitForElementAppearPartialLinkText(string partialLinkText, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.PartialLinkText(partialLinkText));
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }
        
        public static void HoverElementByXpath(string xpath, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            while (FindElementFlag)
            {
                try
                {

                    Actions action = new Actions(driver);
                    action.MoveToElement(driver.FindElement(By.XPath(GetXpath(xpath)))).Perform();
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
        }

        public static void HoverElementById(string id, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            while (FindElementFlag)
            {
                try
                {

                    Actions action = new Actions(driver);
                    action.MoveToElement(driver.FindElement(By.Id(GetID(id)))).Perform();
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
        }

        public static void HoverElementByClassName(string classname, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            while (FindElementFlag)
            {
                try
                {

                    Actions action = new Actions(driver);
                    action.MoveToElement(driver.FindElement(By.ClassName(GetClass(classname)))).Perform();
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
        }

        public static void HoverElementByCss(string css, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            while (FindElementFlag)
            {
                try
                {

                    Actions action = new Actions(driver);
                    action.MoveToElement(driver.FindElement(By.CssSelector(GetCSS(css)))).Perform();
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
        }
       
        public static void HoverElementByLinkText(string linkText, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            while (FindElementFlag)
            {
                try
                {

                    Actions action = new Actions(driver);
                    action.MoveToElement(driver.FindElement(By.LinkText(linkText))).Perform();
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
        }
       
        public static void HoverElementByPartialLinkText(string partialLinkText, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            while (FindElementFlag)
            {
                try
                {

                    Actions action = new Actions(driver);
                    action.MoveToElement(driver.FindElement(By.PartialLinkText(partialLinkText))).Perform();
                    //Check web page completely loaded.
                    FindElementFlag = false;
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
        }

        public static IWebElement WaitForElementVanishByXpath(string xpath, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.XPath(GetXpath(xpath)));
                    //Check web page completely loaded.
                    
                }
                catch (Exception ex)
                {
                    FindElementFlag = false;
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement WaitForElementVanishByClass(string classname, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.ClassName(GetClass(classname)));
                    //Check web page completely loaded.
                    
                }
                catch (Exception ex)
                {
                    FindElementFlag = false;
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement WaitForElementVanishByCss(string css, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.CssSelector(GetCSS(css)));
                    //Check web page completely loaded.
                    
                }
                catch (Exception ex)
                {
                    FindElementFlag = false;
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }

        public static IWebElement WaitForElementVanishById(string id, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.Id(GetID(id)));
                    //Check web page completely loaded.
                   
                }
                catch (Exception ex)
                {
                    FindElementFlag = false;
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }
        
        public static IWebElement WaitForElementVanishByLinkText(string linkText, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.LinkText(linkText));
                    //Check web page completely loaded.

                }
                catch (Exception ex)
                {
                    FindElementFlag = false;
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }
       
        public static IWebElement WaitForElementVanishByPartialLinkText(string partialLinkText, int timeout = 30, IWebDriver driver = null)
        {
            if (driver == null)
            {
                driver = WebDriver;
            }
            DateTime DateTimeVar = DateTime.Now;
            bool FindElementFlag = true;
            IWebElement element = null;
            while (FindElementFlag)
            {
                try
                {

                    element = driver.FindElement(By.PartialLinkText(partialLinkText));
                    //Check web page completely loaded.

                }
                catch (Exception ex)
                {
                    FindElementFlag = false;
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
            return element;
        }
    }
}

using System;
using System.Collections.ObjectModel;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;


namespace WebTesting
{
    class FilterPage
    {
        private IWebDriver _webDriver = null;

        const int Week = 7;

        By CityField = By.XPath("//input[@id='ss']");
        By CalendarButton = By.XPath("//div[contains(@class,'xp__dates-inner xp__dates__checkin')]//span[contains(@class,'sb-date-field__icon sb-date-field__icon-btn bk-svg-wrapper calendar-restructure-sb')]");
        By LeftCalendarTable = By.XPath("//body[@id='b2indexPage']/div[contains(@class,'xpi__content__wrapper xpi__content__wrappergray')]/div[contains(@class,'xpi__content__container')]/div[contains(@class,'xpi__searchbox js-ds-layout-events-search-form')]/div[contains(@class,'sb-searchbox__outer')]/form[@id='frm']/div[contains(@class,'xp__fieldset js--sb-fieldset accommodation')]/div[contains(@class,'xp__dates xp__group')]/div[contains(@class,'xp-calendar')]/div[contains(@class,'bui-calendar')]/div[contains(@class,'bui-calendar__main b-a11y-calendar-contrasts')]/div[contains(@class,'bui-calendar__content')]/div[1]");
        By RightCalendarTable = By.XPath("//form[1]//div[1]//div[2]//div[2]//div[1]//div[1]//div[3]//div[2]");
        By NextMonthButtonXPath = By.XPath("//body[@id='b2indexPage']/div[contains(@class,'xpi__content__wrapper xpi__content__wrappergray')]/div[@class='xpi__content__container']/div[@class='xpi__searchbox js-ds-layout-events-search-form']/div[2]/form[1]/div[1]/div[2]/div[2]/div[1]/div[1]/div[2]/*[1] ");
        By DayTag = By.TagName("td");
        
        By PeoplePanelLocator = By.XPath("//label[@id='xp__guests__toggle']");
        By adultsCountLocator = By.XPath("//span[@class='bui-stepper__display'][contains(text(),'2')]");
        By adultsPlusButton = By.XPath("//div[@class='sb-group__field sb-group__field-adults']//span[@class='bui-button__text'][contains(text(),'+')] ");
        By adultsMinusButton = By.XPath("//div[@class='sb-group__field sb-group__field-adults']//button[contains(@class,'bui-button bui-button--secondary bui-stepper__subtract-button')] ");
        
        By childrenCountLocator = By.XPath("//div[contains(@class,'sb-group__field sb-group-children')]//span[@class='bui-stepper__display'][contains(text(),'0')] ");
        By childrenPlusButton = By.XPath("//div[contains(@class,'sb-group__field sb-group-children')]//span[@class='bui-button__text'][contains(text(),'+')] ");
        By childrenMinusButton = By.XPath("//div[contains(@class,'sb-group__field sb-group-children')]//button[contains(@class,'bui-button bui-button--secondary bui-stepper__subtract-button')]//span[@class='bui-button__text'] ");
        
        By roomCountLocator = By.XPath("//div[@class='sb-group__field sb-group__field-rooms']//span[@class='bui-stepper__display'][contains(text(),'1')]");
        By roomPlusButton = By.XPath("//div[@class='sb-group__field sb-group__field-rooms']//button[contains(@class,'bui-button bui-button--secondary bui-stepper__add-button')] ");
        By roomMinusButton = By.XPath("//button[@class='bui-button bui-button--secondary bui-stepper__subtract-button sb-group__stepper-button-disabled']//span[@class='bui-button__text'] ");


        public FilterPage(IWebDriver webDriver) {

            _webDriver = webDriver;  
        }

        public FilterPage TypeCity(string name) {

            _webDriver.FindElement(CityField).SendKeys(name);
            return this;
        }

        private string GetCurrentDay() {

            string result = string.Empty;
            string today = DateTime.Today.Day.ToString();

            foreach (var it in today) {

                if (it == '.')
                {
                    break;
                }
                else
                {
                    result += it;
                }
            }

            return result;

        }


        public FilterPage TypeDate(int duration) {

            _webDriver.FindElement(CalendarButton).Click();
           
            IWebElement nextMonthButton = _webDriver.FindElement(NextMonthButtonXPath);
            IWebElement dateWidget = _webDriver.FindElement(LeftCalendarTable);

            ReadOnlyCollection<IWebElement> days = dateWidget.FindElements(DayTag);

            int i;
            string today = GetCurrentDay();

            int leavingDay;
            bool leavingDetermined = false;
            int position = 0;

            for (i = 0; i < days.Count; i++)
            {
                if (days[i].Text == today)
                {                    
                    leavingDay = i + Week;

                    //Console.WriteLine(leavingDay + " / " + (days.Count - 1));

                    if (leavingDay > days.Count - 1 || days[leavingDay].Text == string.Empty)
                    {
                        position = i;                        
                    }
                    else
                    {
                        days[leavingDay].Click();
                        leavingDetermined = true;                        
                    }

                    break;

                };

            }

            int difference = 0;

            if (!leavingDetermined)
            {
                for (i = position; i < days.Count; i++)
                {
                    if (days[i].Text == string.Empty)
                    {
                        difference = Week - (i - position);
                        nextMonthButton.Click();
                        break;
                    }
                }

                dateWidget = _webDriver.FindElement(LeftCalendarTable);
                days = dateWidget.FindElements(DayTag);

                for (i = 0; i < days.Count; i++)
                {
                    if (days[i].Text != string.Empty)
                    {
                        days[i + difference].Click();
                        break;
                    }
                }

            }

            dateWidget = _webDriver.FindElement(RightCalendarTable);
            days = dateWidget.FindElements(DayTag);

            for (i = 0; i < days.Count; i++)
            {
                if (days[i].Text != string.Empty)
                {
                    days[i + difference + duration].Click();
                    break;
                }
            }

            return this;
        }

        private void InputCountToServer(int count, By countView, By plusButton, By minusButton)
        {
            IWebElement realCountView = _webDriver.FindElement(countView);
            int realCount = int.Parse(realCountView.Text), i;

            int difference = count - realCount;

            if (realCount < count)
            {
                IWebElement button = _webDriver.FindElement(plusButton);

                for (i = 0; i < difference; i++)
                {
                    button.Click();
                }

            }
            else if (realCount > count)
            {
                IWebElement button = _webDriver.FindElement(minusButton);

                for (i = 0; i < difference; i++)
                {
                    button.Click();
                }
            }
        }


        public FilterPage TypePeople(int adults, int children, int rooms) {

            _webDriver.FindElement(PeoplePanelLocator).Click();         

            InputCountToServer(adults, adultsCountLocator, adultsPlusButton, adultsMinusButton);
            InputCountToServer(children, childrenCountLocator, childrenPlusButton, childrenMinusButton);
            InputCountToServer(rooms, roomCountLocator, roomPlusButton, roomMinusButton);

            return this;        
        }


    }
}

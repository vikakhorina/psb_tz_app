using System;

namespace HorinaTest.App1.MVVM.Model
{
    public class UserInfo
    {
        public string FullName { get; set; }
        public int YearDate { get; set; }

        public int Age { get; set; }
        public string ShortName { get; set; }
        public char Male { get; set; }
        public int YearToRetiree { get; set; }

        public UserInfo(string fullName, int yearDate, int retireeWomen, int retireeMen)
        {
            FullName = fullName;
            YearDate = yearDate;

            Age = (byte)(DateTime.Today.Year - yearDate);

            if (Age < 0)
                throw new Exception($"некорректный возраст для даты рождения: {yearDate}");

            string[] arrName = FullName.Split(' ');
            if (arrName.Length != 3)
                throw new Exception($"некорректное ФИО - {FullName}");

            ShortName = $"{arrName[0]} {arrName[1][..1]}. {arrName[2][..1]}.";

            var lastName = arrName[2];
            char lastSymbol = lastName[lastName.Length - 1];
            if (char.ToLower(lastSymbol) == 'ч')
            {
                Male = 'M';
                YearToRetiree = retireeMen - Age;
            }
            else
            {
                Male = 'W';
                YearToRetiree = retireeWomen - Age;
            }
        }

    }
}

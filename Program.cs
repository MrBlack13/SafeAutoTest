using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace SafeAutoTest
{

    public class Driver
    {
        public  string driverName { get; set; }
        public  double averageSpeedCalc { get; set; }
        public  int averageSpeed { get; set; }
        public  double milesTotalCalc{ get; set; } 
        public  int milesTotal{ get; set; }
        public  Double minsTotal { get; set; }
        public  DateTime startTime { get; set; }
        public  DateTime endTime { get; set; }
        public  double milage { get; set; }
        
        IList<Trip> trips = new List<Trip>();



        public void AddTrip(DateTime startTime, DateTime endTime, Double milage) 
		{
            bool overlap = false;
            foreach(var trip in trips){
                overlap = startTime < trip.endTime && trip.startTime < endTime;
            }
            if(overlap){
                Console.WriteLine("Impossible Trip! One driver cannot have two trips that overlap in times.");
                Console.WriteLine(this.driverName + "'s trips are:");
                foreach(var trip in trips){
                    Console.WriteLine(trip.startTime.ToString("HH:mm")  + " - " + trip.endTime.ToString("HH:mm") + " " + trip.milage);
                }

                System.Threading.Thread.Sleep(5000);

            } else {
                trips.Add(new Trip() {startTime=startTime,endTime=endTime,milage=milage});
                trips[(trips.Count - 1 )].SetTrip(startTime,endTime,milage);
                this.milesTotalCalc = 0;
                this.minsTotal = 0;
                foreach(var trip in trips){
                    if(trip.tripMPH < 100 && trip.tripMPH > 5){
                        this.milesTotalCalc += trip.milage;
                        this.minsTotal += trip.minutes;
                        this.averageSpeedCalc = (this.milesTotalCalc / this.minsTotal) * 60;
                        this.averageSpeed = Convert.ToInt32(System.Math.Round(this.averageSpeedCalc)); 
                        this.milesTotal = Convert.ToInt32(System.Math.Round(this.milesTotalCalc)); 
                    }
                }
            }
		} // Driver-AddTrip


    } //Driver

    class Trip {
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public double milage { get; set; }
        public double minutes { get; set; }
        public double tripMPH { get; set; }
        //method to set trip details
        public void SetTrip(DateTime startTime, DateTime endTime, double milage) 
		{
            TimeSpan interval = endTime - startTime;
			this.startTime  = startTime;
			this.endTime    = endTime;
            this.minutes    = interval.TotalMinutes;
			this.milage     = milage;
            this.tripMPH    = (milage / this.minutes)*60;
		}
         
    } // Trip
    class Program
    {
        static void Main(string[] args)
        {
            ShowSyntax();
            string line;
            IList<Driver> drivers = new List<Driver>();
            bool isDone = false;
            while (!isDone)
            {
                line = Console.ReadLine();
                string[] wordsInput = line.Split(' ');

                switch(wordsInput[0]) 
                {
                case "Driver": 
                    // Check Driver command syntax
                    if(DriverCommandSyntaxOK(wordsInput))
                    {
                        // Check if dirver already exists
                        if(!DriverExists(drivers,wordsInput[1])){
                            drivers.Add(new Driver() {driverName=wordsInput[1]});
                            DisplayStats(drivers);
                        } else {
                            DisplayStats(drivers);
                            Console.WriteLine("Driver " + wordsInput[1] + " already exists.");
                        }
                    }
                    break;
                case "Trip":
                    // Check Trip command syntax
                    if(TripCommandSyntaxOK(wordsInput)){
                        // Check if dirver already exists
                        if(DriverExists(drivers, wordsInput[1]))
                        {
                            DateTime startTime;
                            DateTime endTime;
                                        
                            DateTime.TryParse(wordsInput[2], out startTime);
                            DateTime.TryParse(wordsInput[3], out endTime);

                            AddTrip(drivers,wordsInput[1], startTime, endTime, Convert.ToDouble(wordsInput[4]));
                            DisplayStats(drivers);
                        } 
                        else
                        {
                            DisplayStats(drivers);
                            Console.WriteLine("Driver " + wordsInput[1] + " does not yet exist. Please use the Driver command to add the driver before adding a trip.");
                        }
                    }
                    break;
                case "Quit":
                    Console.WriteLine("We\'re done.  Thanks for playing!");
                    isDone = true;
                    break;
                default:
                    Console.Clear();
                    ShowSyntax();
                    break;
                }
            }
        } // Main
        static void ShowSyntax() 
        {
            Console.WriteLine("COMMAND SYNTAX:   Driver /name/                                      - Add new driver    - ex: Driver RacerX");
            Console.WriteLine("or                Trip /DriverName/ /startTime/ /endTime/ /miles/    - Add new trip      - ex: Trip RacerX 12:45 13:03 22.8");
            Console.WriteLine("or                Quit                                               - Quit");
        } //ShowSyntax

        static bool DriverCommandSyntaxOK(string[] inputArray)
        {
            // For now we only check if there are two values
            // maybe in the future look for only strings that can be a name
            if(inputArray.Length != 2)
            {
                Console.Clear();
                Console.WriteLine("Driver command has incorrect number of arguments.");
                ShowSyntax();
                return false;
            }
            else
            {
                return true;
            }
        } // DriverCommandSyntaxOK

        static bool TripCommandSyntaxOK(string[] inputArray)
        {
            // Check number of Arguments
            if(inputArray.Length != 5) 
            {
                Console.Clear();
                Console.WriteLine("Trip command has incorrect number of arguments.");
                ShowSyntax();
                return false;
            }

            // Check timestamps
            DateTime startTime;
            DateTime endTime;
            
            var isValidTime = DateTime.TryParse(inputArray[2], out startTime);
            // This is usually how I do brackets but in all the C# doc I am reading they put brackets on the next line.
            // It's just a matter preference. I'll do whatever the team agrees is the proper syntax.
            if (!isValidTime) {
                Console.Clear();
                Console.WriteLine("Trip command has invalid StartTime. The correct syntax is hh:mm in military time.");
                Console.WriteLine("Example: 13:00 = 1PM");
                return false;
            }
            // Am I not DRY here?  Do I sacrifice KISS for DRY??  It might be SOLID to have an error handling section.
            // BUT it might not be too KISS to have message numbers in a different section
            isValidTime = DateTime.TryParse(inputArray[3], out endTime);
            if (!isValidTime) {
                Console.Clear();
                Console.WriteLine("Trip command has invalid EndTime. The correct syntax is hh:mm in military time.");
                Console.WriteLine("Example: 13:00 = 1PM");
                return false;
            }

            TimeSpan interval = endTime - startTime;
            if( interval.TotalMinutes <= 0) {
                Console.Clear();
                Console.WriteLine("Trip syntax error: StartTime is after EndTime. Please fix and try again." + ( interval.TotalMinutes ));
                return false;
            }

            // Check Trip Miles
            if(Double.IsNaN(Convert.ToDouble(inputArray[4]))){
                Console.Clear();
                Console.WriteLine("Trip syntax error: Miles is not a number. Please fix and try again.");
                return false;
            }
            return true;
        } // TripCommandSyntaxOK

        static bool DriverExists(IList<Driver> driversList, string driver)
        {
            var drivers = from d in driversList
                       select new { driver = d.driverName };

            foreach(var drvr in drivers)
                if(drvr.driver == driver){
                    return true;
                }
            return false;
        } // DriverExists

        static void DisplayStats(IList<Driver> driversList)
        {
            var drivers = from d in driversList
                        orderby d.milesTotal descending
                       select new { driverName = d.driverName, averageSpeed = d.averageSpeed, milesTotal = d.milesTotal};
 
            Console.Clear();
            foreach(var drvr in drivers)
                if(drvr.milesTotal >0 )
                    Console.WriteLine(drvr.driverName + ": " + drvr.milesTotal + " miles @ " + drvr.averageSpeed + " mph");
                else
                    Console.WriteLine(drvr.driverName + ": " + drvr.milesTotal + " miles");

        } // DisplayStats

        static void AddTrip(IList<Driver> driversList,string driverName,DateTime startTime,DateTime endTime,Double milage)
        {
            foreach(var driver in driversList){
                if(driver.driverName == driverName){
                    driver.AddTrip(startTime,endTime,milage);
                }
            }
        } // AddTrip

    } //Program

}

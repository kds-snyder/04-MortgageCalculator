using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortgageCalculator
{
    class Program
    {

        static decimal homePrice, loanTerm, interestAnnual, downPayment,monthlyPayment,
                    numberPayments, totalMortgageCost, totalInterest;
        static void Main(string[] args)
        {
            try
            {
                // Get the mortgage information from the user
                inputMortgageInfo();             

                // Calculate the mortgage costs
                calcMortgageCosts();

                // Display the mortgage costs
                displayMortgageCosts();


            }
            catch (Exception e)
            {
                Console.WriteLine(" An error occurred: ");
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }

        // Get the user input: home price, loan term, annual interest rate, down payment
        // Make sure that the down payment is smaller than the home pricer
        static void inputMortgageInfo()
        {

            homePrice = inputMoney("Enter the price of the home ($) ");
            loanTerm = inputPosDecNum("Enter the loan term (years) ");
            interestAnnual = inputPercentage("Enter the interest (%) ") * .01M;

            // Ensure that down payment is less than home price
            // Allow down payment to be 0
            while (true)
            {
                downPayment = inputMoney("Enter the down payment ($) ", true);
                if (downPayment < homePrice)
                {
                    return;
                }
                else
                {
                    Console.WriteLine
                        ("The down payment must be less " +
                               "than the home price (${0})", homePrice);
                }
            }
        }

        //  Calculate the following: 
        //    -monthly payment
        //    -total cost of mortgage
        //    -total interest paid
        static void calcMortgageCosts()
        {
            decimal principal = homePrice - downPayment;
            numberPayments = loanTerm * 12;
            decimal interestMonthly = interestAnnual / 12;

            /* Monthly payment is:
               (P * I * (1 + I)^N ) / ((1 + I)^N - 1)
                (P = principal, I = monthly interest, 
                 N = total # monthly payments, ^ = raised to the power of)
            */              
            monthlyPayment = 
                (principal *
                interestMonthly * 
                   (decimal)Math.Pow((double) (1 + interestMonthly), (double)numberPayments)) /
                   ((decimal)Math.Pow((double)(1 + interestMonthly), (double)numberPayments) - 1);

            // Total mortgage cost is the total amount paid - the principal           
             totalMortgageCost = monthlyPayment * numberPayments;

            // Total interest paid is the total mortgage cost minus the principal
            totalInterest = totalMortgageCost - principal;
       }
        /* Display:
            -total number monthly payments
            -monthly payment amount
            -total cost of mortgage
            -interest earned
        */
       static void displayMortgageCosts()
        {
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("Number of monthly payments: {0}", numberPayments);
            Console.WriteLine("Monthly payment amount: {0}", monthlyPayment.ToString("C"));
            Console.WriteLine("Total cost of Mortgage: {0}", totalMortgageCost.ToString("C"));
            Console.WriteLine("Interest Earned: {0}", totalInterest.ToString("C"));
        }

        // Read in currency amount, allowing up to 2 decimal places
        static decimal inputMoney(string message, bool acceptZero = false)
        {
            while (true)
            {
                decimal inputAmount = inputPosDecNum(message, acceptZero);

                // Validate that there are no more than 2 decimal places
               if (inputAmount == Math.Round(inputAmount, 2))
                {
                    return inputAmount;
                }
                else
                {
                    Console.WriteLine("Please enter a number with no more than 2 decimal places.");
                }
            }
        }
        // Read in percentage; must be greater than 0 and less than 100
        static decimal inputPercentage(string message)
        {
            while (true)
            {
                decimal inputAmount = inputPosDecNum(message);

                // Validate that amount is less than 100
                if (inputAmount < 100)
                {
                    return inputAmount;
                }
                else
                {
                    Console.WriteLine("Please enter a number lower than 100.");
                }
            }
        }

        // Output message, then read in a decimal number that cannot be negative;
        //  it can be 0 if specified by user
        // If input is invalid, keep prompting
        // When input is valid, return the decimal number
        static decimal inputPosDecNum(string message, bool acceptZero = false)
        {
                decimal resultNum = 0;
                while (true)
                {
                    // Output message, and read user input
                    Console.Write(message);
                    string input = Console.ReadLine();

                    // Attempt to convert input to decimal
                    // If result is positive decimal number, return the result,
                    //    else output appropriate message
                    if (Decimal.TryParse(input, out resultNum))
                    {
                        if (resultNum < 0)
                        {
                            Console.WriteLine("Please enter a positive number");
                            continue;                        
                        }

                        if (!acceptZero && resultNum == 0)
                        {
                                Console.WriteLine("Please enter a nonzero number.");
                                continue;
                         }

                         return resultNum;

                     }
                     else
                    {
                        Console.WriteLine("Please enter a valid number that is not too large.");
                    }
                
                }
            }
        }
}

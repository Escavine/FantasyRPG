// Week 8 Software Development 2

#include <iostream>
#include <stdlib.h>
#include <string>
#include <regex>
#include <chrono>
#include <fstream>
#include <sstream>
 

// bool emailPattern() // Will be used for email pattern detection
// {
	// std::regex pattern("\\d{3}-\\d{3}-\\d{3}"); // Defining email pattern
	// return true;
// }


bool studentNum(std::string ID) // Will be used for student number matching
{

	std::regex pattern("[A-Za-z]{3}\\d{6}"); // Defining student number, 3 characters followed by 6 digits

	return std::regex_match(ID, pattern);

}



void clearBuffer() // In-case of incorrect input, will be used to clear the buffer
{
	std::cin.clear();
	std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');

}


bool containsCharacter(char c) // Checks if email/name contains a letter
{
	return ((c >= 'a' && c <= 'z')) || (c >= 'A' && c <= 'Z');
}


bool containsNumber(std::string number) // Checks if there's a number for phone numbers
{
	if (number.length() < 11) // If number is smaller than 11 digits 
	{
		return false; // Return false
	}
	else
	{
		for (int i = 0; i < number.length(); i++)
		{


			if (number[i] >= '0' && number[i] <= '9') // Check the range to ensure that the numbers are of that region
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}

}


// bool numberValidation(int telephoneNumber) // Checks if number is in correct format
// {

// }


bool dayValidator(int d)
{
	// Checking for validation of day
	if (d < 1)
	{
		return false;
	}
	else if (d > 31)
	{
		return false;
	}
	else
	{
		return true;
	}
}

bool monthValidator(int m)
{
	// Checking for validation of month
	if (m < 1)
	{
		return false;
	}
	else if (m > 12)
	{
		return false;
	}
	else
	{
		return true;
	}


}


bool yearValidator(int y)
{
	int yearDiff = 2024;  

	yearDiff = yearDiff - y; // find the difference

	if (yearDiff >= 18) // Determines whether the individual is 18 or not
	{
		return true;
	}
	else
	{
		return false;
	}

}

bool emailValidation(std::string email)
{
	if (!containsCharacter(email[0])) // Checks if first character is a character
	{
		return false;
	}

	int at = -1, dot = -1; // Set these values to -1 for testing

	for (int i = 0; i < email.length(); i++)
	{
		// Check containing characters
		if (email[i] == '@')
		{
			at = i; 
		}
		else if (email[i] == '.')
		{
			dot = i;
		}
	}

	// Validation checks
	if (at == -1 || dot == -1) // This is the case if there aren't any '.' or "@" is present
	{
		return false;
	}

	if (at > dot)
	{
		return false;
	}


}


void readCurrentStudents()
{
	// Will output the current students available on the '.txt' record

	std::ifstream file("students.txt");
	std::string line;

	if (!file.is_open())
	{
		std::cout << "File not found...";
	}
	else
	{
		std::cout << "Students currently on record:\n" << std::endl;
		while (std::getline(file, line))
		{
			std::stringstream ss(line); // Creating a streamstream
			std::string field; // Field
			std::vector<std::string> fields; // Numerous fields

			while (std::getline(ss, field, ','))
			{
				fields.push_back(field); // Append each field
			}

			for (const auto& f : fields) // Forecah loop equivalent for C++
			{
				std::cout << f << std::endl;
			}

			std::cout << std::endl;
		}
	}

}

void storingData(std::string fullName, std::string email, std::string telephoneNumber, std::string ID, std::string dateOfBirth)
{
	int userInput;
	// Will store the values in a single row within a '.txt' file

	// Display user details
	std::cout << "\nDetails of student: " << std::endl;
	std::cout << "Name: " << fullName << std::endl;
	std::cout << "Email: " << email << std::endl;
	std::cout << "Number: " << telephoneNumber << std::endl;
	std::cout << "Student ID: " << ID << std::endl;
	std::cout << "Date of Birth: " << dateOfBirth << std::endl;

	std::ofstream file("students.txt", std::ios_base::app); // Open the file in append mode

	// Write the following information to the file, allocating a space, ensuring that they are on a single row
	file << "Date of birth: " << dateOfBirth << ", ";
	file << "Full name: " << fullName << ", ";
	file << "Email: " << email << ", ";
	file << "Number: " << telephoneNumber << ", ";
	file << "StudentID: " << ID;
	file << "\n"; // Allow an extra line for each individual (row spacing)

	std::cout << "\nWould you like to see the current students on record? (1 for 'yes' and any other key for 'no'): ";
	std::cin >> userInput;

	switch (userInput)
	{
		case 1:
			system("CLS"); // Clear the console
			readCurrentStudents();
			break;
		default:
			exit(0); // Exit the program

	}

}


void insertDetails(int remainingAttempts)
{
	// Variables for storing details
	std::string fullName, email, telephoneNumber, ID;
	int d, m, y;



	// Remaining user attempts will be displayed, if the user uses incorrect validation for given input
	if (remainingAttempts < 3)
	{
		if (remainingAttempts == 0)
		{
			std::cout << "System terminating after too many incorrect attempts." << std::endl;
			exit(1); // Terminate should this condition be true
		}
		else
		{
			std::cout << "Remaining Attempts: " << remainingAttempts << std::endl;
		}
	}

	std::cout << "Student Registerer System" << std::endl;

	std::cout << "Enter the day of birth: ";
	std::cin >> d;

	bool dayCheck = dayValidator(d);

	// Validate the day
	if (dayCheck)
	{
		std::cout << "Valid day format.\n" << std::endl;
		clearBuffer();

	}
	else
	{
		std::cout << "Invalid day format.\n" << std::endl;
		clearBuffer(); // Clear the buffer
		insertDetails(remainingAttempts - 1); // Reduce number of attempts
	}

	std::cout << "Enter the month of birth: ";
	std::cin >> m;

	bool monthCheck = monthValidator(m);

	// Validate the month
	if (monthCheck)
	{
		std::cout << "Valid month format.\n" << std::endl;
		clearBuffer();


	}
	else
	{
		std::cout << "Invalid month format.\n" << std::endl;
		clearBuffer(); // Clear the buffer
		insertDetails(remainingAttempts - 1); // Reduce number of attempts
	}

	std::cout << "Enter the year of birth: ";
	std::cin >> y;

	bool yearCheck = yearValidator(y);

	// Validate the year
	if (yearCheck)
	{
		std::cout << "Valid email format.\n" << std::endl;
		clearBuffer();

	}
	else
	{
		std::cout << "You are not 18/Invalid year format.\n" << std::endl;
		clearBuffer(); // Clear the buffer
		insertDetails(remainingAttempts - 1); // Reduce number of attempts
	}

	// Convert the following values to string format
	std::string day = std::to_string(d);
	std::string month = std::to_string(m);
	std::string year = std::to_string(y);

	std::string dateOfBirth = day + "-" + month + "-" + year; // Combine the following to create a string format date of birth, sepearted by '-'
	std::cout << "Date of Birth: " << dateOfBirth << "\n" << std::endl; // Debugging

	std::cout << "Enter your full name: ";
	std::getline(std::cin, fullName);

	bool nameCheck = containsCharacter(fullName[0]); // Check if name contains character

	// Check the format of the name
	if (nameCheck)
	{
		std::cout << "Valid name format.\n" << std::endl;
		clearBuffer();
	}
	else
	{
		std::cout << "Invalid name format.\n" << std::endl;
		clearBuffer(); // Clear the buffer
		insertDetails(remainingAttempts - 1); // Reduce number of attempts
	}


	std::cout << "Enter your email: ";
	std::getline(std::cin, email);
	bool validationCheck2 = emailValidation(email); // Validate the user's email

	// Check for the result of the email
	if (validationCheck2)
	{
		std::cout << "Valid email format.\n" << std::endl;

	}
	else
	{
		std::cout << "Invalid email format.\n" << std::endl;
		clearBuffer(); // Clear the buffer
		insertDetails(remainingAttempts - 1); // Reduce number of attempts
	}

	std::cout << "Enter your phone number: ";
	std::cin >> telephoneNumber;

	// Validation check for the phone number
	bool validationCheck3 = containsNumber(telephoneNumber);

	// Check for the result of phone number
	if (validationCheck3)
	{
		std::cout << "Valid phone number format.\n" << std::endl;
		clearBuffer();
	}
	else
	{
		std::cout << "Invalid phone number format.\n" << std::endl;
		clearBuffer(); // Clear the buffer
		insertDetails(remainingAttempts - 1); // Reduce number of attempts
	}


	std::cout << "Enter your student number: " << std::endl;
	std::getline(std::cin, ID);

	// Validation check for the student number
	bool validationCheck4 = studentNum(ID);

	// Check for the result of student ID
	if (validationCheck4)
	{
		std::cout << "Valid StudentID format.\n" << std::endl;
		clearBuffer();
	}
	else
	{
		std::cout << "Invalid StudentID format.\n" << std::endl;
		clearBuffer(); // Clear the buffer
		insertDetails(remainingAttempts - 1); // Reduce number of attempts
	}


	// Once all validation checks are done, proceed to storing the data.
	storingData(fullName, email, telephoneNumber, ID, dateOfBirth);


}


void userMenu()
{
	std::string userChoice;
	int remainingAttempts = 3; // User has 3 attempts to ensure that their inputted information is correct, before the system terminates

	std::cout << "User Menu\n" << std::endl;

	std::cout << "1. Register a student\n" << std::endl;
	std::cout << "2. Check current students on record\n" << std::endl;
	std::cout << "3. Exit program" << std::endl;

	std::cout << "\nEnter a corresponding value: ";
	std::getline(std::cin, userChoice);

	if (userChoice == "1")
	{
		system("CLS");
		insertDetails(remainingAttempts);
	}
	else if (userChoice == "2")
	{
		system("CLS");
		readCurrentStudents();
	}
	else
	{
		std::cout << "\nIncorrect value, please try again.\n" << std::endl;
		userMenu(); // Recurse
	}

}


// Driver code
int main()
{
	userMenu();
}



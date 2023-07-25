# Description
Chrome Password Checker is a C# console application that allows you to conveniently view all the passwords stored in your Google Chrome browser. It provides a command-line interface and leverages the power of C# and .NET 6.0 to retrieve and display the saved passwords securely. Please note that this application is intended for personal use or in cases where you have explicit permission to access the passwords.

# Features
- **Password Retrieval:** Chrome Password Checker utilizes C# and .NET 6.0 to access the password database of your Google Chrome browser and securely retrieve the stored passwords.

- **Password Security:** The application does not store any passwords locally or transmit them over the internet, ensuring the security and privacy of your sensitive information.

# System Requirements
- Windows, macOS, or Linux operating system
- .NET 7.0 runtime (download from [Microsoft .NET website](https://dotnet.microsoft.com/en-us/download/dotnet/7.0))

# How to Use
**Clone the Repository:** Clone this repository to your local machine using the following command.

`git clone https://github.com/ovpoddar/ChromePasswordChecker.git`

**Navigate to Project Directory:** Open a terminal or command prompt and navigate to the project directory.

`cd ChromePasswordChecker`

**Compile the Application:** Use the following .NET CLI command to compile the application

`dotnet build`

**Run the Application:** Execute the following command to run the Chrome Password Checker

`dotnet run`

**View Passwords:** After running the application, you will see a list of saved passwords along with their associated website URLs and usernames.

**Export Passwords (Optional):** If you wish to back up or further analyze the passwords, you can use the "Export" feature to save the password data to a secure file on your computer.

# Security Considerations
1. **Use at Your Own Risk:** Chrome Password Checker is intended for personal use or with explicit permission from the owner of the Chrome browser. Always ensure you have proper authorization to access the passwords.

2. **Protect Your Machine:** Keep your operating system, antivirus software, and other security measures up-to-date to prevent unauthorized access to your data.

3. **Be Mindful of Password Exports:** If you choose to export passwords, ensure you store the exported file in a secure location and protect it with a strong password.

# Disclaimer
Chrome Password Checker is not affiliated with Google or the Chrome browser. The application is provided "as is" without warranty of any kind, express or implied. Use the application responsibly and at your own risk.
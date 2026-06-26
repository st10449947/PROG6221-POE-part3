using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace Part1.chatbot;

public class User
{
    public void Greeting()
    {
        DrawBorder("CyberBot");
        // user interaction.
        Console.WriteLine("Welcome to the Cybersecurity bot, Please enter your name.");


        Console.ForegroundColor = ConsoleColor.Green;
        string name = Console.ReadLine();
        Console.ResetColor();
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Please enter something.");
            name = GetValidInput();
        }

        name = name.ToLower();

        Console.WriteLine("Heyy " + name + " how are you?");

        Console.ForegroundColor = ConsoleColor.Green;
        string user = Console.ReadLine().ToLower();
        Console.ResetColor();
        if (string.IsNullOrWhiteSpace(user))
        {
            Console.WriteLine("Please enter something.");
            user = GetValidInput();
        }
        user = user.ToLower();

        Console.WriteLine("");
        Console.WriteLine(new string('*', 100));
        Console.WriteLine("");

        if (user.Contains("how are you") || user.Contains("im good and you") || user.Contains("good and you"))
        {
            Console.WriteLine($"I'm doing great, thanks for asking, {name}!");
            Console.WriteLine("I specialize in helping users understand cybersecurity concerns and stay safe online.");
            Console.WriteLine("Would you like to explore some cybersecurity topics together? (yes/no)");
            Console.WriteLine("If not, I can also explain my purpose or suggest things you can ask me about.");

            Console.ForegroundColor = ConsoleColor.Green;
            string userInput = Console.ReadLine().ToLower();
            Console.ResetColor();
            if (string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine("Please enter something.");
                userInput = GetValidInput();
            }
            userInput = userInput.ToLower();

            Console.WriteLine("");

            if (userInput.Contains("yes"))
            {
                //topics
                showTopics();

            }
            else if (userInput.Contains("purpose"))
            {
                Console.WriteLine("");
                Purpose();
              
            }
            else if (userInput.Contains("things") || userInput.Contains("ask"))
            {
                Console.WriteLine("");
                Console.WriteLine("You can ask me about strong passwords, spotting phishing attempts, and safe browsing habits. " +
                    "I can also provide general cybersecurity tips and best practices.");
                ask();
            }
            else if (userInput.Contains("no"))
            {
                Console.WriteLine("");
                Console.WriteLine("No worries! stay safe online.");
            }
            else
            {
                Console.WriteLine("I didn't quite catch that. refer to my question");
                verify();
            }
        }

        // "else if" add something if the user does not ask how are you, to make it more interactive and less robotic.
        else if (!user.Contains("how are you") || user.Contains("im good and you") || user.Contains("good and you"))
        {

            Console.WriteLine($"oh wow, how rude!{name} you did not even ask how I'm doing. Go on, ask me i promise i wont bite!  ");

            Console.ForegroundColor = ConsoleColor.Green;
            user = Console.ReadLine().ToLower();
            Console.ResetColor();
            if (string.IsNullOrWhiteSpace(user))
            {
                Console.WriteLine("Please enter something.");
                user = GetValidInput();
            }
            user = user.ToLower();

            Console.WriteLine(" ");
            if (user.Contains("how are you"))
            {

                Console.WriteLine(new string('*', 100));
                Console.WriteLine($"I'm doing great, thanks for asking, {name}!");
                Console.WriteLine("I specialize in helping users understand cybersecurity concerns and stay safe online.");
                Console.WriteLine("Would you like to explore some cybersecurity topics together? (yes/no)");
                Console.WriteLine("If not, I can also explain my purpose or suggest things you can ask me about.");

                Console.ForegroundColor = ConsoleColor.Green;
                string userInput = Console.ReadLine().ToLower();
                Console.ResetColor();
                if (string.IsNullOrWhiteSpace(userInput))
                {
                    Console.WriteLine("Please enter something.");
                    userInput = GetValidInput();
                }
                userInput = userInput.ToLower();
                if (userInput.Contains("yes"))
                {
                    //topics
                    showTopics();
                }
                else if (userInput.Contains("purpose"))
                {
                    Console.WriteLine("");
                    Purpose();
                  
                }
                else if (userInput.Contains("things") || userInput.Contains("ask"))
                {
                    Console.WriteLine("");
                    Console.WriteLine("You can ask me about strong passwords, spotting phishing attempts, and safe browsing habits. " +
                        "I can also provide general cybersecurity tips and best practices.");
                    ask();

                }
                else
                {
                    verify();
                    return;
                }

                

            }

        }
    }
    // helper method to ensure user input is not empty or whitespace.
    private string GetValidInput()
    {
        string input;

        do
        {
            Console.ForegroundColor = ConsoleColor.Green;
            input = Console.ReadLine();
            Console.ResetColor();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.Write("Please enter something: ");
            }

        } while (string.IsNullOrWhiteSpace(input));

        return input;
    }

    public void PasswordSafety()
    {
        DrawBorder("Password Safety Tips");
        Console.WriteLine("1. Use strong passwords (12+ chars).");
        Console.WriteLine("2. Avoid common passwords.");
        Console.WriteLine("3. Use different passwords for each account.");
        Console.WriteLine("4. Enable multi-factor authentication.");
        Console.WriteLine("5. Update passwords regularly.");
        Console.WriteLine("\n" +
            "i hope you have found these tips helpful!");
        Console.WriteLine( "press 'y' to return to the main menu or 'x' to exit.");
        Console.ForegroundColor = ConsoleColor.Green;
        string choice = Console.ReadLine().ToLower();
        Console.ResetColor();

        
        if (string.IsNullOrWhiteSpace(choice))
        {
            Console.WriteLine("Please enter something.");
            choice = GetValidInput();
        }
        if (choice.Contains("y"))
        {
            showTopics();
        }
        else if (choice.Contains("x"))
        {
            Console.WriteLine("Goodbye!");
        }
        else
        {
            Console.WriteLine("I didn't quite catch that. Please respond with 'y/x'.");
            PasswordSafety();
        }
        

    }
    public void PhishingAwareness()
    {
        DrawBorder("Phishing Awareness Tips");
        Console.WriteLine("1. Check sender's email.");
        Console.WriteLine("2. Don't click suspicious links.");
        Console.WriteLine("3. Avoid sharing personal info.");
        Console.WriteLine("4. Watch for urgent language.");
        Console.WriteLine("5. Use multi-factor authentication.");
        Console.WriteLine("6. Keep software updated.");
        Console.WriteLine("7. Report strange emails.");
        Console.WriteLine("\n" +
           "i hope you have found these tips helpful!");
        Console.WriteLine("press 'y' to return to the main menu or 'x' to exit.");
        Console.ForegroundColor = ConsoleColor.Green;
        string choice = Console.ReadLine().ToLower();
        Console.ResetColor();
        if (string.IsNullOrWhiteSpace(choice))
        {
            Console.WriteLine("Please enter something.");
            choice = GetValidInput();
        }
        if (choice.Contains("y"))
        {
            showTopics();
        }
        else if (choice.Contains("x"))
        {
            Console.WriteLine("Goodbye!");
        }
        else
        {
            Console.WriteLine("I didn't quite catch that. Please respond with 'y/x'.");
            PhishingAwareness();
        }

    }
    public void SafeBrowsing()
    {
        DrawBorder("Safe Browsing Tips");
        Console.WriteLine("1. Always check for HTTPS in websites.");
        Console.WriteLine("2. Avoid downloading files from unknown sources.");
        Console.WriteLine("3. Keep your browser updated.");
        Console.WriteLine("4. Use antivirus software.");
        Console.WriteLine("5. Be cautious with pop-ups and ads.");
        Console.WriteLine("\n" +
           "i hope you have found these tips helpful!");
        Console.WriteLine("press 'y' to return to the main menu or 'x' to exit.");
        Console.ForegroundColor = ConsoleColor.Green;
        string choice = Console.ReadLine().ToLower();
        Console.ResetColor();
        if (string.IsNullOrWhiteSpace(choice))
        {
            Console.WriteLine("Please enter something.");
            choice = GetValidInput();
        }
        if (choice.Contains("y"))
        {
            showTopics();
        }
        else if (choice.Contains("x"))
        {
            Console.WriteLine("Goodbye!");
        }
        else
        {
            Console.WriteLine("I didn't quite catch that. Please respond with 'y/x'.");
            SafeBrowsing();
        }
    }

    public void showTopics()
    {
        Console.WriteLine( "");
        Console.WriteLine(new string ('*',100));
        Console.WriteLine( "");
        Console.WriteLine("Great! I can provide information on various cybersecurity topics like \n" +
             "1. STRONG PASSWORDS\n" +
             "2. SPOTTING PHISHING ATTEMPTS\n" +
             "3. SAFE BROWSING.\n" +
             "Which one intrests you the most?");
        Console.ForegroundColor = ConsoleColor.Green;
        string choice2 = Console.ReadLine().ToLower();
        Console.ResetColor();

        if (string.IsNullOrWhiteSpace(choice2))
        {
            Console.WriteLine("Please enter something.");
            choice2 = GetValidInput();
        }
        choice2 = choice2.ToLower();


        switch (choice2)
        {
            case "1":
            case "password":
            case "strong passwords":
            case "passwords":
            case "strong password":
                PasswordSafety();
                break;
            case "2":
            case "phishing":
            case "spotting phishing attempts":
            case "attempts":
                PhishingAwareness();
                break;
            case "3":
            case "safe browsing":
                SafeBrowsing();
                break;
            default:
                Console.WriteLine("Sorry, I didn't understand that choice.");
                showTopics();

                break;
        }

    }
    public void Purpose()

    {
        Console.WriteLine("My purpose is to educate users about cybersecurity best practices and help them stay safe online.\n" +
            "I can provide tips on creating strong passwords, recognizing phishing attempts, \n" +
            "and safe browsing habits." +
            "My goal is to empower users with knowledge to protect their personal information\n" +
            "and navigate the digital world securely.");

              ask();
    }
    public void ask()
    {
        Console.WriteLine("");
        Console.WriteLine("Is there anything else you would like to know about cybersecurity? (yes/no)");

        Console.ForegroundColor = ConsoleColor.Green;
        string userInput = Console.ReadLine().ToLower();
        Console.ResetColor();
        if (string.IsNullOrWhiteSpace(userInput))
        {
            Console.WriteLine("Please enter something.");
            userInput = GetValidInput();
        }
        userInput = userInput.ToLower();
        if (userInput.Contains("yes") || userInput.Contains("y"))
        {
            showTopics();
        }
        else if (userInput.Contains("no")||userInput.Contains("n"))
        {
            Console.WriteLine("No worries! stay safe online.");
        }
        else
        {
            Console.WriteLine("I didn't quite catch that. Please respond with 'yes/no'.");
            ask();
        }

    }
    public void verify()
    {
        Console.WriteLine($"I imagine you might be intrested in cybersecurity" +
                       "concerns" +
                       "and would like us to talk about them? (yes/no) if not, i could tell you about my " +
                       "purpose or things you can ask me about ");

        Console.ForegroundColor = ConsoleColor.Green;
        string userInput = Console.ReadLine().ToLower();
        Console.ResetColor();
        if (string.IsNullOrWhiteSpace(userInput))
        {
            Console.WriteLine("Please enter something.");
            userInput = GetValidInput();
        }
        userInput = userInput.ToLower();



        if (userInput.Contains("yes"))
        {
            //topics
            showTopics();

        }
        else if (userInput.Contains("purpose"))
        {
            Purpose();
            ask();
        }
        else if (userInput.Contains("things") || userInput.Contains("ask"))
        {
            Console.WriteLine("You can ask me about strong passwords, spotting phishing attempts, and safe browsing habits. " +
                "I can also provide general cybersecurity tips and best practices.");
            ask();
        }
        else if (userInput.Contains("no"))
        {
            Console.WriteLine("No worries! stay safe online.");
        }
        else
        {
            Console.WriteLine("I didn't quite catch that. Please respond with 'yes/no'.");
        }
    }
 
         public void DrawBorder(string text)
        {          int totalWidth = text.Length + 100;
                   string border = new string('*', totalWidth);

                   int padding = (totalWidth - text.Length - 2) / 2;
                   int extraSpace = (totalWidth - text.Length - 2) % 2;

                   string spaces = new string(' ', padding);

                  // Top border
                  Console.WriteLine(border);

                  // Left star
                  Console.Write("*");

                  // Left padding
                  Console.Write(spaces);

                  // Change text color to red
                  Console.ForegroundColor = ConsoleColor.Red;

                  // Typing effect
                  foreach (char c in text)
                 {
                 Console.Write(c);
                 Thread.Sleep(50);
                 }

                Console.ResetColor();

                
                Console.Write(spaces + new string(' ', extraSpace));

                // Right star
                Console.WriteLine("*");

                // Bottom border
                Console.WriteLine(border);
}
}




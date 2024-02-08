﻿using System;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;


// FantasyRPG: A console based RPG game, which sole purpose is to improve on my current programming skills (mainly OOP as that is my weakness).

namespace FantasyRPG
{
    class CharacterDefault // fixed preset for all classes
    {
        // Generic character attributes
        public string name;
        public int health;
        public string weaponType;
        public string weaponName;
        public float numOfPotionsInInventory;
        public float maxPotions;
        public int mana;
        public string[] currentInventory; // Will contain the users potions and other weapons.
        public int arcaniaGoldCoins; // Currency for the city of Arcanith
        // public int atk;
        // public int def;

        // Levelling attributes
        public float exp;
        public int level;
        private int experienceRequiredForNextLevel;

        public CharacterDefault(string _name, string _weaponName, string _weaponType, string[] _currentInventory, int _arcaniaGoldCoins) // Default preset for all classes during the start of the game :3
        {
            name = _name;
            weaponType = _weaponType;
            weaponName = _weaponName;
            currentInventory = _currentInventory;
            arcaniaGoldCoins = 0;
            health = 100;
            exp = 0f;
            numOfPotionsInInventory = 0;
            maxPotions = 5;
            level = 1;
            mana = 100;
        }

        // WIll allow user to equip the following weapon (e.g. if they use a bow, blades, sword etc.)
        // public void EquipWeapon(Weapon weapon)
        // {
        //    CurrentWeapon = weapon;
        // }


        // All methods for all user choice classes


        // Display the users inventory
        public void CheckInventory()
        {
            for (int i = 0; i < currentInventory.Length; i++)
            {
                Console.WriteLine(currentInventory[i]);

            }

        }

        // Allow for the user to check their current status
        public void CheckStatus()
        {
            Console.WriteLine(name + " current status: ");
            Console.WriteLine("Health: " + health);
            Console.WriteLine("Experience accumuated: " + exp);
            Console.WriteLine("Current level: " + level);
        }

        // Used for recovery
        public void Meditate()
        {
            Console.WriteLine(name + " has meditated ");
            mana = mana + 20;
            health = health + 20;
            Console.WriteLine(name + " has meditated and has recovered:\n");
            Console.WriteLine("+20 health");
            Console.WriteLine("+20 mana");
        }


        // Levelling methods 
        public void CalculateExperienceForNextLevel()
        {
            if (level < 5)
            {
                experienceRequiredForNextLevel = 10 * level;
                Console.WriteLine("For the next level, you'll need " + experienceRequiredForNextLevel + " amount of experience.");
            }
            else if (level > 10)
            {
                experienceRequiredForNextLevel = 100 * level;
                Console.WriteLine("For the next level, you'll need " + experienceRequiredForNextLevel + " amount of experience.");
            }
            // This sequence of logic will continue as the console game develops (probably not haha)

        }

        // Should the condiition be met
        public void LevelUp()
        {
            level++;
            Console.WriteLine(name + " has levelled up! " + " You are now level " + level);
            CalculateExperienceForNextLevel();

        }

        // Check if user has enough to level up
        public void GainExperience(int experiencePoints)
        {
            exp += experiencePoints;

            // Check if the character should level up
            if (exp >= experienceRequiredForNextLevel)
            {
                LevelUp();
            }

        }

    }
    class Knight : CharacterDefault // Knight class properties and methods
    {
        public string normalAtkName;
        public string specialAtkName;
        public int specialAtkDmg;
        public int normalAtkDmg;
        public int specialAtkRecharge;// Percentage value representing 100%

        public Knight(string _name, string _weaponName, string _weaponType, string _specialAtkName, string[] _currentInventory, int _arcaniaGoldCoins) : base(_name, _weaponName, _weaponType, _currentInventory, _arcaniaGoldCoins)
        {
            name = _name;
            weaponName = _weaponName;
            weaponType = _weaponType;
            specialAtkName = _specialAtkName;
            currentInventory = _currentInventory;
            normalAtkName = "Sword Slash";
            specialAtkRecharge = 0; // 0% preset, every time the user attacks, this will increase
            specialAtkDmg = 10; // Preset damage from sword special attack
            normalAtkDmg = 4;
        }

        public void BasicAtk() // Primitive knight attack
        {
            Console.WriteLine(name + " has used " + normalAtkName + " and has dealt " + normalAtkDmg + " damage.");
        }

        public void SpecialAtk(int specialAtkRecharge) // Knight's special ability (has a recharge and will be executed should the recharge be 100%)
        {
            if (specialAtkRecharge == 100)
            {
                Console.WriteLine("Conditions met\n");
                Console.WriteLine(name + " has used " + specialAtkName + " and has dealt " + specialAtkDmg + " damage.");
            }
            else
            {
                Console.WriteLine("Your recharge isn't high enough.");
            }

        }

        public void KnightTraining()
        {
            // generate a random value for exp
            Random rd = new Random();
            int rand_num = rd.Next(1, 5);

            Console.WriteLine(name + " has decided to improve on their skills, ", " their experience has increased by " + rand_num);
            exp = exp + rand_num;

        }


    }

    class Mage : CharacterDefault // Wizard class properties + methods
    {
        // Properties for common wizard attributes
        public string[] magicSpells;
        string[] magicSpeciality; // User can have multiple magic specialties
        public int spellUsage; // Spell usage to keep spells in control

        public Mage(string _name, string _weaponName, string _weaponType, string[] _magicSpeciality, int _arcaniaGoldCoins, string[] _magicSpells, string[] _currentInventory) : base(_name, _weaponName, _weaponType, _currentInventory, _arcaniaGoldCoins)
        {
            name = _name;
            weaponName = _weaponName;
            weaponType = _weaponType;
            magicSpeciality = _magicSpeciality;
            currentInventory = _currentInventory;
            magicSpells = _magicSpells; // Predefined variables for every new wizard in the game
            spellUsage = 5;
        }


        // methods for a wizard
        public void SpellCast() // Spell casting for enemies
        {
            Console.WriteLine(name + " has casted " + spellUsage);
            spellUsage--;
            mana = mana - 30;
            exp += 0.3f;

        }

        public void IncreaseSpellInInventory()
        {
            spellUsage++;
            Console.WriteLine(name + " has gained 1 spell in the inventory");
        }

        public void MageTraining()
        {
            // generate a random value for exp
            Random rd = new Random();
            int rand_num = rd.Next(1, 5);

            Console.WriteLine(name + " has decided to improve on their skills, ", " their experience has increased by " + rand_num);
            exp = exp + rand_num;

        }

        public void chooseNewSpeciality(string[] magicSpeciality, string name) // Should the mage level up, they'll be able to pick another speciality (only 1)
        {
            // For every 10 levels, a mage can pick a new speciality

            Console.Clear(); // Cleaning purposes

            Console.BackgroundColor = ConsoleColor.Blue;
            string userInput;
            int correspondingNumOrder = 1;
            List<string> updatedMagicChoices = new List<string>();
            string[] chosenSpecialty = magicSpeciality;

            // Magic choices + Elements
            string[] magicChoices = { "Fire", "Lightning", "Water", "Dark", "Light", "Eucladian-Magic" };

            // Output this to the terminal
            Console.WriteLine("Mage's Prestiege!");
            Console.WriteLine("\n" + name + "'s" + " current known magic specialities:");


            for (int j = 0; j < magicSpeciality.Length; j++) // Display the user's current magic specialties
            {
                Console.WriteLine(magicSpeciality[j]);
            }

            // Updating the magic choices by appending the magic elements that the user doesn't possess in a list
            foreach (var choice in magicChoices)
            {
                if (!magicSpeciality.Contains(choice))
                {
                    updatedMagicChoices.Add(choice);
                }
            }

            Console.WriteLine("\nPick a new magic speciality!");


            for (int i = 0; i < updatedMagicChoices.Count; i++)
            {
                Console.WriteLine(correspondingNumOrder + ". " + updatedMagicChoices[i]); // Display the magic choices avaliable to the user
                correspondingNumOrder++;

            }

            userInput = Convert.ToString(Console.ReadLine());

            Console.WriteLine("Input based on the corresponding number");

            // Appending the magic element to array.
            int selectedIndex;

            if (int.TryParse(userInput, out selectedIndex) && selectedIndex >= 1 && selectedIndex <= updatedMagicChoices.Count)
            {
                // chosenSpecialties is used to keep track of the magic that was learnt
                chosenSpecialty[0] = updatedMagicChoices[selectedIndex - 1];
                Console.WriteLine(name + " has learnt the magic speciality: " + chosenSpecialty[0]);

                Array.Resize(ref magicSpeciality, magicSpeciality.Length + 1);
                magicSpeciality[magicSpeciality.Length - 1] = updatedMagicChoices[selectedIndex - 1];
                Console.WriteLine($"Updated magic specialties: {string.Join(", ", magicSpeciality)}");

                learnNewSpells(chosenSpecialty, magicSpeciality, magicSpells); // Redirect the user to this function for them to learn new spells for their given speciality.

            }
            else
            {
                // Invalid input
                Console.WriteLine("Invalid input. Please enter a valid number corresponding to the magic speciality.");

            }
        }

        public void learnNewSpells(string[] chosenSpecialty, string[] magicSpeciality, string[] magicSpells)
        {
            string[] magicChoices = { "Fire", "Lightning", "Water", "Dark", "Light", "Eucladian-Magic" }; // Future Reference: Make a method in the Mage Class that allows for a mage to learn more magic specialities and skills as they level up.
            string[] fireMagicSpells = { "Infrared", "Blazing Rage", "Flamestrike", "Pyroburst", "Phoenix Fury" }; // Future Reference: Add a damage system for the magic spells (e.g. infrared deals 8 damage etc.)
            string[] lightningMagicSpells = { "Thunderstrike", "Striking Surge", "Volt Surge", "Arcane Thunder" };
            string[] waterMagicSpells = { "Aqua Torrent", "Hydroburst", "Lunar Tide", "Ripple Cascade" };
            string[] darkMagicSpells = { "Shadow Veil", "Umbral Surge", "Wraith's Curse", "Eclipsed Oblivion" };
            string[] lightMagicSpells = { "Luminous Beam", "Solar Flare", "Etherial Halo", "Aurora's Illumination", "Divine Judgement" };
            string[] eucladianMagicSpells = { "Esoteric Paradigm", "Fractural Fissure", "Quantum Flux", "Etherial Nexus" };

            int totalSpellsDisplayed = 0; // Keep track of the number of total spells displahyed

            // Display the given spells to the user

            for (int z = 0; z < 1; z++)
            {
                Console.WriteLine("\n" + chosenSpecialty[z] + " Spells:");

                switch (chosenSpecialty[z])
                {
                    case "Fire":
                        foreach (string spell in fireMagicSpells)
                        {
                            Console.WriteLine((totalSpellsDisplayed + 1) + ". " + spell);
                            totalSpellsDisplayed++;
                            Console.WriteLine("Press Enter to see the next spell...");
                            Console.ReadLine();
                        }
                        break;
                    case "Lightning":
                        foreach (string spell in lightningMagicSpells)
                        {
                            Console.WriteLine((totalSpellsDisplayed + 1) + ". " + spell);
                            totalSpellsDisplayed++;
                            Console.WriteLine("Press Enter to see the next spell...");
                            Console.ReadLine();
                        }
                        break;
                    case "Water":
                        foreach (string spell in waterMagicSpells)
                        {
                            Console.WriteLine((totalSpellsDisplayed + 1) + ". " + spell);
                            totalSpellsDisplayed++;
                            Console.WriteLine("Press Enter to see the next spell...");
                            Console.ReadLine();
                        }
                        break;
                    case "Dark":
                        foreach (string spell in darkMagicSpells)
                        {
                            Console.WriteLine((totalSpellsDisplayed + 1) + ". " + spell);
                            totalSpellsDisplayed++;
                            Console.WriteLine("Press Enter to see the next spell...");
                            Console.ReadLine();
                        }
                        break;
                    case "Light":
                        foreach (string spell in lightMagicSpells)
                        {
                            Console.WriteLine((totalSpellsDisplayed + 1) + ". " + spell);
                            totalSpellsDisplayed++;
                            Console.WriteLine("Press Enter to see the next spell...");
                            Console.ReadLine();
                        }
                        break;
                    case "Eucladian-Magic":
                        foreach (string spell in eucladianMagicSpells)
                        {
                            Console.WriteLine((totalSpellsDisplayed + 1) + ". " + spell);
                            totalSpellsDisplayed++;
                            Console.WriteLine("Press Enter to see the next spell...");
                            Console.ReadLine();
                        }
                        break;
                    default:
                        Console.WriteLine("Unknown Error");
                        Environment.Exit(0);
                        break;
                }

            }

            int specialityIndex = 0;

            for (specialityIndex = 0; specialityIndex < 1; specialityIndex++)
            {
                Console.WriteLine($"Select 2 magic spells for {chosenSpecialty[specialityIndex]} by entering the corresponding numbers. (1-4 for each element)");

                List<string> currentMagicSpells = new List<string>(); // Dynamic list which will be used to store the chosen magical spells of the users

                switch (chosenSpecialty[chosenSpecialty.Length - 1]) // Fix this that way it only displays the spells for exclusively the new magic element learnt (e.g. if user learns fire, then only display fire magic spells)
                {
                    case "Fire":
                        currentMagicSpells = fireMagicSpells.ToList();
                        break;
                    case "Lightning":
                        currentMagicSpells = lightningMagicSpells.ToList();
                        break;
                    case "Water":
                        currentMagicSpells = waterMagicSpells.ToList();
                        break;
                    case "Dark":
                        currentMagicSpells = darkMagicSpells.ToList();
                        break;
                    case "Light":
                        currentMagicSpells = lightMagicSpells.ToList();
                        break;
                    case "Eucladian-Magic":
                        currentMagicSpells = eucladianMagicSpells.ToList();
                        break;
                    default:
                        Console.WriteLine("Unknown magic speciality.");
                        Environment.Exit(0);
                        break;
                }


                // Let the user pick 2 spells for the given magic specialty

                int spellIndex = 0; // Keep track of index within array

                for (int spellNumber = 0; spellNumber < 2; spellNumber++)
                {
                    Console.WriteLine($"Choose magic spell #{spellNumber + 1} for {chosenSpecialty[specialityIndex]}:");
                    int magicSpellChoice;
                    while (!int.TryParse(Console.ReadLine(), out magicSpellChoice) || magicSpellChoice < 1 || magicSpellChoice > magicChoices.Length)
                    {
                        Console.WriteLine("Invalid choice. Please enter a valid number corresponding to the magic specialty.");
                    }
                    Array.Resize(ref magicSpells, magicSpells.Length + 1); // Increase the length of the array by 1
                    magicSpells[magicSpells.Length - 1] = currentMagicSpells[magicSpellChoice - 1];
                    spellIndex++;
                }
            }


            Console.WriteLine(name + " has learned the following magical abilities: ");

            for (int i = 0; i < magicSpells.Length; i++) // Display the magic spells that the user knows
            {
                Console.WriteLine(magicSpells[i]);
            }

            chosenSpecialty = null; // Clear the array of any specialties, for the next time this is run
        }
    }


    class SomaliPirate : CharacterDefault
    {
        public string weaponAura, normalAtkName, specialAtkName;
        public int normalAtkDmg, specialAtkDmg, specialAtkCharge;
        public SomaliPirate(string _name, string _weaponName, string _weaponType, string _weaponAura, string _normalAtkName, string _specialAtkName, string[] _currentInventory, int _arcaniaGoldCoins) : base(_name, _weaponName, _weaponName, _currentInventory, _arcaniaGoldCoins)
        {
            name = _name;
            weaponName = _weaponName;
            weaponType = _weaponType;
            weaponAura = _weaponAura;
            normalAtkName = _normalAtkName; // Presets for all new Somali Pirates in the game
            arcaniaGoldCoins = _arcaniaGoldCoins;
            specialAtkName = _specialAtkName;
            currentInventory = _currentInventory;
            normalAtkDmg = 8;
            specialAtkDmg = 16;
            specialAtkCharge = 100;

        }

        // All methods for the somaliPirate class
        public void PirateNormalAtk()
        {
            Console.WriteLine("The brave Somali Pirate named " + name + " has used " + weaponName + " to deal " + normalAtkDmg);
        }

        public void PirateSpecialAtk()
        {
            Console.WriteLine("The brave Somali Pirate named " + name + " has used " + weaponName + " to deal " + specialAtkDmg);
        }

        public void PirateTraining()
        {
            // Generate a random value for experience
            Random rd = new Random();
            int rand_num = rd.Next(1, 5);

            Console.WriteLine(name + " has decided to improve on their skills, ", " their experience has increased by " + rand_num);
            exp = exp + rand_num;

        }


    }

    class Archer : CharacterDefault
    {
        public Archer(string _name, string _weaponName, string _weaponType, string[] _currentInventory, int _arcaniaGoldCoins) : base(_name, _weaponName, _weaponType, _currentInventory, _arcaniaGoldCoins)
        {
            name = _name;
            weaponName = _weaponName;
            weaponType = _weaponType;
            arcaniaGoldCoins = _arcaniaGoldCoins;
        }
    }


    public class Weapon // Equip/Unequip weapon 
    {
        public string Name { get; set; }
        public int Damage { get; set; }
        public string WeaponName { get; set; }
        public string WeaponType { get; set; }

    }

    // Warrior class
    class Warrior : CharacterDefault
    {
        public Warrior(string _name, string _weaponName, string _weaponType, string[] _currentInventory, int _arcaniaGoldCoins) : base(_name, _weaponName, _weaponType, _currentInventory, _arcaniaGoldCoins)
        {
            name = _name;
            weaponName = _weaponName;
            weaponType = _weaponType;
            currentInventory = _currentInventory;
            arcaniaGoldCoins = _arcaniaGoldCoins;
        }
    }


    class userMenu // Future reference: Authentication system before user logs in, will create a table to store the users information
    {
        //
    }


    class GameMenu
    {
        static void Main(string[] args) // Future reference: With the implementation of the authentication system soon, this will be moved.
        {
            GameMenu menu = new GameMenu();
            menu.gameMenu(); // User is first directed to the game menu method
        }


        void gameMenu() // After user information is authenticated, they'll be lead here
        {
            SmoothConsole smoothOutput = new SmoothConsole(); // Initialize the smooth console

            // Future reference: Implementing AI mobs and perhaps AI individuals

            int userChoice; // Used for the start of the game
            string[] gameTips = {"Did you know that every 10 levels, you can get an extra ability/speciality?",
                "This game is still in development, so if there's an issue please contact me through my GitHub (Escavine) and send a pull request which I'll review.",
            "Eucladian abilities are very overpowered, but in turn they'll cost you some health.", "This game have a sneaky RNG factor, you'll see later as you play :3",
            "For you down bad individuals, I MIGHT introduce a harem feature, perhaps implement it with AI, imagine how insane that'll be? LOL" }; // Array containing necessary game tips, more will be added in the future.

            // Initiation of the console game
            smoothOutput.FastPrint("---------FantasyRPG----------\n");
            Console.WriteLine("Game advice: When inputting values, input a corresponding value to the action (e.g. enter the value 1 in order to start the game\n"); // Display game advice
            Random ran = new Random();
            int ran_num = ran.Next(0, 5);
            Console.WriteLine("Game Tip: " + gameTips[ran_num] + "\n"); // Display a random game tip in the menu

            Console.WriteLine("Game Menu\n");
            smoothOutput.FastPrint("1. Get started\n");
            smoothOutput.FastPrint("2. Load save game\n"); // Feature doesn't work yet
            smoothOutput.FastPrint("3. Help\n");
            smoothOutput.FastPrint("4. Make a suggestion\n"); // Feature doesn't work yet
            smoothOutput.FastPrint("5. Future plans\n");

            // Register user input
            userChoice = Convert.ToInt32(Console.ReadLine());

            switch (userChoice)
            {
                case 1:
                    Console.WriteLine("\nYour game session will now begin!");
                    ClassSelection selectClass = new ClassSelection(); // Create a new game session
                    selectClass.userClass(); // Proceed to let the user pick a character class
                    break;
                case 2:
                    loadingSaveData(); // Lead user to the method
                    break;
                case 3:
                    helpSection(); // Lead user to the method
                    break;
                case 4:
                    makeGameSuggestion(); // Lead user to the method
                    break;
                case 5:
                    futurePlans(); // Lead user to the method
                    break;
                default:
                    Console.WriteLine("Invalid option, please try again!");
                    break;
            }
        }


        void loadingSaveData()
        {
            bool loadingSaveData = true;
            int loadingSaveDataInput = 0;

            while (loadingSaveData == true)
            {
                // Should the user be logged in, they'll be able to access their save data
                Console.WriteLine("This feature isn't avaliable yet, would you like to go back" +
                    "to the menu? (1 for yes and 2 for no)");

                if (loadingSaveDataInput == 1)
                {
                    ("You will be lead back to the menu");
                    gameMenu();
                }

            }

        }

        void helpSection()
        {
            int userInput;
            string[] gameAdvice = { "You might die at any point within the game unknowingly.",
                        "Eucladian abilities are quite overpowered, if you find the opportunity to pursue it, then do so.",
                    "Having a strong romantical bond with someone, can potentially increase your abilities.", "There are many classes to choose from, all having unique features.",
                    "Avoid fighting overpowered foes early in-game (i.e. dragons), you'll probably get destroyed." };
            Console.WriteLine("--------Help Section--------\n");
            Console.WriteLine("What is FantasyRPG?\n");

            // Introduction to Arcania, the world of FantasyRPG
            Console.WriteLine("Welcome to FantasyRPG, a text-based adventure that transports you to the mystical realm of Arcania!");
            Console.WriteLine("Embark on an epic journey through a vast and enchanting world, where hidden treasures await discovery at every turn.");
            Console.WriteLine("Prepare yourself for the challenges ahead, as you confront life-and-death situations, battle formidable foes, and overcome treacherous obstacles.");
            Console.WriteLine();
            Console.WriteLine("In Arcania, your choices shape your destiny. Navigate the immersive landscape, forge alliances with fellow travelers, and encounter mythical creatures that will test your courage and resolve.");
            Console.WriteLine();
            Console.WriteLine("But beware, adventurer, for danger lurks in the shadows. Face cunning enemies, solve challenging puzzles, and unravel the mysteries that lie dormant in this magical land.");
            Console.WriteLine();
            Console.WriteLine("Amidst the chaos, there is also the promise of something more. As you progress, open your heart to the possibility of romantic connections, adding depth to your personal story.");
            Console.WriteLine();
            Console.WriteLine("Are you ready to embark on a journey into the heart of Arcania, where every decision shapes your fate? Your adventure begins now!");

            // Ask if the user wants to see any game advice in the help section
            Console.WriteLine("Would you like to see any game advice?\n");
            Console.WriteLine("1. Yes");
            Console.WriteLine("2. No");
            userInput = Convert.ToInt32(Console.ReadLine());

            switch (userInput)
            {
                case 1:
                    Console.WriteLine("Game Advice:\n");

                    foreach (string s in gameAdvice) // Display game advice
                    {
                        Console.WriteLine(s + "\n");
                    }
                    break;

                case 2: // Future reference: Move the game menu code to different methods rather than the main, that way you can utilise recursion
                    Environment.Exit(0); // Terminate the game session
                    break;
                default:
                    Console.WriteLine("Invalid input, please try again.");
                    break;
            }

            Console.ReadKey(); // Wait for key input
        }


        void makeGameSuggestion() // Game suggestions
        {
            Console.WriteLine("Send a message to kmescavine@gmail.com in order to send your ideas!"); // Future reference: Use an SMTP feature to allow the user to input their email and send their suggestion
            Console.ReadKey();
        }

        void futurePlans() // Future plans for the game development
        {
            int count = 1;
            string[] futurePlans = { "Adding new classes", "Potential romance feature", "Harem feature (not likely)", "A chance of randomly dying", "Illnesses and cures", "Game difficulty (easy, normal, hard, impossible)" };
            Console.WriteLine("Future plans for FantasyRPG include:\n");

            foreach (string plan in futurePlans)
            {
                Console.WriteLine("Plan " + count + ": " + plan + "\n");
                count++;
            }

            Console.ReadKey(); // Wait for key input


        }




    }
}

    public class ClassSelection // This class will allow a user to pick from a variety of different roles in the game, before embarking on their journey.
    {
        public void userClass()
        {
            SmoothConsole smoothPrinting = new SmoothConsole(); // initiate the smooth console class

            int userChoice; // Define the user choice

            // Defining the different classes and rarity of items
            string[] fantasyClasses = { "Mage", "Knight", "Somali Pirate", "Shadowwrath", "Archer", "Assassin" }; // Predefined array of roles
            string[] rarity = { "Archaic", "Uncommon", "Mythical", "Divine" }; // Predefined values :3
            int num = 1;

            smoothPrinting.SlowPrint("Welcome to the dungeon game!");
            Console.WriteLine("\nPick your class");
            Console.WriteLine("-------------------\n"); // Neater

            for (int i = 0; i < fantasyClasses.Length; i++)
            {

                Console.WriteLine(num + ". " + fantasyClasses[i]);
                num++;
            }

            userChoice = Convert.ToInt32(Console.ReadLine());

            switch (userChoice)
            {
                // Should the user decided to become a Mage
                case 1:
                    int choiceIncrementer = 1; // Used to increment the user choice when picking magic types
                    int startMageJourneyInput;

                    // Arrays containing the variety of different magic choices, spells and weapons.
                    string[] magicChoices = { "Fire", "Lightning", "Water", "Dark", "Light", "Eucladian-Magic" };
                    int arcaniaGoldCoins = 0; // You start of as a brokie 

                    // Tuple dictionary for each Fire magic spell, which is associated with a damage value and a mana requirement 
                    Dictionary<string, (int, int)> fireMagicSpells = new Dictionary<string, (int, int)>()
                    {
                        { "Infrared", (3, 15) },
                        { "Blazing Rage", (5, 20) },
                        { "Flamestrike", (7, 25) },
                        { "Pyroburst", (9, 30) },
                        { "Phoenix Fury", (12, 35) }
                    };

                    // Tuple dictionary for each Lightning magic spell, which is associated with a damage value and a mana requirement 
                    Dictionary<string, (int, int)> lightningMagicSpells = new Dictionary<string, (int, int)>()
                    {
                        { "Thunderstrike", (4, 15) },
                        { "Striking Surge", (6, 20) },
                        { "Volt Surge", (8, 25) },
                        { "Arcane Thunder", (10, 30) }
                    };

                    // Tuple dictionary for each Water magic spell, which is associated with a damage value and a mana requirement 
                    Dictionary<string, (int, int)> waterMagicSpells = new Dictionary<string, (int, int)>()
                    {
                        { "Aqua Torrent", (2, 10) },
                        { "Hydroburst", (4, 15) },
                        { "Lunar Tide", (6, 20) },
                        { "Ripple Cascade", (8, 25) }
                    };

                    // Tuple dictionary for each Dark magic spell, which is associated with a damage value and a mana requirement 
                    Dictionary<string, (int, int)> darkMagicSpells = new Dictionary<string, (int, int)>()
                    {
                        { "Shadow Veil", (3, 15) },
                        { "Umbral Surge", (5, 20) },
                        { "Wraith's Curse", (7, 25) },
                        { "Eclipised Oblivion", (9, 30) }
                    };

                        // Tuple dictionary for each Light magic spell, which is associated with a damage value and a mana requirement 
                    Dictionary<string, (int, int)> lightMagicSpells = new Dictionary<string, (int, int)>()
                    {
                        { "Luminous Beam", (3, 15) },
                        { "Solar Flare", (5, 20) },
                        { "Etherial Halo", (7, 25) },
                        { "Aurora's Illumination", (9, 30) },
                        { "Divine Judgement", (12, 35) }
                    };

                    // Tuple dictionary for each Eucladian magic spell, which is associated with a damage value and a mana requirement 
                    Dictionary<string, (int, int)> eucladianMagicSpells = new Dictionary<string, (int, int)>()
                    {
                        { "Esoteric Paradigm", (3, 15) },
                        { "Fractural Fissure", (5, 20) },
                        { "Quantum Flux", (7, 25) },
                        { "Etherial Nexus", (9, 30) }
                    };

                    // Tuple dictionary for the starter weapons, which is associated with a damage value and a rarity type
                    Dictionary<string, (int, string)> starterMageWeapons = new Dictionary<string, (int, string)>()
                    {
                        { "Weathered Oakwind", (5, "Archaic") },
                        { "Ancient Runestaff", (7, "Uncommon") },
                        { "Runic Wooden Scepter", (3, "Archaic") },
                        { "Dusty Relic Rod", (2, "Archaic") },
                        { "Emerald Crystal Staff", (10, "Mythical") }
                    };

                    Console.Clear(); // Cleaning purposes
                    Console.WriteLine("Mage's Route");
                    smoothPrinting.FastPrint("\nYou undergo intense mana training and finally become a Mage.\n");

                    Console.WriteLine("What is your name, adventurer?");
                    string name = Convert.ToString(Console.ReadLine());

                    Random ranNum = new Random();
                    int random_index = ranNum.Next(0, starterMageWeapons.Count); // Select a random weapon for the user


                    string[] mageWeaponNames = new string[starterMageWeapons.Count]; // All values will be assigned to the array
                    starterMageWeapons.Keys.CopyTo(mageWeaponNames, 0);
                    string staffName = mageWeaponNames[random_index]; // Assign a weapon randomly to the user from the converted dictionary

                    List<string> mageInventory = new List<string>();
                    mageInventory.Add(staffName); // Add the staff to the users current inventory


                    string staffWeaponType = "Staff"; // Fixed and cannot be changed

                    smoothPrinting.SlowPrint("\nChoose two magic specialties from the list: \n");

                    List<string> chosenSpecialties = new List<string>(); // Chosen magic specialities
                    List<string> magicSpells = new List<string>(); // Chosen magical spells

                    // Display all the magic choices to the user
                    for (int j = 0; j < magicChoices.Length; j++)
                    {
                        Console.WriteLine(choiceIncrementer + ". " + magicChoices[j]);
                        choiceIncrementer++;
                    }


                    // Convert all the following magic spells dictionary to array values to be used in the loop :3

                    string[] fireSpells = new string[fireMagicSpells.Count];
                    fireMagicSpells.Keys.CopyTo(fireSpells, 0);

                    string[] waterSpells = new string[waterMagicSpells.Count];
                    waterMagicSpells.Keys.CopyTo(waterSpells, 0);

                    string[] lightningSpells = new string[lightningMagicSpells.Count];
                    lightningMagicSpells.Keys.CopyTo(lightningSpells, 0);

                    string[] darkSpells = new string[darkMagicSpells.Count];
                    darkMagicSpells.Keys.CopyTo(darkSpells, 0);

                    string[] lightSpells = new string[lightMagicSpells.Count] ;
                    lightMagicSpells.Keys.CopyTo(lightSpells, 0);

                    string[] eucladianSpells = new string[eucladianMagicSpells.Count];
                    eucladianMagicSpells.Keys.CopyTo(eucladianSpells, 0);

                       

                    // Allow the user to choose two magic specialties
                    for (int k = 0; k < 2; k++)
                    {
                        int chosenSpecialtyIndex;

                        smoothPrinting.FastPrint("\nChoose a magic specialty by entering the corresponding number:");
                        while (!int.TryParse(Console.ReadLine(), out chosenSpecialtyIndex) || chosenSpecialtyIndex < 1 || chosenSpecialtyIndex > magicChoices.Length) // Conditions to ensure user doesn't input trash
                        {
                            Console.WriteLine("Invalid choice. Please enter a valid number corresponding to the magic specialty.");
                        }

                        chosenSpecialties.Add(magicChoices[chosenSpecialtyIndex - 1]);
                    }


                    int totalSpellsDisplayed = 0; // Keep track of the total spells displayed

                    // Will be used to check the magic specialities chosen by the user before displaying the range of spells they can pick


                    for (int z = 0; z < chosenSpecialties.Count; z++)
                    {
                        Console.WriteLine("\n" + chosenSpecialties[z] + " Spells:");

                        switch (chosenSpecialties[z])
                        {
                            case "Fire":
                                foreach (string spell in fireSpells)
                                {
                                    smoothPrinting.FastPrint((totalSpellsDisplayed + 1) + ". " + spell);
                                    totalSpellsDisplayed++;
                                    Console.WriteLine("\nPress Enter to see the next spell...");
                                    Console.ReadLine();
                                }
                                break;
                            case "Lightning":
                                foreach (string spell in lightningSpells)
                                {
                                    smoothPrinting.FastPrint((totalSpellsDisplayed + 1) + ". " + spell);
                                    totalSpellsDisplayed++;
                                    Console.WriteLine("\nPress Enter to see the next spell...");
                                    Console.ReadLine();
                                }
                                break;
                            case "Water":
                                foreach (string spell in waterSpells)
                                {
                                    smoothPrinting.FastPrint((totalSpellsDisplayed + 1) + ". " + spell);
                                    totalSpellsDisplayed++;
                                    Console.WriteLine("\nPress Enter to see the next spell...");
                                    Console.ReadLine();
                                }
                                break;
                            case "Dark":
                                foreach (string spell in darkSpells)
                                {
                                    smoothPrinting.FastPrint((totalSpellsDisplayed + 1) + ". " + spell);
                                    totalSpellsDisplayed++;
                                    Console.WriteLine("\nPress Enter to see the next spell...");
                                    Console.ReadLine();
                                }
                                break;
                            case "Light":
                                foreach (string spell in lightSpells)
                                {
                                    smoothPrinting.FastPrint((totalSpellsDisplayed + 1) + ". " + spell);
                                    totalSpellsDisplayed++;
                                    Console.WriteLine("\nPress Enter to see the next spell...");
                                    Console.ReadLine();
                                }
                                break;
                            case "Eucladian-Magic":
                                foreach (string spell in eucladianSpells)
                                {
                                    smoothPrinting.FastPrint((totalSpellsDisplayed + 1) + ". " + spell);
                                    totalSpellsDisplayed++;
                                    Console.WriteLine("\nPress Enter to see the next spell...");
                                    Console.ReadLine();
                                }
                                break;
                            default:
                                smoothPrinting.FastPrint("Unknown Error");
                                Environment.Exit(0);
                                break;
                        }

                        // Optionally, you can prompt for the next specialty
                        if (z < chosenSpecialties.Count - 1)
                        {
                            Console.WriteLine("\nPress Enter to see the spells for the next specialty...");
                            Console.ReadLine();
                        }
                    }


                    for (int specialityIndex = 0; specialityIndex < chosenSpecialties.Count; specialityIndex++)
                    {
                        Console.WriteLine($"Select 2 magic spells for {chosenSpecialties[specialityIndex]} by entering the corresponding numbers. (1-4 for each element)");

                        List<string> currentMagicSpells = new List<string>(); // Dynamic list which will be used to store the chosen magical spells of the users

                        switch (chosenSpecialties[specialityIndex])
                        {
                            case "Fire":
                                currentMagicSpells = fireSpells.ToList();
                                break;
                            case "Lightning":
                                currentMagicSpells = lightSpells.ToList();
                                break;
                            case "Water":
                                currentMagicSpells = waterSpells.ToList();
                                break;
                            case "Dark":
                                currentMagicSpells = darkSpells.ToList();
                                break;
                            case "Light":
                                currentMagicSpells = lightSpells.ToList();
                                break;
                            case "Eucladian-Magic":
                                currentMagicSpells = eucladianSpells.ToList();
                                break;
                            default:
                                Console.WriteLine("Unknown magic speciality.");
                                Environment.Exit(0);
                                break;
                        }

                        int spellIndex = 0; // Keep track of index within array

                        for (int spellNumber = 0; spellNumber < 2; spellNumber++)
                        {
                            Console.WriteLine($"Choose magic spell #{spellNumber + 1} for {chosenSpecialties[specialityIndex]}:");
                            int magicSpellChoice;
                            while (!int.TryParse(Console.ReadLine(), out magicSpellChoice) || magicSpellChoice < 1 || magicSpellChoice > magicChoices.Length)
                            {
                                Console.WriteLine("Invalid choice. Please enter a valid number corresponding to the magic specialty.");
                            }
                            magicSpells.Add(currentMagicSpells[magicSpellChoice - 1]);
                            spellIndex++;
                        }
                    }

                    Console.Clear(); // Neatness

                    Mage newWizard = new Mage(name, staffName, staffWeaponType, chosenSpecialties.ToArray(), arcaniaGoldCoins, magicSpells.ToArray(), mageInventory.ToArray());


                    smoothPrinting.FastPrint("Mage Name: " + name + "\nMage's Weapon Type: " + staffWeaponType + "\nMage's Weapon: " + staffName +
                    "\nMage's Magic Specialties: " + string.Join(", ", chosenSpecialties));
                    smoothPrinting.FastPrint("\nMage's Chosen Spells: " + string.Join(", ", magicSpells));


                    Console.WriteLine("\nWould you like to now embark on your journey in the world of Arcania? (1 for Yes and 2 for No)");
                    startMageJourneyInput = Convert.ToInt32(Console.ReadLine()); // Register the user input


                    switch (startMageJourneyInput)
                    {
                        case 1:
                            Console.Clear(); // Neatness
                            Console.WriteLine("You will now be sent to the world of Arcania, make sure to not die.");
                            userJourney wizardJourney = new userJourney(); // Journey start!
                            wizardJourney.usersFirstJourney();
                            break;

                        case 2:
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.WriteLine("Why are you here then?");
                            Console.ReadLine();
                            break;

                        default:
                            Console.WriteLine("Invalid input, please input a sensible value again.");
                            break;

                    }
                    break;

                case 2:
                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("Knight's aren't avaliable as of present :3");
                    break;


                case 3:
                    int startPirateJourneyInput;
                    Console.Clear();
                    string pirateName;

                    // Dictionary to store the weapon types that a pirate can retrieve before embarking on their journey
                    Dictionary<string, (int, string)> pirateWeaponChoice = new Dictionary<string, (int, string)>()
                    {
                        { "Sharp Cutlass", (6, "Sword") },
                        { "Raging Horn", (8, "Longsword") },
                        { "Somali Pride", (11, "Sword") },
                        { "Mohamad's Dagger", (20, "Dagger") },
                        { "Dilapidated Thorn", (14, "Katana") }
                    };

                    // Dictionary to store pirate aura types
                    Dictionary<string, (int, string)> pirateAuras = new Dictionary<string, (int, string)>()
                    {
                        { "Bloodlust", (3, "Rare") },
                        { "Kraken's Pride", (4, "Rare") },
                        { "Mystical Remenance", (8, "Unique") },
                        { "Wriath's Omen", (2, "Uncommon") },
                        { "Devious Sigma Pirate", (20, "Legendary") },
                        { "Somalia's Exudance", (12, "Unique") }
                    };


                    // Story output
                    smoothPrinting.FastPrint("You are a proud Somali Pirate, one who has explored the vast open seas for many years, and now you feel that your ready for a new adventure!\n");

                    // Take users name
                    Console.WriteLine("Enter your name:");
                    pirateName = Convert.ToString(Console.ReadLine());

                    arcaniaGoldCoins = 0; // Preset zero

                    List<string> pirateInventory = new List<string>();

                    // User will be randomly assigned a weapon
                    Random weaponPirateRandom = new Random();
                    int pirateRandomWeaponAssignment = weaponPirateRandom.Next(0, pirateWeaponChoice.Count); // Allow for the random generation between index 0 and length of the dictionary

                    // Get a random weapon name from the dictionary
                    string pirateWeaponName = pirateWeaponChoice.ElementAt(pirateRandomWeaponAssignment).Key;

                    pirateInventory.Add(pirateWeaponName); // Insert the weapon into the user's inventory


                    // User will be randomly assigned an aura
                    Random auraPirateRandom = new Random();
                    int pirateAuraRoll = auraPirateRandom.Next(0, pirateAuras.Count); // Allow for the random generation between index 0 and length of the dictionary

                    // Generate random aura for pirate
                    string pirateAuraName = pirateAuras.ElementAt(pirateAuraRoll).Key;


                    // Predefined attributes for a pirate
                    string pirateAtkName = "Slash"; 
                    string pirateSpecialAtkName = "Pirate's might"; 
                    string pirateWeaponType = "Sword/Longsword/Dagger/Blades";  

                    SomaliPirate newPirate = new SomaliPirate(pirateName, pirateWeaponName, pirateWeaponType, pirateAuraName, pirateAtkName, pirateSpecialAtkName, pirateInventory.ToArray(), arcaniaGoldCoins); // Generate the pirate details
                    
                    Console.Clear(); // Neater

                    smoothPrinting.FastPrint("Pirates name: " + pirateName + "\nPirate's Weapon Type: " + pirateWeaponType + "\nPirate's Weapon: " + pirateWeaponName +
                    "\nPirate's Aura: " + string.Join(", ", pirateAuraName)); // Display information to the user


                    Console.WriteLine("\nWould you like to now embark on your journey in the world of Arcania? (1 for Yes and 2 for No)");
                    startPirateJourneyInput = Convert.ToInt32(Console.ReadLine()); // Register the user input

                    // Future reference: For each class chosen, make a seperate method for them
                    switch (startPirateJourneyInput)
                    {
                        case 1:
                            Console.Clear(); // Neatness
                            Console.WriteLine("You will now be sent to the world of Arcania, make sure to not die.");
                            userJourney pirateJourney = new userJourney(); // Journey start!
                            pirateJourney.usersFirstJourney();
                            break;

                        case 2:
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.WriteLine("Why are you here then?");
                            Console.ReadLine();
                            break;

                        default:
                            Console.WriteLine("Invalid input, please input a sensible value again.");
                            break;

                    }
                    break;


                case 4:
                    Console.WriteLine("After long endurance of physical training, you develop eyes as keen as an owl and your bowmanship is first class.");
                    Console.WriteLine("What is your name?");
                    name = Convert.ToString(Console.ReadLine());

                    break;
                case 5:
                    Console.ForegroundColor = ConsoleColor.Red; // devious colour hahahaha
                    Console.WriteLine("I'm in your walls :3");
                    Console.WriteLine("You died");
                    Console.ReadLine();

                    break;
                case 6:
                    Console.ForegroundColor = ConsoleColor.Red; // devious colour hahahaha
                    Console.WriteLine("You are not :3, therefore you are not worthy of becoming a devious sigma");
                    Console.WriteLine("You died");
                    Console.ReadLine();

                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red; // devious colour hahahaha
                    Console.WriteLine("Please pick a sensible choice and understand if you do that again you'll be punished hahaha");

                    break;
            }

        }

    }


    public class userJourney // Once the user selects a class, they'll proceed onto their journey
    {
        int fightChoice;
        string[] customaryScenarios = { "You embark on a long journey, you find yourself lost midway throughout the journey. There appears a dragon, with fangs as sharp as blades and a gaze so intense that you begin to question your fighting prowess despite your training. What do you do?" };

        // Non-static scenarios will be introduced later in the game if I can be asked
        string fixedScenario = "\nYou embark on a long journey, you find yourself lost midway, your eyes are surrounded by vast levels of fog, mitigating your view of the perspective ahead. Closeby, there appears a dragon, with fangs as sharp as blades and a gaze so intense that you begin to question your fighting prowess despite your training.";

        public void usersFirstJourney()
        {
            SmoothConsole smoothPrinting = new SmoothConsole();
            smoothPrinting.FastPrint(fixedScenario + "\n");
            Console.WriteLine("\nWhat do you do?");

            smoothPrinting.FastPrint("\n1. Fight back");
            smoothPrinting.FastPrint("\n2. Escape\n");
            fightChoice = Convert.ToInt32(Console.ReadLine());

            switch (fightChoice)
            {
                case 1:
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("Your level is too low, the dragon proceeds to consume you whole in your defenseless state.");

                    Console.WriteLine("You died.");
                    break;

                case 2:
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.WriteLine("Wise choice, you successfully escape with all your limbs intact.");
                    break;
                default:
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("Don't know why you did that, but you died LOL");
                    break;

            }


        }




    }

    public class SmoothConsole // This will be used to ensure output from the console is smooth and aesthetic looking
{
        public void SlowPrint(string text) // Slower, smoother console output
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(50);
            }
        }

        public void FastPrint(string text) // Faster, smoother console output
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(20);
            }
        }

    }

}
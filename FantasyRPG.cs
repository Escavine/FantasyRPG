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

        // Levelling attributes
        public float exp;
        public int level;
        private int experienceRequiredForNextLevel;

        public CharacterDefault(string _name, string _weaponName, string _weaponType) // Default preset for all classes during the start of the game :3
        {
            name = _name;
            weaponType = _weaponType;
            weaponName = _weaponName;
            health = 100;
            exp = 0f;
            numOfPotionsInInventory = 0;
            maxPotions = 5;
            level = 1;
            mana = 100;

        }


        // All methods for all user choice classes
        public void CheckInventory()
        {
            // Feature will be added later during development :3
        }

        public void CheckStatus()
        {
            Console.WriteLine(name + " current status: ");
            Console.WriteLine("Health: " + health);
            Console.WriteLine("Experience accumuated: " + exp);
            Console.WriteLine("Current level: " + level);
        }

        public void Meditate() // Used for recovering spells in inventory and mana
        {
            Console.WriteLine(name + " has meditated ");
            mana = mana + 20;
            health = health + 20;
            Console.WriteLine(name + " has meditated and has recovered: ");
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
        public void LevelUp()
        {
            level++;
            Console.WriteLine(name + " has levelled up! " + " You are now level " + level);
            CalculateExperienceForNextLevel();

        }
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

        public Knight(string _name, string _weaponName, string _weaponType, string _specialAtkName) : base(_name, _weaponName, _weaponType)
        {
            name = _name;
            weaponName = _weaponName;
            weaponType = _weaponType;
            specialAtkName = _specialAtkName;
            normalAtkName = "Sword Slash";
            specialAtkRecharge = 100; // 100% preset
            specialAtkDmg = 10; // Preset damage from sword special attack
            normalAtkDmg = 4;
        }

        public void BasicAtk()
        {
            Console.WriteLine(name + " has used " + normalAtkName + " and has dealt " + normalAtkDmg + " damage.");
        }

        public void SpecialAtk()
        {
            Console.WriteLine(name + " has used " + specialAtkName + " and has dealt " + specialAtkDmg + " damage.");
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

        public Mage(string _name, string _weaponName, string _weaponType, string[] _magicSpeciality, string[] _magicSpells) : base(_name, _weaponName, _weaponType)
        {
            name = _name;
            weaponName = _weaponName;
            weaponType = _weaponType;
            magicSpeciality = _magicSpeciality;
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
            Console.WriteLine("\n" + name + "'s" + " current known magic specialities.");


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
                chosenSpecialty = null; // Clear the array of any specialties, for the next time this is run


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

            for (int z = 0; z < chosenSpecialty.Length; z++)
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

                // Optionally, you can prompt for the next specialty
                if (z < chosenSpecialty.Length - 1)
                {
                    Console.WriteLine("\nPress Enter to see the spells for the next specialty...");
                    Console.ReadLine();
                }

            }

            int specialityIndex = 0;

            for (specialityIndex = 0; specialityIndex < chosenSpecialty.Length; specialityIndex++)
            {
                Console.WriteLine($"Select 2 magic spells for {chosenSpecialty[specialityIndex]} by entering the corresponding numbers. (1-4 for each element)");

                List<string> currentMagicSpells = new List<string>(); // Dynamic list which will be used to store the chosen magical spells of the users

                switch (chosenSpecialty[specialityIndex])
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

            for (int i = 0;  i < magicSpells.Length; i++) // Display the magic spells that the user knows
            {
                Console.WriteLine(magicSpells[i]);
            }
        }
    }


    class SomaliPirate : CharacterDefault
    {
        public string weaponAura, normalAtkName, specialAtkName;
        public int normalAtkDmg, specialAtkDmg, specialAtkCharge;
        public SomaliPirate(string _name, string _weaponName, string _weaponType, string _weaponAura, string _normalAtkName, string _specialAtkName) : base(_name, _weaponName, _weaponName)
        {
            name = _name;
            weaponName = _weaponName;
            weaponType = _weaponType;
            weaponAura = _weaponAura;
            normalAtkName = _normalAtkName; // Presets for all new Somali Pirates in the game
            specialAtkName = _specialAtkName;
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
        public Archer(string _name, string _weaponName, string _weaponType) : base(_name, _weaponName, _weaponType)
        {
            name = _name;
            weaponName = _weaponName;
            weaponType = _weaponType;
        }
    }
    class gameMenu
    {
        static void Main(string[] args)
        {
            // Future reference: Implementing AI mobs and perhaps AI individuals

            int userChoice; // Used for the start of the game
            string[] gameTips = {"Did you know that every 10 levels, you can get an extra ability/speciality?",
                "This game is still in development, so if there's an issue please contact me through my GitHub (Escavine) and send a pull request which I'll review.",
            "Eucladian abilities are very overpowered, but in turn they'll cost you some health.", "This game have a sneaky RNG factor, you'll see later as you play :3",
            "For you down bad individuals, I MIGHT introduce a harem feature, perhaps implement it with AI, imagine how insane that'll be? LOL" }; // Array containing necessary game tips, more will be added in the future.

            // Initiation of the console game
            Console.WriteLine("---------FantasyRPG----------\n");
            Console.WriteLine("Game advice: When inputting values, input a corresponding value to the action (e.g. enter the value 1 in order to start the game\n"); // Display game advice
            Random ran = new Random();
            int ran_num = ran.Next(0, 5);
            Console.WriteLine("Game Tip: " + gameTips[ran_num] + "\n"); // Display a random game tip in the menu

            Console.WriteLine("Game Menu\n");
            Console.WriteLine("1. Get started");
            Console.WriteLine("2. Load save game"); // Feature doesn't work yet
            Console.WriteLine("3. Help");
            Console.WriteLine("4. Make a suggestion"); // Feature doesn't work yet
            Console.WriteLine("5. Future plans");

            // Register user input
            userChoice = Convert.ToInt32(Console.ReadLine());

            switch (userChoice)
            {
                case 1:
                    Console.WriteLine("\nYour game session will now begin!");
                    classSelection selectClass = new classSelection(); // Create a new game session
                    selectClass.userClass(); // Proceed to let the user pick a character class
                    break;
                case 2:
                    Console.WriteLine("Unfortunately, this feature isn't avaliable yet :3");
                    Console.ReadKey(); // Read user input before terminating
                    break;
                case 3:
                    int userInput;
                    string[] gameAdvice = { "You might die at any point within the game unknowingly.",
                        "Eucladian abilities are quite overpowered, if you find the opportunity to pursue it, then do so.",
                    "Having a strong romantical bond with someone, can potentially increase your abilities.", "There are many classes to choose from, all having unique features.",
                    "Avoid fighting overpowered foes early in-game (i.e. dragons), you'll probably get destroyed." };
                    Console.WriteLine("--------Help Section--------\n");
                    Console.WriteLine("What is FantasyRPG?\n");

                    // Introduction to Arcania, the world of FantasyRPG
                    Console.WriteLine("Welcome to FantasyRPG, a console-based game that transports you to the enchanting realm of Arcania!");
                    Console.WriteLine("Embark on an epic journey through a vast and mystical world, brimming with hidden treasures awaiting discovery.");
                    Console.WriteLine("The path ahead won't be an easy one – be prepared to face life-and-death situations, battling formidable foes and overcoming treacherous obstacles.");

                    Console.WriteLine("\nIn Arcania, every choice you make shapes your destiny. As you navigate through the immersive landscape, forge alliances with fellow travelers and encounter mythical creatures, you'll find yourself entangled in a web of friendships.");

                    Console.WriteLine("\nBut beware, adventurer, for danger lurks in the shadows. Face cunning enemies, solve challenging puzzles, and unravel the mysteries that lay dormant in this magical land.");

                    Console.WriteLine("\nYet, amidst the chaos, there is a chance for something more. As you progress, open your heart to the possibility of romantic connections, adding a layer of complexity to your personal story.");

                    Console.WriteLine("\nAre you ready to delve into the heart of Arcania, where every decision shapes your fate? Your adventure begins now!");

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



                    break;
                case 4:
                    Console.WriteLine("Send a message to kmescavine@gmail.com in order to send your ideas!"); // Future reference: Use an SMTP feature to allow the user to input their email and send their suggestion
                    break;
                case 5:
                    int count = 1;
                    string[] futurePlans = { "Adding new classes", "Potential romance feature", "Harem feature (not likely)", "A chance of randomly dying", "Illnesses and cures", "Game difficulty (easy, normal, hard, impossible)" };
                    Console.WriteLine("Future plans for FantasyRPG include:\n");

                    foreach (string plan in futurePlans)
                    {
                        Console.WriteLine("Plan " + count + ": " + plan + "\n");
                        count++;
                    }

                    Console.ReadKey(); // Wait for key input

                    break;
                default:
                    Console.WriteLine("Invalid option, please try again!");
                    break;
            }

        }
    }

    public class classSelection // This class will allow a user to pick from a variety of different roles in the game, before embarking on their journey.
    {
        public void userClass()
        {
            int userChoice; // Define the user choice

            // Defining the different classes and rarity of items
            string[] fantasyClasses = { "Mage", "Knight", "Somali Pirate", "Shadowwrath", "Archer", "Assassin" }; // Predefined array of roles
            string[] rarity = { "Archaic", "Uncommon", "Mythical", "Divine" }; // Predefined values :3
            int num = 1;

            Console.WriteLine("Welcome to the dungeon game!");
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

                    // Arrays containing the variety of different magic choices, spells and weapons.
                    string[] magicChoices = { "Fire", "Lightning", "Water", "Dark", "Light", "Eucladian-Magic" }; // Future Reference: Make a method in the Mage Class that allows for a mage to learn more magic specialities and skills as they level up.
                    string[] fireMagicSpells = { "Infrared", "Blazing Rage", "Flamestrike", "Pyroburst", "Phoenix Fury" }; // Future Reference: Add a damage system for the magic spells (e.g. infrared deals 8 damage etc.)
                    string[] lightningMagicSpells = { "Thunderstrike", "Striking Surge", "Volt Surge", "Arcane Thunder" };
                    string[] waterMagicSpells = { "Aqua Torrent", "Hydroburst", "Lunar Tide", "Ripple Cascade" };
                    string[] darkMagicSpells = { "Shadow Veil", "Umbral Surge", "Wraith's Curse", "Eclipsed Oblivion" };
                    string[] lightMagicSpells = { "Luminous Beam", "Solar Flare", "Etherial Halo", "Aurora's Illumination", "Divine Judgement" };
                    string[] eucladianMagicSpells = { "Esoteric Paradigm", "Fractural Fissure", "Quantum Flux", "Etherial Nexus" };
                    string[] starterMageWeapons = { "Weathered Oakwand", "Ancient Runestaff", "Runic Wooden Scepter", "Dusty Relic Rod", "Emerald Crystal Staff" };

                    Console.Clear(); // Cleaning purposes
                    Console.WriteLine("\n");
                    Console.WriteLine("Mage's Route");
                    Console.WriteLine("\nYou undergo intense mana training and finally become a Mage.");

                    Console.WriteLine("What is your name?");
                    string name = Convert.ToString(Console.ReadLine());

                    Random ranNum = new Random();
                    int ran_num = ranNum.Next(0, 4); // Select a random weapon for the user

                    string weaponName = starterMageWeapons[ran_num]; // Assigns a random weapon for the user
                    string weaponType = "Staff";

                    Console.WriteLine("\nChoose two magic specialties from the list: \n");

                    List<string> chosenSpecialties = new List<string>(); // Chosen magic specialities
                    List<string> magicSpells = new List<string>(); // Chosen magical spells

                    // Display all the magic choices to the user
                    for (int j = 0; j < magicChoices.Length; j++)
                    {
                        Console.WriteLine(choiceIncrementer + ". " + magicChoices[j]);
                        choiceIncrementer++;
                    }

                    // Allow the user to choose two magic specialties
                    for (int k = 0; k < 2; k++)
                    {
                        int chosenSpecialtyIndex;

                        Console.WriteLine("\nChoose a magic specialty by entering the corresponding number:");
                        while (!int.TryParse(Console.ReadLine(), out chosenSpecialtyIndex) || chosenSpecialtyIndex < 1 || chosenSpecialtyIndex > magicChoices.Length)
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


                    Mage newWizard = new Mage(name, weaponName, weaponType, chosenSpecialties.ToArray(), magicSpells.ToArray());
                    newWizard.chooseNewSpeciality(chosenSpecialties.ToArray(), name); // Debugging 


                    Console.WriteLine("Mage Name: " + name + "\nMage's Weapon Type: " + weaponType + "\nMage's Weapon: " + weaponName +
                            "\nMage's Magic Specialties: " + string.Join(", ", chosenSpecialties));
                    Console.WriteLine("Mage's Chosen Spells: " + string.Join(", ", magicSpells));


                    userJourney wizardJourney = new userJourney(); // Journey start!
                    wizardJourney.usersFirstJourney();
                    break;

                case 2:


                case 3:

                case 4:
                    Console.WriteLine("After long endurance of physical training, your eyes are as sharp as fangs and bowmanship is now your speciality.");
                    Console.WriteLine("What is your name?");
                    name = Convert.ToString(Console.ReadLine());
                    break;
                case 5:
                    Console.ForegroundColor = ConsoleColor.Red; // devious colour hahahaha
                    Console.WriteLine("I'm in your walls :3");
                    Console.WriteLine("You died");
                    Environment.Exit(0);
                    break;
                case 6:
                    Console.ForegroundColor = ConsoleColor.Red; // devious colour hahahaha
                    Console.WriteLine("You are not :3, therefore you are not worthy of becoming a devious sigma");
                    Console.WriteLine("You died");
                    Environment.Exit(0);
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
        string fixedScenario = "\nYou embark on a long journey, you find yourself lost midway, your eyes are surrounded by vast levels of fog, mitigating your view of the perspective ahead. Closeby, there appears a dragon, with fangs as sharp as blades and a gaze so intense that you begin to question your fighting prowess despite your training. What do you do?";

        public void usersFirstJourney()
        {

            Console.WriteLine(fixedScenario);

            Console.WriteLine("1. Fight back");
            Console.WriteLine("2. Escape");
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

}
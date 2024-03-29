﻿using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;


// FantasyRPG: A console based RPG game, which sole purpose is to improve on my current programming skills (mainly OOP as that is my weakness).

namespace FantasyRPG
{
    public class CharacterDefault // fixed preset for all classes
    {
        // Generic character attributes
        public string name;
        public bool inCombat = false; // Combat status (for the combat system)
        public int currentHealth, maxHealth;
        public List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription, string category, int quantity)> weapon;
        public float numOfPotionsInInventory;
        public float maxPotions;
        public int currentMana, maxMana;
        public List<(string itemName, string itemDescription, string itemRarity, int itemPower, string category, int quantity)> currentInventory; // Will contain item name, description, rarity and power (i.e. healing or attack etc.) 
        public int arcaniaGoldCoins; // Currency for the city of Arcanith
        public int specialAtkRecharge;// Percentage value, going upto 100%
        public List<(string npcName, string npcDescription, string npcAffiliation)> npcsEncountered;
        // public int atk;
        // public int def;

        // Levelling attributes
        public float exp;
        public int level;
        private int experienceRequiredForNextLevel;
        // public int randomDyingChance; hehe


        private readonly UIManager UI;
        private readonly SmoothConsole smoothPrinting;

        public CharacterDefault(string _name, List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription, string category, int quantity)> _weapon, List<(string itemName, string itemDescription, string itemRarity, int itemPower, string category, int quantity)> _currentInventory, int _arcaniaGoldCoins, int specialAtkRecharge, List<(string npcName, string npcDescription, string npcAffiliation)> _npcsEncountered, bool _inCombat) // Default preset for all classes during the start of the game :3
        {
            name = _name;
            weapon = _weapon; // WIll store the details of the given weapon (i.e. weapon name, type, damage, etc.)
            currentInventory = _currentInventory;
            npcsEncountered = null; // During the start of the game, the user will have not encountered any NPC's.
            specialAtkRecharge = 0; // Preset to 0%, as user attacks this will linearly rise
            arcaniaGoldCoins = 0;
            currentHealth = 100;
            maxHealth = 100; // This can increase overtime, through gaining more experience + SP (Will be introduced in the future)
            exp = 0f;
            numOfPotionsInInventory = 0;
            maxPotions = 5;
            level = 1;
            currentMana = 100;
            maxMana = 100; // This can increase overtime, through gaining more experience + SP (Will be introduced in the future)
            inCombat = _inCombat;
            UI = new UIManager();
            smoothPrinting = new SmoothConsole();
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
            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine($"FantasyRPG: {name}'s Inventory Check");
            smoothPrinting.PrintLine("--------------------------------------------------");

            // Loop through and display the users inventor
            foreach (var item in currentInventory)
            {
                smoothPrinting.RapidPrint($"\nItem Name: {item.itemName}\nItem Description: {item.itemDescription}\nItem Rarity: {item.itemRarity}\nItem Damage/Healing Property: {item.itemPower}\n\n");
            }


        }

        // Allow for the user to check their current status
        public void CheckStatus(CharacterDefault character)
        {
            string userInput;

            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine($"FantasyRPG: {character.name}'s Status Check");
            smoothPrinting.PrintLine("--------------------------------------------------");

            // Display the user's status
            smoothPrinting.RapidPrint($"\nCurrent Level: {character.level}\n");
            smoothPrinting.RapidPrint($"\nExperience accumulated: {character.exp}\n");
            UI.DisplayProgressBar("Remaining Health:", character.currentHealth, character.maxHealth, 30);
            Console.WriteLine(); // Spacing
            UI.DisplayProgressBar("Current Mana:", character.currentMana, character.maxMana, 30);
            Console.WriteLine(); // Spacing
            smoothPrinting.RapidPrint("\nWould you like to see the EXP required to get to the next level? (1 for 'Yes' and anything else for 'No')\n");
            smoothPrinting.RapidPrint("\nEnter a corresponding value: ");
            userInput = Console.ReadLine(); // Register the input

            switch (userInput)
            {
                case "1":
                    Console.Clear();
                    if (character is Mage)
                    {
                        CalculateExperienceForNextLevel((Mage)character);
                    }
                    else if (character is SomaliPirate)
                    {
                        CalculateExperienceForNextLevel((SomaliPirate)character);
                    }
                    break;
                default:
                    smoothPrinting.RapidPrint("\nYou will now return to the dashboard.");
                    Console.ReadKey(); // Register user input
                    Console.Clear(); // Clear the console to avoid overlapping
                    gameDashboard dash = new gameDashboard();
                    dash.dashboard(character);

                    break;
            }
        }


        // Combat System (All Classes)
        public void CombatSystem(CharacterDefault character, MobDefault mob, bool quickDisplay)
        {
            // WIll be used to display the Mage Combat System header
            void DisplayMageCombatSystemHeader()
            {
                smoothPrinting.PrintLine("--------------------------------------------------");
                smoothPrinting.PrintLine("FantasyRPG: Mage Combat System");
                smoothPrinting.PrintLine("--------------------------------------------------");
            }

            void DisplayMageStatus(CharacterDefault character, MobDefault mob, bool quickDisplay) // Naturally this takes in the mage class and the given mob
            {
                Console.ResetColor();
                bool userTurn, enemyTurn; // These boolean measures are to create the turn based dynamic for the game
                UIManager UI = new UIManager(); // Engage the UI manager for progress bars

                int? numCount = 1; // Will display the numeric choices for the Mage's options
                string? userChoice;

                string[] mageChoices = new string[] { "Attack", "Use Item", "Check Inventory", "Attempt Escape (WARNING: Low Chance)" }; // Array displaying the different Mage's options


                if (mob.currentMobHealth == 0) // Check everytime if the mob has died
                {
                    mob.mobDeath(mob, character); // Run the following method to display the relevant information

                }
                else if (character.currentHealth == 0) // Should the user die instead
                {
                    character.CharacterDeath(character, mob);
                }
                else
                {
                    if (quickDisplay == true)
                    {
                        DisplayMageCombatSystemHeader(); // Display the MCS (Mage Combat System Header)

                        smoothPrinting.RapidPrint($"{character.name} - Mage Status\n");
                        smoothPrinting.RapidPrint($"{mob.name} - Enemy\n");

                        smoothPrinting.RapidPrint("\nMage Status\n");
                        UI.DisplayProgressBar($"{character.name}'s Health", character.currentHealth, character.maxHealth, 30); // Display Mage's health
                        Console.WriteLine(); // Spacing

                        UI.DisplayProgressBar($"{character.name}'s Mana", character.currentMana, character.maxMana, 30); // Display Mage's remaining mana
                        Console.WriteLine(); // Spacing

                        UI.DisplayProgressBar($"{mob.name}'s Health:", mob.currentMobHealth, mob.maxMobHealth, 30); // Display enemies health
                        Console.WriteLine(); // Spacing

                        UI.DisplayProgressBar($"{mob.name} ULT (%):", mob.specialAtkRecharge, 100, 30); // Display enemies special attack recharge
                        Console.WriteLine(); // Spacing

                        foreach (var numOfPotions in character.currentInventory)
                        {
                            if (numOfPotions.category == "Potion") // Filter the current inventory via a category basis
                            {
                                if (numOfPotions.quantity > 0)
                                {
                                    Console.WriteLine($"\nRemaining Healing Potions: {numOfPotions.quantity}\n"); // Display Mage's remaining potions
                                }
                            }

                        }

                        if (character.currentHealth <= 30) // Check if users health is low
                        {
                            smoothPrinting.RapidPrint("\nWarning: Your health is critically low, consider using a health potion or a recovery spell.");
                            Console.WriteLine(); // Spacing
                        }

                        if (character.currentMana <= 30) // Check if users mana levels are low
                        {
                            smoothPrinting.RapidPrint("\nWarning: Your mana is critically low. Consider using a mana potion or a recovery spell to replenish your mana reserves.");
                            Console.WriteLine(); // Spacing
                        }

                        foreach (var choice in mageChoices)
                        {
                            Console.WriteLine($"\n{numCount}. {choice}");
                            numCount++; // Increment the value to display the other remaining choices
                        }

                        smoothPrinting.RapidPrint("\nEnter a corresponding value: ");
                        userChoice = Convert.ToString(Console.ReadLine()); // Register Mage's choice

                        switch (userChoice)
                        {
                            case "1":
                                Console.Clear(); // Clear the console, to avoid overlapping
                                MageSpellAttack(character, mob, userTurn = true, enemyTurn = false, quickDisplay);
                                break;
                            case "2":
                                Console.Clear(); // Clear console, to mitigate overlapping

                                smoothPrinting.PrintLine("--------------------------------------------------");
                                smoothPrinting.PrintLine("FantasyRPG: Combat System - Healing");
                                smoothPrinting.PrintLine("--------------------------------------------------");

                                string? usePotion;

                                // Future reference: if a user has a variety of potions, allow them to filter between then and use the selected one, this is static for now

                                foreach (var potion in character.currentInventory)
                                {
                                    if (potion.category == "Potion") // Filter the current inventory via a category basis
                                    {
                                        if (potion.quantity > 0)
                                        {
                                            smoothPrinting.RapidPrint($"\nHealing Potion: {potion.itemName}\nPotion Description: {potion.itemDescription}\nPotion Potency: {potion.itemPower}\n"); // Display Mage's remaining potions
                                        }
                                    }

                                }

                                smoothPrinting.RapidPrint("\nWould you like to use the following potion? ('1' for yes and any other key for no): ");
                                usePotion = Console.ReadLine();

                                // Future reference: Potions can have various effects, this is catered for healing as of present. This will need to be changed in the future. Infact, the whole functionality should be placed in a function rather than here, which can be used by all classes
                                switch (usePotion)
                                {
                                    case "1":
                                        for (int i = 0; i < character.currentInventory.Count; i++)
                                        {
                                            var currentItem = character.currentInventory[i];
                                            // Check if the item is a potion
                                            if (currentItem.category == "Potion")
                                            {
                                                smoothPrinting.RapidPrint($"\n{character.name} has used {currentItem.itemName}, regaining +{currentItem.itemPower} health!\n");
                                                character.currentHealth += currentItem.itemPower; // Adjust the character's health
                                                                                                  // Create a new tuple with the updated quantity
                                                var updatedItem = (currentItem.itemName, currentItem.itemDescription, currentItem.itemRarity, currentItem.itemPower, currentItem.category, currentItem.quantity - 1);
                                                // Update the item in the list
                                                character.currentInventory[i] = updatedItem;
                                                UI.PromptUserToContinue();
                                                break; // Exit the loop after using the potion
                                            }
                                        }

                                        CombatSystem(character, mob, quickDisplay); // Return to combat system
                                        break;

                                    default:
                                        smoothPrinting.RapidPrint("\nPotion has not been used, returning back to combat system!");
                                        UI.PromptUserToContinue();
                                        CombatSystem(character, mob, quickDisplay); // Return to combat system
                                        break;
                                }
                                break;
                            case "3":
                                CheckInventory();
                                smoothPrinting.RapidPrint("\nAffirmative? If so, click any key to be redirected back to the M.C.S (Mage Combat System)");
                                Console.ReadKey(); // Register user's input
                                Console.Clear(); // Clear the console to prevent confusion
                                DisplayMageStatus(character, mob, quickDisplay = true); // Recurse back to the original function
                                break;
                            case "4":
                                // Generate a random value
                                Random ran = new Random();
                                int generatedValue = ran.Next(0, 5); // Testing, will be readjusted
                                // For this stage, if the user gets a value such as (i.e. 1, 10, 12, 15) they will luckily escape, otherwise they'll be locked into combat and cannot attempt escape again.


                                // Should the generated value be between the following ranges, then the user will escape
                                if (generatedValue == 1)
                                {
                                    smoothPrinting.RapidPrint($"\nYou have successfully escaped the grasp of {mob.name}.");
                                    // ForestOfMysteries returnToForest = new ForestOfMysteries();

                                    // Future reference: change this to return to wherever the user was intially, (i.e. if they were in the infinite dungeon, then return them to the dashboard)
                                    // returnToForest.forestOfMysteries(character); // Return to the forest, should the user be lucky
                                }
                                // Otherwise...
                                else
                                {
                                    enemyTurn = true; // Should the user fail to escape, then the mob will attack the user out of their foolishness
                                    smoothPrinting.RapidPrint("\nYou have failed to escape!");
                                    Console.ReadKey();
                                    Console.Clear(); // Clear the console
                                    mob.mobAttack(mob, character, enemyTurn); // Mob initates attack
                                }
                                break;
                            default:
                                smoothPrinting.RapidPrint("\nInvalid input, click any key to try again!");
                                Console.ReadKey();
                                Console.Clear(); // Clear the console after letting user read the error message
                                DisplayMageStatus(character, mob, quickDisplay = true); // Recurse back
                                break;
                        }

                    }
                    else
                    {
                        DisplayMageCombatSystemHeader(); // Display the MCS (Mage Combat System Header)

                        smoothPrinting.RapidPrint($"{character.name} - Mage Status\n");
                        smoothPrinting.RapidPrint($"{mob.name} - Enemy\n");

                        UI.DisplayProgressBar($"{character.name}'s Health", character.currentHealth, character.maxHealth, 30); // Display Mage's health
                        Console.WriteLine(); // Spacing

                        UI.DisplayProgressBar($"{character.name}'s Mana", character.currentMana, character.maxMana, 30); // Display Mage's remaining mana
                        Console.WriteLine(); // Spacing

                        UI.DisplayProgressBar($"{mob.name}'s Health:", mob.currentMobHealth, mob.maxMobHealth, 30); // Display enemies health
                        Console.WriteLine(); // Spacing

                        UI.DisplayProgressBar($"{mob.name}'s ULT (%):", mob.specialAtkRecharge, 100, 30); // Display enemies special attack recharge
                        Console.WriteLine(); // Spacing

                        foreach (var numOfPotions in character.currentInventory)
                        {
                            if (numOfPotions.category == "Potion") // Filter the current inventory via a category basis
                            {
                                if (numOfPotions.quantity > 0)
                                {
                                    smoothPrinting.RapidPrint($"\nRemaining Healing Potions: {numOfPotions.quantity}\n"); // Display Mage's remaining potions
                                }
                            }

                        }

                        if (character.currentHealth <= 30) // Check if users health is low
                        {
                            smoothPrinting.RapidPrint("WARINING: Your health is critically low, consider using a health potion or a recovery spell.");
                            Console.WriteLine(); // Spacing
                        }

                        if (character.currentMana <= 30) // Check if users mana levels are low
                        {
                            smoothPrinting.RapidPrint("Warning: Your mana is critically low. Consider using a mana potion or a recovery spell to replenish your mana reserves.");
                            Console.WriteLine(); // Spacing
                        }

                        foreach (var choice in mageChoices)
                        {
                            smoothPrinting.RapidPrint($"\n{numCount}. {choice}\n");
                            numCount++; // Increment the value to display the other remaining choices
                        }

                        smoothPrinting.RapidPrint("\nEnter a corresponding value: ");
                        userChoice = Convert.ToString(Console.ReadLine()); // Register Mage's choice

                        switch (userChoice)
                        {
                            case "1":
                                Console.Clear(); // Clear the console, to avoid overlapping
                                MageSpellAttack(character, mob, userTurn = true, enemyTurn = false, quickDisplay);
                                break;
                            case "2":
                                Console.Clear(); // Clear the console, to avoid overlapping

                                smoothPrinting.PrintLine("--------------------------------------------------");
                                smoothPrinting.PrintLine("FantasyRPG: Combat System - Healing");
                                smoothPrinting.PrintLine("--------------------------------------------------");

                                string? usePotion;

                                // Future reference: if a user has a variety of potions, allow them to filter between then and use the selected one, this is static for now

                                foreach (var potion in character.currentInventory)
                                {
                                    if (potion.category == "Potion") // Filter the current inventory via a category basis
                                    {
                                        if (potion.quantity > 0)
                                        {
                                            smoothPrinting.RapidPrint($"\nHealing Potion: {potion.itemName}\nPotion Description: {potion.itemDescription}\nPotion Potency: {potion.itemPower}\n"); // Display Mage's remaining potions
                                        }
                                    }

                                }

                                smoothPrinting.RapidPrint("\nWould you like to use the following potion? ('1' for yes and any other key for no): ");
                                usePotion = Console.ReadLine();

                                // Future reference: Potions can have various effects, this is catered for healing as of present. This will need to be changed in the future. Infact, the whole functionality should be placed in a function rather than here, which can be used by all classes
                                switch (usePotion)
                                {
                                    case "1":
                                        for (int i = 0; i < character.currentInventory.Count; i++)
                                        {
                                            var currentItem = character.currentInventory[i];
                                            // Check if the item is a potion
                                            if (currentItem.category == "Potion")
                                            {
                                                smoothPrinting.RapidPrint($"\n{character.name} has used {currentItem.itemName}, regaining +{currentItem.itemPower} health!\n");
                                                character.currentHealth += currentItem.itemPower; // Adjust the character's health
                                                                                                  // Create a new tuple with the updated quantity
                                                var updatedItem = (currentItem.itemName, currentItem.itemDescription, currentItem.itemRarity, currentItem.itemPower, currentItem.category, currentItem.quantity - 1);
                                                // Update the item in the list
                                                character.currentInventory[i] = updatedItem;
                                                UI.PromptUserToContinue();
                                                break; // Exit the loop after using the potion
                                            }
                                        }

                                        CombatSystem(character, mob, quickDisplay); // Return to combat system
                                        break;

                                    default:
                                        smoothPrinting.RapidPrint("\nPotion has not been used, returning back to combat system!");
                                        UI.PromptUserToContinue();
                                        CombatSystem(character, mob, quickDisplay); // Return to combat system
                                        break;
                                }

                                break;

                            case "3":
                                CheckInventory();
                                smoothPrinting.RapidPrint("\nAffirmative? If so, click any key to be redirected back to the M.C.S (Mage Combat System)");
                                Console.ReadKey(); // Register user's input
                                Console.Clear(); // Clear the console to prevent confusion
                                DisplayMageStatus(character, mob, quickDisplay = true); // Recurse back to the original function
                                break;
                            case "4":
                                // Generate a random value
                                Random ran = new Random();
                                int generatedValue = ran.Next(0, 1); // Testing, will be readjusted
                                // For this stage, if the user gets a value such as (i.e. 1, 10, 12, 15) they will luckily escape, otherwise they'll be locked into combat and cannot attempt escape again.


                                // Should the generated value be between the following ranges, then the user will escape
                                if (generatedValue == 0 || generatedValue == 1)
                                {
                                    smoothPrinting.RapidPrint($"\nYou have successfully escaped the grasp of {mob.name}.");
                                    Console.ReadKey(); // Read user input
                                    Console.Clear(); // Clear the console

                                    ForestOfMysteries returnToForest = new ForestOfMysteries();

                                    // Future reference: change this to return to wherever the user was intially, (i.e. if they were in the infinite dungeon, then return them to the dashboard)
                                    returnToForest.forestOfMysteries(character); // Return to the forest, should the user be lucky
                                }
                                // Otherwise...
                                else
                                {
                                    enemyTurn = true; // Should the user fail to escape, then the mob will attack the user out of their foolishness
                                    smoothPrinting.RapidPrint("\nYou have failed to escape!");
                                    Console.ReadKey();
                                    Console.Clear(); // Clear the console
                                    mob.mobAttack(mob, character, enemyTurn); // Mob initates attack
                                }
                                break;
                            default:
                                smoothPrinting.RapidPrint("\nInvalid input, click any key to try again!");
                                Console.ReadKey();
                                Console.Clear(); // Clear the console after letting user read the error message
                                DisplayMageStatus(character, mob, quickDisplay = true); // Recurse back
                                break;
                        }
                    }

                }

            }

            void MageSpellAttack(CharacterDefault character, MobDefault mob, bool userTurn, bool enemyTurn, bool quickDisplay) // Will load the Mage Combat System for fighting situations
            {
                UIManager UI = new UIManager(); // To display mana + health progress bar

                smoothPrinting.PrintLine("--------------------------------------------------");
                smoothPrinting.PrintLine("FantasyRPG: Mage Combat System - Attack");
                smoothPrinting.PrintLine("--------------------------------------------------");

                string? userInput; // Register the user input in string format for input validation purposes
                List<(string magicSpell, int damage, int manaRequirement)> chosenSpellForAttack = new List<(string magicSpell, int damage, int manaRequirement)>(); // Will be used to append the chosen spell to attack, and will be cleared through each iteration
                int spellCount = 1; // Likewise with the chosen spell, this will also be cleared through each iteration to keep track of number of user spells

                // magicSpecialties.ToList();

                // for (int z = 0; z = magicSpecialties.Length; z++)
                // {
                // switch (magicSpecialties[z])
                //{
                // case "Fire":
                // break;
                // }

                // }
                smoothPrinting.RapidPrint($"{character.name} - Mage Status\n");
                smoothPrinting.RapidPrint($"{mob.name} - Enemy");

                Console.WriteLine(); // Spacing
                UI.DisplayProgressBar($"{character.name}'s Mana", currentMana, maxMana, 30);
                Console.WriteLine(); // Spacing
                UI.DisplayProgressBar($"{mob.name}'s Health", mob.currentMobHealth, mob.maxMobHealth, 30); // Display mob health
                Console.WriteLine(); // Spacing

                // Combat methods for the Mage class
                foreach (var spell in ((Mage)character).magicSpells) // Display all spells currently avaliable to the Mage
                {
                    smoothPrinting.RapidPrint($"\n{spellCount}. Spell: {spell.magicSpell} - Damage: {spell.damage}\nMana Requirement: {spell.manaRequirement}\n");
                    spellCount++;
                }

                // Register user input
                smoothPrinting.RapidPrint("\nSelect a spell to attack (Enter '0' to return back): ");
                userInput = Console.ReadLine();

                int registeredInput = Int32.Parse(userInput); // Convert value to integer

                // Check if the user input corresponds to a spell index
                if (registeredInput > 0 && registeredInput <= ((Mage)character).magicSpells.Count())
                {
                    // Get the chosen spell based on the user's input
                    var chosenSpell = ((Mage)character).magicSpells[registeredInput - 1];

                    // Add the chosen spell to the list of spells for attack
                    chosenSpellForAttack.Add((chosenSpell.magicSpell, chosenSpell.damage, chosenSpell.manaRequirement));
                }
                else if (registeredInput == 0)
                {
                    // User wants to return back, handle accordingly
                    smoothPrinting.RapidPrint("\nYou will be redirected back to the Mage Combat System (MCS)");
                    UI.PromptUserToContinue();
                    DisplayMageStatus(character, mob, quickDisplay = true);
                }
                else
                {
                    // Invalid input, handle accordingly
                    smoothPrinting.RapidPrint("\nInvalid input, please try again.");
                    UI.PromptUserToContinue();
                    MageSpellAttack(character, mob, userTurn, enemyTurn, quickDisplay);
                }

                foreach (var spell in chosenSpellForAttack)
                {
                    if (character.currentMana >= spell.manaRequirement)
                    {
                        // Check for this condition first
                        if (mob.currentMobHealth <= spell.damage) // Check if the spell damage is more than the enemies health (in that case, set the enemies health to zero, to avoid game crash)
                        {
                            foreach (var damageBoost in weapon)
                            {
                                // int totalSpellDamage = (int)(spell.damage + damageBoost.damage * 0.3); // Retrieve the total spell damage as an integer value
                                smoothPrinting.RapidPrint($"\n{character.name} has casted {spell.magicSpell}, dealing {(int)(spell.damage + (damageBoost.damage * 0.3))} damage to {mob.name}.");
                                mob.currentMobHealth = 0; // Set the enemies health to zero, to prevent game from crashing
                                character.currentMana -= spell.manaRequirement; // Linearly reduce the mage's mana based on the mana requirement of the spell
                                Console.ReadKey();
                                Console.Clear();
                                DisplayMageStatus(character, mob, quickDisplay = true); // Return after final blow
                            }
                        }
                        // Otherwise...
                        else
                        {
                            foreach (var damageBoost in weapon) // The weapon will enable further damage with the encompassing spell used
                            {
                                smoothPrinting.RapidPrint($"\n{character.name} has casted {spell.magicSpell}, dealing {(int)(spell.damage + (damageBoost.damage * 0.3))} damage to {mob.name}.");
                                // int totalSpellDamage = (int)(spell.damage + damageBoost.damage * 0.3); // Retrieve the total spell damage as an integer value
                                mob.currentMobHealth -= (int)(spell.damage + (damageBoost.damage * 0.3)); // Ensure the mob health remains integer, as to avoid game crashing
                                character.currentMana -= spell.manaRequirement; // Linearly reduce the mage's mana based on the mana requirement of the spell
                                Console.ReadKey();
                                Console.Clear();
                                enemyTurn = true; // Turn this true as the users turn has been used
                                mob.mobAttack(mob, character, enemyTurn); // Enemies turn to attack
                            }

                        }
                    }
                    else
                    {
                        smoothPrinting.RapidPrint("\nYou do not have enough mana to cast this spell.");
                        Console.ReadKey();
                        Console.Clear();
                        MageSpellAttack(character, mob, userTurn, enemyTurn, quickDisplay = true); // Recurse back to the function, should the user wish to use an alternative spell
                    }

                }
            }

            // smoothPrinting.RapidPrint("\nThis feature is currently in development, so you'll be redirected back to the M.C.S (Mage Combat System) Menu.\n");
            // smoothPrinting.RapidPrint("\nAffirmative? If so, click any key to return back to the MCS.");

            // Console.ReadKey(); // Register user input
            // Console.Clear(); // Clear the console to avoid overlapping
            // DisplayMageStatus(mage, mob); // Return back (still in development)

            // userInput = Convert.ToString(Console.ReadLine());


            // Next steps: Create a function that will count the number of spells within the individuals 'mageSpell' list and make a switch case for it.



            // chosenSpellForAttack.Add(magicSpells.); // Append the chosen spell to another variable

            if (character is Mage)
            {
                DisplayMageStatus(character, mob, quickDisplay); // Run the following function call
            }


        }

        // Used for recovery
        // public void Meditate()
        // {
        // Console.WriteLine(name + " has meditated ");
        // mana = mana + 20;
        // health = health + 20;
        // Console.WriteLine(name + " has meditated and has recovered:\n");
        // Console.WriteLine("+20 health");
        // Console.WriteLine("+20 mana");
        // }


        public void CharacterDeath(CharacterDefault character, MobDefault mob) // If the user dies, then this function will run
        {
            string? userInput;

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine($"FantasyRPG: You have died by {mob.name}");
            smoothPrinting.PrintLine("--------------------------------------------------");

            smoothPrinting.SlowPrint("\nYou have died...\n");
            smoothPrinting.SlowPrint("\nWould you like to go back to the Menu? ('1' for Yes, any other key for No): ");
            userInput = Console.ReadLine();

            switch (userInput)
            {
                case "1":
                    smoothPrinting.RapidPrint("\nYou will now be redirected back to the Menu...");
                    userInput = null; // This is case measure to prevent the userInput value from already having a stored value, if the user reaches this point in the game again
                    Console.Clear(); // Clear the console
                    Console.ResetColor(); // Reset console color
                    GameMenu redirectUserToMenu = new GameMenu();
                    redirectUserToMenu.gameMenu(); // Redirect user to the game menu, if they enter the value '1'
                    break;
                default:
                    smoothPrinting.RapidPrint("\nConsole will now terminate, press any key to leave the game.");
                    Console.ReadKey();
                    Environment.Exit(1); // Exit the game forcefully, after user input
                    break;
            }
        }


        // Levelling methods 
        public void CalculateExperienceForNextLevel(CharacterDefault character)
        {
            UIManager UI = new UIManager(); // Engage the UIManager for progress bars
            Console.WriteLine(); // Spacing to stop overlapping
            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine($"FantasyRPG: {character.name}'s Status Check - Required EXP for next Level");
            smoothPrinting.PrintLine("--------------------------------------------------");


            // Depending on level, requirement for level is adjusted, when user reaches level 10 and above, exp requirements are increased
            if (level < 10)
            {
                experienceRequiredForNextLevel = 10 * level;
                UI.DisplayProgressBar($"Experience required for Level {character.level + 1}", exp, experienceRequiredForNextLevel, 30);

                if (character.exp > character.experienceRequiredForNextLevel) // Should the individual have more exp than the requirement, then they'll level up accordingly, this could happen if the user defeats a strong opponent
                {
                    LevelUp(character);
                }

                Console.WriteLine(); // Spacing
            }
            else if (level >= 10)
            {
                experienceRequiredForNextLevel = 100 * level;
                UI.DisplayProgressBar($"Experience required for Level {character.level + 1}", exp, experienceRequiredForNextLevel, 30);
            }
            Console.WriteLine(); // Spacing to avoid overlaps
            smoothPrinting.RapidPrint("\nAffirmative? If so, click any key to return back to the dashboard.");
            Console.ReadKey(); // Register user input
            Console.Clear(); // Clear the console to avoid overlapping
            gameDashboard dash = new gameDashboard();
            dash.dashboard((Mage)character); // Return to the user dashboard





        }

        // Should the condition be met
        public void LevelUp(CharacterDefault character)
        {
            if (character is Mage)
            {
                Console.WriteLine(); // Spacing
                Console.WriteLine(); // Double spacing to avoid overlapping
                smoothPrinting.PrintLine("--------------------------------------------------");
                smoothPrinting.PrintLine($"FantasyRPG: Mage Level Up!");
                smoothPrinting.PrintLine("--------------------------------------------------");
                character.level++; // Increment the level
                character.exp -= character.experienceRequiredForNextLevel; // Decrement to avoid overdistrubution
                character.maxHealth += 5; // For every level, increase the users maximum health by 5 points
                character.maxMana += 5; // For every level, increase the users maximum mana by 5 points
                character.currentHealth += character.maxHealth; // Replenish the users health
                character.currentMana += character.maxMana; // Replenish the users mana
                smoothPrinting.RapidPrint($"\n{character.name} has levelled up, you are now level {character.level}!");
                smoothPrinting.RapidPrint($"\nYour maximum HP has increased by +5");
                smoothPrinting.RapidPrint($"\nYour maximum mana has increased by +5");
                smoothPrinting.RapidPrint($"\nYour current health and mana have been replenished!");

                if (((Mage)character).level == 10)
                {
                    ((Mage)character).chooseNewSpeciality(character);
                }


                CalculateExperienceForNextLevel((Mage)character);

            }
            else if (character is SomaliPirate)
            {
                Console.WriteLine(); // Spacing
                smoothPrinting.PrintLine("--------------------------------------------------");
                smoothPrinting.PrintLine($"FantasyRPG: Pirate Level Up!");
                smoothPrinting.PrintLine("--------------------------------------------------");
                character.level++; // Increment the level
                character.maxHealth += 5; // For every level, increase the users maximum health by 5 points
                character.maxMana += 5; // For every level, increase the users maximum mana by 5 points
                character.currentHealth += character.maxHealth; // Replenish the users health
                character.currentMana += character.maxMana; // Replenish the users mana
                smoothPrinting.RapidPrint($"\n{character.name} has levelled up, you are now level {character.level}!");
                smoothPrinting.RapidPrint($"\nYour maximum HP has increased by +5");
                smoothPrinting.RapidPrint($"\nYour maximum mana has increased by +5");
                smoothPrinting.RapidPrint($"\nYour current health and mana have been replenished!");
                CalculateExperienceForNextLevel((SomaliPirate)character);
            }

        }

        // Check if user has enough to level up
        public void GainExperience(CharacterDefault character, float experiencePoints)
        {
            character.exp += experiencePoints;


            // Check if the character should level up
            if (character.exp >= character.experienceRequiredForNextLevel)
            {
                LevelUp((Mage)character);
            }

        }

    }

    public class MobSpawner
    {
        private readonly CharacterDefault playerCharacter;
        private readonly MobType mobType;
        private readonly SmoothConsole smoothPrinting;

        public MobSpawner(CharacterDefault character, MobType type)
        {
            playerCharacter = character ?? throw new ArgumentNullException(nameof(character));
            mobType = type;
            smoothPrinting = new SmoothConsole();
        }

        public void MobSpawn(CharacterDefault character, MobDefault mob)
        {
            if (mob == null)
            {
                throw new ArgumentNullException(nameof(mob));
            }

            bool quickDisplay = false;

            switch (mobType)
            {
                case MobType.Dragon:
                    Dragon dragon = new Dragon(mob.name, mob.currentMobHealth, mob.maxMobHealth, mob.normalAtkNames, mob.specialAtkNames, mob.specialAtkRecharge, mob.itemDrop, mob.expDrop, mob.dropChance, mob.mobLevel);
                    dragon.exertPressure(character, mob);
                    playerCharacter.CombatSystem(playerCharacter, dragon, quickDisplay);
                    break;

                case MobType.Crawler:
                    // Handle Crawler instantiation
                    break;

                case MobType.Wolf:
                    Wolf wolf = new Wolf("Wolf", mob.currentMobHealth, mob.maxMobHealth, mob.normalAtkNames, mob.specialAtkNames, mob.specialAtkRecharge, mob.itemDrop, mob.expDrop, mob.dropChance, mob.mobLevel);
                    playerCharacter.CombatSystem(playerCharacter, wolf, quickDisplay);
                    break;
                case MobType.Boar:
                    Boar boar = new Boar("Wolf", mob.currentMobHealth, mob.maxMobHealth, mob.normalAtkNames, mob.specialAtkNames, mob.specialAtkRecharge, mob.itemDrop, mob.expDrop, mob.dropChance, mob.mobLevel);
                    playerCharacter.CombatSystem(playerCharacter, boar, quickDisplay);
                    break;
                case MobType.OversizedBoar:
                    Boar oversizedBoar = new Boar("Oversized Boar", 75, 75, mob.normalAtkNames, mob.specialAtkNames, mob.specialAtkRecharge, mob.itemDrop, mob.expDrop, mob.dropChance, mob.mobLevel);
                    playerCharacter.CombatSystem(playerCharacter, oversizedBoar, quickDisplay);
                    break;
                default:
                    smoothPrinting.RapidPrint("\nUnknown mob type, please check the code.");
                    break;
            }
        }
    }


    public enum MobType // Current mob types in the game
    {
        Dragon,
        Wolf,
        Boar,
        OversizedBoar,
        Crawler
    }

    public class MobDefault // Mob preset for the game
    {
        public string name;
        public int specialAtkRecharge, currentMobHealth, maxMobHealth, expDrop, dropChance, mobLevel;
        private readonly UIManager UI; // Progress bars and repeatable functions
        private readonly SmoothConsole smoothPrinting; // Cleaner and neater output

        // Mobs can have different attack names and varying item drops, each associated with a rarity and damage value
        public Dictionary<string, (int damage, string rarity, string weaponDescription, string weaponType, string category, int quantity)> itemDrop { get; } // First string defines the weapon name, second integer defines the weapon damage, thirs stirng defines the weapon rarity and fourth string defines the weapon type
        public Dictionary<string, (int damage, string magicType)> normalAtkNames { get; }
        public Dictionary<string, (int damage, string magicType)> specialAtkNames { get; }

        public MobDefault(string _name, Dictionary<string, (int damage, string magicType)> _normalAtkNames, Dictionary<string, (int damage, string magicType)> _specialAtkNames, int _specialAtkRecharge, int _currentMobHealth, int _maxMobHealth, Dictionary<string, (int damage, string rarity, string weaponDescription, string weaponType, string category, int quantity)> _itemDrop, int _expDrop, int _dropChance, int _mobLevel) // Presets for all mobs within the game (i.e. dragons, shadow stalkers, arcane phantons, crawlers etc.)
        {
            name = _name;
            normalAtkNames = _normalAtkNames;
            specialAtkNames = _specialAtkNames;
            itemDrop = _itemDrop; // Mobs have a chance to drop a random item once they die
            specialAtkRecharge = 0;
            currentMobHealth = _currentMobHealth;
            maxMobHealth = _maxMobHealth;
            mobLevel = _mobLevel;
            expDrop = _expDrop;
            dropChance = _dropChance;
            UI = new UIManager();
            smoothPrinting = new SmoothConsole();
        }


        // Method for spawning a specific mob based on the context (i.e. if a dragon is encountered, then spawn a dragon)
        // public void MobSpawn(CharacterDefault character)
        // {
        // bool quickDisplay = false; // This is for combat convenience, if this is true, then all options are displayed quickly

        // Spawn the specified mob type based on the value of the mob variable
        // switch (mob)
        // {
        // case Dragon _:
        // Spawn a dragon
        // Dragon dragon = new Dragon(mob.name, mob.currentMobHealth, mob.maxMobHealth, mob.normalAtkNames, mob.specialAtkNames, mob.maxMobHealth, mob.itemDrop, mob.expDrop, mob.dropChance, mob.mobLevel);
        // character.CombatSystem(character, mob, quickDisplay);
        // break;
        // case Crawler _:
        // Crawler crawler = new Crawler(mob.name, mob.normalAtkNames, mob.specialAtkNames, mob.specialAtkRecharge, mob.currentMobHealth, mob.maxMobHealth, mob.itemDrop, mob.expDrop, mob.dropChance, mob.mobLevel);
        // Spawn another type of mob (replace with your mob class and parameters)
        // break;
        // Add other cases for different mob types if needed
        // default:
        // Handle unexpected mob type
        // break;
        // }

        // Now you can use the spawned mob object
        // }

        // Infinite dungeon method
        public void InfiniteDungeon()
        {
            // Insert logic for the dungeon here
            // It's quite similar to mob spawning, only difference is that the switch case takes in a random value
        }

        public void displayMobStatus(MobDefault mob)
        {
            // Add parameters such as the mobs health etc.
            UI.DisplayProgressBar("Mob Health", mob.currentMobHealth, mob.maxMobHealth, 30);
            Console.WriteLine();
            Console.WriteLine(); // Double spacing to avoid overlapping
        }

        public void mobAttackHeader(MobDefault mob) // For when the mob's turn is active
        {
            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine($"FantasyRPG: {mob.name}'s Attack");
            smoothPrinting.PrintLine("--------------------------------------------------");
        }

        // Mob attack
        public void mobAttack(MobDefault mob, CharacterDefault character, bool enemyTurn)
        {
            bool quickDisplay = true;
            mob.normalAtkNames.ToList();
            mob.specialAtkNames.ToList();

            mobAttackHeader(mob); // Display the header, to let the user know that it's the mob's turn to attack

            // Display users status
            Console.WriteLine($"{character.name} - Mage Status");
            Console.WriteLine($"{mob.name} - Enemy");

            UI.DisplayProgressBar($"{character.name}'s Health", character.currentHealth, character.maxHealth, 30); // Display Mage's health
            Console.WriteLine(); // Spacing

            displayMobStatus(mob); // Display the mob's status as well
            if (enemyTurn)
            {
                if (mob.specialAtkRecharge != 100)
                {
                    Random ran = new Random();
                    int randomNormalAttack = ran.Next(0, normalAtkNames.Count()); // Dynamic selection for the mob attacks

                    foreach (var chosenNormalAtk in mob.normalAtkNames)
                    {
                        if (chosenNormalAtk.Value.damage > character.currentHealth) // Check if the mob's attack does more damage than the users current health
                        {
                            // Allow mob to use their special attack
                            smoothPrinting.RapidPrint($"{mob.name} has used {chosenNormalAtk.Key} dealing {chosenNormalAtk.Value.damage} damage.");
                            character.currentHealth = 0; // Set the users health to zero to prevent game from crashing
                            mob.specialAtkRecharge += 20; // Mob's special charge increases by 20% per attack
                            enemyTurn = false; // Enemy turn has been used, so reset this case
                            Console.WriteLine(); // Spacing
                            UI.PromptUserToContinue(); // Prompt the user to continue
                            character.CombatSystem(character, mob, quickDisplay); // Return to the combat system, after the damage has been dealt by the enemy
                        }
                        else if (chosenNormalAtk.Value.damage < character.currentHealth) // Otherwise...
                        {
                            // Allow mob to use their special attack
                            smoothPrinting.RapidPrint($"{mob.name} has used {chosenNormalAtk.Key} dealing {chosenNormalAtk.Value.damage} damage.");
                            character.currentHealth -= chosenNormalAtk.Value.damage; // Linearly reduce the users health based on mob attack damage
                            mob.specialAtkRecharge += 20; // Mob's special charge increases by 20% per attack
                            enemyTurn = false; // Enemy turn has been used, so reset this case
                            Console.WriteLine(); // Spacing
                            UI.PromptUserToContinue(); // Prompt the user to continue
                            character.CombatSystem(character, mob, quickDisplay); // Return to the combat system, after the damage has been dealt by the enemy

                        }

                    }
                }
                else if (mob.specialAtkRecharge == 100)
                {
                    Random ran = new Random();
                    int randomSpecialAttack = ran.Next(0, specialAtkNames.Count()); // Dynamic selection for the mob attacks

                    specialAtkNames.ToList(); // Convert the attacks to a list

                    foreach (var chosenSpecialAtk in mob.specialAtkNames)
                    {
                        if (chosenSpecialAtk.Value.damage > character.currentHealth) // Check if the mob's attack does more damage than the users current health
                        {
                            UI.DisplayProgressBar("ULT Charge:", mob.specialAtkRecharge, 100, 30);
                            smoothPrinting.RapidPrint("\nSpecial Attack");

                            smoothPrinting.RapidPrint($"\n{mob.name} has used {chosenSpecialAtk.Key} deaing {chosenSpecialAtk.Value.damage} damage.");
                            character.currentHealth = 0; // Set the users health to zero, to avoid game crashing
                            specialAtkRecharge = 0; // Reset the special attack recharge counter, once used
                            enemyTurn = false; // Enemy turn has been used, so reset this case
                            Console.WriteLine(); // Spacing
                            UI.PromptUserToContinue(); // Prompt the user to continue
                            character.CombatSystem(character, mob, quickDisplay); // Return to the combat system, after the damage has been dealt by the enemy
                        }
                        else if (chosenSpecialAtk.Value.damage < character.currentHealth) // Otherwise...
                        {
                            smoothPrinting.RapidPrint($"\n{mob.name} has used {chosenSpecialAtk.Key} deaing {chosenSpecialAtk.Value.damage} damage.");
                            character.currentHealth -= chosenSpecialAtk.Value.damage; // Linearly reduce users health based on the damage done
                            specialAtkRecharge = 0; // Reset the special attack recharge counter, once used
                            enemyTurn = false; // Enemy turn has been used, so reset this case
                            Console.WriteLine(); // Spacing
                            UI.PromptUserToContinue(); // Prompt the user to continue
                            character.CombatSystem(character, mob, quickDisplay); // Return to the combat system, after the damage has been dealt by the enemy
                        }
                    }
                }
                else
                {
                    smoothPrinting.RapidPrint("\nError with mob, please check the code and ensure that there's a fix made immediately!");
                    Console.ReadKey();
                }

            }


        }

        public void mobDeath(MobDefault mob, CharacterDefault character) // Should a mob die, the user will be displayed with the following information
        {
            if (mob.currentMobHealth == 0)
            {
                Console.Clear();

                smoothPrinting.PrintLine("--------------------------------------------------");
                smoothPrinting.PrintLine($"FantasyRPG: Defeated {mob.name}");
                smoothPrinting.PrintLine("--------------------------------------------------");

                smoothPrinting.RapidPrint($"\n{mob.name} has been defeated by {character.name}\n");

                smoothPrinting.RapidPrint("\nFinal battle stats\n");
                UI.DisplayProgressBar("Health", character.currentHealth, character.maxHealth, 30); // Display Mage's health
                Console.WriteLine(); // Spacing

                UI.DisplayProgressBar("Mana", character.currentMana, character.maxMana, 30); // Display Mage's remaining mana
                Console.WriteLine(); // Spacing

                UI.DisplayProgressBar("Enemy Health:", mob.currentMobHealth, mob.maxMobHealth, 30); // Display enemies health
                Console.WriteLine(); // Spacing

                smoothPrinting.RapidPrint($"\nRewards incoming...");



                Random itemDropChance = new Random(); // Each mob class should have a dynamic integer for the item drop chance, this way it isn't the same drop rate for all mobs
                int dropChance = itemDropChance.Next(0, mob.dropChance); // Drop chance ranges depending on the mob

                if (dropChance == 0 || dropChance == 1) // Random chance of item drop
                {
                    Console.WriteLine(); // Spacing
                    Console.WriteLine(); // Double spacing to stop overlapping
                    smoothPrinting.PrintLine("--------------------------------------------------");
                    smoothPrinting.PrintLine($"FantasyRPG: You received a drop!");
                    smoothPrinting.PrintLine("--------------------------------------------------");
                    mob.dropItem(dropChance, character, mob, itemDrop); // Should the random number be zero, then the mob will drop an item
                }

                smoothPrinting.RapidPrint($"\nYou received {mob.expDrop} EXP!"); // Display the received EXP to the user
                character.exp += mob.expDrop; // Update the accumulated EXP for the user
                character.GainExperience(character, character.exp); // Check the user level and level up when needed
                character.inCombat = false; // Break the condition, as the combat has now reached its conclusion


            }

        }

        // If the user gets lucky, then they can get a mob drop, which can vary as it is RNG
        public void dropItem(int dropChance, CharacterDefault character, MobDefault mob, Dictionary<string, (int damage, string rarity, string weaponType, string weaponDescription, string category, int quantity)> itemDrop)
        {
            Random ran = new Random(); // Determine which item will be dropped
            int randomWeapon = ran.Next(0, itemDrop.Count()); // Generate a value between the zero index to the limit of the dictionary
            string userChoice;

            itemDrop.ToList(); // Convert the item drops to a list
            var drop = itemDrop.ElementAtOrDefault(randomWeapon); // Select the weapon based on random index




            if (!string.IsNullOrEmpty(drop.Key))
            {

                smoothPrinting.RapidPrint($"\n{mob.name} Drop: {character.name} has received...\n\nItem Name: {drop.Key}\nDamage: {drop.Value.damage}\nRarity: {drop.Value.rarity}\nItem Description: {drop.Value.weaponDescription}\nWeapon Type: {drop.Value.weaponType}");
                Console.WriteLine(); // Spacing

                foreach (var currentWeaponStats in character.weapon)
                {
                    // Displays the users current equipped weapon 
                    smoothPrinting.RapidPrint($"\n\nCurrent Weapon Equipped:\nWeapon Name: {currentWeaponStats.weaponName}\nWeapon Damage: {currentWeaponStats.damage}\nWeapon Rarity: {currentWeaponStats.rarity}\nWeapon Description: {currentWeaponStats.weaponDescription}\nWeapon Type: {currentWeaponStats.weaponType}\n");

                    // Displaying the stat difference between the weapons/items
                    smoothPrinting.RapidPrint($"\nStat Comparision:\nDamage Difference: {drop.Value.damage - currentWeaponStats.damage}\nRarity Differnce: (Current Weapon) {currentWeaponStats.rarity} -> (Dropped Weapon) {drop.Value.rarity}\nWeapon Type Difference: (Current Weapon) {currentWeaponStats.weaponType} -> (Dropped Weapon) {drop.Value.weaponType}\n");

                }

                smoothPrinting.RapidPrint($"\nWould you like to equip {drop.Key}? (1 for 'Yes' and any other key to store the item in your inventory)\n");

                smoothPrinting.RapidPrint("\nEnter a corresponding value: ");
                userChoice = Console.ReadLine(); // Register user input

                if (userChoice == "1")
                {
                    character.weapon.Clear(); // Remove the current weapon equipped by the user
                    character.weapon.Add((drop.Key, drop.Value.damage, drop.Value.rarity, drop.Value.weaponType, drop.Value.weaponDescription, drop.Value.category, drop.Value.quantity));
                }
                else
                {
                    smoothPrinting.RapidPrint("\nWeapon will be stored to inventory.");
                }

                character.currentInventory.Add((drop.Key, drop.Value.weaponDescription, drop.Value.rarity, drop.Value.damage, drop.Value.category, drop.Value.quantity)); // Add the item drop to the player's inventory
            }
            else
            {
                // Debugging measure (try, except)
                smoothPrinting.RapidPrint("No weapon selected.");
            }
        }



    }


    class Crawler : MobDefault // Crawler class
    {
        // Dictionary containing crawler attacks and their associated damage value
        Dictionary<string, int> normalAtkNames = new Dictionary<string, int>() // Preset name for all dragon's normal attacks
            {
                { "Crawler's Scratch", 5 },
                { "Crawler's Screech", 10 },
                { "Crawler's Bite", 3 }
            };

        // Dictionary that contains weapon name, damage, rarity and weapon type (item drops)
        Dictionary<string, (int damage, string rarity, string weaponType, string weaponDescription)> itemDrop = new Dictionary<string, (int, string, string, string)>()
            {
                { "Staff of Spite", (9, "Common", "Staff", "The emotion of the Crawler once it perishes, nothing but spite, wishing that it could finish you should it have been alive.") },
                { "Crawler's Revant", (15, "Uncommon", "Rapier/Sword", "A fairly rusty rapier that looks like it hasn't been used in a long time, perhaps could make good scraps for a better sword.") },
            };


        public Crawler(string _name, Dictionary<string, (int damage, string magicType)> _normalAtkNames, Dictionary<string, (int, string)> _specialAtkNames, int _specialAtkRecharge, int _currentMobHealth, int _maxMobHealth, Dictionary<string, (int damage, string rarity, string weaponDescription, string weaponType, string category, int quantity)> _itemDrop, int _expDrop, int _dropChance, int _mobLevel) : base(_name, _normalAtkNames, _specialAtkNames, _specialAtkRecharge, _currentMobHealth, _maxMobHealth, _itemDrop, _expDrop, _dropChance, _mobLevel)
        {
            // Default presets for a crawler, inherited from the mob default class
            name = "Crawler";
            currentMobHealth = 35; // Crawlers have '35' HP by default, as they are small and weak
            maxMobHealth = 20;
            expDrop = 30; // Each Crawler drops '30' EXP
            dropChance = 3; // Drop chance for items isn't that low, as it is a weak creature
            mobLevel = 5;

        }


        // public void crawlerNormalAtk(int health)
        // {
        //  Random rd = new Random();
        // List<string> attackNames = normalAtkNames.Keys.ToList(); // Get all attack names

        // int randomIndex = rd.Next(0, attackNames.Count); // Generate a random index

        // string randomAttackName = attackNames[randomIndex]; // Get a random attack name
        // int damage = normalAtkNames[randomAttackName]; // Get the damage associated with the attack

        // smoothPrinting.FastPrint("Crawler has used " + randomAttackName + " dealing " + damage + " damage.\n");
        // }


        // public void crawlerDeath(CharacterDefault character, MobDefault mob) // If the crawler dies, then the user gains exp and has a chance of receiving an item drop
        // {
        // if (mobHealth == 0)
        // {
        // Random itemDropChance = new Random();
        // int dropChance = itemDropChance.Next(1, 2); // 50% drop rate, as the mob is easy to defeat
        // smoothPrinting.FastPrint("\nDragon has been successfully defeated!");

        // if (dropChance == 0)
        // {
        // mob.dropItem(dropChance, character, mob, itemDrop); // Should the random number be zero, then the mob will drop an item
        // }

        // character.exp += 5; // User gets experience from the drop
        // smoothPrinting.SlowPrint("User has gained " + character.exp + " experience points!");

        // }
        // }

    }

    public class Wolf : MobDefault
    {
        private readonly UIManager UI;
        private readonly SmoothConsole smoothPrinting;


        private readonly Dictionary<string, (int damage, string magicType)> normalAtkNames = new Dictionary<string, (int damage, string magicType)>()
        {
            {"Scratch", (5, "Normal")},
            {"Bite", (8, "Normal")},
            {"Hounding Tempest", (10, "Wind-Magic")}
        };

        private readonly Dictionary<string, (int damage, string magicType)> specialAtkNames = new Dictionary<string, (int damage, string magicType)>()
        {
            { "Pack Ambush", (25, "Normal")},
            { "Howl", (20, "Sound-Magic")},
            { "Bloody Rage", (15, "Fire-Magic") }
        };


        public Wolf(string _name, int _currentMobHealth, int _maxMobHealth, Dictionary<string, (int damage, string magicType)> _normalAtkNames, Dictionary<string, (int damage, string magicType)> _specialAtkNames, int _specialAtkRecharge, Dictionary<string, (int damage, string rarity, string weaponDescription, string weaponType, string category, int quantity)> _itemDrop, int _expDrop, int _dropChance, int _mobLevel) : base(_name, _normalAtkNames, _specialAtkNames, _specialAtkRecharge, _currentMobHealth, _maxMobHealth, _itemDrop, _expDrop, _dropChance, _mobLevel)
        {
            UI = new UIManager();
            smoothPrinting = new SmoothConsole();
            normalAtkNames = _normalAtkNames;
            specialAtkNames = _specialAtkNames;
        }

    }

    public class Boar : MobDefault // Boar mob
    {
        private readonly UIManager UI;
        private readonly SmoothConsole smoothPrinting;

        private readonly Dictionary<string, (int damage, string magicType)> normalAtkNames = new Dictionary<string, (int damage, string magicType)>()
        {
            {"Tusk Swipe", (15, "Physical")},
            {"Feral Charge", (8, "Physical")},
            {"Mud Slam", (10, "Earth-Magic")}
        };

        private readonly Dictionary<string, (int damage, string magicType)> specialAtkNames = new Dictionary<string, (int damage, string magicType)>()
        {
            { "Rampaging Roar", (20, "Sound-Magic")},
            { "Earthquake Stomp", (25, "Earth-Magic")},
            { "Inferno Charge", (30, "Fire-Magic") }
        };

        public Boar(string _name, int _currentMobHealth, int _maxMobHealth, Dictionary<string, (int damage, string magicType)> _normalAtkNames, Dictionary<string, (int damage, string magicType)> _specialAtkNames, int _specialAtkRecharge, Dictionary<string, (int damage, string rarity, string weaponDescription, string weaponType, string category, int quantity)> _itemDrop, int _expDrop, int _dropChance, int _mobLevel) : base(_name, _normalAtkNames, _specialAtkNames, _specialAtkRecharge, _currentMobHealth, _maxMobHealth, _itemDrop, _expDrop, _dropChance, _mobLevel)
        {
            UI = new UIManager();
            smoothPrinting = new SmoothConsole();
        }

    }

    public class Dragon : MobDefault
    {
        private readonly UIManager UI;
        private readonly SmoothConsole smoothPrinting;

        private readonly Dictionary<string, (int damage, string magicType)> normalAtkNames = new Dictionary<string, (int damage, string magicType)>()
        {
            {"Dragon's Claw", (30, "Dragon-Magic")},
            {"Dragon's Breath", (40, "Dragon-Magic")},
            {"Raging Tempest", (50, "Dragon-Magic")}
        };

        private readonly Dictionary<string, (int damage, string magicType)> specialAtkNames = new Dictionary<string, (int damage, string magicType)>()
        {
            { "Arcane Nexus", (100, "Eucladian-Magic")},
            { "Umbral Charge", (120, "Dark-Magic")},
            { "Rampant Flame Charge", (200, "Fire-Magic") }
        };

        private readonly Dictionary<string, (int damage, string rarity, string weaponDescription, string weaponType, string category, int quantity)> itemDrop = new Dictionary<string, (int damage, string rarity, string weaponDescription, string weaponType, string category, int quantity)>()
        {
            { "Frostfire Fang", (65, "Unique", "Forged in the icy flames of the dragon's breath, this fang drips with frostfire, capable of freezing enemies in their tracks.", "Staff", "Staff", 1) },
            { "Serpent's Gaze", (50, "Unique", "Crafted from the scales of the ancient serpent, this gaze holds the power to petrify foes with a single glance.", "Rapier/Sword", "Rapier", 1) },
            { "Chaosfire Greatsword", (60, "Unique", "Tempered in the chaosfire of the dragon's lair, this greatsword burns with an insatiable hunger for destruction.", "Greatsword/Sword", "Greatsword", 1) },
            { "Nightshade Arc", (55, "Unique", "Fashioned from the sinew of the nocturnal shadows, this bow strikes with deadly accuracy under the cover of darkness.", "Bow", "Bow", 1) },
            { "Aerith's Heirloom", (80, "Legendary", "Once wielded by the legendary Aerith, this staff channels the primordial magic of creation itself, capable of reshaping reality.", "Staff", "Staff", 1) },
            { "Eucladian's Aura", (55, "Legendary", "Embrace the ethereal aura of the Eucladian, granting unmatched protection against all forms of magic and malevolence.", "Aura", "Aura", 1) }
        };

        public Dragon(string _name, int _currentMobHealth, int _maxMobHealth, Dictionary<string, (int damage, string magicType)> _normalAtkNames, Dictionary<string, (int damage, string magicType)> _specialAtkNames, int _specialAtkRecharge, Dictionary<string, (int damage, string rarity, string weaponDescription, string weaponType, string category, int quantity)> _itemDrop, int _expDrop, int _dropChance, int _mobLevel) : base(_name, _normalAtkNames, _specialAtkNames, _specialAtkRecharge, _currentMobHealth, _maxMobHealth, _itemDrop, _expDrop, _dropChance, _mobLevel)
        {
            // name = _name;
            // currentMobHealth = 350;
            // maxMobHealth = 350;
            // specialAtkRecharge = 0;
            // dropChance = 12; // Low chance for item drop
            // mobLevel = 25;
            // expDrop = 300; // Dragon drops 300 EXP by default

            UI = new UIManager();
            smoothPrinting = new SmoothConsole();
        }

        // Future reference: Create different types of dragons that have weaknesses (i.e. water dragons, shadow dragons etc)

        public void exertPressure(CharacterDefault character, MobDefault mob) // Dragons will exert 'pressure' to make humans fear them, should the users level be lower than expected
        {
            if (character.level <= 10) // Should the users level be below level 10, then the dragon will exert pressure to the individual, reducing their attack value.
            {

                smoothPrinting.PrintLine("--------------------------------------------------");
                smoothPrinting.PrintLine("FantasyRPG: Dragon Race - Exerting Pressure");
                smoothPrinting.PrintLine("--------------------------------------------------");

                Console.ForegroundColor = ConsoleColor.Red;
                smoothPrinting.RapidPrint($"{mob.name}:\n");
                smoothPrinting.RapidPrint("\n*Roars with a deafening sound, shaking the very ground beneath you.*\n");

                smoothPrinting.RapidPrint($"\nYour level is lower than expected, {mob.name} exerts immense pressure, casting a shadow of dread over you. You feel your resolve weaken as fear grips your heart.\n");

                smoothPrinting.RapidPrint($"\nThe ancient power emanating from {mob.name} fills the air, suffocating your magical abilities. You sense a drain on your strength, your magical potency diminishing.\n");

                smoothPrinting.RapidPrint("\nYour attack damage is reduced as the overwhelming presence of Windsom weighs heavily upon you.\n");

                smoothPrinting.RapidPrint("\nDue to your weakened state, your weapon does less damage.");

                // Future reference: Add a part where the individuals weapon does less damage

                UI.PromptUserToContinue();
                Console.ResetColor(); // Reset Console Colour
            }



        }

        // public void dragonNormalAtk(int health)
        // {
        // Random rd = new Random();
        // List<string> attackNames = normalAtkNames.Keys.ToList(); // Get all attack names

        // int randomIndex = rd.Next(0, attackNames.Count); // Generate a random index

        // string randomAttackName = attackNames[randomIndex]; // Get a random attack name
        // int damage = normalAtkNames[randomAttackName]; // Get the damage associated with the attack

        // smoothPrinting.FastPrint("Dragon has used " + randomAttackName + " dealing " + damage + " damage.\n");
        // health = health - damage; // Return the difference by reducing users health, based on damage inflicted
        // }


        // public void dragonSpecialAtk() // If the dragons special attack recharge reaches 100%, then this will be activated
        // {
        // Random rd = new Random();

        // List<string> attackNames = specialAtkNames.Keys.ToList(); // Get all attack names
        // int randomIndex = rd.Next(0, attackNames.Count); // Generate a random index

        // string randomAttackName = attackNames[randomIndex]; // Get a random attack name
        // int damage = normalAtkNames[randomAttackName]; // Get the damage associated with the attack

        // if (specialAtkRecharge == 100) // Should the dragon's special attack recharge reach 100%, then it'll use its special ability, dealing high levels of damage, it also increases its health
        // {
        // smoothPrinting.SlowPrint("\nDragon ULT");
        // smoothPrinting.SlowPrint("\nDragon has used " + randomAttackName + " and has dealt " + damage + "\n");
        // smoothPrinting.RapidPrint("\nDragon has recovered +20 health");
        // currentMobHealth = currentMobHealth + 20; // Slight health regen
        // specialAtkRecharge = 0; // The special attack has been used by this point, so therefore it should be set to zero.
        // }

        // }

    }


    class Knight : CharacterDefault // Knight class properties and methods
    {
        public string normalAtkName;
        public string specialAtkName; // Remove these static features
        public int specialAtkDmg;
        public int normalAtkDmg;
        public bool inCombat = false;

        public Knight(string _name, List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription, string category, int quantity)> _weapon, string _specialAtkName, List<(string itemName, string itemDescription, string itemRarity, int itemPower, string category, int quantity)> _currentInventory, int _arcaniaGoldCoins, int _specialAtkRecharge, List<(string npcName, string npcDescription, string npcAffiliation)> _npcsEncountered, bool _inCombat) : base(_name, _weapon, _currentInventory, _arcaniaGoldCoins, _specialAtkRecharge, _npcsEncountered, _inCombat)
        {
            name = _name;
            weapon = _weapon;
            specialAtkName = _specialAtkName;
            currentInventory = _currentInventory;
            inCombat = _inCombat;
            normalAtkName = "Sword Slash"; // Remove these static features
            specialAtkDmg = 10; // Remove these static features
            normalAtkDmg = 4; // Remove these static features
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
        SmoothConsole smoothPrinting = new SmoothConsole();

        // Properties for common wizard attributes
        public List<(string magicSpell, int damage, int manaRequirement)> magicSpells = new List<(string magicSpell, int damage, int manaRequirement)>();
        string[] magicSpecialties; // User can have multiple magic specialties
        public bool inCombat = false;

        public Mage(string _name, List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription, string category, int quantity)> _weapon, string[] _magicSpecialties, int _arcaniaGoldCoins, List<(string magicSpell, int damage, int manaRequirement)> _magicSpells, List<(string itemName, string itemDescription, string itemRarity, int itemPower, string category, int quantity)> _currentInventory, int _specialAtkRecharge, List<(string npcName, string npcDescription, string npcAffiliation)> _npcsEncountered, bool _inCombat) : base(_name, _weapon, _currentInventory, _arcaniaGoldCoins, _specialAtkRecharge, _npcsEncountered, _inCombat)
        {
            name = _name;
            weapon = _weapon;
            inCombat = _inCombat;
            magicSpecialties = _magicSpecialties;
            currentInventory = _currentInventory;
            magicSpells = _magicSpells; // Predefined variables for every new wizard in the game
            npcsEncountered = _npcsEncountered;
        }



        // All methods for the Mage Class

        // public void SpellCast() Spell casting for enemies
        // {
        // smoothPrinting.RapidPrint($"{name} has casted:");
        // spellUsage--;
        // mana = mana - 30;
        // exp += 0.3f;
        // }


        // public override void CheckStatus()
        // {
        // base.CheckStatus(cha);
        // }


        public void MageTraining() // Might remove.
        {
            // Generate a random value for exp
            Random rd = new Random();
            int rand_num = rd.Next(1, 5);

            Console.WriteLine(name + " has decided to improve on their skills, ", " their experience has increased by " + rand_num);
            exp = exp + rand_num;

        }

        // Promotion method for the Mage class
        public void chooseNewSpeciality(CharacterDefault character) // Every 10 levels, a mage will be able to pick another speciality (only 1)
        {
            // For every 10 levels, a mage can pick a new speciality
            SmoothConsole smoothPrinting = new SmoothConsole(); // Aesthetic output
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Clear(); // Cleaning purposes


            int choiceIncrementer = 1; // Used to increment the user choice when picking magic types
            List<string> updatedMagicChoices = new List<string>(); // Will display magic types that the user isn't familiar with
            List<string> chosenMagicSpecialityByUser = new List<string>(); // This will contain the chosen magic speciality that the user has selected


            // Arrays containing the variety of different magic choices, spells and weapons.
            string[] magicChoices = { "Fire-Magic", "Water-Magic", "Lightning-Magic", "Ice-Magic", "Dark-Magic", "Light-Magic", "Eucladian-Magic" }; // Future reference: add 'level' as an argument to make other magic specialities exclusive

            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine("FantasyRPG: Mage's Prestiege");
            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.RapidPrint($"\n {character.name}'s current known magic specialities: \n");

            for (int j = 0; j < magicSpecialties.Length; j++) // Display the user's current magic specialties
            {
                smoothPrinting.FastPrint("* " + magicSpecialties[j] + "\n");
            }


            // Updating the magic choices by appending the magic elements that the user doesn't possess in a list
            foreach (var choice in magicChoices)
            {
                if (!magicSpecialties.Contains(choice))
                {
                    updatedMagicChoices.Add(choice);
                }
            }

            smoothPrinting.FastPrint("\nChoose a new speciality from the list: \n");


            // Display generic magic choices to the user (i.e. fire, water, lightning, ice)
            for (int j = 0; j < updatedMagicChoices.Count; j++)
            {
                smoothPrinting.FastPrint(choiceIncrementer + ". " + updatedMagicChoices[j] + "\n");
                choiceIncrementer++;
            }


            smoothPrinting.FastPrint("\nChoose a magic specialty by entering the corresponding number:");
            string userInput = Convert.ToString(Console.ReadLine());


            int chosenSpecialtyIndex;

            if (int.TryParse(userInput, out chosenSpecialtyIndex) && chosenSpecialtyIndex >= 1 && chosenSpecialtyIndex <= updatedMagicChoices.Count)
            {
                // Adjusting the index to match the list indexing (which starts from 0)
                chosenSpecialtyIndex--; // Decrease by 1 to match the zero-based indexing

                chosenMagicSpecialityByUser.Add(updatedMagicChoices[chosenSpecialtyIndex]);
                smoothPrinting.FastPrint("\n" + name + " has learnt the magic speciality: " + chosenMagicSpecialityByUser[0]);

                // Add the chosen specialty to the magicSpecialties array
                Array.Resize(ref magicSpecialties, magicSpecialties.Length + 1);
                magicSpecialties[magicSpecialties.Length - 1] = chosenMagicSpecialityByUser[0];

                smoothPrinting.FastPrint($"\nUpdated magic specialties: {string.Join(", ", magicSpecialties)}\n");
                LearnNewSpells(magicSpecialties, magicSpells.ToList(), chosenMagicSpecialityByUser, magicChoices, level); // Pass as array to learnNewSpells
            }
        }
        public void LearnNewSpells(string[] magicSpecialties, List<(string magicSpell, int damage, int manaRequirement)> magicSpells, List<string> chosenMagicSpecialtyByUser, string[] magicChoices, int level)
        {
            SmoothConsole smoothPrinting = new SmoothConsole(); // Cleaner output

            // Tuple dictionary for each Fire magic spell, which is associated with a damage value and a mana requirement 
            Dictionary<string, (int damage, int manaRequirement)> fireMagicSpells = new Dictionary<string, (int damage, int manaRequirement)>()
                    {
                        { "Infrared", (3, 15) },
                        { "Blazing Rage", (5, 20) },
                        { "Flamestrike", (7, 25) },
                        { "Pyroburst", (9, 30) },
                        { "Phoenix Fury", (12, 35) }
                    };

            // Tuple dictionary for each Water magic spell, which is associated with a damage value and a mana requirement 
            Dictionary<string, (int damage, int manaRequirement)> waterMagicSpells = new Dictionary<string, (int damage, int manaRequirement)>()
                    {
                        { "Aqua Torrent", (2, 10) },
                        { "Hydroburst", (4, 15) },
                        { "Lunar Tide", (6, 20) },
                        { "Ripple Cascade", (8, 25) }
                    };

            // Tuple dictionary for each Ice magic spell, which is associated with a damage value a mana requirement
            Dictionary<string, (int damage, int manaRequirement)> iceMagicSpells = new Dictionary<string, (int damage, int manaRequirement)>()
                    {
                        { "Frostbite", (5, 20) },
                        { "Ice Lance", (9, 30) },
                        { "Blizzard Tundra", (15, 50) },
                        { "Frozen Fury", (7, 25) }
                    };

            // Tuple dictionary for each Lightning magic spell, which is associated with a damage value and a mana requirement 
            Dictionary<string, (int damage, int manaRequirement)> lightningMagicSpells = new Dictionary<string, (int damage, int manaRequirement)>()
                    {
                        { "Thunderstrike", (4, 15) },
                        { "Striking Surge", (6, 20) },
                        { "Volt Surge", (8, 25) },
                        { "Arcane Thunder", (10, 30) }
                    };

            // Tuple dictionary for each Dark magic spell, which is associated with a damage value and a mana requirement 
            Dictionary<string, (int damage, int manaRequirement)> darkMagicSpells = new Dictionary<string, (int damage, int manaRequirement)>()
                    {
                        { "Shadow Veil", (3, 15) },
                        { "Umbral Surge", (5, 20) },
                        { "Wraith's Curse", (7, 25) },
                        { "Eclipised Oblivion", (9, 30) }
                    };

            // Tuple dictionary for each Light magic spell, which is associated with a damage value and a mana requirement 
            Dictionary<string, (int damage, int manaRequirement)> lightMagicSpells = new Dictionary<string, (int damage, int manaRequirement)>()
                    {
                        { "Luminous Beam", (3, 15) },
                        { "Solar Flare", (5, 20) },
                        { "Etherial Halo", (7, 25) },
                        { "Aurora's Illumination", (9, 30) },
                        { "Divine Judgement", (12, 35) }
                    };

            // Tuple dictionary for each Eucladian magic spell, which is associated with a damage value and a mana requirement 
            Dictionary<string, (int damage, int manaRequirement)> eucladianMagicSpells = new Dictionary<string, (int damage, int manaRequirement)>()
                    {
                        { "Esoteric Paradigm", (3, 15) },
                        { "Fractural Fissure", (5, 20) },
                        { "Quantum Flux", (7, 25) },
                        { "Etherial Nexus", (9, 30) }
                    };



            // Will be used to check the magic specialities chosen by the user before displaying the range of spells they can pick

            int totalSpellsDisplayed = 0;

            for (int z = 0; z < 1; z++)
            {
                Console.WriteLine("\n" + chosenMagicSpecialtyByUser[z] + " Spells:");

                switch (chosenMagicSpecialtyByUser[z])
                {
                    case "Fire-Magic":
                        foreach (var spell in fireMagicSpells)
                        {
                            smoothPrinting.FastPrint($"{(totalSpellsDisplayed + 1)}. {spell.Key} - Damage: {spell.Value.damage}, Mana Requirement for Activation: {spell.Value.manaRequirement}"); totalSpellsDisplayed++;
                            Console.WriteLine("\nPress Enter to see the next spell...");
                            Console.ReadLine();
                        }
                        break;

                    case "Water-Magic":
                        foreach (var spell in waterMagicSpells)
                        {
                            smoothPrinting.FastPrint($"{(totalSpellsDisplayed + 1)}. {spell.Key} - Damage: {spell.Value.damage}, Mana Requirement for Activation: {spell.Value.manaRequirement}"); totalSpellsDisplayed++;
                            Console.WriteLine("\nPress Enter to see the next spell...");
                            Console.ReadLine();
                        }
                        break;

                    case "Ice-Magic":
                        foreach (var spell in iceMagicSpells)
                        {
                            smoothPrinting.FastPrint($"{(totalSpellsDisplayed + 1)}. {spell.Key} - Damage: {spell.Value.damage}, Mana Requirement for Activation: {spell.Value.manaRequirement}"); totalSpellsDisplayed++;
                            Console.WriteLine("\nPress Enter to see the next spell....");
                            Console.ReadLine();
                        }
                        break;

                    case "Lightning-Magic":
                        foreach (var spell in lightningMagicSpells)
                        {
                            smoothPrinting.FastPrint($"{(totalSpellsDisplayed + 1)}. {spell.Key} - Damage: {spell.Value.damage}, Mana Requirement for Activation: {spell.Value.manaRequirement}"); totalSpellsDisplayed++;
                            Console.WriteLine("\nPress Enter to see the next spell...");
                            Console.ReadLine();
                        }
                        break;

                    case "Dark-Magic":
                        foreach (var spell in darkMagicSpells)
                        {
                            smoothPrinting.FastPrint($"{(totalSpellsDisplayed + 1)}. {spell.Key} - Damage: {spell.Value.damage}, Mana Requirement for Activation: {spell.Value.manaRequirement}"); totalSpellsDisplayed++;
                            Console.WriteLine("\nPress Enter to see the next spell...");
                            Console.ReadLine();
                        }
                        break;

                    case "Light-Magic":
                        foreach (var spell in lightMagicSpells)
                        {
                            smoothPrinting.FastPrint($"{(totalSpellsDisplayed + 1)}. {spell.Key} - Damage: {spell.Value.damage} , Mana Requirement for Activation:  {spell.Value.manaRequirement}"); totalSpellsDisplayed++;
                            totalSpellsDisplayed++;
                            Console.WriteLine("\nPress Enter to see the next spell...");
                            Console.ReadLine();
                        }
                        break;

                    case "Eucladian-Magic":
                        foreach (var spell in eucladianMagicSpells)
                        {
                            smoothPrinting.FastPrint($"{(totalSpellsDisplayed + 1)}. {spell.Key} - Damage: {spell.Value.damage} , Mana Requirement for Activation:  {spell.Value.manaRequirement}"); totalSpellsDisplayed++;
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
                if (z < chosenMagicSpecialtyByUser.Count - 1)
                {
                    Console.WriteLine("\nPress Enter to see the spells for the next specialty...");
                    Console.ReadLine();
                }
            }



            for (int specialityIndex = 0; specialityIndex < chosenMagicSpecialtyByUser.Count; specialityIndex++)
            {
                Console.WriteLine($"Select magic spells for {chosenMagicSpecialtyByUser[specialityIndex]} by entering the corresponding numbers. (1-4 for each element)");
                List<(string magicSpell, int damage, int manaRequirement)> currentMagicSpells = new List<(string magicSpell, int damage, int manaRequirement)>(); // Dynamic list which will be used to store the chosen magical spells of the users

                switch (chosenMagicSpecialtyByUser[specialityIndex])
                {
                    case "Fire-Magic":
                        currentMagicSpells = fireMagicSpells.Select(entry => (entry.Key, entry.Value.damage, entry.Value.manaRequirement)).ToList();
                        break;
                    case "Water-Magic":
                        currentMagicSpells = waterMagicSpells.Select(entry => (entry.Key, entry.Value.damage, entry.Value.manaRequirement)).ToList();
                        break;
                    case "Ice-Magic":
                        currentMagicSpells = iceMagicSpells.Select(entry => (entry.Key, entry.Value.damage, entry.Value.manaRequirement)).ToList();
                        break;
                    case "Lightning-Magic":
                        currentMagicSpells = lightningMagicSpells.Select(entry => (entry.Key, entry.Value.damage, entry.Value.manaRequirement)).ToList();
                        break;
                    case "Dark-Magic":
                        currentMagicSpells = darkMagicSpells.Select(entry => (entry.Key, entry.Value.damage, entry.Value.manaRequirement)).ToList();
                        break;
                    case "Light-Magic":
                        currentMagicSpells = lightMagicSpells.Select(entry => (entry.Key, entry.Value.damage, entry.Value.manaRequirement)).ToList();
                        break;
                    case "Eucladian-Magic":
                        currentMagicSpells = eucladianMagicSpells.Select(entry => (entry.Key, entry.Value.damage, entry.Value.manaRequirement)).ToList();
                        break;
                    default:
                        Console.WriteLine("Unknown magic speciality.");
                        Environment.Exit(0);
                        break;
                }

                // Allow the user to select spells for the current magic specialty
                for (int spellNumber = 0; spellNumber < 2; spellNumber++)
                {
                    Console.WriteLine($"Choose magic spell #{spellNumber + 1} for {chosenMagicSpecialtyByUser[specialityIndex]}:");
                    int magicSpellChoice;

                    string input = Console.ReadLine(); // Prompt for input inside the loop
                    while (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out magicSpellChoice) || magicSpellChoice < 1 || magicSpellChoice > currentMagicSpells.Count) // Mitigating empty or invalid input
                    {
                        Console.WriteLine("Invalid choice. Please enter a valid number corresponding to the magic specialty.");
                        input = Console.ReadLine(); // Prompt again for input
                    }
                    magicSpells.Add(currentMagicSpells[magicSpellChoice - 1]);
                }
            }

            Console.Clear(); // Neater

            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine("FantasyRPG: Mage's Prestiege");
            smoothPrinting.PrintLine("--------------------------------------------------");

            smoothPrinting.FastPrint(name + "'s" + " current known magic specialities:" + "\n");


            for (int j = 0; j < magicSpecialties.Length; j++) // Display the user's updated magic specialties
            {
                smoothPrinting.FastPrint("* " + magicSpecialties[j] + "\n");
            }

            smoothPrinting.FastPrint("\n" + name + "'s " + "current known magical spells/abilities:\n");

            foreach (var spell in magicSpells)
            {
                smoothPrinting.FastPrint($"\n* {spell.magicSpell}: Damage - {spell.damage}, Mana Requirement - {spell.manaRequirement}");
            }


            chosenMagicSpecialtyByUser = null; // Clear the array of any specialties, for the next time this is executed when the user reaches this point in the game again


            int userContinue = 0;
            Console.WriteLine(); // Spacing
            Console.WriteLine("\nAre you ready to go back? (1 for Yes)"); // Give mage user time to read their updated information
            userContinue = Convert.ToInt32(Console.ReadLine());

            switch (userContinue)
            {
                case 1:
                    Console.WriteLine("Redirecting " + name + " back to FantasyRPG...");
                    Console.ForegroundColor = ConsoleColor.Gray; // Return back to default colour
                    Console.Clear(); // Neater
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input, please try again.");
                    break;
            }

        }
    }


    class SomaliPirate : CharacterDefault
    {

        public List<(string attack, int damage, int manaRequirement, string elementType, string description)> pirateNormalAtks;
        public List<(string attack, int damage, int manaRequirement, string elementType, string description)> pirateSpecialAtks; // Normal and special attack lists, containing all relevant information
        public List<(string auraName, int damage, string rarity, string description)> weaponAura; // Weapon aura

        public SomaliPirate(string _name, List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription, string category, int quantity)> _weapon, List<(string auraName, int damage, string rarity, string description)> _weaponAura, List<(string attack, int damage, int manaRequirement, string elementType, string description)> _pirateNormalAtks, List<(string attack, int damage, int manaRequirement, string elementType, string description)> _pirateSpecialAtks, List<(string itemName, string itemDescription, string itemRarity, int itemPower, string category, int quantity)> _currentInventory, int _arcaniaGoldCoins, int _specialAtkRecharge, List<(string npcName, string npcDescription, string npcAffiliation)> _npcsEncountered, bool _inCombat) : base(_name, _weapon, _currentInventory, _arcaniaGoldCoins, _specialAtkRecharge, _npcsEncountered, _inCombat)
        {
            name = _name;
            weapon = _weapon;
            weaponAura = _weaponAura;
            inCombat = _inCombat;
            pirateNormalAtks = _pirateNormalAtks; // Presets for all new Somali Pirates in the game
            pirateSpecialAtks = _pirateSpecialAtks;
            currentInventory = _currentInventory; // This will be readjusted to a list in the future
            npcsEncountered = _npcsEncountered;
        }




        // All methods for the somaliPirate class
        public void PirateNormalAtk(List<(string attack, int damage, int manaRequirement, string elementType, string description)> pirateNormalAtks, int mobHealth)
        {
            // Console.WriteLine("The brave Somali Pirate named " + name + " has used " + weapon + " to deal " + normalAtkDmg);
        }

        public void PirateSpecialAtk(List<(string attack, int damage, int manaRequirement, string elementType, string description)> pirateSpecialAtks, int mobHealth)
        {
            // Console.WriteLine("The brave Somali Pirate named " + name + " has used " + weapon + " to deal " + specialAtkDmg);
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

    // class Archer : CharacterDefault
    // {
    // public Archer(string _name, List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription)> _weapon, string[] _currentInventory, int _arcaniaGoldCoins, int _specialAtkRecharge) : base(_name, _weapon, _currentInventory, _arcaniaGoldCoins, _specialAtkRecharge)
    // {
    // name = _name;
    // weapon = _weapon;
    // }
    // }


    // Warrior class
    // class Warrior : CharacterDefault
    // {
    // public Warrior(string _name, List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription)> _weapon, string[] _currentInventory, int _arcaniaGoldCoins, int _specialAtkRecharge) : base(_name, _weapon, _currentInventory, _arcaniaGoldCoins, _specialAtkRecharge)
    // {
    // name = _name;
    // weapon = _weapon;
    // currentInventory = _currentInventory;
    // }
    // }


    class userMenu // Future reference: Authentication system before user logs in, will create a table to store the users information
    {
        //
    }


    class GameMenu
    {
        static void Main(string[] args) // Future reference: With the implementation of the authentication system soon, this will be moved.
        {
            SmoothConsole smoothPrinting = new SmoothConsole();
            UIManager UI = new UIManager();
            Console.Title = "FantasyRPG";
            // MagicCouncil encounter = new MagicCouncil(); // Debugging
            // string name = "Silver"; // Debugging
            // encounter.firstEncounter(name); // Debugging
            // FirstScenario firstScenario = new FirstScenario();
            // firstScenario.usersFirstJourney("Tristian");

            // Define values for debugging mage
            // string mageName = "Khalid Du-Lucérian";

            // List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription, string category, int quantity)> mageWeapon = new List<(string, int, string, string, string, string, int)> 
            // {
            // ("Heartblades Vesper", 250, "Legendary", "Staff", "Developer weapon :3", "Developer-Exclusive", 1)
            // };

            // string[] mageSpecialties = new string[] { "Fire-Magic", "Lightning-Magic", "Eucladian-Magic", "Light-Magic", "Dark-Magic" };
            // int arcaniaGoldCoins = 100000;

            // List<(string, int, int)> magicSpells = new List<(string, int, int)> {
            // ("Lucerian's Wrath", 350, 80),
            // ("Umbral Surge", 120, 50),
            // ("Cyclone Strike", 50, 20)
            // };

            // List<(string itemName, string itemDescription, string itemRarity, int itemPower, string category, int quantity)> currentInventory = new List<(string, string, string, int, string, int)>()
            // {
            // ("Heartblades Vesper", "Developer weapon :3", "Legendary", 250, "Weapon", 1),
            //  ("Du-Lucérian's Elixir", "A legendary potion crafted by Khalid Du-Lucérian, renowned for its mystical properties. It instantly restores +100 health to the imbiber.", "Legendary", 100, "Potion", 50)
            // };

            // int specialAtkRecharge = 100;

            // List<(string npcName, string npcInformation, string npcAffiliation)> npcsEncountered = new List<(string, string, string)> {
            // ("Veridian Pendragon", "False ranker and solo assassin, very capable and someone not to underestimate.", "Heartblade Association/Pendragon Lineage"),
            // ("Evelyn Everbright", "Rank 10 of the Arcania's Magic Council and Guildmaster of Arcania's Magic Council.", "Arcania's Magic Council/Arcane Sentinels"),
            // ("Khalid Du-Lucérian", "The true leader of Arcania's Magic Council, identity remains unknown.", "Arcania's Magic Council/Heartblade Association/Lucerian Lineage"),
            // ("Cloud (Real Identity - Silver Eucladian-Nine)", "Rank 1 of the Arcania's Magic Council.", "Arcania's Magic Council/Eucladian-Nine Lineage")
            // };


            // bool debuggingCombat = false;

            // Create the debugging mage object with the specified arguments
            // Mage debuggingMage = new Mage(mageName, mageWeapon, mageSpecialties, arcaniaGoldCoins, magicSpells, currentInventory, specialAtkRecharge, npcsEncountered, debuggingCombat); // Debugging Mage

            // OP Parameter changes
            // debuggingMage.currentHealth = 500;
            // debuggingMage.maxHealth = 500;
            // debuggingMage.currentMana = 500;
            // debuggingMage.maxMana = 500;
            // debuggingMage.level = 60;

            // ForestOfMysteries scenario = new ForestOfMysteries();
            // int remainingAttempts = 3;

            // Debugging Mode - GAME IS IN TESTING PHASE
            // smoothPrinting.PrintLine("--------------------------------------------------");
            // smoothPrinting.PrintLine("FantasyRPG: Debugging Mode");
            // smoothPrinting.PrintLine("--------------------------------------------------");

            // smoothPrinting.RapidPrint($"{debuggingMage.name} - Debugger Mage\n");
            // smoothPrinting.RapidPrint($"Level: {debuggingMage.level}\n");

            // smoothPrinting.PrintLine("--------------------------------------------------");
            // smoothPrinting.PrintLine("FantasyRPG: Debugging Mage - Stats");
            // smoothPrinting.PrintLine("--------------------------------------------------");

            // UI.DisplayProgressBar($"{debuggingMage.name}'s Health", debuggingMage.currentHealth, debuggingMage.maxHealth, 30); // Display Mage's health

            // Console.WriteLine(); // Spacing

            // UI.DisplayProgressBar($"{debuggingMage.name}'s Mana", debuggingMage.currentMana, debuggingMage.maxMana, 30);

            // Console.WriteLine(); // Spacing
            // Console.WriteLine(); // Spacing

            // smoothPrinting.PrintLine("--------------------------------------------------");
            // smoothPrinting.PrintLine("FantasyRPG: Debugging Mage - Weapon");
            // smoothPrinting.PrintLine("--------------------------------------------------"); 

            // Display details of the current weapon used
            // foreach (var weaponDetails in debuggingMage.weapon)
            // {
            // smoothPrinting.RapidPrint($"\nWeapon Name: {weaponDetails.weaponName}\nWeapon Damage: {weaponDetails.damage}\nWeapon Rarity: {weaponDetails.rarity}\nWeapon Description: {weaponDetails.weaponDescription}\n");
            // }

            // Console.WriteLine(); // Spacing

            // int spellCount = 1;

            // smoothPrinting.PrintLine("--------------------------------------------------");
            // smoothPrinting.PrintLine("FantasyRPG: Debugging Mage - Spells");
            // smoothPrinting.PrintLine("--------------------------------------------------");

            // Display the moveset of the mage
            // foreach (var spell in ((Mage)debuggingMage).magicSpells) // Display all spells currently avaliable to the Mage
            // {
            // smoothPrinting.RapidPrint($"\n{spellCount}. Spell: {spell.magicSpell} - Damage: {spell.damage}\nMana Requirement: {spell.manaRequirement}\n");
            // spellCount++;
            // }

            // UI.PromptUserToContinue();

            // scenario.forestOfMysteries(debuggingMage, remainingAttempts); // Call the forestOfMysteries method with the Mage object and remaining attempts


            GameMenu menu = new GameMenu();
            menu.gameMenu(); // User is first directed to the game menu method

            // List<(string npcName, string npcDescription, string npcAffiliation)> npcsEncountered = new List<(string npcName, string npcDescription, string npcAffiliation)>() // Debugging: populating data
            // {
            //  ("Kaelen Stormer", "Rank 6 of Arcania's Magic Council, known for being one of the most formiddable Dark Elves conquering his enemies with meticulous assasination capabilities.", "Arcania's Magic Council"),
            // ("Silver Eucladian-Nine", "The real identity of Cloud, Rank 1 in Arcania's Magic Council.", "Arcania's Magic Council/Eucladian Lineage"),
            // ("Evelyn Everbright", "Rank 10 of Arcania's Magic Council, known for her gracious beauty that graces wherever she goes.", "Arcania's Magic Council/Arcane Sentinels"),
            // ("Mo Blade", "Rank 3 of Arcania's Magic Council, known to be one of the most vicious pirates around!", "Arcania's Magic Council/Red Sea")
            // };


            // gameDashboard dash = new gameDashboard();
            // dash.dashboard(debuggingMage);

        }


        public void gameMenu() // After user information is authenticated, they'll be lead here
        {
            SmoothConsole smoothOutput = new SmoothConsole(); // Initialize the smooth console
            int userChances = 3; // Will be used for recursive measures to prevent brute force and idiotic input
                                 // Future reference: Implementing AI mobs and perhaps AI individuals

            string? userChoice; // Used for the start of the game
            string[] gameTips = {"Did you know that every 10 levels, you can get an extra ability/speciality?",
                "This game is still in development, so if there's an issue please contact me through my GitHub (Escavine) and send a pull request which I'll review.",
                "Just to be clear, Eucladian is a magical type ability, not to be confused with the mathematical term Euclidean.",
            "Eucladian abilities are very overpowered, but in turn they'll cost you some health.", "This game have a sneaky RNG factor, you'll see later as you play :3",
            }; // Array containing necessary game tips, more will be added in the future.

            // Initiation of the console game
            smoothOutput.PrintLine("--------------------------------------------------");
            smoothOutput.PrintLine("FantasyRPG");
            smoothOutput.PrintLine("--------------------------------------------------");

            Console.WriteLine("\nGame advice: When inputting values, input a corresponding value to the action (e.g. enter the value 1 in order to start the game"); // Display game advice
            Console.WriteLine("\nIt is highly recommended to play this game in full screen, to allow all the text to fit in order to get the best experience");

            Random ran = new Random();
            int ran_num = ran.Next(0, 5);
            Console.WriteLine("\nGame Tip: " + gameTips[ran_num] + "\n"); // Display a random game tip in the menu

            smoothOutput.PrintLine("=======================");
            smoothOutput.PrintLine("Game Menu");
            smoothOutput.PrintLine("=======================\n");

            smoothOutput.RapidPrint("\n1 - Get started\n");
            smoothOutput.RapidPrint("\n2 - Load save game (N/A)\n");
            smoothOutput.RapidPrint("\n3 - Help\n");
            smoothOutput.RapidPrint("\n4 - Make a suggestion\n");
            smoothOutput.RapidPrint("\n5 - Future plans\n");
            smoothOutput.RapidPrint("\nEnter a corresponding value: ");
            // Register user input
            userChoice = Convert.ToString(Console.ReadLine());

            switch (userChoice)
            {
                case "1":
                    Console.Clear();
                    ClassSelection selectClass = new ClassSelection(); // Create a new game session
                    selectClass.userClass(); // Proceed to let the user pick a character class
                    break;
                case "2":
                    loadingSaveData(userChances); // Lead user to the method
                    break;
                case "3":
                    Console.Clear(); // Neatness structuring
                    userChoice = null;
                    helpSection(userChances); // Lead user to the method
                    break;
                case "4":
                    makeGameSuggestion(); // Lead user to the method
                    break;
                case "5":
                    futurePlans(); // Lead user to the method
                    break;
                default:
                    Console.WriteLine("Invalid option, please try again!");
                    Console.Clear();
                    gameMenu(); // Recuse to ensure that the user picks the correct option, and to prevent the program from breaking so easily
                    break;
            }
        }


        void loadingSaveData(int userChances)
        {
            Console.Clear();
            SmoothConsole smoothPrinting = new SmoothConsole();
            int? loadingSaveDataInput;

            // Should the user be logged in, they'll be able to access their save data
            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine("FantasyRPG: " + "Loading Save Data");
            smoothPrinting.PrintLine("--------------------------------------------------");

            // Display the user's remaining input attempts, if they input an incorrect value
            if (userChances <= 3)
            {
                Console.WriteLine($"Remaining input chances: {userChances}"); // Display the number of remaining attempts

                if (userChances == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    smoothPrinting.RapidPrint("\nToo many incorrect attempts, FantasyRPG will now terminate."); // Terminate users session, should they have no remaining chances 
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }

            smoothPrinting.PrintLine("\nLoading current save progress isn't available yet.");
            smoothPrinting.RapidPrint("\nOptions");
            smoothPrinting.RapidPrint("\n1. Return to Menu\n");
            smoothPrinting.RapidPrint("\nEnter a corresponding value: ");

            loadingSaveDataInput = Convert.ToInt32((Console.ReadLine())); // Porcess user input.

            if (loadingSaveDataInput == 1)
            {
                smoothPrinting.FastPrint("\nYou will be lead back to the Menu.\n");
                Console.Clear();
                gameMenu();
            }
            else
            {
                smoothPrinting.FastPrint("\nInvalid input, please try again."); // Inform the user about the invalid input
                Console.Clear(); // Neatness
                loadingSaveDataInput = null; // Clear the variable to allow reinput
                loadingSaveData(userChances - 1); // Recursively call the method with reduced chances

            }

        }

        public void helpSection(int userChances)
        {
            Console.Clear(); // Neatness
            SmoothConsole smoothPrinting = new SmoothConsole();

            // Display the user's remaining input attempts, if they input an incorrect value
            if (userChances <= 3)
            {
                Console.WriteLine($"Remaining input chances: {userChances}"); // Display the number of remaining attempts

                if (userChances == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    smoothPrinting.PrintLine("--------------------------------------------------");
                    smoothPrinting.PrintLine("FantasyRPG: " + "Help Section");
                    smoothPrinting.PrintLine("--------------------------------------------------");
                    smoothPrinting.RapidPrint("\nToo many incorrect attempts, FantasyRPG will now terminate."); // Terminate users session, should they have no remaining chances 
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }

            int? userInput;
            string[] gameAdvice = { "You might die at any point within the game unknowingly.",
                        "Eucladian abilities are quite overpowered, if you find the opportunity to pursue it, then do so.",
                    "Having a strong romantical bond with someone, can potentially increase your abilities.", "There are many classes to choose from, all having unique features.",
                    "Avoid fighting overpowered foes early in-game (i.e. dragons), you'll probably get destroyed." };
            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine("FantasyRPG: " + "Help Section");
            smoothPrinting.PrintLine("--------------------------------------------------");

            Console.WriteLine("Input the following options: ");
            smoothPrinting.RapidPrint("\n1. What is FantasyRPG?\n");
            smoothPrinting.RapidPrint("\n2. Arcania's Magic Council\n");
            smoothPrinting.RapidPrint("\n3. Game advice from the developers\n");
            smoothPrinting.RapidPrint("\nEnter a corresponding value: ");

            // Ask if the user wants to see any game advice in the help section
            userInput = Convert.ToInt32(Console.ReadLine());

            switch (userInput)
            {
                case 1:
                    Console.Clear();
                    smoothPrinting.FastPrint("What is FantasyRPG?\n");
                    // Introduction to Arcania, the world of FantasyRPG
                    smoothPrinting.RapidPrint("\nWelcome to FantasyRPG, a text-based adventure that transports you to the mystical realm of Aeolus! Embark on an epic journey through a vast and enchanting world, where hidden treasures await discovery at every turn. Prepare yourself for the challenges ahead, as you confront life-and-death situations, battle formidable foes, and overcome treacherous obstacles.\n");
                    Console.WriteLine();
                    smoothPrinting.RapidPrint("In Aeolus, your choices shape your destiny. Navigate the immersive landscape, forge alliances with fellow travelers, and encounter mythical creatures that will test your courage and resolve. But beware, adventurer, for danger lurks in the shadows. Face cunning enemies, solve challenging puzzles, and unravel the mysteries that lie dormant in this magical land.\n");
                    Console.WriteLine();
                    smoothPrinting.RapidPrint("Amidst the chaos, there is also the promise of something more. As you progress, open your heart to the possibility of romantic connections, adding depth to your personal story.\n");
                    Console.WriteLine();
                    smoothPrinting.FastPrint("Are you ready to embark on a journey into the heart of Aeolus, where every decision shapes your fate? Your adventure begins now!\n");
                    smoothPrinting.RapidPrint("\nAffirmative? If so, click anywhere to be redirected back to the Menu. ");
                    Console.ReadKey();
                    Console.Clear();
                    gameMenu(); // Redirect user back to the menu...
                    break;
                case 2:
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow; // Yellow output for the description
                    smoothPrinting.PrintLine("--------------------------------------------------");
                    smoothPrinting.PrintLine("FantasyRPG: " + "Arcania's Magic Council");
                    smoothPrinting.PrintLine("--------------------------------------------------");
                    Console.WriteLine(); // Neatness
                    smoothPrinting.RapidPrint("The Magic Council consists of the 10 strongest individuals from the city of Arcania, ranging from 1 being the strongest to 10 being the weakest. Each member holds immense power and knowledge in their respective fields of magic, making them formidable forces within the realm. \r\n\r\nRoles and Responsibilities: \r\n\r\nThe council members oversee and regulate magical affairs across Arcania, ensuring balance and order in the city and other regions. They adjudicate disputes among magical practitioners, enforce magical laws, and protect the lands from magical threats. Additionally, they serve as advisors to the ruling powers of Arcania, providing counsel on matters relating to magic and arcane knowledge. \r\n\r\nChallenging for a Seat: \r\n\r\nShould you possess the strength and develop a certain level of reputation, you’ll be able to challenge one of their members for a seat within the council. However, this is no easy feat, as the cost for losing is death, resulting in the game being reset. Yet, if you emerge victorious, you can rightfully claim their seat as your own and slowly rise in the ranks. \r\n\r\nBenefits of Council Membership: \r\n\r\nBeing a member of the Magic Council grants numerous benefits, including access to rare magical artifacts, exclusive knowledge of ancient spells, and influence over magical institutions and organizations. Council members also enjoy protection and prestige within Arcania, as well as opportunities for further personal and magical growth. ");
                    // Display the Arcania's Magic Council members
                    Console.WriteLine(); // Neatness
                    smoothPrinting.RapidPrint("\nAffirmative? If so, click any key to see the current rankers within the Magic Council. ");
                    Console.ReadKey();
                    Console.Clear(); // Neatness
                    Console.ForegroundColor = ConsoleColor.Yellow; // Yellow output for the rankings
                    smoothPrinting.PrintLine("--------------------------------------------------");
                    smoothPrinting.PrintLine("FantasyRPG: " + "Arcania's Magic Council (Current Rankings)");
                    smoothPrinting.PrintLine("--------------------------------------------------");
                    Console.WriteLine(); // Neatness

                    // Future reference: Display the rankings using a dictionary, so that way when an individual defeats a user of the top 10, they can manually append their details to the rankings
                    smoothPrinting.RapidPrint("Rank 1: ??? - Rank: S** (Class: ???, Race: ???) \r\n\r\nRank 2: ??? - Rank: S* (Class: ???, Race: ???) \r\n\r\nRank 3: ??? - Rank: S (Class: ???, Race: ???) \r\n\r\nRank 4: Lister Everbright - Rank: A* (Class: Knight, Race: Elf) \r\n\r\nRank 5: Aurelia Eucladian-Nine - Rank: S- (Class: Mage, Race: Human) \r\n\r\nRank 6: Kaelen Stormer - Rank: S* (Class: Assassin, Race: Dark Elf) \r\n\r\nRank 7: Lyra Leywin - Rank: S- (Class: Necromancer, Race: Demon) \r\n\r\nRank 8: Windsom - Rank: A* (Class: Guardian, Race: Dragon) \r\n\r\nRank 9: Selene - Rank: A (Class: Succubus, Race: Demon) \r\n\r\nRank 10: Evelyn Everbright - Rank: S- (Class: High-Elf Warrior, Race: Elf) ");
                    Console.WriteLine(); // Neatness
                    smoothPrinting.RapidPrint("\nAffirmative? If so, click anywhere to be redirected back to the Menu. ");
                    Console.ReadKey();
                    Console.Clear(); // Neatness
                    Console.ForegroundColor = ConsoleColor.Gray; // Reset console color
                    gameMenu(); // Redirect user back to the menu...
                    break;

                case 3:
                    Console.Clear();
                    smoothPrinting.PrintLine("--------------------------------------------------");
                    smoothPrinting.PrintLine("FantasyRPG: " + "Game Advice");
                    smoothPrinting.PrintLine("--------------------------------------------------");
                    Console.WriteLine();
                    foreach (string s in gameAdvice) // Display game advice
                    {
                        smoothPrinting.FastPrint("* " + s + "\n");
                        Console.WriteLine(); // Neatness
                    }
                    smoothPrinting.RapidPrint("\nAffirmative? If so, click anywhere to be redirected back to the Menu. ");
                    Console.ReadKey();
                    Console.Clear();
                    gameMenu(); // Redirect user back to the menu...
                    break;
                default:
                    smoothPrinting.FastPrint("\nInvalid input, please try again."); // Inform the user about the invalid input
                    helpSection(userChances - 1); // Recursively call the method with reduced chances
                    break;
            }

            Console.ReadKey(); // Wait for key input
        }


        public void makeGameSuggestion() // Game suggestions
        {
            Console.Clear();
            SmoothConsole smoothPrinting = new SmoothConsole();
            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine("FantasyRPG: " + "Game Suggestions");
            smoothPrinting.PrintLine("--------------------------------------------------");
            Console.WriteLine();
            smoothPrinting.FastPrint("Send a message to kmescavine@gmail.com in order to send your ideas!"); // Future reference: Use an SMTP feature to allow the user to input their email and send their suggestion
            Console.WriteLine();
            smoothPrinting.RapidPrint("\nAffirmative? If so, click anywhere to be redirected back to the Menu. ");
            Console.ReadKey();
            Console.Clear();
            gameMenu(); // Redirect user back to the menu...
        }

        public void futurePlans() // Future plans for the game development
        {
            Console.Clear();
            SmoothConsole smoothPrinting = new SmoothConsole();

            string[] futurePlans = { "Adding new classes", "Potential romance feature", "Illnesses that can lead to unexpected deaths, and cures", "Game difficulty (easy, normal, hard, impossible)" };
            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine("FantasyRPG: " + "Future Endeavours");
            smoothPrinting.PrintLine("--------------------------------------------------");
            Console.WriteLine();
            foreach (string plan in futurePlans)
            {
                smoothPrinting.RapidPrint("* " + plan + "\n");
                Console.WriteLine();
            }
            smoothPrinting.RapidPrint("\nAny other suggestions can be sent to kmescavine@gmail.com for review.\n");
            smoothPrinting.RapidPrint("\nAffirmative? If so, click anywhere to be redirected back to the Menu. ");
            Console.ReadKey();
            Console.Clear();

            gameMenu(); // Redirect user back to the menu...


        }




    }

    public class ClassSelection // This class will allow a user to pick from a variety of different roles in the game, before embarking on their journey.
    {
        public void userClass()
        {
            SmoothConsole smoothPrinting = new SmoothConsole(); // initiate the smooth console class

            // Storyline: Explain how the user came to the world
            smoothPrinting.CenterPrint("---------Summoned by Destiny: The Journey to Aeolus----------\n");
            smoothPrinting.RapidPrint("\nIn a quiet library on Earth, you find yourself inexplicably drawn to an alluring book that is basked within the circumference of the sun's rays. Intrigued by the mysterious markings, you reach out to the book. \r\n\r\nAs your fingers contact the book, a flash of light engulfs you, and you feel pulled into a vortex of energy. In an instant, you are whisked away from their familiar surroundings to a world unlike anything they've ever seen. \r\n\r\nConfused about how you ended up in this strange new realm, you soon learn that they've been summoned to Aeolus by powerful forces seeking help in a time of dire need, as you are the destined one, that isn’t contained by the limits of fate. It seems your unique abilities and knowledge are crucial for overcoming a looming threat that could destroy the entire world of Aeolus. \r\n\r\nWith no way to return, you must now navigate the unfamiliar landscapes and cultures of Aeolus, forging alliances and honing their skills as they strive to fulfill their newfound destiny and save this world from destruction. ");

            smoothPrinting.RapidPrint("\n\nNaturally, you are not familiar with the system that is implemented in the world of Aeolus, therefore you are sent to a bleak white room, where you learn how to familiarize yourself with the world's customs. This includes the plethora of mana surrounding Aeolus and the world's rulings. You can now select a class type.");

            string? userChoice; // Define the user choice

            // Defining the different classes and rarity of items
            string[] fantasyClasses = { "Mage", "Knight (N/A)", "Pirate", "Shadowwrath (N/A)", "Archer (N/A)", "Return to Menu" }; // Predefined array of roles
            string[] rarity = { "Common", "Uncommon", "Rare", "Unique", "Legendary" }; // Predefined values :3
            int num = 1;

            Console.WriteLine("\n");
            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine("FantasyRPG: " + "Class Selection");
            smoothPrinting.PrintLine("--------------------------------------------------\n");

            for (int i = 0; i < fantasyClasses.Length; i++)
            {

                smoothPrinting.FastPrint("\n" + num + " - " + fantasyClasses[i] + "\n");
                num++;
            }

            smoothPrinting.RapidPrint("\nEnter a corresponding value: ");

            // Future reference: INPUT VALIDATION
            userChoice = Convert.ToString(Console.ReadLine());

            switch (userChoice) // Future reference: Rather than have a userchoice fixed to a single method, add multiple methods for different classes (i.e. a mage class if a user chooses the mage role etc, that way you can implement recursion if the user wants to reset their details)
            {
                // Should the user decided to become a Mage
                case "1":
                    int choiceIncrementer = 1; // Used to increment the user choice when picking magic types
                    string? startMageJourneyInput;
                    List<(string npcName, string npcDescription, string npcAffiliation)> mageClassNpcsEncountered = null; // During class selection, individual will have not met any NPCs, therefore this value will be remained null.

                    // Arrays containing the variety of different magic choices, spells and weapons.
                    string[] magicChoices = { "Fire-Magic", "Water-Magic", "Ice-Magic", "Lightning-Magic", "Dark-Magic", "Light-Magic", "Eucladian-Magic" };
                    int arcaniaGoldCoins = 0; // You start of as a brokie 

                    // Tuple dictionary for each Fire magic spell, which is associated with a damage value and a mana requirement 
                    Dictionary<string, (int damage, int manaRequirement)> fireMagicSpells = new Dictionary<string, (int damage, int manaRequirement)>()
                    {
                        { "Infrared", (3,15) },
                        { "Blazing Rage", (5,20) },
                        { "Flamestrike", (7,25) },
                        { "Pyroburst", (9,30) },
                        { "Phoenix Fury", (12,35) }
                    };

                    // Tuple dictionary for each Water magic spell, which is associated with a damage value and a mana requirement 
                    Dictionary<string, (int damage, int manaRequirement)> waterMagicSpells = new Dictionary<string, (int damage, int manaRequirement)>()
                    {
                        { "Aqua Torrent", (2,10) },
                        { "Hydroburst", (4,15) },
                        { "Lunar Tide", (6,20) },
                        { "Ripple Cascade", (8,25) }
                    };

                    // Tuple dictionary for each Ice magic spell, which is associated with a damage value a mana requirement
                    Dictionary<string, (int damage, int manaRequirement)> iceMagicSpells = new Dictionary<string, (int damage, int manaRequirement)>()
                    {
                        { "Frostbite", (5,20) },
                        { "Ice Lance", (9,30) },
                        { "Blizzard Tundra", (15,50) },
                        { "Frozen Fury", (7,25) }
                    };

                    // Tuple dictionary for each Lightning magic spell, which is associated with a damage value and a mana requirement 
                    Dictionary<string, (int damage, int manaRequirement)> lightningMagicSpells = new Dictionary<string, (int damage, int manaRequirement)>()
                    {
                        { "Thunderstrike", (4,15) },
                        { "Striking Surge", (6,20) },
                        { "Volt Surge", (8,25) },
                        { "Arcane Thunder", (10,30) }
                    };

                    // Tuple dictionary for each Dark magic spell, which is associated with a damage value and a mana requirement 
                    Dictionary<string, (int damage, int manaRequirement)> darkMagicSpells = new Dictionary<string, (int damage, int manaRequirement)>()
                    {
                        { "Shadow Veil", (3,15) },
                        { "Umbral Surge", (5,20) },
                        { "Wraith's Curse", (7,25) },
                        { "Eclipised Oblivion", (9,30) }
                    };

                    // Tuple dictionary for each Light magic spell, which is associated with a damage value and a mana requirement 
                    Dictionary<string, (int damage, int manaRequirement)> lightMagicSpells = new Dictionary<string, (int damage, int manaRequirement)>()
                    {
                        { "Luminous Beam", (3,15) },
                        { "Solar Flare", (5,20) },
                        { "Etherial Halo", (7,25) },
                        { "Aurora's Illumination", (9,30) },
                        { "Divine Judgement", (12,35) }
                    };

                    // Tuple dictionary for each Eucladian magic spell, which is associated with a damage value and a mana requirement 
                    Dictionary<string, (int damage, int manaRequirement)> eucladianMagicSpells = new Dictionary<string, (int damage, int manaRequirement)>()
                    {
                        { "Esoteric Paradigm", (3,15) },
                        { "Fractural Fissure", (5,20) },
                        { "Quantum Flux", (7,25) },
                        { "Etherial Nexus", (9,30) }
                    };

                    Dictionary<string, (int damage, string rarity, string weaponType, string weaponDescription, string category, int quantity)> starterMageWeapons = new Dictionary<string, (int damage, string rarity, string weaponType, string weaponDescription, string category, int quantity)>()
                    {
                        { "Weathered Oakwind", (5, "Common", "Staff", "A primitive staff made with oak, that has been weathered down with time. Has the potential to regain it's once lost status, should it be with the 'Chosen One'.", "Starter Weapon", 1) },
                        { "Ancient Runestaff", (7, "Uncommon", "Staff", "Found in the lost ruins, filled with ancient mysteries yet to be untold.", "Starter Weapon", 1) },
                        { "Runic Wooden Scepter", (3, "Common", "Staff", ".", "Starter Weapon", 1) },
                        { "Dusty Relic Rod", (2, "Common", "Staff", "Dusty and archaic staff, tough luck if you receive this staff.", "Starter Weapon", 1) },
                        { "Emerald Crystal Staff", (10, "Unique", "Staff", "A staff adorned with a seraphic crystal, bolstering its power. This staff is a sign of blessed luck!", "Starter Weapon", 1) }
                    };

                    Console.Clear(); // Cleaning purposes
                    Console.ForegroundColor = ConsoleColor.White;
                    smoothPrinting.PrintLine("--------------------------------------------------");
                    smoothPrinting.PrintLine("FantasyRPG: " + "Mage Class");
                    smoothPrinting.PrintLine("--------------------------------------------------\n");

                    smoothPrinting.RapidPrint("What is your name, adventurer? ");
                    string mageName = Convert.ToString(Console.ReadLine());


                    // Display the starter weapons
                    // smoothPrinting.RapidPrint("\nDisplaying starter weapons...");
                    // Console.WriteLine("\n"); // Neat stucturing

                    // foreach (var starterWeapon in starterMageWeapons) {

                    // smoothPrinting.RapidPrint($"\n* {starterWeapon.Key}, Damage: {starterWeapon.Value.damage}, Rarity: {starterWeapon.Value.rarity}");
                    // }

                    // smoothPrinting.RapidPrint("Would you like to pick a weapon?");
                    // Console.ReadKey();


                    // Console.ForegroundColor = ConsoleColor.Red;
                    // smoothPrinting.RapidPrint("Did you seriously think you had a choice as to what you get to pick? You don't."); // User isn't given a choice :3
                    // Console.ReadKey();
                    // Console.Clear();

                    Console.ForegroundColor = ConsoleColor.White; // Reset the console color output
                    smoothPrinting.RapidPrint("\nYou will be randomly assigned a starter weapon...");


                    List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription, string category, int quantity)> starterMageWeaponChoices = starterMageWeapons.Select(entry => (entry.Key, entry.Value.damage, entry.Value.rarity, entry.Value.weaponType, entry.Value.weaponDescription, entry.Value.category, entry.Value.quantity)).ToList();

                    List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription, string category, int quantity)> mageStaff = new List<(string, int, string, string, string, string, int)>(starterMageWeaponChoices);

                    Random ranNum = new Random();
                    int random_index = ranNum.Next(0, starterMageWeaponChoices.Count); // Generates random value that'll decide on which weapon the user gets 
                    mageStaff.Add(starterMageWeaponChoices[random_index]); // Append the weapon into the mage staff list

                    List<(string itemName, string itemDescription, string itemRarity, int itemPower, string category, int quantity)> mageInventory = new List<(string, string, string, int, string, int)>();
                    mageInventory.Add((starterMageWeaponChoices[random_index].weaponName, starterMageWeaponChoices[random_index].weaponDescription, starterMageWeaponChoices[random_index].rarity, starterMageWeaponChoices[random_index].damage, starterMageWeaponChoices[random_index].category, starterMageWeaponChoices[random_index].quantity)); // Store the weapon in the users inventory

                    smoothPrinting.RapidPrint("\nOnce you are done reading the details, press any key to move on.");
                    Console.ReadKey();
                    Console.Clear();

                    var randomWeapon = mageStaff.First(); // Retrieve the only element added to mageStaff
                                                          // smoothPrinting.FastPrint($"Assigned weapon: {randomWeapon.weaponName}, Damage: {randomWeapon.damage}, Rarity: {randomWeapon.rarity}, Weapon Type: {randomWeapon.weaponType}, \nWeapon Description: {randomWeapon.weaponDescription}"); // Display the assigned weapon to the user

                    // Console.WriteLine("Affirmative? Press any key to continue.");
                    // Console.ReadKey();
                    // Console.Clear();

                    smoothPrinting.PrintLine("---------Selecting a Magic Speciality----------\n"); // Display the magic speciality selection

                    List<string> magicSpecialties = new List<string>(); // Chosen magic specialities
                    List<(string magicSpell, int damage, int manaRequirement)> magicSpells = new List<(string magicSpell, int damage, int manaRequirement)>(); // Chosen magical spells

                    // Display all the magic choices to the user
                    for (int j = 0; j < magicChoices.Length; j++)
                    {
                        smoothPrinting.FastPrint($"\n{choiceIncrementer} - {magicChoices[j]}\n");
                        choiceIncrementer++;
                    }


                    // Allow the user to choose a single magic specialty
                    for (int k = 0; k < 1; k++)
                    {
                        int chosenSpecialtyIndex;

                        // Prompt the user to choose a magic specialty
                        smoothPrinting.FastPrint("\nChoose a magic specialty by entering the corresponding number: ");

                        // Keep prompting until a valid choice is made
                        while (!int.TryParse(Console.ReadLine(), out chosenSpecialtyIndex) || chosenSpecialtyIndex < 1 || chosenSpecialtyIndex > magicChoices.Length)
                        {
                            // Display an error message for invalid input
                            Console.WriteLine("Invalid choice. Please enter a valid number corresponding to the magic specialty.");
                        }

                        // Add the chosen magic specialty to the list
                        magicSpecialties.Add(magicChoices[chosenSpecialtyIndex - 1]);
                    }

                    Console.Clear(); // Neatness

                    int totalSpellsDisplayed = 0; // Keep track of the total spells displayed

                    // Will be used to check the magic specialities chosen by the user before displaying the range of spells they can pick


                    for (int z = 0; z < magicSpecialties.Count; z++)
                    {
                        smoothPrinting.PrintLine($"---------{magicSpecialties[z]} Spells---------\n");
                        switch (magicSpecialties[z])
                        {
                            case "Fire-Magic":
                                Console.ForegroundColor = ConsoleColor.Red; // Red for fire
                                foreach (var spell in fireMagicSpells)
                                {
                                    smoothPrinting.FastPrint($"{(totalSpellsDisplayed + 1)}. {spell.Key} - Damage: {spell.Value.damage}, Mana Requirement for Activation: {spell.Value.manaRequirement}");
                                    totalSpellsDisplayed++;
                                    Console.WriteLine("\nPress Enter to see the next spell...");
                                    Console.ReadLine();
                                }
                                break;

                            case "Water-Magic":
                                Console.ForegroundColor = ConsoleColor.DarkBlue; // Dark-blue for water
                                foreach (var spell in waterMagicSpells)
                                {
                                    smoothPrinting.FastPrint($"{(totalSpellsDisplayed + 1)}. {spell.Key} - Damage: {spell.Value.damage}, Mana Requirement for Activation: {spell.Value.manaRequirement}");
                                    totalSpellsDisplayed++;
                                    Console.WriteLine("\nPress Enter to see the next spell...");
                                    Console.ReadLine();
                                }
                                break;

                            case "Lightning-Magic":
                                Console.ForegroundColor = ConsoleColor.Yellow; // Yellow for lightning
                                foreach (var spell in lightningMagicSpells)
                                {
                                    smoothPrinting.FastPrint($"{(totalSpellsDisplayed + 1)}. {spell.Key} - Damage: {spell.Value.damage} , Mana Requirement for Activation:  {spell.Value.manaRequirement}");
                                    totalSpellsDisplayed++;
                                    Console.WriteLine("\nPress Enter to see the next spell...");
                                    Console.ReadLine();
                                }
                                break;

                            case "Ice-Magic":
                                Console.ForegroundColor = ConsoleColor.Blue; // Blue for ice
                                foreach (var spell in iceMagicSpells)
                                {
                                    smoothPrinting.FastPrint($"{(totalSpellsDisplayed + 1)}. {spell.Key} - Damage: {spell.Value.damage} , Mana Requirement for Activation:  {spell.Value.manaRequirement}");
                                    totalSpellsDisplayed++;
                                    Console.WriteLine("\nPress Enter to see the next spell...");
                                    Console.ReadLine();
                                }
                                break;

                            case "Dark-Magic":
                                Console.ForegroundColor = ConsoleColor.DarkGray; // Dark gray for dark magic
                                foreach (var spell in darkMagicSpells)
                                {
                                    smoothPrinting.FastPrint($"{(totalSpellsDisplayed + 1)}. {spell.Key} - Damage: {spell.Value.damage} , Mana Requirement for Activation:  {spell.Value.manaRequirement}");
                                    totalSpellsDisplayed++;
                                    Console.WriteLine("\nPress Enter to see the next spell...");
                                    Console.ReadLine();
                                }
                                break;

                            case "Light-Magic":
                                Console.ForegroundColor = ConsoleColor.White; // White for light magic
                                foreach (var spell in lightMagicSpells)
                                {
                                    smoothPrinting.FastPrint($"{(totalSpellsDisplayed + 1)}. {spell.Key} - Damage: {spell.Value.damage} , Mana Requirement for Activation:  {spell.Value.manaRequirement}");
                                    totalSpellsDisplayed++;
                                    Console.WriteLine("\nPress Enter to see the next spell...");
                                    Console.ReadLine();
                                }
                                break;

                            case "Eucladian-Magic":
                                Console.ForegroundColor = ConsoleColor.DarkCyan; // Dark red for Eucladian Magic
                                foreach (var spell in eucladianMagicSpells)
                                {
                                    smoothPrinting.FastPrint($"{(totalSpellsDisplayed + 1)}. {spell.Key} - Damage: {spell.Value.damage} , Mana Requirement for Activation:  {spell.Value.manaRequirement}");
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
                        if (z < magicSpecialties.Count - 1)
                        {
                            Console.WriteLine("\nPress Enter to see the spells for the next specialty...");
                            Console.ReadLine();
                        }
                    }


                    for (int specialityIndex = 0; specialityIndex < magicSpecialties.Count; specialityIndex++)
                    {
                        Console.WriteLine($"Select 2 magic spells for {magicSpecialties[specialityIndex]} by entering the corresponding numbers (1-4 for each element): ");

                        List<(string magicSpell, int damage, int manaRequirement)> currentMagicSpells = new List<(string magicSpell, int damage, int manaRequirement)>();

                        switch (magicSpecialties[specialityIndex])
                        {
                            case "Fire-Magic":
                                currentMagicSpells = fireMagicSpells.Select(entry => (entry.Key, entry.Value.damage, entry.Value.manaRequirement)).ToList();
                                break;
                            case "Water-Magic":
                                currentMagicSpells = waterMagicSpells.Select(entry => (entry.Key, entry.Value.damage, entry.Value.manaRequirement)).ToList();
                                break;
                            case "Ice-Magic":
                                currentMagicSpells = iceMagicSpells.Select(entry => (entry.Key, entry.Value.damage, entry.Value.manaRequirement)).ToList();
                                break;
                            case "Lightning-Magic":
                                currentMagicSpells = lightningMagicSpells.Select(entry => (entry.Key, entry.Value.damage, entry.Value.manaRequirement)).ToList();
                                break;
                            case "Dark-Magic":
                                currentMagicSpells = darkMagicSpells.Select(entry => (entry.Key, entry.Value.damage, entry.Value.manaRequirement)).ToList();
                                break;
                            case "Light-Magic":
                                currentMagicSpells = lightMagicSpells.Select(entry => (entry.Key, entry.Value.damage, entry.Value.manaRequirement)).ToList();
                                break;
                            case "Eucladian-Magic":
                                currentMagicSpells = eucladianMagicSpells.Select(entry => (entry.Key, entry.Value.damage, entry.Value.manaRequirement)).ToList();
                                break;
                            default:
                                Console.WriteLine("Unknown magic speciality.");
                                Environment.Exit(0);
                                break;
                        }


                        int spellIndex = 0; // Keep track of index within array

                        for (int spellNumber = 0; spellNumber < 2; spellNumber++)
                        {
                            Console.WriteLine($"Choose magic spell #{spellNumber + 1} for {magicSpecialties[specialityIndex]}:");
                            int magicSpellChoice;

                            string input = Console.ReadLine(); // Prompt for input inside the loop
                            while (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out magicSpellChoice) || magicSpellChoice < 1 || magicSpellChoice > currentMagicSpells.Count) // Mitigating empty or invalid input
                            {
                                Console.WriteLine($"Invalid choice. Please enter a valid number corresponding to the magic specialty (1-{currentMagicSpells.Count}).");
                                input = Console.ReadLine(); // Prompt again for input
                            }

                            // Add the chosen magic spell to magicSpells
                            magicSpells.Add(currentMagicSpells[magicSpellChoice - 1]);
                        }
                    }

                    Console.Clear(); // Neatness
                    Console.ForegroundColor = ConsoleColor.White; // Reset the console color

                    int mageSpecialAtkRecharge = 0; // Preset
                    bool mageCombat = false;

                    Mage mage = new Mage(mageName, mageStaff, magicSpecialties.ToArray(), arcaniaGoldCoins, magicSpells, mageInventory, mageSpecialAtkRecharge, mageClassNpcsEncountered, mageCombat);
                    DisplayMageDetails(); // Proceed to the function via function call to display Mage's details
                    break;


                    void DisplayMageDetails()
                    {
                        smoothPrinting.PrintLine("---------Mage Status----------\n"); // Display the users status (i.e. their chosen attack types, weapon etc.)
                        smoothPrinting.FastPrint($"Mage Name: {mageName} \nMage's Weapon Type: {randomWeapon.weaponType} \nMage's Weapon: {randomWeapon.weaponName}, Damage: {randomWeapon.damage}, Rarity: {randomWeapon.rarity}");
                        smoothPrinting.FastPrint("\nMage's Magic Specialities: " + string.Join(", ", magicSpecialties));

                        Console.WriteLine(); // Space the properties for neatness

                        // Display users chosen spells
                        smoothPrinting.FastPrint("\n---------Mage's Chosen Spells---------");

                        foreach (var chosenSpell in magicSpells)
                        {
                            smoothPrinting.RapidPrint($"\n * {chosenSpell.magicSpell}: Damage - {chosenSpell.damage}, Mana Requirement - {chosenSpell.manaRequirement}");
                        };

                        Console.WriteLine(); // Seperate lines
                        smoothPrinting.CenterPrint("\nWould you like to embark on your journey in the world of Arcania?");
                        smoothPrinting.RapidPrint("\nEnter the following value, to be directed");
                        Console.WriteLine(); // Seperate lines
                        smoothPrinting.RapidPrint("1 - Start your adventure");
                        smoothPrinting.RapidPrint("\n2 - Return to class selection");
                        smoothPrinting.RapidPrint("\n3 - Return to the Menu");
                        Console.WriteLine(); // Seperate lines
                        smoothPrinting.RapidPrint("\nEnter a corresponding value: ");
                        startMageJourneyInput = Console.ReadLine(); // Register the user input


                        switch (startMageJourneyInput)
                        {
                            case "1":
                                Console.Clear(); // Neatness
                                smoothPrinting.FastPrint("First scenario\n");
                                Console.WriteLine("You will now be sent to the world of Arcania, make sure to not die.");
                                Console.Clear(); // Neatness
                                Console.ForegroundColor = ConsoleColor.White; // Reset the console colour
                                ForestOfMysteries mageJourney = new ForestOfMysteries(); // Journey start!
                                mageJourney.forestOfMysteries(mage);
                                break;
                            case "2":
                                startMageJourneyInput = null;
                                userChoice = null;
                                mageName = null;
                                mageStaff = null;
                                magicSpecialties = null;
                                magicSpells = null; // Clear all parameters from their initial values
                                mageInventory = null;
                                smoothPrinting.FastPrint("\nYou will now be redirected to the class selection screen...");
                                Console.ForegroundColor = ConsoleColor.White; // Reset console color
                                Console.Clear(); // Clear the console to prevent confusion + cleaner look
                                userClass(); // Redirect user to select a different class...
                                break;
                            case "3":
                                startMageJourneyInput = null;
                                userChoice = null;
                                mageName = null;
                                mageStaff = null;
                                magicSpecialties = null;
                                magicSpells = null; // Clear all parameters from their initial values
                                mageInventory = null;
                                GameMenu menu = new GameMenu();
                                smoothPrinting.FastPrint("\nYou will now be directed to the game menu...");
                                Console.ForegroundColor = ConsoleColor.White; // Reset console color
                                Console.Clear(); // Clear the console to prevent confusion + cleaner look
                                menu.gameMenu(); // Redirect user back to the the game menu
                                break;
                            default:
                                Console.WriteLine("Invalid input, ensure that you enter the correct value (i.e. the value '1').");
                                Console.ReadKey();
                                Console.Clear();
                                DisplayMageDetails();
                                break;

                        }
                    }

                case "2":
                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.Red;
                    smoothPrinting.RapidPrint("\nKnight's aren't avaliable as of present :3");
                    Console.ReadKey();
                    break;


                case "3":
                    int startPirateJourneyInput;
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    int specialAtkRecharge = 0; // This remains fixed
                    List<(string npcName, string npcDescription, string npcAffiliation)> pirateClassNpcsEncountered = null; // During this stage of the game, the MC will have not encountered any NPC's therefore this value remains null

                    smoothPrinting.PrintLine("--------------------------------------------------");
                    smoothPrinting.PrintLine("FantasyRPG: " + "Pirate Class");
                    smoothPrinting.PrintLine("--------------------------------------------------\n");
                    // Take users name
                    smoothPrinting.RapidPrint("\nWhat is your name, adventurer? ");

                    string pirateName = Convert.ToString(Console.ReadLine());
                    Console.Clear();

                    Dictionary<string, (int damage, string weaponType, string rarity, string weaponDescription, string category, int quantity)> pirateWeaponChoices = new Dictionary<string, (int, string, string, string, string, int)>()
                    {
                        { "Sharp Cutlass", (6, "Sword", "Common", "A short, nimble sword ideal for quick strikes.", "Melee", 1) },
                        { "Raging Horn", (8, "Longsword", "Common", "A curved longsword evoking power, perfect for forceful attacks.", "Melee", 1) },
                        { "Somali Pride", (11, "Sword", "Uncommon", "A rare sword of Somali origin, passed down through generations.", "Melee", 1) },
                        { "Mohamad's Dagger", (20, "Dagger", "Rare", "A concealable dagger named after a famous pirate, perfect for surprise attacks.", "Melee", 1) },
                        { "Dilapidated Thorn", (14, "Katana", "Rare", "A worn katana with a sharp edge, nicknamed for its piercing ability.", "Melee", 1) }
                    };

                    // Auras give damage bonuses on attacks
                    Dictionary<string, (int damage, string rarity, string auraDescription)> pirateWeaponAuras = new Dictionary<string, (int, string, string)>()
                    {
                        { "Bloodlust", (3, "Rare", "Embrace your inner rage and strike fear into your enemies' hearts.") },
                        { "Kraken's Pride", (4, "Rare", "Channel the power of the legendary Kraken, striking with unmatched ferocity.") },
                        { "Mystical Remenance", (8, "Unique", "Harness the forgotten magic of the ancients, wielding arcane energy with devastating effect.") },
                        { "Wraiths's Omen", (2, "Uncommon", "Command the chilling presence of the spectral realm, striking fear and reaping bonus rewards from fallen foes.") },
                        { "Devious Sigma Pirate", (20, "Legendary", "Unleash the cunning and ruthlessness of the Sigma Pirate legend, your attacks imbued with an aura of tactical brilliance.") },
                        { "Somalia's Exudance", (12, "Unique", "Tap into the vibrant energy of Somalia, bolstering your resilience and striking with invigorating fervor.") },
                        { "Eucladian's Myst", (25, "Legendary", "Relish in it's power and strike fear into your enemies hearts! Eucladian's authority will ensure that no one stands in your way!") }
                    };

                    // Dictionary for storing pirate's normal attack choices
                    Dictionary<string, (int damage, int manaRequirement, string elementType, string description)> pirateNormalAttackChoices = new Dictionary<string, (int, int, string, string)>()
                    {
                        // Single target attacks
                        { "Riposte", (6, 20, "Physical", "Parry and counter with a swift strike.") },
                        { "Savage Flurry", (10, 30, "Lightning", "Unleash a relentless series of slashes, imbuing your blade with lightning for each hit.") },
                        { "Piercing Thrust", (8, 25, "Ice", "Aim for a gap and deliver a high-precision stab, infused with frost to slow your opponent.") },
                        { "Whirlwind Strike", (7, 20, "Fire", "Spin, deflecting attacks and damaging nearby enemies with a fiery whirlwind.") },
                    };

                    // Dictionary for storing pirate's special attack choices
                    Dictionary<string, (int damage, int manaRequirement, string elementType, string description)> pirateSpecialAttackChoices = new Dictionary<string, (int, int, string, string)>()
                    {
                        // Single-target attacks
                        { "Blazing Cut", (15, 50, "Fire", "Unleash a fiery slash, dealing high damage and burning your opponent.") },
                        { "Icy Impale", (12, 40, "Ice", "Pierce your enemy with an ice-infused blade, slowing their movement and dealing moderate damage.") },

                        // Unique and powerful attacks
                        { "Shadow Blade", (25, 80, "Dark", "Forges a blade of pure darkness that cuts through defenses and inflicts grievous wounds." ) },
                        { "Thunderous Fury", (20, 65, "Lightning", "Channel a powerful lightning bolt, dealing massive damage but leaving you vulnerable.") },
                        { "Tidal Wave", (18, 60, "Water", "Summon a wave of water, pushing back and damaging all enemies in its path.") },
                        { "Eucladian Cleave", (30, 90, "Eucladian", "Unleash a reality-bending slash, ignoring enemy defenses and dealing high damage.") },
                    };

                    // Convert dictionaries to arrays of strings for display
                    string[] pirateNormalAttackChoicesKeys = pirateNormalAttackChoices.Keys.ToArray();
                    string[] pirateSpecialAttackChoicesKeys = pirateSpecialAttackChoices.Keys.ToArray();

                    // List to store chosen pirate special attacks
                    List<string> chosenPirateSpecialAttacks = new List<string>();

                    // Counter variables for output tracking
                    int normalAttackChoiceCount = 0;
                    int specialAttackChoiceCount = 0;
                    // int chosenNormalAttackCount = 0;


                    smoothPrinting.PrintLine("---------Normal Attack Selection----------\n");

                    // Display pirate's normal attack choices
                    foreach (var normalAttackChoice in pirateNormalAttackChoices)
                    {
                        smoothPrinting.RapidPrint($"\n{normalAttackChoiceCount + 1}. {normalAttackChoice.Key} - Damage: {normalAttackChoice.Value.damage}, Mana Requirement for Activation: {normalAttackChoice.Value.manaRequirement}, Element Type: {normalAttackChoice.Value.elementType} \nDescription: {normalAttackChoice.Value.description}\n");
                        normalAttackChoiceCount++;
                    }

                    // Store selected normal attacks with all details
                    List<(string attack, int damage, int manaRequirement, string elementType, string description)> chosenPirateNormalAttacks = new List<(string, int, int, string, string)>();

                    for (int normalAttackChoiceIndex = 0; normalAttackChoiceIndex < 2; normalAttackChoiceIndex++)
                    {
                        Console.WriteLine($"\nSelect #{normalAttackChoiceIndex + 1} normal attack (1-4 for each move choice):");

                        // Prompt user for input
                        smoothPrinting.FastPrint("Enter the number of the attack: ");
                        if (int.TryParse(Console.ReadLine().Trim(), out int selectedAttackNumber))
                        {
                            // Check if the entered number corresponds to a valid attack
                            if (selectedAttackNumber >= 1 && selectedAttackNumber <= pirateNormalAttackChoices.Count)
                            {
                                string[] attackKeys = pirateNormalAttackChoices.Keys.ToArray();
                                string selectedNormalAttackKey = attackKeys[selectedAttackNumber - 1];

                                var normalAttackDetails = pirateNormalAttackChoices[selectedNormalAttackKey];
                                chosenPirateNormalAttacks.Add((selectedNormalAttackKey, normalAttackDetails.damage, normalAttackDetails.manaRequirement, normalAttackDetails.elementType, normalAttackDetails.description));
                            }
                            else
                            {
                                Console.WriteLine("Invalid attack number. Please enter a number corresponding to the provided options.");
                                normalAttackChoiceIndex--; // Decrement to re-ask for the current attack slot
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a valid number.");
                            normalAttackChoiceIndex--; // Decrement to re-ask for the current attack slot
                        }
                    }

                    Console.Clear(); // Cleaning the console for neatness

                    // Display selected normal attacks with all details
                    smoothPrinting.PrintLine("---------Selected Normal Attacks----------\n");
                    foreach (var attack in chosenPirateNormalAttacks)
                    {
                        smoothPrinting.RapidPrint($"* {attack.attack}, Damage: {attack.damage}, Mana Requirement: {attack.manaRequirement}, Element Type: {attack.elementType}\nDescription: {attack.description}");
                        Console.WriteLine("\n"); // Neat structure for displaying selected normal attacks
                    }

                    Console.WriteLine("Affirmative? Press any key to continue.");
                    Console.ReadKey(); // Allow the user to check before proceeding into selecting special attack choices

                    Console.Clear(); // Neatness

                    smoothPrinting.PrintLine("---------Special Attack Selection----------\n");
                    foreach (var specialAtkChoices in pirateSpecialAttackChoices) // Display the normal attack choices to the user with other associated values
                    {
                        smoothPrinting.RapidPrint($"\n{specialAttackChoiceCount + 1}. {specialAtkChoices.Key} - Damage: {specialAtkChoices.Value.Item1}, Mana Requirement for Activation: {specialAtkChoices.Value.Item2}, Element Type: {specialAtkChoices.Value.Item3} \nDescription: {specialAtkChoices.Value.Item4}\n");
                        specialAttackChoiceCount++;
                    }

                    List<(string attack, int damage, int manaRequirement, string elementType, string description)> chosenSpecialAttacks = new List<(string, int, int, string, string)>();

                    for (int specialChoiceIndex = 0; specialChoiceIndex < 1; specialChoiceIndex++)
                    {
                        Console.WriteLine($"\nSelect #{specialChoiceIndex + 1} normal attack (1-6 for each move choice):");

                        // Prompt user for input
                        smoothPrinting.FastPrint("Enter the number of the attack: ");
                        if (int.TryParse(Console.ReadLine().Trim(), out int selectedSpecialAtkNumber))
                        {
                            // Check if the entered number corresponds to a valid attack
                            if (selectedSpecialAtkNumber >= 1 && selectedSpecialAtkNumber <= pirateSpecialAttackChoices.Count)
                            {
                                string[] attackKeys = pirateSpecialAttackChoices.Keys.ToArray();
                                string selectedSpecialAttackKey = attackKeys[selectedSpecialAtkNumber - 1];

                                var specialAttackDetails = pirateSpecialAttackChoices[selectedSpecialAttackKey];
                                chosenSpecialAttacks.Add((selectedSpecialAttackKey, specialAttackDetails.damage, specialAttackDetails.manaRequirement, specialAttackDetails.elementType, specialAttackDetails.description));
                            }
                            else
                            {
                                Console.WriteLine("Invalid attack number. Please enter a number corresponding to the provided options.");
                                specialChoiceIndex--; // Decrement to re-ask for the current attack slot
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a valid number.");
                            specialChoiceIndex--; // Decrement to re-ask for the current attack slot
                        }
                    }

                    Console.Clear(); // Neatness

                    // Display selected special attacks with all details
                    smoothPrinting.PrintLine("---------Selected Normal Attacks----------\n");
                    foreach (var attack in chosenSpecialAttacks)
                    {
                        smoothPrinting.RapidPrint($"* {attack.attack}, Damage: {attack.damage}, Mana Requirement: {attack.manaRequirement}, Element Type: {attack.elementType}\nDescription: {attack.description}");
                        Console.WriteLine("\n"); // Neat structure for displaying selected normal attacks
                    }


                    Console.WriteLine("Affirmative? Press any key to continue.");
                    Console.ReadKey(); // Allow the user to check before proceeding into selecting special attack choices

                    Console.Clear(); // Neatness


                    // Future debugging
                    // smoothPrinting.CenterPrint("---------Displaying Starter Weapons----------\n"); // Display the starter weapons to the user

                    // foreach (var weapon in pirateWeaponChoices) // Display starter weapons
                    // {
                    //     smoothPrinting.RapidPrint($"\n{weapon.Key} - Damage: {weapon.Value.damage} - Weapon Type: {weapon.Value.weaponType}, Item Rarity: {weapon.Value.rarity}\nWeapon Description: {weapon.Value.weaponDescription}\n");
                    // }

                    // smoothPrinting.FastPrint("\nIf everything is clear, press any key to continue to the auras.");
                    // Console.ReadKey();
                    // Console.Clear(); // Clear the console to display the auras, since displaying both weapons and auras at once takes too much space

                    // smoothPrinting.CenterPrint("---------Displaying Auras----------\n"); // Display the auras to the user

                    // foreach (var weaponAura in pirateWeaponAuras)  
                    // {
                    //     smoothPrinting.RapidPrint($"\n{weaponAura.Key} - Damage: {weaponAura.Value.damage}, Rarity: {weaponAura.Value.rarity}\nAura Description: {weaponAura.Value.auraDescription}\n");
                    // }

                    smoothPrinting.PrintLine("---------Random Selection (Pirate Class)----------\n");
                    smoothPrinting.FastPrint("Weapon will be randomly assigned...\n");
                    smoothPrinting.FastPrint("Aura will be randomly assigned...");

                    Console.WriteLine("\n"); // Structuring
                    smoothPrinting.FastPrint("\nWould you like to continue? ");
                    Console.ReadKey(); // Allow user to read contents

                    arcaniaGoldCoins = 0; // Preset zero

                    List<(string itemName, string itemDescription, string itemRarity, int itemPower, string category, int quantity)> pirateInventory = new List<(string, string, string, int, string, int)>();

                    // User will be randomly assigned a weapon
                    Random weaponPirateRandom = new Random();
                    int pirateRandomWeaponAssignment = weaponPirateRandom.Next(0, pirateWeaponChoices.Count); // Allow for the random generation between index 0 and length of the dictionary

                    List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription, string category, int quantity)> pirateWeapon = new List<(string, int, string, string, string, string, int)>(); // This list will store the assigned weapon for the pirate class

                    // Access the weapon details directly using the index
                    var randomPirateWeapon = pirateWeaponChoices.ElementAt(pirateRandomWeaponAssignment);

                    // Add the weapon details to the pirateWeapon list
                    pirateWeapon.Add((randomPirateWeapon.Key, randomPirateWeapon.Value.damage, randomPirateWeapon.Value.weaponType, randomPirateWeapon.Value.rarity, randomPirateWeapon.Value.weaponDescription, randomPirateWeapon.Value.category, randomPirateWeapon.Value.quantity));

                    // Add the weapon name to the pirateInventory list
                    pirateInventory.Add((randomPirateWeapon.Key, randomPirateWeapon.Value.weaponDescription, randomPirateWeapon.Value.rarity, randomPirateWeapon.Value.damage, randomPirateWeapon.Value.category, randomPirateWeapon.Value.quantity)); ;


                    // User will be randomly assigned an aura
                    Random auraPirateRandom = new Random();
                    int pirateAuraRoll = auraPirateRandom.Next(0, pirateWeaponAuras.Count); // Allow for the random generation between index 0 and length of the dictionary

                    List<(string auraName, int damage, string rarity, string description)> pirateWeaponAura = new List<(string auraName, int damage, string rarity, string description)>(); // Aura will be stored in a list

                    var randomAura = pirateWeaponAuras.ElementAt(pirateAuraRoll); // Assign a random aura from the dictionary

                    // Add the random aura to the list
                    pirateWeaponAura.Add((randomAura.Key, randomAura.Value.damage, randomAura.Value.rarity, randomAura.Value.auraDescription));

                    bool pirateCombat = false;

                    SomaliPirate myPirate = new SomaliPirate(pirateName, pirateWeapon, pirateWeaponAura, chosenPirateNormalAttacks, chosenSpecialAttacks, pirateInventory, arcaniaGoldCoins, specialAtkRecharge, pirateClassNpcsEncountered, pirateCombat); // Generate the pirate details

                    Console.Clear(); // Neater


                    // Display information to the user
                    smoothPrinting.PrintLine("---------Pirate Status----------\n"); // Display the users status (i.e. their chosen attack types, weapon etc.)
                    smoothPrinting.FastPrint($"Pirate's Name: {pirateName} \nPirate's Weapon Type: {randomPirateWeapon.Value.weaponType} \nPirate's Weapon: {randomPirateWeapon.Key}, Damage: {randomPirateWeapon.Value.damage}, Rarity: {randomPirateWeapon.Value.rarity} \nPirate's Aura: {randomAura.Key}, Damage: {randomAura.Value.damage}, Rarity: {randomAura.Value.rarity}");

                    Console.WriteLine(); // Seperate lines
                    smoothPrinting.PrintLine("\n---------Chosen Normal Attacks----------"); // Display the users chosen normal attack skills

                    foreach (var chosenNormalAttack in chosenPirateNormalAttacks) // Display all chosen normal attacks moves of the user
                    {
                        smoothPrinting.RapidPrint($"\n* {chosenNormalAttack.attack}: Damage - {chosenNormalAttack.damage}, Mana Requirement - {chosenNormalAttack.manaRequirement}, Element Type - {chosenNormalAttack.elementType} \nDescription: {chosenNormalAttack.description}");
                        Console.WriteLine(); // Neat structuring
                    };

                    Console.WriteLine(); // Seperate lines
                    smoothPrinting.PrintLine("\n---------Chosen Special Attack----------"); // Display the users special attack skill

                    foreach (var chosenSpecialAttack in chosenSpecialAttacks) // Display all chosen special attacks moves of the user
                    {
                        smoothPrinting.RapidPrint($"\n* {chosenSpecialAttack.attack}: Damage - {chosenSpecialAttack.damage}, Mana Requirement - {chosenSpecialAttack.manaRequirement}, Element Type - {chosenSpecialAttack.elementType} \nDescription: {chosenSpecialAttack.description}");
                        Console.WriteLine(); // Neat structuring
                    };

                    Console.WriteLine(); // Seperate lines
                    smoothPrinting.CenterPrint("\nWould you like to embark on your journey in the world of Arcania?");
                    Console.WriteLine("\nEnter the following value, to be directed\n");
                    Console.WriteLine("1: Start your adventure");
                    Console.WriteLine("2: Return to class selection");
                    Console.WriteLine("3: Return to the Menu");

                    startPirateJourneyInput = Convert.ToInt32(Console.ReadLine()); // Register the user input

                    // Future reference: For each class chosen, make a seperate method for them
                    switch (startPirateJourneyInput)
                    {
                        case 1:
                            Console.Clear(); // Neatness
                            Console.WriteLine("You will now be sent to the world of Arcania, make sure to not die.");
                            Console.ForegroundColor = ConsoleColor.White; // Reset the console colour
                            Console.Clear(); // Neatness
                            ForestOfMysteries pirateJourney = new ForestOfMysteries();
                            pirateJourney.forestOfMysteries(myPirate);
                            break;
                        case 2:
                            userChoice = null;
                            pirateName = null;
                            pirateWeapon = null;
                            pirateWeaponAura = null;
                            chosenPirateNormalAttacks = null; // Clear all parameters from their initial values
                            chosenSpecialAttacks = null;
                            pirateInventory = null;
                            smoothPrinting.FastPrint("\nYou will now be redirected to the class selection screen...");
                            Console.Clear(); // Clear the console to prevent confusion + cleaner look
                            userClass(); // Redirect user back to class selection
                            break;
                        case 3:
                            userChoice = null;
                            pirateName = null;
                            pirateWeapon = null;
                            pirateWeaponAura = null;
                            chosenPirateNormalAttacks = null; // Clear all parameters from their initial values
                            chosenSpecialAttacks = null;
                            pirateInventory = null;
                            smoothPrinting.FastPrint("\nYou will now be redirected to the game menu...");
                            GameMenu menu = new GameMenu();
                            Console.Clear(); // Clear the console to prevent confusion + cleaner look
                            menu.gameMenu(); // Redirect user back to the the game menu
                            break;
                        default:
                            Console.WriteLine("Invalid input, please input a sensible value again.");
                            break;

                    }
                    break;


                case "4":
                    Console.WriteLine("After long endurance of physical training, you develop eyes as keen as an owl and your bowmanship is first class.");
                    Console.WriteLine("What is your name?");
                    string archerName = Convert.ToString(Console.ReadLine());

                    break;
                case "5":
                    Console.ForegroundColor = ConsoleColor.Red; // devious colour hahahaha
                    Console.WriteLine("I'm in your walls :3");
                    Console.WriteLine("You died");
                    Console.ReadLine();

                    break;
                case "6":
                    Console.ForegroundColor = ConsoleColor.Red; // devious colour hahahaha
                    Console.WriteLine("You are not :3, therefore you are not worthy of becoming a devious sigma");
                    Console.WriteLine("You died");
                    Console.ReadLine();

                    break;
                case "7":
                    GameMenu redirectUserToMenu = new GameMenu();
                    smoothPrinting.FastPrint("You will now be redirected back to the menu....\n");
                    Console.Clear(); // Neater
                    redirectUserToMenu.gameMenu();
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.Red; // devious colour hahahaha
                    Console.WriteLine("Invalid input, please try again!");
                    Console.Clear(); // Clear the console
                    userClass(); // Recurse and load the story + selection of classes
                    break;
            }

        }

    }



    public class ForestOfMysteries // Once the user selects a class, they'll be directed to the Forest of Mysteries, located within Arcania, the most well known forest in the continent of Tenebris.
    {

        // Customary scenarios will be used to allow dynamicness to the game and make it less monotomous
        // string[] customaryScenarios = { "You embark on a long journey, you find yourself lost midway throughout the journey. There appears a dragon, with fangs as sharp as blades and a gaze so intense that you begin to question your fighting prowess despite your training. What do you do?" };

        // User first encounters the dragon in the forest of mysteries.
        string oneTimeFixedScenario = "\nYou spawn in the Forest of Mysteries, now with the understanding of the rulings within the world along with a primitive understanding of mana. As you keep exploring this vast forest, you eventually find yourself lost midway, your eyes surrounded by vast levels of fog, mitigating your view of the perspective ahead. Close by, there appears a dragon, with fangs as sharp as blades and a gaze so intense that you begin to question whether you’ll survive or not. ";
        bool completedFirstScenario = false; // This is a measure to see if the user has completed this scenario in the Forest of Mysteries, should this be the case, they'll no longer see this scenario when exploring the forest
        string? firstSelection;

        private readonly UIManager UI;
        private readonly SmoothConsole smoothPrinting;


        public ForestOfMysteries()
        {
            UI = new UIManager();
            smoothPrinting = new SmoothConsole();
        }

        public void forestOfMysteries(CharacterDefault character, int remainingAttempts = 3)
        {
            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine("FantasyRPG: Forest of Mysteries");
            smoothPrinting.PrintLine("--------------------------------------------------");


            if (completedFirstScenario == false)
            {
                if (remainingAttempts == 3)
                {
                    smoothPrinting.RapidPrint(oneTimeFixedScenario + "\n");
                }
                else
                {
                    Console.WriteLine(oneTimeFixedScenario + "\n");
                }
            }


            Console.WriteLine("\n[Available Commands:]");
            smoothPrinting.PrintLine("\n1. Approach: Get closer to the Dragon");
            smoothPrinting.PrintLine("\n2. North: Move northward along the path (N/A)");
            smoothPrinting.PrintLine("\n3. Inventory: View your current inventory of items");
            smoothPrinting.PrintLine("\n4. Help: Display a list of available commands");
            smoothPrinting.RapidPrint("\nEnter a corresponding value: ");
            string firstSelection = Convert.ToString(Console.ReadLine());

            try
            {
                switch (firstSelection)
                {
                    case "1":
                        DragonConfrontation(character);
                        break;
                    case "2":
                        // Move northward
                        NorthDirection(character);
                        break;
                    case "3":
                        character.CheckInventory();
                        UI.PromptUserToContinue();
                        forestOfMysteries(character, remainingAttempts - 1); // Recurse to avoid breaking program haha
                        break;
                    case "4":
                        smoothPrinting.PrintLine("--------------------------------------------------");
                        smoothPrinting.PrintLine("FantasyRPG: Available Commands");
                        smoothPrinting.PrintLine("--------------------------------------------------");

                        smoothPrinting.RapidPrint("\nEnter the value '1' if you want to get closer to the dragon\n");
                        smoothPrinting.RapidPrint("\nEnter the value '2' if you want to go northwards\n");
                        smoothPrinting.RapidPrint("\nEnter the value '3' to check your inventory");
                        Console.WriteLine(); // Spacing
                        UI.PromptUserToContinue();
                        forestOfMysteries(character, remainingAttempts); // Return the user back to the scenario
                        break;
                    default:
                        Console.WriteLine("\nInvalid input, please try again");
                        Console.ReadKey();
                        Console.Clear();
                        forestOfMysteries(character, remainingAttempts - 1);
                        break;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("\nThere has been an error, error code: " + ex.Message);
            }

        }


        public void NorthDirection(CharacterDefault character)
        {
            string? userInput;
            bool meetingGuildLeader = false; // Should an overgrown boar spawn, then this will be 'true' as this is part of a mission

            Random random = new Random();
            int generatedValue = random.Next(1, 4); // Increase range to account for additional mob types

            if (generatedValue == 1)
            {
                bool inCombat = true;

                smoothPrinting.PrintLine("--------------------------------------------------");
                smoothPrinting.PrintLine("FantasyRPG: Wolf mob encounter!");
                smoothPrinting.PrintLine("--------------------------------------------------");

                smoothPrinting.RapidPrint("\nYou have encountered a wild wolf, one that is showing hostility, you have no choice but to fight to the battle of death!");

                Console.WriteLine(); // Spacing
                UI.PromptUserToContinue();

                // Create an instance of the Wolf class (or whatever represents your wolf mob)

                Dictionary<string, (int damage, string magicType)> normalAtkNames = new Dictionary<string, (int damage, string magicType)>()
                {
                    {"Scratch", (5, "Normal")},
                    {"Bite", (8, "Normal")},
                    {"Hounding Tempest", (10, "Wind-Magic")}
                };

                Dictionary<string, (int damage, string magicType)> specialAtkNames = new Dictionary<string, (int damage, string magicType)>()
                {
                    { "Pack Ambush", (25, "Normal")},
                    { "Howl", (20, "Sound-Magic")},
                    { "Bloody Rage", (15, "Fire-Magic") }
                };

                Dictionary<string, (int damage, string rarity, string weaponType, string weaponDescription, string category, int quantity)> itemDrop = new Dictionary<string, (int damage, string rarity, string weaponType, string weaponDescription, string category, int quantity)>()
                {
                    { "Sharp Fang", (20, "Common", "Sword", "A sharp fang torn from the jaws of a wolf, suitable for close combat.", "Weapon", 1) },
                    { "Howler's Claw", (25, "Rare", "Rapier", "A claw imbued with the power of the alpha wolf, capable of rending through armor.", "Weapon", 1) },
                };

                MobType Wolf = new MobType();

                Wolf wolf = new Wolf("Wolf", 30, 30, normalAtkNames, specialAtkNames, 0, itemDrop, 25, 3, 3); // Assuming you have a Wolf class defined

                // Create a MobSpawner object passing the player character and the wolf instance
                MobSpawner mobSpawner = new MobSpawner(character, Wolf); // Pass null or remove the unnecessary parameter


                // Spawn the wolf using MobSpawn method
                mobSpawner.MobSpawn(character, wolf);



            }

            Console.Clear();
            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine("FantasyRPG: Forest of Mysteries - North");
            smoothPrinting.PrintLine("--------------------------------------------------");

            smoothPrinting.RapidPrint("\nYou venture northward into the dense forest, where towering trees cast shifting patterns of light and shadow. The rustle of leaves underfoot and the distant calls of wildlife fill the air. " +
                "Among the trees, you catch glimpses of wolves, bears, and other creatures, each adding to the allure of the wilderness. The cool breeze carries whispers of ancient tales, fueling your resolve to unravel the mysteries that lie ahead.");

            smoothPrinting.RapidPrint("\nWhat do you do?\n");

            Console.WriteLine("\n[Available Commands:]");
            smoothPrinting.PrintLine("\n1. Fight: Take on a random mob within the forest");
            smoothPrinting.PrintLine("\n2. Explore: Search the area for hidden treasures or clues");
            smoothPrinting.PrintLine("\n3. Inventory: View your current inventory of items");
            smoothPrinting.RapidPrint("\nEnter a corresponding value: ");
            userInput = Console.ReadLine();

            try
            {
                switch (userInput)
                {
                    case "1":
                        // TESTING CASE: WILL BE CHANGED TO RANDOM CASE, TO SPAWN RANDOM MOBS, AND ALLOW USER TO FARM EXP IN THE AREA

                        smoothPrinting.PrintLine("--------------------------------------------------");
                        smoothPrinting.PrintLine("FantasyRPG: Boar Encounter!");
                        smoothPrinting.PrintLine("--------------------------------------------------");

                        smoothPrinting.RapidPrint("\nYou have encountered a wild boar, one that is showing hostility, you have no choice but to fight to the battle of death!");

                        Console.WriteLine(); // Spacing
                        UI.PromptUserToContinue();

                        // Create an instance of the Bore class 
                        Dictionary<string, (int damage, string magicType)> normalAtkNames = new Dictionary<string, (int damage, string magicType)>()
                        {
                            {"Tusk Swipe", (25, "Physical")},
                            {"Feral Charge", (30, "Physical")},
                            {"Mud Slam", (20, "Earth-Magic")}
                        };

                        Dictionary<string, (int damage, string magicType)> specialAtkNames = new Dictionary<string, (int damage, string magicType)>()
                        {
                            {"Rampaging Roar", (50, "Sound-Magic")},
                            {"Earthquake Stomp", (60, "Earth-Magic")},
                            {"Inferno Charge", (70, "Fire-Magic")}
                        };

                        Dictionary<string, (int damage, string rarity, string weaponType, string weaponDescription, string category, int quantity)> itemDrop = new Dictionary<string, (int damage, string rarity, string weaponType, string weaponDescription, string category, int quantity)>()
                        {
                            { "Razor Tusk", (20, "Common", "Sword", "A sharp tusk capable of tearing through armor.", "Weapon", 1) },
                            { "Behemoth's Hoof", (30, "Rare", "Hammer", "A massive hoof that belonged to a legendary bore, imbued with immense strength.", "Weapon", 1) },
                        };

                        MobType Boar = new MobType();

                        MobSpawner mobspawn = new MobSpawner(character, Boar);

                        Boar boarSpawn = new Boar("Boar", 50, 50, normalAtkNames, specialAtkNames, 0, itemDrop, 35, 5, 6);

                        mobspawn.MobSpawn(character, boarSpawn);

                        break;
                    case "2":

                        break;
                    case "3":
                        character.CheckInventory();
                        UI.PromptUserToContinue();
                        NorthDirection(character); // Recurse
                        break;
                    default:
                        Console.WriteLine("\nInvalid input, please try again");
                        Console.ReadKey();
                        Console.Clear();
                        NorthDirection(character);
                        break;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("\nThere has been an error, error code: " + ex.Message);
            }


        }

        void DragonConfrontation(CharacterDefault character)
        {
            string? userDecision;
            SmoothConsole smoothPrinting = new SmoothConsole();
            Console.Clear();
            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine("FantasyRPG: Forest of Mysteries - Approaching the Dragon");
            smoothPrinting.PrintLine("--------------------------------------------------");

            smoothPrinting.RapidPrint("\nAs your curiosity overwhelms you, you inch closer to the colossal Dragon, its scales gleaming in the sunlight.");
            smoothPrinting.RapidPrint("\nTo avoid detection, you stealthily maneuver through the dense foliage, blending into the shadows as you track the Dragon's every movement.");
            smoothPrinting.RapidPrint(" Heart pounding, you crouch behind the thick bushes, feeling the earth tremble beneath you with each beat of the Dragon's wings.\n\n");
            smoothPrinting.RapidPrint("With bated breath, you watch as the Dragon takes flight, its majestic form soaring into the vast expanse of the sky.");
            smoothPrinting.RapidPrint(" Eager to uncover its secrets, you remain hidden, determined to unveil the mysteries that lie beyond the horizon.");
            smoothPrinting.RapidPrint(" As you keep approaching closer to the dragon, you accidentally slip, and a noticeable thud can be heard from the nearby forest dwellers.\n");

            smoothPrinting.RapidPrint("\nWhat do you do?\n");

            smoothPrinting.RapidPrint("\n1. Keep Exploring: Stay hidden in the shadows and carefully study the dragon's behavior\n");
            smoothPrinting.RapidPrint("\n2. Retreat: Quietly retreat to a safer distance and return back to your original position\n");
            smoothPrinting.RapidPrint("\n3. Combat: Ready your weapon and prepare to engage the dragon in combat\n");

            smoothPrinting.RapidPrint("\nEnter a corresponding value: ");
            userDecision = Console.ReadLine(); // Register the user input

            switch (userDecision)
            {
                case "1":
                    Console.Clear();

                    smoothPrinting.PrintLine("--------------------------------------------------");
                    smoothPrinting.PrintLine("FantasyRPG: Forest of Mysteries - Approaching the Dragon");
                    smoothPrinting.PrintLine("--------------------------------------------------");

                    smoothPrinting.RapidPrint("\nYou remain hidden, observing the dragon's every move with intense focus. You analyze its behavior, searching for any weaknesses or patterns.");
                    smoothPrinting.RapidPrint("\nTime seems to slow as you study the dragon, hoping to gain valuable insights that could aid you in perhaps finding out why you've been summoned to the world.");

                    smoothPrinting.RapidPrint("\nYou notice that the dragon immediately starts flying away at a really fast pace, in hopes to keep up, you attempt to start running in order to close the distance and " +
                        "keep in range of the dragon.");

                    smoothPrinting.RapidPrint("\nThe dragon rapidly halts its movements, it feels as if time has frozen with how intense the situation has now become. The dragon comes down, inching every second closer to you...");
                    Console.WriteLine();
                    UI.PromptUserToContinue();
                    dragonCombat(character);

                    break;

                case "2":
                    Console.Clear();

                    smoothPrinting.PrintLine("--------------------------------------------------");
                    smoothPrinting.PrintLine("FantasyRPG: Forest of Mysteries - Retreat");
                    smoothPrinting.PrintLine("--------------------------------------------------");

                    smoothPrinting.RapidPrint("\nWith a cautious step backward, you retreat to a safer distance, allowing yourself a moment to catch your breath and collect your thoughts.");
                    smoothPrinting.RapidPrint("\nThe decision to retreat buys you precious time to reconsider your choices, tracing back the path you took, in order to get back to where you orignally were.");
                    smoothPrinting.RapidPrint("\nSlowly but surely, you finally make your way back.");
                    Console.WriteLine();
                    UI.PromptUserToContinue();

                    int remainingAttempts = 3;
                    forestOfMysteries(character, remainingAttempts); // Recurse back to the original point

                    break;
                case "3":
                    // Engage in combat with the dragon
                    smoothPrinting.RapidPrint("\nWith determination in your eyes, you ready your weapon, prepared to confront the dragon head-on in a battle for survival.");
                    smoothPrinting.RapidPrint("\nYour grip tightens around the hilt of your weapon as adrenaline courses through your veins, fueling your resolve to emerge victorious.");
                    break;

                default:
                    smoothPrinting.RapidPrint("\nYour indecision leaves you paralyzed, unable to take action. The dragon's gaze narrows, sensing your hesitation and immediately screeches, scaring all the nearby dwellers, leaving only yourself and the dragon.");
                    Console.WriteLine();
                    UI.PromptUserToContinue();
                    dragonCombat(character); // Engage combat as the user is caught
                    break;
            }



            void dragonCombat(CharacterDefault character) // A function that engages the fight between the MC and the dragon
            {
                smoothPrinting.PrintLine("--------------------------------------------------");
                smoothPrinting.PrintLine("Forest of Mysteries");
                smoothPrinting.PrintLine("--------------------------------------------------");

                // The dragon introduces itself as Windsom, the Guardian of the forests
                smoothPrinting.RapidPrint("\nDragon: \"I am Windsom, the Guardian of these forests, and you have trespassed into my domain, Mortal.\"\n");

                smoothPrinting.RapidPrint("\n*You are taken aback to hear that the Dragon can speak, though it's come to a point in time where you cannot take back your actions*\n");

                // {mage.name} expresses surprise at the dragon's name
                smoothPrinting.RapidPrint($"\n{character.name}: \"Windsom? Never heard that name before.\"\n");

                // The character ponders whether the dragon knows about their summoning
                smoothPrinting.RapidPrint("\nMC's Thoughts: \"Perhaps he knows about why I've been summoned to this world...\"\n");

                // {mage.name} directly asks the dragon about their summoning
                smoothPrinting.RapidPrint($"\n{character.name}: \"Do you know why I've been summoned to this world?\"\n");

                // The dragon responds cryptically, challenging the character to prove their worth
                smoothPrinting.RapidPrint("\nWindsom (The Guardian Dragon): \"Ah, the mysteries of summonings. Perhaps I do, perhaps I don't. But why should I reveal such knowledge to a mere mortal like you? Prove your worth, Mage. Defeat me in battle, and perhaps then, I shall consider sharing what I know.\"\n");


                smoothPrinting.RapidPrint("\n*Windsom slowly sets down into the forest, it's wings shuddering the leaves, crushing branches and any other obstacle that gets in its way, setting the stage for battle*\n");

                // Prompt the user to engage in combat
                smoothPrinting.RapidPrint("\nAre you ready to engage in combat? Press any key to start.");
                Console.ReadKey(); // Wait for user input
                Console.Clear(); // Clear the console to prepare for combat

                // Instantiate a MobType object if necessary
                MobType dragonType = new MobType();

                // Dictionary containing normal attack names and their associated damage and magic type
                Dictionary<string, (int damage, string magicType)> normalAtkNames = new Dictionary<string, (int damage, string magicType)>()
                {
                    { "Dragon's Claw", (30, "Dragon-Magic") },
                    { "Dragon's Breath", (40, "Dragon-Magic") },
                    { "Raging Tempest", (50, "Dragon-Magic") }
                };

                // Dictionary containing special attack names and their associated damage and magic type
                Dictionary<string, (int damage, string magicType)> specialAtkNames = new Dictionary<string, (int damage, string magicType)>()
                {
                    { "Arcane Nexus", (100, "Eucladian-Magic") },
                    { "Umbral Charge", (120, "Dark-Magic") },
                    { "Rampant Flame Charge", (200, "Fire-Magic") }
                };


                // Dictionary containing item names and their associated damage, rarity, weapon type, and description
                Dictionary<string, (int damage, string rarity, string weaponDescription, string weaponType, string category, int quantity)> itemDrop = new Dictionary<string, (int damage, string rarity, string weaponDescription, string weaponType, string category, int quantity)>()
                {
                    { "Frostfire Fang", (65, "Unique", "Forged in the icy flames of the dragon's breath, this fang drips with frostfire, capable of freezing enemies in their tracks.", "Staff", "Staff", 1) },
                    { "Serpent's Gaze", (50, "Unique", "Crafted from the scales of the ancient serpent, this gaze holds the power to petrify foes with a single glance.", "Rapier/Sword", "Rapier", 1) },
                    { "Chaosfire Greatsword", (60, "Unique", "Tempered in the chaosfire of the dragon's lair, this greatsword burns with an insatiable hunger for destruction.", "Greatsword/Sword", "Greatsword", 1) },
                    { "Nightshade Arc", (55, "Unique", "Fashioned from the sinew of the nocturnal shadows, this bow strikes with deadly accuracy under the cover of darkness.", "Bow", "Bow", 1) },
                    { "Aerith's Heirloom", (80, "Legendary", "Once wielded by the legendary Aerith, this staff channels the primordial magic of creation itself, capable of reshaping reality.", "Staff", "Staff", 1) },
                    { "Eucladian's Aura", (55, "Legendary", "Embrace the ethereal aura of the Eucladian, granting unmatched protection against all forms of magic and malevolence.", "Aura", "Aura", 1) }
                };


                Dragon dragon = new Dragon(
                    "Windsom",  // Name
                    350,        // Current health
                    350,        // Maximum health
                    normalAtkNames,    // Normal attack names dictionary
                    specialAtkNames,   // Special attack names dictionary
                    0,        // Special attack recharge percentage
                    itemDrop,   // Item drop dictionary
                    300,        // Experience points drop
                    1,         // Drop chance
                    25          // Mob level
                );

                // Create a MobSpawner object passing character and dragonType
                MobSpawner mobSpawner = new MobSpawner(character, dragonType);
                character.inCombat = true; // Enable combat status

                do
                {
                    // Spawn the dragon using MobSpawn method
                    mobSpawner.MobSpawn(character, dragon);

                } while (character.inCombat == true);


            }




            // string dragonName = "Windsom";
            // int specialAtkRecharge = 100;
            // int currentMobHealth = 350;
            // int maxMobHealth = 350;


            // Dictionary containing dragon attacks and their associated damage value
            // Dictionary<string, (int damage, string magicType)> normalAtkNames = new Dictionary<string, (int damage, string magicType)>() // Preset names for all dragon's normal attacks
            // {
            // {"Dragon's Claw", (30, "Dragon-Magic")},
            // {"Dragon's Breath", (40, "Dragon-Magic")},
            // {"Raging Tempest", (50, "Dragon-Magic")}
            //};

            // Dictionary containing dragon attacks and their associated damage value
            // Dictionary<string, (int, string)> specialAtkNames = new Dictionary<string, (int, string)>()
            // {
            // { "Arcane Nexus", (100, "Eucladian-Magic") },
            // { "Umbral Charge", (120, "Dark-Magic") },
            // { "Rampant Flame Charge", (200, "Fire-Magic") }
            // };

            // Dictionary that contains weapon name, damage, rarity, and weapon type (item drops)
            // Dictionary<string, (int damage, string rarity, string weaponType, string weaponDescription)> itemDrop = new Dictionary<string, (int, string, string, string)>()
            // {
            // { "Frostfire Fang", (65, "Unique", "Staff", "Forged in the icy flames of the dragon's breath, this fang drips with frostfire, capable of freezing enemies in their tracks.") },
            // { "Serpent's Gaze", (50, "Unique", "Rapier/Sword", "Crafted from the scales of the ancient serpent, this gaze holds the power to petrify foes with a single glance.") },
            // { "Chaosfire Greatsword", (60, "Unique", "Greatsword/Sword", "Tempered in the chaosfire of the dragon's lair, this greatsword burns with an insatiable hunger for destruction.") },
            // { "Nightshade Arc", (55, "Unique", "Bow", "Fashioned from the sinew of the nocturnal shadows, this bow strikes with deadly accuracy under the cover of darkness.") },
            // { "Aerith's Heirloom", (80, "Legendary", "Staff", "Once wielded by the legendary Aerith, this staff channels the primordial magic of creation itself, capable of reshaping reality.") },
            // { "Eucladian's Aura", (55, "Legendary", "Aura", "Embrace the ethereal aura of the Eucladian, granting unmatched protection against all forms of magic and malevolence.") }
            // };


            // Create a new instance of the dragon for combat
            // Dragon windsom = new Dragon(dragonName, normalAtkNames, specialAtkNames, specialAtkRecharge, currentMobHealth, maxMobHealth, itemDrop);

            // Exert pressure based on mage's level
            // windsom.exertPressure(character, windsom); // Pass mage and dragon class with relevant attributes and methods here
            // bool quickDisplay = false;

            // Engage the combat system
            // character.CombatSystem(character, windsom, quickDisplay);
        }

    }



    public class InfiniteDungeon
    {
        public void dungeon()
        {
        }
    }

    public class MagicCouncil
    {
        private readonly UIManager UI;
        private readonly SmoothConsole smoothPrinting;

        public MagicCouncil()
        {
            UI = new UIManager();
            smoothPrinting = new SmoothConsole();
        }


        // One-time executable function: Will not happen again.
        public void firstEncounter(string name, List<(string npcName, string npcDescription, string npcAffiliation)> npcsEncountered) // This will be the first meeting between the MC and the other council members (not all of them are present at that given moment)
        {
            Console.Clear(); // Clear the console for neatness
            Console.ForegroundColor = ConsoleColor.White;
            string choice1; // NPC interaction choice

            // Should I decide to make this section more interactive, then these data types will come to use
            // string choice2, choice3;


            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine("First Encounter - Arcania's Magic Council");
            smoothPrinting.PrintLine("--------------------------------------------------");

            // Explanation of Arcania's Magic Council being aware of the current situation at hand
            smoothPrinting.RapidPrint("\nThe Magic Council are naturally aware of the ongoing situation, with tension rising within Paralax (Dragon City), between the Draconith and Vesperon family and due to the growing conflict, an emergency meeting is held with all council members. Evelyn invites you as a representative along with Eurelian Frostweaver to the meeting, other council members also invite their fellow representatives. For safety purposes, all representatives are to wear a mask to conceal their identities. ");

            UI.PromptUserToContinue();

            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine("First Encounter - Arcania's Magic Council");
            smoothPrinting.PrintLine("--------------------------------------------------");

            smoothPrinting.RapidPrint("\nAs you make your way towards the Magic Council Hall alongside Evelyn, you can't help but marvel at the hyper-detailed infrastructures that tower over you. Their imposing presence commands attention, and you find yourself drawn to their intricate designs. Curious, you turn to Evelyn and inquire about the significance of these structures. With a knowing smile, she explains that they are different idols of the previous 1st rankers and generations back. Each idol holds a different weapon, symbolizing the unique specialties of its respective ranker. The idols vary in posture and exude a sense of power and mastery over their chosen disciplines. ");
            Console.WriteLine();
            UI.PromptUserToContinue();


            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine("First Encounter - Arcania's Magic Council");
            smoothPrinting.PrintLine("--------------------------------------------------");

            smoothPrinting.RapidPrint("\nWhile walking with Evelyn, you see a bulletin that contains a bunch of names, associted with a ranking and extra details and enquire to her what it exactly entails. She briefly explains that it is the ranking board for the current Arcania's Magic Council members, and lets you see from up-close\n");
            UI.PromptUserToContinue();


            Console.ForegroundColor = ConsoleColor.Yellow; // Display the rankings in a yellow color
            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine("Arcania's Magic Council - Current Rankings");
            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.RapidPrint("Rank 1: ??? - Rank: S** (Class: ???, Race: ???) \r\n\r\nRank 2: ??? - Rank: S* (Class: ???, Race: ???) \r\n\r\nRank 3: ??? - Rank: S (Class: ???, Race: ???) \r\n\r\nRank 4: Lister Everbright - Rank: A* (Class: Knight, Race: Elf) \r\n\r\nRank 5: Aurelia Eucladian-Nine - Rank: S- (Class: Mage, Race: Human) \r\n\r\nRank 6: Kaelen Stormer - Rank: S* (Class: Assassin, Race: Dark Elf) \r\n\r\nRank 7: Lyra Leywin - Rank: S- (Class: Necromancer, Race: Demon) \r\n\r\nRank 8: Windsom - Rank: A* (Class: Guardian, Race: Dragon) \r\n\r\nRank 9: Selene - Rank: A (Class: Succubus, Race: Demon) \r\n\r\nRank 10: Evelyn Everbright - Rank: S- (Class: High-Elf Warrior, Race: Elf) ");
            Console.ForegroundColor = ConsoleColor.White; // Revert to original color for terminal
            Console.WriteLine();
            smoothPrinting.RapidPrint("\nMC's Inner Thoughts: “The top 3 aren't even known? I seriously wonder how powerful they are, perhaps they have a clue as to how I got summoned into this world...”\n");
            UI.PromptUserToContinue();


            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine("First Encounter - Arcania's Magic Council");
            smoothPrinting.PrintLine("--------------------------------------------------");

            smoothPrinting.RapidPrint("As you continue walking, you can't help but feel a sense of reverence for those who came before, their legacies immortalized in stone and metal. It's a reminder of the long history and tradition that surrounds the Magic Council, and you can't help but feel honored to be a part of it. Before proceeding inside the Hall, Evelyn stumbles upon her brother, who she hasn’t met for a long time due to her duties as a Guildmaster, they end up meeting each other and talking for some time, there she introduces you, the ‘MC’ and he greets you with a heartwarming smile, just like Evelyn, you can clearly tell that they are siblings. ");
            UI.PromptUserToContinue();


            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine("First Encounter - Arcania's Magic Council");
            smoothPrinting.PrintLine("--------------------------------------------------");

            smoothPrinting.RapidPrint("Lister's overwhelming aura exudes strength and confidence, casting a palpable sense of authority and prowess. It surrounds him like an invisible shield, commanding respect and attention from those around him. The intensity of his presence is undeniable, leaving an indelible impression on all who encounter him. It's a combination of his unwavering determination, honed skill, and unwavering commitment to his craft that contribute to the formidable aura he emits. In the presence of Lister, one can't help but feel a sense of reverence and admiration for his formidable abilities and commanding demeanor. ");
            UI.PromptUserToContinue();


            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine("First Encounter - Arcania's Magic Council");
            smoothPrinting.PrintLine("--------------------------------------------------");

            // Lazy output, fix this when you get the time so it is structurered properly
            smoothPrinting.RapidPrint("\nEvelyn: “Hey Lister, it’s been a long time since we last met. How are you doing?” \r\n\r\nLister: “Oh Evelyn, it really has been a while. You seem even more capable and stronger now. I’m doing great. By the way, who is that with you? I don’t think I’ve seen him before.” \r\n\r\nEvelyn: “He's a recent addition to my guild.” \r\n\r\nLister: “Is that so? Well, I'm usually good at reading people, but I can’t seem to read him. Guess you picked out someone interesting.” ");
            UI.PromptUserToContinue();


            firstEncounterDialogue1(); // Function call to enable dialogue


            void firstEncounterDialogue1()
            {
                smoothPrinting.PrintLine("--------------------------------------------------");
                smoothPrinting.PrintLine("First Encounter - Arcania's Magic Council");
                smoothPrinting.PrintLine("--------------------------------------------------");

                smoothPrinting.RapidPrint("\nLister: “Hello there, what's your name brave one?”\n");

                smoothPrinting.RapidPrint("Interaction Choice\n");

                smoothPrinting.RapidPrint($"\n1. “My name is {name}, it is an honor to meet you.”");
                smoothPrinting.RapidPrint($"\n2. “My identity remains a secret, that I shall not reveal.\n”");
                smoothPrinting.RapidPrint("\nEnter a corresponding value: ");

                choice1 = Console.ReadLine();

                switch (choice1)
                {
                    case "1":
                        smoothPrinting.RapidPrint($"\n{name}: “My name is {name}, it is an honor to meet you.”");
                        smoothPrinting.RapidPrint($"\nLister: “Nice to meet you {name}, you seem quite capable, and can potentially see you replacing our predacessors at some point in the ranks ha-ha-ha?”");
                        // Future reference: Add a impression increase for Lister NPC
                        break;
                    case "2":
                        smoothPrinting.RapidPrint($"\n{name}: “My identity remains a secret, that I shall not reveal”\n");
                        smoothPrinting.RapidPrint("\nLister: “You are quite interesting, Evelyn's recent representative, though I feel we won't get along...”");
                        // Future reference: Add a impression drop for Lister NPC
                        break;
                    default:
                        if (string.IsNullOrEmpty(choice1))
                        {
                            smoothPrinting.RapidPrint("Invalid input, please try again");
                            Console.ReadKey(); // Let user know that their input is invalid
                            Console.Clear(); // Clears the console
                            firstEncounterDialogue1(); // Recurse again to ensure user enters correct input

                        }
                        break;

                }
            }

            smoothPrinting.RapidPrint("\r\n\r\nEvelyn: “I am always good at this sort of thing.” \r\n\r\nLister: “Guess I shouldn’t be surprised, ha-ha. Your guild has become quite the powerhouse as well.” \r\n\r\nEvelyn: “It’s been some time since I last saw your representative. He seems to have toughened up a lot. Your training regimen must’ve really bolstered him, ha-ha-ha.” \r\n\r\nLister: “Well, our Everbright techniques will make anyone stronger.” \r\n\r\nEvelyn: “Brother, have you been training lately? I can tell that you have grown a lot stronger. Care to give your sister some advice?” *she says with a soft voice* \r\n\r\nLister: “Just make sure you are working hard, and especially utilizing your mana effectively and training with it very frequently. That’s how I recently broke through to Arcane Savant (Awakening 7).” \r\n\r\nMC Inner thoughts: Arcane Savant? How strong are the magic council members? I wonder if I’ll ever reach their ranks... \r\n\r\nEvelyn: “Okay bro, thanks for the advice. I’ll make sure to work really hard so that way when there’s another meeting, I'll be stronger than before!” \r\n\r\nLister: “Yeah, I hope that's the case as well sis! Now let’s meet with the other members. They are surely taking their time, huh?” \r\n\r\nMC Inner thoughts: I can't believe I'm in the presence of such powerful beings. But I won't let that intimidate me. I'll work hard and prove myself worthy of standing among them.");
            UI.PromptUserToContinue();

            // This section will be removed when this mission comes to become an integral part of the game, right now this is all for debugging purposes

            // gameDashboard dash = new gameDashboard();
            // dash.dashboard(character);

        }
    }


    public class gameDashboard
    {
        private readonly UIManager UI;
        private readonly SmoothConsole smoothPrinting;

        public gameDashboard()
        {
            UI = new UIManager();
            smoothPrinting = new SmoothConsole();
        }

        public void dashboard(CharacterDefault character)
        {
            string userInput;

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Gray;
            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine($"FantasyRPG: {character.name}'s Dashboard");
            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.RapidPrint("Current Continent: Tenebris\n"); // Fixed variable, will be dynamic in the future
            smoothPrinting.RapidPrint("\n1. Main Storyline (N/A)\n");
            smoothPrinting.RapidPrint("\n2. Infinite Dungeon (N/A)\n");
            smoothPrinting.RapidPrint("\n3. Arcane Sentinels (N/A)\n");
            smoothPrinting.RapidPrint("\n4. Shop (N/A)\n");
            smoothPrinting.RapidPrint("\n5. NPC's Encountered (N/A)\n");
            smoothPrinting.RapidPrint("\n6. Character Status (N/A)\n");
            smoothPrinting.RapidPrint("\n7. Check Inventory\n");
            smoothPrinting.RapidPrint("\n8. Continents (N/A)\n");
            smoothPrinting.RapidPrint("\nEnter a corresponding value: ");

            userInput = Console.ReadLine(); // Register user input

            switch (userInput)
            {
                case "1":
                    Console.Clear(); // Clear the console
                    smoothPrinting.PrintLine("--------------------------------------------------");
                    smoothPrinting.PrintLine("FantasyRPG: Main Storyline");
                    smoothPrinting.PrintLine("--------------------------------------------------");

                    smoothPrinting.RapidPrint("\nMain Storyline is not available yet.");
                    Console.ReadKey();
                    Console.Clear(); // Clear the console.
                    dashboard(character); // Due to lack of functionality, return user back to the dashboard
                    break;
                case "2":
                    Console.Clear(); // Clear the console
                    smoothPrinting.PrintLine("--------------------------------------------------");
                    smoothPrinting.PrintLine("FantasyRPG: Infinite Dungeon");
                    smoothPrinting.PrintLine("--------------------------------------------------");

                    smoothPrinting.RapidPrint("\nInfinite Dungeon is not available yet.");
                    Console.ReadKey();
                    Console.Clear(); // Clear the console.
                    dashboard(character); // Due to lack of functionality, return user back to the dashboard
                    break;
                case "3":
                    Console.Clear(); // Clear the console
                    smoothPrinting.PrintLine("--------------------------------------------------");
                    smoothPrinting.PrintLine("FantasyRPG: Arcane Sentinels");
                    smoothPrinting.PrintLine("--------------------------------------------------");

                    smoothPrinting.RapidPrint("\nArcane Sentinels is not available yet.");
                    Console.ReadKey();
                    Console.Clear(); // Clear the console.
                    dashboard(character); // Due to lack of functionality, return user back to the dashboard
                    break;
                case "4":
                    Console.Clear(); // Clear the console
                    smoothPrinting.PrintLine("--------------------------------------------------");
                    smoothPrinting.PrintLine("FantasyRPG: Shop");
                    smoothPrinting.PrintLine("--------------------------------------------------");

                    smoothPrinting.RapidPrint("\nShop is not available yet.");
                    Console.ReadKey();
                    Console.Clear(); // Clear the console.
                    dashboard(character); // Due to lack of functionality, return user back to the dashboard
                    break;
                case "5":
                    NPCEncounters(character, smoothPrinting, UI);
                    break;
                case "6":
                    Console.Clear();
                    character.CheckStatus(character);
                    Console.WriteLine();
                    UI.PromptReturnToDashboard();
                    dashboard(character); // Due to lack of functionality, return user back to the dashboard
                    // Handle character status
                    break;
                case "7":
                    character.CheckInventory();
                    UI.PromptReturnToDashboard();
                    dashboard(character);
                    break;
                case "8":
                    // Future reference: Create a dictionary, and will unlock in an instance that the continent has been explore (i.e. with a Boolean function)
                    Console.Clear();
                    smoothPrinting.PrintLine("--------------------------------------------------");
                    smoothPrinting.PrintLine("FantasyRPG: Continents");
                    smoothPrinting.PrintLine("--------------------------------------------------");
                    smoothPrinting.RapidPrint("\n* Tenebris");
                    smoothPrinting.RapidPrint("\n* ???\n");
                    smoothPrinting.RapidPrint("* ???\n");
                    smoothPrinting.RapidPrint("* ???\n");
                    UI.PromptReturnToDashboard();
                    dashboard(character); // Due to lack of functionality, return user back to the dashboard
                    break;
                default:
                    if (string.IsNullOrEmpty(userInput))
                    {
                        smoothPrinting.RapidPrint("\nInvalid input, please try again.");
                        Console.ReadKey(); // Allow user to see error message
                        dashboard(character); // Due to lack of functionality, return user back to the dashboard
                    }
                    else
                    {
                        smoothPrinting.RapidPrint("\nInvalid input, please try again.");
                        Console.ReadKey(); // Allow user to see error message
                        dashboard(character); // Due to lack of functionality, return user back to the dashboard
                    }
                    break;
            }

        }


        void NPCEncounters(CharacterDefault character, SmoothConsole smoothPrinting, UIManager UI)
        {

            Console.Clear();
            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine("FantasyRPG: NPC's Encountered");
            smoothPrinting.PrintLine("--------------------------------------------------");
            Console.WriteLine(); // Spacing

            if (character is Mage || character is SomaliPirate)
            {
                if (character.npcsEncountered != null && character.npcsEncountered.Count > 0)
                {
                    foreach (var npc in character.npcsEncountered)
                    {
                        smoothPrinting.RapidPrint($"\nName: {npc.npcName}\nDescription: {npc.npcDescription}\nAffiliation: {npc.npcAffiliation}\n");
                    }
                }
                else
                {
                    smoothPrinting.RapidPrint("\nYou haven't encountered any NPCs at this point.");
                }
            }
            else
            {
                smoothPrinting.RapidPrint("\nCharacter type not supported.");
            }

            // Prompt the user to continue
            UI.PromptReturnToDashboard();
            dashboard(character); // Return user back to the dashboard
        }

        void mageDisplayPlayerStatus(Mage mage)
        {
            smoothPrinting.RapidPrint($"Name: {mage.name}\n Weapon: {mage.weapon}\n, Currency (Arcania's Golden Coins): {mage.arcaniaGoldCoins}\n, Magic Spells: {mage.magicSpells}");
        }

        // void guild()
        // {
        // This will contain information such as guild members, speaking with guild members such as asking them questions etc, which I'll try make dynamic and also being able to see ones current reputation
        // }

        // void infiniteDungeon()
        // {
        // This will be a recursive dungeon that will allow players to be able to obtain loot and level up, in turn enabling them to become stronger
        // }

        // void shop()
        // {
        // Items will be sold within the shop, that the user can purchase, along with weapons etc.
        // }

    }
}


public class UIManager // UIManager - a class that will allow for the display of progress bars, prompts etc.
{
    SmoothConsole smoothPrinting = new SmoothConsole(); // Engage the smoothconsole class
    public void DisplayProgressBar(string title, float currentValue, float maxValue, float barLength)
    {
        // Ensure currentValue does not exceed maxValue
        currentValue = Math.Min(currentValue, maxValue);

        // Calculate the percentage
        double percentage = currentValue / maxValue;

        // Calculate the number of filled characters
        int filledLength = (int)Math.Round(percentage * barLength);

        // Generate the progress bar
        string progressBar = new string('█', filledLength) + new string(' ', (int)barLength - filledLength);

        // Output the progress bar
        smoothPrinting.RapidPrint($"\n{title}: [{progressBar}] [{currentValue}/{maxValue}]");
    }



    public void PromptUserToContinue()
    {
        smoothPrinting.RapidPrint("\nAffirmative? If so, click any key to continue.");
        Console.ReadKey();
        Console.Clear();

    }

    public void PromptReturnToDashboard()
    {
        smoothPrinting.RapidPrint("\nAffirmative? If so, click any key to return back to the dashboard.");
        Console.ReadKey();
        Console.Clear();

    }

}

public class SmoothConsole // This will be used to ensure output from the console is smooth and aesthetic looking
{
    public void CenterPrint(string text) // Will center user output
    {
        int width = Console.WindowWidth;
        int spaces = (width - text.Length) / 2;
        Console.Write(new string(' ', spaces) + text);
        Thread.Sleep(10);
    }

    public void RapidPrint(string text) // Even faster console output
    {
        foreach (char c in text)
        {
            Console.Write(c);
            Thread.Sleep(10);
        }
    }
    public void SlowPrint(string text) // Slower, smoother console output
    {
        foreach (char c in text)
        {

            Console.Write(c);
            Thread.Sleep(50);

        }

    }

    public void PrintLine(string line) // Printing lines 
    {
        Console.WriteLine(line.PadLeft((Console.WindowWidth + line.Length) / 2));
    }

    public void FastPrint(string text) // Faster, smoother console output
    {
        foreach (char c in text)
        {

            Console.Write(c);
            Thread.Sleep(20);

        }
    }

}  // Namespace coverage: DO NOT REMOVE THIS
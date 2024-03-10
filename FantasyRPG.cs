using Microsoft.Win32.SafeHandles;
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
        public int currentHealth, maxHealth;
        public List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription)> weapon;
        public float numOfPotionsInInventory;
        public float maxPotions;
        public int currentMana, maxMana;
        public List<(string itemName, string itemDescription, string itemRarity, int itemPower)> currentInventory; // Will contain item name, description, rarity and power (i.e. healing or attack etc.) 
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

        public CharacterDefault(string _name, List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription)> _weapon, List<(string itemName, string itemDescription, string itemRarity, int itemPower)> _currentInventory, int _arcaniaGoldCoins, int specialAtkRecharge, List<(string npcName, string npcDescription, string npcAffiliation)> _npcsEncountered) // Default preset for all classes during the start of the game :3
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
                    smoothPrinting.RapidPrint("You will now be redirected back to the dashboard.");
                    smoothPrinting.RapidPrint("\nAffirmative? If so, click any key to return back to the dashboard.");
                    Console.ReadKey(); // Register user input
                    Console.Clear(); // Clear the console to avoid overlapping
                    gameDashboard dash = new gameDashboard();
                    dash.dashboard(character); // Return to the user dashboard
                    break;
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


        // Levelling methods 
        public void CalculateExperienceForNextLevel(CharacterDefault character)
        {
            UIManager UI = new UIManager(); // Engage the UIManager for progress bars
            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine($"FantasyRPG: {name}'s Status Check - Required EXP for next Level");
            smoothPrinting.PrintLine("--------------------------------------------------");


            if (character is Mage)
            {
                // Depending on level, requirement for level is adjusted
                if (level < 5)
                {
                    experienceRequiredForNextLevel = 10 * level;
                    UI.DisplayProgressBar($"Experience required for Level {level + 1}", exp, experienceRequiredForNextLevel, 30);
                    Console.WriteLine(); // Spacing
                }
                else if (level > 10)
                {
                    experienceRequiredForNextLevel = 100 * level;
                    UI.DisplayProgressBar($"Experience required for Level {level + 1}", exp, experienceRequiredForNextLevel, 30);
                }
                smoothPrinting.RapidPrint("\nAffirmative? If so, click any key to return back to the dashboard.");
                Console.ReadKey(); // Register user input
                Console.Clear(); // Clear the console to avoid overlapping
                gameDashboard dash = new gameDashboard();
                dash.dashboard((Mage)character); // Return to the user dashboard

            }
            else if (character is SomaliPirate)
            {
                // Depending on level, requirement for level is adjusted
                if (level < 5)
                {
                    experienceRequiredForNextLevel = 10 * level;
                    UI.DisplayProgressBar($"Experience required for Level {level + 1}", exp, experienceRequiredForNextLevel, 30);
                }
                else if (level > 10)
                {
                    experienceRequiredForNextLevel = 100 * level;
                    UI.DisplayProgressBar($"Experience required for Level {level + 1}", exp, experienceRequiredForNextLevel, 30);
                }

                smoothPrinting.RapidPrint("\nAffirmative? If so, click any key to return back to the dashboard.");
                Console.ReadKey(); // Register user input
                Console.Clear(); // Clear the console to avoid overlapping
                gameDashboard dash = new gameDashboard();
                dash.dashboard((SomaliPirate)character); // Return to the user dashboard
            }



        }

        // Should the condiition be met
        public void LevelUp(CharacterDefault character)
        {
            if (character is Mage)
            {
                smoothPrinting.PrintLine("--------------------------------------------------");
                smoothPrinting.PrintLine($"FantasyRPG: Mage Level Up!");
                smoothPrinting.PrintLine("--------------------------------------------------");
                level++;
                Console.WriteLine(name + " has levelled up! " + " You are now level " + level);
                CalculateExperienceForNextLevel((Mage)character);

            }
            else if (character is SomaliPirate)
            {
                smoothPrinting.PrintLine("--------------------------------------------------");
                smoothPrinting.PrintLine($"FantasyRPG: Pirate Level Up!");
                smoothPrinting.PrintLine("--------------------------------------------------");
                level++;
                Console.WriteLine(name + " has levelled up! " + " You are now level " + level);
                CalculateExperienceForNextLevel((SomaliPirate)character);
            }

        }

        // Check if user has enough to level up
        public void GainExperience(CharacterDefault character, float experiencePoints)
        {
            if (character is Mage)
            {
                exp += experiencePoints;

                // Check if the character should level up
                if (exp >= experienceRequiredForNextLevel)
                {
                    LevelUp((Mage)character);
                }

            }
            else if (character is SomaliPirate)
            {
                exp += experiencePoints;

                // Check if the character should level up
                if (exp >= experienceRequiredForNextLevel)
                {
                    LevelUp((SomaliPirate)character);
                }
            }

        }

    }

    class MobDefault // Mob preset for the game
    {
        public string name;
        public int specialAtkRecharge, currentMobHealth, maxMobHealth;


        // Mobs can have different attack names and varying item drops, each associated with a rarity and damage value
        public Dictionary<string, (int, string, string, string)> itemDrop; // First string defines the weapon name, second integer defines the weapon damage, thirs stirng defines the weapon rarity and fourth string defines the weapon type
        public Dictionary<string, int> normalAtkNames;
        public Dictionary<string, (int, string)> specialAtkNames;

        public MobDefault(string _name, Dictionary<string, int> _normalAtkNames, Dictionary<string, (int, string)> _specialAtkNames, int _specialAtkRecharge, int _currentMobHealth, int _maxMobHealth, Dictionary<string, (int, string, string, string)> _itemDrop) // Presets for all mobs within the game (i.e. dragons, shadow stalkers, arcane phantons, crawlers etc.)
        {
            name = _name;
            normalAtkNames = _normalAtkNames;
            specialAtkNames = _specialAtkNames;
            itemDrop = _itemDrop; // Mobs have a chance to drop a random item once they die
            specialAtkRecharge = 100;
            currentMobHealth = _currentMobHealth;
            maxMobHealth = _maxMobHealth;

        }

        public void displayMobStatus(MobDefault mob)
        {
            UIManager UI = new UIManager(); // Displaying progress bar

            // Add parameters such as the mobs health etc.
            UI.DisplayProgressBar("Mob Health", mob.currentMobHealth, mob.maxMobHealth, 30);
            Console.WriteLine();
            Console.WriteLine(); // Double spacing to avoid overlapping

        }

        public void dragonDeath(MobDefault mob, CharacterDefault character)
        {
            if (mob.currentMobHealth == 0)
            {
                Random itemDropChance = new Random();
                int dropChance = itemDropChance.Next(0, 1); // 20% drop rate, due to OP item drops
                smoothPrinting.FastPrint($"\n{name} has successfully killed the dragon!");

                if (dropChance == 0 || dropChance == 1)
                {

                    dropItem(dropChance, character, mob); // Should the random number be zero, then the mob will drop an item
                }

                character.exp += 300; // User gains huge exp from defeating the dragon 
                character.GainExperience(character, character.exp);

            }

        }

        public void dropItem(int dropChance, CharacterDefault character, MobDefault mob)
        {
            Random ran = new Random(); // Determine which item will be dropped
            int randomWeapon = ran.Next(0, 6); // Generate a value between 0 to 5 (inclusive)
            string userChoice;

            itemDrop.ToList(); // Convert the item drops to a list
            var weapon = itemDrop.ElementAtOrDefault(randomWeapon); // Select the weapon based on random index

            if (!string.IsNullOrEmpty(weapon.Key))
            {
                smoothPrinting.RapidPrint($"\n{mob.name} Drop: {character.name} has received {weapon.Key}, would you like to equip this weapon? (1 for 'yes' and any other key for 'no'");
                userChoice = Console.ReadLine(); // Register user input

                if (userChoice == "1")
                {
                    character.weapon.Clear(); // Remove the current weapon equipped by the user
                    character.weapon.Add((weapon.Key, weapon.Value.damage, weapon.Value.rarity, weapon.Value.weaponType, weapon.Value.weaponDescription));
                }
                else
                {
                    smoothPrinting.RapidPrint("\nWeapon will be stored to inventory.");
                }

                character.currentInventory.Add((weapon.Key, weapon.Value.weaponDescription, weapon.Value.rarity, weapon.Value.damage)); // Add the weapon to the character's inventory
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
        SmoothConsole smoothPrinting = new SmoothConsole();

        public Crawler(string _name, Dictionary<string, int> _normalAtkNames, Dictionary<string, (int, string)> _specialAtkNames, int _specialAtkRecharge, int _currentMobHealth, int _maxMobHealth, Dictionary<string, (int, string, string, string)> _itemDrop) : base(_name, _normalAtkNames, _specialAtkNames, _specialAtkRecharge, _currentMobHealth, _maxMobHealth, _itemDrop)
        {

            // Default presets for a crawler, inherited from the mob default class

            name = "Crawler";
            currentMobHealth = 20; // Crawlers are very weak creatures, and by default have 20 health
            maxMobHealth = 20;


            // Dictionary containing crawler attacks and their associated damage value
            Dictionary<string, int> normalAtkNames = new Dictionary<string, int>() // Preset name for all dragon's normal attacks
            {
                { "Crawler's Scratch", 5 },
                { "Crawler's Screech", 10 },
                { "Crawler's Bite", 3 }
            };

            // Dictionary that contains weapon name, damage, rarity and weapon type (item drops)
            Dictionary<string, (int, string, string, string)> itemDrop = new Dictionary<string, (int, string, string, string)>()
            {
                { "Staff of Spite", (7, "(Common)", "Staff", "Not cool") },
                { "Crawler's Revant", (10, "(Uncommon)", "Rapier/Sword", "Bad") },
            };

            itemDrop = _itemDrop;
            normalAtkNames = _normalAtkNames;


            // Future reference: Add a condition to insert the item into the users inventory, the user has a choice to accept/decline, should they decline, the weapon gets discareded into parts which can be used to refine other weapons
        }


        public void crawlerNormalAtk(int health)
        {
            Random rd = new Random();
            List<string> attackNames = normalAtkNames.Keys.ToList(); // Get all attack names

            int randomIndex = rd.Next(0, attackNames.Count); // Generate a random index

            string randomAttackName = attackNames[randomIndex]; // Get a random attack name
            int damage = normalAtkNames[randomAttackName]; // Get the damage associated with the attack

            smoothPrinting.FastPrint("Crawler has used " + randomAttackName + " dealing " + damage + " damage.\n");
        }


        public void crawlerDeath(int mobHealth, int exp) // If the crawler dies, then the user gains exp and has a chance of receiving an item drop
        {
            if (mobHealth == 0)
            {
                Random itemDropChance = new Random();
                int dropChance = itemDropChance.Next(1, 2); // 50% drop rate, as the mob is easy to defeat
                smoothPrinting.FastPrint("\nDragon has been successfully defeated!");

                if (dropChance == 0)
                {
                    dropItem(mobHealth); // Should the random number be zero, then the mob will drop an item
                }

                exp += 5; // User gets experience from the drop
                smoothPrinting.SlowPrint("User has gained " + exp + " experience points!");

            }
        }

        public void dropItem(int dropChance)
        {


        }


    }

    class Dragon : MobDefault // Dragon class
    {
        SmoothConsole smoothPrinting = new SmoothConsole();
        string name;
        int currentMobHealth, maxMobHealth = 350;


        // Dictionary containing dragon attacks and their associated damage value
        Dictionary<string, int> normalAtkNames = new Dictionary<string, int>() // Preset names for all dragon's normal attacks
            {
                { "Dragon's Claw", 30 },
                { "Dragon's Breath", 40 },
                { "Raging Tempest", 50 }
            };

        // Dictionary that contains weapon name, damage, rarity, and weapon type (item drops)
        Dictionary<string, (int damage, string rarity, string weaponType, string weaponDescription)> itemDrop = new Dictionary<string, (int, string, string, string)>()
        {
            { "Frostfire Fang", (65, "Unique", "Staff", "Forged in the icy flames of the dragon's breath, this fang drips with frostfire, capable of freezing enemies in their tracks.") },
            { "Serpent's Gaze", (50, "Unique", "Rapier/Sword", "Crafted from the scales of the ancient serpent, this gaze holds the power to petrify foes with a single glance.") },
            { "Chaosfire Greatsword", (60, "Unique", "Greatsword/Sword", "Tempered in the chaosfire of the dragon's lair, this greatsword burns with an insatiable hunger for destruction.") },
            { "Nightshade Arc", (55, "Unique", "Bow", "Fashioned from the sinew of the nocturnal shadows, this bow strikes with deadly accuracy under the cover of darkness.") },
            { "Aerith's Heirloom", (80, "Legendary", "Staff", "Once wielded by the legendary Aerith, this staff channels the primordial magic of creation itself, capable of reshaping reality.") },
            { "Eucladian's Aura", (55, "Legendary", "Aura", "Embrace the ethereal aura of the Eucladian, granting unmatched protection against all forms of magic and malevolence.") }
        };

        Dictionary<string, (int damage, string magicType)> specialAtkNames = new Dictionary<string, (int, string)>() // Preset names for all dragon's special attacks
            {
                { "Arcane Nexus", (100, "Eucladian-Magic")}, // Eucladian type ULT
                { "Umbral Charge", (120, "Dark-Magic")}, // Dark type ULT
                { "Rampant Flame Charge", (200, "Fire-Magic") } // Flame type ULT
            };

        public Dragon(string _name, Dictionary<string, int> _normalAtkNames, Dictionary<string, (int, string)> _specialAtkNames, int _specialAtkRecharge, int _currentMobHealth, int _maxMobHealth, Dictionary<string, (int, string, string, string)> _itemDrop) : base(_name, _normalAtkNames, _specialAtkNames, _specialAtkRecharge, _currentMobHealth, _maxMobHealth, _itemDrop)
        {
            // Default presets for a dragon, inherited from the mob default class
            name = _name;
            currentMobHealth = _currentMobHealth; // Dragons have 350HP by default
            maxMobHealth = _maxMobHealth;
            normalAtkNames = _normalAtkNames;
            specialAtkNames = _specialAtkNames;
            itemDrop = _itemDrop;
            UIManager UI = new UIManager();
        }

        // Future reference: Create different types of dragons that have weaknesses (i.e. water dragons, shadow dragons etc)

        public void exertPressure(CharacterDefault character, MobDefault mob) // Dragons will exert 'pressure' to make humans fear them, should the users level be lower than expected
        {
            if (character.level <= 10) // Should the users level be below level 10, then the dragon will exert pressure to the individual, reducing their attack value.
            {
                UIManager UI = new UIManager();
                smoothPrinting.PrintLine("--------------------------------------------------");
                smoothPrinting.PrintLine("FantasyRPG: Dragon Race - Exerting Pressure");
                smoothPrinting.PrintLine("--------------------------------------------------");

                Console.ForegroundColor = ConsoleColor.Red;
                smoothPrinting.RapidPrint($"{mob.name}:\n");
                smoothPrinting.RapidPrint("\n*Roars with a deafening sound, shaking the very ground beneath you.*\n");

                smoothPrinting.RapidPrint($"\nYour level is lower than expected, {mob.name} exerts immense pressure, casting a shadow of dread over you. You feel your resolve weaken as fear grips your heart.\n");

                smoothPrinting.RapidPrint($"\nThe ancient power emanating from {mob.name} fills the air, suffocating your magical abilities. You sense a drain on your strength, your magical potency diminishing.\n");

                smoothPrinting.RapidPrint("\nYour attack damage is reduced as the overwhelming presence of Windsom weighs heavily upon you.\n");

                UI.PromptUserToContinue();
                Console.ResetColor(); // Reset Console Colour

            }

        }

        public void dragonNormalAtk(int health)
        {
            Random rd = new Random();
            List<string> attackNames = normalAtkNames.Keys.ToList(); // Get all attack names

            int randomIndex = rd.Next(0, attackNames.Count); // Generate a random index

            string randomAttackName = attackNames[randomIndex]; // Get a random attack name
            int damage = normalAtkNames[randomAttackName]; // Get the damage associated with the attack

            smoothPrinting.FastPrint("Dragon has used " + randomAttackName + " dealing " + damage + " damage.\n");
            health = health - damage; // Return the difference by reducing users health, based on damage inflicted
        }


        public void dragonSpecialAtk() // If the dragons special attack recharge reaches 100%, then this will be activated
        {
            Random rd = new Random();

            List<string> attackNames = specialAtkNames.Keys.ToList(); // Get all attack names
            int randomIndex = rd.Next(0, attackNames.Count); // Generate a random index

            string randomAttackName = attackNames[randomIndex]; // Get a random attack name
            int damage = normalAtkNames[randomAttackName]; // Get the damage associated with the attack

            if (specialAtkRecharge == 100) // Should the dragon's special attack recharge reach 100%, then it'll use its special ability, dealing high levels of damage, it also increases its health
            {
                smoothPrinting.SlowPrint("\nDragon ULT");
                smoothPrinting.SlowPrint("\nDragon has used " + randomAttackName + " and has dealt " + damage + "\n");
                smoothPrinting.RapidPrint("\nDragon has recovered +20 health");
                currentMobHealth = currentMobHealth + 20; // Slight health regen
                specialAtkRecharge = 0; // The special attack has been used by this point, so therefore it should be set to zero.
            }

        }

    }


    class Knight : CharacterDefault // Knight class properties and methods
    {
        public string normalAtkName;
        public string specialAtkName; // Remove these static features
        public int specialAtkDmg;
        public int normalAtkDmg;

        public Knight(string _name, List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription)> _weapon, string _specialAtkName, List<(string itemName, string itemDescription, string itemRarity, int itemPower)> _currentInventory, int _arcaniaGoldCoins, int _specialAtkRecharge, List<(string npcName, string npcDescription, string npcAffiliation)> _npcsEncountered) : base(_name, _weapon, _currentInventory, _arcaniaGoldCoins, _specialAtkRecharge, _npcsEncountered)
        {
            name = _name;
            weapon = _weapon;
            specialAtkName = _specialAtkName;
            currentInventory = _currentInventory;
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

        public Mage(string _name, List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription)> _weapon, string[] _magicSpecialties, int _arcaniaGoldCoins, List<(string magicSpell, int damage, int manaRequirement)> _magicSpells, List<(string itemName, string itemDescription, string itemRarity, int itemPower)> _currentInventory, int _specialAtkRecharge, List<(string npcName, string npcDescription, string npcAffiliation)> _npcsEncountered) : base(_name, _weapon, _currentInventory, _arcaniaGoldCoins, _specialAtkRecharge, _npcsEncountered)
        {
            name = _name;
            weapon = _weapon;
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

        // WIll be used to display the Mage Combat System header
        public void DisplayMageCombatSystemHeader()
        {
            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine("FantasyRPG: Mage Combat System");
            smoothPrinting.PrintLine("--------------------------------------------------");
        }

        public void MageSpellAttack(Mage mage, MobDefault mob, bool userTurn, bool enemyTurn, bool quickDisplay) // Will load the Mage Combat System for fighting situations
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
            smoothPrinting.RapidPrint($"{mage.name} - Mage Status\n");
            smoothPrinting.RapidPrint($"{mob.name} - Enemy\n");

            // Display users mana and remaining health
            UI.DisplayProgressBar("Health", currentHealth, maxHealth, 30);
            Console.WriteLine(); // Spacing
            UI.DisplayProgressBar("Mana", currentMana, maxMana, 30);
            Console.WriteLine(); // Spacing
            UI.DisplayProgressBar("Enemy Health", mob.currentMobHealth, mob.maxMobHealth, 30); // Display mob health
            Console.WriteLine(); // Spacing

            // Combat methods for the Mage class
            foreach (var spell in magicSpells) // Display all spells currently avaliable to the Mage
            {
                smoothPrinting.RapidPrint($"\n{spellCount}. Spell: {spell.magicSpell} - Damage: {spell.damage}\nMana Requirement: {spell.manaRequirement}\n");
                spellCount++;
            }

            // Register user input
            smoothPrinting.RapidPrint("\nSelect a spell to attack (Enter '0' to return back): ");
            userInput = Console.ReadLine();

            int registeredInput = Int32.Parse(userInput); // Convert value to integer

            // Check if the user input corresponds to a spell index
            if (registeredInput > 0 && registeredInput <= magicSpells.Count)
            {
                // Get the chosen spell based on the user's input
                var chosenSpell = magicSpells[registeredInput - 1];

                // Add the chosen spell to the list of spells for attack
                chosenSpellForAttack.Add((chosenSpell.magicSpell, chosenSpell.damage, chosenSpell.manaRequirement));
            }
            else if (registeredInput == 0)
            {
                // User wants to return back, handle accordingly
                smoothPrinting.RapidPrint("\nYou will be redirected back to the Mage Combat System (MCS)");
                UI.PromptUserToContinue();
                DisplayMageStatus(mage, mob, quickDisplay = true);
            }
            else
            {
                // Invalid input, handle accordingly
                smoothPrinting.RapidPrint("\nInvalid input, please try again.");
                UI.PromptUserToContinue();
                MageSpellAttack(mage, mob, userTurn, enemyTurn, quickDisplay);
            }

            foreach (var spell in chosenSpellForAttack)
            {
                if (mage.currentMana >= spell.manaRequirement)
                {
                    smoothPrinting.RapidPrint($"\n{mage.name} has casted {spell.magicSpell}, dealing {spell.damage} damage to {mob.name}.");
                    mob.currentMobHealth -= spell.damage;
                    mage.currentMana -= spell.manaRequirement; // Linearly reduce the mage's mana based on the mana requirement of the spell
                    Console.ReadKey();
                    Console.Clear();
                    DisplayMageStatus(mage, mob, quickDisplay = true); // Return after attack (TESTING)
                }
                else
                {
                    smoothPrinting.RapidPrint("\nYou do not have enough mana to cast this spell.");
                    Console.ReadKey();
                    Console.Clear();
                    MageSpellAttack(mage, mob, userTurn, enemyTurn, quickDisplay = true); // Recurse back to the function, should the user wish to use an alternative spell
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


        }

        // public override void CheckStatus()
        // {
        // base.CheckStatus(cha);
        // }

        public void DisplayMageStatus(Mage mage, MobDefault mob, bool quickDisplay) // Naturally this takes in the mage class and the given mob
        {
            bool userTurn, enemyTurn; // These boolean measures are to create the turn based dynamic for the game
            UIManager UI = new UIManager(); // Engage the UI manager for progress bars

            int? numCount = 1; // Will display the numeric choices for the Mage's options
            string? userChoice;

            string[] mageChoices = new string[] { "Attack", "Check Inventory", "Check Status", "Attempt Escape (WARNING: Low Chance)" }; // Array displaying the different Mage's options


            if (mob.currentMobHealth == 0) // Check everytime if the mob has died
            {
                Console.Clear();
                smoothPrinting.PrintLine("--------------------------------------------------");
                smoothPrinting.PrintLine($"FantasyRPG: Defeated {mob.name}");
                smoothPrinting.PrintLine("--------------------------------------------------");

                UI.DisplayProgressBar("Health", mage.currentHealth, mage.maxHealth, 30); // Display Mage's health
                Console.WriteLine(); // Spacing

                UI.DisplayProgressBar("Mana", currentMana, maxMana, 30); // Display Mage's remaining mana
                Console.WriteLine(); // Spacing

                UI.DisplayProgressBar("Enemy Health:", mob.currentMobHealth, mob.maxMobHealth, 30); // Display enemies health
                Console.WriteLine(); // Spacing

                smoothPrinting.RapidPrint($"\n{mob.name} has been defeated by {mage.name}, rewards incoming...");


                // gameDashboard dash = new gameDashboard();
                // dash.dashboard(mage);
            }
            else if (mage.currentHealth == 0) // Should the user die instead
            {
                string? userInput;

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                smoothPrinting.PrintLine("--------------------------------------------------");
                smoothPrinting.PrintLine($"FantasyRPG: You have died by {mob.name}");
                smoothPrinting.PrintLine("--------------------------------------------------");

                smoothPrinting.RapidPrint("\nWould you like to go back to the Menu? ('1' for Yes, any other key for No)");
                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        smoothPrinting.RapidPrint("\nYou will now be redirected back to the Menu...");
                        userInput = null; // This is case measure to prevent the userInput value from already having a stored value, if the user reaches this point in the game again
                        GameMenu redirectUserToMenu = new GameMenu();
                        redirectUserToMenu.gameMenu(); // Redirect user to the game menu, if they enter the value '1'
                        break;
                    default:
                        smoothPrinting.RapidPrint("\nConsole will now terminate, press any key to leave the game");
                        Console.ReadKey();
                        break;
                }


            }
            else
            {
                if (quickDisplay == true)
                {
                    DisplayMageCombatSystemHeader(); // Display the MCS (Mage Combat System Header)

                    Console.WriteLine($"{mage.name} - Mage Status");
                    Console.WriteLine($"{mob.name} - Enemy");

                    UI.DisplayProgressBar("Health", mage.currentHealth, mage.maxHealth, 30); // Display Mage's health
                    Console.WriteLine(); // Spacing

                    UI.DisplayProgressBar("Mana", currentMana, maxMana, 30); // Display Mage's remaining mana
                    Console.WriteLine(); // Spacing
                    UI.DisplayProgressBar("Enemy Health:", mob.currentMobHealth, mob.maxMobHealth, 30); // Display enemies health
                    Console.WriteLine(); // Spacing
                    Console.WriteLine($"\nRemaining Healing Potions: {numOfPotionsInInventory}\n"); // Display Mage's remaining potions


                    if (mage.currentHealth <= 30) // Check if users health is low
                    {
                        smoothPrinting.RapidPrint("WARINING: Your health is critically low, consider using a health potion or a recovery spell.");
                        Console.WriteLine(); // Spacing
                    }

                    if (mage.currentMana <= 30) // Check if users mana levels are low
                    {
                        smoothPrinting.RapidPrint("Warning: Your mana is critically low. Consider using a mana potion or a recovery spell to replenish your mana reserves.");
                        Console.WriteLine(); // Spacing
                    }

                    foreach (var choice in mageChoices)
                    {
                        Console.WriteLine($"\n{numCount}. {choice}");
                        numCount++; // Increment the value to display the other remaining choices
                    }

                    Console.WriteLine("\nEnter a corresponding value: ");
                    userChoice = Convert.ToString(Console.ReadLine()); // Register Mage's choice

                    switch (userChoice)
                    {
                        case "1":
                            Console.Clear(); // Clear the console, to avoid overlapping
                            MageSpellAttack(mage, mob, userTurn = true, enemyTurn = false, quickDisplay);
                            break;
                        case "2":
                            CheckInventory();
                            smoothPrinting.RapidPrint("\nAffirmative? If so, click any key to be redirected back to the M.C.S (Mage Combat System)");
                            Console.ReadKey(); // Register user's input
                            Console.Clear(); // Clear the console to prevent confusion
                            DisplayMageStatus(mage, mob, quickDisplay = true); // Recurse back to the original function
                            break;
                        case "3":
                            Console.Clear();
                            CheckStatus(mage); // Allow user to check their status (FUTURE REFERENCE: ALLOW FOR STATUS TO BE USED DURING COMBAT AND OUTSIDE OF COMBAT)
                            break;
                        case "4":
                            // Generate a random value
                            // Random ran = new Random(); 
                            // ran.Next(0, 50);
                            // For this stage, if the user gets a value such as (i.e. 1, 10, 12, 15) they will luckily escape, otherwise they'll be locked into combat and cannot attempt escape again.
                            // Should they escape, they'll return to the Forest Of Mysteries
                            smoothPrinting.RapidPrint("\nThis feature is currently in development, so you'll be redirected back to the M.C.S (Mage Combat System) Menu.\n");
                            smoothPrinting.RapidPrint("\nAffirmative? If so, click any key to be redirected back to the M.C.S (Mage Combat System)");
                            Console.ReadKey();
                            Console.Clear();
                            DisplayMageStatus(mage, mob, quickDisplay = true); // Recurse back
                            break;
                        default:
                            smoothPrinting.RapidPrint("\nInvalid input, click any key to try again!");
                            Console.ReadKey();
                            Console.Clear(); // Clear the console after letting user read the error message
                            DisplayMageStatus(mage, mob, quickDisplay = true); // Recurse back
                            break;
                    }

                }
                else
                {
                    DisplayMageCombatSystemHeader(); // Display the MCS (Mage Combat System Header)

                    smoothPrinting.RapidPrint($"{mage.name} - Mage Status\n");
                    smoothPrinting.RapidPrint($"{mob.name} - Enemy\n");

                    UI.DisplayProgressBar("Health", mage.currentHealth, mage.maxHealth, 30); // Display Mage's health
                    Console.WriteLine(); // Spacing

                    UI.DisplayProgressBar("Mana", currentMana, maxMana, 30); // Display Mage's remaining mana
                    Console.WriteLine(); // Spacing

                    UI.DisplayProgressBar("Enemy Health:", mob.currentMobHealth, mob.maxMobHealth, 30); // Display enemies health
                    Console.WriteLine(); // Spacing

                    smoothPrinting.RapidPrint($"\nRemaining Healing Potions: {numOfPotionsInInventory}\n"); // Display Mage's remaining potions

                    if (mage.currentHealth <= 30) // Check if users health is low
                    {
                        smoothPrinting.RapidPrint("WARINING: Your health is critically low, consider using a health potion or a recovery spell.");
                        Console.WriteLine(); // Spacing
                    }

                    if (mage.currentMana <= 30) // Check if users mana levels are low
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
                            MageSpellAttack(mage, mob, userTurn = true, enemyTurn = false, quickDisplay);
                            break;
                        case "2":
                            CheckInventory();
                            smoothPrinting.RapidPrint("\nAffirmative? If so, click any key to be redirected back to the M.C.S (Mage Combat System)");
                            Console.ReadKey(); // Register user's input
                            Console.Clear(); // Clear the console to prevent confusion
                            DisplayMageStatus(mage, mob, quickDisplay = true); // Recurse back to the original function
                            break;
                        case "3":
                            Console.Clear();
                            CheckStatus(mage); // Allow user to check their status (FUTURE REFERENCE: ALLOW FOR STATUS TO BE USED DURING COMBAT AND OUTSIDE OF COMBAT)
                            break;
                        case "4":
                            // Generate a random value
                            // Random ran = new Random(); 
                            // ran.Next(0, 50);
                            // For this stage, if the user gets a value such as (i.e. 1, 10, 12, 15) they will luckily escape, otherwise they'll be locked into combat and cannot attempt escape again.
                            // Should they escape, they'll return to the Forest Of Mysteries
                            smoothPrinting.RapidPrint("\nThis feature is currently in development, so you'll be redirected back to the M.C.S (Mage Combat System) Menu.\n");
                            smoothPrinting.RapidPrint("\nAffirmative? If so, click any key to be redirected back to the M.C.S (Mage Combat System)");
                            Console.ReadKey();
                            Console.Clear();
                            DisplayMageStatus(mage, mob, quickDisplay = true); // Recurse back
                            break;
                        default:
                            smoothPrinting.RapidPrint("\nInvalid input, click any key to try again!");
                            Console.ReadKey();
                            Console.Clear(); // Clear the console after letting user read the error message
                            DisplayMageStatus(mage, mob, quickDisplay = true); // Recurse back
                            break;
                    }

                }
                

            }


        }

        public void MageTraining() // Might remove.
        {
            // Generate a random value for exp
            Random rd = new Random();
            int rand_num = rd.Next(1, 5);

            Console.WriteLine(name + " has decided to improve on their skills, ", " their experience has increased by " + rand_num);
            exp = exp + rand_num;

        }

        // Promotion method for the Mage class
        public void chooseNewSpeciality(string[] magicSpecialties, string name) // Every 10 levels, a mage will be able to pick another speciality (only 1)
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
            smoothPrinting.RapidPrint($"\n {name}'s current known magic specialities: ");

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


            smoothPrinting.FastPrint("\nChoose a magic specialty by entering the corresponding number:\n");
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

        public SomaliPirate(string _name, List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription)> _weapon, List<(string auraName, int damage, string rarity, string description)> _weaponAura, List<(string attack, int damage, int manaRequirement, string elementType, string description)> _pirateNormalAtks, List<(string attack, int damage, int manaRequirement, string elementType, string description)> _pirateSpecialAtks, List<(string itemName, string itemDescription, string itemRarity, int itemPower)> _currentInventory, int _arcaniaGoldCoins, int _specialAtkRecharge, List<(string npcName, string npcDescription, string npcAffiliation)> _npcsEncountered) : base(_name, _weapon, _currentInventory, _arcaniaGoldCoins, _specialAtkRecharge, _npcsEncountered)
        {
            name = _name;
            weapon = _weapon;
            weaponAura = _weaponAura;
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
            // GameMenu menu = new GameMenu();
            // MagicCouncil encounter = new MagicCouncil(); // Debugging
            // string name = "Silver"; // Debugging
            // encounter.firstEncounter(name); // Debugging
            // FirstScenario firstScenario = new FirstScenario();
            // firstScenario.usersFirstJourney("Tristian");

            // Define values for debugging mage
            string mageName = "Khalid Du-Lucérian";
            List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription)> mageWeapon = new List<(string, int, string, string, string)> {
               ("Heartblades Vesper", 250, "Legendary", "Staff", "A staff that has been a part of the Heartblade's for many generations, till I took it, that's right. I took it, the developer himself :3")
            };

            string[] mageSpecialties = new string[] { "Fire-Magic", "Lightning-Magic", "Eucladian-Magic", "Light-Magic", "Dark-Magic" };
            int arcaniaGoldCoins = 100000;

            List<(string, int, int)> magicSpells = new List<(string, int, int)> {
                ("Eucladian-Eye", 130, 30),
                ("Developer's Wrath", 300, 70),
                ("Cyclone Strike", 50, 30)
             };

            List<(string itemName, string itemDescription, string itemRarity, int itemPower)> currentInventory = new List<(string, string, string, int)>
                {
                    ("Heartblades Vesper", "A staff that has been a part of the Heartblade's for many generations, till I took it, that's right, I took it, the developer himself :3", "Legendary", 250),
                    ("Healing Potion", "Regenerates +20 health", "Uncommon", 20)
                };

            int specialAtkRecharge = 100;

            List<(string npcName, string npcInformation, string npcAffiliation)> npcsEncountered = new List<(string, string, string)> {
                 ("Veridian Pendragon", "False ranker and solo assassin, very capable and someone not to underestimate.", "Heartblade Association/Pendragon Lineage"),
                ("Evelyn Everbright", "Rank 10 of the Arcania's Magic Council and Guildmaster of Arcania's Magic Council.", "Arcania's Magic Council/Arcane Sentinels"),
                ("Khalid Du-Lucérian", "The true leader of Arcania's Magic Council, identity remains unknown.", "Arcania's Magic Council/Heartblade Association/Lucerian Lineage"),
                ("Cloud (Real Identity - Silver Eucladian-Nine)", "Rank 1 of the Arcania's Magic Council.", "Arcania's Magic Council/Eucladian-Nine Lineage")

            };

            // Create the debugging mage object with the specified arguments
            Mage debuggingMage = new Mage(mageName, mageWeapon, mageSpecialties, arcaniaGoldCoins, magicSpells, currentInventory, specialAtkRecharge, npcsEncountered); // Debugging Mage
            ForestOfMysteries scenario = new ForestOfMysteries();
            int remainingAttempts = 3;

            scenario.forestOfMysteries(debuggingMage, remainingAttempts); // Call the forestOfMysteries method with the Mage object and remaining attempts



            // menu.gameMenu(); // User is first directed to the game menu method

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

                    // Tuple dictionary for the starter weapons, which is associated with a damage value and a rarity type
                    Dictionary<string, (int damage, string rarity, string weaponType, string weaponDescription)> starterMageWeapons = new Dictionary<string, (int damage, string rarity, string weaponType, string weaponDescription)>()
                    {
                        { "Weathered Oakwind", (5, "Common", "Staff", "A primitive staff made with oak, that has been weathered down with time. Has the potential to regain it's once lost status, should it be with the 'Chosen One'.") },
                        { "Ancient Runestaff", (7, "Uncommon", "Staff", "Found in the lost ruins, filled with ancient mysteries yet to be untold.") },
                        { "Runic Wooden Scepter", (3, "Common", "Staff", ".") },
                        { "Dusty Relic Rod", (2, "Common", "Staff", "Dusty and archaic staff, tough luck if you receive this staff.") },
                        { "Emerald Crystal Staff", (10, "Unique", "Staff", "A staff adorned with a seraphic crystal, bolstering its power. This staff is a sign of blessed luck!") }
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


                    List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription)> starterMageWeaponChoices = starterMageWeapons.Select(entry => (entry.Key, entry.Value.damage, entry.Value.rarity, entry.Value.weaponType, entry.Value.weaponDescription)).ToList();

                    List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription)> mageStaff = new List<(string, int, string, string, string)>(starterMageWeaponChoices);

                    Random ranNum = new Random();
                    int random_index = ranNum.Next(0, starterMageWeaponChoices.Count); // Generates random value that'll decide on which weapon the user gets 
                    mageStaff.Add(starterMageWeaponChoices[random_index]); // Append the weapon into the mage staff list

                    List<(string itemName, string itemDescription, string itemRarity, int itemPower)> mageInventory = new List<(string, string, string, int)>();
                    mageInventory.Add((starterMageWeaponChoices[random_index].weaponName, starterMageWeaponChoices[random_index].weaponDescription, starterMageWeaponChoices[random_index].rarity, starterMageWeaponChoices[random_index].damage)); // Store the weapon in the users inventory

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

                    Mage mage = new Mage(mageName, mageStaff, magicSpecialties.ToArray(), arcaniaGoldCoins, magicSpells, mageInventory, mageSpecialAtkRecharge, mageClassNpcsEncountered);
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

                    Dictionary<string, (int damage, string weaponType, string rarity, string weaponDescription)> pirateWeaponChoices = new Dictionary<string, (int, string, string, string)>()
                    {
                        { "Sharp Cutlass", (6, "Sword", "Common", "A short, nimble sword ideal for quick strikes.") },
                        { "Raging Horn", (8, "Longsword", "Common", "A curved longsword evoking power, perfect for forceful attacks.") },
                        { "Somali Pride", (11, "Sword", "Uncommon", "A rare sword of Somali origin, passed down through generations.") },
                        { "Mohamad's Dagger", (20, "Dagger", "Rare", "A concealable dagger named after a famous pirate, perfect for surprise attacks.") },
                        { "Dilapidated Thorn", (14, "Katana", "Rare", "A worn katana with a sharp edge, nicknamed for its piercing ability.") },
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
                    int chosenNormalAttackCount = 0;


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

                    List<(string itemName, string itemDescription, string itemRarity, int itemPower)> pirateInventory = new List<(string, string, string, int)>();

                    // User will be randomly assigned a weapon
                    Random weaponPirateRandom = new Random();
                    int pirateRandomWeaponAssignment = weaponPirateRandom.Next(0, pirateWeaponChoices.Count); // Allow for the random generation between index 0 and length of the dictionary

                    List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription)> pirateWeapon = new List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription)>(); // This list will store the assigned weapon for the pirate class

                    // Access the weapon details directly using the index
                    var randomPirateWeapon = pirateWeaponChoices.ElementAt(pirateRandomWeaponAssignment);

                    // Add the weapon details to the pirateWeapon list
                    pirateWeapon.Add((randomPirateWeapon.Key, randomPirateWeapon.Value.damage, randomPirateWeapon.Value.weaponType, randomPirateWeapon.Value.rarity, randomPirateWeapon.Value.weaponDescription));

                    // Add the weapon name to the pirateInventory list
                    pirateInventory.Add((randomPirateWeapon.Key, randomPirateWeapon.Value.weaponDescription, randomPirateWeapon.Value.rarity, randomPirateWeapon.Value.damage));



                    // User will be randomly assigned an aura
                    Random auraPirateRandom = new Random();
                    int pirateAuraRoll = auraPirateRandom.Next(0, pirateWeaponAuras.Count); // Allow for the random generation between index 0 and length of the dictionary

                    List<(string auraName, int damage, string rarity, string description)> pirateWeaponAura = new List<(string auraName, int damage, string rarity, string description)>(); // Aura will be stored in a list

                    var randomAura = pirateWeaponAuras.ElementAt(pirateAuraRoll); // Assign a random aura from the dictionary

                    // Add the random aura to the list
                    pirateWeaponAura.Add((randomAura.Key, randomAura.Value.damage, randomAura.Value.rarity, randomAura.Value.auraDescription));


                    SomaliPirate myPirate = new SomaliPirate(pirateName, pirateWeapon, pirateWeaponAura, chosenPirateNormalAttacks, chosenSpecialAttacks, pirateInventory, arcaniaGoldCoins, specialAtkRecharge, pirateClassNpcsEncountered); // Generate the pirate details

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

        public void forestOfMysteries(CharacterDefault character, int remainingAttempts = 3)
        {
            UIManager UI = new UIManager();
            SmoothConsole smoothPrinting = new SmoothConsole();



            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine("Forest of Mysteries");
            smoothPrinting.PrintLine("--------------------------------------------------");

            if (remainingAttempts == 3)
            {
                smoothPrinting.RapidPrint(oneTimeFixedScenario + "\n");
            }
            else
            {
                Console.WriteLine(oneTimeFixedScenario + "\n");
            }


            Console.WriteLine("\n[Available Commands:]");
            smoothPrinting.PrintLine("\n1. Fight: Confront the Dragon (TESTING)");
            smoothPrinting.PrintLine("\n2. North: Move northward along the path (N/A)");
            smoothPrinting.PrintLine("\n3. Inventory: View your current inventory of items (N/A)");
            smoothPrinting.PrintLine("\n4. Help: Display a list of available commands (N/A)");
            smoothPrinting.RapidPrint("\nEnter a corresponding value: ");
            string firstSelection = Convert.ToString(Console.ReadLine());

            switch (firstSelection)
            {
                case "1":
                    if (character is Mage)
                    {
                        MageConfrontation((Mage)character);
                    }
                    else if (character is SomaliPirate)
                    {
                        SomaliPirateConfrontation((SomaliPirate)character);
                    }
                    break;
                case "2":
                    // Move northward
                    break;
                case "3":
                    character.CheckInventory();
                    UI.PromptUserToContinue();
                    forestOfMysteries(character, remainingAttempts - 1); // Recurse to avoid breaking program haha
                    break;
                default:
                    Console.WriteLine("\nInvalid input, please try again");
                    Console.ReadKey();
                    Console.Clear();
                    forestOfMysteries(character, remainingAttempts - 1);
                    break;
            }
        }

        private void MageConfrontation(Mage mage)
        {
            SmoothConsole smoothPrinting = new SmoothConsole();
            Console.Clear();
            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine("Forest of Mysteries: Confronting the Dragon");
            smoothPrinting.PrintLine("--------------------------------------------------");

            // Mage challenges the dragon
            smoothPrinting.RapidPrint($"\n{mage.name}: \"Stop flying away, and face me at once!\"\n");

            // The dragon responds with hostility
            smoothPrinting.RapidPrint("\nDragon: \"This mere mortal dares speak to me in such manner? So be it, you shall now face my wrath!\"\n");

            // The realization that the dragon can speak surprises the character
            // However, it's too late to retract their actions
            smoothPrinting.RapidPrint("\n*You are taken aback to hear that the Dragon can speak, though it's come to a point in time where you cannot take back your actions*\n");

            // The dragon introduces itself as Windsom, the Guardian of the forests
            smoothPrinting.RapidPrint("\nDragon: \"Mortal, you know not the gravity of your words. I am Windsom, the Guardian of these forests, and you have trespassed into my domain.\"\n");

            // {mage.name} expresses surprise at the dragon's name
            smoothPrinting.RapidPrint($"\n{mage.name}: \"Windsom? Never heard that name before.\"\n");

            // The character ponders whether the dragon knows about their summoning
            smoothPrinting.RapidPrint("\nMC's Thoughts: \"Perhaps he knows about why I've been summoned to this world...\"\n");

            // {mage.name} directly asks the dragon about their summoning
            smoothPrinting.RapidPrint($"\n{mage.name}: \"Do you know why I've been summoned to this world?\"\n");

            // The dragon responds cryptically, challenging the character to prove their worth
            smoothPrinting.RapidPrint("\nWindsom (The Guardian Dragon): \"Ah, the mysteries of summonings. Perhaps I do, perhaps I don't. But why should I reveal such knowledge to a mere mortal like you? Prove your worth, Mage. Defeat me in battle, and perhaps then, I shall consider sharing what I know.\"\n");


            smoothPrinting.RapidPrint("\n*Windsom slowly sets down into the forest, it's wings shuddering the leaves, crushing branches and any other obstacle that gets in its way, setting the stage for battle*\n");

            // Prompt the user to engage in combat
            smoothPrinting.RapidPrint("\nAre you ready to engage in combat? Press any key to start.");
            Console.ReadKey(); // Wait for user input
            Console.Clear(); // Clear the console to prepare for combat


            string dragonName = "Windsom";
            int specialAtkRecharge = 100;
            int currentMobHealth = 350;
            int maxMobHealth = 350;


            Dictionary<string, int> normalAtkNames = new Dictionary<string, int>()
            {
                { "Dragon's Claw", 30 },
                { "Dragon's Breath", 40 },
                { "Raging Tempest", 50 }
            };

            Dictionary<string, (int, string)> specialAtkNames = new Dictionary<string, (int, string)>()
            {
                { "Arcane Nexus", (100, "Eucladian-Magic") },
                { "Umbral Charge", (120, "Dark-Magic") },
                { "Rampant Flame Charge", (200, "Fire-Magic") }
            };



            // Dictionary that contains weapon name, damage, rarity, and weapon type (item drops)
            Dictionary<string, (int damage, string rarity, string weaponType, string weaponDescription)> itemDrop = new Dictionary<string, (int, string, string, string)>()
            {
                { "Frostfire Fang", (65, "Unique", "Staff", "Forged in the icy flames of the dragon's breath, this fang drips with frostfire, capable of freezing enemies in their tracks.") },
                { "Serpent's Gaze", (50, "Unique", "Rapier/Sword", "Crafted from the scales of the ancient serpent, this gaze holds the power to petrify foes with a single glance.") },
                { "Chaosfire Greatsword", (60, "Unique", "Greatsword/Sword", "Tempered in the chaosfire of the dragon's lair, this greatsword burns with an insatiable hunger for destruction.") },
                { "Nightshade Arc", (55, "Unique", "Bow", "Fashioned from the sinew of the nocturnal shadows, this bow strikes with deadly accuracy under the cover of darkness.") },
                { "Aerith's Heirloom", (80, "Legendary", "Staff", "Once wielded by the legendary Aerith, this staff channels the primordial magic of creation itself, capable of reshaping reality.") },
                { "Eucladian's Aura", (55, "Legendary", "Aura", "Embrace the ethereal aura of the Eucladian, granting unmatched protection against all forms of magic and malevolence.") }
            };



            // Create a new instance of the dragon for combat
            Dragon windsom = new Dragon(dragonName, normalAtkNames, specialAtkNames, specialAtkRecharge, currentMobHealth, maxMobHealth, itemDrop);

            // Exert pressure based on mage's level
            windsom.exertPressure(mage, windsom); // Pass mage and dragon class with relevant attributes and methods here
            bool quickDisplay = false;

            // Engage the combat system
            mage.DisplayMageStatus(mage, windsom, quickDisplay);
        }

        private void SomaliPirateConfrontation(SomaliPirate pirate)
        {
            // Handle confrontation logic for Somali Pirate
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
            smoothPrinting.RapidPrint("\n7. Continents (N/A)\n");
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
                        smoothPrinting.RapidPrint("Invalid input, please try again.");
                        Console.ReadKey(); // Allow user to see error message
                        dashboard(character); // Due to lack of functionality, return user back to the dashboard
                    }
                    else
                    {
                        smoothPrinting.RapidPrint("Invalid input, please try again.");
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
    public void DisplayProgressBar(string title, float currentValue, int maxValue, int barLength)
    {
        // Calculate the percentage
        double percentage = currentValue / maxValue;

        // Calculate the number of filled characters
        int filledLength = (int)Math.Round(percentage * barLength);

        // Generate the progress bar
        string progressBar = new string('█', filledLength) + new string(' ', barLength - filledLength);

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
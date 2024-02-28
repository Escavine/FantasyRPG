using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml.Linq;


// FantasyRPG: A console based RPG game, which sole purpose is to improve on my current programming skills (mainly OOP as that is my weakness).

namespace FantasyRPG
{
    class CharacterDefault // fixed preset for all classes
    {
        // Generic character attributes
        public string name;
        public int health;
        public List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription)> weapon;
        public float numOfPotionsInInventory;
        public float maxPotions;
        public int mana;
        public string[] currentInventory; // Will contain the users potions and other weapons.
        public int arcaniaGoldCoins; // Currency for the city of Arcanith
        public int specialAtkRecharge;// Percentage value, going upto 100%
        public List<(string npcName, string npcDescription, string npcAffiliation)> npcsEncountered;
        // public int atk;
        // public int def;

        // Levelling attributes
        public float exp;
        public int level;
        private int experienceRequiredForNextLevel;
        // public int randomDyingChance;

        public CharacterDefault(string _name, List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription)> _weapon, string[] _currentInventory, int _arcaniaGoldCoins, int specialAtkRecharge, List<(string npcName, string npcDescription, string npcAffiliation)> _npcsEncountered) // Default preset for all classes during the start of the game :3
        {
            name = _name;
            weapon = _weapon; // WIll store the details of the given weapon (i.e. weapon name, type, damage, etc.)
            currentInventory = _currentInventory;
            npcsEncountered = null; // During the start of the game, the user will have not encountered any NPC's.
            specialAtkRecharge = 0; // Preset to 0%, as user attacks this will linearly rise
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

    class MobDefault // Mob preset for the game
    {
        public string name;
        public int normalAtkDmg, specialAtkDmg, specialAtkRecharge, mobHealth;


        // Mobs can have different attack names and varying item drops, each associated with a rarity and damage value
        public Dictionary<string, (int, string, string)> itemDrop; // First string defines the weapon name, second integer defines the weapon damage, thirs stirng defines the weapon rarity and fourth string defines the weapon type
        public Dictionary<string, int> normalAtkNames;
        public Dictionary<string, (int, string)> specialAtkNames;

        public MobDefault(string _name, Dictionary<string, int> _normalAtkNames, Dictionary<string, (int, string)> _specialAtkNames, int _normalAtkDmg, int _specialAtkDmg, int _specialAtkRecharge, int _mobHealth, Dictionary<string, (int, string, string)> _itemDrop) // Presets for all mobs within the game (i.e. dragons, shadow stalkers, arcane phantons, crawlers etc.)
        {
            name = _name;
            normalAtkNames = _normalAtkNames;
            specialAtkNames = _specialAtkNames;
            normalAtkDmg = _normalAtkDmg;
            specialAtkDmg = 100; // If your a beginner, this is a one shot 
            itemDrop = _itemDrop; // Mobs have a chance to drop a random item once they die
            specialAtkRecharge = 100;
            mobHealth = _mobHealth;

        }

    }


    class Crawler : MobDefault // Crawler class
    {
        SmoothConsole smoothPrinting = new SmoothConsole();

        public Crawler(string _name, Dictionary<string, int> _normalAtkNames, Dictionary<string, (int, string)> _specialAtkNames, int _normalAtkDmg, int _specialAtkDmg, int _specialAtkRecharge, int _mobHealth, Dictionary<string, (int, string, string)> _itemDrop) : base(_name, _normalAtkNames, _specialAtkNames, _normalAtkDmg, _specialAtkDmg, _specialAtkRecharge, _mobHealth, _itemDrop)
        {

            // Default presets for a crawler, inherited from the mob default class

            name = "Crawler";
            mobHealth = 20; // Crawlers are very weak creatures, and by default have 20 health

            // Dictionary containing crawler attacks and their associated damage value
            Dictionary<string, int> normalAtkNames = new Dictionary<string, int>() // Preset name for all dragon's normal attacks
            {
                { "Crawler's Scratch", 5 },
                { "Crawler's Screech", 10 },
                { "Crawler's Bite", 3 }
            };

            // Dictionary that contains weapon name, damage, rarity and weapon type (item drops)
            Dictionary<string, (int, string, string)> itemDrop = new Dictionary<string, (int, string, string)>()
            {
                { "Staff of Spite", (7, "(Common)", "Staff") },
                { "Crawler's Revant", (10, "(Uncommon)", "Rapier/Sword") },
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

        public Dragon(string _name, Dictionary<string, int> _normalAtkNames, Dictionary<string, (int, string)> _specialAtkNames, int _normalAtkDmg, int _specialAtkDmg, int _specialAtkRecharge, int _mobHealth, Dictionary<string, (int, string, string)> _itemDrop) : base(_name, _normalAtkNames, _specialAtkNames, _normalAtkDmg, _specialAtkDmg, _specialAtkRecharge, _mobHealth, _itemDrop)
        {
            // Default presets for a dragon, inherited from the mob default class

            name = "Dragon";
            mobHealth = 350; // Dragons have 350HP by default

            // Dictionary containing dragon attacks and their associated damage value
            Dictionary<string, int> normalAtkNames = new Dictionary<string, int>() // Preset names for all dragon's normal attacks
            {
                { "Dragon's Claw", 30 },
                { "Dragon's Breath", 40 },
                { "Raging Tempest", 50 }
            };

            // Dictionary that contains weapon name, damage, rarity and weapon type (item drops)
            Dictionary<string, (int, string, string)> itemDrop = new Dictionary<string, (int, string, string)>()
            {
                { "Etherial Froststaff", (50, "Unique", "Staff") },
                { "Nightfall Rapier", (50, "Unique", "Rapier/Sword") },
                { "Chaosfire Greatsword", (60, "Unique", "Greatsword/Sword") }, // OP item drops
                { "Nightshade Arc", (55, "Unique", "Bow") },
                { "Aerith's Heirloom", (80, "Legendary", "Staff") },
                { "Eucladian's Aura", (55, "Legendary", "Aura") } // Should the individual get lucky, then they could potentially get an aura drop, this is only equipabble by knights, pirates, shadowwraths etc.
            };

            Dictionary<string, (int, string)> specialAtkNames = new Dictionary<string, (int, string)>() // Preset names for all dragon's special attacks
            {
                { "Arcane Nexus", (100, "Eucladian-Magic")}, // Eucladian type ULT
                { "Umbral Charge", (120, "Dark-Magic")}, // Dark type ULT
                { "Rampant Flame Charge", (200, "Fire-Magic") } // Flame type ULT
            };

            itemDrop = _itemDrop;
            normalAtkNames = _normalAtkNames;
        }

        // Future reference: Create different types of dragons that have weaknesses (i.e. water dragons, shadow dragons etc)

        public void exertPressure(int level) // Dragons will use to make humans fear them, should the users level be lower than expected
        {
            if (level < 10) // Should the users level be below level 10, then the dragon will exert pressure to the individual, reducing their attack value.
            {
                Console.ForegroundColor = ConsoleColor.Red;
                smoothPrinting.FastPrint("Your level is lower than expected, the Dragon exerts pressure, reducing your attack damage");
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
                mobHealth = mobHealth + 20; // Slight health regen
                specialAtkRecharge = 0; // The special attack has been used by this point, so therefore it should be set to zero.
            }

        }

        public void dragonDeath(int mobHealth, int exp)
        {
            if (mobHealth == 0)
            {
                Random itemDropChance = new Random();
                int dropChance = itemDropChance.Next(0, 10); // 9% drop rate, due to OP item drops
                smoothPrinting.FastPrint("\nDragon has been successfully defeated!");

                if (dropChance == 0)
                {
                    dropItem(dropChance); // Should the random number be zero, then the mob will drop an item
                }

                exp += 300; // User gains huge exp from defeating the dragon 

            }

        }

        public void dropItem(int dropChance)
        {


        }
    }


    class Knight : CharacterDefault // Knight class properties and methods
    {
        public string normalAtkName;
        public string specialAtkName; // Remove these static features
        public int specialAtkDmg;
        public int normalAtkDmg;

        public Knight(string _name, List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription)> _weapon, string _specialAtkName, string[] _currentInventory, int _arcaniaGoldCoins, int _specialAtkRecharge, List<(string npcName, string npcDescription, string npcAffiliation)> _npcsEncountered) : base(_name, _weapon, _currentInventory, _arcaniaGoldCoins, _specialAtkRecharge, _npcsEncountered)
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
        // Properties for common wizard attributes
        public List<(string magicSpell, int damage, int manaRequirement)> magicSpells = new List<(string magicSpell, int damage, int manaRequirement)>();
        string[] magicSpecialties; // User can have multiple magic specialties
        public int spellUsage; // Spell usage to keep spells in control

        public Mage(string _name, List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription)> _weapon, string[] _magicSpecialties, int _arcaniaGoldCoins, List<(string magicSpell, int damage, int manaRequirement)> _magicSpells, string[] _currentInventory, int _specialAtkRecharge, List<(string npcName, string npcDescription, string npcAffiliation)> _npcsEncountered) : base(_name, _weapon, _currentInventory, _arcaniaGoldCoins, _specialAtkRecharge, _npcsEncountered)
        {
            name = _name;
            weapon = _weapon;
            magicSpecialties = _magicSpecialties;
            currentInventory = _currentInventory;
            magicSpells = _magicSpells; // Predefined variables for every new wizard in the game
            spellUsage = 5;
        }


        // Methods for a mage
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
            // Generate a random value for exp
            Random rd = new Random();
            int rand_num = rd.Next(1, 5);

            Console.WriteLine(name + " has decided to improve on their skills, ", " their experience has increased by " + rand_num);
            exp = exp + rand_num;

        }

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

            smoothPrinting.SlowPrint("Mage's Prestiege, congrats!\n");
            smoothPrinting.RapidPrint("\n" + name + "'s" + " current known magic specialities:" + "\n");


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

        public SomaliPirate(string _name, List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription)> _weapon, List<(string auraName, int damage, string rarity, string description)> _weaponAura, List<(string attack, int damage, int manaRequirement, string elementType, string description)> _pirateNormalAtks, List<(string attack, int damage, int manaRequirement, string elementType, string description)> _pirateSpecialAtks, string[] _currentInventory, int _arcaniaGoldCoins, int _specialAtkRecharge, List<(string npcName, string npcDescription, string npcAffiliation)> _npcsEncountered) : base(_name, _weapon, _currentInventory, _arcaniaGoldCoins, _specialAtkRecharge, _npcsEncountered)
        {
            name = _name;
            weapon = _weapon;
            weaponAura = _weaponAura;
            pirateNormalAtks = _pirateNormalAtks; // Presets for all new Somali Pirates in the game
            pirateSpecialAtks = _pirateSpecialAtks;
            currentInventory = _currentInventory; // This will be readjusted to a list in the future
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
            // GameMenu menu = new GameMenu();
            // MagicCouncil encounter = new MagicCouncil(); // Debugging
            // string name = "Silver"; // Debugging
            // encounter.firstEncounter(name); // Debugging
            // FirstScenario firstScenario = new FirstScenario();
            // firstScenario.usersFirstJourney("Tristian");

            FirstScenario scenario = new FirstScenario();
            string name = "Eurelian";
            int remainingAttempts = 3;
            scenario.usersFirstJourney(name, remainingAttempts);

           // menu.gameMenu(); // User is first directed to the game menu method

            // List<(string npcName, string npcDescription, string npcAffiliation)> npcsEncountered = new List<(string npcName, string npcDescription, string npcAffiliation)>() // Debugging: populating data
            // {
               //  ("Kaelen Stormer", "Rank 6 of Arcania's Magic Council, known for being one of the most formiddable Dark Elves conquering his enemies with meticulous assasination capabilities.", "Arcania's Magic Council"),
                // ("Silver Eucladian-Nine", "The real identity of Cloud, Rank 1 in Arcania's Magic Council.", "Arcania's Magic Council/Eucladian Lineage"),
                // ("Evelyn Everbright", "Rank 10 of Arcania's Magic Council, known for her gracious beauty that graces wherever she goes.", "Arcania's Magic Council/Arcane Sentinels"),
                // ("Mo Blade", "Rank 3 of Arcania's Magic Council, known to be one of the most vicious pirates around!", "Arcania's Magic Council/Red Sea")
            // };


            // gameDashboard dash = new gameDashboard();
            // dash.dashboard(name, npcsEncountered);

        }


        public void gameMenu() // After user information is authenticated, they'll be lead here
        {
            SmoothConsole smoothOutput = new SmoothConsole(); // Initialize the smooth console
            int userChances = 3; // Will be used for recursive measures to prevent brute force and idiotic input
            // Future reference: Implementing AI mobs and perhaps AI individuals

            int? userChoice; // Used for the start of the game
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
            userChoice = Convert.ToInt32(Console.ReadLine());

            switch (userChoice)
            {
                case 1:
                    Console.Clear();
                    ClassSelection selectClass = new ClassSelection(); // Create a new game session
                    selectClass.userClass(); // Proceed to let the user pick a character class
                    break;
                case 2:
                    loadingSaveData(userChances); // Lead user to the method
                    break;
                case 3:
                    Console.Clear(); // Neatness structuring
                    userChoice = null;
                    helpSection(userChances); // Lead user to the method
                    break;
                case 4:
                    makeGameSuggestion(); // Lead user to the method
                    break;
                case 5:
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

            int? userChoice; // Define the user choice

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
            userChoice = Convert.ToInt32(Console.ReadLine());

            switch (userChoice) // Future reference: Rather than have a userchoice fixed to a single method, add multiple methods for different classes (i.e. a mage class if a user chooses the mage role etc, that way you can implement recursion if the user wants to reset their details)
            {
                // Should the user decided to become a Mage
                case 1:
                    int choiceIncrementer = 1; // Used to increment the user choice when picking magic types
                    int startMageJourneyInput;
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


                    List<string> mageInventory = new List<string>();
                    mageInventory.Add(starterMageWeaponChoices[random_index].weaponName); // Store the weapon in the users inventory

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

                    Mage newWizard = new Mage(mageName, mageStaff, magicSpecialties.ToArray(), arcaniaGoldCoins, magicSpells, mageInventory.ToArray(), mageSpecialAtkRecharge, mageClassNpcsEncountered);

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
                    startMageJourneyInput = Convert.ToInt32(Console.ReadLine()); // Register the user input


                    switch (startMageJourneyInput)
                    {
                        case 1:
                            Console.Clear(); // Neatness
                            smoothPrinting.FastPrint("First scenario\n");
                            Console.WriteLine("You will now be sent to the world of Arcania, make sure to not die.");
                            Console.Clear(); // Neatness
                            Console.ForegroundColor = ConsoleColor.White; // Reset the console colour
                            FirstScenario wizardJourney = new FirstScenario(); // Journey start!
                            wizardJourney.usersFirstJourney(mageName);
                            break;
                        case 2:
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
                        case 3:
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
                            Console.Clear();
                            break;

                    }
                    break;

                case 2:
                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.Red;
                    smoothPrinting.RapidPrint("\nKnight's aren't avaliable as of present :3");
                    Console.ReadKey();
                    break;


                case 3:
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

                    List<string> pirateInventory = new List<string>();

                    // User will be randomly assigned a weapon
                    Random weaponPirateRandom = new Random();
                    int pirateRandomWeaponAssignment = weaponPirateRandom.Next(0, pirateWeaponChoices.Count); // Allow for the random generation between index 0 and length of the dictionary

                    List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription)> pirateWeapon = new List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription)>(); // This list will store the assigned weapon for the pirate class

                    // Access the weapon details directly using the index
                    var randomPirateWeapon = pirateWeaponChoices.ElementAt(pirateRandomWeaponAssignment);

                    // Add the weapon details to the pirateWeapon list
                    pirateWeapon.Add((randomPirateWeapon.Key, randomPirateWeapon.Value.damage, randomPirateWeapon.Value.weaponType, randomPirateWeapon.Value.rarity, randomPirateWeapon.Value.weaponDescription));

                    // Add the weapon name to the pirateInventory list
                    pirateInventory.Add(randomPirateWeapon.Key);



                    // User will be randomly assigned an aura
                    Random auraPirateRandom = new Random();
                    int pirateAuraRoll = auraPirateRandom.Next(0, pirateWeaponAuras.Count); // Allow for the random generation between index 0 and length of the dictionary

                    List<(string auraName, int damage, string rarity, string description)> pirateWeaponAura = new List<(string auraName, int damage, string rarity, string description)>(); // Aura will be stored in a list

                    var randomAura = pirateWeaponAuras.ElementAt(pirateAuraRoll); // Assign a random aura from the dictionary

                    // Add the random aura to the list
                    pirateWeaponAura.Add((randomAura.Key, randomAura.Value.damage, randomAura.Value.rarity, randomAura.Value.auraDescription));


                    SomaliPirate newPirate = new SomaliPirate(pirateName, pirateWeapon, pirateWeaponAura, chosenPirateNormalAttacks, chosenSpecialAttacks, pirateInventory.ToArray(), arcaniaGoldCoins, specialAtkRecharge, pirateClassNpcsEncountered); // Generate the pirate details

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
                            FirstScenario pirateJourney = new FirstScenario(); // Journey start!
                            pirateJourney.usersFirstJourney(pirateName);
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


                case 4:
                    Console.WriteLine("After long endurance of physical training, you develop eyes as keen as an owl and your bowmanship is first class.");
                    Console.WriteLine("What is your name?");
                    string archerName = Convert.ToString(Console.ReadLine());

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
                case 7:
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



    public class FirstScenario // Once the user selects a class, they'll proceed onto their journey
    {

        // Customary scenarios will be used to allow dynamicness to the game and make it less monotomous
        // string[] customaryScenarios = { "You embark on a long journey, you find yourself lost midway throughout the journey. There appears a dragon, with fangs as sharp as blades and a gaze so intense that you begin to question your fighting prowess despite your training. What do you do?" };

        // User first encounters the dragon in the forest of mysteries.
        string oneTimeFixedScenario = "\nYou spawn in the Forest of Mysteries, now with the understanding of the rulings within the world along with a primitive understanding of mana. As you keep exploring this vast forest, you eventually find yourself lost midway, your eyes surrounded by vast levels of fog, mitigating your view of the perspective ahead. Close by, there appears a dragon, with fangs as sharp as blades and a gaze so intense that you begin to question whether you’ll survive or not. ";
        bool completedFirstScenario = false; // This is a measure to see if the user has completed this scenario in the Forest of Mysteries, should this be the case, they'll no longer see this scenario when exploring the forest
        string? firstSelection;

        public void usersFirstJourney(string name, int remainingAttempts = 3) // User gets 3 attempts to ensure that they put the correct input, should they fail all 3, the game terminates.
        {
            SmoothConsole smoothPrinting = new SmoothConsole();
            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine("Forest of Mysteries");
            smoothPrinting.PrintLine("--------------------------------------------------");

            if (remainingAttempts == 3)
            {
                smoothPrinting.RapidPrint(oneTimeFixedScenario + "\n"); // This is all an indirect skip functionality, as it is quite complicated to manually implement one, saves users time and is convenient
            }

            Console.WriteLine("\n[Available Commands:]");
            smoothPrinting.PrintLine("\n1. Fight: Confront the dragon (N/A)");
            smoothPrinting.PrintLine("\n2. North: Move northward along the path (N/A)");
            smoothPrinting.PrintLine("\n3. Inventory: View your current inventory of items (N/A)");
            smoothPrinting.PrintLine("\n4. Help: Display a list of available commands (N/A)");
            smoothPrinting.RapidPrint("\nEnter a corresponding value: ");
            firstSelection = Convert.ToString(Console.ReadLine());


            switch (firstSelection)
            {
                case "1":
                    confrontingTheDragon(); // Function call to enable combat functionality against the dragon
                    break;
                default:
                    Console.WriteLine("\nInvalid input, please try again");
                    Console.ReadKey(); // Let the user know, before clearing the console
                    Console.Clear();
                    usersFirstJourney(name, remainingAttempts - 1); // Recurse and reduce remaining attempts by 1
                    break;
            }

            void confrontingTheDragon() // Incomplete
            {
                Console.Clear(); // Clear the console, before proceeding to this section of the plot.
                smoothPrinting.PrintLine("--------------------------------------------------");
                smoothPrinting.PrintLine("Forest of Mysteries");
                smoothPrinting.PrintLine("--------------------------------------------------");

                smoothPrinting.RapidPrint("You decide that you want to confront the dragon, rather than cower in fear. Inside you are conflicted, as to whether you've made a brave or incredibly stupid decision" +
                    ", anyhow you slowly make your way across");
            }


        }

    }

    public class Combat // Will be used to initialize the combat system
    {
        public void Fighting()
        {



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
        // One-time executable function: Will not happen again.
        public void firstEncounter(string name, List<(string npcName, string npcDescription, string npcAffiliation)> npcsEncountered) // This will be the first meeting between the MC and the other council members (not all of them are present at that given moment)
        {
            Console.Clear(); // Clear the console for neatness
            SmoothConsole smoothPrinting = new SmoothConsole();
            Console.ForegroundColor = ConsoleColor.White;
            string choice1, choice2, choice3; // NPC interaction choices

            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine("First Encounter - Arcania's Magic Council");
            smoothPrinting.PrintLine("--------------------------------------------------");

            // Explanation of Arcania's Magic Council being aware of the current situation at hand
            smoothPrinting.RapidPrint("\nThe Magic Council are naturally aware of the ongoing situation, with tension rising within Paralax (Dragon City), between the Draconith and Vesperon family and due to the growing conflict, an emergency meeting is held with all council members. Evelyn invites you as a representative along with Eurelian Frostweaver to the meeting, other council members also invite their fellow representatives. For safety purposes, all representatives are to wear a mask to conceal their identities. ");

            smoothPrinting.RapidPrint("\n\nAffirmative? Press any key to continue...");
            Console.ReadKey(); // Read the users input, before generating more output.
            Console.Clear(); // Clear the console for neatness

            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine("First Encounter - Arcania's Magic Council");
            smoothPrinting.PrintLine("--------------------------------------------------");

            smoothPrinting.RapidPrint("\nAs you make your way towards the Magic Council Hall alongside Evelyn, you can't help but marvel at the hyper-detailed infrastructures that tower over you. Their imposing presence commands attention, and you find yourself drawn to their intricate designs. Curious, you turn to Evelyn and inquire about the significance of these structures. With a knowing smile, she explains that they are different idols of the previous 1st rankers and generations back. Each idol holds a different weapon, symbolizing the unique specialties of its respective ranker. The idols vary in posture and exude a sense of power and mastery over their chosen disciplines. ");
            Console.WriteLine();
            smoothPrinting.RapidPrint("\nAffirmative? Press any key to continue...");
            Console.ReadKey(); // Read the users input, before generating more output.
            Console.Clear(); // Clear the console for neatness

            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine("First Encounter - Arcania's Magic Council");
            smoothPrinting.PrintLine("--------------------------------------------------");

            smoothPrinting.RapidPrint("\nWhile walking with Evelyn, you see a bulletin that contains a bunch of names, associted with a ranking and extra details and enquire to her what it exactly entails. She briefly explains that it is the ranking board for the current Arcania's Magic Council members, and lets you see from up-close\n");
            smoothPrinting.RapidPrint("\nAffirmative? Press any key to continue...");
            Console.ReadKey(); // Read the users input, before generating more output.
            Console.Clear(); // Clear the console for neatness

            Console.ForegroundColor = ConsoleColor.Yellow; // Display the rankings in a yellow color
            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine("Arcania's Magic Council - Current Rankings");
            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.RapidPrint("Rank 1: ??? - Rank: S** (Class: ???, Race: ???) \r\n\r\nRank 2: ??? - Rank: S* (Class: ???, Race: ???) \r\n\r\nRank 3: ??? - Rank: S (Class: ???, Race: ???) \r\n\r\nRank 4: Lister Everbright - Rank: A* (Class: Knight, Race: Elf) \r\n\r\nRank 5: Aurelia Eucladian-Nine - Rank: S- (Class: Mage, Race: Human) \r\n\r\nRank 6: Kaelen Stormer - Rank: S* (Class: Assassin, Race: Dark Elf) \r\n\r\nRank 7: Lyra Leywin - Rank: S- (Class: Necromancer, Race: Demon) \r\n\r\nRank 8: Windsom - Rank: A* (Class: Guardian, Race: Dragon) \r\n\r\nRank 9: Selene - Rank: A (Class: Succubus, Race: Demon) \r\n\r\nRank 10: Evelyn Everbright - Rank: S- (Class: High-Elf Warrior, Race: Elf) ");
            Console.ForegroundColor = ConsoleColor.White; // Revert to original color for terminal
            Console.WriteLine();
            smoothPrinting.RapidPrint("\nMC's Inner Thoughts: “The top 3 aren't even known? I seriously wonder how powerful they are, perhaps they have a clue as to how I got summoned into this world...”\n");
            smoothPrinting.RapidPrint("\nAffirmative? Press any key to continue...");
            Console.ReadKey(); // Read the users input, before generating more output.
            Console.Clear(); // Clear the console for neatness

            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine("First Encounter - Arcania's Magic Council");
            smoothPrinting.PrintLine("--------------------------------------------------");

            smoothPrinting.RapidPrint("As you continue walking, you can't help but feel a sense of reverence for those who came before, their legacies immortalized in stone and metal. It's a reminder of the long history and tradition that surrounds the Magic Council, and you can't help but feel honored to be a part of it. Before proceeding inside the Hall, Evelyn stumbles upon her brother, who she hasn’t met for a long time due to her duties as a Guildmaster, they end up meeting each other and talking for some time, there she introduces you, the ‘MC’ and he greets you with a heartwarming smile, just like Evelyn, you can clearly tell that they are siblings. ");
            smoothPrinting.RapidPrint("\n\nAffirmative? Press any key to continue...");
            Console.ReadKey(); // Read the users input, before generating more output.
            Console.Clear(); // Clear the console for neatness

            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine("First Encounter - Arcania's Magic Council");
            smoothPrinting.PrintLine("--------------------------------------------------");

            smoothPrinting.RapidPrint("Lister's overwhelming aura exudes strength and confidence, casting a palpable sense of authority and prowess. It surrounds him like an invisible shield, commanding respect and attention from those around him. The intensity of his presence is undeniable, leaving an indelible impression on all who encounter him. It's a combination of his unwavering determination, honed skill, and unwavering commitment to his craft that contribute to the formidable aura he emits. In the presence of Lister, one can't help but feel a sense of reverence and admiration for his formidable abilities and commanding demeanor. ");
            smoothPrinting.RapidPrint("\n\nAffirmative? Press any key to continue...");
            Console.ReadKey(); // Read the users input, before generating more output.
            Console.Clear(); // Clear the console for neatness

            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine("First Encounter - Arcania's Magic Council");
            smoothPrinting.PrintLine("--------------------------------------------------");

            // Lazy output, fix this when you get the time so it is structurered properly
            smoothPrinting.RapidPrint("\nEvelyn: “Hey Lister, it’s been a long time since we last met. How are you doing?” \r\n\r\nLister: “Oh Evelyn, it really has been a while. You seem even more capable and stronger now. I’m doing great. By the way, who is that with you? I don’t think I’ve seen him before.” \r\n\r\nEvelyn: “He's a recent addition to my guild.” \r\n\r\nLister: “Is that so? Well, I'm usually good at reading people, but I can’t seem to read him. Guess you picked out someone interesting.” ");
            smoothPrinting.RapidPrint("\n\nAffirmative? Press any key to continue...");
            Console.ReadKey(); // Read the users input, before generating more output.
            Console.Clear(); // Clear the console for neatness

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

                choice1 = Convert.ToString(Console.ReadLine());

                switch (choice1)
                {
                    case "1":
                        smoothPrinting.RapidPrint($"\n{name}: “My name is {name}, it is an honor to meet you.”");
                        smoothPrinting.RapidPrint($"\nLister: “Nice to meet you {name}, you seem quite capable, and can potentially see you replacing our predacessors at some point in the ranks ha-ha-ha?”");
                        // Future reference: Add a impression increase for Lister NPC
                        break;


                    case "2":
                        smoothPrinting.RapidPrint($"\n{name}: “My identity remains a secret, that I shall not reveal”");
                        smoothPrinting.RapidPrint("\nLister: “He's really interesting, though I feel we won't get along...”");
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
            smoothPrinting.RapidPrint("\n\nAffirmative? Press any key to continue...");

            // This section will be removed when this mission comes to become an integral part of the game, right now this is all for debugging purposes
            Console.ReadKey(); // Read the users input, before generating more output.
            Console.Clear(); // Clear the console for neatness
            gameDashboard dash = new gameDashboard();
            dash.dashboard(name, npcsEncountered);

        }
    }


    public class gameDashboard
    {
        public void dashboard(string name, List<(string npcName, string npcDescription, string npcAffiliation)> npcsEncountered) // Will display the user dashboard for the game
        {
            string userInput;
            SmoothConsole smoothPrinting = new SmoothConsole();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Gray;
            smoothPrinting.PrintLine("--------------------------------------------------");
            smoothPrinting.PrintLine("FantasyRPG: " + name + "'s " + "Dashboard");
            smoothPrinting.PrintLine("--------------------------------------------------");

            smoothPrinting.RapidPrint("\n1. Main Storyline (N/A)\n");
            smoothPrinting.RapidPrint("\n2. Infinite Dungeon (N/A)\n");
            smoothPrinting.RapidPrint("\n3. Arcane Sentinels (N/A)\n");
            smoothPrinting.RapidPrint("\n4. Shop (N/A)\n");
            smoothPrinting.RapidPrint("\n5. NPC's Encountered (N/A)\n");
            smoothPrinting.RapidPrint("\nEnter a corresponding value: ");

            userInput = Convert.ToString(Console.ReadLine()); // Register user input

            switch (userInput)
            {
                case "1":
                    Console.Clear(); // Clear the console
                    smoothPrinting.PrintLine("--------------------------------------------------");
                    smoothPrinting.PrintLine("FantasyRPG: " + "Storyline Missions");
                    smoothPrinting.PrintLine("--------------------------------------------------");

                    smoothPrinting.RapidPrint("\nMissions are not available yet.");
                    Console.ReadKey(); 
                    Console.Clear(); // Clear the console.
                    dashboard(name, npcsEncountered); // Due to lack of functionality, return user back to the dashboard

                    break;
                case "2":
                    Console.Clear(); // Clear the console
                    smoothPrinting.PrintLine("--------------------------------------------------");
                    smoothPrinting.PrintLine("FantasyRPG: " + "Infinite Dungeon");
                    smoothPrinting.PrintLine("--------------------------------------------------");

                    smoothPrinting.RapidPrint("\nInfinite Dungeon is not available yet.");
                    Console.ReadKey();
                    Console.Clear(); // Clear the console.
                    dashboard(name, npcsEncountered); // Due to lack of functionality, return user back to the dashboard
                    break;
                case "3":
                    Console.Clear(); // Clear the console
                    smoothPrinting.PrintLine("--------------------------------------------------");
                    smoothPrinting.PrintLine("FantasyRPG: " + "Guild Reputation");
                    smoothPrinting.PrintLine("--------------------------------------------------");

                    smoothPrinting.RapidPrint("\nGuild Reputation is not available yet.");
                    Console.ReadKey();
                    Console.Clear(); // Clear the console.
                    dashboard(name, npcsEncountered); // Due to lack of functionality, return user back to the dashboard
                    break;
                case "4":
                    Console.Clear(); // Clear the console
                    smoothPrinting.PrintLine("--------------------------------------------------");
                    smoothPrinting.PrintLine("FantasyRPG: " + "Shop");
                    smoothPrinting.PrintLine("--------------------------------------------------");

                    smoothPrinting.RapidPrint("\nShop is not available yet.");
                    Console.ReadKey();
                    Console.Clear(); // Clear the console.
                    dashboard(name, npcsEncountered); // Due to lack of functionality, return user back to the dashboard
                    break;
                case "5":
                    NPCEncounters(npcsEncountered);
                    break;
                default:
                    while (string.IsNullOrEmpty(userInput))
                    {
                        smoothPrinting.RapidPrint("Invalid input, please try again.");
                        Console.ReadKey(); // Allow user to see error message
                        dashboard(name, npcsEncountered); // Due to lack of functionality, return user back to the dashboard to ensure that they put in the correct input
                    }
                    break;


            }

            void NPCEncounters(List<(string npcName, string npcDescription, string npcAffiliation)> npcsEncountered) // This function will display all the npcs that the user has encountered during their time playing the game
            {
                Console.Clear(); // Clear the console 
                smoothPrinting.PrintLine("--------------------------------------------------");
                smoothPrinting.PrintLine("FantasyRPG: " + "NPC's Encountered");
                smoothPrinting.PrintLine("--------------------------------------------------");
                Console.WriteLine(); // Spacing

                foreach (var npc in npcsEncountered)
                {
                    smoothPrinting.RapidPrint($"\nName: {npc.npcName}\nDescription: {npc.npcDescription}\nAffiliation: {npc.npcAffiliation}\n");
                }

                smoothPrinting.RapidPrint("\nAffirmative? Press any key to continue...");
                Console.ReadKey(); // Read the users input before going back to the dashboard.
                Console.Clear(); // Clear the console for neatness
                dashboard(name, npcsEncountered); // Return user back to the dashboard
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

    public class SmoothConsole // This will be used to ensure output from the console is smooth and aesthetic looking
    {
        private bool skipRequested = false; // Flag to indicate if skip is requested
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

    }

} // Namespace coverage: DO NOT REMOVE THIS

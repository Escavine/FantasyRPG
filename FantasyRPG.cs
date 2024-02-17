using System;
using System.Collections.Generic;
using System.Numerics;
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
        public List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription)> weapon;
        public float numOfPotionsInInventory;
        public float maxPotions;
        public int mana;
        public string[] currentInventory; // Will contain the users potions and other weapons.
        public int arcaniaGoldCoins; // Currency for the city of Arcanith
        public int specialAtkRecharge;// Percentage value, going upto 100%
        // public int atk;
        // public int def;

        // Levelling attributes
        public float exp;
        public int level;
        private int experienceRequiredForNextLevel;
        // public int randomDyingChance;

        public CharacterDefault(string _name, List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription)> _weapon, string[] _currentInventory, int _arcaniaGoldCoins, int specialAtkRecharge) // Default preset for all classes during the start of the game :3
        {
            name = _name;
            weapon = _weapon; // WIll store the details of the given weapon (i.e. weapon name, type, damage, etc.)
            currentInventory = _currentInventory;
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

                exp+=5; // User gets experience from the drop
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

        public Knight(string _name, List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription)> _weapon, string _specialAtkName, string[] _currentInventory, int _arcaniaGoldCoins, int _specialAtkRecharge) : base(_name, _weapon, _currentInventory, _arcaniaGoldCoins, _specialAtkRecharge)
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

        public Mage(string _name, List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription)> _weapon, string[] _magicSpecialties, int _arcaniaGoldCoins, List<(string magicSpell, int damage, int manaRequirement)> _magicSpells, string[] _currentInventory, int _specialAtkRecharge) : base(_name, _weapon, _currentInventory, _arcaniaGoldCoins, _specialAtkRecharge)
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
            string[] magicChoices = { "Fire-Magic", "Water-Magic", "Lightning-Magic", "Ice-Magic", "Dark-Magic", "Light-Magic", "Eucladian-Magic"}; // Future reference: add 'level' as an argument to make other magic specialities exclusive

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

        public SomaliPirate(string _name, List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription)> _weapon, List<(string auraName, int damage, string rarity, string description)> _weaponAura, List<(string attack, int damage, int manaRequirement, string elementType, string description)> _pirateNormalAtks, List<(string attack, int damage, int manaRequirement, string elementType, string description)> _pirateSpecialAtks, string[] _currentInventory, int _arcaniaGoldCoins, int _specialAtkRecharge) : base(_name, _weapon, _currentInventory, _arcaniaGoldCoins, _specialAtkRecharge)
        {
            name = _name;
            weapon = _weapon;
            weaponAura = _weaponAura;
            pirateNormalAtks = _pirateNormalAtks; // Presets for all new Somali Pirates in the game
            pirateSpecialAtks = _pirateSpecialAtks;
            currentInventory = _currentInventory;

        }




        // All methods for the somaliPirate class
        public void PirateNormalAtk(List<(string attack, int damage, int manaRequirement, string elementType, string description)> pirateNormalAtks, int mobHealth)
        {
            Console.WriteLine("The brave Somali Pirate named " + name + " has used " + weapon + " to deal " + normalAtkDmg);
        }

        public void PirateSpecialAtk(List<(string attack, int damage, int manaRequirement, string elementType, string description)> pirateSpecialAtks, int mobHealth)
        {
            Console.WriteLine("The brave Somali Pirate named " + name + " has used " + weapon + " to deal " + specialAtkDmg);
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
        public Archer(string _name, List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription)> _weapon, string[] _currentInventory, int _arcaniaGoldCoins, int _specialAtkRecharge) : base(_name, _weapon, _currentInventory, _arcaniaGoldCoins, _specialAtkRecharge)
        {
            name = _name;
            weapon = _weapon;
        }
    }


    // Warrior class
    class Warrior : CharacterDefault
    {
        public Warrior(string _name, List<(string weaponName, int damage, string rarity, string weaponType, string weaponDescription)> _weapon, string[] _currentInventory, int _arcaniaGoldCoins, int _specialAtkRecharge) : base(_name, _weapon, _currentInventory, _arcaniaGoldCoins, _specialAtkRecharge)
        {
            name = _name;
            weapon = _weapon;
            currentInventory = _currentInventory;
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


        public void gameMenu() // After user information is authenticated, they'll be lead here
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
            Console.WriteLine("\nGame advice: When inputting values, input a corresponding value to the action (e.g. enter the value 1 in order to start the game\n"); // Display game advice
            Random ran = new Random();
            int ran_num = ran.Next(0, 5);
            Console.WriteLine("\nGame Tip: " + gameTips[ran_num] + "\n"); // Display a random game tip in the menu

            Console.WriteLine("\nGame Menu\n");
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
                    Console.Clear();
                    Console.WriteLine("Your game session will now begin!");
                    ClassSelection selectClass = new ClassSelection(); // Create a new game session
                    selectClass.userClass(); // Proceed to let the user pick a character class
                    break;
                case 2:
                    loadingSaveData(); // Lead user to the method
                    break;
                case 3:
                    Console.Clear(); // Neatness structuring
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
            SmoothConsole smoothConsole = new SmoothConsole();
            bool loadingSaveData = true;
            int loadingSaveDataInput = 0;

            while (loadingSaveData == true)
            {
                // Should the user be logged in, they'll be able to access their save data
                Console.WriteLine("This feature isn't avaliable yet, would you like to go back" +
                    "to the menu? (1 for yes and 2 for no)");

                if (loadingSaveDataInput == 1)
                {
                    loadingSaveData = false;
                    smoothConsole.FastPrint("You will be lead back to the menu\n");
                    gameMenu();
                }
                else if (loadingSaveDataInput == 2)
                {
                    loadingSaveData = false;
                    Environment.Exit(0);
                }
                else
                {
                    smoothConsole.FastPrint("Invalid input"); // Keep recursing till conditions have been met
                }


            }

        }

        public void helpSection()
        {
            SmoothConsole smoothPrint = new SmoothConsole();

            int userInput;
            string[] gameAdvice = { "You might die at any point within the game unknowingly.",
                        "Eucladian abilities are quite overpowered, if you find the opportunity to pursue it, then do so.",
                    "Having a strong romantical bond with someone, can potentially increase your abilities.", "There are many classes to choose from, all having unique features.",
                    "Avoid fighting overpowered foes early in-game (i.e. dragons), you'll probably get destroyed." };
            smoothPrint.FastPrint("--------Help Section--------\n");
            Console.WriteLine();
            smoothPrint.FastPrint("What is FantasyRPG?\n");

            // Introduction to Arcania, the world of FantasyRPG
            smoothPrint.RapidPrint("\nWelcome to FantasyRPG, a text-based adventure that transports you to the mystical realm of Arcania!");
            smoothPrint.RapidPrint(" Embark on an epic journey through a vast and enchanting world, where hidden treasures await discovery at every turn.");
            smoothPrint.RapidPrint(" Prepare yourself for the challenges ahead, as you confront life-and-death situations, battle formidable foes, and overcome treacherous obstacles.\n");
            Console.WriteLine();
            smoothPrint.RapidPrint("In Arcania, your choices shape your destiny. Navigate the immersive landscape, forge alliances with fellow travelers, and encounter mythical creatures that will test your courage and resolve.");
            Console.WriteLine();
            smoothPrint.RapidPrint("\nBut beware, adventurer, for danger lurks in the shadows. Face cunning enemies, solve challenging puzzles, and unravel the mysteries that lie dormant in this magical land.");
            Console.WriteLine();
            smoothPrint.RapidPrint("\nAmidst the chaos, there is also the promise of something more. As you progress, open your heart to the possibility of romantic connections, adding depth to your personal story.\n");
            Console.WriteLine();
            smoothPrint.FastPrint("Are you ready to embark on a journey into the heart of Arcania, where every decision shapes your fate? Your adventure begins now!\n");

            // Ask if the user wants to see any game advice in the help section
            Console.WriteLine();
            smoothPrint.FastPrint("Would you like to see any game advice?\n");
            Console.WriteLine("1. Yes");
            Console.WriteLine("2. No");
            userInput = Convert.ToInt32(Console.ReadLine());

            switch (userInput)
            {
                case 1:
                    Console.Clear();
                    smoothPrint.FastPrint("Game Advice:\n");

                    foreach (string s in gameAdvice) // Display game advice
                    {
                        smoothPrint.FastPrint("* " + s + "\n");
                    }

                    Console.ReadKey();
                    Console.Clear();
                    gameMenu(); // Redirect user back to the menu...

                    break;

                case 2:
                    Console.Clear();
                    gameMenu(); // Redirect user back to the menu...
                    break;
                default:
                    Console.WriteLine("Invalid input, please try again.");
                    break;
            }

            Console.ReadKey(); // Wait for key input
        }


        public void makeGameSuggestion() // Game suggestions
        {
            Console.Clear();
            SmoothConsole smooth = new SmoothConsole();
            smooth.FastPrint("Send a message to kmescavine@gmail.com in order to send your ideas!"); // Future reference: Use an SMTP feature to allow the user to input their email and send their suggestion
            Console.ReadKey();
            Console.Clear();
            gameMenu(); // Redirect user back to the menu...
        }

        public void futurePlans() // Future plans for the game development
        {
            Console.Clear();
            SmoothConsole smoothOutput = new SmoothConsole();

            int count = 1;
            string[] futurePlans = { "Adding new classes", "Potential romance feature", "Harem feature (not likely)", "A chance of randomly dying", "Illnesses and cures", "Game difficulty (easy, normal, hard, impossible)" };
            smoothOutput.FastPrint("Future plans for FantasyRPG include:\n");

            foreach (string plan in futurePlans)
            {
                smoothOutput.FastPrint("Plan " + count + ": " + plan + "\n");
                count++;
            }

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

            int userChoice; // Define the user choice

            // Defining the different classes and rarity of items
            string[] fantasyClasses = { "Mage", "Knight", "Somali Pirate", "Shadowwrath", "Archer", "Return to menu" }; // Predefined array of roles
            string[] rarity = { "Common", "Uncommon", "Rare", "Unique", "Legendary" }; // Predefined values :3
            int num = 1;

            smoothPrinting.RapidPrint("Welcome to the world of Arcania!\n");
            Console.WriteLine("\nPick your class");
            Console.WriteLine("-------------------\n"); // Neater

            for (int i = 0; i < fantasyClasses.Length; i++)
            {

                smoothPrinting.FastPrint(num + ". " + fantasyClasses[i] + "\n");
                num++;
            }

            userChoice = Convert.ToInt32(Console.ReadLine());

            switch (userChoice) // Future reference: Rather than have a userchoice fixed to a single method, add multiple methods for different classes (i.e. a mage class if a user chooses the mage role etc, that way you can implement recursion if the user wants to reset their details)
            {
                // Should the user decided to become a Mage
                case 1:
                    int choiceIncrementer = 1; // Used to increment the user choice when picking magic types
                    int startMageJourneyInput;

                    // Arrays containing the variety of different magic choices, spells and weapons.
                    string[] magicChoices = { "Fire-Magic", "Water-Magic", "Ice-Magic", "Lightning-Magic", "Dark-Magic", "Light-Magic", "Eucladian-Magic" };
                    int arcaniaGoldCoins = 0; // You start of as a brokie 

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
                    smoothPrinting.RapidPrint("Mage's Route\n\n");

                    smoothPrinting.RapidPrint("\nAs a fledgling mage, you embark on a journey of discovery and growth, eager to unlock the secrets of the arcane.\n\n");

                    smoothPrinting.RapidPrint("After years of dedicated study and rigorous training, you finally feel a spark of power awakening within you. The arcane energies, once elusive and mysterious, begin to take shape under your guidance.\n\n");

                    smoothPrinting.RapidPrint("Your journey is filled with challenges and trials as you undergo intense mana training. Each spell cast and incantation spoken pushes you closer to mastering the arcane arts. You spend countless hours in solitude, practicing spells, weaving intricate magical patterns, and honing your control over the elemental forces.\n\n");

                    smoothPrinting.RapidPrint("Through perseverance and determination, you overcome obstacles and setbacks, slowly but steadily progressing on your path. With each small victory, you gain confidence in your abilities and deepen your understanding of magic.\n\n");

                    smoothPrinting.RapidPrint("And then, one day, it happens. In a moment of clarity and focus, you feel a surge of power rushing through you. The raw energy of the arcane flows effortlessly from your fingertips, illuminating the world around you with its brilliance.\n\n");

                    smoothPrinting.RapidPrint("With newfound confidence and a hunger for knowledge, you step into a world of endless possibilities. As a novice mage, your journey has only just begun, and the mysteries of magic await your exploration.\n\n");


                    Console.WriteLine("What is your name, adventurer?");
                    string mageName = Convert.ToString(Console.ReadLine());


                    // Display the starter weapons
                    smoothPrinting.RapidPrint("\nDisplaying starter weapons...");
                    Console.WriteLine("\n"); // Neat stucturing

                    foreach (var starterWeapon in starterMageWeapons)
                    {
                        smoothPrinting.RapidPrint($"\n* {starterWeapon.Key}, Damage: {starterWeapon.Value.damage}, Rarity: {starterWeapon.Value.rarity}");
                    }

                    smoothPrinting.RapidPrint("Would you like to pick a weapon?");
                    Console.ReadKey();


                    Console.ForegroundColor = ConsoleColor.Red;
                    smoothPrinting.RapidPrint("Did you seriously think you had a choice as to what you get to pick? You don't."); // User isn't given a choice :3
                    Console.ReadKey();
                    Console.Clear();
                  
                    Console.ForegroundColor = ConsoleColor.White; // Reset the console color output
                    Console.WriteLine("\n"); // Neat structuring
                    smoothPrinting.RapidPrint("Assigning weapon...");
                    Console.WriteLine("\n"); // Neat structuring


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

                    Console.ForegroundColor = ConsoleColor.Magenta;
                    var randomWeapon = mageStaff.First(); // Retrieve the only element added to mageStaff
                    smoothPrinting.FastPrint($"Assigned weapon: {randomWeapon.weaponName}, Damage: {randomWeapon.damage}, Rarity: {randomWeapon.rarity}, Weapon Type: {randomWeapon.weaponType}, \nWeapon Description: {randomWeapon.weaponDescription}"); // Display the assigned weapon to the user

                    Console.WriteLine("Affirmative? Press any key to continue.");
                    Console.ReadKey();
                    Console.Clear();

                    smoothPrinting.RapidPrint("\nChoose a magic specialties from the list: \n");

                    List<string> magicSpecialties = new List<string>(); // Chosen magic specialities
                    List<(string magicSpell, int damage, int manaRequirement)> magicSpells = new List<(string magicSpell, int damage, int manaRequirement)>(); // Chosen magical spells

                    // Display all the magic choices to the user
                    for (int j = 0; j < magicChoices.Length; j++)
                    {
                        smoothPrinting.FastPrint(choiceIncrementer + ". " + magicChoices[j] + "\n");
                        choiceIncrementer++;
                    }



                    // Allow the user to choose a single magic specialty
                    for (int k = 0; k < 1; k++)
                    {
                        int chosenSpecialtyIndex;

                        // Prompt the user to choose a magic specialty
                        smoothPrinting.FastPrint("\nChoose a magic specialty by entering the corresponding number:\n");

                        // Keep prompting until a valid choice is made
                        while (!int.TryParse(Console.ReadLine(), out chosenSpecialtyIndex) || chosenSpecialtyIndex < 1 || chosenSpecialtyIndex > magicChoices.Length)
                        {
                            // Display an error message for invalid input
                            Console.WriteLine("Invalid choice. Please enter a valid number corresponding to the magic specialty.");
                        }

                        // Add the chosen magic specialty to the list
                        magicSpecialties.Add(magicChoices[chosenSpecialtyIndex - 1]);
                    }


                    int totalSpellsDisplayed = 0; // Keep track of the total spells displayed

                    // Will be used to check the magic specialities chosen by the user before displaying the range of spells they can pick


                    for (int z = 0; z < magicSpecialties.Count; z++)
                    {
                        Console.WriteLine("\n" + magicSpecialties[z] + " Spells:");

                        switch (magicSpecialties[z])
                        {
                            case "Fire-Magic":
                                foreach (var spell in fireMagicSpells)
                                {
                                    smoothPrinting.FastPrint($"{(totalSpellsDisplayed + 1)}. {spell.Key} - Damage: {spell.Value.damage}, Mana Requirement for Activation: {spell.Value.manaRequirement}");
                                    totalSpellsDisplayed++;
                                    Console.WriteLine("\nPress Enter to see the next spell...");
                                    Console.ReadLine();
                                }
                                break;

                            case "Water-Magic":
                                foreach (var spell in waterMagicSpells)
                                {
                                    smoothPrinting.FastPrint($"{(totalSpellsDisplayed + 1)}. {spell.Key} - Damage: {spell.Value.damage}, Mana Requirement for Activation: {spell.Value.manaRequirement}");
                                    totalSpellsDisplayed++;
                                    Console.WriteLine("\nPress Enter to see the next spell...");
                                    Console.ReadLine();
                                }
                                break;

                            case "Lightning-Magic":
                                foreach (var spell in lightningMagicSpells)
                                {
                                    smoothPrinting.FastPrint($"{(totalSpellsDisplayed + 1)}. {spell.Key} - Damage: {spell.Value.damage} , Mana Requirement for Activation:  {spell.Value.manaRequirement}");
                                    totalSpellsDisplayed++;
                                    Console.WriteLine("\nPress Enter to see the next spell...");
                                    Console.ReadLine();
                                }
                                break;

                            case "Ice-Magic":
                                foreach (var spell in iceMagicSpells)
                                {
                                    smoothPrinting.FastPrint($"{(totalSpellsDisplayed + 1)}. {spell.Key} - Damage: {spell.Value.damage} , Mana Requirement for Activation:  {spell.Value.manaRequirement}");
                                    totalSpellsDisplayed++;
                                    Console.WriteLine("\nPress Enter to see the next spell...");
                                    Console.ReadLine();
                                }
                                break;

                            case "Dark-Magic":
                                foreach (var spell in darkMagicSpells)
                                {
                                    smoothPrinting.FastPrint($"{(totalSpellsDisplayed + 1)}. {spell.Key} - Damage: {spell.Value.damage} , Mana Requirement for Activation:  {spell.Value.manaRequirement}");
                                    totalSpellsDisplayed++;
                                    Console.WriteLine("\nPress Enter to see the next spell...");
                                    Console.ReadLine();
                                }
                                break;

                            case "Light-Magic":
                                foreach (var spell in lightMagicSpells)
                                {
                                    smoothPrinting.FastPrint($"{(totalSpellsDisplayed + 1)}. {spell.Key} - Damage: {spell.Value.damage} , Mana Requirement for Activation:  {spell.Value.manaRequirement}");
                                    totalSpellsDisplayed++;
                                    Console.WriteLine("\nPress Enter to see the next spell...");
                                    Console.ReadLine();
                                }
                                break;

                            case "Eucladian-Magic":
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
                        Console.WriteLine($"Select 2 magic spells for {magicSpecialties[specialityIndex]} by entering the corresponding numbers. (1-4 for each element)");

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

                    Mage newWizard = new Mage(mageName, mageStaff, magicSpecialties.ToArray(), arcaniaGoldCoins, magicSpells, mageInventory.ToArray());


                    smoothPrinting.FastPrint($"Mage Name: {mageName} \nMage's Weapon Type: {randomWeapon.weaponType} \nMage's Weapon: {randomWeapon.weaponName}");
                    smoothPrinting.FastPrint("\nMage's Magic Specialities: " + string.Join(", ", magicSpecialties));

                    // Display users chosen spells
                    smoothPrinting.FastPrint("\nMage's Chosen Spells: ");

                    foreach (var chosenSpell in magicSpells)
                    {
                        smoothPrinting.RapidPrint($"\n{chosenSpell.magicSpell}: Damage - {chosenSpell.damage}, Mana Requirement - {chosenSpell.manaRequirement}");
                    };

                    Console.WriteLine("\n"); // Neat structuring
                    Console.WriteLine("\nWould you like to now embark on your journey in the world of Arcania? (Enter 1 for Yes)");
                    startMageJourneyInput = Convert.ToInt32(Console.ReadLine()); // Register the user input


                    switch (startMageJourneyInput)
                    {
                        case 1:
                            Console.Clear(); // Neatness
                            smoothPrinting.FastPrint("First scenario\n");
                            Console.WriteLine("You will now be sent to the world of Arcania, make sure to not die.");
                            FirstScenario wizardJourney = new FirstScenario(); // Journey start!
                            wizardJourney.usersFirstJourney(mageName);
                            break;

                        case 2:
                            Console.BackgroundColor = ConsoleColor.Red;
                            smoothPrinting.RapidPrint("Don't try play tricks, this game isn't easy");
                            smoothPrinting.RapidPrint("\nYou died :3");
                            break;
                        default:
                            Console.WriteLine("Invalid input, ensure that you enter the correct value (in this case, the value '1').");
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
                    string pirateName;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    int specialAtkRecharge = 0; // This remains fixed

                    // Story output (this will be further expanded)
                    smoothPrinting.FastPrint("You are a proud Somali Pirate, one who has explored the vast open seas for many years, and now you feel that your ready for a new adventure!\n");

                    // Take users name
                    Console.WriteLine("Enter your name:");
                    pirateName = Convert.ToString(Console.ReadLine());

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


                    smoothPrinting.RapidPrint("\nNormal Attack Selection");

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
                    Console.WriteLine("Confirmed normal attack selected by user:");
                    foreach (var attack in chosenPirateNormalAttacks)
                    {
                        smoothPrinting.RapidPrint($"* {attack.attack}, Damage: {attack.damage}, Mana Requirement: {attack.manaRequirement}, Element Type: {attack.elementType}\nDescription: {attack.description}");
                        Console.WriteLine("\n"); // Neat structure for displaying selected normal attacks
                    }

                    Console.WriteLine("Affirmative? Press any key to continue.");
                    Console.ReadKey(); // Allow the user to check before proceeding into selecting special attack choices

                    Console.Clear(); // Neatness

                    Console.WriteLine("Special Attack Selection");
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
                    Console.WriteLine("Confirmed special attack selected by user:");
                    foreach (var attack in chosenSpecialAttacks)
                    {
                        smoothPrinting.RapidPrint($"* {attack.attack}, Damage: {attack.damage}, Mana Requirement: {attack.manaRequirement}, Element Type: {attack.elementType}\nDescription: {attack.description}");
                        Console.WriteLine("\n"); // Neat structure for displaying selected normal attacks
                    }


                    Console.WriteLine("Affirmative? Press any key to continue.");
                    Console.ReadKey(); // Allow the user to check before proceeding into selecting special attack choices

                    Console.Clear(); // Neatness


                    smoothPrinting.FastPrint("Displaying starter weapons...");

                    foreach (var weapon in pirateWeaponChoices) // Display starter weapons
                    {
                        smoothPrinting.RapidPrint($"\n{weapon.Key} - Damage: {weapon.Value.damage} - Weapon Type: {weapon.Value.weaponType}, Item Rarity: {weapon.Value.rarity}\nWeapon Description: {weapon.Value.weaponDescription}\n");
                    }

                    smoothPrinting.FastPrint("\nIf everything is clear, press any key to continue to the auras.");
                    Console.ReadKey();
                    Console.Clear(); // Clear the console to display the auras, since displaying both weapons and auras at once takes too much space

                    smoothPrinting.FastPrint("Displaying auras..."); // Display weapon auras

                    foreach (var weaponAura in pirateWeaponAuras)  
                    {
                        smoothPrinting.RapidPrint($"\n{weaponAura.Key} - Damage: {weaponAura.Value.damage}, Rarity: {weaponAura.Value.rarity}\nAura Description: {weaponAura.Value.auraDescription}\n");
                    }

                    smoothPrinting.FastPrint("\nWeapon will be randomly assigned...");
                    smoothPrinting.FastPrint("\nAura will be randomly assigned...");

                    Console.WriteLine("\n"); // Structuring
                    smoothPrinting.FastPrint("\nWould you like to continue?");
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

                    var randomAuraIndex = pirateWeaponAuras.ElementAt(pirateAuraRoll); // Assign a random index to the aura

                    pirateWeaponAura.Add(randomAuraIndex.Key, randomAuraIndex.Value.damage, randomAuraIndex.Value.rarity, randomAuraIndex.Value.auraDescription); // Add the entire key-value pair to the list


                    SomaliPirate newPirate = new SomaliPirate(pirateName, pirateWeapon, pirateWeaponAura, chosenPirateNormalAttacks, chosenPirateSpecialAttacks, pirateInventory.ToArray(), arcaniaGoldCoins, specialAtkRecharge); // Generate the pirate details

                    Console.Clear(); // Neater


                    // Display information to the user
                    smoothPrinting.FastPrint($"Pirate's Name: {pirateName} \nPirate's Weapon Type: {randomPirateWeapon.Value.weaponType}, \nPirate's Weapon: {randomPirateWeapon.Key}, Damage: {randomPirateWeapon.Value.damage} \nPirate's Aura: {pirateAuraName}");

                    smoothPrinting.FastPrint("\n\nPirate's Normal Attacks: ");

                    foreach (var chosenNormalAttack in chosenPirateNormalAttacks) // Display all chosen normal attacks moves of the user
                    {
                        smoothPrinting.RapidPrint($"\n* {chosenNormalAttack.attack}: Damage - {chosenNormalAttack.damage}, Mana Requirement - {chosenNormalAttack.manaRequirement}, Element Type - {chosenNormalAttack.elementType} \nDescription: {chosenNormalAttack.description}");
                    };

                    smoothPrinting.FastPrint("\n\nPirate's Special Attacks: ");

                    foreach (var chosenSpecialAttack in chosenSpecialAttacks) // Display all chosen special attacks moves of the user
                    {
                        smoothPrinting.RapidPrint($"\n* {chosenSpecialAttack.attack}: Damage - {chosenSpecialAttack.damage}, Mana Requirement - {chosenSpecialAttack.manaRequirement}, Element Type - {chosenSpecialAttack.elementType} \nDescription: {chosenSpecialAttack.description}");
                    };

                    Console.WriteLine("\nWould you like to now embark on your journey in the world of Arcania? (1 for Yes)");
                    startPirateJourneyInput = Convert.ToInt32(Console.ReadLine()); // Register the user input

                    // Future reference: For each class chosen, make a seperate method for them
                    switch (startPirateJourneyInput)
                    {
                        case 1:
                            Console.Clear(); // Neatness
                            Console.WriteLine("You will now be sent to the world of Arcania, make sure to not die.");
                            FirstScenario pirateJourney = new FirstScenario(); // Journey start!
                            pirateJourney.usersFirstJourney(pirateName);
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
                    Console.WriteLine("Please pick a sensible choice and understand if you do that again you'll be punished hahaha");

                    break;
            }

        }

    }



    public class FirstScenario // Once the user selects a class, they'll proceed onto their journey
    {
        int fightChoice;
        string[] customaryScenarios = { "You embark on a long journey, you find yourself lost midway throughout the journey. There appears a dragon, with fangs as sharp as blades and a gaze so intense that you begin to question your fighting prowess despite your training. What do you do?" };

        // Non-static scenarios will be introduced later in the game if I can be asked
        string fixedScenario = "\nYou embark on a long journey, you find yourself lost midway, your eyes are surrounded by vast levels of fog, mitigating your view of the perspective ahead. Closeby, there appears a dragon, with fangs as sharp as blades and a gaze so intense that you begin to question your fighting prowess despite your training.";

        public void usersFirstJourney(string name)
        {
            SmoothConsole smoothPrinting = new SmoothConsole();
            smoothPrinting.RapidPrint(fixedScenario + "\n");
            Console.WriteLine("\nWhat do you do?");

            smoothPrinting.FastPrint("\n1. Fight back");
            smoothPrinting.FastPrint("\n2. Escape\n");
            fightChoice = Convert.ToInt32(Console.ReadLine());

            switch (fightChoice)
            {
                case 1:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Your level is too low, the dragon proceeds to consume you whole in your defenseless state.");

                    Console.WriteLine("You died.");
                    break;

                case 2:
                    Console.ForegroundColor = ConsoleColor.Green;
                    smoothPrinting.FastPrint("Wise choice, you successfully escape with all your limbs intact.");
                    smoothPrinting.FastPrint("TESTING MEASURE: YOU WILL NOW BE LEAD TO THE USER DASHBOARD");
                    gameDashboard userDashboard = new gameDashboard();
                    userDashboard.dashboard(name);
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Don't know why you did that, but you died LOL");
                    break;

            }


        }




    }



    public class gameDashboard
    {
        public void dashboard(string name) // Will display the user dashboard for the game
        {
            SmoothConsole smoothPrinting = new SmoothConsole();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Gray;
            smoothPrinting.RapidPrint(name + "'s " + "dashboard\n");
            smoothPrinting.RapidPrint("1. Main storyline (NOT WORKING YET)\n");
            smoothPrinting.RapidPrint("2. Infinite dungeon (NOT WORKING YET)\n");
            smoothPrinting.RapidPrint("3. Guild reputation(NOT WORKING YET)\n");
            smoothPrinting.RapidPrint("4. Shop (NOT WORKING YET)\n");
            Console.ReadKey(); // No functionality for now

        }
    }

    public class SmoothConsole // This will be used to ensure output from the console is smooth and aesthetic looking
    {

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